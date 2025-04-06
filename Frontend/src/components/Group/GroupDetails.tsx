import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { GroupDetailsInterface } from './GroupInterface';
import { api } from '../../globalHelpers/apiAxios';
import { Box, Button, Card, CircularProgress, Grid2, IconButton, List, ListItem, Paper, TextField } from '@mui/material';
import { useAuth } from '../../globalHelpers/useAuth';
import UserButton from '../User/UserButton';
import DeleteIcon from '@mui/icons-material/Delete';
import CheckIcon from '@mui/icons-material/Check';

const GroupDetails: React.FC = () => {
    const { id } = useParams();
    const [groupDetails, setGroupDetails] = useState<GroupDetailsInterface | undefined>(undefined);
    const [inviteUser, setInviteUser] = useState<string>("")
    const [invitedUsers, setInvitedUsers] = useState<string[]>([])
    const { userName } = useAuth()
    // const navigate = useNavigate()

    // not owner/participant
    const [hasjoinRequest, setHasJoinRequest] = useState(false)
    const [joinDescription, setjoinDescription] = useState("")
    const [requestUsers, setRequestUsers] = useState<string[]>([])


    useEffect(() => {
        api.get(`/group/${id}`).then((response) => {
            setGroupDetails(response.data);
        })

        api.get(`/group/hasJoin/${id}`).then((response) => { setHasJoinRequest(response.data) })
    }, [id]);

    useEffect(() => {
        api.get(`/group/invites/${id}`).then((response) => setInvitedUsers(response.data))
        api.get(`/group/requests/${id}`).then((response) => setRequestUsers(response.data))
    }, [groupDetails, id, userName])

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
        api.post(`/group/requests/accept/${id}`, { userName: name }).then(() => {
            setRequestUsers(requestUsers.filter((user) => user !== name))
            api.get(`/group/${id}`).then((response) => {
                setGroupDetails(response.data);
            })
        })
    }
    function acceptRequest(name: string): void {
        api.post(`/group/requests/accept/${id}`, { userName: name }).then(() => {
            setRequestUsers(requestUsers.filter((user) => user !== name))

        })
    }

    return (
        <>

            <Grid2 container >
                <Grid2 size={{ xs: 0, md: 0.5 }} />
                <Grid2 container size={{ xs: 12, md: 7 }}>
                    <Paper variant="detailsPaper" sx={{ paddingBottom: 0 }} >

                        {!groupDetails ? (<>
                            <CircularProgress />
                        </>) : (<>
                            <Grid2 container spacing={2} size={8} sx={{ flexGrow: 1, display: "flex", flexDirection: "row", alignContent: "center" }}>

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

                </Grid2>
                <Grid2 size={{ xs: 0, md: 0.1 }} />

                <Grid2 size={{ xs: 0, md: 0 }} />

                <Grid2 container size={{ xs: 4, sm: 4, md: 4, lg: 4 }} direction={"row"}>
                    <Paper variant="detailsPaper" sx={{ paddingBottom: 0 }} >
                        <Box>

                            {!groupDetails ? (<>
                                <CircularProgress />
                            </>) : (<>

                                <Grid2 size={{ sm: 12, md: 12 }} container>
                                    <Grid2 size={{ sm: 0, md: 1 }} />
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

        </>
    );
};

export default GroupDetails;