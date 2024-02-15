using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    public Player player;

    public void Hit()
    {
        player.Hit();
        GetComponent<PlayerMovement>().Swap();
    }
}
