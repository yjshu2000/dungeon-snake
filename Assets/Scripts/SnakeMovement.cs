using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    public Vector2Int gridSize = new Vector2Int(1, 1); // Grid size
    private Vector2Int direction = Vector2Int.zero; // Default direction

    private float moveTimer = 0f;
    public float moveInterval = 0.2f; // Time between movement steps

    private bool inputReceived = false;

    public GameObject segmentPrefab; // Prefab for a single snake segment
    private List<Transform> segments = new List<Transform>(); // List of snake body segments

    void Start() {
        // Initialize with head as first segment
        segments.Add(transform);
    }

    void Update() {
        HandleInput();

        // TEST FUNCTION: Press 'G' to grow snake manually
        if (Input.GetKeyDown(KeyCode.G)) {
            GrowSnake();
        }
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

    void MoveSnake() {
        moveTimer += Time.fixedDeltaTime;
        if (moveTimer >= moveInterval || inputReceived == true) {
            // Move each segment to the position of the previous segment
            for (int i = segments.Count - 1; i > 0; i--) {
                segments[i].position = segments[i - 1].position;
            }
            // Move the head in the current direction
            transform.position += new Vector3(direction.x * gridSize.x, direction.y * gridSize.y, 0);

            moveTimer = 0f;
            inputReceived = false;
        }
    }

    public void GrowSnake() {
        for (int i = 0; i < 4; i++) { // Add 4 segments 
            Vector3 newSegmentPosition = segments[segments.Count - 1].position;
            GameObject newSegment = Instantiate(segmentPrefab, newSegmentPosition, Quaternion.identity);
            segments.Add(newSegment.transform);
        }
    }
}
