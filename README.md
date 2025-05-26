# 📦 Resource Manager

Консольное .NET-приложение для загрузки ресурсов по манифесту, поддерживающее версионирование. Используется в демо-режиме через Docker с сохранением состояния между запусками.

## 🚀 Бысткий старт

### 1. Клонируй репозиторий и перейди в каталог проекта

```bash
git clone https://github.com/flexer1992/resource-manager
cd resource-manager
```

### 2. Создай .env рядом с Dockerfile (Interview) 
```
APP_VERSION=v1
BASE_URL=https://base.url
```

### 3. Построй Docker-образ
```bash
docker build -f Interview/Dockerfile -t resource-manager .
```

### 4. Запусти контейнер

```bash
  docker run -v $(pwd)/resources:/app/resources --env-file Interview/.env resource-manager
```
💾 Все загруженные ресурсы и манифесты будут сохранены в папке resources/ на вашей машине и использованы повторно при следующем запуске.

⚙️ Переменные окружения

| Переменная      | Описание                                   | Пример                     |
| --------------- | ------------------------------------------ |----------------------------|
| `APP_VERSION`   | Версия приложения, которую нужно загрузить | `v3`                       |
| `BASE_URL`      | Базовый адрес API                          | `https://platform-url.com` |


