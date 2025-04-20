import { Box, Button, Grid2, Paper, TextField } from "@mui/material";
import { FormEvent, useState } from "react";
import { useAuth } from "../Utils/useAuth";
import { useNavigate } from "react-router-dom";

export default function Login() {
    const [email, setEmail] = useState("")
    const [password, setPassword] = useState("")
    const navigate = useNavigate()
    const login = useAuth()
    const handlesubmit = async (event: FormEvent) => {
        event.preventDefault()
        login.login(email, password)
        navigate("/home")
    }

    return (

        <Grid2 container justifyContent={"center"} >
            <Paper variant="userPaper">

                <h1>Bejelentkezés</h1>
                <Box component="form" sx={{ display: "flex", flexDirection: "column", gap: 2, minWidth: 300 }} onSubmit={handlesubmit}>
                    <Grid2 size={12}>

                        <TextField
                            className="outlined-input"
                            label="e-mail cím"
                            type="email"
                            sx={{ width: "100%" }}
                            autoComplete="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        />
                    </Grid2>
                    <Grid2 size={12}>
                        <TextField
                            className="outlined-password-input"
                            label="jelszó"
                            type="password"
                            sx={{ width: "100%" }}
                            autoComplete="current-password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                        />
                    </Grid2>
                    <Grid2 size={12}>
                        <Button variant="outlined" type="submit">Bejelentkezés</Button>
                    </Grid2>

                </Box>
            </Paper >
        </Grid2>


    )
}