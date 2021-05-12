using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    //Render height
    public static int height;
    //Render width and
    public static int width;
    //Smoothness of the gound
    public static float smoothness;
    //Smoothness of the caves
    public static int caveSmoothness;
    //Seed to RadnomGenerator
    public static float seed;
    //ground to cave ratio
    public static int fill;

    //Height in every X posiotion
    private int[] perLineHeight;
    //Chunk array representation
    public int[,] map;
    //the coordinate of the beginning of chunk
    public int chunkBegin;

    public static GameObject air;
    public static GameObject grass;
    public static GameObject dirt;
    //Prefab object representing gound
    public static GameObject stone;
    //Prefab object representing caves
    public static GameObject cave;
    public static GameObject gold;
    public static GameObject iron;

    //List of all blocks on that chunk
    public List<GameObject> blocks;


    // Start is called before the first frame update
    void Start()
    {  
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Chunk(int chunkBegin)
    {
        loadChunk(chunkBegin);
    }

    //fill the array with zeros
    //empty map
    private int[,] generateArray()
    {
        int[,] map = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = 0;
            }
        }

        return map;
    }
    //generating noisy terrain
    private int[,] terrainGenerator(int[,] map)
    {
        int perlinHeight;
        int chunkX = chunkBegin;
        System.Random random = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(chunkX / smoothness, seed) * height);
            perLineHeight[x] = perlinHeight;

            for (int y = 0; y < perlinHeight; y++)
            {
                map[x, y] = (random.Next(1, 100) < fill) ? 1 : 2;
            }

            for (int y = 0; y < perlinHeight; y++)
            {
                if (map[x, y] != 2)
                    map[x, y] = (random.Next(1, 100) < 95) ? 1 : (random.Next(1, 100) < 95 ? 4 : 3);
            }
            chunkX++;
        }

        return map;
    }

    //calculating the count of caves around the object
    private int getSurroundingGround(int gridX, int gridY)
    {
        int groundCount = 0;

        for (int nebX = gridX - 1; nebX <= gridX + 1; nebX++)
        {
            for (int nebY = gridY - 1; nebY <= gridY + 1; nebY++)
            {
                if (nebX >= 0 && nebY >= 0 && nebX < width && nebY < height)
                {
                    if (nebX != gridX || nebY != gridY)
                    {
                        if (map[nebX, nebY] == 1)
                        {
                            groundCount++;
                        }
                    }
                }
            }
        }
        return groundCount;
    }

    //smoothing caves
    private void smoothCaves()
    {
        int surroudingGround;
        for (int i = 0; i < caveSmoothness; i++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < perLineHeight[x]; y++)
                {
                    if (x == 0 || y == 0 || x == width - 1 || y == perLineHeight[x] - 1)
                    {
                        map[x, y] = 1;
                    }
                    else
                    {
                        surroudingGround = getSurroundingGround(x, y);
                        if (map[x, y] == 1 || map[x, y] == 2)
                        {
                            if (surroudingGround > 4)
                            {
                                map[x, y] = 1;
                            }
                            else if (surroudingGround < 4)
                            {
                                map[x, y] = 2;
                            }
                        }
                    }
                }
                map[x, perLineHeight[x]] = 5;
                map[x, perLineHeight[x] + 1] = 6;
            }
        }
    }

    //final rendering map to screen
    private void renderMap(int[,] map)
    {
        int xBlockPlace = chunkBegin;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 0)
                {
                    blocks.Add(Instantiate(air, new Vector2(xBlockPlace, y), Quaternion.identity));
                }
                if (map[x, y] == 1)
                {
                    blocks.Add(Instantiate(stone, new Vector2(xBlockPlace, y), Quaternion.identity));
                }
                else if (map[x, y] == 2)
                {
                    blocks.Add(Instantiate(cave, new Vector2(xBlockPlace, y), Quaternion.identity));
                }
                else if (map[x, y] == 3)
                {
                    blocks.Add(Instantiate(gold, new Vector2(xBlockPlace, y), Quaternion.identity));
                }
                else if (map[x, y] == 4)
                {
                    blocks.Add(Instantiate(iron, new Vector2(xBlockPlace, y), Quaternion.identity));
                }
                else if (map[x, y] == 5)
                {
                    blocks.Add(Instantiate(dirt, new Vector2(xBlockPlace, y), Quaternion.identity));
                }
                else if (map[x, y] == 6)
                {
                    blocks.Add(Instantiate(grass, new Vector2(xBlockPlace, y), Quaternion.identity));
                }
            }
            xBlockPlace++;
        }
    }

    public void deleteChunk()
    {
        saveChunk();

        foreach(GameObject block in blocks)
        {
            Destroy(block);
        }
    }

    public void saveChunk()
    {
        string fileName = "filename" + chunkBegin;
        ChunkArray chunkArray = new ChunkArray(map, chunkBegin);
        BinarySerializer.Save(chunkArray, fileName);
        Debug.Log(Application.persistentDataPath + "/GameData/");
        //Here will be saving int[,] map to file
    }

    public void loadChunk(int chunkBegin)
    {
        blocks = new List<GameObject>();
        this.chunkBegin = chunkBegin;

        if (BinarySerializer.HasSaved("filename" + chunkBegin))
        {
            ChunkArray chunkArray = BinarySerializer.Load<ChunkArray>("filename" + chunkBegin);
            this.map = chunkArray.map;
        }
        else
        {
            perLineHeight = new int[width];
            map = generateArray();
            map = terrainGenerator(map);
            smoothCaves();
        }

        renderMap(map);
    }
}
