import { useError } from "../globalHelpers/useError"


export default function ErrorComponent() {
        const {errorMsg} = useError()
        console.log(errorMsg);
        
        return (<>
            <h1>{errorMsg}</h1>
        </>)
}
