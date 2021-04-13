using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDestroy : MonoBehaviour
{
    //blocks
    public Blocks blocks;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("Logowanie");
        int x = (int)gameObject.transform.position.x;
        int y = (int)gameObject.transform.position.y;
        
        Destroy(gameObject);

        Chunk chunk = calculateChunk(x);
        chunk.map[x - chunk.chunkBegin, y] = 2;
        chunk.blocks.Add(Instantiate(blocks.cave, new Vector2(x, y), Quaternion.identity));
    }

    public static Chunk calculateChunk(int x)
    {
        Chunk chunk = TerrainGenerator.centerChunk;

        int distance = x - TerrainGenerator.centerChunk.chunkBegin;
        if (distance < 0)
            return TerrainGenerator.leftChunk;
        else if (distance >= Chunk.width)
            return TerrainGenerator.rightChunk;
        else
            return chunk;
    }

}
