using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public int playerHp = 200;
    public GameObject player;
    [SerializeField]
    Animator playerAni;
    public SpriteRenderer playerRender;

    private void Awake() {
        instance = this;
    }
    void Start()
    {
        playerRender = GetComponent<SpriteRenderer>();
        player = this.gameObject;
        playerAni = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        int enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
        Debug.DrawRay(transform.position, Vector2.right * 1.2f, Color.red);
        RaycastHit2D enemyHit = Physics2D.Raycast(transform.position, Vector2.right, 1.2f, enemyLayer);
        if(enemyHit)
        {
            playerAni.SetBool("isAtk", true);
            playerAni.SetBool("isRun", false);
        }
        if(!enemyHit)
        {
            playerAni.SetBool("isRun",true);
            playerAni.SetBool("isAtk", false);
        }
    }
}
