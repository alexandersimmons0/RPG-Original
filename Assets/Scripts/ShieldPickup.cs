using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShieldPickup : MonoBehaviourPun{
    public int shieldStrength;
    private SpriteRenderer spriteR;
    private PlayerController player;

    void OnTriggerEnter2D(Collider2D other){
        spriteR = GetComponent<SpriteRenderer>();
        if(other.gameObject.CompareTag("Player")){
            player = other.gameObject.GetComponent<PlayerController>();
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if(Input.GetKey(KeyCode.E) && other.gameObject.CompareTag("Player")){
            player.photonView.RPC("NewShield", player.photonPlayer, shieldStrength, spriteR);
            DestroyPickup();
        }
    }

    [PunRPC]
    public void DestroyPickup(){
        Destroy(gameObject);
    }
}
