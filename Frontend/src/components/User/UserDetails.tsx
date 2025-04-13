import { AppBar, Box, Button, Grid2, Pagination, Paper, Stack, Tab, Tabs, Toolbar, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { api } from "../../globalHelpers/apiAxios";
import { SubmissionListDetailsInterface, SubmissionSmallInterface } from "../Submission/SubmissionInterface";
import SubmissionSmallDetails from "../Submission/SubmissionSmallDetails";

interface userDetails {
    id: number;
    userName: string;
}
export default function UserDetails() {
    const { name } = useParams()

    const [user, setUser] = useState<userDetails | undefined>(undefined)

    const [page, setPage] = useState(0)
    const [pageCount, setPageCount] = useState(1)

    const [currentList, setCurrentList] = useState<"finishedSub" | "votingSub" | "createdGroups" | "createdComp">("finishedSub")
    const [submissions, setSubmissions] = useState<SubmissionListDetailsInterface[]>([])

    const handleTabChange = (_event: React.SyntheticEvent, newValue: string) => {
        setCurrentList(newValue as "finishedSub" | "votingSub" | "createdGroups" | "createdComp");
        if (newValue == "finishedSub") {
            api.post(`/submission/list`, { sortByPlacement: true, userName: user?.userName, page: page, phase:"finished" }).then(resp => { setSubmissions(resp.data.submissions); setPageCount(resp.data.pages) })
        }
    }
    const handlePage = (_event: React.ChangeEvent<unknown>, value: number) => {
        setPage(value)
    }

    useEffect(() => {
        api.get(`/user/${name}`).then(resp => setUser(resp.data))
        api.post(`/submission/list`, { sortByPlacement: true, userName: name, page: page, phase:"finished" }).then(resp => { setSubmissions(resp.data.submissions); setPageCount(resp.data.pages) })

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
                    <Tabs value={currentList} onChange={handleTabChange} aria-label="basic tabs example">
                        <Tab value={"finishedSub"} label="Pályázatok" />
                        <Tab value={"votingSub"} label="Pályázatok" />
                        <Tab value={"createdComp"} label="Készített Versenyek" />
                        <Tab value={"createdGroups"} label="Jelenlegi csoportok" />
                    </Tabs>
                </Grid2>

                <Grid2 container size={{ sm: 12, md: 12 }} spacing={2} sx={{ marginTop: "2rem", justifyContent: "center", alignItems: "center" }}>
                    <Grid2 size={12} justifyItems="center">
                        <Pagination count={pageCount} page={page} onChange={handlePage} />
                    </Grid2>
                    <Grid2 container size={12}  sx={{ marginTop: "2rem", justifyContent: "center", alignItems: "center" }}>

                        {currentList == "finishedSub" &&
                            <>
                                {submissions?.map((submission) => (<>
                                    <Grid2 size={{ xs: 6, md: 4, lg: 3 }} key={submission.id} sx={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
                                        <SubmissionSmallDetails submission={submission} />
                                    </Grid2>
                                </>))}
                            </>}
                    </Grid2>
                </Grid2>

            </Grid2 >



        </>

    )
}