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
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("hihi");
    }


    void FixedUpdate()
    {
        Vector2 move = moveAction.ReadValue<Vector2>() * moveSpeed;
        this.transform.position = new Vector3(transform.position.x + move.x, transform.position.y + move.y, transform.position.z);

        if ((move.x < 0 && !sr.flipX) || (move.x > 0 && sr.flipX)) sr.flipX = !sr.flipX;

        if(move != Vector2.zero)
        {
            sock.SendMessage("position", JsonUtility.ToJson(move));
        }
    }

    private void OnDisable()
    {
        playerAc.Disable();
    }
}
