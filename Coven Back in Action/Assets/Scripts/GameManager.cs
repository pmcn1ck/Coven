using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using TbsFramework.Units;

public enum eScene {start, fe, InGame }

public class GameManager : MonoBehaviour
{
    public static GameManager gm;


    [Space]
    [Header("Canvas prefabs")]
    public GameObject pCanvasMainMenu; // This is the templet for the main menu
    public GameObject pCanvasOption;
    public GameObject pCanvasStart;
    public GameObject pCanvasRotation;
    public GameObject pCanvasRanger;




    [Space]
    [Header("Spawn Canvas Script")]
    public cMainMenu c_MainMenu; // This is the spawned main meny script
    public cOption c_Options;
    public GrabRotation c_grabRotation;
    public cPlayer c_player;


    [Space]
    [Header("Scene Manager")]
    int currentScene;
    public eScene eCurScene;

    [Space]
    [Header("Units")]

    public GameObject[] unitTypes;
    public GameObject[] Party; //player's party of units
    public GameObject[] Reserve; //reserve units for player

    public GameObject CurrentUnit;
    public Unit unitToAttack;

    public bool isAttacking;

    [Space]
    [Header("OverWorld")]
    public List<int> rooms;

    private void Awake()
    {
        if(gm == null)
        {
            DontDestroyOnLoad(this.gameObject);
            gm = this;
        }
        else if (gm != this)
        {
            Destroy(gameObject);
        }
        CreatePlayers();
        foreach (var item in Party)
        {
            var sUnit = item.GetComponent<Unit>();
            sUnit.TotalHitPoints = sUnit.HitPoints;
        }
    }




    private void Start()
    {
        
    }

    void CreatePlayers()
    {
        Party = new GameObject[unitTypes.Length];
        for (int i = 0; i < unitTypes.Length; i++)
        {
            Party[i] = unitTypes[i];
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }


    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {

        currentScene = scene.buildIndex;
        eCurScene = (eScene)currentScene;
        switch (eCurScene)
        {
            case eScene.start:
                SpawnStart();
                break;
            case eScene.fe:
                SpawnMainMenu();
                break;
            case eScene.InGame:
                break;
        }

    }


    public void LoadScene(eScene _scene)
    {
        SceneManager.LoadScene((int)_scene);
    }

      

    public void SpawnMainMenu()
    {
        c_MainMenu = Instantiate(pCanvasMainMenu).GetComponent<cMainMenu>();
    }

    
    public void SpawnStart()
    {
        c_MainMenu = Instantiate(pCanvasStart).GetComponent<cMainMenu>();
    }

    public void SpawnOption()
    {
        c_Options = Instantiate(pCanvasOption).GetComponent<cOption>();
        c_Options.InitUI();
    }

    public void SpawnRotation()
    {
        c_grabRotation = Instantiate(pCanvasRotation).GetComponent<GrabRotation>();
        Debug.Log("Spawning ConvasRotation");
    }



    

}
