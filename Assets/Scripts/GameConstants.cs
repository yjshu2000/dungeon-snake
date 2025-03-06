using System.Collections.Generic;
using UnityEngine;

public static class GameConstants {
    // Floor Grid constants
    public const int ChunkSize = 4; // Each chunk is a 4x4 area of tiles
    public const int GridSize = 4; // Initial size of map (number of chunks)

    // Snake constants - HP
    public const float SnakeMaxHP = 100f;
    public const float FoodHealAmount = 1f;
    public const float SelfCollisionDmg = 20f;
    public const float AdventurerBaseDmg = 10f;
    // the rest
    public const float MoveInterval = 0.2f; // Time between movement steps
    public const float MoveSpeed = 5f; // Speed of movement
    public const int GrowRate = 4; // Number of segments to grow by when eating food
    public const int InitialSnakeSize = 3; // Initial body segments

    // Panel name constants
    public const string GameStartPanel = "GameStartPanel";
    public const string GameInProgressPanel = "GameInProgressPanel";
    public const string GameOverPanel = "GameOverPanel";

    // GameState to Panel mapping
    public static readonly Dictionary<GameState, string> GameStateToPanelMap = new Dictionary<GameState, string> {
        { GameState.Start, GameStartPanel },
        { GameState.Playing, GameInProgressPanel },
        { GameState.GameOver, GameOverPanel }
    };
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