# CarparkWebAPI

CarparkWebAPI is a simple ASP.NET Core 5.0 Web API application that displays carpark availability data to authenticated users.

Users who register and login will be issued a Jwt Token, which will be used to authorize them to access the Carpark Data page and Account Details page.

Jwt Authentication is handled by JwtBearer package, and tokens expire within 1 day or when the browser is closed.

MySQL database running on localhost server is used to store registered user information.

Carpark availability data is fetched from https://data.gov.sg/dataset/carpark-availability


## Login & Register
![Login-Register](https://user-images.githubusercontent.com/68676763/139402132-4d9a5cae-11cd-4c6a-a615-6bf8bf135f09.gif)

## Successful JWT Authentication
![JWT-Auth](https://user-images.githubusercontent.com/68676763/139402147-ef0cebe2-b0c1-4048-a5fb-18a5dfb8860e.gif)

## Expired JWT Token
![Expired-Token](https://user-images.githubusercontent.com/68676763/139402155-6ddcce76-c995-476b-a079-308cbf9f0dd3.gif)



# Installation

* Database: MySQL with MySQL Workbench
* IDE: Visual Studio 2019
* Framework: .NET Core 5.0

1. Open Visual Studio 2019 
2. Click **Clone a repository** and paste this link: `https://github.com/myc37/CarparkWebAPI.git`
3. Open `CarparkWebAPI.sln`
4. Open MySQL Workbench and run a local MySQL server on port 3306 with root user
    
    ![image](https://user-images.githubusercontent.com/68676763/139389200-80ea274e-358e-4208-b39f-7ca5f1c5fef5.png)

5. Update the **default connection string** password to your root password in `appsettings.json`
    
    ![image](https://user-images.githubusercontent.com/68676763/139389327-7c8d7f55-87c4-4dc6-b8d0-a3d054f38cd4.png)

    - Delete all files in the `Migrations` folder
    
        ![image](https://user-images.githubusercontent.com/68676763/139388279-0a53c729-a346-4b0b-80a3-b4059a484949.png)

    - Open **package manager console** in the **Tools** section
        
        ![image](https://user-images.githubusercontent.com/68676763/139388577-f64c038d-25bf-45e4-9634-502a8905f353.png)

    - Run `add-migration Init` in **package manager console**

        ![image](https://user-images.githubusercontent.com/68676763/139388886-5f00c402-f5ec-415e-afdd-95291642bdc1.png)

    - Run `update-databse` in **package manager console**

        ![image](https://user-images.githubusercontent.com/68676763/139388980-a7b2be27-dcd8-418d-aac8-6ac9c0efb9ef.png)

6. Run the project with IIS Express
7. Trust SSL Connection (if necessary)
8. The app should run on `https://localhost:44305`


