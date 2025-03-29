
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
import CreateSubmission from './components/Submission/CreateSubmission';
import SubmissionDetails from './components/Submission/SubmissionDetails';
import SubmissionSmallDetails from './components/Submission/SubmissionSmallDetails';

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
                <Route path='/submission/create/:id' element={<CreateSubmission />} />
                <Route path='/submission/:id' element={<SubmissionDetails />} />
                <Route path='/submission/test' element={<SubmissionSmallDetails submission={{id:2,title:"asd",type:"movie"}} />} />
              </Routes>
            </BrowserRouter>
          </CssBaseline>
        </ThemeProvider>
      </AuthProvider>
    </>

  );
}

export default App
