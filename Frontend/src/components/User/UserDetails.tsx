import { Grid2, Pagination, Paper, Tab, Tabs } from "@mui/material";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { api } from "../../globalHelpers/apiAxios";
import { SubmissionListDetailsInterface } from "../Submission/SubmissionInterface";
import SubmissionSmallDetails from "../Submission/SubmissionSmallDetails";
import CompetitionDetailsCard from "../Competition/CompetitionDetailsCard";
import { CompetitionSmallInterface } from "../Competition/CompetitionInterface";
import { GroupSmallDetailsInterface } from "../Group/GroupInterface";
import { GroupSmallDetails } from "../Group/GroupSmallDetails";
import { useAuth } from "../../globalHelpers/useAuth";

interface userDetails {
    id: number;
    userName: string;
}
export default function UserDetails() {
    const { name } = useParams()

    const [user, setUser] = useState<userDetails | undefined>(undefined)
    const { userName } = useAuth()

    const [page, setPage] = useState(0)
    const [pageCount, setPageCount] = useState(1)

    const [currentList, setCurrentList] = useState<"finishedSub" | "votingSub" | "createdGroups" | "createdComp" | "pendingGroups">("finishedSub")
    const [submissions, setSubmissions] = useState<SubmissionListDetailsInterface[]>([])
    const [competitions, setCompetitions] = useState<CompetitionSmallInterface[]>([])
    const [groups, setGroups] = useState<GroupSmallDetailsInterface[]>([])

    const handleTabChange = (_event: React.SyntheticEvent, newValue: string) => {
        setCurrentList(newValue as "finishedSub" | "votingSub" | "createdGroups" | "createdComp"| "pendingGroups");
        if (newValue == "finishedSub") {
            api.post(`/submission/list`, { sortByPlacement: true, userName: user?.userName, page: page, phase: "finished" }).then(resp => { setSubmissions(resp.data.submissions); setPageCount(resp.data.pages) })
        }
        if (newValue == "votingSub") {
            api.post(`/submission/list`, { sortByPlacement: true, userName: user?.userName, page: page, phase: "voting" }).then(resp => { setSubmissions(resp.data.submissions); setPageCount(resp.data.pages) })
        }
        if (newValue == "createdComp") {
            api.post(`/competition/list`, { userName: user?.userName, page: page }).then(resp => { setCompetitions(resp.data.competitions); setPageCount(resp.data.pages) })
        }
        if (newValue == "createdGroups") {
            api.post(`/group/userOpenGroups/${user?.userName}`, page).then(resp => { setGroups(resp.data.groups); setPageCount(resp.data.pages) })
        }
    }

    const handlePage = (_event: React.ChangeEvent<unknown>, value: number) => {
        setPage(value)
    }

    useEffect(() => {
        api.get(`/user/${name}`).then(resp => setUser(resp.data))
        api.post(`/submission/list`, { sortByPlacement: true, userName: name, page: page, phase: "finished" }).then(resp => { setSubmissions(resp.data.submissions); setPageCount(resp.data.pages) })

    }
        , [name, page, setPage, setSubmissions])
    return (
        <>
            <Grid2 container spacing={2} width={"100%"} >
                <Grid2 size={{ xs: 0, md: 2, lg: 4 }} />
                <Grid2 container size={{ xs: 12, md: 6, lg: 4 }}>
                    <Paper variant="detailsPaper" sx={{ padding: 2, marginTop: 0, width: "30%", display: "relative", justifyContent: "center", alignItems: "center" }}>
                        <Grid2 size={{ xs: 12, md: 12, lg: 12 }}>
                            <h1>{user?.userName}</h1>
                        </Grid2>
                        <Grid2 size={{ xs: 12, md: 12 }}>
                            <h1>Profilja</h1>
                        </Grid2>
                    </Paper>
                </Grid2>

                <Grid2 container size={{ sm: 12, md: 12 }} spacing={2} sx={{ marginTop: "2rem", justifyContent: "center", alignItems: "center" }}>
                    <Tabs value={currentList}
                        onChange={handleTabChange}
                        aria-label="basic tabs example"
                        variant="scrollable"
                        scrollButtons="auto">
                        <Tab value={"finishedSub"} wrapped label="Befejezett Pályázatok" />
                        <Tab value={"votingSub"} wrapped label="Szavazás allatt álló pályázatok" />
                        <Tab value={"createdComp"} label="Készített Versenyek" />
                        <Tab value={"createdGroups"} label="Jelenlegi csoportok" />
                        <Tab hidden={userName != user?.userName} value={"pendingGroups"} wrapped label="Függőben lévő csoportok" />
                        <Tab hidden={userName != user?.userName} value={"pendingSubmissions"} wrapped label="Függőben lévő Pályázatok" />

                    </Tabs>
                </Grid2>

                <Grid2 container size={{ sm: 12, md: 12 }} spacing={2} sx={{ marginTop: "2rem", justifyContent: "center", alignItems: "center" }}>
                    <Grid2 size={12} justifyItems="center">
                        <Pagination count={pageCount} page={page} onChange={handlePage} />
                    </Grid2>
                    <Grid2 container size={12} sx={{ marginTop: "2rem", justifyContent: "center", alignItems: "center" }}>

                        {(currentList == "finishedSub" || currentList == "votingSub") &&
                            <>
                                {submissions?.map((submission) => (<>
                                    <Grid2 size={{ xs: 12, sm: 6, md: 3, lg: 3 }} key={submission.id} sx={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
                                        <SubmissionSmallDetails submission={submission} />
                                    </Grid2>
                                </>))}
                            </>}
                        {currentList == "createdComp" && (
                            <>

                                {competitions?.map((competition) => (<>
                                    <Grid2 size={{ xs: 12, sm: 6, md: 3, lg: 3 }} key={competition.id} sx={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
                                        <CompetitionDetailsCard competition={competition} />
                                    </Grid2>
                                </>))}
                            </>)}
                        {currentList == "createdGroups" && (
                            <>
                                {groups?.map((group) => (<>
                                    <Grid2 size={{ xs: 12, sm: 6, md: 3, lg: 3 }} key={group.groupId} sx={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
                                        <GroupSmallDetails group={group} />
                                    </Grid2>
                                </>))}
                            </>)}
                    </Grid2>
                </Grid2>

            </Grid2 >



        </>

    )
}