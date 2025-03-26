
import { ThemeProvider } from '@emotion/react';
import './App.css'
import MenuBar from './components/MenuBar';
import Register from './components/register';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { darkTheme } from "./theme"
import { CssBaseline } from '@mui/material';
import Login from './components/login';
import { AuthProvider } from './AuthContext';

function App() {

  return (
    <>
      <AuthProvider>

        <ThemeProvider theme={darkTheme}>
          <CssBaseline>


            <BrowserRouter>
              <MenuBar></MenuBar>
              <Routes>
                <Route path='/register' element={<Register />} />
                <Route path='/login' element={<Login />} />
              </Routes>
            </BrowserRouter>
          </CssBaseline>
        </ThemeProvider>
      </AuthProvider>
    </>

  );
}

export default App
