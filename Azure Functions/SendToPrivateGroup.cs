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

namespace DemoServer.SendToPrivateGroup
{
    public static class SendToPrivateGroup
    {
        [FunctionName("SendToPrivateGroup")]
       public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            dynamic DeserializedData = JsonConvert.DeserializeObject<dynamic>(requestBody);
            dynamic NestedEventData = JsonConvert.DeserializeObject(DeserializedData.PlayStreamEventEnvelope.EventData.ToString());

            string GroupName = NestedEventData.GroupName;

            //SignalR
            HubConnection connection = new HubConnectionBuilder().WithUrl("https://deployingsignalrserver.azurewebsites.net/Gamehub").Build();
            await connection.StartAsync();

            await connection.InvokeAsync<string>("SendDataToGroup", requestBody,GroupName);

            await connection.StopAsync();

            return new OkObjectResult(requestBody);
        }
    }
}
