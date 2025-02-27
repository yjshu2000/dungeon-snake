using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    private float moveTimer = 0f;
    public float moveInterval = 0.2f; // Time between movement steps
    public float moveSpeed = 5f; // Speed of movement
    public Vector2Int gridSize = new Vector2Int(1, 1); // Grid size
    private Vector2Int direction = Vector2Int.zero; // Default direction

    private bool inputReceived = false;

    public GameObject segmentPrefab; // Prefab for a single snake segment
    private List<Transform> snakeSegments = new List<Transform>(); // List of snake body segments

    private FoodSpawner foodSpawner;
    private FloorGridManager floorGridManager;

    void Start() {
        snakeSegments.Add(transform); // Add head as first segment, then add 1 body segment
        GameObject firstSegment = Instantiate(segmentPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        snakeSegments.Add(firstSegment.transform);
        foodSpawner = FindAnyObjectByType<FoodSpawner>();
        floorGridManager = FindAnyObjectByType<FloorGridManager>();
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
        if (direction == Vector2Int.zero) {
            return;
        }
        if (moveTimer >= moveInterval || inputReceived == true) {
            // Move each segment to the position of the previous segment
            for (int i = snakeSegments.Count - 1; i > 0; i--) {
                snakeSegments[i].position = snakeSegments[i - 1].position;
            }
            // Move the head in the current direction
            transform.position += new Vector3(direction.x * gridSize.x, direction.y * gridSize.y, 0);

            moveTimer = 0f;
            inputReceived = false;
        }
    }

    public void GrowSnake() {
        for (int i = 0; i < 4; i++) { // Add 4 segments 
            Vector3 newSegmentPosition = snakeSegments[snakeSegments.Count - 1].position;
            GameObject newSegment = Instantiate(segmentPrefab, newSegmentPosition, Quaternion.identity);
            snakeSegments.Add(newSegment.transform);
        }
    }

    public List<Vector2Int> GetSnakePositions() {
        List<Vector2Int> positions = new List<Vector2Int>();
        foreach (var segment in snakeSegments) {
            positions.Add(new Vector2Int((int)segment.position.x, (int)segment.position.y));
        }
        return positions;
    }

    // COLLISION HANDLING
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Food")) {
            Debug.Log("Hit a food!");
            Destroy(other.gameObject);
            foodSpawner.SpawnFood();
            GrowSnake();
            floorGridManager.ExpandBoard();
        }
        else if (other.CompareTag("Wall")) {
            // Implement damage or stop movement logic
            Debug.Log("Hit a wall!");
        }
        else if (other.CompareTag("SnakeBody")) {
            // Game over logic
            Debug.Log("Hit yourself!");
        }
    }
}
