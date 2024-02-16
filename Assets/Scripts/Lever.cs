using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lever : MonoBehaviour
{
    private bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        // disable animator
        GetComponent<Animator>().enabled = false;
        triggered = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // if the Collider2D is triggered by a player, call the Activate method
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.gameObject.CompareTag("Player"))
        {
            // write "lever activated" to the console
            Debug.Log("lever activated");
            // enable the animator
            GetComponent<Animator>().enabled = true;
            // set the triggered variable to true
            triggered = true;
        }
    }
}
