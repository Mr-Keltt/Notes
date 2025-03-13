// src/context/ActiveUserContext.jsx
import React, { createContext, useContext, useState, useEffect } from 'react';

const ActiveUserContext = createContext();

const SAMPLE_USERS = [
  { guid: '3fa85f64-5717-4562-b3fc-2c963f66afa6' },
  { guid: '12345678-1234-1234-1234-123456789012' },
  { guid: 'abcdefab-cdef-abcd-efab-cdefabcdefab' },
];

const STORAGE_KEY = 'activeUser';

export const ActiveUserProvider = ({ children }) => {
  const [activeUser, setActiveUser] = useState(null);
  const [users, setUsers] = useState(SAMPLE_USERS);

  useEffect(() => {
    const stored = localStorage.getItem(STORAGE_KEY);
    let storedData = null;
    if (stored) {
      try {
        storedData = JSON.parse(stored);
      } catch (e) {
        storedData = null;
      }
    }
    const now = Date.now();
    const oneHour = 3600000; // 1 час в миллисекундах
    if (
      storedData &&
      storedData.timestamp &&
      now - storedData.timestamp < oneHour
    ) {
      const exists = users.find((user) => user.guid === storedData.id);
      if (exists) {
        setActiveUser(storedData.id);
        return;
      }
    }
    // Если сохранённого пользователя нет, или он устарел или не найден в списке – активным делаем первого
    if (users.length > 0) {
      setActiveUser(users[0].guid);
      localStorage.setItem(
        STORAGE_KEY,
        JSON.stringify({ id: users[0].guid, timestamp: now })
      );
    }
  }, [users]);

  const updateActiveUser = (id) => {
    const now = Date.now();
    setActiveUser(id);
    localStorage.setItem(
      STORAGE_KEY,
      JSON.stringify({ id, timestamp: now })
    );
  };

  return (
    <ActiveUserContext.Provider value={{ activeUser, updateActiveUser, users }}>
      {children}
    </ActiveUserContext.Provider>
  );
};

export const useActiveUserContext = () => useContext(ActiveUserContext);
