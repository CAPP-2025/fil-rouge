using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickLevel : MonoBehaviour
{
    public GameManager game;
    public GameObject player;

    private Animator anim;

    private float half_door;
    private float door_pos;

    void Start() {
        this.half_door = transform.localScale.x / 2;
        this.door_pos = transform.position.x;
        anim = GetComponent<Animator>();
    }

    void Update() {
        if (Input.GetKey(KeyCode.UpArrow)) {
            float player_pos = player.transform.position.x;
            if (player_pos > door_pos - half_door && player_pos < door_pos + half_door) {
                anim.Play("door_animation");
                game.menu = false;
                game.LoadLevel(
                    name[0] - '0', 
                    name[1] - '0');
            }
        }
    }
}
