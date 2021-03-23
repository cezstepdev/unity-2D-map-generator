using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject player;
    private int chunkBegin;
    private int chunkEnd;
    private Chunk leftChunk;
    private Chunk centerChunk;
    private Chunk rightChunk;

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
        centerChunk = new Chunk(chunkBegin);
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
        
        //If player is closer to end of chunk then render weight then we create next chunk
        if (getPlayerX() + width - 1 > chunkEnd)
        {
            rightChunk = new Chunk(chunkEnd);
            chunkEnd += width;
        }
        //Deleting left chunk
        if (getPlayerX() > chunkBegin + 2 * width)
        {
            //Debug.Log(getPlayerX() + " " + chunkBegin + " " + chunkEnd);
            chunkBegin += width;
            leftChunk.deleteChunk();
            leftChunk = centerChunk;
            centerChunk = rightChunk;
        }
        //If player is closer to begin of chunk then render weight then we create next chunk
        if (getPlayerX() - width + 1  < chunkBegin)
        {
            leftChunk = new Chunk(chunkBegin - width);
            chunkBegin -= width;
        }
        //Deleting right chunk
        if (getPlayerX() < chunkEnd - 2 * width)
        {
            chunkEnd -= width;
            rightChunk.deleteChunk();
            rightChunk = centerChunk;
            centerChunk = leftChunk;
        }
    }

    int getPlayerX()
    {
        return (int)player.transform.position.x;
    }
}