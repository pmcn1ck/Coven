using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class OverWorldLoad : MonoBehaviour
{
    public GameObject Player;
    public Transform PlayerSpawn;
    public List<sRoom> RoomDatabase;
    public int curRoomID;
    public sRoom curRoom;
    public Transform[] objectsToRotate;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.gm.rooms != null)
        {
            foreach (int num in GameManager.gm.rooms)
            {
                RoomDatabase[num].complete = true;
            }
        }
        curRoomID = GameManager.gm.rooms.Last();
        curRoom = RoomDatabase[curRoomID];
        PlayerSpawn = curRoom.transform;
        Debug.Log("spawn now");
        Instantiate(Player, new Vector3 (curRoom.transform.position.x, curRoom.transform.position.y, curRoom.transform.position.z), Quaternion.identity);
    }


}
