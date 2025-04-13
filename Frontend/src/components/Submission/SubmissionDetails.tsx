import { useEffect, useState } from "react"
import { useNavigate, useParams } from "react-router-dom"
import { api } from "../../globalHelpers/apiAxios"
import SubmissionInterface from "./SubmissionInterface"
import { Button, Card, Grid2, List, ListItem, Paper } from "@mui/material"
import EmbeddedLink from "./embeddedLink"
import { useAuth } from "../../globalHelpers/useAuth"
import UserButton from "../User/UserButton"
import Placement from "./Placement"


export default function SubmissionDetails() {
    const { id } = useParams()
    const [submission, setSubmission] = useState<undefined | SubmissionInterface>(undefined)
    const { userName } = useAuth()
    const navigate = useNavigate()

    useEffect(() => {
        api.get(`/submission/${id}`).then(resp => setSubmission(resp.data))


    }, [id])
    const handleVote = () => {
        api.post(`/submission/vote/${id}`)
            .then(() => {
                if (submission) {
                    const newSub = { ...submission };
                    newSub.canVote = false;
                    newSub.hasVote = true;
                    newSub.votes++;
                    setSubmission(newSub);
                }
            })
    }
    const handleUnVote = () => {
        api.post(`/submission/unvote/${id}`)
            .then(() => {
                if (submission) {
                    const newSub = { ...submission };
                    newSub.canVote = true;
                    newSub.hasVote = false;
                    newSub.votes--;
                    setSubmission(newSub);
                }
            })
    }

    const handleCreate = () => {
        navigate("/submission/create/" + id)
    }

    return (
        <>
            <Paper variant="detailsPaper">
                {!submission ? (<>
                    <h1>Error</h1>
                </>) : (<>
                    {submission.creatorName == userName && !submission.isSubmitted ? (<>
                        <Grid2 container>
                            <Grid2 size={12}>
                                <h1>Még nem adtad be a pályázatot</h1>
                            </Grid2>
                            <Grid2 size={12}>
                                <Button variant="outlined" onClick={handleCreate} title="Vote!" >Pályázat készítése</Button>
                            </Grid2>
                        </Grid2>
                    </>) : (<>
                        {submission.participateNames.includes(userName) && !submission.isSubmitted && (
                            <h1>Még nem adtatok be pályázatot</h1>
                        )}
                        {/*TODO*/}
                    </>)}

                    {submission.isSubmitted && (<>

                        <Grid2 container spacing={2} sx={{ alignSelf: "center" }} width={{ sx: "100%", md: "100%" }}>

                            <Grid2 size={12}>
                                <h1>{submission.title}</h1>

                            </Grid2>

                            <Grid2 size={{ sm: 12, md: 8 }}>
                                <Card variant='outlined' sx={{ marginBottom: 2 }}>
                                    Csapat tagjai:
                                    <List sx={{
                                        width: '100%', position: 'relative',
                                        overflow: 'auto',
                                        maxHeight: 100,
                                        minHeight: 30,
                                    }}>
                                        {submission.participateNames.map((name) =>
                                            <ListItem key={name}><UserButton username={name} /></ListItem>
                                        )}
                                    </List>
                                </Card>
                            </Grid2>
                            {submission.placement == 0 && (<>
                                <Grid2 size={4}>
                                    {submission.canVote ? (<>
                                        <Button variant="outlined" onClick={handleVote} title="Vote!" >Szavazás</Button>

                                    </>) : (<>
                                        {submission.hasVote ? (<>
                                            <Button variant="outlined" onClick={handleUnVote} title="Vote!" >visszavonás</Button>
                                        </>)
                                            : (<></>)}
                                    </>)}
                                    <h5>Szavazatok:{submission.votes}</h5>
                                </Grid2>
                            </>)}
                            {submission.placement != 0 && (<>
                                <Grid2 size={4}>
                                    <Placement placement={submission.placement} />
                                </Grid2>
                            </>)}
                            <Grid2 size={{ xs: 12, md: 12 }} sx={{ flexGrow: 1, display: "flex", flexDirection: "row", alignContent: "flex-start" }} >
                                <Card variant="outlined" sx={{ flexGrow: 1, flexDirection: "column" }}>
                                    <p
                                        style={{ padding: "0 10px", lineBreak: "auto", textAlign: "left", whiteSpace: "pre-line" }}
                                    >
                                        {submission.description}
                                    </p>
                                </Card>
                            </Grid2>
                            {submission.movieDetails && (<>
                                <Grid2 size={6}>
                                    <h2>Trailer</h2>
                                </Grid2>
                                <Grid2 size={6}>
                                    <h2>Film</h2>
                                </Grid2>
                                <Grid2 size={6} height={300}>
                                    <EmbeddedLink title={submission.title} link={submission.movieDetails.movieLink} />
                                </Grid2>
                                <Grid2 size={6} height={300}>
                                    <EmbeddedLink title={submission.title} link={submission.movieDetails.trailerLink} />
                                </Grid2>
                            </>)}
                            {submission.musicDetails && (<>
                                <Grid2 size={12}>
                                    <h2>Zene</h2>
                                </Grid2>
                                <Grid2 size={12} height={300}>
                                    <EmbeddedLink title={submission.title} link={submission.musicDetails.link} />
                                </Grid2>
                            </>)}
                            {submission.shortStoryDetails && (<>
                                <Grid2 size={{ xs: 12, md: 12 }} sx={{ flexGrow: 1, display: "flex", flexDirection: "row", alignContent: "flex-start" }} >
                                    <Card variant="outlined" sx={{ flexGrow: 1, flexDirection: "column" }}>
                                        <h2>Történet</h2>
                                        <p
                                            style={{ padding: "0 10px", lineBreak: "auto", textAlign: "left", whiteSpace: "pre-line" }}
                                        >
                                            {submission.description}
                                        </p>
                                    </Card>
                                </Grid2>
                            </>)}


                        </Grid2>

                    </>)}
                </>
                )
                }
            </Paper>
        </>
    )


}

{/* <>
<Grid2 container spacing={2} sx={{ flexGrow: 1, alignContent: "flex-start" }}>

    <Grid2 size={8}>
        <h1>{submission.title}</h1>

    </Grid2>
    <Grid2 size={4}>
        {submission.canVote ? (<>
            <Button variant="outlined" onClick={handleVote} title="Vote!" >Vote!</Button>

        </>) : (<>
        {submission.hasVote?(<>
            <Button variant="outlined" onClick={handleUnVote} title="Vote!" >Revoke vote</Button>
        </>)
        :(<></>)}
        </>)}
        <h5>Current votes:{submission.votes}</h5>
    </Grid2>
    <Grid2 size={{ xs: 12, md: 8 }} sx={{ flexGrow: 1, display: "flex", flexDirection: "row", alignContent: "flex-start" }} >
        <Card variant="outlined" sx={{ flexGrow: 1, flexDirection: "column" }}>
            <p style={{ lineBreak: "anywhere" }}>{submission.description}</p>
        </Card>
    </Grid2>
    {submission.movieDetails ? (<>
        <Grid2 size={12}>
            <EmbeddedLink title={submission.title} link={submission.movieDetails.movieLink} />
        </Grid2>
        <Grid2 size={12}>
            <EmbeddedLink title={submission.title} link={submission.movieDetails.trailerLink} />
        </Grid2>
    </>) : (<>
        {submission.musicDetails ? (<>
            <Grid2 size={6}>
                <EmbeddedLink title={submission.title} link={submission.musicDetails.link} />
            </Grid2>

        </>) : (<>

        </>)}
    </>)}

</Grid2>

</> */}