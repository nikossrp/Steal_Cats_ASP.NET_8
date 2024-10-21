## Prerequisites
Ensure Docker is installed and running on your PC before proceeding.

You need to have installed dotnet-ef, dotnet CLI (version 8)


## Run the API
1. Install and run DB 

    Open the terminal and run the command:  ` docker compose up -d` 

    - Pull the latest image for Microsoft SQL Server from DockerHub: [Microsoft SQL Server](https://hub.docker.com/r/microsoft/mssql-server)
    - It will create a container with your DB ready to store all the information.

2. Run the web app 

    - Navigate to the root directory of the project (Steal_Cats_ASP.NET_8/src/StealCatsService/).
    - Run the following commands:
      
        `dotnet ef database update`
      
        ` dotnet watch ` 

    This will start the web app and make the Swagger endpoint available for testing and using API requests.
