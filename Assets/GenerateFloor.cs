using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFloor : MonoBehaviour
{
    public GameObject floorTileWhite;
    public GameObject floorTileBlack;
    public GameObject BorderPrefab;
    public int x, y;
    public float tileSize = 1.0f;

    private void Awake()
    {
        if (x % 2 == 0 || y % 2 == 0)
        {
            Debug.LogWarning("x and y should be odd for a center-aligned grid.");
        }

        int xOffset = x / 2;
        int yOffset = y / 2;

        
        for (int i = -xOffset; i <= xOffset; i++)
        {
            for (int j = -yOffset; j <= yOffset; j++)
            {
                GameObject tilePrefab = ((i + j) % 2 == 0) ? floorTileWhite : floorTileBlack;

               
                GameObject spawnedTile = Instantiate(tilePrefab, new Vector3(i * tileSize, -1, j * tileSize), Quaternion.identity);

                
                spawnedTile.transform.parent = this.transform;
            }
        }

        
        SpawnBorderTiles(xOffset, yOffset);
        DontDestroyOnLoad(this.gameObject);
    }

    void SpawnBorderTiles(int xOffset, int yOffset)
    {
        float borderHeight = 0.6f;

        for (int i = -xOffset; i <= xOffset; i++)
        {
            GameObject topBorder = Instantiate(BorderPrefab, new Vector3(i * tileSize, borderHeight / 2, yOffset * tileSize), Quaternion.identity);
            topBorder.transform.parent = this.transform;

            GameObject bottomBorder = Instantiate(BorderPrefab, new Vector3(i * tileSize, borderHeight / 2, -yOffset * tileSize), Quaternion.identity);
            bottomBorder.transform.parent = this.transform;
        }

        for (int j = -yOffset; j <= yOffset; j++)
        {
            GameObject rightBorder = Instantiate(BorderPrefab, new Vector3(xOffset * tileSize, borderHeight / 2, j * tileSize), Quaternion.identity);
            rightBorder.transform.parent = this.transform;

            GameObject leftBorder = Instantiate(BorderPrefab, new Vector3(-xOffset * tileSize, borderHeight / 2, j * tileSize), Quaternion.identity);
            leftBorder.transform.parent = this.transform;
        }
    }
}
