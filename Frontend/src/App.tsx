
import './App.css'
import MenuBar from './components/MenuBar';
import Register from './components/register';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Login from './components/login';
import CreateCompetition from './components/Competition/CreateCompetition';
import CompetitionDetails from './components/Competition/CompetitionDetails';
import UserDetails from './components/User/UserDetails';
import CreateSubmission from './components/Submission/CreateSubmission';
import SubmissionDetails from './components/Submission/SubmissionDetails';

import CompetitionList from './components/Competition/CompetitionList';
import ErrorComponent from './components/ErrorComponent';
import CreateGroup from './components/Group/CreateGroup';
import GroupDetails from './components/Group/GroupDetails';
import { useAuth } from './Utils/useAuth';
import HomeComponent from './components/Home';

function App() {
  const { isLoggedIn } = useAuth()
  return (
    <>
      <BrowserRouter>
        <MenuBar></MenuBar>
        <Routes>
          <Route path='/register' element={<Register />} />
          <Route path='/login' element={<Login />} />
          {!isLoggedIn ? (
            <Route path='*' element={<HomeComponent />} />
          ) : (
            <>
              <Route path='/home' element={<HomeComponent />} />
              <Route path='/competition/create' element={<CreateCompetition />} />
              <Route path='/competition/list' element={<CompetitionList />} />
              <Route path='/competition/details/:id' element={<CompetitionDetails />} />
              <Route path='/user/profile/:name' element={<UserDetails />} />
              <Route path='/submission/create/:id' element={<CreateSubmission />} />
              <Route path='/submission/:id' element={<SubmissionDetails />} />
              <Route path="/group/:id" element={<CreateGroup />} />
              <Route path="/group/details/:id" element={<GroupDetails />} />
            </>
          )}
        </Routes>
      </BrowserRouter>

      <ErrorComponent />


    </>

  );
}

export default App
