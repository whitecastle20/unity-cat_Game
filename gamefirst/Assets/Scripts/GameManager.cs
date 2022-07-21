using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  //LoadScene ����� �� �ʿ�

public class GameManager : MonoBehaviour
{
    //����, �������� ���� ����
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int forHighPoint;
    public int health;  //HP����

   
    public PlayerMove player;
    
    public GameObject[] Stages;

    //UI ���� ������ 
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
        setTime = 200.0f;    // Ÿ�̸� 200��
        countdownText.text = setTime.ToString();
    }

    public void NextStage()
    {
        // stageIndex�� ���� �������� Ȱ��ȭ/��Ȱ��ȭ       
        if (stageIndex < Stages.Length-1)  // �������� ���� Ȯ���ؼ� ���� �������� �̵� / ����
        {
          
            Stages[stageIndex].SetActive(false);
            stageIndex++;    
            Stages[stageIndex].SetActive(true);

          
            PlayerReposition();

            UIStage.text = "STAGE " + (stageIndex + 1);   //�������� �ܰ� ���
               
        }
        else//���� ��(
        {
            // Player Control Lock
            Time.timeScale = 0;  //�����ϸ� timeScale = 0 ���� �ð� �����
                               
            //restart button ui
          
            Text btnText1 = UIRestartBtn.GetComponentInChildren<Text>();
            btnText1.text = "Retry";
            UIRestartBtn.SetActive(true);   //����� ��ư�� ���� ������ �� Ȱ��ȭ

            Text btnText2 = UIQuitBtn.GetComponentInChildren<Text>();
            btnText2.text = "Main";
            UIQuitBtn.SetActive(true);   //�������� ���ư���(��������) ��ư�� ���� ������ �� Ȱ��ȭ
            
            Text btnText3 = UIGameState.GetComponentInChildren<Text>();
            btnText3.text = "Game Clear!";
            UIGameState.SetActive(true);  // ���� Ŭ���� ������ ���� ��ư Ȱ��ȭ

        ;

        }


        // ���� ���
        totalPoint += stagePoint;  // �������� �ö󰡸� totalPoint�� ����
       
        stagePoint = 0;
 

    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
            UIhealth[health].color = new Color(0, 0, 0, 0.4f);   // ü�� �̹��� ���� ��Ӱ� ����
           
        }       
        else if(setTime <= 0.0f || health <=1)   // �ð� ���� �ʰų� ��� ������
        {
            player.OnDie(); //�÷��̾� ���� �Լ� ȣ��
            if(health<=1)   //��� ������
                setTime = 0.0f;
            Text btnText3 = UIGameState.GetComponentInChildren<Text>();

            if(setTime <= 0.0f) //�ð� ������
            {
                for(int i=0; i<health; i++)
                    UIhealth[i].color = new Color(0, 0, 0, 0.4f);   //��� ��� �� ����
            }
        
            btnText3.text = "Game Over!";

            
            // ����� ��ư UI
            UIRestartBtn.SetActive(true);   //����� ��ư�� ���� ������ �� Ȱ��ȭ
            UIQuitBtn.SetActive(true);   //���θ޴�(���ӳ�����) ��ư�� ���� ������ �� Ȱ��ȭ
            UIGameState.SetActive(true);  // ���� ���� ������ ���� ��ư Ȱ��ȭ

           
          
        }

     
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")   //�÷��̾ ���������� ��������
        {
            // �� �ϳ� ������ �� ������ ��쿡�� ����ġ�� ���ư��� ����
            if(health > 1)
            {
                //�÷��̾� ���� ��ġ�� ����
                PlayerReposition();
            }

            HealthDown();   // �� �Լ� ȣ��

        }
            
    }

    void PlayerReposition()
    {
        //�������� �ٲ�� �÷��̾� ��ġ�� �ٲ�� �������ֱ� ���� �Լ�
        player.transform.position = new Vector3(0,0,-1);
        player.VelocityZero();
    }
    
    // Update is called once per frame
    void Update()
    {
        //���� �پ��ٰ� 0���� ����
        if (setTime > 0)
        {
            setTime -= Time.deltaTime;
         
        }
            
        else if (setTime <= 0)
        {
            setTime = 0.0f;
            HealthDown();
        }
          

        countdownText.text = Mathf.Round(setTime).ToString();   //�Ҽ��� ���ְ� Ÿ�̸� ���
        forHighPoint = totalPoint + stagePoint;
        UIPoint.text = (totalPoint + stagePoint).ToString();    //���� ������ ���ڷ� �ٲ㼭 ����

      
        // ���̽��ھ� ������ƮtotalPoint + stagePoint
        if (PlayerPrefs.GetInt("HighScore") < forHighPoint)
        {
         
            PlayerPrefs.SetInt("HighScore", forHighPoint);
        }
           
     
    }

    public void Restart()
    {
        Time.timeScale = 1; //�ð� ����
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Time.timeScale = 1; //�ð� ����
        SceneManager.LoadScene(0);
    }

 
}
