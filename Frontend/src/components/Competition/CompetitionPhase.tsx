import { Chip } from "@mui/material";


export default function CompetitionPhase({ phase }: { phase: string}) {

    {
        return (
            <>
                <Chip variant="outlined" color="info" label={phase}></Chip>

                
            </>
        )

    }


}