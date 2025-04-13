import { Button, Card, Grid2 } from "@mui/material";
import { CompetitionSmallInterface } from "./CompetitionInterface";
import { useNavigate } from "react-router-dom";
import CompetitionType from "./CompetitionType";
import CompetitionPhase from "./CompetitionPhase";
import CompetitionDate from "./CompetitionDate";
import { FormEvent } from "react";
import { Height } from "@mui/icons-material";


export default function CompetitionDetailsCard({ competition }: { competition: CompetitionSmallInterface }) {
    const navigate = useNavigate()
    const handleOnclick = (e: FormEvent) => {
        e.preventDefault();
        navigate(`/competition/details/${competition.id}`)
    }
    return (<>
        <Card variant="outlined" sx={{minHeight:250, width:"100%"}}>
            <Grid2 container padding={1} spacing={1} direction="row" justifyContent="space-between">
                <Grid2 container size={12} gap={0}>
                    <Grid2 size={5.5} >
                        <CompetitionType type={competition.type} />

                    </Grid2>
                    <Grid2 size={6.5}>
                        <CompetitionPhase phase={competition.phase}  />
                    </Grid2>
                </Grid2>
                {!competition.relevantDate ? (<></>) : (<>
                    <Grid2 size={12} height={20}>
                        <CompetitionDate phase={competition.phase} date={competition.relevantDate} />
                    </Grid2>
                </>)}
                <Grid2 size={12}>
                    <h3 style={{ wordWrap: "break-word" }}>{competition.title}</h3>
                </Grid2>
                {competition.relevantDate ? (<></>) : (<>
                    <Grid2 size={12} height={20} />
                </>)}
                <Grid2 size={12}>
                    <Button variant="outlined" onClick={handleOnclick}>Verseny megtekintése!</Button>
                </Grid2>
            </Grid2>

        </Card>

    </>)
}