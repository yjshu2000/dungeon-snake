using System.Collections.Generic;
using UnityEngine;

public class FloorGridManager : MonoBehaviour {
    private int chunkSize = 4; // Each chunk is a 4x4 area of tiles
    private int gridSize = 4; // Initial size of map (number of chunks)
    public GameObject tilePrefab; // Prefab for individual tiles

    private HashSet<Vector2Int> chunkPositions = new HashSet<Vector2Int>(); // Tracks occupied chunks
    private Dictionary<Vector2Int, GameObject> gridMap = new Dictionary<Vector2Int, GameObject>();

    private List<Vector2Int> tilePositions = new List<Vector2Int>(); // Store all tile positions
    public List<Vector2Int> TilePositions => tilePositions; // Public getter

    public GameObject wallPrefab; // Prefab for walls
    private HashSet<Vector2Int> wallChunkPositions = new HashSet<Vector2Int>(); // Tracks occupied wall chunks
    private Dictionary<Vector2Int, GameObject> wallGridMap = new Dictionary<Vector2Int, GameObject>(); // Store all wall tiles
    private List<Vector2Int> wallPositions = new List<Vector2Int>(); // Store all wall positions

    void Start() {
        InitializeGrid();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            ExpandBoard();
        }
    }

    public void InitializeGrid() {
        // clear all existing tiles and walls
        RemoveAllTiles();
        RemoveAllWall();
        AddChunk(new Vector2Int(0, 0));
        for (int i = 0; i < gridSize; i++) {
            ExpandBoard();
        }
    }

    //adds the chunk at a specified position
    public void AddChunk(Vector2Int chunkPosition) {
        if (chunkPositions.Contains(chunkPosition))
            return;

        chunkPositions.Add(chunkPosition);

        for (int x = 0; x < chunkSize; x++) {
            for (int y = 0; y < chunkSize; y++) {
                Vector2Int tilePosition = new Vector2Int(
                    chunkPosition.x * chunkSize + x,
                    chunkPosition.y * chunkSize + y
                );

                if (!gridMap.ContainsKey(tilePosition)) {
                    GameObject newTile = Instantiate(tilePrefab, new Vector3(tilePosition.x, tilePosition.y, 0), Quaternion.identity);
                    newTile.name = $"Tile {tilePosition.x},{tilePosition.y}";
                    gridMap[tilePosition] = newTile;
                    tilePositions.Add(tilePosition);
                }
            }
        }
    }

    public void ExpandBoard() {
        List<Vector2Int> edgeChunks = FindEdgeChunks();

        Vector2Int selectedChunk = edgeChunks[Random.Range(0, edgeChunks.Count)];
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        List<Vector2Int> possibleExpansions = new List<Vector2Int>();

        RemoveAllWall();

        foreach (Vector2Int dir in directions) {
            Vector2Int newChunkPos = selectedChunk + dir;
            if (!chunkPositions.Contains(newChunkPos)) {
                possibleExpansions.Add(newChunkPos);
            }
        }

        if (possibleExpansions.Count > 0) {
            Vector2Int expansionPos = possibleExpansions[Random.Range(0, possibleExpansions.Count)];
            AddChunk(expansionPos);
        }

        BuildWall();
    }

    private List<Vector2Int> FindEdgeChunks() {
        List<Vector2Int> edgeChunks = new List<Vector2Int>();
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (Vector2Int chunk in chunkPositions) {
            foreach (Vector2Int dir in directions) {
                Vector2Int neighbor = chunk + dir;
                if (!chunkPositions.Contains(neighbor)) {
                    edgeChunks.Add(chunk);
                    break;
                }
            }
        }
        return edgeChunks;
    }

    public Vector2Int GetRandomTilePosition(HashSet<Vector2Int> blockedPositions) {
        if (tilePositions.Count == 0) return Vector2Int.zero; // Prevent errors if no tiles exist

        List<Vector2Int> validPositions = tilePositions.FindAll(pos => !blockedPositions.Contains(pos));

        if (validPositions.Count == 0) return Vector2Int.zero; // No valid position found

        return validPositions[Random.Range(0, validPositions.Count)];
    }

    public void BuildWall() {
        List<Vector2Int> edgeChunks = FindEdgeChunks();
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        List<Vector2Int> possibleExpansions = new List<Vector2Int>();

        foreach (Vector2Int dir in directions) {
            foreach (Vector2Int chunk in edgeChunks) {
                Vector2Int newChunkPos = chunk + dir;
                if (!chunkPositions.Contains(newChunkPos)) {
                    possibleExpansions.Add(newChunkPos);
                }
            }
        }

        // fill possibleExpansions with wall tiles
        foreach (Vector2Int expansionPos in possibleExpansions) {
            AddWallChunk(expansionPos);
        }
    }

    public void AddWallChunk(Vector2Int wallChunkPosition) {
        if (wallChunkPositions.Contains(wallChunkPosition))
            return;

        wallChunkPositions.Add(wallChunkPosition);

        for (int x = 0; x < chunkSize; x++) {
            for (int y = 0; y < chunkSize; y++) {
                Vector2Int wallPosition = new Vector2Int(
                    wallChunkPosition.x * chunkSize + x,
                    wallChunkPosition.y * chunkSize + y
                );

                if (!wallGridMap.ContainsKey(wallPosition)) {
                    GameObject newWall = Instantiate(wallPrefab, new Vector3(wallPosition.x, wallPosition.y, 0), Quaternion.identity);
                    newWall.name = $"Wall {wallPosition.x},{wallPosition.y}";
                    wallGridMap[wallPosition] = newWall;
                    wallPositions.Add(wallPosition);
                }
            }
        }
    }

    public void RemoveAllWall() {
        foreach (Vector2Int wallPosition in wallPositions) {
            Destroy(wallGridMap[wallPosition]);
            wallGridMap.Remove(wallPosition);
            // remove game object from scene
            GameObject wall = GameObject.Find($"Wall {wallPosition.x},{wallPosition.y}");
            Destroy(wall);
        }
        wallPositions.Clear();
        wallChunkPositions.Clear();
        wallGridMap.Clear();
    }

    public void RemoveAllTiles() {
        foreach (Vector2Int tilePosition in tilePositions) {
            Destroy(gridMap[tilePosition]);
            gridMap.Remove(tilePosition);
            // remove game object from scene
            GameObject tile = GameObject.Find($"Tile {tilePosition.x},{tilePosition.y}");
            Destroy(tile);
        }
        tilePositions.Clear();
        chunkPositions.Clear();
        gridMap.Clear();
    }
}