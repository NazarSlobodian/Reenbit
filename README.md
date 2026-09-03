This test task couldn't be deployed on Azure because all attempts to register an account and open a free trial there ended with "Account illegible" error.
It is, however, fully containerized.
## How to Run

1. Ensure **Docker** is installed on your machine.
2. In the root directory of the project, run the following command to start the database (MSSQL) and the API:
```bash
   docker-compose up --build
```
4. Open the Swagger UI (http://localhost:5140/swagger/index.html) in your browser to test the API endpoints.
5. Open http://localhost:4200/ to check the UI.

You can also run the concurrency test by opening the Test.IntegrationTests folder in cmd and running:
```bash
   dotnet test
```
