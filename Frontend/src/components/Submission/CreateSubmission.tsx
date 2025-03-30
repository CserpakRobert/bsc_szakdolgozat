import { Box, Button, Paper, TextField } from "@mui/material";
import { FormEvent, useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import CompetitionInterface from "../Competition/CompetitionInterface";
import { api } from "../../globalHelpers/apiAxios";


export default function CreateSubmission() {
    const { id } = useParams()
    const [title, setTitle] = useState("")
    const [competition, setCompetition] = useState<undefined | CompetitionInterface>(undefined)
    const [description, setDescription] = useState("")
    const [trailerLink, setTrailerLink] = useState("")
    const [movieLink, setMovieLink] = useState("")
    const [musicLink, setMusicLink] = useState("")
    const [shortStory, setshortStory] = useState("")

    useEffect(() => {

        api.get(`competition/${id}`)
            .then(
                resp => setCompetition(resp.data)
            )
            .catch(

        )
    }, [id])

    const navigate = useNavigate()

    const handleSubmit = async (event: FormEvent) => {
        event.preventDefault()
        if (competition?.isMovieCompetition) {
            api.post("submission/create", { competitionid: id, title, description, moviedetails: { trailerLink, movieLink } }).then(resp => navigate(`/submission/${resp.data.id}`))
        } else if (competition?.isMusicCompetition) {
            api.post("submission/create", { competitionid: id, title, description, musicdetails: { link: musicLink } }).then(resp => navigate(`/submission/${resp.data.id}`))
        }else if (competition?.isShortStoryCompetition) {
            api.post("submission/create", { competitionid: id, title, description, story: { link: musicLink } }).then(resp => navigate(`/submission/${resp.data.id}`))
        }

    }
    return (<>

        <Paper variant="createPaper">
            {competition ? (

                <>
                    <h1>Pályázat beadása a(z) {competition.title} versenyre</h1>
                    <Box component="form" sx={{ display: "flex", flexDirection: "column", gap: 2, width: { xs: "100%" } }} onSubmit={handleSubmit}>

                        <TextField
                            className="outlined-input"
                            label="Cím"
                            type="text"
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                        />
                        <TextField
                            className="outlined-input"
                            label="Leírás"
                            type="text"
                            value={description}
                            multiline
                            rows={5}
                            onChange={(e) => setDescription(e.target.value)}
                        />
                        {competition.isMovieCompetition ? (<>

                            <TextField
                                className="outlined-input"
                                label="Trailer Link"
                                type="text"
                                value={trailerLink}
                                onChange={(e) => setTrailerLink(e.target.value)}
                            />                        <TextField
                                className="outlined-input"
                                label="Movie Link"
                                type="text"
                                value={movieLink}
                                onChange={(e) => setMovieLink(e.target.value)}
                            />
                        </>) : (<></>)}
                        {competition.isMusicCompetition ? (<>

                            <TextField
                                className="outlined-input"
                                label="Music Link"
                                type="text"
                                value={musicLink}
                                onChange={(e) => setMusicLink(e.target.value)}
                            />
                        </>) : (<></>)}
                        {competition.isShortStoryCompetition ? (<>

                            <TextField
                                className="outlined-input"
                                label="Történet"
                                type="text"
                                multiline
                                rows={10}
                                value={shortStory}
                                onChange={(e) => setshortStory(e.target.value)}
                            />
                        </>) : (<></>)}
                        <Button sx={{ width: "50%", display: "flex", flexDirection: "column", }} variant="outlined" type="submit">Create</Button>
                    </Box>
                </>
            ) : (

                <>

                </>
            )}

        </Paper >

    </>)
}