# Exercise #3

## Goal

Explore service registration and discovery practices.

## Expected Results

Register existing microservice with a service registry. Update a client application so that it discovers the microservice and uses it.

## Introduction

This exercise helps us understand how to register our microservices with the Spring Cloud Services Registry, and also discover those services at runtime. Update previous service to register itself with the Service Registry Modify our products microservice to automatically register itself upon startup.

1. In the existing microservice project, add a Nuget package dependency by entering the following command into the Terminal.

    ```Windows
    dotnet add package Pivotal.Discovery.ClientCore --version 2.1.0
    ```

    *If you weren’t using Spring Cloud Services, you could use a vanilla Steeltoe package for service discovery: **Steeltoe.Discovery.ClientCore**.*

2. Open the `appsettings.json` ﬁle and add this block after the "spring" block:

    ```json
    {
        "Logging": {
            "IncludeScopes": false,
            "LogLevel": {
                "Default": "Warning"
                }
            },
            "spring": {
                "application": {
                    "name": "branch1"
                    },
                    "cloud": {
                        "config": {
                            "name": "branch1"
                            }
                        }
                    },
                    + "eureka": {
                        +   "client": {
                            +     "shouldRegisterWithEureka": true,
                            +     "shouldFetchRegistry": true
                        +   }
                        + }
                    }
    ```

3. In `Startup.cs`, add the discovery client to the service container.

    ```cs
    // ...
   + using Pivotal.Discovery.Client;

    namespace bootcamp_webapi
    {
        public class Startup
        {
            // ...

            public IConfiguration Configuration { get; }

            // This method gets called by the runtime. Use this method to add services to the containe
            public void ConfigureServices(IServiceCollection services)
            {
                // ...

                services.AddConfiguration(Configuration);
        +       services.AddDiscoveryClient(Configuration);

                services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            }

            // ...
        }
    }

    ```

4. In the same class, update the `Conﬁgure` method to use the discovery client.

    ```cs
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseMvc();
    +   app.UseDiscoveryClient();
    }
    ```

5. Update `manifest.yml` so that the microservice also binds to the `Service Registry` upon push.

    ```yml
    ---
    applications:
    - name: core-cf-microservice-<enter your application name>
        instances: 1
        memory: 256M
        # determines which environment to pull configs from
        env:
        ASPNETCORE_ENVIRONMENT: qa
        services:
        - <your configuration service name>
    +   - <your registry name
    ```

6. Push the application ( `cf push` ). Go "manage" the `Service Registry` instance from within Apps Manager. Notice our service is now listed!

## Download and open up front-end ASP.NET Core application

Conﬁgure a pre-built front end app so that it discovers our products microservice.

1. Go to <https://github.com/tezizzm/cloud-native-net-sampleui> to download the pre-built front-end application.

2. Download the code into a folder on your machine.

3. Open the project in Visual Studio Code.

4. Observe in `bootcamp-core-ui.csproj` that the project references the discovery package.  Edit the `bootcamp-core-ui.csproj` file by setting the target framework to `netcoreapp2.1`.

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>[SET_TARGET_FRAMEWORK]</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <Folder Include="wwwroot\" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore" Version="1.1.1" />
    <PackageReference Include="Microsoft.AspNetCore.Mvc" Version="1.1.3" />
    <PackageReference Include="Microsoft.Extensions.Configuration" Version="1.1.2" />
    <PackageReference Include="Pivotal.Discovery.Client" Version="1.0.1" />
    <PackageReference Include="Steeltoe.Extensions.Configuration.CloudFoundry" Version="1.1.0" />
    <PackageReference Include="System.Net.Http" Version="4.3.2" />
  </ItemGroup>
</Project>
```

5. See in `appsettings.json` that this app does NOT register itself with Eureka, but just pulls the registry. Also see in the `Startup.cs` ﬁle that it loads the discovery service.

## Update front-end application to pull services from the registry and use them

Replace placeholder values so that your app talks to your own microservice.

1. Open the `HomeController.cs` ﬁle in the Controllers folder.

2. Go to the `Index()` operation and replace the placeholder string with the microservice’s application name (not the url) from your `Service Registry`.

    ```cs
    [Route("/home")]
    public IActionResult Index()
    {
        var client = GetClient();
    +   var result = client.GetStringAsync("https://<replace me>/api/products").Result;
        ViewData["products"] = result;
        return View();
    }
    ```

3. In `manifest.yml`, replace the "application name" and "service registry name" placeholders.

4. Push your application to Cloud Foundry ( `cf push` ).

5. Go to the `/Home` endpoint of your application. You should see the web page with products retrieved from your data microservices.