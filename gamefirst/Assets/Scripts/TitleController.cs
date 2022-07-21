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
        // 하이스코어 표시
        highScoreLabel.text = "High Score : " + PlayerPrefs.GetInt("HighScore") + "points";
    }
    

    public void OnStartButtonClicked()  //버튼 클릭 시
    {
      
        SceneManager.LoadScene(1);  //게임화면으로 전환
    }
 
}
