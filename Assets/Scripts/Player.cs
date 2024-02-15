using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer playerRenderer;
    public SpriteRenderer petRenderer;
    public CapsuleCollider2D capsuleCollider { get; private set; }
    public DeathAnimation deathAnimation { get; private set; }

    public bool dead => deathAnimation.enabled;
    public bool starpower { get; private set; }

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        deathAnimation = GetComponent<DeathAnimation>();
    }

    public void Hit()
    {
        if (!dead && !starpower)
        {
            Death();
        }
    }

    public void Death()
    {
        Camera.main.GetComponent<SideScrolling>().player = this.transform;
        playerRenderer.enabled = false;
        petRenderer.enabled = false;
        deathAnimation.enabled = true;

        GameManager.Instance.ResetLevel(3f);
    }

    public void Starpower()
    {
        StartCoroutine(StarpowerAnimation());
    }

    private IEnumerator StarpowerAnimation()
    {
        starpower = true;

        float elapsed = 0f;
        float duration = 10f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0) {
                playerRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }

            yield return null;
        }

        playerRenderer.spriteRenderer.color = Color.white;
        starpower = false;
    }

}
