
import { ThemeProvider } from '@emotion/react';
import './App.css'
import MenuBar from './components/MenuBar';
import Register from './components/register';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { darkTheme } from "./globalHelpers/theme"
import { CssBaseline } from '@mui/material';
import Login from './components/login';
import { AuthProvider } from './globalHelpers/AuthContext';
import CreateCompetition from './components/Competition/CreateCompetition';
import CompetitionDetails from './components/Competition/CompetitionDetails';
import UserDetails from './components/User/UserDetails';

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
                <Route path='/competition/create' element={<CreateCompetition />} />
                <Route path='/competition/details/:id' element={<CompetitionDetails />} />
                <Route path='/user/profile/:name' element={<UserDetails />} />
              </Routes>
            </BrowserRouter>
          </CssBaseline>
        </ThemeProvider>
      </AuthProvider>
    </>

  );
}

export default App
