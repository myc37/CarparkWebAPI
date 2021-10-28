# CarparkWebAPI

CarparkWebAPI is a simple ASP.NET Core 5.0 Web API application that displays carpark availability data to authenticated users.

Users who register and login will be issued a Jwt Token, which will be used to authorize them to access the Carpark Data page and Account Details page.

Jwt Authentication is handled by JwtBearer package, and tokens expire within 1 day or when the browser is closed.

MySQL database running on localhost server is used to store registered user information.

Carpark availability data is fetched from https://data.gov.sg/dataset/carpark-availability

<gif>

# Installation

* Database: MySQL
* IDE: Visual Studio 2019
* Framework: .NET Core 5.0

1. Clone this github repository to local machine by running `git clone https://github.com/myc37/CarparkWebAPI.git`
2. Open `CarparkWebAPI.sln` with Visual Studio 2019
3. Run a local MySQL server on port 3306 and update the default connection string password to your root password in `appsettings.json`
    - Delete all files in the `Migrations` folder
    - Run `add-migration Init` in package manager console
    - Run `update-databse` in package manager console
4. Run the project in Visual Studio 2019 with IIS Express
5. The app should run on `https://localhost:44315`


