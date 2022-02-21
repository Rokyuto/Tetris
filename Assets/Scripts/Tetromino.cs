using UnityEngine;
using UnityEngine.Tilemaps;

//Enum with Tetrominoes
public enum Tetromino
{
    I,
    O,
    T,
    J,
    L,
    S,
    Z,
}

[System.Serializable] // Help Unity to Serialize(View) TetrominoData

public struct TetrominoData //Contains all Information aout Shapes
{
    public Tetromino tetromino; //Tetromino
    public Tile tile; //Color / Tile
    public Vector2Int[] cells { get; private set; } //2D Int Vector for creating Tetromino shapes & store cells || C# Property || Not visible in the Editor

    public void Initialize()
    {
        this.cells = Data.Cells[this.tetromino]; //Store the tetronimo types in array
    }
}