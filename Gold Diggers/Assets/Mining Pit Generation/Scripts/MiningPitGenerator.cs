using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningPitGenerator : MonoBehaviour
{
    public List<List<List<byte>>> blockList = new List<List<List<byte>>>();

    [Header("Data Generation Size")]
    public int sizeX = 64;
    public int sizeY = 64;
    public int sizeZ = 64;

    [Header("Data Tier Indexes")]
    public int tier1End = 63;
    public int tier2End = 47;
    public int tier3End = 31;
    public int tier4End = 15;

    [Header("Block Percentages")]
    [Range(0, 1)] public float deathChance = 0.6f;
    [Range(0, 1)] public float adamantineChance = 0.4f;
    [Range(0, 1)] public float goldChance = 0.6f;
    [Range(0, 1)] public float ironChance = 0.6f;
    [Range(0, 1)] public float dirtChance = 0.6f;

    [Header("Block Type Indexes")]
    public byte death = 1;
    public byte adamantine = 2;
    public byte gold = 3;
    public byte iron = 4;
    public byte dirt = 5;
    public byte stone = 6;
    public byte grass = 7;

    void Awake()
    {
        initialize3DList();
    }

    void initialize3DList()
    {
        bool dangerEnd = false;

        for (int i = 0; i < sizeZ; i++)
        {
            blockList.Add(new List<List<byte>>());

            for (int j = 0; j < sizeX; j++)
            {
                blockList[i].Add(new List<byte>());

                for (int k = 0; k < sizeY; k++)
                {
                    if (!dangerEnd && dangerCheck(k))
                    {
                        blockList[i][j].Add(death);
                    }
                    else
                    {
                        dangerEnd = true;
                        
                        if (k == sizeY - 1)
                        {
                            blockList[i][j].Add(grass);
                        }
                        else
                        {
                            blockList[i][j].Add(blockCheck(k));
                        }
                    }
                    
                }

                dangerEnd = false;
            }
        }
    }

    bool dangerCheck(int depth)
    {
        if (depth > tier4End)
        {
            return false;
        }
        else if (depth > 0)
        {
            if (Random.Range(0f, 1f) <= deathChance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    byte blockCheck(int depth)
    {
        if (depth > tier2End)
        {
            if (Random.Range(0f, 1f) <= dirtChance)
            {
                return dirt;
            }
        }
        else if (depth > tier3End)
        {
            if (Random.Range(0f, 1f) <= ironChance)
            {
                return iron;
            }
        }
        else if (depth > tier4End)
        {
            if (Random.Range(0f, 1f) <= goldChance)
            {
                return gold;
            }
        }
        else
        {
            if (Random.Range(0f, 1f) <= adamantineChance)
            {
                return adamantine;
            }
        }

        return stone;
    }
}
