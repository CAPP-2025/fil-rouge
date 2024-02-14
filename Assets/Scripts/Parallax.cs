using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform mainCam;
    public Transform middleBG;
    public Transform sideBG;

    [SerializeField] public float imageLength;

    // Update is called once per frame
    void Update()
    {
        if (mainCam.position.x > middleBG.position.x)
            sideBG.position = middleBG.position + Vector3.right * imageLength;

        if (mainCam.position.x < middleBG.position.x)
            sideBG.position = middleBG.position + Vector3.left * imageLength;

        if (mainCam.position.x > sideBG.position.x || mainCam.position.x 
                < sideBG.position.x)
        {
            Transform tmp = middleBG;
            middleBG = sideBG;
            sideBG = tmp;
        }
    }
}
