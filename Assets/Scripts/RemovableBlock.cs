using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovableBlock : MonoBehaviour
{
    public Lever lever;

    void Update()
    {
        if (lever.triggered)
        {
            Destroy(gameObject);
        }
    }
}
