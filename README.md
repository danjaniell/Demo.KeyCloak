This repository contains a Blazor Server and WebAPI implementation secured with [KeyCloak](https://www.keycloak.org/).

---
### Technology

This project uses the following technologies:

- [.NET SDK 7](https://dotnet.microsoft.com/download/dotnet-core/7.0)
- [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
- [KeyCloak](https://www.keycloak.org/)
- [Docker](https://www.docker.com/)

### Running the Application

To run this project, follow these steps:

1. Clone this repository

2. Run KeyCloak server locally using docker. `docker compose -f .\keycloack-docker.yml up`

3. Configure KeyCloak, some things to take note of are:
    ```
    Navigate to http://localhost:8080/admin/master/console
    Create Realm(Test)
    Create User in Test Realm
    Create Client(test_client) in Test Realm
        * Check implicit flow
    Create Audience in created Client
        * Under Client scopes, click test_client-dedicated, create Mapper
    Configure redirect URIs to your application URL

   ```

4. Modify `appsettings.json`. Configured with default values so it should be ready to run.

6. Move to the root folder of the project and run the following command in a terminal window:

   ```shell
   dotnet run --project src/Application
   or
   dotnet run --project src/Server
   ```
   Also can be run through VS Code `Run Application or Run Server`

7. Now, point your browser to [https://localhost:5001](https://localhost:5001/)

   You should be able to authenticate and access the application through the login button.
   
   And for the Web API [https://localhost:7001](https://localhost:7001/)
   
   You can use client name in the Implicit Auth option

