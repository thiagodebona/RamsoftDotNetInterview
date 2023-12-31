# Welcome to MiniJira tasks manager App!
## Created from scratch, took 30 hours to build it. Including this documentation

Simple .Net Core 6 App tasks manager  **application** meant to be sent to **Ramsoft DotNet interview** for **Thiago**

Used technologies:
 - Fully **RESTfull asyncronous** back-end
 - Dotnet core 6 - **Minimalism code style**
 - MongoDB
 - JWT Authenticator
 - WebSocket - **For live data streaming**
  - Fully mocked unit tests that cover all the cases
  
 Used tools:
  - Visual studio 2022 community edition
  - Docker desktop for windows
  - MongoDBCompass
  
 The project includes all the DockerFiles to run the MiniJira.
  
Including ->   Simple console application to see the MiniJira tasks and board at **live**


## What was requested

- Users can add new tasks to the task board
   - Details include name, description, deadline
- Users can edit tasks and all details
- Users can delete tasks
- Users can attach images to tasks
- Users can drill into tasks to see all details
- Users can add columns to the task board representing different work states (ie. ToDo, In Progress and Done)
- Users can move tasks between columns
- Users can sort tasks in each column alphabetically by name
  - Favorited tasks should be sorted to the top every time
 

## What was delivered
- All the requested feature plus the following:
   > Multiple boards management, create/update/delete boards and it's informations
   
   > User management and authentication
    
   > Know the user who created the task
   
   > Assign users to tasks
   
   > User can delete columns from boards and all it's linked tasks will be moved to the archived tasks
 
   > Event dispatcher, notify and update connected users with the changes via WebSocket

   > Live dashboard app to see the data
 
# MiniJira 1.0 Features

 - Fully functional SwaggerUI with Bearer Auth available
 - Manage **multiple** boards
 - Create and authenticate new user
 - Manage multiple columns inside the boards
 - Can create/update/delete multiple tasks in different boards
   - Can move tasks between columns, ex: **Todo, In progress, In test, done**
   - Can create custom columns
   - Can favorite tasks, favorited tasks are in the top of the board tasks
   - Can assign a task to a user that you've created
   - Can include multiple attachments in the tasks
 - Live connection with the back-end
 - Authenticated real time console application to see the board

![User](./assets/aa113.PNG)

# Project Files Structure
![User](./assets/2314.PNG)

- The project is organized in six projects
 
    > Dotnet.MiniJira.API
  
    > Dotnet.MiniJira.Domain 

    > Dotnet.MiniJira.Application
  
    > Dotnet.MiniJira.Infrastructure
  
    > Dotnet.MiniJira.Live.Dashboard.App
  
    > Dotnet.MiniJira.Tests
    
- The project contains 7 folder.
	> Between folder 1. and 4. we find all the required project to run the **MiniJira** sub-menu.
	
	> **Docker files** folder has the dockerfile to instance a new MongoDB in your local docker container
  
    > **Docker files** for the Dotnet.MiniJira.API is localted in the Dotnet.MiniJira.API/Dockerfile
	
    > **Tests** folder contains all mocked tests to keep integrity in the source code
- Secondary projects
-   > Dotnet.MiniJira.Tests
-   > Dotnet.MiniJira.Live.Dashboard.App - Connect to the **back-end WebSocker** app for **live streamming of data**

## How to run locally the project?

You'll need a mongoDb docker container running or a mongodb local instance
To start a new mongodb docker instace simple run the the following command
> docker compose -f "Mongo-docker-compose.yml" up -d --build 

Then 
 > Configure your VisualStudio to run multiple projects at once
 > **Dotnet.MiniJira.API** and **Dotnet.MiniJira.Live.Dashboard.App**

![User](./assets/app.PNG)

 When the Dotnet.MiniJira.API runs, it automatically seeds the data base with some information. It'll automatically create a new board called **Ramsoft - Issues board**, will create four column **Todo, In progress, Testing and done**
 
 You will have three users available to use
 
 > Administrator -> Username[admin] Password[admin]
 > Developer      -> Username[developer] Password[developer]
 > Tester -> Username[tester] Password[tester]
 
