import React from "react";
import { createContext, useState, ReactNode } from "react";
import { setErrorHandler } from "./apiAxios";

interface ErrorContextType {
    errorMsg: string
    setErrorMsg: (errorMsg: string) => void
}

const ErrorContext = createContext<ErrorContextType | undefined>(undefined);

interface ErrorProviderProps {
    children: ReactNode;
}

export const ErrorProvider = ({ children }: ErrorProviderProps) => {
    const [errorMsg, setError] = useState("");
    const setErrorMsg = (msg:string) => setError(msg);

      React.useEffect(() => {
        setErrorHandler(setErrorMsg);
      });
    return (
        <ErrorContext.Provider value={{ errorMsg, setErrorMsg }}>
            {children}
        </ErrorContext.Provider>
    );
};

export default ErrorContext;