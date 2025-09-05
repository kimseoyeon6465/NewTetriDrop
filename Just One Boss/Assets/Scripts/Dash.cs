using UnityEngine;

public class Dash : MonoBehaviour
{
    private Spawner spawner;

    // Spawner에서 Init으로 참조를 넘겨줌
    public void Init(Spawner spawnerRef)
    {
        spawner = spawnerRef;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player와 Dash 충돌");

            // 스폰 상태 false로 돌려놓음
            if (spawner != null)
                spawner.isDashSpawned = false;

            Destroy(gameObject);
        }
    }
}
