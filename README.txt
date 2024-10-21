# Prerequisites
Ensure that Docker is installed and running on your PC before proceeding.

## 1. Install and run DB 

	>> docker compose up -d 

	- Pull the latest image for Microsoft SQL Server from DockerHub:https://hub.docker.com/r/microsoft/mssql-server
	- it will create a container with your DB ready to store all the information


2. Run the web app 

- Navigate to the root directory of the project. (Steal_Cats_ASP.NET_8/src/StealCatsService/)
- Run the following command to start the backend service: 
	>> dotnet watch to see swagger endpoint and to use requests

This will start the web app and make the Swagger endpoint available for testing and using API requests.
