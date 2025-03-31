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

drop event UpdateCompetition;


-- TODO Convert groups into submissions
SHOW EVENTS;
SHOW VARIABLES LIKE 'event_scheduler';