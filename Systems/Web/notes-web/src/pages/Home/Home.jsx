import React from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../../components/Header/Header';
import NoteCard from '../../components/NoteCard/NoteCard';
import AddNoteButton from '../../components/AddNoteButton/AddNoteButton';
import { useActiveUserContext } from '../../context/ActiveUserContext';
import './Home.css';

const sampleNotes = [
  {
    uid: '1',
    title: 'Первая заметка',
    text: 'Это пример заметки, которая содержит достаточно текста, чтобы показать, как работает обрезка текста с троеточием...',
  },
  {
    uid: '2',
    title: 'Вторая заметка',
    text: 'Здесь ещё один пример заметки для демонстрации адаптивной сетки карточек. Текст ограничен тремя строками...',
  },
  {
    uid: '3',
    title: 'Третья заметка',
    text: 'Короткий текст заметки.',
  },
];

const Home = () => {
  const navigate = useNavigate();
  const { activeUser } = useActiveUserContext();

  const handleAddNote = () => {
    alert('Добавить новую заметку');
  };

  const handleCardClick = (noteId) => {
    navigate(`/note/${noteId}`);
  };

  return (
    <>
      <Header />
      <div className="main-content">
        <div className="container">
          <div className="notes-grid">
            {sampleNotes.map(note => (
              <NoteCard key={note.uid} note={note} onClick={handleCardClick} />
            ))}
          </div>
        </div>
      </div>
      <AddNoteButton onClick={handleAddNote} />
    </>
  );
};

export default Home;
