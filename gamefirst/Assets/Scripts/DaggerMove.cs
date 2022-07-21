using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerMove : MonoBehaviour
{
  
    // Update is called once per frame
    void Update()
    {
        transform.Translate(-0.1f, 0, 0);   //프레임마다 등속 이동

    }
}
