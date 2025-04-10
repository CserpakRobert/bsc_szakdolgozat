import { useEffect, useState } from "react"
import { useNavigate, useParams } from "react-router-dom"
import { api } from "../../globalHelpers/apiAxios"
import { Paper, Grid2, Card, Button, Box, CircularProgress, Pagination } from "@mui/material"
import CompetitionInterface from "./CompetitionInterface"
import UserButton from "../User/UserButton"
import CompetitionDate from "./CompetitionDate"
import CompetitionPhase from "./CompetitionPhase"
import CompetitionType from "./CompetitionType"
import { useAuth } from "../../globalHelpers/useAuth"
import { GroupSmallListInterface } from "../Group/GroupInterface"
import { GroupSmallDetails } from "../Group/GroupSmallDetails"

export default function CompetitionDetails() {
    const { id } = useParams()
    const navigate = useNavigate()

    const [competition, setCompetition] = useState<undefined | CompetitionInterface>(undefined)
    const { userName } = useAuth()
    const [groupSmalls, setGroupSmalls] = useState<GroupSmallListInterface | undefined>(undefined)
    const [page, setPage] = useState(0)
    const [pageCount, setPageCount] = useState(1)

    useEffect(() => {
        api.get(`competition/${id}`)
            .then(
                resp => setCompetition(resp.data)
            )
            .catch(
        )
        api.post(`group/smallDetails/${id}`, 0).then(resp => { setGroupSmalls(resp.data); setPageCount(resp.data.count) })
    }, [id]
    )

    useEffect(() => {
        api.post(`group/smallDetails/${id}`, page - 1).then(resp => { setGroupSmalls(resp.data); setPageCount(resp.data.count) })
    }
        , [page, id])
    const handlePage = (_event: React.ChangeEvent<unknown>, value: number) => {
        setPage(value)
    }
    // TODO navigations
    const handleJoin = () => {
        navigate(`/group/${id}`)
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
                        <Grid2 container spacing={2} >
                            { }
                            <Grid2 size={{ sm: 12, md: 12 }}>
                                <h1>{competition.title}</h1>
                            </Grid2>

                            <Grid2 size={{ xs: 0, md: 2 }} />
                            <Grid2 container size={{ sm: 12, md: 8 }}>
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
                            <Grid2 size={{ xs: 0, md: 2 }} />

                            <Grid2 size={{ xs: 0, md: 3 }} />
                            <Grid2 size={{ sm: 12, md: 6 }}>
                                <h3>Verseny készítője:</h3>
                                <UserButton username={competition.creatorname} />
                            </Grid2>
                            <Grid2 size={{ xs: 0, md: 3 }} />

                            <Grid2 size={{ xs: 0, md: 3 }} />
                            <Grid2 container size={{ sm: 12, md: 6, lg: 6 }} sx={{ alignContent: "center" }}>
                                {competition.phase === "preparation" && (
                                    <>
                                        {
                                            competition.creatorname !== userName &&
                                            <>
                                                <Grid2 size={{ xs: 12, md: 12 }} >
                                                    <Button variant="outlined" onClick={handleJoin}>Jelentkezés</Button>
                                                </Grid2>
                                            </>
                                        }
                                    </>
                                )}
                                {competition.phase === "submit" && (
                                    <>
                                        <Grid2 size={{ xs: 12, md: 12 }} >
                                            <h3>A beadási idő leteltével megnyílik a szavazási időszak</h3>
                                        </Grid2>
                                    </>
                                )}
                                {competition.phase === "voting" && (
                                    <>
                                        <Grid2 size={{ xs: 12, md: 12 }} >
                                            <Button variant="outlined" onClick={handleListSubmissions}>Szavazás</Button>
                                        </Grid2>
                                    </>
                                )}
                                {competition.phase === "finished" && (
                                    <>
                                        <Grid2 size={{ xs: 12, md: 12 }} >
                                            <Button variant="outlined" onClick={handleListSubmissions}>Eredmények megtekintése</Button>
                                        </Grid2>
                                    </>
                                )}
                            </Grid2>
                            <Grid2 size={{ xs: 0, md: 3 }} />

                            <Grid2 size={{ xs: 0, md: 1 }} />
                            <Grid2 size={{ xs: 12, md: 10 }} sx={{ flexGrow: 1, display: "flex", flexDirection: "row", alignContent: "flex-start" }} >
                                <Card variant="outlined" sx={{ flexGrow: 1, flexDirection: "column" }}>
                                    <p style={{ padding: "0 10px", lineBreak: "auto", textAlign: "left", whiteSpace: "pre-line" }}>{competition.description}</p>
                                </Card>
                            </Grid2>
                            <Grid2 size={{ xs: 0, md: 1 }} />
                        </Grid2>
                    </>)}
                </Box>
            </Paper>
            {competition?.phase === "preparation" && (<>
                <Grid2 container size={{ sm: 12, md: 8 }} spacing={2} sx={{ marginTop: "2rem", justifyContent: "center", alignItems: "center" }}>

                    <Grid2 size={12} justifyItems="center">
                        <Pagination count={pageCount} page={page} onChange={handlePage} />
                    </Grid2>
                    {groupSmalls?.groups?.map((group) => (<>
                        <Grid2 size={{ xs: 6, md: 3, lg: 2 }} key={group.groupId} sx={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
                            <GroupSmallDetails group={group} />
                        </Grid2>
                    </>))}

                </Grid2>
            </>)}
        </>
    )
}
