# Azure-Functions-and-SignalR-Working-Demo

- The project is split into two different parts , Azure Functions and SignalR using Unity 


## What it does?
-  Authenticates and logs in/Signs up a user via Playfab SDK in Unity
- Registers the user as a SignalR client in the GameHub(found in server Files)
- Triggers a playstream event in playfab 
- The Azure Functions catch the playstream events and relay the information to other clients 
> Note : Depending on the Data being passed through the playstream event , the target clients can be global (all registered online users) OR a target group

## Prerequisites

- Switching the project to .Net Framework (4.0x)
