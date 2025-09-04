using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class Player : MonoBehaviour
{
    private Grid grid;
    private Tilemap tilemap;
    private Vector3Int currentPosition;
    //[SerializeField] private Vector2Int minTileIndex = new Vector2Int(-8, -1);
    //[SerializeField] private Vector2Int maxTileIndex = new Vector2Int(-1, 2);

    public void SetDependencies(Grid grid, Tilemap tilemap, Vector3Int spawnIndex)
    {
        this.grid = grid;
        this.tilemap = tilemap;
        this.currentPosition = spawnIndex;
        this.transform.position = grid.GetCellCenterWorld(spawnIndex);
    }


    void Update()
    {
        Vector2Int direction = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Vector2Int.up;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Vector2Int.left;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Vector2Int.down;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Vector2Int.right;
        }

        if (direction != Vector2Int.zero)
        {
            Move(direction);
            Debug.Log(this.transform.position);
        }
    }

    private void Move(Vector2Int translation)
    {
        Vector3Int newPos = currentPosition + new Vector3Int(translation.x, translation.y, 0);

        if (IsValid(newPos))
        {
            currentPosition = newPos;
            transform.position = grid.GetCellCenterWorld(currentPosition);
        }
        else
        {
            Debug.Log("�� ��� ���̶� �̵� �Ұ�!");
        }

    }


    private bool IsValid(Vector3Int pos)
    {
        // BoundsInt.max�� ����� ���� ĭ�� ��Ÿ���Ƿ� -1
        BoundsInt bounds = tilemap.cellBounds;
        Vector3Int minBound = bounds.min;
        Vector3Int maxBound = bounds.max - Vector3Int.one;

        if (pos.x >= minBound.x && pos.x <= maxBound.x &&
            pos.y >= minBound.y && pos.y <= maxBound.y)
        {
            return tilemap.HasTile(pos);//�����Լ� HasTile
        }

        return false;
    }
}
