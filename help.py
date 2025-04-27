with open('output.txt', 'w') as file:
    file.write(f"insert into competitions"
            f"(title, description, creatorid, ismoviecompetition, ismusiccompetition,"
            f"isshortstorycompetition, isinpreparationphase, isinsubmissionphase, isinvotingphase,"
            f"submissionstartdate, votingstartdate, finishdate)"
            f"values"+'\n')

    for i in range(1, 101):
        values = (
            f"('TesztElek{i}', 'TESZTELEK{i}', 'TESZTELEK{i}@GMAIL.COM', 'TESZTELEK{i}@GMAIL.COM', "
            f"'AQAAAAIAAYagAAAAEGKB/lEFXZvlE0kNqIR22zRgD8wry9nSKQ0GwDyzO78KqCS4zxYpx/yEfPw0z5FFZg==', "
            f"'2X6X726DA7JORXG3VSEOGHEFUTOCVUNV', 0, 0, 0, 0, 0)"
        )
        file.write(values + '\n')

    file.write(f"insert into competitions" 
                f"(title, description, creatorid, ismoviecompetition, ismusiccompetition,"
                f"isshortstorycompetition, isinpreparationphase, isinsubmissionphase, isinvotingphase,"
                f"submissionstartdate, votingstartdate, finishdate)"
                f"values")
    i = 0
    while i < 300:
        values = ()
        file.write(values + '\n')


    