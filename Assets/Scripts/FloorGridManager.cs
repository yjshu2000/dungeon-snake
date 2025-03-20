using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;
using static GameConstants;

public class FloorGridManager : MonoBehaviour {
    public GameObject floorTilePrefab; // Prefab for individual tiles
    private HashSet<Vector2Int> floorChunkPositions = new HashSet<Vector2Int>(); // Tracks occupied chunks
    private Dictionary<Vector2Int, GameObject> floorGridMap = new Dictionary<Vector2Int, GameObject>();
    private List<Vector2Int> floorTilePositions = new List<Vector2Int>(); // Store all tile positions
    public List<Vector2Int> TilePositions => floorTilePositions; // Public getter

    public GameObject wallPrefab; // Prefab for walls
    private List<Vector2Int> wallChunkPositions = new List<Vector2Int>(); // Tracks occupied wall chunks
    private Dictionary<Vector2Int, GameObject> wallGridMap = new Dictionary<Vector2Int, GameObject>(); // Store all wall tiles
    private List<Vector2Int> wallTilePositions = new List<Vector2Int>(); // Store all wall positions

    void Start() {
        InitializeGrid();
    }

    void Update() {
        if (IN_DEBUG_MODE) {
            if (Input.GetKeyDown(KeyCode.E)) {
                ExpandBoard();
            }
        }
    }

    public void InitializeGrid() {
        // clear all existing tiles and walls
        RemoveAllTiles();
        RemoveAllWalls();
        AddFloorChunk(new Vector2Int(0, 0));
        AddWallChunk(new Vector2Int(1, 0));
        AddWallChunk(new Vector2Int(-1, 0));
        AddWallChunk(new Vector2Int(0, 1));
        AddWallChunk(new Vector2Int(0, -1));
        for (int i = 0; i < InitialGridChunks; i++) {
            ExpandBoard();
        }
    }

    //adds the chunk at a specified position
    public void AddFloorChunk(Vector2Int chunkPosition) {
        if (floorChunkPositions.Contains(chunkPosition))
            return;
        floorChunkPositions.Add(chunkPosition);
        for (int x = 0; x < ChunkSize; x++) {
            for (int y = 0; y < ChunkSize; y++) {
                Vector2Int tilePosition = new Vector2Int(
                    chunkPosition.x * ChunkSize + x,
                    chunkPosition.y * ChunkSize + y
                );
                if (!floorGridMap.ContainsKey(tilePosition)) {
                    GameObject newTile = Instantiate(floorTilePrefab, new Vector3(tilePosition.x, tilePosition.y, 0), Quaternion.identity);
                    newTile.name = $"Tile {tilePosition.x},{tilePosition.y}";
                    floorGridMap[tilePosition] = newTile;
                    floorTilePositions.Add(tilePosition);
                }
            }
        }
    }

    public void AddWallChunk(Vector2Int wallChunkPosition) {
        if (wallChunkPositions.Contains(wallChunkPosition) || floorChunkPositions.Contains(wallChunkPosition))
            return;
        wallChunkPositions.Add(wallChunkPosition);
        for (int x = 0; x < ChunkSize; x++) {
            for (int y = 0; y < ChunkSize; y++) {
                Vector2Int wallPosition = new Vector2Int(
                    wallChunkPosition.x * ChunkSize + x,
                    wallChunkPosition.y * ChunkSize + y
                );

                if (!wallGridMap.ContainsKey(wallPosition) && !floorGridMap.ContainsKey(wallPosition)) {
                    GameObject newWall = Instantiate(wallPrefab, new Vector3(wallPosition.x, wallPosition.y, 0), Quaternion.identity);
                    newWall.name = $"Wall {wallPosition.x},{wallPosition.y}";
                    wallGridMap[wallPosition] = newWall;
                    wallTilePositions.Add(wallPosition);
                }
            }
        }
    }

    public void ExpandBoard() {
        if (wallChunkPositions.Count == 0) {
            if (IN_DEBUG_MODE) {
                Debug.Log("why expand when no walls");
            }
            return;
        }
        Vector2Int newFloorPosition = wallChunkPositions[Random.Range(0, wallChunkPositions.Count)];
        AddFloorChunk(newFloorPosition);
        UpdateWalls(newFloorPosition);
    }

    private void UpdateWalls(Vector2Int newFloorPosition) {
        // Remove the wall chunk that's been converted to floor
        RemoveWallChunk(newFloorPosition);

        // Check adjacent chunks and add walls if needed
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (Vector2Int dir in directions) {
            Vector2Int adjacentChunkPosition = newFloorPosition + dir;

            // Only add a wall if the adjacent position doesn't already have a floor or wall
            if (!floorChunkPositions.Contains(adjacentChunkPosition) &&
                !wallChunkPositions.Contains(adjacentChunkPosition)) {
                AddWallChunk(adjacentChunkPosition);
            }
        }

    }

    private void RemoveWallChunk(Vector2Int chunkPosition) {
        if (!wallChunkPositions.Contains(chunkPosition))
            return;
        wallChunkPositions.Remove(chunkPosition);
        // Remove all wall tiles in this chunk
        for (int x = 0; x < ChunkSize; x++) {
            for (int y = 0; y < ChunkSize; y++) {
                Vector2Int wallPosition = new Vector2Int(
                    chunkPosition.x * ChunkSize + x,
                    chunkPosition.y * ChunkSize + y
                );

                if (wallGridMap.ContainsKey(wallPosition)) {
                    Destroy(wallGridMap[wallPosition]);
                    wallGridMap.Remove(wallPosition);
                    wallTilePositions.Remove(wallPosition);
                }
            }
        }
    }

    public void RemoveAllWalls() {
        foreach (var wall in wallGridMap.Values) {
            Destroy(wall);
        }
        wallTilePositions.Clear();
        wallChunkPositions.Clear();
        wallGridMap.Clear();
    }

    public void RemoveAllTiles() {
        foreach (var tile in floorGridMap.Values) {
            Destroy(tile);
        }
        floorTilePositions.Clear();
        floorChunkPositions.Clear();
        floorGridMap.Clear();
    }

    public Vector2Int GetRandomTilePosition(HashSet<Vector2Int> blockedPositions) {
        if (floorTilePositions.Count == 0) return Vector2Int.zero; // Prevent errors if no tiles exist
        List<Vector2Int> validPositions = floorTilePositions.FindAll(pos => !blockedPositions.Contains(pos));
        if (validPositions.Count == 0) return Vector2Int.zero; // No valid position found
        return validPositions[Random.Range(0, validPositions.Count)];
    }

    // check if a position is a valid tile position
    public bool IsValidTilePosition(Vector2Int position) {
        return floorTilePositions.Contains(position);
    }
}