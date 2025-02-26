using System.Collections.Generic;
using UnityEngine;

public class FloorTilesManager : MonoBehaviour
{
    // tilePrefab must be a GameObject that represents the tile to be instantiated in the grid.
    // This can be any prefab that you have created in Unity, such as a simple plane, a textured tile, or any other object you want to use as a floor tile.
    public GameObject tilePrefab;
    public int tileScale = 32; // the width and height of the tile in world units. this is used to calculate the position of the tiles
    public int initialRows = 4;
    public int initialColumns = 4;
    private Dictionary<Vector2Int, GameObject> tiles; // Dictionary to store the tiles with coordinates as keys
    // expansion variables
    public int expansionRowsMin = 4;
    public int expansionRowsMax = 4;
    public int expansionColumnsMin = 4;
    public int expansionColumnsMax = 4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateInitialGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Creates the initial grid of tiles based on initialRows and initialColumns
    void CreateInitialGrid()
    {
        tiles = new Dictionary<Vector2Int, GameObject>(); // Initialize the dictionary
        for (int i = 0; i < initialRows; i++)
        {
            for (int j = 0; j < initialColumns; j++)
            {
                // Calculate the position of the tile based on the row and column
                Vector3 position = new Vector3(i * tileScale, j * tileScale, 0);
                // Instantiate the tile prefab at the calculated position
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                // Set the parent of the tile to this object
                tile.transform.parent = this.transform;
                // Store the tile in the dictionary with coordinates as key
                tiles[new Vector2Int(i, j)] = tile;
            }
        }
    }

    private void AddTiles(Vector2Int bottomLeftCorner, int rows, int columns)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // Calculate the position of the tile based on the row and column
                Vector3 position = new Vector3((bottomLeftCorner.x + i) * tileScale, (bottomLeftCorner.y + j) * tileScale, 0);
                // Instantiate the tile prefab at the calculated position
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                // Set the parent of the tile to this object
                tile.transform.parent = this.transform;
                // Store the tile in the dictionary with coordinates as key
                tiles[new Vector2Int(bottomLeftCorner.x + i, bottomLeftCorner.y + j)] = tile;
            }
        }
    }

    // attach an additional rectangle of tiles to the grid
    // the additional rectangle's size is determined by the parameters
    // the rectangle can be attached to any side of the current grid and form any irregular shape
    public void ExpandGrid()
    {
        // find the border tiles of the grid
        List<Vector2Int> borderTiles = FindBorderTiles();
        // choose a random border tile
        Vector2Int randomBorderTile = borderTiles[Random.Range(0, borderTiles.Count)];
        // choose a random expansion size
        int rows = Random.Range(expansionRowsMin, expansionRowsMax + 1);
        int columns = Random.Range(expansionColumnsMin, expansionColumnsMax + 1);
        // calculate the bottom left corner of the new rectangle
        Vector2Int bottomLeftCorner = randomBorderTile;
        if (Random.Range(0, 2) == 0)
        {
            bottomLeftCorner.x -= rows;
        }
        else
        {
            bottomLeftCorner.y -= columns;
        }
        // add the new tiles to the grid
        AddTiles(bottomLeftCorner, rows, columns);
    }

    // find all the border tiles of the grid within the tiles dictionary
    // border tiles are the tiles that have at least one empty neighbor
    public List<Vector2Int> FindBorderTiles()
    {
        List<Vector2Int> borderTiles = new List<Vector2Int>();
        foreach (var tile in tiles)
        {
            Vector2Int position = tile.Key;
            if (IsBorderTile(position))
            {
                borderTiles.Add(position);
            }
        }
        return borderTiles;
    }

    // check if a tile is a border tile
    // a tile is a border tile if it has at least one empty neighbor
    public bool IsBorderTile(Vector2Int position)
    {
        if (!tiles.ContainsKey(position))
        {
            return false;
        }
        if (!tiles.ContainsKey(new Vector2Int(position.x + 1, position.y)) ||
            !tiles.ContainsKey(new Vector2Int(position.x - 1, position.y)) ||
            !tiles.ContainsKey(new Vector2Int(position.x, position.y + 1)) ||
            !tiles.ContainsKey(new Vector2Int(position.x, position.y - 1)))
        {
            return true;
        }
        return false;
    }
}
