import { useJwt } from "react-jwt";
import { api } from "./apiAxios";
import { createContext, useState, ReactNode, useEffect } from 'react';


interface AuthContextType {
    isLoggedIn: boolean;
    userName: string
    login: (email: string, password: string) => void;
    logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
    children: ReactNode;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {

    const [isLoggedIn, setIsLoggedIn] = useState(!!localStorage.getItem("token"));
    const [userName, setUserName] = useState(localStorage.getItem("user") ?? "")

    const login = async (email: string, password: string) => {
        await api.post("/user/login", { email, password }).then((resp) => {
            localStorage.setItem("token", resp.data.token);
            localStorage.setItem("user", resp.data.userName);
            setIsLoggedIn(true);
            setUserName(resp.data.userName);
        })
    };

    const logout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        setIsLoggedIn(false);
        setUserName("")
    };

    return (
        <AuthContext.Provider value={{ isLoggedIn, userName, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};
export default AuthContext;