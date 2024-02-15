using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lever : MonoBehaviour
{
    // private triggered
    private bool triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        // disable animator
        GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // if the Collider2D is triggered by a player, call the Activate method
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !triggered)
        {
            Activate();
        }
    }

    // Activate the lever
    private void Activate()
    {
        // enable the animator
        GetComponent<Animator>().enabled = true;

        // set triggered to true
        triggered = true;

        // play the animation
        GetComponent<Animator>().Play("Lever_animation");

        Debug.Log("Lever activated");
    }
}
