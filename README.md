# blog-web-application
A blog web application that allows users with different access levels to make changes to the blog. Used Microsoft SQL for the local database. ASP .NET Core 7.0, C#, and HTML-related skillsets were used.  

*To use Claudinary*  
Create account in Claudinary, and copy and paste your CloudName, ApiKey, ApiSecret to appsetting.json  

*To deploy to Azure DB*  
Create Azure Db, connect to connectionstring in appsetting.json.
Move Dbsets from local database to azure database by right click-task-

Change connectionstring of appsetting.json  
To see errors during the deployment process, set ASPNETCORE_ENVIRONMENT to Development from Azure Server. 
Steps: Go to App Service, Configuration, press New Application Setting, and add ASPNETCORE_ENVIRONMENT to name, Development to value.  
