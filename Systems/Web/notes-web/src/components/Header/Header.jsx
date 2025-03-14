// src/components/Header/Header.jsx

import React from 'react';
import { useNavigate } from 'react-router-dom';
import './Header.css';
import Logo from './Logo';
import UserChangeButton from './UserChangeButton';
import BackButton from './BackButton';

/**
 * Header Component
 * Displays the logo and either a back button or a user change button.
 */
const Header = ({ showBack }) => {
  // Function to navigate to the users page
  const navigate = useNavigate();
  const handleUserChange = () => {
    navigate('/users');
  };

  return (
    <header className="header">
      <Logo />
      {showBack ? (
        <BackButton />
      ) : (
        <UserChangeButton onClick={handleUserChange} />
      )}
    </header>
  );
};

export default Header;
