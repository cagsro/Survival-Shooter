using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score;

    public GameObject finishScreen;
    public GameObject startScreen;
    public GameObject joystickLeft;
    public GameObject JoystickRight;
    public static bool isGameStarted = false;
    public static bool isGameEnded = false;

    public Text text; 


    private void Awake()
    {        
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        isGameStarted = false;
        isGameEnded = false;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Score: " + score.ToString();
    }

    public void ShootButton()
    {
        PlayerShooting.instance.Shoot();
    }

    public void OnLevelStarted()
    {
        isGameStarted = true;
        startScreen.SetActive(false);
        joystickLeft.SetActive(true);
        JoystickRight.SetActive(true);


    }

    public void OnLevelEnded()
    {
        
    }

    public void OnLevelCompleted()
    {

    }

    public void OnLevelFailed() // Game Over ekranini cagirma
    {
        finishScreen.SetActive(true);
        isGameEnded = true;
        joystickLeft.SetActive(false);
        JoystickRight.SetActive(false);
    }
    
    public void Restart ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GameEnded()
    {
        isGameEnded=true;
    }
}
