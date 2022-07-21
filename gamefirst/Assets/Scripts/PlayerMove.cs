using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.CapsuleCollider2D;


public class PlayerMove : MonoBehaviour
{
    public float maxSpeed; // inspector â���� 4.5�� ������
    Rigidbody2D rigid;
    public float jumpPower;
    SpriteRenderer spriteRenderer; //�÷��̾� ���� �ٲٸ� ���� �ݴ� �������� ���̰�
    Animator anim;

    public GameManager gameManager; //���� ������ �����ϱ�  ���� 
    CapsuleCollider2D capsuleCollider;

    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioFinish;

    AudioSource audioSource;

    void Awake()
    {
        //�ʱ�ȭ
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }
    
    void PlaySound(string action)    //�� �׼Ǹ��� ����� Ŭ�� �ٲٰ� ����ϴ� �Լ� 
    {
        switch(action)
        {
            case "JUMP":
                audioSource.clip = audioJump; // ���� �����
                break;
            case "ATTACK":
                audioSource.clip = audioAttack; // ���� �����
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged; // ������ �����
                break;
            case "ITEM":
                audioSource.clip = audioItem; // ������ �����
                break;
            case "FINISH":
                audioSource.clip = audioFinish; // ���� �����
                break;
        }
        audioSource.Play();
    }
    void Update()   //�ܹ����� Ű �Է�
    {

        //����
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))  //�������� ����
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);    //�÷��̾� ���� ����
            anim.SetBool("isJumping", true);    // �����ϴ� ����� �÷��̾�
            PlaySound("JUMP"); //�����


        }


        if (Input.GetButtonUp("Horizontal")) //Ű���� ����
        {
            //�ӷ� ����(���� ��)
           // rigid.velocity.normalized => ����+ũ�� ���� ���� �־ ũ�⸦ ������ �������Normarlized = ���ⱸ��
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);    

        }

        //�÷��̾� ���� ��ȯ
        if (Input.GetButton("Horizontal"))  //Ű �Է½�
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;    //(����Ű->���ʸ��, ������Ű->�����ʸ��)
        }


        // �����ӿ� ���� �ִϸ��̼� ��ȯ
        if (Mathf.Abs(rigid.velocity.x) < 0.3) // |0.3| �̸��̸� 
            anim.SetBool("isWalking", false);   //���� / �ִϸ����Ϳ��� ���� �Ķ���Ͱ� bool���̶�, ������� �Ķ���� �̸� : isWalking
        else
            anim.SetBool("isWalking", true);    //�����̰� ����


    }


    void FixedUpdate()  // �������� Ű �Է�, ����Ʈ : 1�� �� 50ȸ 
    {
        //�÷��̾ �¿�� �̵��� ��
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse); //�� ������ ��û �÷��̾�(cat) ������  ==> �÷��̾ ���� �������� ���� �ʰ� �ִ밪 �ɾ���

        if(rigid.velocity.x > maxSpeed) // ���������� �̵��� ��, �÷��̾� ����ӵ��� �ִ밪 ������ = �ʹ� ������ 
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);     // �ִ밪���� ����� (y�� 0���� ������ ������ �� ���缭 y���� �״��)
        }
        else if (rigid.velocity.x < maxSpeed * (-1))   // �������� �̵��� ��, ���ǵ� �ִ� ������
        {
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);     
        }


        //������ �� 
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0)); //���λ� ��(����׶� ���� ���� ������x)
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform")); // Ray�� ���� ������Ʈ
            if (rayHit.collider != null) 
                if (rayHit.distance < 0.5f)     //�÷��̾ �ٴڿ� ������ �� �˾Ƴ��� ����(�÷��̾� ũ�� 1�̶�)
                    anim.SetBool("isJumping", false);

        }
    }
    void OnCollisionEnter2D(Collision2D collision)  //�浹 ����
    {
        if (collision.gameObject.tag == "Enemy")    //�÷��̾�� ���� ������Ʈ�� ���� �� 
        {
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)   //���ͺ��� ���� ���� + �Ʒ��� �����ϴ� ���̸� => ��Ƽ� �浹�� ��� ��, ����(�������� ��)
            {
                OnAttack(collision.transform);
                PlaySound("ATTACK"); //�����
            }
            else            //������ ����
            {
                OnDamaged(collision.transform.position); //�浹 ���� �� ���� ȿ��
                PlaySound("DAMAGED"); //�����
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)//�÷��̾ ������(����)������ �������
    {
        if(collision.gameObject.tag == "Item")//������ ������=������
        {

            //���� ȹ��
            bool isFish = collision.gameObject.name.Contains("item_1");
            bool isChicken = collision.gameObject.name.Contains("item_2");
            bool isExFish = collision.gameObject.name.Contains("item_3");
         
            // �����ۺ� ���� ȹ��
            if(isFish)
                gameManager.stagePoint += 50;
            else if(isChicken)
                gameManager.stagePoint += 100;
            else if(isExFish)
                gameManager.stagePoint += 150;

            collision.gameObject.SetActive(false);//������ �����

            PlaySound("ITEM"); //�����

        }
        else if (collision.gameObject.tag == "Finish")  //��±�� �����ϸ�
        {
            // ���� �ܰ�(�÷��̾ �ƴ� �Ŵ�����)
            gameManager.NextStage();
            PlaySound("FINISH"); //�����
        }

    }


    void OnAttack(Transform enemy)
    {
        gameManager.stagePoint += 100;  //����Ʈ ȹ��

        // reaction force
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        // ���� ���� 
        EnemyMove enemyMove =  enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();  //���� ���忡�� ���ݴ���
    }


    void OnDamaged(Vector2 targetPos) // ���� ȿ�� �Լ� ����
    {
        //hp ����
        gameManager.HealthDown();

        gameObject.layer = 11;  // ���̾� ����(PlayerDamaged��)

        spriteRenderer.color = new Color(1, 1, 1, 0.4f);//����ڿ��� ��� �����̶�� ����� �˷��ֱ� ���� ���� �����ϰ� ����

        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1; //�ε��� ������Ʈ�� �����ʿ��� ƨ������ ���������� ������, ���ʿ��� ƨ������ �������� ������
        rigid.AddForce(new Vector2(dirc,1)*7, ForceMode2D.Impulse);   // ƨ�ܳ�����(�ۿ�,���ۿ뿡 ��)


      
        anim.SetTrigger("doDamaged");  //�� ���� �� �ִϸ��̼�


        Invoke("OffDamaged", 3); // ���� �ð� �־ ���� �Լ� ȣ��
   
    }

    void OffDamaged() // ���� ���� �Լ�
    {
        gameObject.layer = 10;  // ���̾� ����(������)

        spriteRenderer.color = new Color(1, 1, 1, 1);//���� �������

    }
    public void OnDie()    //�÷��̾� ����(���Ӹ޴������� ȣ��-> public)
    {

       
        spriteRenderer.color = new Color(0, 0, 0, 0);    // �� ����

        spriteRenderer.flipY = true;    //������ä�� �߶��ϰ�

        capsuleCollider.enabled = false; //��Ȱ��ȭ 
        // ��¦ �����ƴٰ� �Ʒ��� �߶��ϰ�
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

 

    }


    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
   
}



