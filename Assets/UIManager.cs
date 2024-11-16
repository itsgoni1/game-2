using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text player1ChoiceText;
    public Text player2ChoiceText;
    public Text resultText;
    public Text turnText;

    public GameManager gameManager;

    public void UpdateChoices()
    {
        player1ChoiceText.text = GetChoiceName(gameManager.Player1Choice.Value);
        player2ChoiceText.text = GetChoiceName(gameManager.Player2Choice.Value);
    }

    public void DisplayResult(string result)
    {
        resultText.text = result;
    }

    private string GetChoiceName(int choice)
    {
        switch (choice)
        {
            case 0: return "Rock";
            case 1: return "Paper";
            case 2: return "Scissors";
            default: return "Waiting...";
        }
    }

    public void UpdateTurn(ulong activePlayerId)
    {
        turnText.text = activePlayerId == NetworkManager.Singleton.LocalClientId
            ? "Your Turn"
            : "Opponent's Turn";
    }
}
