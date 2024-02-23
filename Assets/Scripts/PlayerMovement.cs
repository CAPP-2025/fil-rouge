using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private new Camera camera;
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    [SerializeField] private AudioSource killed;
    [SerializeField] private AudioSource jumpChampi;

    public Vector2 velocity;
    private float inputAxis;
    public PlayerMovement otherMovement;
    public FollowPlayer followPlayer;
    public bool isMain = true;
    private Transform originalParent;

    // speed constants
    public float moveSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity;

    // state assessments
    public bool grounded ;//{ get; private set; }
    public bool jumping { get; private set; }
    private float walljumpTimer = 0f;
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;
    public bool falling => velocity.y < 0f && !grounded;
    private bool sinking = false;
    private bool moving = false;
    public bool gamePaused = false;

    public AudioSource bonkSound;

    private void Awake()
    {
        gravity = (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f);
        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        originalParent = transform.parent;
        if (!isMain)
        {
            enabled = false;
        }
    }

    private void OnEnable()
    {
        rigidbody.isKinematic = false;
        collider.enabled = true;
        velocity = Vector2.zero;
        jumping = false;
    }

    private void OnDisable()
    {
        rigidbody.isKinematic = true;
        collider.enabled = false;
        velocity = Vector2.zero;
        inputAxis = 0f;
        jumping = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused;
            if (gamePaused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
        if (walljumpTimer > 0f) {
            walljumpTimer -= Time.deltaTime;
        }

        //cast down and at an angle to check for ground and walls
        if (isMain)
        {
            grounded = (rigidbody.Raycast(Vector2.down)
            || rigidbody.Raycast(Vector2.down + Vector2.right + Vector2.down)
            || rigidbody.Raycast(Vector2.down + Vector2.left + Vector2.down))
            && (collider.IsTouchingLayers(LayerMask.GetMask("Default", "Moving", "Water")));
        }
        else
        {
            grounded = (rigidbody.Raycast(Vector2.down, 0.375f, 0.12f)
            || rigidbody.Raycast(Vector2.down + Vector2.right + Vector2.down, 0.53f, 0.1f)
            || rigidbody.Raycast(Vector2.down + Vector2.left + Vector2.down, 0.53f, 0.1f))
            && (collider.IsTouchingLayers(LayerMask.GetMask("Default", "Moving", "Water")));
        }

        grounded = grounded || moving;
        if (grounded) {
            walljumpTimer = 0f;
            //GroundedMovement();
            if (!sinking || !moving)
                velocity.y = Mathf.Max(velocity.y, 0f);
            jumping = velocity.y > 0f;

            // perform jump
            if (Input.GetButtonDown("Jump"))
            {
                ResetParent();
                velocity.y = jumpForce;
                jumping = true;
            }
        }
        if (rigidbody.Raycast(Vector2.right) && collider.IsTouchingLayers(LayerMask.GetMask("Default")) && !grounded) {
            WallMovement(true);
            if (velocity.x > 0f) {
                velocity.x = 0f;
            }
        }
        if (rigidbody.Raycast(Vector2.left) && collider.IsTouchingLayers(LayerMask.GetMask("Default")) && !grounded) {
            WallMovement(false);
            if (velocity.x < 0f) {
                velocity.x = 0f;
            }
        }
        HorizontalMovement();
        ApplyGravity();

        if (Input.GetKeyDown(KeyCode.PageDown) && grounded && inputAxis == 0f && collider.IsTouchingLayers(LayerMask.GetMask("Default")))
        {
            Swap();
        }
    }

    public void Swap()
    {
        otherMovement.enabled = true;
        enabled = false;
        followPlayer.enabled = !isMain;
        camera.GetComponent<SideScrolling>().player = otherMovement.transform;
    }

    private void FixedUpdate/*Position*/()
    {
        Vector2 position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;

        // // clamp within the screen bounds
        Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        rigidbody.MovePosition(position);
    }

    private void HorizontalMovement()
    {
        if (walljumpTimer <= 0f)
        {
            // accelerate / decelerate
            inputAxis = Input.GetAxis("Horizontal");
            if (inputAxis != 0f)
            {
                ResetParent();
            }
            velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * moveSpeed, moveSpeed * Time.deltaTime);
        }

        //stop horizontal movement if grounded and not pressing anything
        if ((inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f) || (inputAxis == 0f && grounded)) {
            velocity.x = 0f;
        }

        // flip sprite to face direction
        if (velocity.x > 0f) {
            transform.eulerAngles = Vector3.zero;
        } else if (velocity.x < 0f) {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private void GroundedMovement()
    {
        // prevent gravity from infinitly building up
        if (!sinking)
            velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;

        // perform jump
        if (Input.GetButtonDown("Jump"))
        {
            ResetParent();
            velocity.y = jumpForce;
            jumping = true;
        }
    }

    private void WallMovement(bool right)
    {
        // perform jump and push away from wall
        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            inputAxis = right ? -1f : 1f;
            velocity.x = inputAxis * moveSpeed;
            jumping = true;
            walljumpTimer = 0.5f;
        }
    }

    private void ApplyGravity()
    {
        // check if falling
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;

        // apply gravity and terminal velocity
        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (isMain && transform.DotTest(collision.transform, Vector2.down))
            {
                // bounce back up from the enemy's head, and kill the enemy
                velocity.y = jumpForce;
                jumping = true;
                jumpChampi.Play();
            }
            else
                killed.Play();
        }
        else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp") && collision.gameObject.layer != LayerMask.NameToLayer("Water"))
        {
            // stop vertical movement if mario bonks his head
            if (!sinking)
            {
                if (isMain)
                {
                    if (rigidbody.Raycast(Vector2.up, 1.45f, 0.39f))
                    {
                        bonkSound.Play();
                        velocity.y = 0f;
                    }
                }
                else
                {
                    if (rigidbody.Raycast(Vector2.up, 0.375f, 0.375f))
                    {
                        bonkSound.Play();
                        velocity.y = 0f;
                    }
                }
            }
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Moving"))
        {
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                if (collision.gameObject.transform.parent.GetComponent<MovingPlatform>().direction != Vector2.up)
                {
                    transform.parent = collision.transform;
                }
                else
                {
                    ResetParent();
                }
            }
        }
    }

    private void ResetParent()
    {
        transform.parent = originalParent;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Moving"))
        {
            ResetParent();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Moving"))
        {
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                if (collision.gameObject.transform.parent.GetComponent<MovingPlatform>().direction != Vector2.up)
                {
                    transform.parent = collision.transform;
                }
                else
                {
                    ResetParent();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            sinking = true;
            maxJumpHeight = 2f;
            maxJumpTime = 0.5f;
            moveSpeed = 3f;
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("MovingRoot"))
        {
            moving = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            // if more than 1/2 of the player is in the collider, reduce jump height to 1f
            if (collider.bounds.max.y > transform.position.y)
            {
                maxJumpHeight = 1f;
            }
            else
            {
                maxJumpHeight = 2f;
            }
            if (velocity.y < 0f)
            {
                velocity.y /= 10f;
            }
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("MovingRoot"))
        {
            moving = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            sinking = false;
            maxJumpHeight = 5f;
            maxJumpTime = 1f;
            moveSpeed = 8f;
            gravity = (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f);
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("MovingRoot"))
        {
            moving = false;
        }
    }
}
