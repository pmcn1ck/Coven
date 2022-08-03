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

    Animator anim;

    float animationTIme = 100f;

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
        
        StartCoroutine(WaitToDestroy());
        //pauseMenuUI.SetActive(false);
        // Destroy(gameObject);
        Time.timeScale = 1f;
        GameIsPaused = false;
        
       
        
        //Destroy(gameObject);
        

    }

    IEnumerator WaitToDestroy()
    {
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).length + animationTIme);
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);

    }

    public void OnDisable()
    {
        

    }

    void Pause()
    {

        pauseMenuUI.SetActive(true);
        Instantiate(pause, tPause);
        //Time.timeScale = 0f;
        //GameIsPaused = true;
        StartCoroutine(WaitToPause());
    
    }

    IEnumerator WaitToPause()
    {
        
        yield return new WaitForSeconds(0.8f);
        Time.timeScale = 0f;
        GameIsPaused = true;

    }

    public void InitOptions()
    {
        Time.timeScale = 1f;
        GameObject obj = Instantiate(optionsUI);

    }

    public void Quit()
    {
        GameManager.gm.LoadScene(eScene.fe);
    }
}
