using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;    //행동지표를 결정할 변수
    Animator anim;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Awake() // 초기화
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        Invoke("Think", 5); //시간 지정(5초 뒤에 Think() 함수 호출)
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //사용자가 키보드로 움직이게 하는게 아니라 알아서 움직이게(기본 움직임)
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);


        // 플랫폼 체크 
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.2f, rigid.position.y); // 이동하는 경로의 상태 예측 => 앞 방향 체크
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)        //앞이 낭떨어지일 때
            Turn();


    }


   
    void Think()     //행동지표 바꿔줄 함수(재귀함수)
    {
        // 다음 활동
        nextMove = Random.Range(-1, 2); //-1~1 값을 random에 넣음

        anim.SetInteger("WalkSpeed", nextMove); //nextMove 값을 애니메이터 파라미터에 넣음

        if (nextMove != 0)//서있을 때는 방향 바꿀 필요 없으니
            spriteRenderer.flipX = nextMove == 1; //애니메이션 방향

        // 재귀
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);     //생각하는 시간도 랜덤하게
    }
    void Turn()
    {
        nextMove = nextMove * -1; //방향 정반대가 됨(1이면->-1로 / -1이면 -> 1로)
        spriteRenderer.flipX = nextMove == 1; //애니메이션 방향 전환
        CancelInvoke();          // 현재 작동중인 Invoke 함수 멈춤
        Invoke("Think", 5);       //다시 Invoke() 동작하게
    }

    public void OnDamaged() //몬스터 죽을 때 취해야하는 액션 (플레이어가 호출하니 public)
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);    // 색상 투명 

        spriteRenderer.flipY = true;    //뒤집힌채로 추락하게

        boxCollider.enabled = false; //비활성화 
        // 살짝 점프됐다가 아래로 추락하게
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);    
        Invoke("DeActive", 5);   //5초뒤에 비활성화되게 함수 호출, 
    }

    void DeActive() //비활성화 함수
    {
        gameObject.SetActive(false);
    }
}
