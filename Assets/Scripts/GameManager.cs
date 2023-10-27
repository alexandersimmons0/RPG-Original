using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class GameManager : MonoBehaviourPun{
    [Header("Players")]
    public string playerPrefabPath;
    public Transform[] spawnPoints;
    public PlayerController[] players;
    public float respawnTime;

    private int playersInGame;
    public int shroomCount;
    public static GameManager instance;

    void Awake(){
        instance = this;
    }

    void Start(){
        players = new PlayerController[PhotonNetwork.PlayerList.Length];
        photonView.RPC("ImInGame", RpcTarget.AllBuffered);
    }

    void SpawnPlayer(){
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabPath, spawnPoints[Random.Range(0,spawnPoints.Length)].position, Quaternion.identity);
        playerObj.GetComponent<PhotonView>().RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }

    [PunRPC]
    void ImInGame(){
        playersInGame++;
        if(playersInGame == PhotonNetwork.PlayerList.Length){
            SpawnPlayer();
        }
    }

    [PunRPC]
    void GiveShroom(int value){
        shroomCount += value;
        GameUI.instance.UpdateShroomText(shroomCount);
    }
}
