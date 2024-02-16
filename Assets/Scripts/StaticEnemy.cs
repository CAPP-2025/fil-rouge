using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.Hit();
        }
        if (collision.gameObject.CompareTag("Pet")) {
            Pet pet = collision.gameObject.GetComponent<Pet>();
            pet.Hit();
        }
    }
}
