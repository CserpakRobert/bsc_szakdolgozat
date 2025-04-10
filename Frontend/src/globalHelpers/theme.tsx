import { CardProps, createTheme, PaperProps } from "@mui/material";


declare module "@mui/material/Paper" {
    interface PaperPropsVariantOverrides {
        userPaper: true;
        createPaper: true;
        detailsPaper: true;
    }
}
declare module "@mui/material/Card" {
    interface CardPropsVariantOverrides {
        detailsCard: true;
    }
}
export const darkTheme = createTheme({
    palette: {
        mode: "dark",
        primary: { main: "#bb86fc" },
        background: { default: "#121212", paper: "#1e1e1e" },
        text: { primary: "#ffffff" },
        success: { main: "#3af259" },
        warning: { main: "#2d78fa" },
        secondary: { main: "#f2be2e" }

    },
    typography: {
        fontSize: 15,
    },
    components: {
        MuiPaper: {
            variants: [
                {
                    props: {
                        variant: "userPaper" as PaperProps["variant"]
                    },
                    style: {
                        display: "flex",
                        flexDirection: "column",
                        flexGrow: 1,
                        alignContent: "center",
                        alignItems: "center",
                        margin: "auto",
                        "@media (min-width:600px)": { width: "70%", height: "100%" },
                        "@media (min-width:960px)": { width: "30%", height: "100%" },
                        minWidth: "20vw",
                        marginTop: "5rem",
                        color: "#fafafa",
                        padding: "2rem",
                        borderRadius: "8px",
                        boxShadow: "0px 4px 6px rgba(0,0,0,0.1)",
                    },
                },
                {
                    props: {
                        variant: "createPaper" as PaperProps["variant"]
                    },
                    style: {
                        display: "flex",
                        flexDirection: "column",
                        flexGrow: 1,
                        alignContent: "center",
                        margin: "auto",
                        "@media (min-width:600px)": { width: "70%", height: "100%" },
                        "@media (min-width:960px)": { width: "50%", height: "100%" },
                        minWidth: "20vw",
                        marginTop: "5rem",
                        color: "#fafafa",
                        padding: "2rem",
                        borderRadius: "8px",
                        boxShadow: "0px 4px 6px rgba(0,0,0,0.1)",
                    },


                },
                {
                    props: {
                        variant: "detailsPaper" as PaperProps["variant"]
                    },
                    style: {
                        display: "flex",
                        flexDirection: "column",
                        flexGrow: 1,
                        alignContent: "center",
                        margin: "auto",
                        "@media (min-width:600px)": { width: "90%", height: "100%" },
                        "@media (min-width:960px)": { width: "50%", height: "100%" },
                        minWidth: "20vw",
                        marginTop: "5rem",
                        color: "#fafafa",
                        padding: "2rem",
                        borderRadius: "8px",
                        boxShadow: "0px 4px 6px rgba(0,0,0,0.1)",
                    },


                }
            ]

        },
        MuiCard: {
            variants: [
                {
                    props: {
                        variant: "detailsCard" as CardProps["variant"]
                    },
                    style: {

                        color: "#fafafa",
                        padding: "2rem",
                        borderRadius: "8px",
                        border: "px",
                        borderColor: "fafafa",
                        boxShadow: "0px 4px 6px rgba(0,0,0,0.1)",


                    },
                }]

        }
    }
});