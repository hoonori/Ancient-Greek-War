using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile water;
    public Tile mountain;
    public Tile ground;
    
    // Start is called before the first frame update
    void Start()
    {
        
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

        switch(tileType)
        {
            case "Water":
                tilemap.SetTile(gridPosition, water);
                break;
            case "Mountain":
                tilemap.SetTile(gridPosition, mountain);
                break;
            case "Ground":
                tilemap.SetTile(gridPosition, ground);
                break;
        }
    }
}
