import { Button, Card } from "@mui/material";
import { SubmissionSmallInterface } from "./SubmissionInterface";
import { useNavigate } from "react-router-dom";

export default function SmallCompetition({ submission }: { submission: SubmissionSmallInterface }) {
    const navigate = useNavigate()
    return (<>
        <Card variant="detailsCard" sx={{flexDirection:"column", alignItems:"flex-start", justifyContent:"start"}}>
            <h5>{submission.type}</h5>
            <h5>{submission.title}</h5>
            <Button variant="outlined" onClick={(e)=>{e.preventDefault(); navigate(`/submission/${submission.id}`)}}>See details!</Button>
        </Card>
    </>)
}