using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool newGame = true;
    public bool gameInProgress = false;
    public bool gameOver = false;
    public GameObject canvasManager;

    public GameObject snake;
    public GameObject floorGridManager;
    public GameObject foodManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // testing UI
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            canvasManager.GetComponent<CanvasManager>().ShowGameStartPanel();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            canvasManager.GetComponent<CanvasManager>().ShowGameInProgressPanel();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            canvasManager.GetComponent<CanvasManager>().ShowGameOverPanel();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            floorGridManager.GetComponent<FloorGridManager>().InitializeGrid();
            snake.GetComponent<SnakeMovement>().ResetSnake();
            foodManager.GetComponent<FoodSpawner>().ResetFood();
        }

        canvasManager.GetComponent<CanvasManager>().SetSnakeLength(snake.GetComponent<SnakeMovement>().GetSnakeLength());
    }
}
