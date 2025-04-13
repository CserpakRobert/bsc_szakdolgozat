    import { AppBar, Grid2, MenuItem, Pagination, Select, SelectChangeEvent, Toolbar } from "@mui/material"
import { useEffect, useState } from "react"
import { CompetitionSmallInterface } from "./CompetitionInterface"
import CompetitionDetailsCard from "./CompetitionDetailsCard"
import { api } from "../../globalHelpers/apiAxios"

export default function CompetitionList() {
    const [competitions, setCompetitions] = useState<[CompetitionSmallInterface]>()

    const [selectedPhase, setPhase] = useState("-")
    const [selectedType, setType] = useState("-")
    const [page, setPage] = useState(0)
    const [pageCount, setPageCount] = useState(1)
    useEffect(() => {

        api.post('/competition/list', { type: selectedType, phase: selectedPhase, page: page }).then(resp => { setCompetitions(resp.data.competitions); setPageCount(resp.data.pageCount) });

    }, [selectedPhase, page, selectedType]
    )
    const handlePhase = (event: SelectChangeEvent) => {
        setPhase(event.target.value as string)
    }
    const handleType = (event: SelectChangeEvent) => {
        setType(event.target.value as string)
    }
    const handlePage = (_event: React.ChangeEvent<unknown>, value: number) => {
        setPage(value)
    }
    //TODO SCrolling
    return (<>



            <AppBar position="fixed" sx={{ marginTop: 10, overflow: "hidden" }}>
                <Toolbar sx={{ gap: 2 }}>
                    <MenuItem disabled={true}>Szűrés</MenuItem>
                    <Select
                        value={selectedPhase}
                        onChange={handlePhase}
                    >
                        <MenuItem value={"-"}>Minden Fázis</MenuItem>
                        <MenuItem value={"preparation"}>Jelentkezés</MenuItem>
                        <MenuItem value={"vote"}>Szavazás</MenuItem>
                        <MenuItem value={"finished"}>Befejezett</MenuItem>
                    </Select>
                    <Select
                        value={selectedType}
                        onChange={handleType}
                    >
                        <MenuItem value={"-"}>Minden Típus</MenuItem>
                        <MenuItem value={"shortstory"}>Történet</MenuItem>
                        <MenuItem value={"movie"}>Film</MenuItem>
                        <MenuItem value={"music"}>Zene</MenuItem>
                    </Select>
                </Toolbar>
            </AppBar>
        <Grid2 container position={"relative"} gap={2} marginTop={20} width={"100%"} sx={{justifyContent: "center"}}>

            {competitions?.map(c =>
                <Grid2 size={{ xs: 12, sm: 5, md: 3, lg: 2.5, xl: 2.8 }} key={c.id} >
                    <CompetitionDetailsCard competition={c} />
                </Grid2>
            )}
            <Grid2 size={12} justifyItems="center">
                <Pagination count={pageCount} page={page} onChange={handlePage} />
            </Grid2>
        </Grid2>
        

    </>)
}