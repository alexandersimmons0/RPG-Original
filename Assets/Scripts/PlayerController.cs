using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPun{
    [HideInInspector]
    public int id;
    private int keyValue;

    [Header("Info")]
    public float moveSpeed;
    public int goldCount;
    public int curHp;
    public int maxHp;
    public bool dead;

    [Header("Attack")]
    public int damage;
    public int shieldStrength;
    public float attackRange;
    public float attackRate;
    private float lastAttackTime;
    private int lockDamage;
    private int lockShieldStrength;
    private float lockAttackRange;
    private float lockAttackRate;

    [Header("Components")]
    public Rigidbody2D rig;
    public Player photonPlayer;
    public SpriteRenderer sr;
    public Animator weaponAnim;
    public HeaderInfo headerInfo;
    public SpriteRenderer WeaponSprite;
    public SpriteRenderer SheildSprite;

    public static PlayerController me;

    [PunRPC]
    public void Initialize(Player player){
        id = player.ActorNumber;
        photonPlayer = player;
        GameManager.instance.players[id - 1] = this;
        headerInfo.Initialize(player.NickName, maxHp);
        if(player.IsLocal){
            me = this;
        }else{
            rig.isKinematic = true;
        }
        keyValue = 0;
        lockDamage = damage;
        lockShieldStrength = shieldStrength;
        lockAttackRange = attackRange;
        lockAttackRate = attackRate;
    }

    void Update(){
        if(!photonView.IsMine){
            return;
        }
        Move();
        if(Input.GetMouseButtonDown(0) && Time.time - lastAttackTime > attackRate){
            Attack();
        }
        float mouseX = (Screen.width / 2) - Input.mousePosition.x;
        if(mouseX < 0){
            weaponAnim.transform.parent.localScale = new Vector3(1,1,1);
        }else{
            weaponAnim.transform.parent.localScale = new Vector3(-1,1,1);
        }
    }

    void Move(){
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        rig.velocity = new Vector2(x,y) * moveSpeed;
    }

    void Attack(){
        lastAttackTime = Time.time;
        Vector3 dir = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + dir, dir, attackRange);
        weaponAnim.SetTrigger("Attack");
        if(hit.collider != null && hit.collider.gameObject.CompareTag("Enemy")){
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            enemy.photonView.RPC("TakeDamage", RpcTarget.MasterClient, damage);
        }
    }

    [PunRPC]
    public void TakeDamage(int damage){
        curHp -= (damage + shieldStrength);
        headerInfo.photonView.RPC("UpdateHealthBar", RpcTarget.All, curHp);
        if(curHp <= 0){
            Die();
        }else{
            StartCoroutine(DamageFlash());
            IEnumerator DamageFlash(){
                sr.color = Color.red;
                yield return new WaitForSeconds(0.05f);
                sr.color = Color.white;
            }
        }
    }

    [PunRPC]
    void Heal(int amountToHeal){
        curHp = Mathf.Clamp(curHp + amountToHeal, 0, maxHp);
        headerInfo.photonView.RPC("UpdateHealthBar", RpcTarget.All, curHp);
    }

    [PunRPC]
    void GiveGold(int goldToGive){
        goldCount += goldToGive;
        GameUI.instance.UpdateGoldText(goldCount);
    }

    [PunRPC]
    void GiveKey(int keyToGive){
        keyValue = keyToGive;
        GameUI.instance.UpdateHasKey(keyValue);
    }

    [PunRPC]
    void NewWeapon(int newDamage, float newAttackRange, float newAttackRate, SpriteRenderer spriteR){
        WeaponSprite.sprite = spriteR.sprite;
        damage = lockDamage;
        attackRange = lockAttackRange;
        attackRate = lockAttackRate;
        damage += newDamage;
        attackRange += newAttackRange;
        attackRate += newAttackRate;
    }

    [PunRPC]
    void NewShield(int newShieldStrength, SpriteRenderer spriteR){
        SheildSprite.sprite = spriteR.sprite;
        shieldStrength = lockShieldStrength;
        shieldStrength += newShieldStrength;
    }

    void Die(){
        dead = true;
        rig.isKinematic = true;
        transform.position = new Vector3(0,99,0);
        Vector3 spawnPos = GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)].position;
        StartCoroutine(Spawn(spawnPos, GameManager.instance.respawnTime));
    }

    IEnumerator Spawn(Vector3 spawnPos, float timeToSpawn){
        yield return new WaitForSeconds(timeToSpawn);
        dead = false;
        transform.position = spawnPos;
        curHp = maxHp;
        headerInfo.photonView.RPC("UpdateHealthBar", RpcTarget.All, curHp);
        rig.isKinematic = false;
    }
}
