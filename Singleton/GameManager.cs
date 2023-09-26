using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    public static GameManager instance;

    public GameObject pauseCanvas;
    public bool isLockAWSD;

    private void Awake() {
        if(instance != null && instance != this){
            Destroy(this);
        }else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
        isLockAWSD = false;
    }

    private void Update() 
    {
        //Pause
        if(Input.GetKeyDown(KeyCode.Escape) && !isLockAWSD)
        {
            Pause();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isLockAWSD)
        {
            Resume();
        }
    }
    private void Pause()
    {  
        //Cinemachine brain
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        //Stop time
        Time.timeScale = 0;
        //Lock AWSD
        isLockAWSD = true;
        //Enable Cursor
        CursorLockManager.instance.UnLockCursor();
    }

    private void Resume()
    {
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;
        Time.timeScale = 1;
        isLockAWSD = false;
        CursorLockManager.instance.DisableCursor();
    }

    private void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    public void LoadSceneMainMenu()
    {
        CursorLockManager.instance.UnLockCursor();
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }   

    public void LoadSceneDead()
    {
        CursorLockManager.instance.UnLockCursor();
        Time.timeScale = 1;
        SceneManager.LoadScene("DeadScene");
    }

    public void LoadSceneVictory()
    {
        CursorLockManager.instance.UnLockCursor();
        Time.timeScale = 1;
        SceneManager.LoadScene("VictoryScene");
    } 
}
