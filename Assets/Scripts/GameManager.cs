using System.Collections.Generic;
using UnityEngine;
using static GameConstants;

public class GameManager : MonoBehaviour {
    public GameState gameState { get; private set; } = GameState.Start;

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
        SetGameState(gameState);
    }

    // Update is called once per frame
    void Update() {
        // testing UI
        if (IN_DEBUG_MODE) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                canvasManager.ShowPanel(GameStartPanel);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                canvasManager.ShowPanel(GameInProgressPanel);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                canvasManager.ShowPanel(GameOverPanel);
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            ResetGame();
        }
    }

    public void SetGameState(GameState newState) {
        gameState = newState;
        if (GameStateToPanelMap.TryGetValue(gameState, out string panelName)) {
            canvasManager.ShowPanel(panelName);
        }
    }

    private void ResetGame() {
        floorGridManager.InitializeGrid();
        snakeMovement.ResetSnake();
        foodSpawner.ResetFood();
        SetGameState(GameState.Start);
    }
}
