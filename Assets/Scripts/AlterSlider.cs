using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AlterSlider : MonoBehaviourPun{
    public Animator slideAnim;
    public GameManager gameManager;
    private PlayerController player;
    private bool hasSlid = false;
    
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            player = other.gameObject.GetComponent<PlayerController>();
        }
    }


    [PunRPC]
    void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            if(Input.GetKey(KeyCode.E) && !hasSlid && gameManager.shroomCount >= 10){
                hasSlid = true;
                slideAnim.SetTrigger("Slide");
                gameManager.GiveShroom(-10);
            }
        }
    }
}
