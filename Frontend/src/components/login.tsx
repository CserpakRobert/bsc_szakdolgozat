import { Box, Button, Paper, TextField } from "@mui/material";
import { FormEvent, useState } from "react";
import { useAuth } from "../globalHelpers/useAuth";

export default function Login() {
    const [email, setEmail] = useState("")
    const [password, setPassword] = useState("")

    const login = useAuth()
    const handlesubmit = async (event: FormEvent) => {
        event.preventDefault()
         login.login(email, password)
    }

    return (

        <Paper variant="userPaper">
            <h1>Login</h1>
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
                    className="outlined-password-input"
                    label="Password"
                    type="password"
                    autoComplete="current-password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
                <Button sx={{ width: "50%", display: "flex", flexDirection: "column", }} variant="outlined" type="submit">Login</Button>
            </Box>


        </Paper >
    )
}