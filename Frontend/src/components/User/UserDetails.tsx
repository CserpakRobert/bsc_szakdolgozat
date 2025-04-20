import { Grid2, Pagination, Paper, Tab, Tabs } from "@mui/material";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { api } from "../../Utils/apiAxios";
import { SubmissionListDetailsInterface } from "../Submission/SubmissionInterface";
import SubmissionSmallDetails from "../Submission/SubmissionSmallDetails";
import CompetitionDetailsCard from "../Competition/CompetitionDetailsCard";
import { CompetitionSmallInterface } from "../Competition/CompetitionInterface";
import { GroupSmallDetailsInterface } from "../Group/GroupInterface";
import { GroupSmallDetails } from "../Group/GroupSmallDetails";
import { useAuth } from "../../Utils/useAuth";

interface userDetails {
    id: number;
    userName: string;
}
export default function UserDetails() {
    const { name } = useParams()

    const [user, setUser] = useState<userDetails | undefined>(undefined)
    const { userName } = useAuth()

    const [privateTab, setprivateTab] = useState(false)
    const [page, setPage] = useState(1)
    const [pageCount, setPageCount] = useState(1)

    const [currentList, setCurrentList] = useState<"finishedSub" | "votingSub" | "createdGroups" | "createdComp" | "pendingGroups" | "privateGroups" | "invites" | "requests" | "inSubmissionSubmissions">("finishedSub")
    const [submissions, setSubmissions] = useState<SubmissionListDetailsInterface[]>([])
    const [competitions, setCompetitions] = useState<CompetitionSmallInterface[]>([])
    const [groups, setGroups] = useState<GroupSmallDetailsInterface[]>([])

    const handleTabChange = (_event: React.SyntheticEvent, newValue: string) => {
        setCurrentList(newValue as "finishedSub" | "votingSub" | "createdGroups" | "createdComp" | "pendingGroups" | "privateGroups" | "invites" | "requests" | "inSubmissionSubmissions");
        // if (newValue == "finishedSub") {
        //     api.post(`/submission/list`, { sortByPlacement: true, userName: user?.userName, page: page, phase: "finished" }).then(resp => { setSubmissions(resp.data.submissions); setPageCount(resp.data.pages) })
        // }
        // else if (newValue == "votingSub") {
        //     api.post(`/submission/list`, { sortByPlacement: true, userName: user?.userName, page: page, phase: "voting" }).then(resp => { setSubmissions(resp.data.submissions); setPageCount(resp.data.pages) })
        // }
        // else if (newValue == "createdComp") {
        //     api.post(`/competition/list`, { userName: user?.userName, page: page }).then(resp => { setCompetitions(resp.data.competitions); setPageCount(resp.data.pages) })
        // }
        // else if (newValue == "createdGroups") {
        //     api.post(`/group/userOpenGroups/${user?.userName}`, page).then(resp => { setGroups(resp.data.groups); setPageCount(resp.data.pages) })
        // }
        switch (newValue) {
            case "finishedSub":
                api.post(`/submission/list`, { sortByPlacement: true, userName: user?.userName, page: page - 1, phase: "finished" }).then(resp => { setSubmissions(resp.data.submissions); setPageCount(resp.data.pages) });
                break;
            case "votingSub":
                api.post(`/submission/list`, { sortByPlacement: true, userName: user?.userName, page: page - 1, phase: "voting" }).then(resp => { setSubmissions(resp.data.submissions); setPageCount(resp.data.pages) });
                break;
            case "createdComp":
                api.post(`/competition/list`, { userName: user?.userName, page: page - 1 }).then(resp => { setCompetitions(resp.data.competitions); setPageCount(resp.data.pages) });
                break;
            case "createdGroups":
                api.post(`/group/userOpenGroups/${user?.userName}`, page - 1).then(resp => { setGroups(resp.data.groups); setPageCount(resp.data.pages) });
                break;
            case "pendingGroups":
                api.post(`/group/userPendingGroups/${user?.userName}`, page - 1).then(resp => { setGroups(resp.data.groups); setPageCount(resp.data.pages) });
                break;
            // case "privateGroups":
            // api.post(`/group/userPrivateGroups/${user?.userName}`, page).then(resp => { setGroups(resp.data.groups); setPageCount(resp.data.pages) });
            // break;
            // case "invites":
            // api.get(`/group/invitedGroups/${user?.userName}`, page).then(resp => { setGroups(resp.data.groups); setPageCount(resp.data.pages) });
            // break;
            // case "requests":
            // api.post(`/group/userRequests/${user?.userName}`, page).then(resp => { setGroups(resp.data.groups); setPageCount(resp.data.pages) });
            // break;
            // case "inSubmissionSubmissions":
            // api.post(`/submission/list`, { userName: user?.userName, page: page, phase: "inSubmission" }).then(resp => { setSubmissions(resp.data.submissions); setPageCount(resp.data.pages) });
            // break;
            default:
                break;
        }
    }
    const handlePrivateTabChange = (_event: React.SyntheticEvent, newValue: boolean) => {
        setprivateTab(newValue);
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
                <Grid2 position={"relative"} container size={{ xs: 12, md: 6, lg: 4 }}>
                    <Paper variant="detailsPaper" sx={{ padding: 2, marginTop: 0, width: "30%", display: "relative", justifyContent: "center", alignItems: "center" }}>
                        <Grid2 size={{ xs: 12, md: 12, lg: 12 }}>
                            <h1>{user?.userName}</h1>
                        </Grid2>
                        <Grid2 size={{ xs: 12, md: 12 }}>
                            <h1>profilja</h1>
                        </Grid2>
                    </Paper>
                </Grid2>
                <Grid2 container size={{ sm: 12, md: 12 }} spacing={2} sx={{ marginTop: "2rem", justifyContent: "center", alignItems: "center" }}>
                    {userName == user?.userName && (

                        <Tabs value={privateTab}
                            onChange={handlePrivateTabChange}
                            aria-label="basic tabs example"
                            variant="scrollable"
                            scrollButtons="auto">
                            <Tab value={false} wrapped label="Publikus" />
                            <Tab  value={true} wrapped label="Privát" />
                        </Tabs>

                    )}
                </Grid2>
                <Grid2 container size={{ sm: 12, md: 12 }} spacing={2} sx={{ marginTop: "2rem", justifyContent: "center", alignItems: "center" }}>
                    {!privateTab && (
                        <Tabs value={currentList}
                            onChange={handleTabChange}
                            aria-label="basic tabs example"
                            variant="scrollable"
                            scrollButtons="auto">
                            <Tab value={"finishedSub"} wrapped label="Befejezett Pályázatok" />
                            <Tab value={"votingSub"} wrapped label="Szavazás allatt álló pályázatok" />
                            <Tab value={"createdComp"} label="Készített Versenyek" />
                            <Tab value={"createdGroups"} label="Jelenlegi csoportok" />
                        </Tabs>
                    )}
                    {privateTab && (
                        <Tabs value={currentList}
                            onChange={handleTabChange}
                            aria-label="basic tabs example"
                            variant="scrollable"
                            scrollButtons="auto">
                            <Tab value={"privateGroups"} wrapped label="Privát csoportok" />
                            <Tab value={"invites"} wrapped label="Meghívások" />
                            <Tab value={"requests"} label="Jelentkezések" />
                            <Tab value={"inSubmissionSubmissions"} label="Beadás alatt álló pályázatok" />
                        </Tabs>
                    )}
                </Grid2>

                <Grid2 container size={{ sm: 12, md: 12 }} spacing={2} sx={{ marginTop: "2rem", justifyContent: "center", alignItems: "center" }}>
                    <Grid2 size={12} justifyItems="center">
                        <Pagination count={pageCount} page={page} onChange={handlePage} />
                    </Grid2>
                    <Grid2 container size={12} sx={{ marginTop: "2rem", justifyContent: "center", alignItems: "center" }}>

                        {(currentList == "finishedSub" || currentList == "votingSub") &&
                            <>
                                {submissions?.map((submission) => (
                                    <Grid2 size={{ xs: 12, sm: 6, md: 3, lg: 3 }} key={submission.id} sx={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
                                        <SubmissionSmallDetails submission={submission} />
                                    </Grid2>
                                ))}
                            </>}
                        {currentList == "createdComp" && (
                            <>

                                {competitions?.map((competition) => (
                                    <Grid2 size={{ xs: 12, sm: 6, md: 3, lg: 3 }} key={competition.id} sx={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
                                        <CompetitionDetailsCard competition={competition} />
                                    </Grid2>
                                ))}
                            </>)}
                        {currentList == "createdGroups" && (
                            <>
                                {groups?.map((group) => (
                                    <Grid2 size={{ xs: 12, sm: 6, md: 3, lg: 3 }} key={group.groupId} sx={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
                                        <GroupSmallDetails group={group} />
                                    </Grid2>
                                ))}
                            </>)}
                    </Grid2>
                </Grid2>

            </Grid2 >



        </>

    )
}