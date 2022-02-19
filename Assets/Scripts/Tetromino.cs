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
    public Tetromino tetromino; //Termino
    public Tile tile; //Color / Tile
    public Vector2Int[] cells; //2D Int Vector for creating shapes & store cells || C# Property || Not visible in the Editor

}