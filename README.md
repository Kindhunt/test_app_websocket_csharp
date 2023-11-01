# TestApplicationWebSocket

## Описание

Тестовый проект, являющийся Client-Server приложением. 
Клиентская часть представляет собой WPF приложение, 
использующее Websocket соединение.
Есть всего 3 текстовых поля и кнопка Send.
При её нажатии осуществляется отправка на сервер,
где сам сервер отсылает уведомление остальным клиентам, 
подключенным к нему.
Серверная часть простой ASP.NET сервер.

## Приступая к работе

### Клиент 

Перейдите в папку ~\\TestAppWebsocket\\TestAppWebsocket
Найдите AppExample.config
Создайте App.config по примеру
Для использования клиентом защищенного соединения измените IsSecuredConnection на true
ServerConnectionWS - не защищенное соединение
ServerConnectionWSS - защищенное соединение

### Сервер
 
Перейдите в папку ~\\WebSocketServer\\WebSocketServer
Найдите AppExample.config
Создайте App.config по примеру
DatabaseConnection - соединение с базой данных
Чтобы его задать заполните поля Host, Port, Database, Username, Password
Host - имя хоста или IP-адрес сервера базы данных
Port - порт хоста
Database - название базы данных
Username - имя юзера БД
Password - пароль юзера БД
ServerConnectionHTTP - не защищенное соединение
ServerConnectionHTTP - защищенное соединение
