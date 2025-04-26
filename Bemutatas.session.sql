insert into submissions (OwnerId, Title, CompetitionId, Placement, Description)
values (14, "Bakonyi békák", 18, 1, ""),
 (14, "Túl a tengeren", 15, 3, ""),
 (14, "Misztikus hangok", 48, 5, ""),
 (14, "Nyugodt hangok", 48, 13, ""),
 (14, "Nyugodt hangok", 48, 13, "");

insert into collaboratedSubmissions (UserId, SubmissionId)
values (14,89),
 (14,90),
 (14,91),
 (14,92),
 (14,93);

delete from submissions;

select * from competition 
where IsShortStoryCompetition = TRUE;