using Microsoft.AspNetCore.SignalR;

public class GameHub : Hub{
    public Dictionary<string,string> OnlinePlayers = new Dictionary<string,string>();


    //Called From Azure Functions
    public async Task SendDataToAll(string data){
        await Clients.Group("Public").SendAsync("Data",data);
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
    public async Task AddToChannel(string ChannelName,string PlayerName){
        OnlinePlayers.Add(Context.ConnectionId, PlayerName);
        await Groups.AddToGroupAsync(Context.ConnectionId, ChannelName);
    }

    public async Task RemoveFromChannel(string ChannelName){
        OnlinePlayers.Remove(Context.ConnectionId);
         await Groups.RemoveFromGroupAsync(Context.ConnectionId, ChannelName);
    }

    public async Task SendSignalRIDToClient(){
        await Clients.Client(Context.ConnectionId).SendAsync("SignalRID",Context.ConnectionId);
    }
}