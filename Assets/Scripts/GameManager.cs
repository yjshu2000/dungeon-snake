using UnityEngine;
using static GameConstants;

public class GameManager : MonoBehaviour {
    public enum GameState {
        Start,
        Playing,
        Paused,
        GameOver
    }
    public GameState gameState = GameState.Start;

    public GameObject canvasManagerObject;
    public GameObject snakeObject;
    public GameObject floorGridManagerObject;
    public GameObject foodManagerObject;

    private CanvasManager canvasManager;
    private SnakeMovement snakeMovement;
    private FloorGridManager floorGridManager;
    private FoodSpawner foodSpawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        canvasManager = canvasManagerObject.GetComponent<CanvasManager>();
        snakeMovement = snakeObject.GetComponent<SnakeMovement>();
        floorGridManager = floorGridManagerObject.GetComponent<FloorGridManager>();
        foodSpawner = foodManagerObject.GetComponent<FoodSpawner>();
    }

    // Update is called once per frame
    void Update() {
        // testing UI
        // if (Input.GetKeyDown(KeyCode.Alpha1)) {
        //     canvasManagerComponent.ShowGameStartPanel();
        // }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            canvasManager.ShowPanel(GameInProgressPanel);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            canvasManager.ShowPanel(GameOverPanel);
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            floorGridManager.InitializeGrid();
            snakeMovement.ResetSnake();
            foodSpawner.ResetFood();
            canvasManager.ShowPanel(GameStartPanel);
            gameState = GameState.Start;
        }

        // if snake is moving, set gameState to playing
        if (snakeMovement.IsMoving) {
            gameState = GameState.Playing;
        }

        // if snake is dead, set gameState to game over
        if (!snakeMovement.IsAlive) {
            gameState = GameState.GameOver;
        }

        canvasManager.SetSnakeLength(snakeMovement.GetSnakeLength());
        canvasManager.SetSnakeHP(snakeMovement.SnakeHP);
        canvasManager.SetHealthBar(snakeMovement.SnakeHP, 3);

        if (gameState == GameState.Start) {
            canvasManager.ShowPanel(GameStartPanel);
        }
        else if (gameState == GameState.Playing) {
            canvasManager.ShowPanel(GameInProgressPanel);
        }
        else if (gameState == GameState.GameOver) {
            canvasManager.ShowPanel(GameOverPanel);
        }
    }
}
