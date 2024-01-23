using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    private int hitTargets = 0;
    public List<GameObject> pathDisplay;

    //Public materials to assign in the inspector
    public Material unSelectedStar;
    public Material unSelectedLane;
    public Material selected;

    void Update()
    {
        pathDisplay = Dijkstras.path;
        //cast a ray of the user left-clicks
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.blue, 10f);

                if (Array.IndexOf(GenSystem.GetStars(), hit.collider.gameObject) >= 0)
                {
                    //Updates the selectedStar array with the new selection
                    DeselectStar(UI_Controller.selectedStars[hitTargets % 2]);
                    UI_Controller.selectedStars[hitTargets % 2] = hit.collider.gameObject;
                    SelectStar(hit.collider.gameObject);
                    hitTargets++;
                }
                
            }

            if (UI_Controller.selectedStars[0] != null && UI_Controller.selectedStars[1] != null && UI_Controller.selectedStars[0] != UI_Controller.selectedStars[1])
            {
                //If two stars are currently selected then start the pathfinding and then highlight the path found
                Dijkstras.StartPathFind(UI_Controller.selectedStars);
                HighlightCurrentPath(true);
                
            }
        }
    }

    //Change the material to show the star has been selected
    private void SelectStar(GameObject star)
    {
        star.GetComponent<MeshRenderer>().material = selected;
    }

    //Change the material to show the star has been deselected
    private void DeselectStar(GameObject star)
    {
        if (star != null)
        {
            star.GetComponent<MeshRenderer>().material = unSelectedStar;
        }
    }

    //Resets the materials of all stars and star lanes to default 
    public void ClearStars()
    {
        DeselectStar(UI_Controller.selectedStars[0]);
        DeselectStar(UI_Controller.selectedStars[1]);
        HighlightCurrentPath(false);
        UI_Controller.selectedStars = new GameObject[2];
        hitTargets = 0;
        Camera.main.GetComponent<Raycaster>().hitTargets = 0;
        Dijkstras.distance = 0;
        Dijkstras.pathFound = false;
    }

    private void HighlightCurrentPath(bool newPath)
    {
        //Resest all star lanes to default material
        for (int i = 0; i < StarLaneCreator.starLanes.Length; i++)
        {
            if (StarLaneCreator.starLanes[i] != null)
            {
                LineRenderer lane = StarLaneCreator.starLanes[i].GetComponent<LineRenderer>();
                lane.material = unSelectedLane;
            }
        }

        if (newPath) //If a new path needs to be displayed then find the appropriate lanes and change the material
        {
            List<GameObject> path = Dijkstras.path;

            for (int i = 0; i < path.Count - 1; i++)
            {
                foreach (GameObject line in StarLaneCreator.starLanes)
                {
                    if (line != null)
                    {
                        LineRenderer lane = line.GetComponent<LineRenderer>();
                        if ((path[i].transform.position == lane.GetPosition(0) || path[i].transform.position == lane.GetPosition(1)) && (path[i + 1].transform.position == lane.GetPosition(0) || path[i + 1].transform.position == lane.GetPosition(1)))
                        {
                            lane.material = selected;
                            break;
                        }
                    }
                }
            }
        }
    }
}
