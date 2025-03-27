import { Box, Button, FormControlLabel, Paper,  Radio,  RadioGroup,  TextField } from "@mui/material";
import { FormEvent, use, useState } from "react";
import { api } from "../../globalHelpers/apiAxios";
import { useNavigate } from "react-router-dom";

export default function CreateCompetition() {
    const [title, setTitle] = useState("")
    const [description, setdescription] = useState("")
    const [category, setCategory] = useState("movie")
    const navigate = useNavigate()
    const handlesubmit = async (event: FormEvent) => {
        event.preventDefault();
        return api.post("/competition/create", {title,description,category})
        .then(resp=> navigate(`/competition/details/${resp.data.id}`))
        .catch(e=>console.log(e)
        )
    }

    return (

        <Paper variant="createPaper">
            <h1>Create Competition</h1>
            <Box component="form" sx={{ display: "flex", flexDirection: "column", gap: 2, width: {xs:"100%"} }}onSubmit={handlesubmit}>

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
                    onChange={(e) => setdescription(e.target.value)}
                />
                <RadioGroup row value={category} onChange={(e)=>setCategory(e.target.value)}>
                    <FormControlLabel label="Movie" value="movie" control={<Radio/>}/>
                    <FormControlLabel label="Music" value="Music" control={<Radio/>}/>
                </RadioGroup>
                <Button sx={{ width: "50%", display: "flex", flexDirection: "column", }} variant="outlined" type="submit">Create</Button>
            </Box>


        </Paper >
    )
}