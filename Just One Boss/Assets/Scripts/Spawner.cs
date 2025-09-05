using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    public GameObject dashPrefab; // 스폰할 Dash 프리팹
    public Tilemap tilemap;
    public float spawnInterval = 4f; // 몇 초마다 생성할지



    private float timer;
    [HideInInspector] public bool isDashSpawned = false; // 상태 플래그
    private Vector3Int lastSpawnedPos; // 마지막 스폰된 위치 저장

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnDash();
            timer = 0f;
        }
    }

    void SpawnDash()
    {
        if (isDashSpawned) return; // 이미 스폰되어 있으면 무시

        BoundsInt bounds = tilemap.cellBounds;

        for (int i = 0; i < 20; i++) // 최대 20번 시도
        {
            int x = UnityEngine.Random.Range(bounds.xMin, bounds.xMax);
            int y = UnityEngine.Random.Range(bounds.yMin, bounds.yMax);
            Vector3Int cellPos = new Vector3Int(x, y, 0);

            // 타일이 있고, 이전에 스폰된 위치와 다르면 OK
            if (tilemap.HasTile(cellPos) && cellPos != lastSpawnedPos)
            {
                Vector3 worldPos = tilemap.GetCellCenterWorld(cellPos);
                GameObject dash = Instantiate(dashPrefab, worldPos, Quaternion.identity);

                // Dash에게 Spawner 참조 넘기기
                dash.GetComponent<Dash>().Init(this);

                lastSpawnedPos = cellPos;
                isDashSpawned = true;
                return;
            }
        }
    }
}
