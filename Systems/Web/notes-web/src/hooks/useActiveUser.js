// src/hooks/useActiveUser.js

import { useState, useEffect } from 'react';

const STORAGE_KEY = 'activeUser';

/**
 * Custom hook to manage the active user state.
 * Retrieves and updates the active user from localStorage.
 */
export function useActiveUser(userList) {
  const [activeUser, setActiveUserState] = useState(null);

  useEffect(() => {
    // Try to load stored active user data from localStorage
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
    const oneHour = 3600000; // 1 hour in ms

    // If stored data is recent and valid, set it as active
    if (storedData && storedData.timestamp && now - storedData.timestamp < oneHour) {
      const exists = userList.find(user => user.guid === storedData.id);
      if (exists) {
        setActiveUserState(storedData.id);
        return;
      }
    }

    // Otherwise, use the first user in the list
    if (userList.length > 0) {
      setActiveUserState(userList[0].guid);
      localStorage.setItem(
        STORAGE_KEY,
        JSON.stringify({ id: userList[0].guid, timestamp: now })
      );
    }
  }, [userList]);

  // Function to update the active user and persist the change
  const updateActiveUser = (id) => {
    const now = Date.now();
    setActiveUserState(id);
    localStorage.setItem(
      STORAGE_KEY,
      JSON.stringify({ id, timestamp: now })
    );
  };

  return [activeUser, updateActiveUser];
}
