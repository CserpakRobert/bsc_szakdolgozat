import { Box, Button, FormControlLabel, Grid2, Paper, Radio, RadioGroup, TextField } from "@mui/material";
import { FormEvent, useState } from "react";
import { api } from "../../globalHelpers/apiAxios";
import { useNavigate } from "react-router-dom";
import { DatePicker } from "@mui/x-date-pickers";
import dayjs, { Dayjs } from "dayjs";

export default function CreateCompetition() {
    const [title, setTitle] = useState("")
    const [description, setdescription] = useState("")
    const [category, setCategory] = useState("movie")
    const [preparationLastDate, setpreparationLastDate] = useState<Dayjs>(dayjs(Date.now()))
    const [submissionLastDate, setsubmissionLastDate] = useState<Dayjs>(dayjs(Date.now()).add(1, "day"))
    const [votingLastDate, setvotingLastDate] = useState<Dayjs>(dayjs(Date.now()).add(2, "day"))



    const navigate = useNavigate()

    const handlesubmit = async (event: FormEvent) => {
        event.preventDefault();
        return api.post("/competition/create", {
            title,
            description,
            category,
            submissionStartDate: preparationLastDate.format("YYYY-MM-DD"),
            votingStartDate:submissionLastDate.format("YYYY-MM-DD"),
            finishDate:votingLastDate.format("YYYY-MM-DD")
        })
            .then(resp => navigate(`/competition/details/${resp.data}`))
            .catch(e => console.log(e)
            )
    }

    return (

        <Box>


            <Paper variant="createPaper" >

                <Grid2 container spacing={2} width={{ sx: "80%", md: "85%" }} sx={{ alignSelf: "center" }} onSubmit={handlesubmit} >
                    <Grid2 size={{ sm: 12, md: 12 }}>
                        <h1>Verseny készítése</h1>
                    </Grid2>
                    <Grid2 size={{ sm: 12, md: 12 }}>
                        <TextField
                            sx={{ height: "100%", width: "100%" }}
                            className="outlined-input"
                            label="Verseny Címe"
                            type="text"
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                        />
                    </Grid2>
                    <Grid2 size={{ sm: 12, md: 12 }}>
                        <TextField
                            sx={{ height: "100%", width: "100%" }}
                            className="outlined-input"
                            label="Verseny leírása"
                            type="text"
                            value={description}
                            multiline
                            rows={5}
                            onChange={(e) => setdescription(e.target.value)}
                        />
                    </Grid2>



                    <Grid2 size={{ sm: 12, md: 4 }}>
                        <DatePicker
                            label="Jelentkezés utolsó napja"
                            minDate={dayjs(Date.now())}
                            value={preparationLastDate}
                            onChange={(newValue) => setpreparationLastDate(newValue ?? dayjs(Date.now()))}
                        />
                    </Grid2>
                    <Grid2 size={{ sm: 12, md: 4 }}>
                        <DatePicker
                            label="Beadás utolsó napja"
                            minDate={preparationLastDate.add(1, "day")}
                            value={submissionLastDate}
                            onChange={(newValue) => setsubmissionLastDate(newValue ?? dayjs(Date.now()))}
                        />
                    </Grid2>
                    <Grid2 size={{ sm: 12, md: 4 }}>
                        <DatePicker
                            label="Szavazás utolsó napja"
                            minDate={submissionLastDate.add(1, "day")}
                            value={votingLastDate}
                            onChange={(newValue) => setvotingLastDate(newValue ?? dayjs(Date.now()))}
                        />
                    </Grid2>


                    <Grid2 size={{ sm: 12, md: 12 }} columnSpacing={0} justifyContent={"center"} container>
                        <RadioGroup row value={category} onChange={(e) => setCategory(e.target.value)}>
                            <Grid2 size={{ sm: 3, md: 3 }} >
                                <FormControlLabel label="Film" value="movie" control={<Radio />} />
                            </Grid2>
                            <Grid2 size={{ sm: 0, md: 1.5 }} />
                            <Grid2 size={{ sm: 3, md: 3 }} >
                                <FormControlLabel label="Zene" value="music" control={<Radio />} />
                            </Grid2>
                            <Grid2 size={{ sm: 0, md: 1.5 }} />
                            <Grid2 size={{ sm: 3, md: 3 }} >
                                <FormControlLabel label="Történet" value="shortstory" control={<Radio />} />
                            </Grid2>
                        </RadioGroup>
                    </Grid2>

                    <Grid2 size={{ sm: 12, md: 12 }}>
                        <Button onClick={handlesubmit} variant="outlined" sx={{ width: "40%", height: "100%" }} >Verseny Készítése</Button>
                    </Grid2>

                </Grid2>
            </Paper>
        </Box>
    )

    {/* <Paper variant="createPaper">
            <h1>Create Competition</h1>
            <Box component="form" sx={{ display: "flex", flexDirection: "column", gap: 2, width: { xs: "100%" } }} onSubmit={handlesubmit}>

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
                <RadioGroup row value={category} onChange={(e) => setCategory(e.target.value)}>
                    <FormControlLabel label="Movie" value="movie" control={<Radio />} />
                    <FormControlLabel label="Music" value="music" control={<Radio />} />
                    <FormControlLabel label="ShortStory" value="shortstory" control={<Radio />} />
                </RadioGroup>
                <Button sx={{ width: "50%", display: "flex", flexDirection: "column", }} variant="outlined" type="submit">Create</Button>
            </Box>


        </Paper > */}

}