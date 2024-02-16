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
        if (--HealthManager.health > 0)
        {
            StartCoroutine(GetHurt());
        }
        else if (!dead && !starpower)
        {
            Death();
        }
    }

    IEnumerator GetHurt() {
        Physics2D.IgnoreLayerCollision(3, 7, true);
        for (int i = 0; i < 6; i++) {
            yield return new WaitForSeconds(0.25F);
            playerRenderer.enabled = false;
            yield return new WaitForSeconds(0.25F);
            playerRenderer.enabled = true;
        }
        Physics2D.IgnoreLayerCollision(3, 7, false);
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
