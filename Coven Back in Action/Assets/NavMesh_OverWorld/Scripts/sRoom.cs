using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using TbsFramework.Units;

public class sRoom : MonoBehaviour
{
    public int roomID;
    public soRoom room;
    public List<OffMeshLink> roomConnectors;
    public GameObject pEvent;
    public GameObject pPath;
    public List<GameObject> enemyObjects;
    public bool complete;
    public bool isCombat;
    public AudioClip warning;
    public AudioSource audiosource;

    [Header("EventWindowComponents")]
    public Text tEventName;
    public Text tDescription;
    public Text tContinueButton;
    public Button bContinue;


    IEnumerator Start()
    {
        audiosource = GameManager.gm.audioManager.GetComponent<AudioSource>();
        if (room != null && room.combatArena == true)
        {
            isCombat = true;
        }
        if (complete)
        {
            if (room != null)
            {
                if (room.combatArena)
                {
                    for (int i = 0; i < enemyObjects.Count; i++)
                    {
                        Debug.Log("Destroying Enemies");
                        Destroy(enemyObjects[i].gameObject);
                    }
                }
            }
        }
        yield return new WaitForSeconds(.01f);
        if (complete)
        {
            if (room != null)
            {
                if (room.combatArena)
                {
                    for (int i = 0; i < enemyObjects.Count; i++)
                    {
                        Debug.Log("Destroying Enemies");
                        Destroy(enemyObjects[i].gameObject);
                    }
                }
            }
        }
        yield return new WaitForSeconds(.5f);
        if (complete)
        {
            
            ActivateMeshLinks();

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!complete)
        {
            if (other.CompareTag("Player"))
            {
                bool roomFound = false;
                foreach (int item in GameManager.gm.rooms)
                {
                    if (roomID == item)
                    {
                        roomFound = true;
                    }
                }
                if (!roomFound)
                {
                    Debug.Log("Room Added");
                    GameManager.gm.rooms.Add(this.gameObject.GetComponent<sRoom>().roomID);
                }
                if (isCombat)
                {
                    other.GetComponent<PlayerController>().isInCombat = true;
                    audiosource.PlayOneShot(warning);
                }
                Debug.Log("Yup that's the player");
                pEvent.SetActive(true);
                if (tEventName != null)
                {
                    tEventName.text = room.eventName;
                    tDescription.text = room.description;
                    tContinueButton.text = "Continue";
                }
                bContinue.onClick.AddListener(delegate { ContinueButton(); });
            }
        }
    }

    public void ContinueButton()
    {
        if (room.combatArena)
        {
            GameManager.gm.LoadScene(room.Scene);
            GameManager.gm.expRewards = room.expRewards;
            pEvent.SetActive(false);
        }
        else
        {
            complete = true;
            pEvent.SetActive(false);
            ActivateMeshLinks();
        }
    }

    public void ActivateMeshLinks()
    {
        if (complete)
        {
            foreach (OffMeshLink item in roomConnectors)
            {
                item.activated = true;
            }
            pPath.SetActive(true);
        }
    }
}
