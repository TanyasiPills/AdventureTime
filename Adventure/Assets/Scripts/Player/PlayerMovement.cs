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

    private GameObject manager;
    private SocksManager sock;

    private Rigidbody2D rb;

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
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("hihi");
    }


    void FixedUpdate()
    {
        Vector2 move = moveAction.ReadValue<Vector2>() * moveSpeed;

        RaycastHit2D[] hits = new RaycastHit2D[1];
        int hitCount = rb.Cast(move, hits, move.magnitude);

        if(hitCount == 0)
        {
            rb.MovePosition(rb.position + move);

            if ((move.x < 0 && !sr.flipX) || (move.x > 0 && sr.flipX)) sr.flipX = !sr.flipX;

            if (move != Vector2.zero)
            {
                sock.SendMessage("position", JsonUtility.ToJson(move));
            }
        }
        
    }

    private void OnDisable()
    {
        playerAc.Disable();
    }
}
