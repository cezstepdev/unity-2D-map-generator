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
    //Height in every X posiotion
    int[] perLineHeight;

    // Start is called before the first frame update
    void Start()
    {
        assignValuesToChunk();
        
        chunkBegin = getPlayerX() - width / 2;
        chunkEnd = getPlayerX() + width / 2;
        Debug.Log(chunkBegin + " " + chunkEnd + " "+ " " + width);
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
        //Debug.Log("Playerx: " + getPlayerX() + " chunkEnd: " + chunkEnd);
        if (getPlayerX() + width> chunkEnd)
        {
            new Chunk(chunkEnd);
            chunkEnd += width;
        }
        if(getPlayerX() - width < chunkBegin)
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