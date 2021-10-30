using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMove : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer[] backGrounds;

    [SerializeField]
    [Tooltip("맨뒤 배경 속도")]
    private float offsetBackSpeed;
    private float offsetBack;


    [SerializeField]
    [Tooltip("중간 배경 속도")]
    private float offsetMidSpeed;
    private float offsetMid;


    [SerializeField]
    [Tooltip("맨앞 배경 속도")]
    private float offsetFrontSpeed;
    private float offsetFront;
    
    [SerializeField]
    [Tooltip("그라운드 속도")]
    private float offsetGroundSpeed;
    private float offsetGround;


    private void Start() 
    {
        backGrounds = GetComponentsInChildren<MeshRenderer>();
    }
    private void Update() 
    {
        BackGroundMove();
    }

    public void BackGroundMove()
    {
        offsetBack += Time.deltaTime * offsetBackSpeed;
        offsetMid += Time.deltaTime * offsetMidSpeed;
        offsetFront += Time.deltaTime * offsetFrontSpeed;
        offsetGround += Time.deltaTime * offsetGroundSpeed;

        backGrounds[0].material.mainTextureOffset = new Vector2(offsetBack , 0);
        backGrounds[1].material.mainTextureOffset = new Vector2(offsetMid , 0);
        backGrounds[2].material.mainTextureOffset = new Vector2(offsetFront, 0);
        backGrounds[3].material.mainTextureOffset = new Vector2(offsetGround, 0);
    }
   

    
}
