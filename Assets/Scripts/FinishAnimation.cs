using System.Collections;
using UnityEngine;

public class FinishAnimation : MonoBehaviour
{
    public Transform bottom;
    public Transform destination;
    public float speed = 15f;
    public GameObject birds;

    private GameObject player;
    public int nextWorld = 1;
    public int nextStage = 1;
    public AudioSource audioSource;

    private void Start()
    {
        birds.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            Debug.Log("Level complete!");
            audioSource.Play();
            StartCoroutine(LevelCompleteSequence());
        }
    }

    private IEnumerator LevelCompleteSequence()
    {
        // get the player's renderer, it is a child of the player object
        GameObject playerRenderer = player.transform.GetChild(0).gameObject;
        // get the player's pet
        GameObject pet = player.GetComponent<FollowPlayer>().pet;

        player.GetComponent<PlayerMovement>().enabled = false;
        Camera.main.GetComponent<SideScrolling>().enabled = false;


        pet.GetComponent<PlayerMovement>().enabled = false;

        // ground the pet (apply only the gravity)
        //pet.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        //move the pet down to the ground



        yield return MoveTo(player.transform, bottom.position);

        birds.SetActive(true);
        // for each child in the birds object, enable the AnimatedSprite component
        foreach (AnimatedSprite a in birds.GetComponentsInChildren<AnimatedSprite>())
        {
            a.enabled = true;
        }

        // move the birds to the player's position + 1 on the y axis
        yield return MoveTo(birds.transform, player.transform.position);
        player.GetComponent<FollowPlayer>().enabled = false;
        
        // make the birds fly away with the Player
        StartCoroutine(MoveTo(birds.transform, destination.position));
        yield return MoveTo(player.transform, destination.position);

        GameManager.Instance.NextLevel();
    }

    private IEnumerator MoveTo(Transform subject, Vector3 position)
    {
        while (Vector3.Distance(subject.position, position) > 0.125f)
        {
            subject.position = Vector3.MoveTowards(subject.position, position, speed * Time.deltaTime);
            yield return null;
        }

        subject.position = position;
    }

}
