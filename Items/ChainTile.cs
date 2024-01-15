using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainTile : MonoBehaviour
{

    public GameObject chainIcon;
    public List<Tile> sideTiles = new List<Tile>();
    // Start is called before the first frame update
    void Start()
    {
        CheckSideTiles();
        chainIcon.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder + 2;
        LevelManager.instance.OnTileClicked.AddListener(CheckSideTiles);
    }

    // Update is called once per frame
    void Update()
    {
        if (sideTiles.Count <= 0)
        {
            chainIcon.SetActive(false);
            if (gameObject.GetComponentInParent<Tile>().isItem)
            {
                gameObject.GetComponentInParent<Tile>().isItem = false;
            }
        }
    }

    void CheckSideTiles()
    {
        sideTiles.Clear();
        //Search for tiles on the left and right
        int floorIndex = gameObject.GetComponent<Tile>().floorIndex;
        Tile leftTile = LevelManager.instance.tiles.Find(tile => tile.posX == gameObject.GetComponent<Tile>().posX - 1 && tile.posY == gameObject.GetComponent<Tile>().posY && tile.floorIndex == floorIndex);
        Tile rightTile = LevelManager.instance.tiles.Find(tile => tile.posX == gameObject.GetComponent<Tile>().posX + 1 && tile.posY == gameObject.GetComponent<Tile>().posY && tile.floorIndex == floorIndex);
        
        //Add if isnt already in the list
        if (leftTile != null && !sideTiles.Contains(leftTile))
        {
            sideTiles.Add(leftTile);
        }
        if (rightTile != null && !sideTiles.Contains(rightTile))
        {
            sideTiles.Add(rightTile);
        }


    }
}
