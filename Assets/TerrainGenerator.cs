using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] float smoothness;
    [SerializeField] int caveSmoothness;
    [SerializeField] float seed;
    [Range(0, 100)]
    [SerializeField] int fill;
    [SerializeField] GameObject ground;
    [SerializeField] GameObject cave;
    public GameObject player;
    int[] perLineHeight;
    int[,] map;

    // Start is called before the first frame update
    void Start()
    {
        perLineHeight = new int[width];
        map = generateArray(true);
        map = terrainGenerator(map);
        smoothMap();
        renderMap(map);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.RightArrow)) ;
        Debug.Log(getPlayerX());
    }

    private int[,] generateArray(bool empty)
    {
        int[,] map = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = empty ? 0 : 1;
            }
        }

        return map;
    }

    public int getPlayerX()
    {
        return (int)player.transform.position.x;
    }

    private int getSurroundingGround(int gridX, int gridY)
    {
        int groundCount = 0;

        for(int nebX = gridX - 1; nebX <= gridX + 1; nebX++)
        {
            for(int nebY = gridY - 1; nebY <= gridY + 1; nebY++)
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

    private void smoothMap()
    {
        int surroudingGround;
        for (int i = 0; i < caveSmoothness; i++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < perLineHeight[x]; y++)
                {
                    if(x == 0 || y == 0  || x == width - 1 || y == perLineHeight[x] - 1)
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
    private int[,] terrainGenerator(int[,] map)
    {
        int perlinHeight;
        System.Random random = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / smoothness, seed) * height);
            perLineHeight[x] = perlinHeight;

            for (int y = 0; y < perlinHeight; y++)
            {
                map[x, y] = (random.Next(1, 100) < fill) ? 1 : 2;   
            }
        }

        return map;
    }

    private void renderMap(int[,] map)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    Instantiate(ground, new Vector2(x, y), Quaternion.identity);
                }
                else if(map[x,y] == 2)
                {
                    Instantiate(cave, new Vector2(x, y), Quaternion.identity);
                }
            }
        }
    }
}