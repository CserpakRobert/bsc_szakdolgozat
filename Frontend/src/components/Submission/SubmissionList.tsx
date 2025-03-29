import { Grid2 } from "@mui/material";
import { SubmissionSmallInterface } from "./SubmissionInterface";
import SmallCompetition from "./SubmissionSmallDetails";

export default function SubmissionList(submissions:[SubmissionSmallInterface]){
    

    return(<>
    
        <Grid2 container>
            {submissions.map(sub=>
                <>
                    <Grid2 size={{xs:12, md:6, lg:3}}>
                        <SmallCompetition submission={sub}></SmallCompetition>
                    </Grid2>
                </>
            )}
        </Grid2>
    </>)
}