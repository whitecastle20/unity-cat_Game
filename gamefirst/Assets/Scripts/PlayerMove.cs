using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.CapsuleCollider2D;


public class PlayerMove : MonoBehaviour
{
    public float maxSpeed; // inspector 창에서 4.5로 정했음
    Rigidbody2D rigid;
    public float jumpPower;
    SpriteRenderer spriteRenderer; //플레이어 방향 바꾸면 몸도 반대 방향으로 보이게
    Animator anim;

    public GameManager gameManager; //점수 변수에 접근하기  위해 
    CapsuleCollider2D capsuleCollider;

    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioFinish;

    AudioSource audioSource;

    void Awake()
    {
        //초기화
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }
    
    void PlaySound(string action)    //각 액션마다 오디오 클립 바꾸고 재생하는 함수 
    {
        switch(action)
        {
            case "JUMP":
                audioSource.clip = audioJump; // 점프 오디오
                break;
            case "ATTACK":
                audioSource.clip = audioAttack; // 공격 오디오
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged; // 데미지 오디오
                break;
            case "ITEM":
                audioSource.clip = audioItem; // 아이템 오디오
                break;
            case "FINISH":
                audioSource.clip = audioFinish; // 종료 오디오
                break;
        }
        audioSource.Play();
    }
    void Update()   //단발적인 키 입력
    {

        //점프
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))  //무한점프 방지
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);    //플레이어 위로 점프
            anim.SetBool("isJumping", true);    // 점프하는 모습의 플레이어
            PlaySound("JUMP"); //오디오


        }


        if (Input.GetButtonUp("Horizontal")) //키에서 떼면
        {
            //속력 줄임(멈출 때)
           // rigid.velocity.normalized => 방향+크기 같이 갖고 있어서 크기를 단위로 만들어줌Normarlized = 방향구함
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);    

        }

        //플레이어 방향 전환
        if (Input.GetButton("Horizontal"))  //키 입력시
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;    //(왼쪽키->왼쪽모습, 오른쪽키->오른쪽모습)
        }


        // 움직임에 따른 애니메이션 변환
        if (Mathf.Abs(rigid.velocity.x) < 0.3) // |0.3| 미만이면 
            anim.SetBool("isWalking", false);   //멈춤 / 애니메이터에서 만든 파라미터가 bool값이라, 만들었던 파라미터 이름 : isWalking
        else
            anim.SetBool("isWalking", true);    //움직이고 있음


    }


    void FixedUpdate()  // 지속적인 키 입력, 디폴트 : 1초 약 50회 
    {
        //플레이어가 좌우로 이동할 때
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse); //꾹 누르면 엄청 플레이어(cat) 빨라짐  ==> 플레이어가 가속 무한으로 가지 않게 최대값 걸어줌

        if(rigid.velocity.x > maxSpeed) // 오른쪽으로 이동할 때, 플레이어 현재속도가 최대값 넘으면 = 너무 빠르면 
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);     // 최대값으로 잡아줌 (y축 0으로 잡으면 점프뛸 때 멈춰서 y축은 그대로)
        }
        else if (rigid.velocity.x < maxSpeed * (-1))   // 왼쪽으로 이동할 때, 스피드 최대 넘으면
        {
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);     
        }


        //착지할 때 
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0)); //연두색 선(디버그라 실제 게임 보이지x)
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform")); // Ray에 닿은 오브젝트
            if (rayHit.collider != null) 
                if (rayHit.distance < 0.5f)     //플레이어가 바닥에 착지한 걸 알아내기 위해(플레이어 크기 1이라)
                    anim.SetBool("isJumping", false);

        }
    }
    void OnCollisionEnter2D(Collision2D collision)  //충돌 판정
    {
        if (collision.gameObject.tag == "Enemy")    //플레이어와 닿은 오브젝트가 적일 때 
        {
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)   //몬스터보다 위에 있음 + 아래로 낙하하는 중이면 => 밟아서 충돌할 경우 즉, 공격(데미지를 줌)
            {
                OnAttack(collision.transform);
                PlaySound("ATTACK"); //오디오
            }
            else            //데미지 입음
            {
                OnDamaged(collision.transform.position); //충돌 됐을 때 무적 효과
                PlaySound("DAMAGED"); //오디오
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)//플레이어가 아이템(생선)먹으면 사라지게
    {
        if(collision.gameObject.tag == "Item")//아이템 먹으면=닿으면
        {

            //점수 획득
            bool isFish = collision.gameObject.name.Contains("item_1");
            bool isChicken = collision.gameObject.name.Contains("item_2");
            bool isExFish = collision.gameObject.name.Contains("item_3");
         
            // 아이템별 점수 획득
            if(isFish)
                gameManager.stagePoint += 50;
            else if(isChicken)
                gameManager.stagePoint += 100;
            else if(isExFish)
                gameManager.stagePoint += 150;

            collision.gameObject.SetActive(false);//아이템 사라짐

            PlaySound("ITEM"); //오디오

        }
        else if (collision.gameObject.tag == "Finish")  //결승깃발 도착하면
        {
            // 다음 단계(플레이어가 아닌 매니저가)
            gameManager.NextStage();
            PlaySound("FINISH"); //오디오
        }

    }


    void OnAttack(Transform enemy)
    {
        gameManager.stagePoint += 100;  //포인트 획득

        // reaction force
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        // 몬스터 죽음 
        EnemyMove enemyMove =  enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();  //몬스터 입장에선 공격당함
    }


    void OnDamaged(Vector2 targetPos) // 무적 효과 함수 생성
    {
        //hp 감소
        gameManager.HealthDown();

        gameObject.layer = 11;  // 레이어 변경(PlayerDamaged로)

        spriteRenderer.color = new Color(1, 1, 1, 0.4f);//사용자에게 잠시 무적이라는 사실을 알려주기 위해 색상 투명하게 변경

        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1; //부딪힌 오브젝트와 오른쪽에서 튕겼으면 오른쪽으로 나가고, 왼쪽에서 튕겼으면 왼족으로 나가게
        rigid.AddForce(new Vector2(dirc,1)*7, ForceMode2D.Impulse);   // 튕겨나가게(작용,반작용에 읳)


      
        anim.SetTrigger("doDamaged");  //적 닿을 때 애니메이션


        Invoke("OffDamaged", 3); // 무적 시간 넣어서 해제 함수 호출
   
    }

    void OffDamaged() // 무적 해제 함수
    {
        gameObject.layer = 10;  // 레이어 변경(원래로)

        spriteRenderer.color = new Color(1, 1, 1, 1);//색상 원래대로

    }
    public void OnDie()    //플레이어 죽음(게임메니저에서 호출-> public)
    {

       
        spriteRenderer.color = new Color(0, 0, 0, 0);    // 색 변경

        spriteRenderer.flipY = true;    //뒤집힌채로 추락하게

        capsuleCollider.enabled = false; //비활성화 
        // 살짝 점프됐다가 아래로 추락하게
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

 

    }


    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
   
}



