use szakdol;
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
        FROM competitionsc
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

    UPDATE Competition
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
    FROM competition
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

        UPDATE competitionsc
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
     lastSubmissionId,
     isMovie,
     isMusic,
     isStory 
     INT;

    DECLARE cur CURSOR FOR 
        SELECT g.Id groupId, g.GroupOwnerId ownerId,
         g.CompetitionId competitionId,
         c.IsMovieCompetition isMovie,
         c.IsMusicCompetition isMusic,
         c.isShortStoryCompetition isStory
        FROM Groups g
        JOIN competitionsc ON g.competitionId = c.Id
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
            SET @lastSubmissionId = LAST_INSERT_ID();

            INSERT INTO moviedetails (TrailerLink, MovieLink, SubmissionId)
                VALUES ("", "", @lastSubmissionId);

            SET @movieDetailsId = LAST_INSERT_ID();

            UPDATE Submissions
            set MovieDetailsId = @movieDetailsId
            where Id = @lastSubmissionId;

        END IF;

        IF isMusic THEN
            INSERT INTO Submissions (OwnerId, CompetitionId, description, title, IsSubmitted)
                VALUES (ownerId, competitionId, "", "", FALSE);
            SET @lastSubmissionId = LAST_INSERT_ID();

            INSERT INTO musicdetails (Link, SubmissionId)
                VALUES ("", @lastSubmissionId);

            SET @musicDetailsId = LAST_INSERT_ID();
            -- SET @lastSubmissionId = 59;

            UPDATE Submissions
            set MusicDetailsId = @musicDetailsId
            where Id = @lastSubmissionId;
        END IF;

        IF isStory THEN
            INSERT INTO Submissions (OwnerId, CompetitionId, description, title, IsSubmitted)
                VALUES (ownerId, competitionId, "", "", FALSE);
            SET @lastSubmissionId = LAST_INSERT_ID();

            INSERT INTO shortstorydetails (Story, SubmissionId)
                VALUES ("", @lastSubmissionId);

            SET @storyDetailsId = LAST_INSERT_ID();

            UPDATE Submissions
            set ShortStoryDetailsId = @storyDetailsId
            where Id = @lastSubmissionId;
        END IF;


        -- Get submission's Id
    -- Create submission END

    -- Insert into collaboratedSubmissions
        -- Insert participants into collaborators
        INSERT INTO CollaboratedSubmissions (UserId, SubmissionId)
            SELECT upg.UserId, @lastSubmissionId 
            FROM userparticipategroups upg
            WHERE upg.GroupId = groupId;
    -- Insert into collaboratedSubmissions END

    -- Delete group
        -- delete group participants
        DELETE FROM userparticipategroups
            WHERE GroupId = groupId ;

        DELETE FROM joinrequests
            WHERE GroupId = groupId ;

        DELETE FROM groupinvites
            WHERE GroupId = groupId ;
        -- Delete group
        DELETE FROM Groups
            WHERE Id = groupId;
    -- Delete group END

    END LOOP;
    CLOSE cur;

    -- Update Competition
        UPDATE Competition
            SET IsInPreparationPhase = 0, IsInSubmissionPhase = 1
            WHERE IsInPreparationPhase = 1 AND SubmissionStartDate < CURDATE();
    -- Update Competition END

    COMMIT;
    set AUTOCOMMIT = 1;

END;
//

CREATE OR REPLACE EVENT UpdateCompetitions
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


-- drop event UpdateCompetitions;

-- -- select * from competition;
-- update competitions
--     set SubmissionStartDate = "0001-01-01"
--     where Id = 5;
-- call PreparationEnd();
-- update competitions
--     set VotingStartDate = "0001-01-01"
--     where Id = 2;
-- call SubmissionEnd();
-- update competitions
--     set FinishDate = "0001-01-01"
--     where Id = 4;
-- call VotingEnd();
-- delete from competitionswhere id = 3;