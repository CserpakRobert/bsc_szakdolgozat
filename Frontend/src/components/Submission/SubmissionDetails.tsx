import { useEffect, useState } from "react"
import { useParams } from "react-router-dom"
import { api } from "../../globalHelpers/apiAxios"
import SubmissionInterface from "./SubmissionInterface"
import { Card, Grid2, Paper } from "@mui/material"


export default function SubmissionDetails() {
    const { id } = useParams()
    const [submission, setSubmission] = useState<undefined | SubmissionInterface>(undefined)

    useEffect(() => {
        api.get(`/submission/${id}`).then(resp => setSubmission(resp.data)).catch(console.log);


    }, [id])


    return (
        <>
            <Paper variant="detailsPaper">
                {!submission ? (<>
                    <h1>Error</h1>
                </>) : (
                    <>
                        <Grid2 container spacing={2} sx={{ flexGrow: 1, alignContent: "flex-start" }}>

                            <Grid2 size={12}>
                                <h1>{submission.title}</h1>
                            </Grid2>
                            <Grid2 size={{ xs: 12, md: 8 }} sx={{ flexGrow: 1, display: "flex", flexDirection: "row", alignContent: "flex-start" }} >
                                <Card variant="outlined" sx={{ flexGrow: 1, flexDirection: "column" }}>
                                    <p style={{ lineBreak: "anywhere" }}>{submission.description}</p>
                                </Card>
                            </Grid2>

                        </Grid2>)

                    </>
                )
                }
            </Paper>
        </>
    )


}

