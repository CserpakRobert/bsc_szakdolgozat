import { createTheme } from "@mui/material";
import { PaperProps } from "@mui/material";


declare module "@mui/material/Paper" {
    interface PaperPropsVariantOverrides {
        userPaper: true;
        createPaper: true
    }
}
export const darkTheme = createTheme({
    palette: {
        mode: "dark",
        primary: { main: "#bb86fc" },
        background: { default: "#121212", paper: "#1e1e1e" },
        text: { primary: "#ffffff" },
    },
    components: {
        MuiPaper: {
            variants: [
                {
                    props: {
                        variant: "userPaper" as PaperProps["variant"]
                    },
                    style: {

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
                        display:"flex",
                        flexDirection:"column",
                        "@media (min-width:600px)": { width: "90%", height: 500 },
                        "@media (min-width:960px)": { width: "100%", height: 450 },
                        
                        color: "#fafafa",
                        padding: "2rem",
                        borderRadius: "8px",
                        boxShadow: "0px 4px 6px rgba(0,0,0,0.1)",
                    },

                }
            ]

        }

    }
});