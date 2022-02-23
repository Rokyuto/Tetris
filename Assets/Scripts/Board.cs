using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public TetrominoData[] tetrominoes; //Create Array with all tetrominoes data (tetromino & color / tile)

    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(17, 20);

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x /2, -this.boardSize.y /2); // Top Left Corner
            return new RectInt(position, this.boardSize); //Set Bounds (Top Left to Bottom Right)
        }
    }

    //Spawn piece

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>(); //Get the Element in Piece.cs 

        for (int i = 0; i < tetrominoes.Length; i++)
        {
            this.tetrominoes[i].Initialize(); //Initialize tetrominoes
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, tetrominoes.Length); //Generate Random Index
        TetrominoData data = this.tetrominoes[random]; //Get Random Tetromino

        this.activePiece.Initialize(this, this.spawnPosition, data); //Initialize  Active Piece
        
        if(IsValidPosition(this.activePiece, this.spawnPosition))
        {
            Set(this.activePiece);
        }
        else
        {
            GameOver();
        }
        
        Set(this.activePiece); //Apply Settings to the Active Piece
    }

    private void GameOver()
    {
        this.tilemap.ClearAllTiles(); //Clear the level 
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile); //Set Tile Position Data
        }
    }

    public void Clear(Piece piece) //Clear the Piece from the board
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null); //Clear Tile Position Data
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position; //Shape Position

            if (!bounds.Contains((Vector2Int)tilePosition)) //If the bounds don't contains tilePosition
            {
                return false;
            }

            if (this.tilemap.HasTile(tilePosition)) //If the tile already exist at the tile Position
            {
                return false;
            }
        }

        return true; // After all set done the new Position
    }

    public void ClearLines()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;

        while (row < bounds.yMax)
        {
            if (IsLineFull(row))
            {
                LineClear(row);
            }
            else
            {
                row++; //Tiles are +1
            }
        }
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col<bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!this.tilemap.HasTile(position))
            {
                return false;
            }
        }
        return true;
    }

    private void LineClear(int row)
    {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col<bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0); //Initialize row to clear
            this.tilemap.SetTile(position, null); //Clear tiles at this row
        }

        //Fall down the rows above the cleared
        while(row < bounds.yMax)
        {
            for(int col = bounds.xMin; col<bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0); //Initialize the above row
                TileBase above = this.tilemap.GetTile(position); 

                position = new Vector3Int(col,row, 0); //Initialize the bottom row
                this.tilemap.SetTile(position,above); //Move Down
            }
            row++;
        }
    }

}