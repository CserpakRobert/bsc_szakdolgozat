import { Chip } from "@mui/material";


export default function CompetitionPhase({ phase }: { phase: string}) {
    const phaseToColor = (phase: string):("warning"| "info" | "error") =>  {
        switch (phase) {
            case "finished":
                return "error"
            case "preparation":
                return "info"
            case "voting":
                return "warning"
            default:
                return "warning"
        }
    }
    const phaseToHUN = (phase:string):string =>{

        switch (phase) {
            case "finished":
                return "befejezett"
            case "preparation":
                return "jelentkezés"
            case "voting":
                return "szavazás"
            default:
                return "warning"
        }
    }
    {
        return (
            <>

                <Chip variant="outlined" color={phaseToColor(phase)} label={phaseToHUN(phase)}></Chip>

            </>
        )

    }


}