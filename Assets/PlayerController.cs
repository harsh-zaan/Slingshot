using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPun
{
    public Player photonPlayer;

    public Bow playerBow;

    public static PlayerController me;          // local player
    public static PlayerController enemy;       // enemy player (non-local)

    // Called when the game starts
    [PunRPC]
    void Initialize(Player player)
    {
        photonPlayer = player;

        if (player.IsLocal)
        {
            me = this;
        }
        else
            enemy = this;

    }

    public void EndTurn()
    {
        me.playerBow.canShoot = false;
        enemy.playerBow.canShoot = true;
        GameManager.instance.photonView.RPC("SetNextTurn",RpcTarget.All);
    }

    // called when it's our turn
    public void BeginTurn()
    {
        me.playerBow.canShoot = true;
        enemy.playerBow.canShoot = false;
    }
}