import { useEffect, useState } from "react"
import { useNavigate, useParams } from "react-router-dom"
import { api } from "../../globalHelpers/apiAxios"
import { Paper, Grid2, Card, Button, Box, CircularProgress } from "@mui/material"
import CompetitionInterface from "./CompetitionInterface"
import UserButton from "../User/UserButton"
import CompetitionDate from "./CompetitionDate"
import CompetitionPhase from "./CompetitionPhase"
import CompetitionType from "./CompetitionType"

export default function CompetitionDetails() {
    const { id } = useParams()
    const navigate = useNavigate()

    const [competition, setCompetition] = useState<undefined | CompetitionInterface>(undefined)

    useEffect(() => {
        api.get(`competition/${id}`)
            .then(
                resp => setCompetition(resp.data)
            )
            .catch(
        )
    }, [id]
    )
    // TODO navigations
    const handleJoin = () => {

    }

    const handleFindGroups = () => {

    }
    const handleListSubmissions = () => {

    }

    return (
        <>

            <Paper variant="detailsPaper" >
                <Box>
                    {!competition ? (<>

                        <CircularProgress />
                    </>) : (<>
                        <Grid2 container spacing={2} sx={{}} size={8}>
                            { }
                            <Grid2 size={{ sm: 12, md: 12 }}>
                                <h1>{competition.title}</h1>
                            </Grid2>

                            <Grid2 size={{ xs: 0, md: 3 }} />
                            <Grid2 container size={{ sm: 12, md: 6 }}>
                                <Grid2 size={{ xs: 12, md: 4 }}>
                                    <CompetitionType type={competition.type} />
                                </Grid2>
                                <Grid2 size={{ xs: 12, md: 4 }}>
                                    <CompetitionPhase phase={competition.phase} />
                                </Grid2>
                                <Grid2 size={{ xs: 12, md: 4 }}>
                                    <CompetitionDate phase={competition.phase} date={competition.relevantDate} />
                                </Grid2>
                            </Grid2>
                            <Grid2 size={{ xs: 0, md: 3 }} />

                            <Grid2 size={{ sm: 12, md: 4 }}>
                                <h3>Creator:</h3>
                                <UserButton username={competition.creatorname} />
                            </Grid2>
                            <Grid2 size={{ xs: 0, md: 2, lg: 4 }} />
                            <Grid2 container size={{ sm: 12, md: 6, lg: 4 }} sx={{ alignContent: "center" }}>
                                {competition.phase === "preparation" && (
                                    <>
                                        <Grid2 size={{ xs: 12, md: 5 }} >
                                            <Button variant="outlined" onClick={handleJoin}>Jelentkezés</Button>
                                        </Grid2>
                                        <Grid2 size={{ xs: 12, md: 7 }} >
                                            <Button variant="outlined" onClick={handleFindGroups}>Csapat keresése</Button>
                                        </Grid2>
                                    </>
                                )}
                                {competition.phase === "voting" && (
                                    <>
                                        <Grid2 size={{ xs: 12, md: 6 }} >
                                            <Button variant="outlined" onClick={handleListSubmissions}>Szavazás</Button>
                                        </Grid2>
                                    </>
                                )}
                                {competition.phase === "finished" && (
                                    <>
                                        <Grid2 size={{ xs: 12, md: 6 }} >
                                            <Button variant="outlined" onClick={handleListSubmissions}>Eredmények megtekintése</Button>
                                        </Grid2>
                                    </>
                                )}
                                {competition.phase === "submit" && (
                                    <>
                                        <Grid2 size={{ xs: 12, md: 12 }} >
                                        <h3>A beadási idő leteltével megnyílik a szavazási időszak</h3>

                                        </Grid2>
                                    </>
                                )}
                            </Grid2>

                            <Grid2 size={{ xs: 0, md: 2 }} />
                            <Grid2 size={{ xs: 12, md: 8 }} sx={{ flexGrow: 1, display: "flex", flexDirection: "row", alignContent: "flex-start" }} >
                                <Card variant="outlined" sx={{ flexGrow: 1, flexDirection: "column" }}>
                                    <p style={{ padding: "0 10px", lineBreak: "auto", textAlign: "left", whiteSpace: "pre-line" }}>{competition.description}</p>
                                </Card>
                            </Grid2>
                            <Grid2 size={{ xs: 0, md: 2 }} />
                        </Grid2>
                    </>)}
                </Box>
            </Paper>
        </>
    )
}
