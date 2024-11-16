using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public NetworkVariable<int> PlayerChoice = new NetworkVariable<int>(-1);

    public void MakeChoice(int choice)
    {
        if (IsOwner) // Ensure only the local player can make a choice
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager.ActivePlayerId.Value == OwnerClientId) // Check if it's this player's turn
            {
                PlayerChoice.Value = choice;
                gameManager.SetPlayerChoice(OwnerClientId, choice);
            }
            else
            {
                Debug.Log("Not your turn!");
            }
        }
    }
}
