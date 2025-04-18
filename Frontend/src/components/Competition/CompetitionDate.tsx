import { Chip, } from "@mui/material";

export default function CompetitionDate({ phase, date }: { phase: string, date: string }) {
    const phaseToColor = (phase: string):("warning"| "info" | "error"|"secondary") =>  {
        switch (phase) {
            case "finished":
                return "error"
            case "preparation":
                return "info"
            case "voting":
                return "secondary"
            default:
                return "warning"
        }
    }
    {
        return (
            <>

                {date ? (<><Chip variant="outlined" color={phaseToColor(phase)} label={"eddig: " + date}></Chip></>) : (<></>)}

            </>
        )

    }


}