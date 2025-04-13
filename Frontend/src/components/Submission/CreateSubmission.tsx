import {  Button, Grid2, Paper, TextField } from "@mui/material";
import { FormEvent, useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { api } from "../../globalHelpers/apiAxios";


export default function CreateSubmission() {
    const { id } = useParams()

    const [type, setType] = useState<"movie" | "music" | "shortstory" | undefined>(undefined)

    const [title, setTitle] = useState("")

    const [description, setDescription] = useState("")
    const [trailerLink, setTrailerLink] = useState("")
    const [movieLink, setMovieLink] = useState("")
    const [musicLink, setMusicLink] = useState("")
    const [shortStory, setshortStory] = useState("")

    useEffect(() => {

        api.get(`submission/type/${id}`)
            .then(
                resp => setType(resp.data)
            )
        // api.get(`competition/${id}`)
        //     .then(resp => setCompetition(resp.data))
    }, [id])

    const navigate = useNavigate()

    const handleSubmit = async (event: FormEvent) => {
        event.preventDefault()

        if (!type) return
        if (type == "movie") {
            api.post("submission/create/movie",
                {
                    id: id,
                    title,
                    description,
                    trailerLink: extractYouTubeVideoId(trailerLink),
                    movieLink: extractYouTubeVideoId(movieLink)
                }
            ).then(resp => navigate(`/submission/${resp.data}`))
        }
        if (type == "music") {
            api.post("submission/create/music", { id: id, title, description, link: extractYouTubeVideoId(musicLink) }).then(resp => navigate(`/submission/${resp.data}`))
        }
        if (type == "shortstory") {
            api.post("submission/create/story", { id: id, title, description, story: shortStory }).then(resp => navigate(`/submission/${resp.data}`))
        }

    }
    function extractYouTubeVideoId(url: string): string | null {
        const regex =
            /(?:youtube\.com\/(?:.*v=|v\/|embed\/|shorts\/)|youtu\.be\/)([a-zA-Z0-9_-]{11})/;
        const match = url.match(regex);
        return match ? match[1] : null;
    }
    return (<>

        <Paper variant="createPaper">
            {type ? (

                <>
                    <Grid2 container spacing={2} sx={{ alignSelf: "center" }} width={{ sx: "80%", md: "80%" }} >
                        <Grid2 size={{ sm: 12, lg: 12 }}>
                            <h1>Pályázat beadása </h1>
                        </Grid2>
                        <Grid2 size={{ sm: 12, lg: 12 }}>
                            <TextField
                                className="outlined-input"
                                label="Cím"
                                sx={{ width: "80%" }}
                                type="text"
                                value={title}
                                onChange={(e) => setTitle(e.target.value)}
                            />
                        </Grid2>
                        <Grid2 size={{ sm: 12 }}>
                            <TextField
                                className="outlined-input"
                                label="Leírás"
                                type="text"
                                sx={{ width: "80%" }}
                                value={description}
                                multiline
                                rows={5}
                                onChange={(e) => setDescription(e.target.value)}
                            />
                        </Grid2>
                        {type == "movie" && <>
                            <Grid2 size={{ sm: 12 }}>
                                <TextField
                                    className="outlined-input"
                                    label="Trailer Link"
                                    type="text"
                                    sx={{ width: "80%" }}
                                    value={trailerLink}
                                    onChange={(e) => setTrailerLink(e.target.value)}
                                />
                            </Grid2>
                            <Grid2 size={{ sm: 12 }}>
                                <TextField
                                    className="outlined-input"
                                    label="Movie Link"
                                    type="text"
                                    sx={{ width: "80%" }}
                                    value={movieLink}
                                    onChange={(e) => setMovieLink(e.target.value)}
                                />
                            </Grid2>

                        </>}
                        {type == "music" && <>
                            <Grid2 size={{ sm: 12 }}>
                                <TextField
                                    className="outlined-input"
                                    label="Music Link"
                                    type="text"
                                    sx={{ width: "80%" }}
                                    value={musicLink}
                                    onChange={(e) => setMusicLink(e.target.value)}
                                />
                            </Grid2>

                        </>}
                        {type == "shortstory" && <>
                            <Grid2 size={{ sm: 12 }}>
                                <TextField
                                    className="outlined-input"
                                    label="Történet"
                                    type="text"
                                    multiline
                                    sx={{ width: "80%" }}
                                    rows={10}
                                    value={shortStory}
                                    onChange={(e) => setshortStory(e.target.value)}
                                />
                            </Grid2>
                        </>}
                        <Grid2 size={{ sm: 4 }} />
                        <Grid2 size={{ sm: 8 }}>
                            <Button
                                sx={{ width: "50%", display: "flex", flexDirection: "column", }}
                                variant="outlined"
                                onClick={handleSubmit}>
                                Create
                            </Button>
                        </Grid2>

                    </Grid2>
                </>
            ) : (

                <>

                </>
            )}

        </Paper >

    </>)
}