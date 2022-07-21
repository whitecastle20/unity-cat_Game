using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;    //�ൿ��ǥ�� ������ ����
    Animator anim;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Awake() // �ʱ�ȭ
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        Invoke("Think", 5); //�ð� ����(5�� �ڿ� Think() �Լ� ȣ��)
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //����ڰ� Ű����� �����̰� �ϴ°� �ƴ϶� �˾Ƽ� �����̰�(�⺻ ������)
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);


        // �÷��� üũ 
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.2f, rigid.position.y); // �̵��ϴ� ����� ���� ���� => �� ���� üũ
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)        //���� ���������� ��
            Turn();


    }


   
    void Think()     //�ൿ��ǥ �ٲ��� �Լ�(����Լ�)
    {
        // ���� Ȱ��
        nextMove = Random.Range(-1, 2); //-1~1 ���� random�� ����

        anim.SetInteger("WalkSpeed", nextMove); //nextMove ���� �ִϸ����� �Ķ���Ϳ� ����

        if (nextMove != 0)//������ ���� ���� �ٲ� �ʿ� ������
            spriteRenderer.flipX = nextMove == 1; //�ִϸ��̼� ����

        // ���
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);     //�����ϴ� �ð��� �����ϰ�
    }
    void Turn()
    {
        nextMove = nextMove * -1; //���� ���ݴ밡 ��(1�̸�->-1�� / -1�̸� -> 1��)
        spriteRenderer.flipX = nextMove == 1; //�ִϸ��̼� ���� ��ȯ
        CancelInvoke();          // ���� �۵����� Invoke �Լ� ����
        Invoke("Think", 5);       //�ٽ� Invoke() �����ϰ�
    }

    public void OnDamaged() //���� ���� �� ���ؾ��ϴ� �׼� (�÷��̾ ȣ���ϴ� public)
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);    // ���� ���� 

        spriteRenderer.flipY = true;    //������ä�� �߶��ϰ�

        boxCollider.enabled = false; //��Ȱ��ȭ 
        // ��¦ �����ƴٰ� �Ʒ��� �߶��ϰ�
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);    
        Invoke("DeActive", 5);   //5�ʵڿ� ��Ȱ��ȭ�ǰ� �Լ� ȣ��, 
    }

    void DeActive() //��Ȱ��ȭ �Լ�
    {
        gameObject.SetActive(false);
    }
}
