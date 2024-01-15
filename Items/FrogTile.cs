using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

public class FrogTile : MonoBehaviour
{
    public bool isBorder = false;
    LevelManager levelManager;

    List<Tile> tilesBelow = new List<Tile>();
    // Start is called before the first frame update
    void Start()
    {
        levelManager = LevelManager.instance;
        isBorder = CheckEdge();
        levelManager.OnTileClicked.AddListener(CheckBelow);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public bool CheckEdge()
    {
        int posX = gameObject.GetComponent<Tile>().posX;
        int posY = gameObject.GetComponent<Tile>().posY;
        int floorIndex = gameObject.GetComponent<Tile>().floorIndex;
        
        //if there is nothing below
        if (levelManager.tiles.Where(tile => tile.posX == posX && tile.posY == posY - 1 && tile.floorIndex == floorIndex).ToList().Count <= 0)
        {
            return true;
        }
        return false;

    }

    public void CheckBelow()
    {
        tilesBelow.Clear();
        int posX = gameObject.GetComponent<Tile>().posX;
        int posY = gameObject.GetComponent<Tile>().posY;
        int floorIndex = gameObject.GetComponent<Tile>().floorIndex;

        tilesBelow = levelManager.tiles.Where(tile => tile.posX == posX && tile.posY == posY - 1 && tile.floorIndex == floorIndex).ToList();

        //if tiles below is empty
        if (tilesBelow.Count <= 0)
        {
            Tilemap tilemap = gameObject.GetComponentInParent<Tilemap>();
            levelManager.SpawnTile(posX, posY - 1, floorIndex, tilemap);
            Debug.Log("Spawned");
        }
        
        
    }
}
