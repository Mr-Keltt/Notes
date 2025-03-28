# Notes Web Application

Это веб-приложение для создания и управления заметками в браузере, разработанное в рамках стажировки в компанию Северсталь. Проект реализован с использованием микросервисной архитектуры и покрыт различными тестами.

## Оглавление

- [Особенности](#особенности)
- [Используемые технологии](#используемые-технологии)
- [Установка и запуск](#установка-и-запуск)
- [Документация API](#документация-api)
- [Здоровье сервиса](#здоровье-сервиса)
- [Фронтенд](#фронтенд)
- [CI/CD](#cicd)
- [Тестирование](#тестирование)
- [Контакты](#контакты)

## Особенности

- **Создание и управление заметками:** Приложение позволяет создавать, редактировать и удалять заметки.
- **Микросервисная архитектура:** Каждый компонент системы отделен для повышения масштабируемости и надежности.
- **Покрытие тестами:** Проект покрыт различными тестами для обеспечения стабильной работы.
- **Автоматизация:** Используется CI/CD для автоматического запуска контейнеров Docker и тестов.

## Используемые технологии

- **Фронтенд:** React  
- **Бэкенд:** C# .NET  
- **База данных:** PostgreSQL (общение с БД через Entity Framework)  
- **Контейнеризация:** Docker и Docker Compose

## Установка и запуск

1. **Клонируйте репозиторий:**

   ```bash
   git clone https://github.com/Mr-Keltt/Notes.git
   cd your-folder
   ```

2. **Запуск приложения:**

   Выполните команду для сборки и запуска контейнеров:

   ```bash
   docker-compose up --build
   ```

   После этого:
   - Бэкенд будет доступен на [http://localhost:10000](http://localhost:10000)
   - Фронтенд будет доступен на [http://localhost:10100](http://localhost:10100)

## Документация API

Swagger документация доступна по адресу:

[http://localhost:10000/docs](http://localhost:10000/docs)

## Здоровье сервиса

Для проверки статуса сервиса воспользуйтесь хелсчекером:

[http://localhost:10000/health](http://localhost:10000/health)

## Фронтенд

Фронтенд часть приложения разработана на React и доступна по следующему адресу:

[http://localhost:10100](http://localhost:10100)

## CI/CD

Проект настроен на автоматический запуск тестов и сборку Docker контейнеров с использованием CI/CD. Это обеспечивает постоянную интеграцию и быструю проверку работоспособности приложения после каждого коммита.

## Тестирование

Приложение покрыто разнообразными тестами, включающими:
- Юнит-тесты для основных компонентов.
- Интеграционные тесты для проверки взаимодействия между сервисами.
