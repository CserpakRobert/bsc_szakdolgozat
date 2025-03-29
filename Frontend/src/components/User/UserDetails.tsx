import { AppBar, Box, Button, Grid2, Paper, Stack, Toolbar, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { api } from "../../globalHelpers/apiAxios";
import { SubmissionSmallInterface } from "../Submission/SubmissionInterface";

interface userDetails {
    id: number;
    userName: string;
}
export default function UserDetails() {
    const { name } = useParams()

    const [user, setUser] = useState<userDetails | undefined>(undefined)

    const [currentList, setCurrentList] = useState<"none"|"createdSub"|"participatedSub"|"createdComp"|"allSubs">("none")

    const [submissions, setSubmissions] = useState<[SubmissionSmallInterface]>()

    const handleListChange = (list:("none"|"createdSub"|"participatedSub"|"createdComp"|"allSubs"))=>{
        if (list == currentList) return;
        setCurrentList(list);
        switch (list) {
            case "none":
                break;
            case "allSubs":
                api.get()

                break;
        
            default:
                break;
        }

    }

    useEffect(() => {
        api.get(`/user/${name}`).then(resp => setUser(resp.data)).catch()
    }
        , [name])
    return (
        <>
            <Box>
                <h1>{user?.userName}</h1>
                <h1>{user?.id}</h1>
                <Stack direction="row" spacing={10}>
                        <Button variant="outlined" onClick={()=>handleListChange("allSubs")}>Created submissions</Button>
                        <h1>asd</h1>
                        <h1>asd</h1>
                </Stack>

                {currentList=="allSubs"?(<>
                    
                </>):(<></>)}
            </Box>
            


        </>

    )
}