import { Grid2 } from "@mui/material"
import { useEffect, useState } from "react"
import { CompetitionSmallInterface } from "./CompetitionInterface"
import CompetitionDetailsCard from "./CompetitionDetailsCard"
import { api } from "../../globalHelpers/apiAxios"

export default function CompetitionList() {
    const [competition, setCompetition] = useState<[CompetitionSmallInterface]>()

    useEffect(() => {

        api.post('/competition/list', { data: { type: "", phase: "", page: 0 } }).then(resp => setCompetition(resp.data));
        console.log("asd");

    }, []
    )
    return (<>
        <Grid2 container gap={1} flexGrow="inherit">
            {competition?.map(c =>
                <Grid2 size={{ xs: 12, sm: 6, md: 4, lg: 2 }} key={c.id}>
                    <CompetitionDetailsCard competition={c} />
                </Grid2>
            )}
        </Grid2>
        <h1>Hello!</h1>
    </>)
}