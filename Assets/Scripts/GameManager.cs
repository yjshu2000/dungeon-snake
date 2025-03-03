using UnityEngine;

public class GameManager : MonoBehaviour {
    public bool gameReady = true;
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
        // if (Input.GetKeyDown(KeyCode.Alpha1)) {
        //     canvasManager.GetComponent<CanvasManager>().ShowGameStartPanel();
        // }
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
            canvasManager.GetComponent<CanvasManager>().ShowGameStartPanel();
            gameReady = true;
            gameInProgress = false;
            gameOver = false;
        }

        // if snake is moving, set gameInProgress to true
        if (snake.GetComponent<SnakeMovement>().IsMoving) {
            gameInProgress = true;
            gameReady = false;
            gameOver = false;
        }

        // if snake is dead, set gameOver to true
        if (!snake.GetComponent<SnakeMovement>().IsAlive) {
            gameInProgress = false;
            gameReady = false;
            gameOver = true;
        }

        canvasManager.GetComponent<CanvasManager>().SetSnakeLength(snake.GetComponent<SnakeMovement>().GetSnakeLength());
        canvasManager.GetComponent<CanvasManager>().SetSnakeHP(snake.GetComponent<SnakeMovement>().SnakeHP);
        canvasManager.GetComponent<CanvasManager>().SetHealthBar(snake.GetComponent<SnakeMovement>().SnakeHP, 3);

        if (gameReady) {
            canvasManager.GetComponent<CanvasManager>().ShowGameStartPanel();
        } else if (gameInProgress) {
            canvasManager.GetComponent<CanvasManager>().ShowGameInProgressPanel();
        } else if (gameOver) {
            canvasManager.GetComponent<CanvasManager>().ShowGameOverPanel();
        }
    }
}
