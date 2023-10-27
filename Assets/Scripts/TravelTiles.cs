using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelTiles : MonoBehaviour{
    public Vector3 location;
    private GameObject player;
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            player = other.gameObject;
            player.transform.position = location;
        }
    }
}