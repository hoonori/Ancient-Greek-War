using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileManager : MonoBehaviour
{
    public Tilemap tilemap;

    public Tile water1;
    public Tile water2;
    public Tile water3;

    public Tile ground1;
    public Tile ground2;
    public Tile ground3;

    public Tile mountain1;
    public Tile mountain2;
    public Tile mountain3;

    public Tile tile;

    int tileIndex;

    // Start is called before the first frame update
    void Start()
    {
        tileIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Input should be "x,y,type" where x and y being grid and type being terrain type.
    public void ChangeTile(string input)
    {
        string[] inputs = input.Split(',');
        int currGridX = System.Convert.ToInt32(inputs[0]);
        int currGridY = System.Convert.ToInt32(inputs[1]);
        string tileType = (inputs[2]);
        Vector3Int gridPosition = new Vector3Int(currGridX, currGridY, 0);

        System.Random random = new System.Random();
        tileIndex += random.Next(3);
        tileIndex %= 3;

        switch (tileType)
        {
            case "Water":
                {
                    switch (tileIndex)
                    {
                        case 0:
                            tile = water1;
                            break;
                        case 1:
                            tile = water2;
                            break;
                        case 2:
                            tile = water3;
                            break;
                    }
                    break;
                }
            case "Mountain":
                {
                    switch (tileIndex)
                    {
                        case 0:
                            tile = mountain1;
                            break;
                        case 1:
                            tile = mountain2;
                            break;
                        case 2:
                            tile = mountain3;
                            break;
                    }
                    break;
                }
            case "Ground":
                {
                    switch (tileIndex)
                    {
                        case 0:
                            tile = ground1;
                            break;
                        case 1:
                            tile = ground2;
                            break;
                        case 2:
                            tile = ground3;
                            break;
                    }
                    break;
                }
        }
        tilemap.SetTile(gridPosition, tile);
    }
}
