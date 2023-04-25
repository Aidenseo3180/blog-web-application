# blog-web-application

## Index
  
  - [About](#About) 
  - [Overview](#Overview)
  - [Getting Started](#getting-started)
  - [License](#license)

## About  

A blog web application that allows users with different access levels to make changes to the blog.  
Used Microsoft SQL for the local database. ASP.NET Core 7.0, C#, and HTML-related skillsets were used.  

## Overview

The blog web application consists of 3 major access levels: user, admin, and superadmin  
Admin can add, edit, and delete tags and blog posts  
Superadmin can add and delete user with user and admin access level  

Superadmin has superadmin, admin, and user access levels  
Admin has both the admin and user levels  

## Getting Started

### Installing

SDK 7.0 and ASP.NET Core Runtime 7.0  
Microsoft SQL Server Management Studio for local database
Either VSCode or the latest version of Microsoft Visual Studio

### Migration

ASP.NET provides various functions for migration  
Use NuGet Package Manager - Package Manage Console for following commands

To create a migration  
```
Add Migration "{comment}" -context "{DBContext}"
```

To update the local database  
```
Update-Database -context "{DBContext}"
```

### How to use Claudinary

Create account in Claudinary. Then, copy and paste your CloudName, ApiKey, ApiSecret to appsetting.json  

## Deployment  

1. Create Microsoft Azure account
2. Right-click from project, click `Publish`
3. Create Azure DB server by creating Id and Password
4. Create dependency using server name, make connections to connectionstrings
5. From Microsoft SQL Studio, connect to both local database and Azure Db using SQL Server Authentication method
6. Move Db tables from local database to azure database by right-click the db table from the localhost `Tasks`, `deploy database to Azure SQL database`  
7. Find connectionstring from Azure Db, copy and paste to appsetting.json with password

If Firewall problem occurs:
*  Go to Microsoft Azure, your db server, Networking, and add new Firewall rule using your IP address.
*  Also add the virtual IP address of web app in Azure to Db server Firewall rule.  

To see errors during the deployment process:
* Set `ASPNETCORE_ENVIRONMENT` to Development from Azure Server. Go to App Service, Configuration, New Application Setting, and add ASPNETCORE_ENVIRONMENT to name, Development to value.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
