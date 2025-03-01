using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    private float moveTimer = 0f;
    public float moveInterval = 0.2f; // Time between movement steps
    public float moveSpeed = 5f; // Speed of movement
    public int growRate = 4; // Number of segments to grow by when eating food
    public Vector2Int gridSize = new Vector2Int(1, 1); // Grid size
    private Vector2Int direction = Vector2Int.zero; // Default direction

    private bool inputReceived = false;

    public GameObject segmentPrefab; // Prefab for a single snake segment
    private List<Transform> snakeSegments = new List<Transform>(); // List of snake body segments

    private FoodSpawner foodSpawner;
    private FloorGridManager floorGridManager;

    public bool IsAlive { get; private set; } = true;
    public bool IsMoving { get; private set; } = false;
    private int defaultSnakeHP = 3;
    public int SnakeHP { get; private set; }
    private Transform headSpriteTransform;

    void Start() {
        snakeSegments.Add(transform); // Add head as first segment, then add 1 body segment
        GameObject firstSegment = Instantiate(segmentPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        snakeSegments.Add(firstSegment.transform);
        foodSpawner = FindAnyObjectByType<FoodSpawner>();
        floorGridManager = FindAnyObjectByType<FloorGridManager>();
        IsAlive = true;
        IsMoving = false;
        SnakeHP = defaultSnakeHP;

        headSpriteTransform = transform.GetComponentInChildren<SpriteRenderer>().transform;
        headSpriteTransform.rotation = Quaternion.Euler(0, 0, -90);
    }

    void Update() {
        HandleInput();

        // TEST FUNCTION: Press 'G' to grow snake manually
        if (Input.GetKeyDown(KeyCode.G)) {
            GrowSnake(growRate);
        }
    }

    // fixedUpdate is called at a fixed interval, independent of frame rate
    void FixedUpdate() {
        MoveSnake();
    }

    void HandleInput() {
        if (!IsAlive) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && direction != Vector2Int.down && direction != Vector2Int.up) {
            direction = Vector2Int.up;
            inputReceived = true;
            RotateHeadSprite();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && direction != Vector2Int.up && direction != Vector2Int.down) {
            direction = Vector2Int.down;
            inputReceived = true;
            RotateHeadSprite();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != Vector2Int.right && direction != Vector2Int.left) {
            direction = Vector2Int.left;
            inputReceived = true;
            RotateHeadSprite();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && direction != Vector2Int.left && direction != Vector2Int.right) {
            direction = Vector2Int.right;
            inputReceived = true;
            RotateHeadSprite();
        }
    }

    void MoveSnake() {
        moveTimer += Time.fixedDeltaTime;
        if (!IsAlive) {
            return;
        }
        if (direction == Vector2Int.zero) {
            return;
        }
        IsMoving = true;
        if (moveTimer >= moveInterval || inputReceived == true) {
            inputReceived = false;
            if (IsMovingIntoWall()) {
                Dead();
                return;
            }
            // check for body collision
            List<Vector2Int> snakePositions = GetSnakePositions();
            for (int i = 1; i < snakePositions.Count; i++) {
                if (snakePositions[i] == new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y))) {
                    HitBody();
                    if (!IsAlive) {
                        return;
                    }
                }
            }
            // Move each segment to the position of the previous segment
            for (int i = snakeSegments.Count - 1; i > 0; i--) {
                snakeSegments[i].position = snakeSegments[i - 1].position;
            }
            // Move the head in the current direction
            transform.position += new Vector3(direction.x * gridSize.x, direction.y * gridSize.y, 0);

            UpdateBodySprites();

            moveTimer = 0f;
        }
    }

    private void UpdateBodySprites() {
        // Use the updated position to update the sprite direction from the second to the last segment
            for (int i = 1; i <= snakeSegments.Count - 1; i++) {
                Vector2Int prevSegmentPos = new Vector2Int((int)snakeSegments[i - 1].position.x, (int)snakeSegments[i - 1].position.y);
                    Vector2Int thisSegmentPos = new Vector2Int((int)snakeSegments[i].position.x, (int)snakeSegments[i].position.y);
                if (i == snakeSegments.Count - 1) {
                    // this is the last segment
                    // Set the sprite direction for the tail segment
                    snakeSegments[i].GetComponent<SegmentDirection>().SetSpriteDirectionTail(thisSegmentPos, prevSegmentPos);
                }
                else {
                    Vector2Int nextSegmentPos = new Vector2Int((int)snakeSegments[i + 1].position.x, (int)snakeSegments[i + 1].position.y);
                    // Set the sprite direction for the body segment
                    snakeSegments[i].GetComponent<SegmentDirection>().SetSpriteDirectionBody(prevSegmentPos, thisSegmentPos, nextSegmentPos);
                }
            }
    }

    private bool IsMovingIntoWall() {
        Vector2Int nextPosition = new Vector2Int(
                Mathf.RoundToInt(transform.position.x) + direction.x * gridSize.x,
                Mathf.RoundToInt(transform.position.y) + direction.y * gridSize.y
            );
        return !floorGridManager.IsValidTilePosition(nextPosition);
    }

    public void GrowSnake(int segments) {
        for (int i = 0; i < segments; i++) { // Add 4 segments 
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
            GrowSnake(growRate);
            floorGridManager.ExpandBoard();
        }
        else if (other.CompareTag("Wall")) {
            // Implement damage or stop movement logic
            Debug.Log("Hit a wall!");
            // Dead();
        }
        else if (other.CompareTag("SnakeBody")) {
            Debug.Log("Hit yourself!");
            // HitBody();
        }
    }

    void HitBody() {
        SnakeHP--;
        if (SnakeHP <= 0) {
            Dead();
        }
    }

    private void Dead() {
        IsMoving = false;
        IsAlive = false;
        direction = Vector2Int.zero;
    }

    // Get the length of the snake
    public int GetSnakeLength() {
        return snakeSegments.Count;
    }

    // Reset the snake
    public void ResetSnake() {
        for (int i = 1; i < snakeSegments.Count; i++) {
            Destroy(snakeSegments[i].gameObject);
        }
        snakeSegments.RemoveRange(1, snakeSegments.Count - 1);
        transform.position = new Vector3(1, 0, 0);
        direction = Vector2Int.zero;
        GameObject firstSegment = Instantiate(segmentPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        snakeSegments.Add(firstSegment.transform);
        IsAlive = true;
        IsMoving = false;
        SnakeHP = defaultSnakeHP;
        headSpriteTransform.rotation = Quaternion.Euler(0, 0, -90);
    }

    private void RotateHeadSprite() {
        if (headSpriteTransform != null) {
            if (direction == Vector2Int.up) {
                // Up direction - default orientation (0 degrees)
                headSpriteTransform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (direction == Vector2Int.right) {
                // Right direction - 90 degrees clockwise
                headSpriteTransform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else if (direction == Vector2Int.left) {
                // Left direction - 90 degrees counterclockwise
                headSpriteTransform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (direction == Vector2Int.down) {
                // Down direction - 180 degrees
                headSpriteTransform.rotation = Quaternion.Euler(0, 0, 180);
            }
        }
    }
}
