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


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.gm.LoadScene(eScene.fe);
        }
    }

}
