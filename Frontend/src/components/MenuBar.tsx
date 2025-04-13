import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import { NavLink } from 'react-router-dom';
import { useAuth } from '../globalHelpers/useAuth';

export default function MenuBar() {

    const { isLoggedIn, userName, logout } = useAuth()

    return (
        <Box sx={{ flexGrow: 1 }}>
            <AppBar position="fixed" sx={{}}>
                <Toolbar>
                    <Typography variant="h6" component="div"  >
                        <NavLink to={`/home`} className={({ isActive, isPending }) =>
                            isPending ? "pending" : isActive ? "active" : "inactive"
                        }>
                            <Button color='inherit'>Főoldal</Button>
                        </NavLink>
                        {isLoggedIn ? (<>
                            <NavLink to={`/competition/list`} className={({ isActive, isPending }) =>
                                isPending ? "pending" : isActive ? "active" : "inactive"
                            }>
                                <Button color='inherit'>Verseynek Listája</Button>
                            </NavLink>
                            <NavLink to={`/competition/create`} className={({ isActive, isPending }) =>
                                isPending ? "pending" : isActive ? "active" : "inactive"
                            }>
                                <Button color='inherit'>Verseny készítése</Button>
                            </NavLink>



                        </>) : (<></>)}
                    </Typography>
                    <Typography variant="h6" component="div" sx={{ marginLeft: "auto" }} >
                        {isLoggedIn ?
                            (<>
                                <NavLink to={`/user/profile/${userName}`} className={({ isActive, isPending }) =>
                                    isPending ? "pending" : isActive ? "active" : "inactive"
                                }>
                                    <Button color='inherit'>Profil</Button>
                                </NavLink>
                                <NavLink to="/login" className={({ isActive, isPending }) =>
                                    isPending ? "pending" : isActive ? "active" : "inactive"
                                }>
                                    <Button color='inherit' onClick={logout}>Kijelentkezés</Button>
                                </NavLink>
                            </>
                            ) : (
                                <>
                                    <NavLink to="/register" className={({ isActive, isPending }) =>
                                        isPending ? "pending" : isActive ? "active" : "inactive"
                                    }>
                                        <Button color='inherit'>Regisztráció</Button>
                                    </NavLink>
                                    <NavLink to="/login" className={({ isActive, isPending }) =>
                                        isPending ? "pending" : isActive ? "active" : "inactive"
                                    }>
                                        <Button color='inherit'>Bejelentkezés</Button>
                                    </NavLink>
                                </>
                            )
                        }
                        {/* <Button color="inherit" >Register</Button> */}
                    </Typography>
                </Toolbar>
            </AppBar>
        </Box>
    );
}