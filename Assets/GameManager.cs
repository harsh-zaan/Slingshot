using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPun
{
    public PlayerController leftPlayer;
    public PlayerController rightPlayer;

    public PlayerController currentPlayer;
    [SerializeField] private float _postGameTime;    // time to return to the menu when the game is over
    // instance
    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // sets master player selection
        if (PhotonNetwork.IsMasterClient)
            SetPlayers();
    }

    // Generates player data and unit spawns
    void SetPlayers()
    {
        // assigns photon views to defined players
        leftPlayer.photonView.TransferOwnership(1);
        rightPlayer.photonView.TransferOwnership(2);

        // Creates the players
        leftPlayer.photonView.RPC("Initialize", RpcTarget.AllBuffered, PhotonNetwork.CurrentRoom.GetPlayer(1));
        rightPlayer.photonView.RPC("Initialize", RpcTarget.AllBuffered, PhotonNetwork.CurrentRoom.GetPlayer(2));

        // starts the first player's turn
        photonView.RPC("SetNextTurn", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void SetNextTurn()
    {
        // first round check
        if (currentPlayer == null)
            currentPlayer = leftPlayer;
        else
            currentPlayer = currentPlayer == leftPlayer ? rightPlayer : leftPlayer;

        // is it our turn
        if (currentPlayer == PlayerController.me)
        {
            PlayerController.me.BeginTurn();
        }

        // if it's our turn, it opens the end turn button
        //GameUI.instance.ToggleEndTurnButton(currentPlayer == PlayerController.me);
    }

    // Selects the other player
    public PlayerController GetOtherPlayer(PlayerController player)
    {
        return player == leftPlayer ? rightPlayer : leftPlayer;
    }

    // Summoned by the player when her unit dies
    // Sent to other enemy by RPC
    public void CheckWinCondition()
    {
        
    }

    // Summoned when all units of the other player are killed
    [PunRPC]
    void WinGame(int winner)
    {
        // get the winning player
        PlayerController player = winner == 0 ? leftPlayer : rightPlayer;

        // set win text
        GameUI.instance.SetWinText(player.photonPlayer.NickName);

        // return to menu after specified time
        Invoke("GoBackToMenu", _postGameTime);
    }

    // return to menu
    void GoBackToMenu()
    {
        PhotonNetwork.LeaveRoom();
        NetworkManager.instance.ChangeScene("Menu");
    }
}