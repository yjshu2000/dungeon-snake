using UnityEngine;


public class GameManager : MonoBehaviour
{
    // floor tiles manager
    public FloorTilesManager floorTilesManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // create the floor tiles manager
        Instantiate(floorTilesManager, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for interact input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Call the ExpandGrid function on the floorTilesManager
            floorTilesManager.ExpandGrid();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            // get the cell position where food can be spawned
            Vector2Int foodPosition = floorTilesManager.GetCellPositionForFood();
            // spawn the food at the cell position
            floorTilesManager.SpawnFood(foodPosition);
        }
    }
}
