// ActiveUserContext: provides state management for the active user and the list of users.

import React, { createContext, useContext, useState, useEffect } from 'react';

// Create a Context for the active user
const ActiveUserContext = createContext();

// Key for localStorage to persist active user data
const STORAGE_KEY = 'activeUser';

/**
 * ActiveUserProvider Component
 * Wraps children with context providing active user, list of users, and related actions.
 */
export const ActiveUserProvider = ({ children }) => {
  // State for the active user and list of users
  const [activeUser, setActiveUser] = useState(null);
  const [users, setUsers] = useState([]);

  // Fetch users from API and store them in state
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

  // Load users when component mounts
  useEffect(() => {
    loadUsers();
  }, []);

  // Set active user based on localStorage data or default to the first user
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

  // Update active user and store change in localStorage
  const updateActiveUser = (id) => {
    const now = Date.now();
    setActiveUser(id);
    localStorage.setItem(
      STORAGE_KEY,
      JSON.stringify({ id, timestamp: now })
    );
  };

  // Add a new user to the list and update active user
  const addUser = (newUser) => {
    setUsers(prev => [...prev, { guid: newUser.uid }]);
    updateActiveUser(newUser.uid);
  };

  // Delete a user from the list and update active user accordingly
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

// Custom hook for using ActiveUserContext
export const useActiveUserContext = () => useContext(ActiveUserContext);
