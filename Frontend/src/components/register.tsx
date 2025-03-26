import { Box, Button, Paper, TextField } from "@mui/material";
import { FormEvent, useState } from "react";
import { api } from "../apiAxios";
import { useNavigate } from "react-router-dom";

export default function Register() {
    const [email, setEmail] = useState("")
    const [userName, setUserName] = useState("")
    const [password, setPassword] = useState("")

    const navigate = useNavigate();
    const handlesubmit = async (event: FormEvent) => {
        event.preventDefault()
        await api.post("/user/register", { "email": email, "username": userName, "password": password })
            .then(() => navigate("/login"))
            .catch((error) => console.log(error))

    }

    return (

        <Paper variant="userPaper">
            <h1>Register</h1>
            <Box component="form" sx={{ display: "flex", flexDirection: "column", gap: 2, width: 300 }} onSubmit={handlesubmit}>
                <TextField
                    className="outlined-input"
                    label="Email"
                    type="email"
                    autoComplete="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
                <TextField
                    className="outlined-input"
                    label="UserName"
                    type="text"
                    autoComplete="username"
                    value={userName}
                    onChange={(e) => setUserName(e.target.value)}
                />
                <TextField
                    id="outlined-password-input"
                    label="Password"
                    type="password"
                    autoComplete="current-password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
                <Button sx={{ width: "50%", display: "flex", flexDirection: "column", }} variant="outlined" type="submit">Register</Button>
            </Box>


        </Paper >
    )
}