using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    // Start is called before the first frame update
    public Tile tile;
    public Board mainBoard;
    public Piece trackingPiece;

    public Tilemap tilemap {  get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector3Int[4];
    }

    private void LateUpdate()//ÀÌ°Ô ¹¹Áö
    {
        Clear();
        Copy();
        Drop();
        Set();
    }
    private void Clear()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int tilePosition = cells[i] + position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    private void Copy()
    {
        for( int i=0; i< this.cells.Length; i++) 
            { this.cells[i] = this.trackingPiece.cells[i]; }
    }
    
    private void Drop()
    {
        Vector3Int position = this.trackingPiece.position;

        int current = position.y;
        int bottom = -this.mainBoard.boardSize.y / 2 - 1;
        this.mainBoard.Clear(this.trackingPiece);
        for (int row = current; row >= bottom; row--)
        {
            position.y = row;
            if(this.mainBoard.IsValidPosition(this.trackingPiece, position))
            {
                this.position = position;
            }
            else
            {
                break;
            }
        }
        this.mainBoard.Set(this.trackingPiece);
    }

    private void Set()
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3Int tilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, this.tile);
        }
    }
}
