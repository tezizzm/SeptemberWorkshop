# Exercise #3

## Goal

See how to use the Config Server for centralized configuration.

## Expected Results

ASP.NET Core microservice created and set to use new configuration provider. Service deployed to Cloud Foundry and leveraging Spring Cloud Services Configuration Server.

## Introduction

In this exercise, we get hands-on with a Git-backed conﬁg store and build an app that consumes it. Point Conﬁg Server to Git repository

1. Create a ﬁle called `config.json` with the following contents. We will use this ﬁle to tell the Conﬁg Server where to get its configurations from.  The Schwab network will not allow external access to github so we will use a Bitbucket repository on the Schwab network.  The instructor will have further directions on connecting to the Bitbucket repository

    ```json
    {
        "git": {
        "password": "<bitbucket password>",
        "uri": "https://<bitbucket Username>@bitbucket.schwab.com/scm/ad00006938/cloud-native-net-config.git",
        "username": "<bitbucket username>"
        }
    }
    ```

2. From the Terminal within Visual Studio Code, enter in the following command.

    ```command
        cf update-service <your config server name> -c config.json
    ```

3. From Apps Manager, navigate to the "Services" view, manage the Conﬁg Server instance, and notice the git URL in the conﬁguration.

## Create ASP.NET Core Web API

Here we create a brand new microservice and set it up with the Steeltoe libraries that pull conﬁguration values from our Spring Cloud Services Conﬁg Server.

1. In the Visual Studio Code Terminal window, navigate up one level and create a new folder (bootcamp-webapi) to hold our data microservice.

2. From within that folder, run the `dotnet new webapi` command in the Terminal. This uses the "web api" template to create scaffolding for a web service.

3. From the Terminal enter

    ```Windows
        dotnet add package Pivotal.Extensions.Configuration.ConfigServerCore --version 2.0.0
    ```

4. Open the newly created folder in Visual Studio Code.

5. In the `Controllers` folder, create a new ﬁle named `ProductsController.cs`.

6. Enter the following code. It deﬁnes a new API controller, speciﬁes the route that handles, it, and an operation we can invoke.

    ```cs

        using System;
        using System.Collections.Generic;
        using Microsoft.AspNetCore.Mvc;
        using Microsoft.Extensions.Configuration;

        namespace bootcamp_webapi.Controllers
        {
            [Route("api/[controller]")]
            public class ProductsController : Controller
            {
                private readonly IConfigurationRoot _config;
                public ProductsController(IConfigurationRoot config)
                {
                    _config = config;
                }

                // GET api/products
                [HttpGet]
                public IEnumerable<string> Get()
                {
                    Console.WriteLine($"connection string is {_config["productdbconnstring"]}");
                    Console.WriteLine($"Log level from config is {_config["loglevel"]}");
                    return new[] {"product1", "product2"};
                }
            }
        }
    ```

## Connect application to Conﬁg Server

Next, we add what's needed to make our ASP.NET Core application retrieve it's conﬁguration from Cloud Foundry and the Conﬁg Server.

1. Edit `appsettings.json` to include the application name and cloud conﬁg name. This maps to the conﬁguration ﬁle read from the server.

    ```json
    {
        "Logging": {
            "IncludeScopes": false,
            "LogLevel": {
                "Default": "Warning"
            }
       + },
       +  "spring": {
       +    "application": {
       +     "name": "branch1"
       +   },
          // determines the name of the files pulled; explicitly set to avoid
          // env variable overwriting it
       +   "cloud": {
       +     "config": {
       +       "name": "branch1"
       +     }
       +   }
        }
    }

    ```

2. In `Program.cs`, set up conﬁguration provider to use the config server

    ```cs
        // ...
    +   using Pivotal.Extensions.Configuration.ConfigServer;

        namespace bootcamp_webapi
        {
            public class Program
            {
                public static void Main(string[] args)
                {
                    CreateWebHostBuilder(args).Build().Run();
                }

                public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
                    WebHost.CreateDefaultBuilder(args)
                    +   .UseCloudFoundryHosting()
                    +   .ConfigureAppConfiguration(b => b.AddConfigServer(new LoggerFactory().AddConsole(LogLevel.Trace)))
                        .UseStartup<Startup>();
            }
        }
    ```

3. In `Startup.cs`, add conﬁguration to the service container, allowing it to be injected into the `ProductsController`.

    ```cs
        // ...
    +  using Pivotal.Extensions.Configuration.ConfigServer;

        namespace bootcamp_webapi
        {
            public class Startup
            {
                // ...

                public void ConfigureServices(IServiceCollection services)
                {
                    // ...

            +       services.AddConfiguration(Configuration);

                    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
                }

                // ...
            }
        }
    ```

4. Add a `manifest.yml` ﬁle to the base of the project. This tells Cloud Foundry how to deploy your app. Enter:

    ```yml
    ---
    applications:
    - name: core-cf-microservice-<enter your name>
      buildpack: dotnet_core_buildpack_2_1_3
      instances: 1
      path: .\publish
      memory: 256M
      # determines which environment to pull configs from
      env:
        ASPNETCORE_ENVIRONMENT: dev
      services:
        - <your config server instance name>
    ```

5. Build your application using the following command.  `dotnet publish -o .\publish`.

6. Execute `cf push` to deploy this application to Cloud Foundry! Note the route of your microservice:

    ```command
    Waiting for app to start...

    name:              core-cf-yourname
    requested state:   started
    instances:         1/1
    usage:             256M x 1 instances
    routes:            core-cf-yourname.apps.chicken.pez.pivotal.io
    last uploaded:     Wed 28 Dec 09:19:42 MDT 2018
    stack:             cflinuxfs2
    buildpack:         https://github.com/cloudfoundry/dotnet-core-buildpack
    start command:     cd ${DEPS_DIR}/0/dotnet_publish && ./mvctest --server.urls http://0.0.0.0:${PORT}
    ```

## Observe behavior when changing application name or label

See how conﬁgurations are pulled and used.

1. Navigate to the `/api/products` endpoint of your microservice and seeing a result.

2. Go to the "Logs" view in Apps Manager and see the connection string and log level written out.

3. Go back to the source code and change the application name and cloud conﬁg name in the `appsettings.json` to "branch3", and in the `manifest.yml` change the environment to "qa." This should resolve to a different conﬁguration ﬁle in the GitHub repo, and load different values into the app's conﬁguration properties.

4. Re-deploy the app ( `cf push` ), hit the API endpoint, and observe the different values logged out.