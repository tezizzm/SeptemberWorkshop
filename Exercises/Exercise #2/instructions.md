# Exercise #2

## Goal

Create Services through Aps Manager and the CF CLI

## Expected Results

Spring Cloud Services instances of Configuration Server and Service Registry deployed to Cloud Foundry

## Introduction

In this exercise, we'll continue setting up our cloud environment so that we're ready to build and run modern .NET applications.

## Instantiate Spring Cloud Services instances

Spring Cloud Services wrap up key Spring Cloud projects with managed capabilities. Here we create a pair of these managed services.

1. In Pivotal Cloud Foundry Apps Manager, click on your "space" on the left, and switch to the "Services" tab.

2. Click "Add Service."

3. Type "Spring" into the search box to narrow down the choices.

4. Select "Service Registry" and select the default plan.

5. Provide an instance name and do not choose to bind the service to any existing applications. Click "Create." This service will take a couple of minutes to become available.

6. Run the command cf marketplace and note the name of the Config Server (p-config-server) as well as the standard service plan name (standard).

7. Run the command to create a service from the command line.  The command should have the following format `cf create-service [service-type] [service-plan] [service-name]`.  And here's an example `cf create-service p-config-server standard myConfigServer`.

8. Type `cf services` to view the service instances created in your current space.  Note that as the service instance you created in the prior step is being created you will see a "create in progress" message for the status.

9. Return to your default space in Apps Manager, click "Services", choose Service Registry, and click the "manage" link. This takes you to the Eureka dashboard.

10. Return again to the default space, click "Service", choose Conﬁg Server, and click the "manage" link.