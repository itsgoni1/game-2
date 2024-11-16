using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public GameObject PlayerPrefab;

    public NetworkVariable<ulong> ActivePlayerId = new NetworkVariable<ulong>(0); // Tracks whose turn it is
    public NetworkVariable<int> Player1Choice = new NetworkVariable<int>(-1);
    public NetworkVariable<int> Player2Choice = new NetworkVariable<int>(-1);


    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            SpawnPlayers();
            SetFirstPlayerTurn();
        }
    }

    private void SpawnPlayers()
    {
        foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var playerInstance = Instantiate(PlayerPrefab);
            playerInstance.GetComponent<NetworkObject>().SpawnWithOwnership(client.ClientId);
        }
    }

    private void SetFirstPlayerTurn()
    {
        // Assign the first turn to the first connected player
        ActivePlayerId.Value = NetworkManager.Singleton.ConnectedClientsList[0].ClientId;
    }

    public void SetPlayerChoice(ulong playerId, int choice)
    {
        if (playerId == ActivePlayerId.Value)
        {
            if (playerId == NetworkManager.Singleton.ConnectedClientsList[0].ClientId)
            {
                Player1Choice.Value = choice;
            }
            else
            {
                Player2Choice.Value = choice;
            }

            AdvanceTurn();
        }
    }

    private void AdvanceTurn()
    {
        // Switch turns if one player has made their choice
        if (Player1Choice.Value == -1 || Player2Choice.Value == -1)
        {
            ActivePlayerId.Value = (ActivePlayerId.Value == NetworkManager.Singleton.ConnectedClientsList[0].ClientId)
                ? NetworkManager.Singleton.ConnectedClientsList[1].ClientId
                : NetworkManager.Singleton.ConnectedClientsList[0].ClientId;
        }
        else
        {
            DetermineWinner();
        }
    }

    private void DetermineWinner()
    {
        int result = (3 + Player1Choice.Value - Player2Choice.Value) % 3;

        if (result == 0)
        {
            Debug.Log("Draw!");
        }
        else if (result == 1)
        {
            Debug.Log("Player 1 Wins!");
        }
        else
        {
            Debug.Log("Player 2 Wins!");
        }

        ResetGame();
    }

    private void ResetGame()
    {
        Player1Choice.Value = -1;
        Player2Choice.Value = -1;
        SetFirstPlayerTurn();
    }
}
