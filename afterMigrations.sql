SET GLOBAL event_scheduler = ON;
DELIMITER //

CREATE OR REPLACE PROCEDURE RankSubmissions(Id INT)
BEGIN

    DECLARE done INT DEFAULT FALSE;

    DECLARE submissionId, voteCount, lastVoteCount, placement, lastVotes INT;
    DECLARE cur CURSOR FOR 
                SELECT s.Id submissionId, s.voteCount
                FROM submissions s
                WHERE s.competitionId = Id
                ORDER BY s.voteCount DESC;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
        BEGIN
                ROLLBACK;
                RESIGNAL;
        END;

    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    SET placement = 0;
    SET lastVoteCount = -1;
    SET AUTOCOMMIT = 0;
    START TRANSACTION;
        
    OPEN cur;
    read_loop: LOOP
        FETCH cur INTO submissionId,voteCount;

        IF done THEN
            LEAVE read_loop;
        END IF;

        IF voteCount = -1 THEN
            SET lastVoteCount = voteCount;
        END IF;

        IF voteCount <> lastVoteCount THEN
            SET placement = placement + 1;
        END IF;

        UPDATE submissions s
            SET s.placement = placement
            WHERE s.Id = submissionId;
        SET lastVoteCount = voteCount;

    END LOOP read_loop;

    COMMIT;
    SET AUTOCOMMIT = 1;
END;
//
CREATE OR REPLACE PROCEDURE VotingEnd()
BEGIN

    DECLARE done INT DEFAULT FALSE;

    DECLARE Id INT;

    DECLARE cur CURSOR FOR 
        SELECT c.Id Id 
        FROM competitions c
        WHERE c.IsInVotingPhase = 1 AND c.FinishDate < CURDATE();
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
        BEGIN
                ROLLBACK;
                RESIGNAL;
        END;

    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    SET AUTOCOMMIT = 0;
    START TRANSACTION;

    OPEN cur;
    read_loop: LOOP
        FETCH cur INTO Id;

        IF done THEN
            LEAVE read_loop;
        END IF;

        call RankSubmissions(Id);
       
    END LOOP;

    CLOSE cur;

    UPDATE Competitions
        SET isinvotingphase = 0
        WHERE IsInVotingPhase = 1 AND FinishDate < CURDATE();

    COMMIT;
    SET AUTOCOMMIT = 1;
END;
//

CREATE OR REPLACE PROCEDURE SubmissionEnd()
BEGIN

    DECLARE done INT DEFAULT FALSE;
    DECLARE competitionId INT;

    DECLARE cur CURSOR FOR 
    SELECT Id competitionId
    FROM competitions
    WHERE IsInSubmissionPhase = 1 AND VotingStartDate < CURDATE();

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
            ROLLBACK;
            RESIGNAL;
    END;

    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    set AUTOCOMMIT = 0;
    START TRANSACTION;

    OPEN cur;
    read_loop: LOOP
        FETCH cur INTO competitionId;

        IF done THEN
            LEAVE read_loop;
        END IF;

        DELETE FROM submissions s
        where s.competitionId = competitionId and s.IsSubmitted = 0;

        UPDATE competitions c
        SET c.IsInSubmissionPhase = 0, c.IsInVotingPhase = 1
        WHERE c.Id = competitionId;
    END LOOP;
    CLOSE cur;

    COMMIT;
    set AUTOCOMMIT = 1;
END;
//

