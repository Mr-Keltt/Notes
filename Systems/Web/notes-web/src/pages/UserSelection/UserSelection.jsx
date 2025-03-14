// src/pages/UserSelection/UserSelection.jsx

import React from 'react';
import Header from '../../components/Header/Header';
import AddButton from '../../components/AddButton/AddButton';
import UserCard from '../../components/UserCard/UserCard';
import WarningBanner from '../../components/WarningBanner/WarningBanner';
import { useActiveUserContext } from '../../context/ActiveUserContext';
import './UserSelection.css';

/**
 * UserSelection Component
 * Allows users to be created, selected, and deleted.
 * Displays a warning banner explaining the lack of authentication features.
 */
const UserSelection = () => {
  const { activeUser, updateActiveUser, users, addUser, deleteUser } = useActiveUserContext();

  // Create a new user via API call and update the context
  const handleAddUser = async () => {
    try {
      const baseUrl = process.env.Main__PublicUrl || 'http://localhost:10000';
      const response = await fetch(`${baseUrl}/api/Users`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({})
      });
      if (!response.ok) {
        throw new Error('Ошибка при создании пользователя');
      }
      const newUser = await response.json();
      addUser(newUser);
    } catch (error) {
      console.error(error);
      alert('Ошибка при создании пользователя');
    }
  };

  // Delete the selected user using the context's delete function
  const handleDeleteUser = async (guid) => {
    await deleteUser(guid);
  };

  return (
    <>
      <Header showBack={true} />
      <div className="main-content">
        {/* Warning banner explaining the user management feature */}
        <WarningBanner 
          text="Так как в ТЗ не было указано требований к авторизации и аутентификации, я просто сделал возможность создавать, выбирать и удалять пользователей для всех желающих" 
        />
        <div className="container">
          {users.length > 0 ? (
            users.map((user) => (
              <UserCard 
                key={user.guid} 
                user={user} 
                active={user.guid === activeUser} 
                onClick={updateActiveUser}
                onDelete={handleDeleteUser}
              />
            ))
          ) : (
            <div className="no-users">Пользователи отсутствуют</div>
          )}
        </div>
      </div>
      <AddButton onClick={handleAddUser} />
    </>
  );
};

export default UserSelection;
