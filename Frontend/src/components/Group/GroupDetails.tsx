import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { GroupDetailsInterface } from './GroupInterface';
import { api } from '../../globalHelpers/apiAxios';
import { Box, Button, Card, CircularProgress, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, Grid2, IconButton, List, ListItem, Paper, TextField } from '@mui/material';
import { useAuth } from '../../globalHelpers/useAuth';
import UserButton from '../User/UserButton';
import DeleteIcon from '@mui/icons-material/Delete';
import CheckIcon from '@mui/icons-material/Check';
import { GroupDetailsDetails } from './GroupDetailsDetails';
import InfoIcon from '@mui/icons-material/Info';

const GroupDetails = () => {
    const { id } = useParams();
    const [groupDetails, setGroupDetails] = useState<GroupDetailsInterface | undefined>(undefined);
    const [inviteUser, setInviteUser] = useState<string>("")
    const [invitedUsers, setInvitedUsers] = useState<string[]>([])
    const { userName } = useAuth()

    const [isInvited, setIsInvited] = useState(false)

    // const navigate = useNavigate()

    // not owner/participant
    const [hasjoinRequest, setHasJoinRequest] = useState(false)
    const [joinDescription, setjoinDescription] = useState("")
    const [requestUsers, setRequestUsers] = useState<string[]>([])

    const [requestDescription, setRequestDescription] = useState<string>("")

    const [open, setOpen] = useState(false);

    useEffect(() => {
        if (requestDescription) {
            setOpen(true);
        }
    }, [requestDescription])

    const handleClose = () => {
        setRequestDescription("")
        setOpen(false);
    };

    useEffect(() => {
        api.get(`/group/${id}`).then((response) => {
            setGroupDetails(response.data);
        })

        api.get(`/group/hasJoin/${id}`).then((response) => { setHasJoinRequest(response.data) })

        api.get(`/group/isInvited/${id}`).then((response) => {
            setIsInvited(response.data)
        })

    }, [id]);

    useEffect(() => {
        api.get(`/group/invites/${id}`).then((response) => setInvitedUsers(response.data))
        api.get(`/group/requests/${id}`).then((response) => setRequestUsers(response.data))
    }, [groupDetails, id, userName])

    const getDescription = (name: string) => {
        api.post(`/group/requests/${id}`, { userName: name }).then((response) => setRequestDescription(response.data))
    }

    const invite = () => {
        api.post(`/group/invite/${id}`, { userName: inviteUser }).then(() => {
            setInvitedUsers([...invitedUsers, inviteUser])
            setInviteUser("")
        })
    }
    const join = () => {
        console.log(joinDescription);

        api.post(`/group/join/${id}`, { Description: joinDescription }).then(() => {
            setHasJoinRequest(true)
        })
    }

    function deleteInvite(name: string): void {
        api.post(`/group/invites/delete/${id}`, { userName: name }).then(() => {
            setInvitedUsers(invitedUsers.filter((user) => user !== name))
        })
    }

    function deleteRequest(name: string): void {
        api.post(`/group/requests/delete/${id}`, { userName: name }).then(() => {
            setRequestUsers(requestUsers.filter((user) => user !== name))
            api.get(`/group/${id}`).then((response) => {
                setGroupDetails(response.data);
            })
        })
    }
    function acceptRequest(name: string): void {
        api.post(`/group/requests/accept/${id}`, { userName: name }).then(() => {
            setRequestUsers([...requestUsers, name])
            api.get(`/group/${id}`).then((response) => {
                setGroupDetails(response.data);
            })
        })
    }

    function acceptInvite(): void {
        api.post(`/group/invites/accept/${id}`).then(() => {
            setIsInvited(false)
            api.get(`/group/${id}`).then((response) => {
                setGroupDetails(response.data);
            })
        })
    }

    function declineInvite(): void {
        api.post(`/group/invites/decline/${id}`).then(() => {
            setIsInvited(false)
        })
    }

    return (
        <>

            <Grid2 container >
                <Grid2 size={{ xs: 0, md: 0.5 }} />
                <Grid2 container size={{ xs: 12, md: 7 }}>
                    <GroupDetailsDetails groupDetails={groupDetails} />
                </Grid2>
                <Grid2 size={{ xs: 0, md: 0.1 }} />
                <Grid2 size={{ xs: 0, md: 0 }} />
                <Grid2 container size={{ xs: 12, sm: 12, md: 4, lg: 4 }} direction={"row"}>
                    <Paper variant="detailsPaper" sx={{ paddingBottom: 0 }} >
                        <Box>

                            {!groupDetails ? (<>
                                <CircularProgress />
                            </>) : (<>

                                <Grid2 size={{ sm: 12, md: 12 }} container>
                                    {/* <Grid2 size={{ sm: 0, md: 1 }} /> */}
                                    <Grid2 size={{ sm: 12, md: 11 }}>
                                        <Card variant='outlined' sx={{ marginBottom: 2 }}>
                                            Csapat tagjai:
                                            <List sx={{
                                                width: '100%', position: 'relative',
                                                overflow: 'auto',
                                                maxHeight: 100,
                                                minHeight: 30,
                                            }}>
                                                {groupDetails.participateNames.map((name) =>
                                                    <ListItem key={name}><UserButton username={name} /></ListItem>
                                                )}
                                            </List>
                                        </Card>
                                    </Grid2>
                                </Grid2>

                                {groupDetails.ownerName == userName ? (<>
                                    <Grid2 size={{ sm: 12, md: 12 }}>
                                        <Grid2 container size={{ sm: 12, md: 12 }} spacing={1}>

                                            <Grid2 size={{ sm: 12, md: 8 }}>
                                                <TextField
                                                    className="outlined-input"
                                                    label="felhasználó"
                                                    type="text"
                                                    value={inviteUser}
                                                    onChange={(e) => setInviteUser(e.target.value)}
                                                />
                                            </Grid2>
                                            <Grid2 size={{ sm: 12, md: 4 }} sx={{}}>
                                                <Button variant="outlined" onClick={invite}>Meghívás</Button>
                                            </Grid2>
                                        </Grid2>
                                    </Grid2>
                                    <Grid2 size={{ sm: 12, md: 12 }} container>
                                        <Grid2 size={{ sm: 0, md: 1 }} />
                                        <Grid2 size={{ sm: 12, md: 11 }}>
                                            <Card variant='outlined' sx={{ marginBottom: 2, marginTop: 2 }}>
                                                Meghívások:
                                                <List sx={{
                                                    width: '100%', position: 'relative',
                                                    overflow: 'auto',
                                                    maxHeight: 100,
                                                    minHeight: 30,
                                                }}>
                                                    {invitedUsers.map((name) =>
                                                        <ListItem key={name} secondaryAction={
                                                            <IconButton edge="start" aria-label="delete" onClick={() => deleteInvite(name)}>
                                                                <DeleteIcon />
                                                            </IconButton>
                                                        }>
                                                            <UserButton username={name} />
                                                        </ListItem>
                                                    )}
                                                </List>
                                            </Card>
                                        </Grid2>
                                    </Grid2>
                                    <Grid2 size={{ sm: 12, md: 12 }} container>
                                        <Grid2 size={{ sm: 0, md: 1 }} />
                                        <Grid2 size={{ sm: 12, md: 11 }}>
                                            <Card variant='outlined' sx={{ marginBottom: 2, marginTop: 2 }}>
                                                Jelentkezések:
                                                <List sx={{
                                                    width: '100%', position: 'relative',
                                                    overflow: 'auto',
                                                    maxHeight: 200,
                                                    minHeight: 30,
                                                }}>
                                                    {requestUsers.map((name) =>
                                                        <ListItem key={name} secondaryAction={
                                                            <>
                                                                <IconButton edge="start" aria-label="accept" onClick={() => getDescription(name)}>
                                                                    <InfoIcon />
                                                                </IconButton>
                                                                <IconButton edge="start" aria-label="accept" onClick={() => acceptRequest(name)}>
                                                                    <CheckIcon />
                                                                </IconButton>

                                                                <IconButton edge="start" aria-label="delete" onClick={() => deleteRequest(name)}>
                                                                    <DeleteIcon />
                                                                </IconButton>
                                                            </>
                                                        }>
                                                            <UserButton username={name} />
                                                        </ListItem>
                                                    )}
                                                </List>
                                            </Card>
                                        </Grid2>
                                    </Grid2>
                                    {/* Jelentkezések */}


                                </>) : (<>
                                    {!groupDetails.participateNames.includes(userName) && <>
                                        {/*JoinRequest*/}
                                        <Grid2 size={{ sm: 12, md: 12 }} container>
                                            <Card variant='outlined' sx={{ marginBottom: 2, marginTop: 2, paddingTop: 2 }}>
                                                <Grid2 container size={{ sm: 12, md: 12 }} spacing={1}>
                                                    {isInvited && (<>
                                                        <Grid2 size={{ sm: 12, md: 12 }}>
                                                            <h3>Erre a versenyre meghívtak</h3>
                                                        </Grid2>
                                                        <Grid2 size={{ sm: 12, md: 6 }}>
                                                            <Button variant='outlined' color='success' onClick={acceptInvite}>Elfogadás</Button>
                                                        </Grid2>
                                                        <Grid2 size={{ sm: 12, md: 6 }}>
                                                            <Button variant='outlined' color='error' onClick={declineInvite}>Elutasítás</Button>
                                                        </Grid2>
                                                    </>)}

                                                </Grid2>
                                            </Card>
                                        </Grid2>
                                    </>}
                                    {!groupDetails.participateNames.includes(userName) && <>
                                        {/*JoinRequest*/}
                                        <Grid2 size={{ sm: 12, md: 12 }} container>
                                            <Card variant='outlined' sx={{ marginBottom: 2, marginTop: 2, paddingTop: 2 }}>
                                                <Grid2 container size={{ sm: 12, md: 12 }} spacing={1}>
                                                    {hasjoinRequest ? (<>
                                                        <h2>A jelentkezésed le lett adva!</h2>
                                                    </>) : (<>

                                                        <Grid2 size={{ sm: 12, md: 12 }}>
                                                            <TextField
                                                                className="outlined-input"
                                                                label="jelentkezés"
                                                                helperText="Írd le röviden, hogy miért szeretnél csatlakozni a csapathoz!"
                                                                type="text"
                                                                value={joinDescription}
                                                                multiline={true}
                                                                rows={5}
                                                                onChange={(e) => setjoinDescription(e.target.value)}
                                                            />
                                                        </Grid2>
                                                        <Grid2 size={{ sm: 12, md: 12 }} sx={{}}>
                                                            <Button variant="outlined" onClick={join}>Jelentkezés</Button>
                                                        </Grid2>
                                                    </>)}

                                                </Grid2>
                                            </Card>
                                        </Grid2>
                                    </>}

                                </>)}</>)}
                        </Box>

                    </Paper>
                </Grid2>
            </Grid2 >
            <Dialog
                open={open}
                keepMounted
                onClose={handleClose}
            >
                {/*TODO*/}
                <DialogTitle>{"Jelentkezés leírása"}</DialogTitle>
                <DialogContent>
                    
                    <DialogContentText sx={{textWrap:""}} id="description">
                        {requestDescription}
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose}>Bezárás</Button>
                </DialogActions>
            </Dialog>

        </>
    );
};

export default GroupDetails;