using UnityEngine;

public class SnakeMovement : MonoBehaviour {
    public float moveSpeed = 5f; // Speed of movement
    public Vector2Int gridSize = new Vector2Int(32, 32); // Grid size
    private Vector2Int direction = Vector2Int.zero; // Default direction

    private float moveTimer = 0f;
    public float moveInterval = 0.5f; // Time between movement steps

    void Update() {
        HandleInput();
    }

    void FixedUpdate() {
        MoveSnake();
    }

    void HandleInput() {
        if (Input.GetKeyDown(KeyCode.UpArrow) && direction != Vector2Int.down)
            direction = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow) && direction != Vector2Int.up)
            direction = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != Vector2Int.right)
            direction = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow) && direction != Vector2Int.left)
            direction = Vector2Int.right;
    }

    void MoveSnake() {
        moveTimer += Time.fixedDeltaTime;
        if (moveTimer >= moveInterval) {
            transform.position += new Vector3(direction.x * gridSize.x, direction.y * gridSize.y, 0);
            moveTimer = 0f;
        }
    }
}
