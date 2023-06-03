# Game Trip ASP .NET Core Web API
This project allows the web application to run smoothly [GameTrip](https://www.game-trip.fr/)

## Getting Started

To setup this project, you need to clone the git repo

```sh
$ git clone https://github.com/Game-Trip/gametrip-client.git
$ cd gametrip-api/GameTrip
```

followed by

```sh
$ dotnet restore
```
Start docker and deploy MS SQL Server

```sh
$ docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=&5*P691&#q^8^96%^01" -e "MSSQL_PID=Express" -p 57022:1433 --name GameTripSQL -h GameTripSQL -d mcr.microsoft.com/mssql/server:2022-latest
```
On Visual Studio Start the project with docker profile 


