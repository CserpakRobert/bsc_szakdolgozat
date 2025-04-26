import { Box, Button, Grid2, Paper } from "@mui/material"
import { useAuth } from "../Utils/useAuth";
import { useNavigate } from "react-router-dom";

export default function HomeComponent() {
    const {isLoggedIn} = useAuth()
    const navigate = useNavigate()
    return (
        <>
            <Paper variant="detailsPaper">
                <Box>

                    <Grid2 container spacing={2}>
                        <Grid2 size={{ sm: 12, lg: 12 }}>
                            <h1>Online Verseny Alkalmazás</h1>
                        </Grid2>
                        <Grid2 size={{ sm: 12, lg: 12 }}>
                            <p style={{ fontSize: 20, textAlign: "justify" }}>
                                Az Online Verseny Alkalmazás egy interneten használható alkalmazás,
                                amely versenyek készítését és a versenyeken való részvételt segíti.
                                A versenyek 3 különböző kategóriában elérhetőek, amelyek Film, Zene és Történetek.
                                <br />
                                <br />
                                A verseny szervezésének megkönnyebítésére a szervezőknek a verseny létrehozásán kívül nincs teendőjük a verseny lebonyolítása során.
                                Az oldalon készült versenyek nyilvánosak, ezért az elbírálás is az oldal felhasználói segítségével történik szavazatok leadásával.
                                <br />
                                <br />
                                Az oldal lehetővé teszi az elkészített versenyekre való jelentkezést csapatok létrehozásával, illetve ezekre a csapatokra való jelentkezéssel.
                                A csoport készítőjének lehetősége van további csapattagok meghívására, illetve ha az oldal felhasználóiból szeretne csapattagokat keresni,
                                akkor lehetősége van jelentkezések fogadására és annak elbírálására is.
                            </p>
                        </Grid2>
                        {!isLoggedIn &&(<>
                        <Grid2 size={{ sm: 12, lg: 6 }}>
                            <Button variant="outlined" onClick={() => {navigate("/login")}}>Bejelentkezés</Button>
                        </Grid2>
                        <Grid2 size={{ sm: 12, lg: 6 }}>
                            <Button variant="outlined" onClick={() => {navigate("/register")}}>Regisztráció</Button>
                        </Grid2>
                        </>)}
                        {isLoggedIn &&(<>
                        <Grid2 size={{ sm: 12, lg: 6 }}>
                            <Button variant="outlined" onClick={() => {navigate("/competition/create")}}>Verseny készítése</Button>
                        </Grid2>
                        <Grid2 size={{ sm: 12, lg: 6 }}>
                            <Button variant="outlined" onClick={() => {navigate("/competition/list")}}>Versenyek megtekintése</Button>
                        </Grid2>
                        </>)}
                    </Grid2>
                </Box>

            </Paper>
        </>
    );
};

