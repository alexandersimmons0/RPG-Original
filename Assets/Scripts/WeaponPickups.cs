using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponPickups : MonoBehaviourPun{
    public int damage;
    public float attackRange;
    public float attackRate;
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
            player.photonView.RPC("NewWeapon", player.photonPlayer, damage, attackRange, attackRate, spriteR);
            DestroyPickup();
        }
    }

    [PunRPC]
    public void DestroyPickup(){
        Destroy(gameObject);
    }
}
