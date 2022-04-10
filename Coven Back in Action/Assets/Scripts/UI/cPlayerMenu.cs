using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cPlayerMenu : MonoBehaviour
{
    public GameObject playerInfo;
    public Transform tplayerInfor;

    public void SpawnWplayer()
    {
        GameObject obj = Instantiate(playerInfo,tplayerInfor);
    }

}
