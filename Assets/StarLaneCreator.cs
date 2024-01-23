using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class StarLaneCreator : MonoBehaviour
{
    //Defining array variables for all the star lanes
    private static bool[,] adjMat;

    public GameObject linePre;
    public static GameObject[] starLanes;
    private Transform[,] laneTransforms;
    private int[] maxLanes;
    private int maxLanesNum;
    private float[] laneDists;

    void Start()
    {
        //Fill the arrays with default values
        adjMat = new bool[GenSystem.numStars, GenSystem.numStars];
        maxLanes = new int[GenSystem.numStars];
        starLanes = new GameObject[GenSystem.numStars * 4];
        laneDists = new float[GenSystem.numStars * 3];
        laneTransforms = new Transform[starLanes.Length, 2];
        maxLanesNum = 4;
        CreateLanes(GenSystem.GetStars());
    }

    public void CreateLanes(GameObject[] stars)
    {
        for (int star = 0; star < stars.Length; star++)
        {
            for (int otherStar = 0; otherStar < stars.Length; otherStar++)
            {
                if (star != otherStar)//For each star, iteralte through all stars and check if the stars are different
                {
                    float distSquare = TwoPosPythag(stars[star].transform, stars[otherStar].transform);
                    if (distSquare < 49 && distSquare > 4 && adjMat[star, otherStar] != true && maxLanes[star] < maxLanesNum && maxLanes[otherStar] < maxLanesNum)
                    {
                        //If the stars are within a specified distance and the number of lanes for the current selected star isnt as maximum then create a new lane
                        adjMat[star, otherStar] = true;
                        adjMat[otherStar, star] = true;
                        maxLanes[star]++;
                        maxLanes[otherStar]++;
                        GameObject lane = Instantiate(linePre);


                        for (int i = 0; i < starLanes.Length; i++)
                        {
                            if (starLanes[i] == null)
                            {
                                //Add the data for the star lane to the arrays
                                starLanes[i] = lane;
                                laneTransforms[i, 0] = stars[star].transform;
                                laneTransforms[i, 1] = stars[otherStar].transform;
                                laneDists[i] = distSquare;
                                break;
                            }
                        }

                        
                        
                    }
                }
            }
        }
    }



    void Update()
    {
        //Ensure the lanes are in the correct position
        for (int i = 0; i < starLanes.Length; i++)
        {
            if (starLanes[i] != null)
            {
                LineRenderer lane = starLanes[i].GetComponent<LineRenderer>();
                lane.SetPosition(0, laneTransforms[i, 0].position);
                lane.SetPosition(1, laneTransforms[i, 1].position);
            }
        }
    }

    //Misc functions
    static float TwoPosPythag(Transform a, Transform b)
    {
        return Square(a.position.x - b.position.x) + Square(a.position.y - b.position.y) + Square(a.position.z - b.position.z);
    }

    static float Square(float num)
    {
        return num * num;
    }

    public static bool[,] GetAdjMat()
    {
        return adjMat;
    }


    
}
