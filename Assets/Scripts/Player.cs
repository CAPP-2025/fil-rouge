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
    public bool immune { get; private set;}
    public GameObject[] hearts = new GameObject[3];
    public GameObject[] emptyHearts = new GameObject[3];

    // roses count is a textmeshpro text
    public TMPro.TextMeshProUGUI rosesCount;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        deathAnimation = GetComponent<DeathAnimation>();
        UpdateLives();
    }

    private void Start()
    {
        UpdateRoses();
    }

    public void UpdateRoses()
    {
        rosesCount.text = $"x {GameManager.Instance.coins:000}";
    }

    public void Hit()
    {
        if (immune) return;
        LoseLife();
        if (GameManager.lives > 0)
        {
            StartCoroutine(GetHurt());
        }
        else if (!dead && !starpower)
        {
            Death();
        }
    }

    public void GainLife()
    {
        if (GameManager.lives < 3)
        {
            GameManager.Instance.AddLife();
            hearts[GameManager.lives - 1].SetActive(true);
            emptyHearts[GameManager.lives - 1].SetActive(false);
        }
    }

    public void LoseLife()
    {
        if (GameManager.lives > 0)
        {
            GameManager.Instance.LoseLife();
            hearts[GameManager.lives].SetActive(false);
            emptyHearts[GameManager.lives].SetActive(true);
        }
    }

    public void UpdateLives()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i < GameManager.lives)
            {
                hearts[i].SetActive(true);
                emptyHearts[i].SetActive(false);
            }
            else
            {
                hearts[i].SetActive(false);
                emptyHearts[i].SetActive(true);
            }
        }
    }

    IEnumerator GetHurt() {
        immune = true;
        Physics2D.IgnoreLayerCollision(3, 7, true);
        for (int i = 0; i < 6; i++) {
            yield return new WaitForSeconds(0.25F);
            playerRenderer.enabled = false;
            yield return new WaitForSeconds(0.25F);
            playerRenderer.enabled = true;
        }
        Physics2D.IgnoreLayerCollision(3, 7, false);
        immune = false;
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
