using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerGenerator : MonoBehaviour
    
{
    // ǥâ �ν��Ͻ� 7�ʸ��� �Ѱ��� ����
    public GameObject Saw_2;
    float span = 7.0f;
    float delta = 0;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -16.0f) // ȭ�� ������ ������ �Ҹ�
            Destroy(gameObject);

        this.delta += Time.deltaTime;
        if(this.delta > this.span)
        {
            this.delta = 0;
            GameObject go = Instantiate(Saw_2) as GameObject;
            int py = Random.Range(-2, 10);  //y�� -2���� 10���� 
            go.transform.position = new Vector3(105, py, 0);
        }

    }
}
