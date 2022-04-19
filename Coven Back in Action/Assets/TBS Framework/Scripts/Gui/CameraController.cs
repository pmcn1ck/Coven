using UnityEngine;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Units;
using System;
using System.Collections.Generic;

namespace TbsFramework.Gui
{
    /// <summary>
    /// Simple movable camera implementation.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        public float ScrollSpeed = 15;
        public float ScrollEdge = 0.01f;
        public bool overWorldCamera;
        public Vector2 maxPosition;
        public Vector2 minPosition;
        public Vector3 startingPos;
        public CellGrid cellGrid;
        List<RaycastHit> hits = new List<RaycastHit>();
        
        private void Start()
        {
            startingPos = this.gameObject.transform.position;
        }

        void Update()
        {
            if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width * (1 - ScrollEdge))
            {
                
                transform.Translate(transform.right * Time.deltaTime * ScrollSpeed, Space.World);
            }
            else if (Input.GetKey("a") || Input.mousePosition.x <= Screen.width * ScrollEdge)
            {
                transform.Translate(transform.right * Time.deltaTime * -ScrollSpeed, Space.World);
            }
            if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height * (1 - ScrollEdge))
            {
                Vector3 panMove = this.transform.up;
                panMove.y = 0f;
                transform.Translate(panMove * Time.deltaTime * ScrollSpeed, Space.World);
            }
            if (Input.GetKey("s") || Input.mousePosition.y <= Screen.height * ScrollEdge)
            {
                Vector3 panMove = this.transform.up;
                panMove.y = 0f;
                transform.Translate(panMove * Time.deltaTime * -ScrollSpeed, Space.World);
            }
            else if (Input.GetKey("space"))
            {
                this.gameObject.transform.position = startingPos;
            }
            if (transform.position.x > maxPosition.x)
            {
                Vector3 temp = transform.position;
                temp.x = maxPosition.x;
                transform.position = temp;
            }
            if (transform.position.z > maxPosition.y)
            {
                Vector3 temp = transform.position;
                temp.z = maxPosition.y;
                transform.position = temp;
            }
            if (transform.position.x < minPosition.x)
            {
                Vector3 temp = transform.position;
                temp.x = minPosition.x;
                transform.position = temp;
            }
            if (transform.position.z < minPosition.y)
            {
                Vector3 temp = transform.position;
                temp.z = minPosition.y;
                transform.position = temp;
            }
            //call method to show/hide objects
            HideObjects();
        }

        public void HideObjects()
        {
            //if Hits has content, for each object in list enable the renderer
            if (hits != null)
            {
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.GetComponent<Renderer>() != null)
                    {
                        hit.collider.GetComponent<Renderer>().enabled = true;
                    }
                }
            }
            //empty Hits
            hits.Clear();
            //for each unit belonging to the currently active player, add objects between them and camera to the list
            if (!overWorldCamera)
            {
                foreach (Unit unit in cellGrid.GetCurrentPlayerUnits())
                {
                    Debug.DrawRay(this.transform.position, (unit.transform.position - this.transform.position), Color.magenta);

                    hits.AddRange(Physics.RaycastAll(this.transform.position, (unit.transform.position - this.transform.position), Vector3.Distance(this.transform.position, unit.transform.position)));
                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.collider.GetComponent<ExperimentalUnit>() == null && hit.collider.GetComponentInParent<ExperimentalUnit>() == null && hit.collider.GetComponent<BlockCell>() == null)
                        {
                            hit.collider.GetComponent<Renderer>().enabled = false;
                        }
                    }
                }
            }
            //make sure to exclude objects that are a child of the Unit
            //for each item in Hits, disable Renderer
        }
    }
}

