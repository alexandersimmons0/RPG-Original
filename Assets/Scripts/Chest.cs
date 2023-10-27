using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Chest : MonoBehaviourPun{
    public bool HighChest;
    public GameObject[] weaponWheel;
    private PlayerController player;
    private GameObject selected;

    void OnTriggerEnter2D(Collider2D other){
        player = other.gameObject.GetComponent<PlayerController>();
    }

    void OnTriggerStay2D(Collider2D other){
        if(HighChest){
            if(Input.GetKey(KeyCode.E) && player.goldCount >= 25){

            }
        }else{
            if(Input.GetKey(KeyCode.E) && player.goldCount >= 10){
                player.photonView.RPC("GiveGold", player.photonPlayer, (-10));
                CreateItem();
            }
        }
    }

    [PunRPC]
    void CreateItem(){
        selected = weaponWheel[Random.Range(0, weaponWheel.Length)];
        Instantiate(selected, this.transform.position, Quaternion.identity);
        BreakChest();
    }

    [PunRPC]
    public void BreakChest(){
        Destroy(gameObject);
    }
}
