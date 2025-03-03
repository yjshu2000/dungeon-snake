using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {
    public GameObject foodPrefab;
    private FloorGridManager floorGridManager;
    private SnakeMovement snake;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        floorGridManager = FindAnyObjectByType<FloorGridManager>();
        snake = FindAnyObjectByType<SnakeMovement>();
        Instantiate(foodPrefab, new Vector3(3, 3, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update() {
        // TEST FUNCTION: Press 'F' to spawn food manually
        if (Input.GetKeyDown(KeyCode.F)) {
            SpawnFood();
        }
    }

    public void SpawnFood() {
        HashSet<Vector2Int> blockedPositions = new HashSet<Vector2Int>(snake.GetSnakePositions());

        Vector2Int foodPosition = floorGridManager.GetRandomTilePosition(blockedPositions);
        if (foodPosition != Vector2Int.zero) {
            Instantiate(foodPrefab, new Vector3(foodPosition.x, foodPosition.y, 0), Quaternion.identity);
        }
    }

    public void ResetFood() {
        GameObject[] foodObjects = GameObject.FindGameObjectsWithTag("Food");
        foreach (GameObject food in foodObjects) {
            Destroy(food);
        }
        // spawn new food
        SpawnFood();
    }
}
