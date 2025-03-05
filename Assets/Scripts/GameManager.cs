using UnityEngine;

public class GameManager : MonoBehaviour {
    public enum GameState {
        Start,
        Playing,
        Paused,
        GameOver
    }
    public GameState gameState = GameState.Start;
    public GameObject canvasManager;

    public GameObject snake;
    public GameObject floorGridManager;
    public GameObject foodManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

    }

    // Update is called once per frame
    void Update() {
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
            gameState = GameState.Start;
        }

        // if snake is moving, set gameState to playing
        if (snake.GetComponent<SnakeMovement>().IsMoving) {
            gameState = GameState.Playing;
        }

        // if snake is dead, set gameState to game over
        if (!snake.GetComponent<SnakeMovement>().IsAlive) {
            gameState = GameState.GameOver;
        }

        canvasManager.GetComponent<CanvasManager>().SetSnakeLength(snake.GetComponent<SnakeMovement>().GetSnakeLength());
        canvasManager.GetComponent<CanvasManager>().SetSnakeHP(snake.GetComponent<SnakeMovement>().SnakeHP);
        canvasManager.GetComponent<CanvasManager>().SetHealthBar(snake.GetComponent<SnakeMovement>().SnakeHP, 3);

        if (gameState == GameState.Start) {
            canvasManager.GetComponent<CanvasManager>().ShowGameStartPanel();
        }
        else if (gameState == GameState.Playing) {
            canvasManager.GetComponent<CanvasManager>().ShowGameInProgressPanel();
        }
        else if (gameState == GameState.GameOver) {
            canvasManager.GetComponent<CanvasManager>().ShowGameOverPanel();
        }
    }
}
