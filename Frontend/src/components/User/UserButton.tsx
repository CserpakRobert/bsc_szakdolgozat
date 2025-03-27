import { Button, Card } from "@mui/material";
import AccountBoxIcon from '@mui/icons-material/AccountBox';
import { useNavigate } from "react-router-dom";

export default function UserButton({ username }: { username: string }) {
    const navigate = useNavigate()
    const userProfile = (username: string) => {
        navigate(`/user/profile/${username}`)
    }

    return (
        <>
            <Card >
                <Button sx={{ display: "flex", flexDirection: "row", width: "100%" }} onClick={()=>userProfile(username)}>

                    <AccountBoxIcon sx={{ width: "10%", display: "" }} />
                    <span style={{ flexGrow: 1 }}>{username}</span>

                </Button>
            </Card>
        </>
    )

}