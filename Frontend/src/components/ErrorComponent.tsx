import { Alert, IconButton, Snackbar, SnackbarCloseReason } from "@mui/material";
import { useError } from "../Utils/useError"
import { useEffect, useState } from "react";
import React from "react";
import CloseIcon from '@mui/icons-material/Close';


export default function ErrorComponent() {
    const { errorMsg, setErrorMsg } = useError()
    const [open, setOpen] = useState(false);

    const handleClose = (
        _event: React.SyntheticEvent | Event,
        reason?: SnackbarCloseReason,
    ) => {
        if (reason === 'clickaway') {
            return;
        }
        setOpen(false);
        setErrorMsg("");
    };

    useEffect(() => {
        if (errorMsg !== "") {
            setOpen(true)
        }
    }, [errorMsg, setErrorMsg])

    const action = (
        <React.Fragment>
            <IconButton
                size="small"
                aria-label="close"
                color="inherit"
                onClick={handleClose}
            >
                <CloseIcon fontSize="small" />
            </IconButton>
        </React.Fragment>
    );
    return (<>
        {errorMsg !== "" &&
            <Snackbar
                open={open}
                autoHideDuration={5000}
                onClose={handleClose}
                anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
                key={"bottom" + "center"}
            >
                <Alert
                    severity="error"
                    variant="filled"
                    action={action}
                >
                    {errorMsg}
                </Alert>
            </Snackbar>
        }
    </>)
}
