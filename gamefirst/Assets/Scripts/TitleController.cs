using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    
    public Text highScoreLabel;
    
    public void Start()
    {
        // ���̽��ھ� ǥ��
        highScoreLabel.text = "High Score : " + PlayerPrefs.GetInt("HighScore") + "points";
    }
    

    public void OnStartButtonClicked()  //��ư Ŭ�� ��
    {
      
        SceneManager.LoadScene(1);  //����ȭ������ ��ȯ
    }
 
}
