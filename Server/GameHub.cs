using Microsoft.AspNetCore.SignalR;

public class GameHub : Hub{
    public Dictionary<string,string> OnlinePlayers = new Dictionary<string,string>();


    public async Task SendData(string data){
        await Clients.Group("Public").SendAsync("Data",data);
    }

    public async Task AddToChannel(string ChannelName,string PlayerName){
        OnlinePlayers.Add(Context.ConnectionId, PlayerName);
        await Groups.AddToGroupAsync(Context.ConnectionId, ChannelName);
    }

    public async Task RemoveFromChannel(string ChannelName){
        OnlinePlayers.Remove(Context.ConnectionId);
         await Groups.RemoveFromGroupAsync(Context.ConnectionId, ChannelName);
    }
}