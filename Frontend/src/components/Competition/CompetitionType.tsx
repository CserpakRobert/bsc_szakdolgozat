import { Chip } from "@mui/material";


export default function CompetitionType({ type }: { type: string }) {

    {
        switch (type) {
            case "movie":
                return (
                    <>
                        <Chip variant="outlined" color="success" label={"rövid film"}></Chip>
                    </>
                )
            case "music":
                return (
                    <>
                        <Chip variant="outlined" color="secondary" label={"zene"}></Chip>
                    </>
                )
            case "shortstory":
                return (
                    <>
                        <Chip variant="outlined" color="warning" label={"történet"}></Chip>
                    </>
                )
            default:
                break;
        }
    }


}