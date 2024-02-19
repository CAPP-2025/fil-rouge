using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DeathBarrier : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().LoseLife();
            other.GetComponent<Player>().Death();
            GameManager.Instance.ResetLevel(1.5f);
        }
        else if (other.CompareTag("Pet"))
        {
            other.GetComponent<Pet>().Death();
            other.gameObject.SetActive(false);
        }
        else
        {
            Destroy(other.gameObject);
        }
    }

}
