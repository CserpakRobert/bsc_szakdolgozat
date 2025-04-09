import { Box, Button, FormControlLabel, Grid2, Paper, Radio, RadioGroup, TextField } from '@mui/material';
import { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import KeyboardReturnIcon from '@mui/icons-material/KeyboardReturn';
import { api } from '../../globalHelpers/apiAxios';



const CreateGroup = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [description, setdescription] = useState<string>("")
    const [open, setOpen] = useState<boolean>(false)

    const returnToCompetition = () => {
        navigate(`/competition/details/${id}`);
    }
    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setOpen(event.target.value === "true");
    }

    const handleJoin = () => {
        api.post(`/group/create/${id}`, { description, isClosed: !open }).then((response) => navigate(`/group/details/${response.data}`))

    }

    return (
        <>
            <Box>

                <Paper variant="createPaper" >
                    <Grid2 container spacing={2} >
                        <Grid2 size={{ xs: 6, md: 3 }} >
                            <Button onClick={returnToCompetition}>
                                <KeyboardReturnIcon />
                            </Button>
                        </Grid2>
                        <Grid2 size={{ xs: 6, md: 9 }} />
                        <Grid2 size={{ xs: 12 }} >
                            <h1>Jelentkezés készítése</h1>
                        </Grid2>

                        <Grid2 size={{ xs: 0, md: 1 }} />
                        <Grid2 size={{ xs: 12, md: 10 }} >
                            <TextField
                                sx={{ height: "100%", width: "100%" }}
                                className="outlined-input"
                                label="leírás"
                                type="text"
                                value={description}
                                multiline
                                rows={5}
                                onChange={(e) => setdescription(e.target.value)}
                            />
                        </Grid2>
                        <Grid2 size={{ xs: 0, md: 1 }} />

                        <Grid2 size={{ xs: 0, md: 1 }} />

                        <Grid2 size={{ xs: 12, md: 7 }} >
                            <RadioGroup
                                row
                                name="controlled-radio-buttons-group"
                                value={open}
                                onChange={handleChange}
                            >
                                <FormControlLabel value="false" control={<Radio />} label="Egyedül / Zárt csapattal jelentkezem" />
                                <FormControlLabel value="true" control={<Radio />} label="Nyitott csapatban jelentkezem" />
                            </RadioGroup>
                        </Grid2>
                        <Grid2 size={{ xs: 0, md: 1 }} />
                        <Grid2 size={{ xs: 0, md: 1 }} >
                            <Button variant="outlined" onClick={handleJoin}>Jelentkezés</Button>
                        </Grid2>


                    </Grid2>
                </Paper>
            </Box>
        </>
    );
};



export default CreateGroup;