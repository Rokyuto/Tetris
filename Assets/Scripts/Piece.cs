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
    public int rotationIndex { get; private set; }

    public float stepDelay = 1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float lockTime;

    public void Initialize(Board board,Vector3Int position,TetrominoData data)
    {
        //On start Settings
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;
        this.stepTime = Time.time + this.stepDelay;
        this.lockTime = 0f;

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

        this.lockTime += Time.deltaTime;

        // Movement

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

        //Call Auto-movement
        if (Time.time >= this.stepTime)
        {
            Step();
        }

        // Rotation

        if(Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(-1); //Left Rotation Direction
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            Rotate(1); //Right Rotation Direction
        }

        this.board.Set(this); //Call Set the Piece location event

    }

    //Auto-movement
    private void Step()
    {
        this.stepTime = Time.time + this.stepDelay; //Time to move to the next row

        Move(Vector2Int.down); //Move 

        if(this.lockTime >= this.lockDelay)
        {
            Lock(); //Spawn NEW Piece
        }
    }

    private void HardDrop()
    {
        while(Move(Vector2Int.down))
        {
            continue; //Countinue to Move down until the Piece reach the max Bottom available Location
        }
        Lock(); //Spawn NEW Piece
    }

    //Spawn NEW Piece
    private void Lock()
    {
        this.board.Set(this);
        this.board.ClearLines();
        this.board.SpawnPiece();
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
            this.lockTime = 0f;
        }

        return valid;
    }

    private void Rotate(int direction)
    {
        int originalRotation = this.rotationIndex;
        this.rotationIndex = Wrap(this.rotationIndex + direction,0,4); //Rotation Index

        ApplyRotationMatrix(direction); //Rotate 

        if(!TestWallKicks(this.rotationIndex,direction)) //If there is not wall kick (have space for rotation)
        {
            this.rotationIndex = originalRotation; //Update Rotation Index
            ApplyRotationMatrix(-direction);
        }
    }

    private void ApplyRotationMatrix(int direction)
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3 cell = this.cells[i];

            int x, y; //New Rotation Coordinates

            switch (this.data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;

            }

            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i<this.data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = this.data.wallKicks[wallKickIndex,i];

            if(Move(translation))
            {
                return true;
            }
        }

        return false;

    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if(rotationDirection < 0)
        {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0));
    }

    private int Wrap(int input, int min, int max)
    {
        if(input < min)
        {
            return max - (min - input) % (max - min); 
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }

}
