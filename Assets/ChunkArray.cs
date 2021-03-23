using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[System.Serializable]class ChunkArray
{
    //Chunk array representation
    public int[,] map;
    public int x;

    public ChunkArray()
    {

    }

    public ChunkArray(int[,] map, int x)
    {
        this.map = map;
        this.x = x;
    }

    public int drawMap()
    {
        return map[0, 0];
    }
}

