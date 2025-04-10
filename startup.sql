use szakdol;
SET GLOBAL event_scheduler = ON;

CREATE EVENT UpdateCompetition
On SCHEDULE EVERY 45 MINUTE
DO
BEGIN

    -- set submission phases
    UPDATE Competition
    SET IsInPreparationPhase = 0, IsInSubmissionPhase = 1
    WHERE IsInPreparationPhase = 1 AND SubmissionStartDate < CURDATE();

    -- set voting phases

    UPDATE Competition
    SET IsInSubmissionPhase = 0, IsInVotingPhase = 1
    WHERE IsInSubmissionPhase = 1 AND VotingStartDate < CURDATE();

    -- set finished phase
    UPDATE Competition
    SET isinvotingphase = 0
    WHERE IsInVotingPhase = 1 AND FinishDate < CURDATE();

END;

DROP EVENT UpdateCompetition;


select LAST_INSERT_ID();

SHOW PROCEDURE STATUS 
where Db = szakdol;
-- TODO Convert groups into submissions
SHOW EVENTS;
SHOW VARIABLES LIKE 'event_scheduler';

SET SQL_MODE='ORACLE';
SELECT @@SQL_MODE;



select * from competition;
select * from groups where competitionID = 32;
select * from userparticipategroups where groupId = 13;
Update competition
set SubmissionStartDate="0001-01-01", IsInPreparationPhase = 1, IsInSubmissionPhase = 0, isinvotingphase = 0
where Id = 32;

select * from userparticipategroups where groupId = 13;

select * from submissions;
select * from collaboratedSubmissions;
delete from submissions;
delete from collaboratedSubmissions;
call PreparationEnd();

SHOW INDEXES FROM collaboratedSubmissions;
--todo details
CREATE or replace PROCEDURE PreparationEnd()
BEGIN

    DECLARE done INT DEFAULT FALSE;

    DECLARE groupId, ownerId, competitionId, lastSubmissionId INT;

    DECLARE cur CURSOR FOR 
        SELECT g.Id groupId, g.GroupOwnerId ownerId, g.CompetitionId competitionId
        FROM Groups g
        JOIN competition c ON g.competitionId = c.Id
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
        FETCH cur INTO groupId, ownerId, competitionId;
        
        IF done THEN
            LEAVE read_loop;
        END IF;

    -- Create submission
        -- Create blank submission
        INSERT INTO Submissions (OwnerId, CompetitionId, description, title, IsSubmitted)
            VALUES (ownerId, competitionId, "", "", FALSE);

        -- Get submission's Id
        SET @lastSubmissionId = LAST_INSERT_ID();
    -- Create submission END

    -- Insert into collaboratedSubmissions
        -- Insert participants into collaborators
        INSERT INTO CollaboratedSubmissions (UserId, SubmissionId)
            SELECT UserId, @lastSubmissionId 
            FROM userparticipategroups upg
            WHERE upg.GroupId = groupId  ;
            -- AND NOT EXISTS (
            --     SELECT 1
            --     FROM CollaboratedSubmissions cs
            --     WHERE cs.UserId = upg.UserId AND cs.SubmissionId = @lastSubmissionId 
            -- );

    -- Insert into collaboratedSubmissions END

    -- Delete group
        -- delete group participants
        DELETE FROM userparticipategroups
            WHERE GroupId = groupId;

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

DELIMITER //