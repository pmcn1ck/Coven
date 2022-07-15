using UnityEngine;

public class cMainMenu : MonoBehaviour
{

    // Start is called before the first frame update
    public void OnPlayClick()
    {
        GameManager.gm.LoadScene(eScene.InGame);
    }

    public void OnOptionClick()
    {
        GameManager.gm.SpawnOption();
    }

    public void OnRotationClick()
    {
        Destroy(gameObject);
        GameManager.gm.SpawnRotation();
        
    }

    public void OnClickQuit()
    {
        Application.Quit();
        Debug.Log("Player quit the game");

    }


    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Return))
        {
            PressStart();
        }*/

        if(GameManager.gm.eCurScene == eScene.fe)
        {
            return;
        }

        if (Input.anyKey)
        {
            GameManager.gm.LoadScene(eScene.fe);
        }
    }

    public void PressStart()
    {
       // GameManager.gm.LoadScene(eScene.fe);
    }

}
