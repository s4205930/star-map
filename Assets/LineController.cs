using UnityEngine;

public class LineController : MonoBehaviour
{
    public LineRenderer line;

    void Start()
    {
        //Place lanes in default position until updated by another script
        line.positionCount = 2;

        line.SetPosition(0, new Vector3(0, 0, 0));
        line.SetPosition(1, new Vector3(10, 0, 0));
    }

}
