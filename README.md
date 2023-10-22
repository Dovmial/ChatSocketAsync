# SocketAPPTest

# Цель: опробовать постоянное соединение между сокетами с асинхронной передачей данных на GUI-клиенте и сервере.

# Миниальная прототип клиент - серверной реализации чата на сокетах со своим протоколом общения.
Реализовано:
 - установка постоянной связи клиента с сервером;
 - хранение подключенных клиентов на сервере;
 - регистрация клиента в базе сервера и выдача уникального иидентификатора (guid), хранение хэшированных солёных паролей;
 - общение с сервером в режиме эха;
 - авторизация клиента с получением всех онлайн пользователей для дальнейшего общения;
 - отправка сообщений другому клиенту посредством сервера;
 - хранение всех сообщений типа клиент-клиент
 - получение по запросу всех сообщений клиента, а так же тех, где он адресат;
 - при разрыве связи клиента - он пропадает из списка других клиентов;
 - хранение настроек подключения к серверу и уникальный номер клиента в файле options.json (НЕЛЬЗЯ ЗАПУСКАТЬ КЛИЕНТОВ ИЗ ОДНОЙ И ТОЙ ЖЕ ПАПКИ).

   База хранится в каталоге сервера.

# Список технологий:
  - C#10 (.NET 6)
  - EF CORE 7
  - SQLite
  - WinForms

![Image](https://github.com/Dovmial/ChatSocketAsync/assets/16364360/9758e312-c212-42b2-add8-bca35642a71b)
