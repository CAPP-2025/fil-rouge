using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type
    {
        Coin,
        ExtraLife,
        MagicMushroom,
        Starpower,
    }

    public Type type;
    public AudioSource coin;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            Collect(other.gameObject);
        }
    }

    private void Collect(GameObject player)
    {
        switch (type)
        {
            case Type.Coin:
                coin.Play();
                GameManager.Instance.AddCoin();
                player.GetComponent<Player>().UpdateRoses();
                break;

            case Type.ExtraLife:
                player.GetComponent<Player>().GainLife();
                break;

            /*case Type.MagicMushroom:
                player.GetComponent<Player>().Grow();
                break;*/

            case Type.Starpower:
                player.GetComponent<Player>().Starpower();
                break;
            default:
                break;
        }

        Destroy(gameObject);
    }

}
