// src/hooks/useActiveUser.js
import { useState, useEffect } from 'react';

const STORAGE_KEY = 'activeUser';

export function useActiveUser(userList) {
  const [activeUser, setActiveUserState] = useState(null);

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
    const oneHour = 3600000; // 1 час в мс
    // Если сохранённое значение существует и не устарело
    if (storedData && storedData.timestamp && now - storedData.timestamp < oneHour) {
      const exists = userList.find(user => user.guid === storedData.id);
      if (exists) {
        setActiveUserState(storedData.id);
        return;
      }
    }
    // Если нет активного пользователя или сохранённого нет/устарело – берем первого в списке (если есть)
    if (userList.length > 0) {
      setActiveUserState(userList[0].guid);
      localStorage.setItem(
        STORAGE_KEY,
        JSON.stringify({ id: userList[0].guid, timestamp: now })
      );
    }
  }, [userList]);

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
