using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    public int chunkSize = 4; // Each chunk is a 4x4 area of tiles
    public int gridSize = 16; // Initial size of map (number of chunks)
    public GameObject tilePrefab; // Prefab for individual tiles (1x1 cells)

    private HashSet<Vector2Int> chunkPositions = new HashSet<Vector2Int>(); // Tracks occupied chunks
    private Dictionary<Vector2Int, GameObject> gridMap = new Dictionary<Vector2Int, GameObject>();

    void Start() {
        InitializeGrid();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            ExpandBoard();
        }
    }

    void InitializeGrid() {
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
                }
            }
        }
    }

    public void ExpandBoard() {
        List<Vector2Int> edgeChunks = FindEdgeChunks();

        Vector2Int selectedChunk = edgeChunks[Random.Range(0, edgeChunks.Count)];
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        List<Vector2Int> possibleExpansions = new List<Vector2Int>();

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
}