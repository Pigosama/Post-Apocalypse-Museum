using UnityEngine;

enum PlayerState
{
    Normal,
    Crouching,
    Attacking,
    UsingItem,
    Interacting,
    Damaged
}

interface IInteractable
{
    void Interact();
}

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    [SerializeField] private InteractionDetector detector;
    private Animator animator;

    private float movement;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        //detector = GetComponent<InteractionDetector>();

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.E))
        {
            detector.Interact();
        }
    }

    private void Move()
    {
        movement = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(movement));

        if(movement != 0)
        {
            animator.SetFloat("Direction", movement);
        }
    }

    // DIPICTION
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movement * speed, rb.linearVelocity.y);
    }

}
