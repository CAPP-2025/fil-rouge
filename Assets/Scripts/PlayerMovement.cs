using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private new Camera camera;
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    [SerializeField] private AudioSource killed;
    [SerializeField] private AudioSource jumpChampi;

    private Vector2 velocity;
    private float inputAxis;
    public PlayerMovement otherMovement;
    public FollowPlayer followPlayer;
    public bool isMain = true;

    // speed constants
    public float moveSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f);

    // state assessments
    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    private float walljumpTimer = 0f;
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;
    public bool falling => velocity.y < 0f && !grounded;

    private void Awake()
    {
        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
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
        jumping = false;
    }

    private void Update()
    {
        if (walljumpTimer > 0f) {
            walljumpTimer -= Time.deltaTime;
        }

        grounded = rigidbody.Raycast(Vector2.down);

        if (grounded) {
            walljumpTimer = 0f;
            GroundedMovement();
        }
        if (rigidbody.Raycast(Vector2.right) && !grounded) {
            WallMovement(true);
        }
        if (rigidbody.Raycast(Vector2.left) && !grounded) {
            WallMovement(false);
        }
        HorizontalMovement();

        ApplyGravity();
        UpdatePosition();
        if (Input.GetKeyDown(KeyCode.PageDown) && grounded && inputAxis == 0f)
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

    private void UpdatePosition()
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
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;

        // perform jump
        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            jumping = true;
        }
    }

    private void WallMovement(bool right)
    {
        // prevent horizontal movement from infinitly building up
        if (right && velocity.x > 0f || !right && velocity.x < 0f) {
            velocity.x = 0f;
        }
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
            // bounce off enemy head
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                // bounce back up from the enemy's head, and kill the enemy
                velocity.y = jumpForce;
                jumping = true;
                jumpChampi.Play();
            }
            else
                killed.Play();
        }
        else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            // stop vertical movement if mario bonks his head
            if (transform.DotTest(collision.transform, Vector2.up)) {
                velocity.y = 0f;
            }
        }
    }

}
