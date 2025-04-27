import { Button, Card, CircularProgress, Grid as Grid2, Paper } from "@mui/material";
import { GroupDetailsInterface } from "./GroupInterface";
import UserButton from "../User/UserButton";
import KeyboardReturnIcon from '@mui/icons-material/KeyboardReturn';
import { useNavigate } from "react-router-dom";


export const GroupDetailsDetails = ({ groupDetails }: { groupDetails: GroupDetailsInterface | undefined }) => {
    const navigate = useNavigate();

    const returnToCompetition = () => {
        if (!groupDetails) return;
        navigate(`/competition/details/${groupDetails.competitionId}`);
    }
    return (
        <>
            <Paper variant="detailsPaper"  >

                {!groupDetails ? (<>
                    <CircularProgress />
                </>) : (<>
                    <Grid2 container spacing={2} sx={{ flexGrow: 1, display: "flex", flexDirection: "row", alignContent: "center" }}>

                        <Grid2 size={{ xs: 6, md: 3 }} >
                            <Button onClick={returnToCompetition}>
                                <KeyboardReturnIcon />
                            </Button>
                        </Grid2>
                        <Grid2 size={{ xs: 6, md: 9 }} />

                        <Grid2 size={{ sm: 12, md: 4 }} />
                        <Grid2 size={{ sm: 12, md: 4 }}>
                            <UserButton username={groupDetails.ownerName} />
                            <h1 style={{ margin: 0 }}> csapata</h1>
                        </Grid2>
                        <Grid2 size={{ sm: 12, md: 4 }} />

                        <Grid2 size={{ sm: 12, md: 4 }} />
                        <Grid2 size={{ sm: 12, md: 4 }}>
                            <h2 style={{ margin: 0 }}>{groupDetails.competitionTitle} <br /> versenyre</h2>
                        </Grid2>
                        <Grid2 size={{ sm: 12, md: 4 }} />


                        <Grid2 size={{ xs: 12, md: 12 }} sx={{ flexGrow: 1, display: "flex", flexDirection: "row", alignContent: "flex-start" }} >
                            <Card variant="outlined" sx={{ flexGrow: 1, flexDirection: "column" }}>
                                <p style={{ padding: "0 10px", lineBreak: "auto", textAlign: "left", whiteSpace: "pre-line" }}>{groupDetails.description}</p>
                            </Card>
                        </Grid2>

                    </Grid2>
                </>)}

            </Paper>
        </>
    );
};

