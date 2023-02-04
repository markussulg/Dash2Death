using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class testLobby : MonoBehaviour
{

    private Lobby hostLobby;
    private float heartbeatTimer;
    private string playerName;

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        playerName = "Dash2Death" + UnityEngine.Random.Range(10, 99);
        Debug.Log(playerName);
    }
    private void Update()
    {
        HandleLobbyHeartbeat();      
    }

    private async void HandleLobbyHeartbeat()
    {
        if(hostLobby != null)
        {
            heartbeatTimer -= Time.deltaTime;
            if(heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    private async void CreateLobby()
    {
        try { 
            string lobbyName = "MyLobby";
            int maxPlayers = 4;
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                Player = getPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, "PVP") }
                }
            };
            Lobby lobby =  await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);

            hostLobby = lobby;

            PrintPlayers(hostLobby);

            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers);
        }catch(LobbyServiceException e) {
            Debug.Log(e);
        }
        
    }

    private async void ListLobbies()
    {
        try {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder>
                {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach(Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        }catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = getPlayer(),
            };
            Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            Debug.Log("Joined lobby with code: " + lobbyCode);
        }catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
        
    }

    private async void QuickJoinLobby()
    {
        try
        {
            await LobbyService.Instance.QuickJoinLobbyAsync();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
        
    }

    private Player getPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
                    {
                        {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
                    }
        };
    }

    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Players in lobby " + lobby.Name);
        foreach(Player player in lobby.Players)
        {
            Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
        }
    }
}
