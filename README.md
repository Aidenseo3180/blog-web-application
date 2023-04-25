# blog-web-application

## Index



## About  

A blog web application that allows users with different access levels to make changes to the blog. Used Microsoft SQL for the local database. ASP .NET Core 7.0, C#, and HTML-related skillsets were used.  

## Getting Started

### Installing

SDK 7.0 and ASP.NET Core Runtime 7.0  
Microsoft SQL Server Management Studio for local database
Either VSCode or the latest version of Microsoft Visual Studio

### Migration

ASP.NET provides various functions for migration  
Use NuGet Package Manager - Package Manage Console for following commands

To create a migration  
'''
Add Migration "{comment}" -context "{DBContext}"
'''

To update the local database  
'''
Update-Database -context "{DBContext}"
'''

### How to use Claudinary

Create account in Claudinary. Then, copy and paste your CloudName, ApiKey, ApiSecret to appsetting.json  

## Deployment  
To deploy to public Azure Db, connect to each of the connectionstrings in appsetting.json.  

Connect to Azure Db using the server name, and SQL Server Authentication method. Enter the User Id and Password to connect.  
If you face a firewall issue that says your IP address does not have an access, go to:  
- Go to Microsoft Azure, your db server, Networking, and add new Firewall rule using your IP address.  
Also make sure to add the virtual IP address of web app in Azure to Db server Firewall rule.  

Move Db from local database to azure database by right clicking the db from the localhost, Tasks, deploy database to Azure SQL database.  

Change connectionstring of appsetting.json  
- Find connectionstring from the Db that you want in Azure, go to connectionstring, copy ADO.NET(SQL authentication) connectionstring, paste to appsetting.json  
- Put your password to connectionstring to connect to database  

To see errors during the deployment process, set ASPNETCORE_ENVIRONMENT to Development from Azure Server.   
Steps: Go to App Service, Configuration, press New Application Setting, and add ASPNETCORE_ENVIRONMENT to name, Development to value.  

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
