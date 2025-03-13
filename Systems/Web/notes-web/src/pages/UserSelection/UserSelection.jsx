// src/pages/UserSelection/UserSelection.jsx
import React from 'react';
import Header from '../../components/Header/Header';
import AddNoteButton from '../../components/AddNoteButton/AddNoteButton';
import UserCard from '../../components/UserCard/UserCard';
import './UserSelection.css';

const sampleUsers = [
  { guid: '3fa85f64-5717-4562-b3fc-2c963f66afa6', active: true },
  { guid: '12345678-1234-1234-1234-123456789012', active: false },
  { guid: 'abcdefab-cdef-abcd-efab-cdefabcdefab', active: false },
];

const UserSelection = () => {
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
        {sampleUsers.map(user => (
          <UserCard 
            key={user.guid} 
            user={user} 
            active={user.active} 
            onDelete={handleDeleteUser} 
          />
        ))}
      </div>
      <AddNoteButton onClick={handleAddUser} />
    </>
  );
};

export default UserSelection;
