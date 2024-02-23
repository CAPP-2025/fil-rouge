using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Sprite flatSprite;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower) {
                Hit();
            } else if (collision.transform.DotTest(transform, Vector2.down)){
                Flatten();
            } else {
                player.Hit();
            }
        }
        if (collision.gameObject.CompareTag("Pet")) {
            Pet pet = collision.gameObject.GetComponent<Pet>();
            pet.Hit();
        }
    }

    private void Flatten()
    {
        GetComponent<Collider2D>().enabled = false;
        if (GetComponent<EntityMovement>() != null)
            GetComponent<EntityMovement>().enabled = false;
        else
            GetComponent<EntityMovementNoFall>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = flatSprite;
        Destroy(gameObject, 0.5f);
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;
        Destroy(gameObject, 3f);
    }

}
