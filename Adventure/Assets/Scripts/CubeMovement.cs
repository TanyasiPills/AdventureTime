using UnityEngine;
using UnityEngine.InputSystem;

public class CubeMovement : MonoBehaviour
{
    public InputActionMap inputAction;
    public InputActionAsset asset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputAction = asset.FindActionMap("Player");
        inputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = inputAction.FindAction("Move").ReadValue<Vector2>() * Time.deltaTime;
        this.transform.position = new Vector3(transform.position.x + move.x, transform.position.y + move.y, transform.position.z);
    }

    private void OnDestroy()
    {
        inputAction.Disable();
    }
}
