import { useEffect, useState } from "react"
import { useNavigate, useParams } from "react-router-dom"
import { api } from "../../Utils/apiAxios"
import { Paper, Grid as Grid2, Card, Button, Box, CircularProgress, Pagination } from "@mui/material"
import CompetitionInterface from "./CompetitionInterface"
import UserButton from "../User/UserButton"
import CompetitionDate from "./CompetitionDate"
import CompetitionPhase from "./CompetitionPhase"
import CompetitionType from "./CompetitionType"
import { useAuth } from "../../Utils/useAuth"
import { GroupSmallDetailsInterface } from "../Group/GroupInterface"
import { GroupSmallDetails } from "../Group/GroupSmallDetails"
import { SubmissionListDetailsInterface } from "../Submission/SubmissionInterface"
import SubmissionSmallDetails from "../Submission/SubmissionSmallDetails"

export default function CompetitionDetails() {
    const { id } = useParams()
    const navigate = useNavigate()

    const [competition, setCompetition] = useState<undefined | CompetitionInterface>(undefined)
    const { userName } = useAuth()
    const [groupSmalls, setGroupSmalls] = useState<GroupSmallDetailsInterface[]>([])
    const [page, setPage] = useState(1)
    const [pageCount, setPageCount] = useState(1)
    const [ownGroupId, setOwnGroupId] = useState<number>(0)
    const [ownSubmission, setOwnSubmission] = useState<number>(0)


    const [submissions, setSubmissions] = useState<SubmissionListDetailsInterface[]>([])

    useEffect(() => {
        api.get(`competition/${id}`)
            .then(resp => {
                setCompetition(resp.data);
            }
            )
            .catch(
        )

    }, [id]
    )
    useEffect(() => {
        if (competition?.phase === "finished") {
            api.post(`submission/list`, {
                competitionId: competition.id,
                page: page - 1, sortByDate: false,
                sortByPlacement: true,
                sortByVoteCount: false,
                phase: "finished"
            })
                .then(resp => { setSubmissions(resp.data.submissions); setPageCount(resp.data.pageCount) })
        }
        if (competition?.phase === "voting") {
            api.post(`submission/list`, {
                competitionId: competition.id,
                page: page - 1, sortByDate: false,
                sortByPlacement: false,
                sortByVoteCount: true,
                phase: "voting"
            })
                .then(resp => { setSubmissions(resp.data.submissions); setPageCount(resp.data.pageCount) })
        }
        if (competition?.phase === "submit" || competition?.phase === "finished") {
            api.get(`submission/ownSubmission/${competition.id}`).then(resp => { setOwnSubmission(resp.data) })
        }
        if (competition?.phase === "preparation") {
            api.get(`group/ownGroup/${competition.id}`)
                .then(resp => {
                    if (resp.data) {
                        setOwnGroupId(resp.data)
                    }
                })
            api.post(`group/smallDetails/${competition.id}`, page - 1).then(resp => { setGroupSmalls(resp.data.groups); setPageCount(resp.data.count) })
        }

    }, [competition, page]
    )
    useEffect(() => {

    }, [competition, id, page])


    const handlePage = (_event: React.ChangeEvent<unknown>, value: number) => {
        setPage(value)
    }
    const handleJoin = () => {
        navigate(`/group/${id}`)
    }

    const handleOwnGroup = () => {
        navigate(`/group/details/${ownGroupId}`)
    }

    const handleOwnSubmission = () => {
        navigate(`/submission/${ownSubmission}`)
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
                                        {ownGroupId == 0 &&
                                            (competition.creatorname !== userName && groupSmalls.every(g => g.ownerName != userName)) &&
                                            <>
                                                <Grid2 size={{ xs: 12, md: 12 }} >
                                                    <Button variant="outlined" onClick={handleJoin}>Jelentkezés</Button>
                                                </Grid2>
                                            </>
                                        }
                                        {ownGroupId != 0 && (
                                            <Grid2 size={{ xs: 12, md: 12 }} >
                                                <Button variant="outlined" onClick={handleOwnGroup}>Saját csapat</Button>
                                            </Grid2>
                                        )}
                                    </>
                                )}
                                {(competition.phase === "submit" || competition.phase === "finished") && (
                                    <>
                                        {(competition.phase === "submit" && competition.ownSubmission === 0) && (
                                            <Grid2 size={{ xs: 12, md: 12 }} >
                                                <h3>A beadási idő leteltével megnyílik a szavazási időszak</h3>
                                            </Grid2>)}
                                        {competition.ownSubmission !== 0 && (
                                            <Grid2 size={{ xs: 12, md: 12 }} >
                                                <Button variant="outlined" onClick={handleOwnSubmission}>Saját pályázat</Button>
                                            </Grid2>)}
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
                    {groupSmalls.map((group) => (<>
                        <Grid2 size={{ xs: 6, md: 3, lg: 2 }} key={group.groupId} sx={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
                            <GroupSmallDetails group={group} />
                        </Grid2>
                    </>))}

                </Grid2>
            </>)}
            {
                (competition?.phase === "finished" || competition?.phase === "voting") && (<>
                    <Grid2 container size={{ sm: 12, md: 8 }} spacing={2} sx={{ marginTop: "2rem", justifyContent: "center", alignItems: "center" }}>
                        <Grid2 size={12} justifyItems="center">
                            <Pagination count={pageCount} page={page} onChange={handlePage} />
                        </Grid2>
                        {submissions?.map((submission) => (
                            <Grid2 size={{ xs: 6, md: 4, lg: 3 }} key={submission.id} sx={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
                                <SubmissionSmallDetails submission={submission} />
                            </Grid2>
                        ))}
                    </Grid2>
                </>)}
        </>
    )
}
