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
                return "Befejezett"
            case "preparation":
                return "Jelentkezés"
            case "voting":
                return "Szavazás"
            default:
                return "Beadás"
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