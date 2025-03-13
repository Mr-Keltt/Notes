// src/pages/UserSelection/UserSelection.jsx
import React from 'react';
import Header from '../../components/Header/Header';
import AddNoteButton from '../../components/AddNoteButton/AddNoteButton';
import UserCard from '../../components/UserCard/UserCard';
import { useActiveUserContext } from '../../context/ActiveUserContext';
import './UserSelection.css';

const UserSelection = () => {
  const { activeUser, updateActiveUser, users } = useActiveUserContext();

  const handleAddUser = () => {
    alert('Добавить нового пользователя');
  };

  const handleDeleteUser = (guid) => {
    alert(`Удалить пользователя ${guid}`);
  };

  return (
    <>
      <Header showBack={true} />
      <div className="container">
        {users.map((user) => (
          <UserCard
            key={user.guid}
            user={user}
            active={user.guid === activeUser}
            onClick={updateActiveUser}
            onDelete={handleDeleteUser}
          />
        ))}
      </div>
      <AddNoteButton onClick={handleAddUser} />
    </>
  );
};

export default UserSelection;
