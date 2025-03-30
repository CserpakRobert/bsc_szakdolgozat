
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import { NavLink } from 'react-router-dom';
import { useAuth } from '../globalHelpers/useAuth';

export default function MenuBar() {

    const { isLoggedIn, userName, logout } = useAuth()

    return (
        <Box sx={{ flexGrow: 1 }}>
            <AppBar position="fixed">
                <Toolbar>
                    <IconButton
                        size="large"
                        edge="start"
                        color="inherit"
                        aria-label="menu"
                        sx={{ mr: 2 }}
                    >
                    </IconButton>

                    <Typography variant="h6" component="div" sx={{ flexGrow: 1 }} >
                        {isLoggedIn?(<>
                        <NavLink to={`/competition/create`} className={({ isActive, isPending }) =>
                            isPending ? "pending" : isActive ? "active" : "inactive"
                        }>
                            <Button color='inherit'>Create</Button>
                        </NavLink>
                        <NavLink to={`/competition/list`} className={({ isActive, isPending }) =>
                            isPending ? "pending" : isActive ? "active" : "inactive"
                        }>
                            <Button color='inherit'>Competitions</Button>
                        </NavLink>
                        
                        
                        </>):(<></>)}
                    </Typography>
                    <Typography variant="h6" component="div" sx={{ flexGrow: 1 }} >
                    {isLoggedIn ?
                        (<>
                            <NavLink to="/login" className={({ isActive, isPending }) =>
                                isPending ? "pending" : isActive ? "active" : "inactive"
                            }>
                                <Button color='inherit' onClick={logout}>Logout</Button>
                            </NavLink>
                            <NavLink to={`/user/profile/${userName}`} className={({ isActive, isPending }) =>
                                isPending ? "pending" : isActive ? "active" : "inactive"
                            }>
                                <Button color='inherit'>Profile</Button>
                            </NavLink>
                        </>
                        ) : (
                            <>
                                <NavLink to="/register" className={({ isActive, isPending }) =>
                                    isPending ? "pending" : isActive ? "active" : "inactive"
                                }>
                                    <Button color='inherit'>Register</Button>
                                </NavLink>
                                <NavLink to="/login" className={({ isActive, isPending }) =>
                                    isPending ? "pending" : isActive ? "active" : "inactive"
                                }>
                                    <Button color='inherit'>Login</Button>
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