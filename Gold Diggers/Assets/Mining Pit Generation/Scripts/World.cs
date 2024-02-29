using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class World : MonoBehaviour
{
    //public Transform player;
    //public Vector3 spawnPosition;

    public MiningPitGenerator data;
    public Material material;
    public BlockType[] blocks;

    Chunk[,,] chunks = new Chunk[VoxelData.WorldSizeInChunks, VoxelData.WorldSizeInChunks, VoxelData.WorldSizeInChunks];

    void Start()
    {
        // spawnPosition = new Vector3((VoxelData.WorldSizeInChunks * VoxelData.ChunkWidth) / 2, VoxelData.WorldSizeInChunks * VoxelData.ChunkHeight + 2, (VoxelData.WorldSizeInChunks * VoxelData.ChunkWidth) / 2);
        GenerateWorld();
    }

    //void Update()
    //{
    //    CheckViewDistance();
    //}

    void GenerateWorld()
    {
        for (int x = 0; x < VoxelData.WorldSizeInChunks; x++) 
        {
            for (int z = 0; z < VoxelData.WorldSizeInChunks; z++)
            {
                for (int y = 0; y < VoxelData.WorldSizeInChunks; y++)
                {
                    CreateNewChunk(x, y, z);
                }
            }
        }

        //player.position = spawnPosition;
    }

    ChunkCoord GetChunkCoordFromVector3(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x / VoxelData.ChunkWidth);
        int y = Mathf.FloorToInt(pos.y / VoxelData.ChunkHeight);
        int z = Mathf.FloorToInt(pos.z / VoxelData.ChunkWidth);

        return new ChunkCoord(x, y, z);
    }

    //void CheckViewDistance()
    //{
    //    ChunkCoord coord = GetChunkCoordFromVector3(player.position);

    //    for (int x = coord.x - VoxelData.ViewDistanceInChunks; x < coord.x + VoxelData.ViewDistanceInChunks; x++)
    //    {
    //        for (int z = coord.z - VoxelData.ViewDistanceInChunks; z < coord.z + VoxelData.ViewDistanceInChunks; z++)
    //        {
    //            for (int y = 0; y < VoxelData.WorldSizeInChunks; y++)
    //            {
    //                if (IsChunkInWorld(new ChunkCoord(x, y, z)))
    //                {
    //                    if (chunks[x, y, z] == null)
    //                    {
    //                        CreateNewChunk(x, y, z);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    /* * * * * * * * * * * * * * * * *
     *                               *
     *     INSERT CHUNK DATA HERE    *
     *                               *
     * * * * * * * * * * * * * * * * */
    public byte GetVoxel(Vector3 pos)
    {
        if (!IsVoxelInWorld(pos))
        {
            return 0;
        }
        else
        {
            return data.blockList[(int)pos.z][(int)pos.x][(int)pos.y];
        }
    }

    void CreateNewChunk(int x, int y, int z)
    {
        chunks[x, y, z] = new Chunk(new ChunkCoord(x, y, z), this);
    }

    bool IsChunkInWorld(ChunkCoord coord)
    {
        if (coord.x > 0 && coord.x < VoxelData.WorldSizeInChunks - 1 && coord.y > 0 && coord.y < VoxelData.WorldSizeInChunks - 1 && coord.z > 0 && coord.z < VoxelData.WorldSizeInChunks - 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsVoxelInWorld(Vector3 pos)
    {
        if (pos.x >= 0 && pos.x < VoxelData.WorldSizeInVoxels && pos.y >= 0 && pos.y < VoxelData.WorldSizeInVoxels && pos.z >= 0 && pos.z < VoxelData.WorldSizeInVoxels)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

[System.Serializable]
public class BlockType
{
    public string name;
    public bool isSolid;

    [Header("Texture Values")]
    public int backFaceTexture;
    public int frontFaceTexture;
    public int topFaceTexture;
    public int bottomFaceTexture;
    public int leftFaceTexture;
    public int rightFaceTexture;

    public int GetTextureID(int faceIndex)
    {
        switch (faceIndex)
        {
            case 0:
                return backFaceTexture;
            case 1:
                return frontFaceTexture;
            case 2:
                return topFaceTexture;
            case 3:
                return bottomFaceTexture;
            case 4:
                return leftFaceTexture;
            case 5:
                return rightFaceTexture;
            default:
                Debug.Log("Error in GetTextureID; invalid face index");
                return 0;
        }
    }
}
