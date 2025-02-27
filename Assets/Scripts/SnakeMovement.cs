using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    public Vector2Int gridSize = new Vector2Int(1, 1); // Grid size
    private Vector2Int direction = Vector2Int.zero; // Default direction

    private float moveTimer = 0f;
    public float moveInterval = 0.5f; // Time between movement steps

    private bool inputReceived = false;

    void Update() {
        HandleInput();
    }

    // fixedUpdate is called at a fixed interval, independent of frame rate
    void FixedUpdate() {
        MoveSnake();
    }

    void HandleInput() {
        // use the size of the snake to determine the grid size
        if (Input.GetKeyDown(KeyCode.UpArrow) && direction != Vector2Int.down && direction != Vector2Int.up) {
            direction = Vector2Int.up;
            inputReceived = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && direction != Vector2Int.up && direction != Vector2Int.down) {
            direction = Vector2Int.down;
            inputReceived = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != Vector2Int.right && direction != Vector2Int.left) {
            direction = Vector2Int.left;
            inputReceived = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && direction != Vector2Int.left && direction != Vector2Int.right) {
            direction = Vector2Int.right;
            inputReceived = true;
        }
    }

    void MoveSnake()
    {
        moveTimer += Time.fixedDeltaTime;
        if (moveTimer >= moveInterval || inputReceived == true) {
            transform.position += new Vector3(direction.x * gridSize.x, direction.y * gridSize.y, 0);
            moveTimer = 0f;
            inputReceived = false;
        }
    }
}