We will see two consoles, one is the **WebAPI** and the other is the **client**, the client is used like a front-end app to see what's going on in the **MiniJira**, the client receive live notification via **webSocket**

![User](./assets/app1.PNG)
![User](./assets/app2.PNG)

## Fully functional Swagger UI with Bearer Auth
> Swagger URL is http://localhost:7100/index.html
![User](./assets/swagger.PNG)

The first thing to do is to authenticate yourself with the existing admin, develop or test user, or create your own user.
> Scroll down until the bottom of Swagger UI, to the section **Users** and **authenticate** with username **admin** and password **admin**
![User](./assets/body.PNG)

Take the JWT token in the response and apply it to the swagger by scrolling to the up and click on button "Authorize", and it will automatically notify the client with the existing boards in the data.

![User](./assets/auth.PNG)

![User](./assets/c5c6.PNG)

And Now you are ready to use the API through Swagger UI

## Connecting the Client to the Notification server via WebSocket

Now you need to inform in the client the username and password and press enter, then it'll connect to the WebSocket and will start receiving live updates from the server.
![User](./assets/app2.PNG)

**The live notification are received whenever one of the following thing happen**
> User is authenticated
> Create/Update/Delete a **board**
> Create/Delete a column in any **board**
> Create/Update/Delete **task**
> Add attachments to a **task**
> Assign a user to **task**
> Move a **task** between different column

## Notifications examples

Now in your SwaggerUI in the section "Users", when you try to authenticate, if the authentication goes well you will receive in your client app the current status of the MiniJira

![User](./assets/k11k12.PNG)

Now let's create a new board

![User](./assets/k7k8.PNG)

Now we can see in the client that a new board was created, with the default column and without tasks

![User](./assets/k5k6.PNG)

Now let's create insert a task on it, using the boardId and ColumnId that we received in the result of the Board's creation

![User](./assets/k3k4.PNG)

Now we can see in the client that a new task was inserted in the board in the column Todo

![User](./assets/k1k2.PNG)



## Business Rules - Permissions

**User module**

|    route|action|admin|develop|tester
|----------------|-----------|----------|-------|-------
|/Authenticate|POST|true|true| true
|/Create|POST|true|true| true|
|/{id}|GET|true|true| true|

**Board module**

|    route|action|admin|develop|tester
|----------------|-----------|----------|-------|-------
|/Board|POST|true|false| false
|/Board|PUT|true|false| false|
|/Board|GET|true|true| true|
|/Board/{id}|DELETE|true|false| false|
|/Board/{id}|GET|true|true| true|
|/Board/DeleteColumn|DELETE|true|true| true|
|/Board/CreateColumn|GET|true|true| true|

**User module**

|    route|action|admin|develop|tester
|----------------|-----------|----------|-------|-------
|/Board/|POST|true|false| false
|/Board/|PUT|true|false| false|
|/Board/|GET|true|true| true|
|/Board/{id}|DELETE|true|false| false|
|/Board/{id}|GET|true|true| true|
|/Board/DeleteColumn|DELETE|true|true| true|
|/Board/CreateColumn|GET|true|true| true|

> Tasks inside of a delete column are moved the list of archived tasks

![image info](./assets/1a.PNG)
> **/Users/Create** -> Do not create **admin** profiles
> It just creates the **Develop** or **Tester** profiles
> The only way  of having an admin user is the back-end that seed the db in the app's initialization


![User](./assets/board.PNG)
> Only **admin** can **CREATE/DELETE/UPDATE** boards information
> **Developer and Testers** can only **CREATE/DELETE** columns in a board

![User](./assets/p4.PNG)
> All the profiles can apply any action on any tasks

# Unit tests
  Fully mocked unit tests that cover all the cases can be found in the folder tests. 33 unit test.
![User](./assets/n1m1.PNG)
