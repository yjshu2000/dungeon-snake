using System.Collections.Generic;
using UnityEngine;
using static GameConstants;

public class FoodSpawner : MonoBehaviour {
    public GameObject foodPrefab;
    public GameObject floorGridManagerObject;
    public GameObject snakeObject;
    private FloorGridManager floorGridManager;
    private SnakeMovement snake;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        floorGridManager = floorGridManagerObject.GetComponent<FloorGridManager>();
        snake = snakeObject.GetComponent<SnakeMovement>();
        Instantiate(foodPrefab, new Vector3(3, 3, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update() {
        if (IN_DEBUG_MODE) {
            if (Input.GetKeyDown(KeyCode.F)) {
                SpawnFood();
            }
        }
    }

    public void SpawnFood() {
        HashSet<Vector2Int> blockedPositions = new HashSet<Vector2Int>(snake.GetSnakePositions());

        Vector2Int foodPosition = floorGridManager.GetRandomTilePosition(blockedPositions);
        if (foodPosition != Vector2Int.zero) {
            Instantiate(foodPrefab, new Vector3(foodPosition.x, foodPosition.y, 0), Quaternion.identity);
        }
        else {
            if (IN_DEBUG_MODE) {
                Debug.Log("No food was spawned??");
            }
            SpawnFood();
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
