using GridSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    [Header("Movement Settings")]
    public float moveSpeed = 10f;

    private GridConfig _gridConfig;

    [SerializeField]private Vector2 _minBounds;
    [SerializeField]private Vector2 _maxBounds;

    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private bool isInitialize = false;
    private void Awake()
    {
        // Initialize the input actions
        inputActions = new PlayerInputActions();

        // Subscribe to the Move action
        inputActions.Camera.Move.performed += OnMovePerformed;
        inputActions.Camera.Move.canceled += OnMoveCanceled;
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
    public void Init(GridConfig gridConfig)
    {
        _gridConfig = gridConfig;



        CalculateGridBounds();

        transform.position = new Vector3(0, 0, -10);
        isInitialize = true;
    }
    void Update()
    {
        if (isInitialize) HandleMovement();
    }
  
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        // Get the movement input as a Vector2
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // Reset the movement input when the key is released
        moveInput = Vector2.zero;
    }
    private void CalculateGridBounds()
    {
        // Calculate the total grid size in world space
        float totalGridWidth = _gridConfig.gridWorldSize.x * _gridConfig.cellSize;
        float totalGridHeight = _gridConfig.gridWorldSize.y * _gridConfig.cellSize;

        // Calculate the min and max bounds for the camera
        _minBounds = new Vector2(-totalGridWidth / 2f, -totalGridHeight / 2f);
        _maxBounds = new Vector2(totalGridWidth / 2f, totalGridHeight / 2f);


    }

    private void HandleMovement()
    {
        // Calculate the new position based on the input
        Vector3 newPosition = transform.position + new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed * Time.deltaTime;

        // Clamp the position within the grid bounds
        newPosition.x = Mathf.Clamp(newPosition.x, _minBounds.x, _maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, _minBounds.y, _maxBounds.y);

        // Update the camera's position
        transform.position = newPosition;
    }

}
