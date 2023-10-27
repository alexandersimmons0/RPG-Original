using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI shroomText;
    public static GameUI instance;
    public GameObject shroomImage;
    public GameObject keyImage;

    void Awake(){
        instance = this;
        shroomImage.SetActive(false);
    }

    public void UpdateGoldText(int gold){
        goldText.text = "<b>Gold: </b>" + gold;
    }

    public void UpdateShroomText(int shroom){
        if(shroom > 0){
            shroomImage.SetActive(true);
        }
        shroomText.text = "" + shroom;
    }

    public void UpdateHasKey(int key){
        if(key == 1){
            keyImage.SetActive(true);
        }else{
            keyImage.SetActive(false);
        }
    }
}
