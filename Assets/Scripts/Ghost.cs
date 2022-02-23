using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    public Tile tile;
    public Board board;
    public Piece trackingPiece;

    public Tilemap tilemap { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector3Int[4];
    }

    private void LateUpdate()
    {
        Clear();
        Copy();
        Drop();
        Set();
    }

    private void Clear()
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3Int tilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, null); //Clear Ghost Tile Position Data
        }
    }

    private void Copy()
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            this.cells[i] = this.trackingPiece.cells[i]; //Set the track cells are the activve cells
        }
    }

    private void Drop()
    {
        Vector3Int position = this.trackingPiece.position;

        int current = position.y; //Get Tile position
        int bottom = -this.board.boardSize.y / 2 - 1; //Bottom row

        this.board.Clear(this.trackingPiece);

        for(int row = current; row >= bottom; row--)
        {
            position.y = row; //Update Position row

            if(this.board.IsValidPosition(this.trackingPiece, position))
            {
                this.position = position; //Update Ghost Position
            }
            else
            {
                break;
            }
        }
        this.board.Set(this.trackingPiece); //Update tracking Piece in the board
    }

    private void Set()
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3Int tilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, this.tile); //Set Ghost Tile Position Data
        }
    }
}
