## Prerequisites
Ensure Docker is installed and running on your PC before proceeding.

You need to have installed dotnet-ef, dotnet CLI (version 8)


## Run the API
1. Install and run DB 

    Open the terminal, navigate to the starting point of this project, and run the following command:

    ` docker compose up -d` 

    - Pull the latest image for Microsoft SQL Server from DockerHub: [Microsoft SQL Server](https://hub.docker.com/r/microsoft/mssql-server)
    - It will create a container with your DB ready to store all the information.

3. Run the web app 

    - Navigate to the root directory of the project (Steal_Cats_ASP.NET_8/src/StealCatsService/).
    - Run the following commands:
      
        `dotnet ef database update`
      
        ` dotnet watch ` 

    
    This will
   
   1)  Make your Tables on DB
   2) Start the web app and make the Swagger endpoint available for testing.
