using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsBlocker : MonoBehaviour
{
    //list of the doors
    public GameObject[] doors;
    //disable the doors if the corresponding previous level is not completed
    private void Start()
    {
        for (int i = 1; i < doors.Length; i++)
        {
            if (GameManager.Instance.levels[i - 1] == false)
            {
                doors[i].SetActive(false);
            }
        }
    }
    
}
