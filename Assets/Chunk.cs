using System.Collections;
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
    //Prefab object representing gound
    public static GameObject ground;
    //Prefab object representing caves
    public static GameObject cave;

    //Height in every X posiotion
    int[] perLineHeight;
    //Chunk array representation
    int[,] map;
    //the coordinate of the beginning of chunk
    private int chunkBegin;


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
        this.chunkBegin = chunkBegin;
        perLineHeight = new int[width];
        map = generateArray();
        map = terrainGenerator(map);
        smoothCaves();
        renderMap(map);
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
                if (map[x, y] == 1)
                {
                    Instantiate(ground, new Vector2(xBlockPlace, y), Quaternion.identity);
                }
                else if (map[x, y] == 2)
                {
                    Instantiate(cave, new Vector2(xBlockPlace, y), Quaternion.identity);
                }
            }
            xBlockPlace++;
        }
    }
}
