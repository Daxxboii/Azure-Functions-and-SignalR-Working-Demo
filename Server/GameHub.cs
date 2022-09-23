using Microsoft.AspNetCore.SignalR;

public class GameHub : Hub{


    //Called From Azure Functions
    public async Task SendDataToAll(string data){
        await Clients.All.SendAsync("Data", data);
    }

    public async Task SendDataToSelf(string data,string ClientID)
    {
        await Clients.Client(ClientID).SendAsync("PrivateData",data);
    }

    public async Task SendDataToGroup(string data,string GroupName)
    {
        await Clients.Group(GroupName).SendAsync("PrivateGroupData", data);
    }

    //Called From Clients
    public async Task AddToGroup(string ChannelName){
        await Groups.AddToGroupAsync(Context.ConnectionId, ChannelName);
    }

    public async Task RemoveFromGroup(string ChannelName){
         await Groups.RemoveFromGroupAsync(Context.ConnectionId, ChannelName);
    }

    public async Task SendSignalRIDToClient(){
        await Clients.Client(Context.ConnectionId).SendAsync("SignalRID",Context.ConnectionId);
    }
}