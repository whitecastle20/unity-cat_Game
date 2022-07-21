using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingPlatform : MonoBehaviour
{
    public Transform startPos;  //������ġ
    public Transform endPos;  //������ġ
    public Transform desPos;  // �������� Transform
    public float speed; //���� �ӵ� ����
    // Start is called before the first frame update

    void Start()
    {
        //������ position�� desPos �ʱ�ȭ (������ desPos�� �����ϸ� desPos�� ���� startPos�� �ʱ�ȭ)
        transform.position = startPos.position;
        desPos = endPos;
        
    }

 
    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, desPos.position, Time.deltaTime*speed);

        if(Vector2.Distance(transform.position, desPos.position) <= 0.05f)  //Distance�� ���ǰ� desPos ���� �Ÿ�
        {
            if (desPos == endPos)
                desPos = startPos;
            else
                desPos = endPos;
        }
    }
}
