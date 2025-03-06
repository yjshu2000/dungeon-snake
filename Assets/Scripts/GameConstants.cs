using UnityEngine;

public static class GameConstants {
    // Floor Grid constants
    public const int ChunkSize = 4; // Each chunk is a 4x4 area of tiles
    public const int GridSize = 4; // Initial size of map (number of chunks)

    // Snake constants
    public const int SnakeMaxHP = 3;
    public const float FoodHealAmount = 0.1f;
    public const float MoveInterval = 0.2f; // Time between movement steps
    public const float MoveSpeed = 5f; // Speed of movement
    public const int GrowRate = 4; // Number of segments to grow by when eating food
    public const int InitialSnakeSize = 3; // Initial body segments

    // Panel name constants
    public const string GameStartPanel = "GameStartPanel";
    public const string GameInProgressPanel = "GameInProgressPanel";
    public const string GameOverPanel = "GameOverPanel";
}

public enum Direction {
    STRAIGHT_VERTICAL,
    STRAIGHT_HORIZONTAL,
    CORNER_UP_LEFT,
    CORNER_UP_RIGHT,
    CORNER_DOWN_LEFT,
    CORNER_DOWN_RIGHT,
    TAIL_UP,
    TAIL_RIGHT,
    TAIL_DOWN,
    TAIL_LEFT,
    COIL
}

public enum GameState {
    Start,
    Playing,
    Paused,
    GameOver
}