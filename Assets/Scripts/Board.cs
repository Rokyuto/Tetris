using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public TetrominoData[] tetrominoes; //Create Array with all tetrominoes data (tetromino & color / tile)
    
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }

    public Vector3Int spawnPosition;

    //Spawn piece

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();    

        for(int i = 0; i < tetrominoes.Length; i++)
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
        Set(this.activePiece); //Apply Settings to the Active Piece
    }

    public void Set(Piece piece)
    {
        for(int i = 0; i < piece.cells.Length;i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position; //Set Tetromino Position
            this.tilemap.SetTile(tilePosition, piece.data.tile); //Set Tile Data
        }
    }


}
