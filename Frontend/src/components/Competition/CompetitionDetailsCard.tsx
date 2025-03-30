import { Button, Card, Chip, Grid2 } from "@mui/material";
import { CompetitionSmallInterface } from "./CompetitionInterface";
import { useNavigate } from "react-router-dom";
import CompetitionType from "./CompetitionType";
import CompetitionPhase from "./CompetitionPhase";


export default function CompetitionDetailsCard({ competition }: { competition: CompetitionSmallInterface }) {
    const navigate = useNavigate()
    // navigate(`/submissions/details/${competition.id}`)
    return (<>
        <Card variant="outlined">
            <Grid2 container padding={2} spacing={0}>
                <Grid2 size={6}>
                    <CompetitionType type={competition.type} />

                </Grid2>
                <Grid2 size={6} >
                    <CompetitionPhase phase={competition.phase}/>
                </Grid2>
               
                <Grid2 size={12}>
                {competition.relevantDate?(<><Chip variant="outlined" color="warning" label={competition.relevantDate}></Chip></>):(<></>)}
                </Grid2>
                <Grid2 size={12}>
                    <h3>{competition.title}</h3>
                </Grid2>
                <Grid2 size={12}>
                    <Button variant="outlined">See submissions</Button>
                </Grid2>
            </Grid2>

        </Card>

    </>)
}