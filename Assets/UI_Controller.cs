using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{

    public static GameObject[] selectedStars = new GameObject[2];

    [SerializeField] private TextMeshProUGUI starA;
    [SerializeField] private TextMeshProUGUI starB;
    [SerializeField] private TextMeshProUGUI pathDist;

    void Update()
    {
        //If statement to control the star names UI. Displays the name if the variable isnt null
        if (selectedStars[0] == null)
        {
            starA.SetText("-");
        }
        else
        {
            starA.SetText(selectedStars[0].gameObject.name);
        }

        if (selectedStars[1] == null)
        {
            starB.SetText("-");
        }
        else
        {
            starB.SetText(selectedStars[1].gameObject.name);
        }

        //If two stars are selected then displays the distance. If there is no path it displays an appropriate message
        if (selectedStars[0] == null || selectedStars[1] == null)
        {
            pathDist.SetText("- Lightyears");
        }
        else if (!Dijkstras.pathFound)
        {
            pathDist.SetText("No Possible Path");
        }
        else
        {
            pathDist.SetText(Dijkstras.distance + " Lightyears");
        }
    }
}
