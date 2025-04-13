import { Button, Card, Chip, Grid2 } from "@mui/material";
import { SubmissionListDetailsInterface } from "./SubmissionInterface";
import { useNavigate } from "react-router-dom";
import CompetitionType from "../Competition/CompetitionType";
import Placement from "./Placement";

export default function SubmissionSmallDetails({ submission }: { submission: SubmissionListDetailsInterface }) {
    const navigate = useNavigate()
    return (<>
        <Card variant="outlined" sx={{ padding: 2 }}>
            <Grid2 container spacing={2} >
                <Grid2 size={{ xs: 12, md: 12 }}>
                    <h3>{submission.title}</h3>
                </Grid2>
                <Grid2 size={{ xs: 12, md: 6 }}>
                    <CompetitionType type={submission.type} />
                </Grid2>
                <Grid2 size={{ xs: 12, md: 6 }}>
                    {submission.phase == "voting"&& <Chip color="secondary" label={submission.voteCount} />}
                    {submission.phase == "finished" && <Placement placement={submission.placement} />}
                </Grid2>
                <Grid2 size={{ xs: 12, md: 12 }}>
                    <Button variant="outlined" onClick={(e) => { e.preventDefault(); navigate(`/submission/${submission.id}`) }}>Pályázat megtekintése</Button>

                </Grid2>
            </Grid2>

        </Card>
    </>)
}