import React, { createContext, useContext, useState, useEffect } from 'react';

const ActiveUserContext = createContext();

const STORAGE_KEY = 'activeUser';

export const ActiveUserProvider = ({ children }) => {
  const [activeUser, setActiveUser] = useState(null);
  const [users, setUsers] = useState([]);

  const loadUsers = async () => {
    try {
      const baseUrl = process.env.Main__PublicUrl || 'http://localhost:10000';
      const response = await fetch(`${baseUrl}/api/Users`);
      if (!response.ok) {
        throw new Error('Ошибка при загрузке пользователей');
      }
      const data = await response.json();
      const loadedUsers = Array.isArray(data)
        ? data.map(user => ({ guid: user.uid }))
        : [];
      setUsers(loadedUsers);
    } catch (error) {
      console.error(error);
      setUsers([]);
    }
  };

  useEffect(() => {
    loadUsers();
  }, []);

  useEffect(() => {
    const now = Date.now();
    const oneHour = 3600000;
    const stored = localStorage.getItem(STORAGE_KEY);
    let storedData = null;
    if (stored) {
      try {
        storedData = JSON.parse(stored);
      } catch (e) {
        storedData = null;
      }
    }
    if (
      storedData &&
      storedData.timestamp &&
      now - storedData.timestamp < oneHour &&
      users.find(user => user.guid === storedData.id)
    ) {
      setActiveUser(storedData.id);
    } else if (users.length > 0) {
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

  const addUser = (newUser) => {
    setUsers(prev => [...prev, { guid: newUser.uid }]);
    updateActiveUser(newUser.uid);
  };

  const deleteUser = async (id) => {
    try {
      const baseUrl = process.env.Main__PublicUrl || 'http://localhost:10000';
      const response = await fetch(`${baseUrl}/api/Users/${id}`, {
        method: 'DELETE',
      });
      if (!response.ok) {
        throw new Error('Ошибка при удалении пользователя');
      }
      setUsers(prev => {
        const newUsers = prev.filter(user => user.guid !== id);
        if (activeUser === id && newUsers.length > 0) {
          updateActiveUser(newUsers[0].guid);
        } else if (newUsers.length === 0) {
          setActiveUser(null);
          localStorage.removeItem(STORAGE_KEY);
        }
        return newUsers;
      });
    } catch (error) {
      console.error(error);
      alert('Ошибка при удалении пользователя');
    }
  };

  return (
    <ActiveUserContext.Provider value={{ activeUser, updateActiveUser, users, addUser, deleteUser }}>
      {children}
    </ActiveUserContext.Provider>
  );
};

export const useActiveUserContext = () => useContext(ActiveUserContext);
