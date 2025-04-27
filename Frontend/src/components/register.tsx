import { Box, Button, Grid as Grid2, Paper, TextField } from "@mui/material";
import { FormEvent, useState } from "react";
import { api } from "../Utils/apiAxios";
import { useNavigate } from "react-router-dom";
import { useError } from "../Utils/useError";

export default function Register() {
    const [email, setEmail] = useState("")
    const [userName, setUserName] = useState("")
    const [password, setPassword] = useState("")
    const {setErrorMsg} = useError()

    const navigate = useNavigate();
    const handlesubmit = async (event: FormEvent) => {
        event.preventDefault()
        if (email == ""){
            setErrorMsg("Az email cím nem lehet üres")
            return
        }
        if (userName == ""){
            setErrorMsg("A felhasználó név nem lehet üres")
            return
        }
        await api.post("/user/register", { "email": email, "username": userName, "password": password })
            .then(() => navigate("/login"))
            .catch((error) => console.log(error))

    }

    return (

        <Grid2 container justifyContent={"center"} >
            <Paper variant="userPaper">

                <h1>Regisztráció</h1>

                <Box component="form" sx={{ display: "flex", flexDirection: "column", gap: 2, minWidth: 300 }} onSubmit={handlesubmit}>
                    <Grid2 size={12}>

                        <TextField
                            className="outlined-input"
                            label="e-mail cím"
                            type="email"
                            autoComplete="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        />
                    </Grid2>
                    <Grid2 size={12}>
                        <TextField
                            className="outlined-input"
                            label="felhasználónév"
                            type="text"
                            autoComplete="username"
                            value={userName}
                            onChange={(e) => setUserName(e.target.value)}
                        />
                    </Grid2>
                    <Grid2 size={12}>
                        <TextField
                            id="outlined-password-input"
                            label="jelszó"
                            type="password"
                            autoComplete="current-password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                        />
                    </Grid2>
                    <Grid2 size={12}>
                        <Button variant="outlined" type="submit">Regisztráció</Button>
                    </Grid2>

                </Box>
            </Paper >

        </Grid2>

    )
}