using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    void Update()
    {
        //Slowly roate a game object
        float newRot = Time.time * -25;
        transform.localEulerAngles = new Vector3(0, newRot, 0);
    }
}
