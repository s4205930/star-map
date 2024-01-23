using UnityEngine;

public class GenSystem : MonoBehaviour
{
    //Defining the prefabs for instantiastion
    public GameObject starPre;
    public GameObject planetPre;
    public GameObject blackHole;
    public GameObject linePre;

    //Variables affecting the topology of the galaxy
    private float galaxyWidth = 55f;
    private float galaxyHeight = 8f;
    private float orbitRange = 1f;
    private float minStarRange = 3f;
    
    public const int numStars = 600;

    //Arrays for the stars
    private static GameObject[] stars;
    private string[] starNames;

    void Start(){

        stars = new GameObject[numStars];
        //fill the starNames array with strings from the CSV file
        ReadCSV();


        for (int i = 0; i < numStars; i++) // loops through the number of starts to instantiate each one
        {
            //creates a new star using the prefab and gives it a random loaction withing the bounds of the galaxy
            GameObject star = Instantiate(starPre, new Vector3(Random.Range(-galaxyWidth, galaxyWidth), Random.Range(-galaxyHeight, galaxyHeight),
                Random.Range(-galaxyWidth, galaxyWidth)), Random.rotation, blackHole.transform);

            //two booleans that find the absolute distance from the black hold at the centre of the galaxy
            bool minDist = (Square(star.transform.localPosition.x) + Square(star.transform.localPosition.z)) < Square(minStarRange);
            bool maxDist = (Square(star.transform.localPosition.x) + Square(star.transform.localPosition.z)) > Square(galaxyWidth);

            while (minDist || maxDist)
            {
                //Gives the star a new random location if either of the booleans are true and repeats until all the parameters are met
                star.transform.localPosition = new Vector3(Random.Range(-galaxyWidth, galaxyWidth), Random.Range(-galaxyHeight, galaxyHeight),
                Random.Range(-galaxyWidth, galaxyWidth));

                minDist = (Square(star.transform.localPosition.x) + Square(star.transform.localPosition.z)) < Square(minStarRange);
                maxDist = (Square(star.transform.localPosition.x) + Square(star.transform.localPosition.z)) > Square(galaxyWidth);
            }

            //Gives the star its name from the CSV and adds it to the array of stars
            star.name = starNames[i];
            stars[i] = star;
            //Gives the star the rotation component so the planets added after give the effect of orbiting
            star.AddComponent<Rotation>();
            GivePlanets(star);
        }

    }

    void GivePlanets(GameObject star)
    {
        for (int j = 0; j < Random.Range(2, 20); j++)// Similar to the star instantiate but uses the smaller planet prefab and is repeated a random numer of times
        {
            GameObject planet = Instantiate(planetPre, star.transform.position, Random.rotation, star.transform);

            //Repeats till the location of the plane meets requirements
            while (Mathf.Abs(planet.transform.localPosition.x) < 0.5f || Mathf.Abs(planet.transform.localPosition.y) > 0.15f || Mathf.Abs(planet.transform.localPosition.z) < 0.5f)//change to better col det
            {
                planet.transform.localPosition = new Vector3(Random.Range(-orbitRange, orbitRange), Random.Range(-0.15f, 0.15f),
                Random.Range(-orbitRange, orbitRange));
            }
        }
    }

    void ReadCSV()
    {
        //Reads the CSV file
        starNames = System.IO.File.ReadAllLines(@"STARS.csv");
    }

    //Miscellaneous functions
    public static GameObject[] GetStars()
    {
        return stars;
    }

    public float Square(float num)
    {
        return num * num;
    }
}
