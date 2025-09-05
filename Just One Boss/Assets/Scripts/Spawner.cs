using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    public GameObject dashPrefab; // ������ Dash ������
    public Tilemap tilemap;
    public float spawnInterval = 4f; // �� �ʸ��� ��������



    private float timer;
    [HideInInspector] public bool isDashSpawned = false; // ���� �÷���
    private Vector3Int lastSpawnedPos; // ������ ������ ��ġ ����

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
        if (isDashSpawned) return; // �̹� �����Ǿ� ������ ����

        BoundsInt bounds = tilemap.cellBounds;

        for (int i = 0; i < 20; i++) // �ִ� 20�� �õ�
        {
            int x = UnityEngine.Random.Range(bounds.xMin, bounds.xMax);
            int y = UnityEngine.Random.Range(bounds.yMin, bounds.yMax);
            Vector3Int cellPos = new Vector3Int(x, y, 0);

            // Ÿ���� �ְ�, ������ ������ ��ġ�� �ٸ��� OK
            if (tilemap.HasTile(cellPos) && cellPos != lastSpawnedPos)
            {
                Vector3 worldPos = tilemap.GetCellCenterWorld(cellPos);
                GameObject dash = Instantiate(dashPrefab, worldPos, Quaternion.identity);

                // Dash���� Spawner ���� �ѱ��
                dash.GetComponent<Dash>().Init(this);

                lastSpawnedPos = cellPos;
                isDashSpawned = true;
                return;
            }
        }
    }
}
