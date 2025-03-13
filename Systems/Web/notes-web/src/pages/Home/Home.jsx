// src/pages/Home/Home.jsx
import React from 'react';
import Header from '../../components/Header/Header';
import NoteCard from '../../components/NoteCard/NoteCard';
import AddNoteButton from '../../components/AddNoteButton/AddNoteButton';
import { useActiveUserContext } from '../../context/ActiveUserContext';
import './Home.css';

const sampleNotes = [
  {
    uid: '1',
    title: 'Первая заметка',
    text: 'Это пример заметки, которая содержит достаточно текста, чтобы показать, как работает обрезка текста с троеточием, если весь текст не помещается в карточке.',
  },
  {
    uid: '2',
    title: 'Вторая заметка',
    text: 'Здесь ещё один пример заметки для демонстрации адаптивной сетки карточек. Текст ограничен тремя строками, и если его много, он будет обрезаться.',
  },
  {
    uid: '3',
    title: 'Третья заметка',
    text: 'Короткий текст заметки.',
  },
];

const Home = () => {
  const { activeUser } = useActiveUserContext();

  const handleAddNote = () => {
    alert('Добавить новую заметку');
  };

  return (
    <>
      <Header />
      <div className="main-content">
        <div className="container">
          <div className="notes-grid">
            {sampleNotes.map(note => (
              <NoteCard key={note.uid} note={note} />
            ))}
          </div>
        </div>
      </div>
      <AddNoteButton onClick={handleAddNote} />
    </>
  );
};

export default Home;
