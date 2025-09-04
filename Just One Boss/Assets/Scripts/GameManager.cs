using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Grid grid;//���� ���۽� �ν��Ͻ�               
    [SerializeField] private Tilemap tilemap;         
    [SerializeField] private GameObject playerPrefab; 

    void Start()
    {
        Vector3Int spawnIndex = new Vector3Int(-4, 0, 0); // ���ϴ� �� �ε���
        Vector3 spawnPos = grid.GetCellCenterWorld(spawnIndex);

        GameObject playerObj = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

        Player playerScript = playerObj.GetComponent<Player>();
        playerScript.SetDependencies(grid, tilemap, spawnIndex);
    }

   
}
