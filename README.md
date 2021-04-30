# TicketManager
Ticket (Task) manager, built with Asp Net Core, Identity Server 4 and EF Core

Для работы приложения необходимо собрать решение (проект Datalayer содержит необходимые контексты для приложений) и запустить сначала приложение AuthApp, 
выполняющее аутентифкацию и авторизацию. 
Первый пуск занимает некоторое время, т.к. создаются новые таблицы для базы данных.
Следующим собирается и запускается приложение TicketsWebApp, выполняющее функцию UI менеджера задач.

Для работы необходим Sql Server

In English, please!

To start the app you must first build entire solution (Datalayer contains DB contexts for both apps) and then run AuthApp project, that controls authentification and authorization flow.
First run will take some time as app must create DB tables for its operation.
Next app to run is TicketsWebApp - ticket manager with UI.

You need SQL server to run this app. 
