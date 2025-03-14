// src/pages/NoteEditor/NoteEditor.jsx
import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import Header from '../../components/Header/Header';
import { useActiveUserContext } from '../../context/ActiveUserContext';
import './NoteEditor.css';

const NoteEditor = () => {
  const { noteId } = useParams();
  const navigate = useNavigate();
  const isEditMode = Boolean(noteId);
  const { activeUser } = useActiveUserContext();

  const [title, setTitle] = useState('');
  const [text, setText] = useState('');
  const [marked, setMarked] = useState(false);
  const [images, setImages] = useState([]);

  // При редактировании загружаем данные заметки с сервера
  useEffect(() => {
    if (isEditMode) {
      const fetchNote = async () => {
        try {
          const baseUrl = process.env.Main__PublicUrl || 'http://localhost:10000';
          const response = await fetch(`${baseUrl}/api/Notes/${noteId}`);
          if (!response.ok) {
            throw new Error('Ошибка при загрузке заметки');
          }
          const note = await response.json();
          setTitle(note.title);
          setText(note.text);
          setMarked(note.marked);
          setImages(note.photos || []); // предполагаем, что изображения возвращаются в поле photos
        } catch (error) {
          console.error(error);
          alert('Ошибка загрузки заметки');
        }
      };
      fetchNote();
    }
  }, [isEditMode, noteId]);

  const handleAttachPhoto = () => {
    // Здесь можно реализовать открытие диалога выбора файлов
    alert('Функция прикрепления фото');
  };

  const handleDeleteImage = (uid) => {
    setImages(prev => prev.filter(img => img.uid !== uid));
  };

  const handleSaveNote = async () => {
    // Валидация: название заметки не должно быть пустым или состоять только из пробелов
    if (title.trim().length === 0) {
      alert('Название заметки не может быть пустым');
      return;
    }

    try {
      const baseUrl = process.env.Main__PublicUrl || 'http://localhost:10000';
      if (isEditMode) {
        // Режим редактирования – PUT-запрос
        const response = await fetch(`${baseUrl}/api/Notes/${noteId}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ title, text, marked }),
        });
        if (response.status !== 204) {
          throw new Error('Ошибка обновления заметки');
        }
      } else {
        // Режим создания – проверяем наличие активного пользователя
        if (!activeUser) {
          alert('Не выбран активный пользователь');
          return;
        }
        const response = await fetch(`${baseUrl}/api/Notes`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ title, text, marked, userId: activeUser }),
        });
        if (response.status !== 201) {
          throw new Error('Ошибка создания заметки');
        }
      }
      // После успешного сохранения переходим на главную страницу
      navigate('/');
    } catch (error) {
      console.error(error);
      alert(error.message);
    }
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
