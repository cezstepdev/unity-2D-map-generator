using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject player;
    private int chunkBegin;
    private int chunkEnd;

    //Render width and height
    [SerializeField] int height, width;
    //Smoothness of the gound
    [SerializeField] float smoothness;
    //Smoothness of the caves
    [SerializeField] int caveSmoothness;
    //Seed to RadnomGenerator
    [SerializeField] float seed;
    //ground to cave ratio
    [Range(0, 100)]
    [SerializeField] int fill;
    //Prefab object representing gound
    [SerializeField] GameObject ground;
    //Prefab object representing caves
    [SerializeField] GameObject cave;

    // Start is called before the first frame update
    void Start()
    {
        //assigning static values ​​needed to generate chunk
        assignValuesToChunk();
        //computing the x position of the first chunk
        chunkBegin = getPlayerX() - width / 2;
        chunkEnd = getPlayerX() + width / 2;
        //Creating first chunk
        Chunk chunk = new Chunk(chunkBegin);
    }

    void assignValuesToChunk()
    {
        Chunk.height = height;
        Chunk.width = width;
        Chunk.smoothness = smoothness;
        Chunk.caveSmoothness = caveSmoothness;
        Chunk.seed = seed;
        Chunk.fill = fill;
        Chunk.ground = ground;
        Chunk.cave = cave;
    }

    // Update is called once per frame
    void Update()
    {
        //If player closer to end of chunk then render weight then we create next chunk
        if (getPlayerX() + width> chunkEnd)
        {
            new Chunk(chunkEnd);
            chunkEnd += width;
        }
        //If player closer to begin of chunk then render weight then we create next chunk
        if (getPlayerX() - width < chunkBegin)
        {
            new Chunk(chunkBegin - width);
            chunkBegin -= width;
        }
            
    }

    int getPlayerX()
    {
        return (int)player.transform.position.x;
    }
}