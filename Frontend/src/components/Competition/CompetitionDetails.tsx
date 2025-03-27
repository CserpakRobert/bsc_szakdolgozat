import { useEffect, useState } from "react"
import { useParams } from "react-router-dom"
import { api } from "../../globalHelpers/apiAxios"
import { Paper, Grid2, Card,  Button } from "@mui/material"
import CompetitionInterface from "./CompetitionInterface"
import UserButton from "../User/userButton"


export default function CompetitionDetails() {
    const { id } = useParams()
    const [error, setError] = useState(false)
    const [competition, setCompetition] = useState<undefined | CompetitionInterface>(undefined)



    useEffect(() => {
        console.log(id);
        api.get(`competition/${id}`)
            .then(
                resp => setCompetition(resp.data)
            )
            .catch(
                () => setError(true)
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
                                <p style={{lineBreak:"anywhere"}}>{competition.description}</p>
                            </Card>
                        </Grid2>
                        <Grid2 size={4}>
                            <h3>Creator:</h3>
                            <UserButton username={competition.creatorname} />
                        </Grid2>
                        {competition.submissionsenabled ? (
                            <>
                                <Grid2 size={8}>
                                </Grid2>
                                <Grid2 size={4}>
                                    <Button>Create a submission</Button>
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

{/* <List sx={{maxHeight:"100px", overflow:"auto"}}>
{["1", "2", "3","1", "2", "3","1", "2", "3"].map(f =>
    <UserButton username={f} />
)}
</List> */}