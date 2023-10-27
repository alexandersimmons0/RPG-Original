using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum PickupType{
    Gold,
    Health,
    Shroom,
    Key
}

public class Pickup : MonoBehaviourPun{
    public PickupType type;
    public int value;

    void OnTriggerEnter2D(Collider2D other){
        if(!PhotonNetwork.IsMasterClient){
            return;
        }
        if(other.CompareTag("Player")){
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            GameManager gameManager = GameObject.Find("_GameManager").GetComponent<GameManager>();
            if(type == PickupType.Gold){
                player.photonView.RPC("GiveGold", player.photonPlayer, value);
            }else if(type == PickupType.Health){
                player.photonView.RPC("Heal", player.photonPlayer, value);
            }else if(type == PickupType.Shroom){
                gameManager.photonView.RPC("GiveShroom", RpcTarget.AllBuffered, value);
            }else if(type == PickupType.Key){
                player.photonView.RPC("GiveKey", player.photonPlayer, value);
            }
            photonView.RPC("DestroyPickup", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void DestroyPickup(){
        Destroy(gameObject);
    }
}
