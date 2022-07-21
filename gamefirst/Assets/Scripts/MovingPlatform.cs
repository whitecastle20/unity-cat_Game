using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingPlatform : MonoBehaviour
{
    public Transform startPos;  //시작위치
    public Transform endPos;  //종료위치
    public Transform desPos;  // 도착지의 Transform
    public float speed; //발판 속도 조절
    // Start is called before the first frame update

    void Start()
    {
        //발판의 position과 desPos 초기화 (발판이 desPos에 도착하면 desPos의 값을 startPos로 초기화)
        transform.position = startPos.position;
        desPos = endPos;
        
    }

 
    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, desPos.position, Time.deltaTime*speed);

        if(Vector2.Distance(transform.position, desPos.position) <= 0.05f)  //Distance는 발판과 desPos 사이 거리
        {
            if (desPos == endPos)
                desPos = startPos;
            else
                desPos = endPos;
        }
    }
}
