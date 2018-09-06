# Exercise #4

## Goal

Check out Cloud Foundry support for .NET applications.

## Expected Results

The ASP.NET Core microservice scales automatically, recovers from failures, and emits logs that the platform aggregates and displays

## Introduction

It’s fun to try out the platform itself and see how it works in certain situations. Here we trigger autoscaling, and observe failure recovery. Conﬁgure and test out autoscaling policies Autoscaling is a key part of any solid platform. Here we create a policy, and trigger it!

1. Download the .NET Core project located at <https://github.com/platform-acceleration-lab/cloud-native-net-loadtest>.

2. Open the project in Visual Studio Code.

3. Update the `Program.cs` ﬁle with a pointer to your API microservice URL.

4. In Apps Manager, go to your microservice's overview page.

5. Click on the autoscaler button to enable for your microservice.

6. Click the "manage" link to set up a scaling policy.

7. Click "edit" on Instance Limits and set it to have a minimum of 1, and maximum of 3.

8. Edit the scaling rules and set an HTTP Throughput policy that scales down if less than 1 request per second, and scales up if more than 4 requests per second.

9. Enable the policy by sliding the toggle at the top of the policy deﬁnition. Save the policy.

10. Start the "load test" .NET Core project on your machine that repeatedly calls your microservice. Start it by entering dotnet run in the Terminal while pointing at that application folder.

11. On the overview page of your microservice in Apps Manager, observe a second instance come online shortly. This is in response to the elevated load, and your autoscaling policy kicking in. Also notice a new "event" added to the list.

12. Stop the load testing app `(Ctrl+C)`, and watch the application scale back down to a single instance within 30 seconds.

## Update microservice with new "fault" endpoint and logs

Let’s add a new microservice endpoint that purposely crashes our app. We can see how the platform behaves when an instance disappears.

1. Return to your products API microservice in Visual Studio Code.

2. Create a new controller named `FailureController.cs` with the following content:

    ```cs
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    namespace bootcamp_webapi.Controllers
    {
        [Route("api/[controller]")]
        public class FailureController : Controller
        {
            public FailureController(IConfigurationRoot config)
            {
                Config = config;
            }

            private IConfigurationRoot Config { get; set; }

            // GET api/failure
            [HttpGet]
            public ActionResult TriggerFailure(string action)
            {
                System.Console.WriteLine("bombing out ...");
                //purposely crash
                System.Environment.Exit(100);
                return null;
            }
        }
    }
    ```

3. Deploy the updated service to Cloud Foundry.

4. Conﬁrm that "regular" URL still works, by navigating to the `/api/products` endpoint of your microservice.

5. Now send a request to the `/api/failure` endpoint.

6. See in Applications Manager that the app crashes, and the platform quickly recovers and spins up a new instance.

7. Visit the "logs" view and see that logs were written out.