CREATE or replace PROCEDURE PreparationEnd()
BEGIN

    DECLARE done INT DEFAULT FALSE;

    DECLARE groupId,
     ownerId,
     competitionId,
     isMovie,
     isMusic,
     isStory ,
     lastSubmissionId,
     movieDetailsId,
     musicDetailsId,
     storyDetailsId
     INT;

    DECLARE cur CURSOR FOR 
        SELECT g.Id groupId, g.GroupOwnerId ownerId,
         g.CompetitionId competitionId,
         c.IsMovieCompetition isMovie,
         c.IsMusicCompetition isMusic,
         c.isShortStoryCompetition isStory
        FROM Groups g
        JOIN competitions c ON g.competitionId = c.Id
        WHERE c.IsInPreparationPhase = 1 
            AND SubmissionStartDate < CURDATE();

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
            ROLLBACK;
            RESIGNAL;
    END;

    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    set AUTOCOMMIT = 0;
    START TRANSACTION;

    OPEN cur;
    read_loop: LOOP
        FETCH cur INTO groupId,
        ownerId,
        competitionId,
        isMovie,
        isMusic,
        isStory;
        
        IF done THEN
            LEAVE read_loop;
        END IF;

    -- Create submission
        -- Create blank submission
        IF isMovie THEN
            INSERT INTO Submissions (OwnerId, CompetitionId, description, title, IsSubmitted)
                VALUES (ownerId, competitionId, "", "", FALSE);
            SET lastSubmissionId = LAST_INSERT_ID();

            INSERT INTO moviedetails (TrailerLink, MovieLink, SubmissionId)
                VALUES ("", "", lastSubmissionId);

            SET movieDetailsId = LAST_INSERT_ID();

            UPDATE Submissions
            set MovieDetailsId = movieDetailsId
            where Id = lastSubmissionId;
        END IF;

        IF isMusic THEN
            INSERT INTO Submissions (OwnerId, CompetitionId, description, title, IsSubmitted)
                VALUES (ownerId, competitionId, "", "", FALSE);
            SET lastSubmissionId = LAST_INSERT_ID();

            INSERT INTO musicdetails (Link, SubmissionId)
                VALUES ("", lastSubmissionId);

            SET musicDetailsId = LAST_INSERT_ID();

            UPDATE Submissions
            set MusicDetailsId = musicDetailsId
            where Id = lastSubmissionId;
        END IF;

        IF isStory THEN
            INSERT INTO Submissions (OwnerId, CompetitionId, description, title, IsSubmitted)
                VALUES (ownerId, competitionId, "", "", FALSE);
            SET lastSubmissionId = LAST_INSERT_ID();

            INSERT INTO shortstorydetails (Story, SubmissionId)
                VALUES ("", lastSubmissionId);

            SET storyDetailsId = LAST_INSERT_ID();

            UPDATE Submissions
            set ShortStoryDetailsId = storyDetailsId
            where Id = lastSubmissionId;
        END IF;
    -- Create submission END
        INSERT INTO debug_log (msg) VALUES (CONCAT('Loop iteration: ', groupId));
        INSERT INTO debug_log (msg) VALUES (CONCAT('lastsub: ', lastSubmissionId));

    -- Insert into collaboratedSubmissions
        -- Insert participants into collaborators


        INSERT INTO CollaboratedSubmissions (UserId, SubmissionId)
            SELECT upg.UserId, lastSubmissionId 
            FROM userparticipategroups upg
            WHERE upg.GroupId = groupId;
    -- Insert into collaboratedSubmissions END

    -- Delete group
        -- delete group participants
        -- DELETE FROM userparticipategroups
        --     WHERE GroupId = groupId;

        -- DELETE FROM joinrequests
        --     WHERE GroupId = groupId;

        -- DELETE FROM groupinvites
        --     WHERE GroupId = groupId;
        -- Delete group
        DELETE FROM Groups
            WHERE Id = groupId;
    -- Delete group END
        INSERT INTO debug_log (msg)
        SELECT CONCAT('Collaborator: UserId=', upg.UserId, ', SubmissionId=', upg.groupId)
        FROM userparticipategroups upg;

    END LOOP;
    CLOSE cur;

    -- Update Competition
        UPDATE Competitions
            SET IsInPreparationPhase = 0, IsInSubmissionPhase = 1
            WHERE IsInPreparationPhase = 1 AND SubmissionStartDate < CURDATE();
    -- Update Competition END

    COMMIT;
    set AUTOCOMMIT = 1;

END;
//

CREATE OR REPLACE EVENT Updatecompetitions 
ON SCHEDULE EVERY '1' DAY
STARTS (TIMESTAMP(CURRENT_DATE) + INTERVAL 1 DAY + INTERVAL 1 HOUR)
DO
BEGIN
    call PreparationEnd();

    call SubmissionEnd();

    call VotingEnd();
END;
//

DELIMITER ;
show events;


use versenydb;
delete from competitions;
delete from submissions;
delete from groups;
delete from userparticipategroups;
delete from collaboratedSubmissions;
delete from debug_log;

insert into competitions 
(title, description, creatorid, ismoviecompetition, ismusiccompetition,
 isshortstorycompetition, isinpreparationphase, isinsubmissionphase, isinvotingphase,
  submissionstartdate, votingstartdate, finishdate)
values
("Minta film verseny", "",4, 
    1,0,0, 
    1,0,0, 
 (CURDATE() + interval 1 day),
 (CURDATE() + interval 8 day),
 (CURDATE() + interval 15 day)
);
set @lastComp = LAST_INSERT_ID();

insert into groups (CompetitionId, GroupOwnerId, description, IsClosed) values
(@lastComp, 5, "", 0),
(@lastComp, 15, "", 0),
(@lastComp, 25, "", 0);

set @lastGroup = LAST_INSERT_ID();
select @lastGroup;
insert into userparticipategroups (GroupId, UserId) values
(@lastGroup,5),
(@lastGroup,6),
(@lastGroup,7),
(@lastGroup,8),
(@lastGroup+1,15),
(@lastGroup+1,16),
(@lastGroup+1,17),
(@lastGroup+1,18),
(@lastGroup+2,25),
(@lastGroup+2,26),
(@lastGroup+2,27),
(@lastGroup+2,28);


-- drop event Updatecompetitions ;

-- select * from competition;
update competitions 
    set SubmissionStartDate = "0001-01-01"
    where Id = 27;
call PreparationEnd();
update competitions 
    set VotingStartDate = "0001-01-01"
    where Id = 7;
call SubmissionEnd();
update competitions 
    set FinishDate = "0001-01-01"
    where Id = 7;
call VotingEnd();
-- delete from competitions where id = 3;
select * from debug_log;

SELECT 
    TABLE_NAME, 
    CONSTRAINT_NAME, 
    REFERENCED_TABLE_NAME, 
    DELETE_RULE
FROM 
    INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS
WHERE 
    CONSTRAINT_SCHEMA = 'versenydb';