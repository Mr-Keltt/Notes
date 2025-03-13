import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import Header from '../../components/Header/Header';
import './NoteEditor.css';

// Dummy данные для примера редактирования
const dummyNote = {
  uid: '1',
  title: 'Пример заметки для редактирования',
  text: 'Полный текст заметки, который уже сохранён и подставляется для редактирования. Lorem ipsum dolor sit amet, consectetur adipiscing elit.',
  images: [
    { uid: 'img1', url: 'https://via.placeholder.com/300' },
    { uid: 'img2', url: 'https://via.placeholder.com/300' },
    { uid: 'img3', url: 'https://via.placeholder.com/300' },
  ]
};

const NoteEditor = () => {
  const { noteId } = useParams();
  const isEditMode = !!noteId; // если noteId есть, то редактирование
  const [title, setTitle] = useState('');
  const [text, setText] = useState('');
  const [images, setImages] = useState([]);

  // При загрузке, если редактирование – подставляем данные из dummyNote
  useEffect(() => {
    if (isEditMode) {
      setTitle(dummyNote.title);
      setText(dummyNote.text);
      setImages(dummyNote.images);
    }
  }, [isEditMode]);

  const handleAttachPhoto = () => {
    // Здесь можно реализовать открытие диалога выбора файлов
    alert('Функция прикрепления фото');
  };

  const handleDeleteImage = (uid) => {
    setImages(prev => prev.filter(img => img.uid !== uid));
  };

  const handleSaveNote = () => {
    // Функция сохранения заметки (создание или обновление)
    alert('Сохранить заметку');
  };

  return (
    <>
      <Header showBack={true} />
      <div className="note-editor-container">
        <div className="note-editor-panel">
          <label htmlFor="title">Название</label>
          <input 
            id="title"
            type="text" 
            value={title} 
            onChange={(e) => setTitle(e.target.value)}
          />
          <label htmlFor="text">Текст</label>
          <textarea 
            id="text"
            value={text} 
            onChange={(e) => setText(e.target.value)}
          ></textarea>
          <button className="attach-photo-btn" onClick={handleAttachPhoto}>
            Прикрепить фото
          </button>
          <div className="note-images-grid">
            {images.map(image => (
              <div className="image-wrapper" key={image.uid}>
                <img 
                  src={image.url} 
                  alt="" 
                />
                <button 
                  className="delete-image-btn" 
                  onClick={() => handleDeleteImage(image.uid)}
                >
                  ✖
                </button>
              </div>
            ))}
          </div>
          <button className="save-note-btn" onClick={handleSaveNote}>
            Сохранить заметку
          </button>
        </div>
      </div>
    </>
  );
};

export default NoteEditor;
