using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerGenerator : MonoBehaviour
    
{
    // 표창 인스턴스 7초마다 한개씩 생성
    public GameObject Saw_2;
    float span = 7.0f;
    float delta = 0;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -16.0f) // 화면 밖으로 나가면 소멸
            Destroy(gameObject);

        this.delta += Time.deltaTime;
        if(this.delta > this.span)
        {
            this.delta = 0;
            GameObject go = Instantiate(Saw_2) as GameObject;
            int py = Random.Range(-2, 10);  //y가 -2부터 10사이 
            go.transform.position = new Vector3(105, py, 0);
        }

    }
}
