using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  //LoadScene 사용할 때 필요

public class GameManager : MonoBehaviour
{
    //점수, 스테이지 전역 변수
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int forHighPoint;
    public int health;  //HP변수

   
    public PlayerMove player;
    
    public GameObject[] Stages;

    //UI 담을 변수들 
    public Image[] UIhealth;
    public Text UIPoint;
    public Text UIStage;
    [SerializeField] Text countdownText;
    [SerializeField] float setTime;


    public GameObject UIRestartBtn; 
    public GameObject UIQuitBtn;
    public GameObject UIGameState;

 

    void Start()
    {
        setTime = 200.0f;    // 타이머 200초
        countdownText.text = setTime.ToString();
    }

    public void NextStage()
    {
        // stageIndex에 따라 스테이지 활성화/비활성화       
        if (stageIndex < Stages.Length-1)  // 스테이지 개수 확인해서 다음 스테이지 이동 / 종료
        {
          
            Stages[stageIndex].SetActive(false);
            stageIndex++;    
            Stages[stageIndex].SetActive(true);

          
            PlayerReposition();

            UIStage.text = "STAGE " + (stageIndex + 1);   //스테이지 단계 출력
               
        }
        else//게임 끝(
        {
            // Player Control Lock
            Time.timeScale = 0;  //완주하면 timeScale = 0 으로 시간 멈춰둠
                               
            //restart button ui
          
            Text btnText1 = UIRestartBtn.GetComponentInChildren<Text>();
            btnText1.text = "Retry";
            UIRestartBtn.SetActive(true);   //재시작 버튼은 게임 끝났을 때 활성화

            Text btnText2 = UIQuitBtn.GetComponentInChildren<Text>();
            btnText2.text = "Main";
            UIQuitBtn.SetActive(true);   //메인으로 돌아가기(게임종료) 버튼은 게임 끝났을 때 활성화
            
            Text btnText3 = UIGameState.GetComponentInChildren<Text>();
            btnText3.text = "Game Clear!";
            UIGameState.SetActive(true);  // 게임 클리어 문구를 담은 버튼 활성화

        ;

        }


        // 점수 계산
        totalPoint += stagePoint;  // 스테이지 올라가면 totalPoint에 누적
       
        stagePoint = 0;
 

    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
            UIhealth[health].color = new Color(0, 0, 0, 0.4f);   // 체력 이미지 색상 어둡게 변경
           
        }       
        else if(setTime <= 0.0f || health <=1)   // 시간 남지 않거나 목숨 없으면
        {
            player.OnDie(); //플레이어 죽음 함수 호출
            if(health<=1)   //목숨 없으면
                setTime = 0.0f;
            Text btnText3 = UIGameState.GetComponentInChildren<Text>();

            if(setTime <= 0.0f) //시간 없으면
            {
                for(int i=0; i<health; i++)
                    UIhealth[i].color = new Color(0, 0, 0, 0.4f);   //모든 목숨 색 투명
            }
        
            btnText3.text = "Game Over!";

            
            // 재시작 버튼 UI
            UIRestartBtn.SetActive(true);   //재시작 버튼은 게임 끝났을 때 활성화
            UIQuitBtn.SetActive(true);   //메인메뉴(게임나가기) 버튼은 게임 끝났을 때 활성화
            UIGameState.SetActive(true);  // 게임 오버 문구를 담은 버튼 활성화

           
          
        }

     
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")   //플레이어가 낭떠러지로 떨어지면
        {
            // 피 하나 남았을 때 떨어진 경우에는 원위치로 돌아가지 않음
            if(health > 1)
            {
                //플레이어 원래 위치로 놓기
                PlayerReposition();
            }

            HealthDown();   // 피 함수 호출

        }
            
    }

    void PlayerReposition()
    {
        //스테이지 바뀌면 플레이어 위치도 바뀌게 설정해주기 위한 함수
        player.transform.position = new Vector3(0,0,-1);
        player.VelocityZero();
    }
    
    // Update is called once per frame
    void Update()
    {
        //숫자 줄어들다가 0에서 멈춤
        if (setTime > 0)
        {
            setTime -= Time.deltaTime;
         
        }
            
        else if (setTime <= 0)
        {
            setTime = 0.0f;
            HealthDown();
        }
          

        countdownText.text = Mathf.Round(setTime).ToString();   //소수점 없애고 타이머 출력
        forHighPoint = totalPoint + stagePoint;
        UIPoint.text = (totalPoint + stagePoint).ToString();    //숫자 더한후 문자로 바꿔서 넣음

      
        // 하이스코어 업데이트totalPoint + stagePoint
        if (PlayerPrefs.GetInt("HighScore") < forHighPoint)
        {
         
            PlayerPrefs.SetInt("HighScore", forHighPoint);
        }
           
     
    }

    public void Restart()
    {
        Time.timeScale = 1; //시간 복구
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Time.timeScale = 1; //시간 복구
        SceneManager.LoadScene(0);
    }

 
}
