using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputSystem_Actions inputActions;
    public InputSystem_Actions.PlayerActions playerAc;
    public InputAction moveAction;
    public InputAction interactAction;

    public float moveSpeed = 2f;

    public SpriteRenderer sr;
    public Animator animator;

    private GameObject manager;
    private SocksManager sock;

    private Rigidbody2D rb;

    private Vector2 prevPos;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        playerAc = inputActions.Player;
    }

    private void OnEnable()
    {
        playerAc.Enable();

        moveAction = playerAc.Move;
        interactAction = playerAc.Interact;

        interactAction.performed += OnInteract;
    }

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager");
        sock = manager.GetComponent<SocksManager>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        prevPos = rb.position;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("hihi");
    }


    void FixedUpdate()
    {
        Vector2 move = moveAction.ReadValue<Vector2>() * moveSpeed;
        rb.MovePosition(rb.position + move);

        Vector2 diff = rb.position - prevPos;

        Debug.Log(diff);

        if ((diff.x < 0 && !sr.flipX) || (diff.x > 0 && sr.flipX)) sr.flipX = !sr.flipX;

        if(diff != Vector2.zero)
        {
            animator.SetBool("isRunning", true);
            sock.SendMessage("position", JsonUtility.ToJson(move));
        }
        else animator.SetBool("isRunning", false);

        prevPos = rb.position;
    }

    private void OnDisable()
    {
        playerAc.Disable();
    }
}
