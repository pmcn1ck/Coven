using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class cPauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject optionsUI;
    public GameObject pause;
    public Transform tPause;

    float animationTIme = 50f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
           
            if (GameIsPaused)
            {
                
                Resume();

            }
            else
            {
                Pause();
            }
        }

    }

   public void Resume()
    {
        // pauseMenuUI.SetActive(false);
        // Destroy(gameObject);
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).length + animationTIme);
        Time.timeScale = 1f;
        
        GameIsPaused = false;

    }

    void Pause()
    {

        //pauseMenuUI.SetActive(true);
        Instantiate(pause, tPause);
        //Time.timeScale = 0f;
        //GameIsPaused = true;
        StartCoroutine(WaitToPause());
    }

    IEnumerator WaitToPause()
    {
        
        yield return new WaitForSeconds(1);
        Time.timeScale = 0f;
        GameIsPaused = true;

    }

    public void InitOptions()
    {
        
        GameObject obj= Instantiate(optionsUI);
    }

    public void Quit()
    {
        GameManager.gm.LoadScene(eScene.fe);
    }
}
