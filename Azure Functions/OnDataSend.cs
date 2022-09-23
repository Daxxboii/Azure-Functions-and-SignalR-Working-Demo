using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;

namespace Azure_Functions
{
    public static class OnDataSend
    {
        [FunctionName("OnDataSend")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            //SignalR
            HubConnection connection = new HubConnectionBuilder().WithUrl("https://deployingsignalrserver.azurewebsites.net/Gamehub").Build();
            await connection.StartAsync();

            await connection.InvokeAsync<string>("SendDataToAll", requestBody);

            await connection.StopAsync();

            return new OkObjectResult("Successful , Recieved");
        }
    }
}
