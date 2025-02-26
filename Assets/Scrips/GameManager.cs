using UnityEngine;


public class GameManager : MonoBehaviour
{
    // floor tiles manager
    public FloorTilesManager floorTilesManager;
    public GameObject foodPrefab;

    // Reference to the FoodSpawning component
    private FoodSpawning foodSpawning;
    public GameObject snakeHead;
    private FloorTilesManager floorTilesManagerInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (floorTilesManager == null)
        {
            Debug.LogError("FloorTilesManager prefab is not assigned! Please assign it in the Inspector.");
            return;
        }
        // create the floor tiles manager
        floorTilesManagerInstance = Instantiate(floorTilesManager, Vector3.zero, Quaternion.identity);
        foodSpawning = floorTilesManagerInstance.gameObject.AddComponent<FoodSpawning>();
        if (foodPrefab == null)
        {
            Debug.LogError("Food prefab is not assigned! Please assign it in the Inspector.");
            return;
        }
        // Set the food prefab reference
        foodSpawning.foodPrefab = foodPrefab;
        if (snakeHead == null)
        {
            Debug.LogWarning("Snake head is not assigned! Please assign it in the Inspector for food collision detection.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for interact input
        if (snakeHead != null && foodSpawning != null)
        {
            foodSpawning.CheckFoodCollision(snakeHead.transform.position);
        }
    }
}
