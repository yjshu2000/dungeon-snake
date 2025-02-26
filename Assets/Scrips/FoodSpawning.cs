using System.Collections.Generic;
using UnityEngine;

public class FoodSpawning : MonoBehaviour
{
    // The food prefab to be instantiated
    public GameObject foodPrefab;

    // Reference to the floor tiles manager
    private FloorTilesManager floorTilesManager;

    // The current food object in the scene
    private GameObject currentFood;

    // The size of each tile/grid cell
    private int tileSize;

    // Start is called before the first frame update
    void Start()
    {
        // Get the FloorTilesManager reference
        floorTilesManager = GetComponent<FloorTilesManager>();

        if (floorTilesManager == null)
        {
            Debug.LogError("FloorTilesManager reference not found!");
            return;
        }

        // Get the tile size from the floor tiles manager
        tileSize = floorTilesManager.tileScale;

        // Check if the food prefab is assigned before trying to spawn food
        if (foodPrefab == null)
        {
            Debug.LogError("Food prefab is not assigned to FoodSpawning component!");
            return;
        }

        // Spawn the initial food
        SpawnFood(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Food interaction is handled by CheckFoodCollision method called from GameManager
    }

    // Spawn food at a random position on the grid
    public void SpawnFood(bool onlyInNewArea)
    {
        // Check for null prefab again to be safe
        if (foodPrefab == null)
        {
            Debug.LogError("Cannot spawn food: food prefab is null!");
            return;
        }

        // Make sure we have a valid floor tiles manager
        if (floorTilesManager == null || floorTilesManager.tiles == null || floorTilesManager.tiles.Count == 0)
        {
            Debug.LogError("Cannot spawn food: floor tiles manager is not properly initialized!");
            return;
        }

        Vector2Int foodPosition;

        if (onlyInNewArea)
        {
            // Get a list of newly expanded border tiles
            List<Vector2Int> borderTiles = floorTilesManager.FindBorderTiles();

            if (borderTiles.Count == 0)
            {
                Debug.LogWarning("No border tiles found for food spawning! Spawning on a random tile instead.");
                // Fall back to spawning on any tile
                List<Vector2Int> allTilePositions = new List<Vector2Int>(floorTilesManager.tiles.Keys);
                foodPosition = allTilePositions[Random.Range(0, allTilePositions.Count)];
            }
            else
            {
                // Choose a random border tile
                foodPosition = borderTiles[Random.Range(0, borderTiles.Count)];
            }
        }
        else
        {
            // Choose a random position from all available tiles
            List<Vector2Int> allTilePositions = new List<Vector2Int>(floorTilesManager.tiles.Keys);

            if (allTilePositions.Count == 0)
            {
                Debug.LogError("No tiles found for food spawning!");
                return;
            }

            foodPosition = allTilePositions[Random.Range(0, allTilePositions.Count)];
        }

        // Calculate the world position for the food
        Vector3 worldPosition = new Vector3(foodPosition.x * tileSize, foodPosition.y * tileSize, -10);//ADJUSTING SPAWN POSITION

        // Instantiate the food at the chosen position
        currentFood = Instantiate(foodPrefab, worldPosition, Quaternion.identity);
        Renderer renderer = currentFood.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.red; // Bright obvious color
        }
        // Or for 2D:
        SpriteRenderer spriteRenderer = currentFood.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }
        // Set the parent of the food to this object
        currentFood.transform.parent = this.transform;
    }

    // Check if the snake's head is at the same position as the food
    public bool CheckFoodCollision(Vector2 snakeHeadPosition)
    {
        if (currentFood == null)
        {
            return false;
        }
        //Come AND LOOK AT IT LATER
        if (currentFood != null)
        {
            Debug.Log("Food spawned at: " + currentFood.transform.position);
        }
        // Convert the snake's position to tile coordinates
        Vector2Int snakeTilePosition = new Vector2Int(
            Mathf.RoundToInt(snakeHeadPosition.x / tileSize),
            Mathf.RoundToInt(snakeHeadPosition.y / tileSize)
        );

        // Convert the food's position to tile coordinates
        Vector2Int foodTilePosition = new Vector2Int(
            Mathf.RoundToInt(currentFood.transform.position.x / tileSize),
            Mathf.RoundToInt(currentFood.transform.position.y / tileSize)
        );

        // Check if the snake's head is at the same position as the food
        if (snakeTilePosition == foodTilePosition)
        {
            // The snake has eaten the food
            EatFood();
            return true;
        }

        return false;
    }

    // Handle the food being eaten
    private void EatFood()
    {
        if (currentFood != null)
        {
            // Destroy the current food
            Destroy(currentFood);
            currentFood = null;

            // Expand the grid
            floorTilesManager.ExpandGrid();

            // Spawn a new food in the newly expanded area
            SpawnFood(true);
        }
    }
}