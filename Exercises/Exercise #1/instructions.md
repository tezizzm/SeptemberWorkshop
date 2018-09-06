# Exercise #1

## Goal

Set up environment and push your first .NET Core and .NET Framework applications.

## Expected Results

Laptop configured with Cloud Foundry CLI, Visual Studio Code, .NET Core SDK. PWS account created and Spring Cloud Services instance deployed. ASP.NET core microservice created and deployed to Cloud Foundry.

## Introduction

In this exercise, we'll set up our workstation and cloud environment so that we're ready to build and run modern .NET applications. Your instructor will provide the url and credentials for the Pivotal Cloud Foundry (PCF) instance you will be using.

Alternatively, you can sign up for a trial account of the hosted version of PCF, called Pivotal Web Services:

1. Go to <http://run.pivotal.io> and choose "sign up for free."

2. Click "create account" link on sign up page.

3. Fill in details.

4. Go to email account provided and click on veriﬁcation email link.

5. Click on "claim free trial" link and provide phone number.

6. Validate your account and create your organization.

## Package manager

We suggest using a package manager to install bootcamp software.  Alternatively use the lastest release for your given operating system found at: <https://github.com/cloudfoundry/cli/releases>

- MacOS: Homebrew\
 ( brew search PACKAGE to search)

- Windows: Chocolatey\
 ( choco search PACKAGE to search)

- Debian-Based Linux: Apt\
 ( apt search PACKAGE to search)

- Fedora-Based Linux: Yum\
 ( yum search PACKAGE to search)

## Install Cloud Foundry CLI

You can interact with Cloud Foundry via Dashboard, REST API, or command line interface (CLI). Here, we install the CLI and ensure it's conﬁgured correctly.

- Windows:

    ```Windows
    choco install cloudfoundry-cli
    ```

- MacOS:

    ```command
    brew install cloudfoundry/tap/cf-cli
    ```

- Debian and Ubuntu:

    ```Linux
    wget -q -O https://packages.cloudfoundry.org/debian/cli.cloudfoundry.org.key
    sudo apt-key add echo "deb https://packages.cloudfoundry.org/debian stable main"
    sudo tee /etc/apt/sources.list.d sudo apt-get update sudo apt-get install cf-cli
    ```

- RHEL, CentOS, and Fedora:

    ```Linux
    sudo wget -O /etc/yum.repos.d/cloudfoundry-cli.repo https://packages.cloudfoundry.org/fedora/cloud`
    sudo yum install cf-cli
    ```

Conﬁrm that it installed successfully by going to a command line, and typing:

```Windows
    cf -v
```

## Install .NET Core and Visual Studio Code

.NET Core represents a modern way to build .NET apps, and here we make sure we have everything needed to build ASP.NET Core apps.

1. Visit <https://www.microsoft.com/net/download> to download and install the latest version of the .NET Core Runtime and SDK.

    ***.NET Core Version 2.1.x is required for this workshop***

2. Conﬁrm that it installed correctly by opening a command line and typing:

    ```Windows
    dotnet --version
    ```

3. Install Visual Studio Code using your favorite package manager.

4. Open Visual Studio Code and go to `View → Extensions`

5. Search for C# and choose the top C# for Visual Studio Code option and click `Install`. This gives you type-ahead support for C#.

## Create an ASP.NET Core project

Visual Studio Code makes it easy to build new ASP.NET Core projects. We'll create a sample project just to prove we can!

1. Within Visual Studio Code, go to `View → Integrated Terminal`. The Terminal gives you a shell interface without leaving Visual Studio Code.

2. Navigate to a location where you'll store your project ﬁles (e.g. C:\BootcampLabs) and create a sub-directory called "mvctest" inside.

3. Navigate into the newly created "mvctest" directory and type in dotnet new mvc to create a new ASP.NET Core MVC project.

4. In Visual Studio Code, click `File → Open` and navigate to the directory containing the new ASP.NET Core project.

5. Observe the ﬁles that were automatically generated. Re-open the Terminal window.

6. Start the project by typing `dotnet run` and visiting <http://localhost:5000>. To stop the application, enter `Ctrl+C`.

## Deploy ASP.NET Core application to Cloud Foundry

Let's push an app! Here we'll experiment with sending an application to Cloud Foundry.

1. In Visual Studio Code, go to View → Extensions.

2. Search for "Cloudfoundry" and install Cloudfoundry Manifest YML support extension. This gives type-ahead support for Cloud Foundry manifest files.

3. In Visual Studio Code, create a new ﬁle called `manifest.yml` at base of your project.

4. Open the `manifest.yml` ﬁle, and type in the following (notice the typing assistance from the extension):

    ```yml
        ---
        applications:
          - name: core-cf-[enter your name]
            buildpack: https://github.com/cloudfoundry/dotnet-core-buildpack
            instances: 1
            memory: 256M
    ```

5. In the Terminal, type in `cf login -a <PCF API url>` and provide your credentials. Now you are connected to Pivotal Cloud Foundry.

6. Enter `cf push` into the Terminal, and watch your application get bundled up and deploy to Cloud Foundry.

7. In Pivotal Cloud Foundry Apps Manager, see your app show up, and visit the app’s URL.

8. Use git to clone the following repository https://github.com/tezizzm/pcf-dotnet-environment-viewer.git.

9. Navigate to the cloned directory and open the solution file in Visual Studio.  Open the manifest file and change the name of your application to something unique.

10. Publish the project to a folder at the root of your application named publish.

11. Navigate to the publish output directory and Enter `cf push` into the terminal.  Notice once again your application get bundled up and deployed to Cloud Foundry.

12. See if you can spot any differences in the output when executing the `cf push` command in step 6 and step 10.