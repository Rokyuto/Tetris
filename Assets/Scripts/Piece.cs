using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script will control all events with the piece

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }

    public void Initialize(Board board,Vector3Int position,TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;

        if(this.cells == null) //If there is not initialized cell
        {
            this.cells = new Vector3Int[data.cells.Length]; //Initialize Tetromino from the data || this Tetromino is the active
        }

        //Copy data from Data.cs to new array
        for(int i= 0;i<data.cells.Length;i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    private void Update()
    {
        this.board.Clear(this); //Call Clear the Piece from the Board event

        if(Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2Int.left); //Move left
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2Int.right); //Move right
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2Int.down); //Move down
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }

        this.board.Set(this); //Call Set the Piece location event

    }

    private void HardDrop()
    {
        while(Move(Vector2Int.down))
        {
            continue; //Countinue to Move down until the Piece reach the max Bottom available Location
        }
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position; // Next Tile Position
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.board.IsValidPosition(this, newPosition); //Check for new Tile Position Validity

        if(valid) //If it's valid
        {
            this.position = newPosition; //Apply to Tile the new Position
        }

        return valid;

    }
}
