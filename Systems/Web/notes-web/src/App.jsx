// src/App.jsx
import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Home from './pages/Home/Home';
import UserSelection from './pages/UserSelection/UserSelection';
import { ActiveUserProvider } from './context/ActiveUserContext';
import './styles/global.css';

function App() {
  return (
    <ActiveUserProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/users" element={<UserSelection />} />
        </Routes>
      </BrowserRouter>
    </ActiveUserProvider>
  );
}

export default App;
