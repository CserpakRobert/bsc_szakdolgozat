import { api } from "./apiAxios";
import { createContext, useState, ReactNode } from 'react';


interface AuthContextType {
    isLoggedIn: boolean;
    login: (email: string, password: string) => void;
    logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
    children: ReactNode;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
    const [isLoggedIn, setIsLoggedIn] = useState(!!localStorage.getItem("token"));

    const login = async (email: string, password: string) => {
        await api.post("/user/login", { email, password }).then((resp) => {
            localStorage.setItem("token", resp.data.token);
             setIsLoggedIn(true);
        })
    };

    const logout = () => {
        localStorage.removeItem('token'); // Remove token from localStorage
        setIsLoggedIn(false);
    };

    return (
        <AuthContext.Provider value={{ isLoggedIn, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};
export default AuthContext;