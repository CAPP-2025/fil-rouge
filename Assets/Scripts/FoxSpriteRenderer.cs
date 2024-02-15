using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FoxSpriteRenderer : MonoBehaviour
{
    private PlayerMovement movement;
    public FollowPlayer followPlayer;
    public SpriteRenderer spriteRenderer { get; private set; }
    public Sprite idle;
    public Sprite jump;
    public AnimatedSprite run;

    private void Awake()
    {
        movement = GetComponentInParent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (movement.enabled)
        {
            run.enabled = movement.running;

            if (movement.jumping) {
                spriteRenderer.sprite = jump;
            } else if (!movement.running) {
                spriteRenderer.sprite = idle;
            }
        }
        else
        {
            run.enabled = followPlayer.running;

            if (followPlayer.jumping) {
                spriteRenderer.sprite = jump;
            } else if (!followPlayer.running) {
                spriteRenderer.sprite = idle;
            }
        }
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
        run.enabled = false;
    }

}
