## The documentation of our application
To create a documentation, we can use the package Swashbuckle.AspNetCore. To install it, run

$ dotnet add package Swashbuckle.AspNetCore -v 5.0.0-rc4

Then we must configure swagger on Startup.cs, and it will serve our documentation.