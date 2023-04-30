# EloService

Simple Elo Microservice Application

Configuration in the appsettings.json file or environment variables or command-line options. 
Set the 'Database' setting to your Postgres Connection string.

Included in the release are a migrations sql script that should upgrade your database, or a efbundle.exe file that can be run just like the application to apply database migrations.

# Endpoints

* `/swagger`: When running in a dev environment ('DOTNET_ENVIRONMENT' environment variable or 'Environment' CLI setting)
* GET `/api/players`: Gets all players in the database
* POST: `/api/matchresults`: Post a new match result with a list of players in each team and which team won
