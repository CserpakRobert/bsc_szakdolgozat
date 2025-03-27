import { Box, Button, Paper, TextField } from "@mui/material";
import { FormEvent, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
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

    useEffect(() => {

        api.get(`competition/${id}`)
            .then(
                resp => setCompetition(resp.data)
            )
            .catch(

        )
    }, [id])

    const handleSubmit = async (event: FormEvent) => {
        event.preventDefault()
        if (competition?.isMovieCompetition) {
            api.post("submission/create", { competitionid: id, title, description, moviedetails: { trailerLink, movieLink } })
        } else if (competition?.isMusicCompetition) {
            api.post("submission/create", { competitionid: id, title, description, musicdetails: { link:musicLink } })

        }

    }
    return (<>

        <Paper variant="createPaper">
            {competition ? (

                <>
                    <h1>Add Submission to  </h1>
                    <h1>{competition.title}</h1>
                    <Box component="form" sx={{ display: "flex", flexDirection: "column", gap: 2, width: { xs: "100%" } }} onSubmit={handleSubmit}>

                        <TextField
                            className="outlined-input"
                            label="title"
                            type="text"
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                        />
                        <TextField
                            className="outlined-input"
                            label="description"
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
                                label="description"
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