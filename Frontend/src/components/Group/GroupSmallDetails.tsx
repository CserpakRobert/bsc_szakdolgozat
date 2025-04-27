import { Button, Card, Grid as Grid2, } from "@mui/material"
import { GroupSmallDetailsInterface } from "./GroupInterface"
import UserButton from "../User/UserButton"
import { useNavigate } from "react-router-dom"





export const GroupSmallDetails = ({ group }: { group: GroupSmallDetailsInterface }) => {
    const navigate = useNavigate()
    const handleJoin = () => {
        navigate(`/group/details/${group.groupId}`)
    }
    return (
        <>
            <Card variant="outlined" sx={{ padding: 2 }}>
                <Grid2 container spacing={2} >
                    <Grid2 size={{ xs: 12, md: 12 }}>
                        <UserButton username={group.ownerName} />
                    </Grid2>
                    <Grid2 size={{ xs: 12, md: 12 }}>
                        <h3>Tagok száma:{group.participateCount}</h3>
                    </Grid2>

                    <Grid2 size={{ xs: 12, md: 12 }}>
                        <Button variant="outlined" color="primary" onClick={handleJoin}>
                            Megtekintés
                        </Button>
                    </Grid2>
                </Grid2>

            </Card>
        </>
    )
}