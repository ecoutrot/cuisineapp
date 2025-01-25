# cuisine

docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Password123!' -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest

docker exec -it sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'Password123!'

./api/src/Cuisine.Infrastructure dotnet ef database update
./api/src/Cuisine.Api dotnet run
.frontend npm instll
.frontend npm run dev

GeminiApiKey : https://aistudio.google.com/