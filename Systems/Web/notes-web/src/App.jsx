import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Home from './pages/Home/Home';
import UserSelection from './pages/UserSelection/UserSelection';
import NoteDetails from './pages/NoteDetails/NoteDetails';
import NoteEditor from './pages/NoteEditor/NoteEditor';
import { ActiveUserProvider } from './context/ActiveUserContext';
import './styles/global.css';

function App() {
  return (
    <ActiveUserProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/users" element={<UserSelection />} />
          <Route path="/note/:noteId" element={<NoteDetails />} />
          <Route path="/note/edit/:noteId" element={<NoteEditor />} />
          <Route path="/note/new" element={<NoteEditor />} />
        </Routes>
      </BrowserRouter>
    </ActiveUserProvider>
  );
}

export default App;
