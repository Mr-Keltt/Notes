import React, { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import Header from '../../components/Header/Header';
import ModalImage from '../../components/ModalImage/ModalImage';
import './NoteDetails.css';

// Функция форматирования даты в формате dd.mm.yyyy
const formatDate = (dateString) => {
  const date = new Date(dateString);
  const dd = String(date.getDate()).padStart(2, '0');
  const mm = String(date.getMonth() + 1).padStart(2, '0');
  const yyyy = date.getFullYear();
  return `${dd}.${mm}.${yyyy}`;
};

// Dummy данные для примера
const dummyNote = {
  uid: '1',
  title: 'Пример заметки',
  text: 'Полный текст заметки, который может быть длинным и содержать много информации. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus interdum sollicitudin purus, id ullamcorper turpis bibendum ac. Sed ut dui nec urna facilisis consequat.',
  dateChange: '2025-03-13T18:21:01.919Z',
  images: [
    { uid: 'img1', url: 'https://via.placeholder.com/300' },
    { uid: 'img2', url: 'https://via.placeholder.com/300' },
    { uid: 'img3', url: 'https://via.placeholder.com/300' },
  ]
};

const NoteDetails = () => {
  const { noteId } = useParams();
  const navigate = useNavigate();
  const note = dummyNote; // Здесь данные для примера
  const [modalImage, setModalImage] = useState(null);

  const openImageModal = (src) => {
    setModalImage(src);
  };

  const closeImageModal = () => {
    setModalImage(null);
  };

  return (
    <>
      <Header showBack={true} />
      <div className="note-details-container">
        <div className="note-header">
          <div className="note-title-section">
            <h1 className="note-title">{note.title}</h1>
            <span className="note-date">{formatDate(note.dateChange)}</span>
          </div>
          <div className="note-header-actions">
            <button 
              className="edit-btn" 
              onClick={() => navigate(`/note/edit/${note.uid}`)}
            >
              ✏️
            </button>
            <button 
              className="delete-btn" 
              onClick={() => alert('Удалить заметку')}
            >
              ❌
            </button>
          </div>
        </div>
        <div className="note-content">
          <div className="note-fulltext-panel">
            <p>{note.text}</p>
          </div>
          <div className="note-images-grid">
            {note.images.map((image) => (
              <div className="image-wrapper" key={image.uid}>
                <img
                  src={image.url}
                  alt=""
                  onClick={() => openImageModal(image.url)}
                />
              </div>
            ))}
          </div>
        </div>
      </div>
      <ModalImage src={modalImage} alt="Просмотр изображения" onClose={closeImageModal} />
    </>
  );
};

export default NoteDetails;
