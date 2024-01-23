using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dijkstras : MonoBehaviour
{
    //Define lists and arrays to keep track of star data
    public static List<GameObject> visited = new List<GameObject>();
    static List<GameObject> unvisited = new List<GameObject>();
    public static List<GameObject> path = new List<GameObject>();

    static GameObject[] stars = GenSystem.GetStars();
    static bool[,] adjMat = StarLaneCreator.GetAdjMat();
    static float[] dists = new float[GenSystem.numStars];
    static GameObject[] previousStars = new GameObject[GenSystem.numStars];

    public static float distance;

    public static bool pathFound;
    public static void StartPathFind(GameObject[] targets)
    {
        bool completed = false;
        pathFound = false;

        GameObject startStar = targets[0];
        GameObject endStar = targets[1];
        GameObject currentStar = startStar;

        //Ensure data is clean so that previous runs do not effect the current path-find
        dists = new float[GenSystem.numStars];
        visited.Clear();
        unvisited.Clear();
        path.Clear();
        Array.Clear(previousStars, 0, previousStars.Length);
        Array.Clear(dists, 0, dists.Length);

        //Add all stars to the unvisited list and give all stars a distance of infinity
        for (int i = 0; i < stars.Length; i++)
        {
            unvisited.Add(stars[i]);
            dists[i] = float.PositiveInfinity;
        }
        //Set distance for start star at 0
        dists[GetIndex(startStar)] = 0;

        //While a path hasnt been found or all stars visited
        while (!completed)
        {
            if (currentStar == null) { break; }
            VisitStar(currentStar);
            
            if (currentStar == endStar)
            {
                completed = true;
                pathFound = true;
            }else if (CheckUnvisited())
            {
                completed = true;
                distance = 0;
            }

            currentStar = GetNextStar();

        }
        if (pathFound)
        {
            CreatePath(startStar, endStar);
        }
    }

    private static void VisitStar(GameObject currentStar)
    {
        //Remove this star fom the unvisited list
        unvisited.Remove(currentStar);
        //Generate a list of all unvisited neighbours
        List<GameObject> unVisNeighbours = GetUnvisitedNeighbours(currentStar);
        //Sorts the list into an order of ascending distance from the current star
        unVisNeighbours = SortList(currentStar, unVisNeighbours);

        for (int i = 0; i < unVisNeighbours.Count; i++)
        {
            float currDist = dists[GetIndex(unVisNeighbours[i])];
            float newDist = TruePythag(currentStar.transform, unVisNeighbours[i].transform) + dists[GetIndex(currentStar)];
            //Check the distance between the two stars from the distance array and update if this path is shorter
            if (currDist > newDist)
            {
                dists[GetIndex(unVisNeighbours[i])] = newDist;
                previousStars[GetIndex(unVisNeighbours[i])] = currentStar;
            }
        }
        //Add the current star to the visited list
        visited.Add(currentStar);
    }

    //Iterate through the unvisited list and return the star with the smallest distance
    private static GameObject GetNextStar()
    {
        float minDist = float.PositiveInfinity;
        int index = 0;

        for (int i = 0; i < dists.Length; i++)
        {
            if (dists[i] < minDist && unvisited.Contains(stars[i]))
            {
                minDist = dists[i];
                index = i;
            }
        }

        return stars[index];
    }

    //Sorts the listof stars from the passed list of targets into ascending distance
    private static List<GameObject> SortList(GameObject currentStar, List<GameObject> targets)
    {
        List<GameObject> sorted = new List<GameObject>();
        List<float> dists = new List<float>();
        
        foreach (GameObject target in targets)
        {
            dists.Add(TruePythag(currentStar.transform, target.transform));
        }

        while(targets.Count > 0)
        {
            float minDist = float.PositiveInfinity;
            foreach (float dist in dists)
            {
                if (dist < minDist)
                {
                    minDist = dist;
                }
            }

            int index = dists.IndexOf(minDist);
            sorted.Add(targets[index]);
            dists.RemoveAt(index);
            targets.RemoveAt(index);
        }

        return sorted;
    }

    //Checks for any unvisited stars that dont have a distance of infinity
    private static bool CheckUnvisited()
    {
        bool infinte = true;
        List<int> indexes = new List<int>();

        foreach (GameObject unvis in unvisited)
        {
            indexes.Add(GetIndex(unvis));
        }

        foreach (int index in indexes)
        {
            if (dists[index] != float.PositiveInfinity)
            {
                infinte = false;
                break;
            }
        }

        return infinte;
    }

    //Returns a list of neighbours from the passed star that havent been visited
    private static List<GameObject> GetUnvisitedNeighbours(GameObject star)
    {
        List<GameObject> neighbours = new List<GameObject> ();
        int starIndex = GetIndex(star);

        for (int i = 0; i < GenSystem.numStars; i++)
        {
            if (adjMat[starIndex, i] == true)
            {
                neighbours.Add(stars[i]);
            }
        }

        List<GameObject> toRemove = new List<GameObject>();

        foreach (GameObject neighbour in neighbours)
        {
            if (visited.Contains(neighbour))
            {
                toRemove.Add(neighbour);
            }
        }

        foreach (GameObject rem in toRemove)
        {
            neighbours.Remove(rem);
        }

        return neighbours;
    }

    //Return the index of the passed star
    private static int GetIndex(GameObject star)
    {
        for (int i = 0; i < GenSystem.numStars; i++)
        {
            if (stars[i] == star)
            {
                return i;
            }
        }
        return -1;
    }

    //Uses the previous stars list to find the optimal path between the end and start then reverse it.
    private static void CreatePath(GameObject startStar, GameObject endStar)
    {
        GameObject currentStar = endStar;
        path.Add(endStar);
        distance = dists[GetIndex(endStar)];

        while (currentStar != startStar)
        {
            GameObject nextInPath = previousStars[GetIndex(currentStar)];
            path.Add(nextInPath);
            currentStar = nextInPath;
        }

        path.Reverse();
    }

    //Misc functions
    private static float Square(float num)
    {
        return num * num;
    }

    private static float TruePythag(Transform a, Transform b)
    {
        return Mathf.Sqrt(Square(a.position.x - b.position.x) + Square(a.position.y - b.position.y) + Square(-a.position.z - b.position.z));
    }
}
