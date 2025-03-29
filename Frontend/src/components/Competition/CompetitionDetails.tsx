import { useEffect, useState } from "react"
import { useNavigate, useParams } from "react-router-dom"
import { api } from "../../globalHelpers/apiAxios"
import { Paper, Grid2, Card, Button } from "@mui/material"
import CompetitionInterface from "./CompetitionInterface"
import UserButton from "../User/UserButton"



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
    return (
        <>
            <Paper variant="detailsPaper">
                {!competition ? (<>

                    <h1>Error</h1>
                </>) : (<>
                    <Grid2 container spacing={2} sx={{ flexGrow: 1, alignContent: "flex-start" }}>
                        { }
                        <Grid2 size={12}>
                            <h1>{competition.title}</h1>
                        </Grid2>
                        <Grid2 size={{ xs: 12, md: 8 }} sx={{ flexGrow: 1, display: "flex", flexDirection: "row", alignContent: "flex-start" }} >
                            <Card variant="outlined" sx={{ flexGrow: 1, flexDirection: "column" }}>
                                <p style={{ lineBreak: "anywhere" }}>{competition.description}</p>
                            </Card>
                        </Grid2>
                        <Grid2 size={4}>
                            <h3>Creator:</h3>
                            <UserButton username={competition.creatorname} />
                        </Grid2>
                        {competition.submissionsEnabled && !competition.isCreator && !competition.isModerator ? (
                            <>
                                <Grid2 size={8}>
                                </Grid2>
                                <Grid2 size={4}>
                                    <Button onClick={()=> navigate(`/submission/create/${competition.id}`)}>Create a submission</Button>
                                </Grid2>
                            </>
                        ) : (<></>)
                        }

                    </Grid2>
                </>)}
            </Paper>



        </>


    )
}
