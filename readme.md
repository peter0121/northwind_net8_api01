## Target
* nlog
* dapper
* polly


## Dev
* MS SQL
```
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=A755873eddeafcd" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
```

* download sql
```
https://github.com/microsoft/sql-server-samples/blob/master/samples/databases/northwind-pubs/instnwnd.sql
```