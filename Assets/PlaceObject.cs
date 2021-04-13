using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{
    public Blocks blocks;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            int x = (int)gameObject.transform.position.x;
            int y = (int)gameObject.transform.position.y;

            Destroy(gameObject);

            Chunk chunk = BlockDestroy.calculateChunk(x);
            chunk.map[x - chunk.chunkBegin, y] = 1;
            chunk.blocks.Add(Instantiate(blocks.ground, new Vector2(x, y), Quaternion.identity));
        }
    }
}
