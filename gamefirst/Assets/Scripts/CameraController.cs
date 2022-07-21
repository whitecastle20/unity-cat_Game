using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; // ī�޶� ���� ���
    public float moveSpeed; // ī�޶� ���� �ӵ�
    private Vector3 playerPosition; // ����� ���� ��ġ

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // ����� �ִ��� üũ
        if (player.gameObject != null)
        {
            // this�� ī�޶� �ǹ� (z���� ī�޶��� �״�� ����)
            playerPosition.Set(player.transform.position.x, player.transform.position.y, this.transform.position.z);

            // vectorA -> B���� T�� �ӵ��� �̵�
            this.transform.position = Vector3.Lerp(this.transform.position, playerPosition, moveSpeed * Time.deltaTime);
        }
    }
}
