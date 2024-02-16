using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterBG : MonoBehaviour
{
    public Transform mainCam;
    public Transform BG;

    // Start is called before the first frame update
    void Start()
    {
        BG.position = new Vector3(mainCam.position.x, mainCam.position.y, 
            BG.position.y);   
    }

    // Update is called once per frame
    void Update()
    {
        BG.position = new Vector3(mainCam.position.x, mainCam.position.y, 
            BG.position.y);   
    }
}
