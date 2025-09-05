using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Card : MonoBehaviour
{
    private Tilemap tilemap;

    public float rotateSpeed;
    public float moveSpeed;

    public void SetDependencies(Tilemap tilemap)
    {
        this.tilemap = tilemap;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        transform.position += new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime;
    }

    private void Rotate()
    {
        transform.Rotate(new Vector3(0, 0, -1) * rotateSpeed * Time.deltaTime);
    }

    private bool IsOutOfBounds(Vector3 position)
    {
        BoundsInt bounds = tilemap.cellBounds;
        return position.x < bounds.xMin || position.x > bounds.xMax ||
               position.y < bounds.yMin || position.y > bounds.yMax;
    }

}
