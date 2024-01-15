using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Linq;

public enum TileType
{
    Normal_1,
    Normal_2,
    Normal_3,
    Normal_4,
    Normal_5,
    Normal_6,
    
    Special_1,
    Special_2,
    Special_3,

}


public class Tile : MonoBehaviour
{

    public Sprite icons;
    public int floorIndex;
    //Hide in inspector
    [HideInInspector] public bool isAvailable = true;

    
    public int posX;
    public int posY;

    public GameObject shadow;

    public TileType tileType;
    public bool isItem = false;
    public bool picked = false;
    public List<GameObject> collideWith = new List<GameObject>();

    LevelManager levelManager;
    // Start is called before the first frame update
    void Start()
    {
        
        levelManager = LevelManager.instance;
        Debug.Log(isAvailable);
        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = icons;
        transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = floorIndex + 1;

        if (tileType == TileType.Special_1)
        {
            isItem = true;
        }
    }


    //check if the tile is on edge
    

    // Update is called once per frame
    void Update()
    {
        
        if (!isAvailable)
        {
            shadow.SetActive(true);
        }
        if (isAvailable)
        {
            shadow.SetActive(false);
        }

        if (collideWith.Count <= 0)
        {
            isAvailable = true;
        }

        CheckCollide();

        if (Input.GetMouseButtonDown(1))
        {
            CheckBelow();
        }
    }

    /// <summary>
    /// Called when the mouse enters the GUIElement or Collider.
    /// </summary>
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isAvailable && !picked && !isItem && LevelManager.instance.itemContainer.Count < LevelManager.instance.maxItemsInContainer)
            {
                Debug.Log("Clicked");
                levelManager.itemContainer.Add(this);
                floorIndex = 0;
                picked = true;
                levelManager.OnTileClicked.Invoke();
                try {
                    levelManager.CheckItemsContainer();
                }
                catch (System.Exception e)
                {
                    Debug.Log(e);
                }
            }
            else
            {
                Debug.Log("Not Available");
            }
        }

        
    }

  


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Tile")
        {
            
            //if this floor index is lower than the other floor index
            if (floorIndex < collision.gameObject.GetComponent<Tile>().floorIndex)
            {
                isAvailable = false;
                collideWith.Add(collision.gameObject);
            }
            Debug.Log("Collision");

        }
        
    }


    public void CheckCollide()
    {
        List<GameObject> tilesToRemove = new List<GameObject>();

        foreach (GameObject tile in collideWith)
        {
            if (tile == null || tile.GetComponent<Tile>().picked == true)
            {
                tilesToRemove.Add(tile);
            }
        }

        foreach (GameObject tileToRemove in tilesToRemove)
        {
            collideWith.Remove(tileToRemove);
        }
    }


    //Frog Items
    //Spawn tile below if empty
    List<Tile> tilesBelow = new List<Tile>();

    void CheckBelow()
    {
        //Get all tiles below
        tilesBelow = levelManager.tiles.Where(tile => tile.posX == posX && tile.posY == posY-1 && tile.floorIndex == floorIndex).ToList();
        Debug.Log(tilesBelow.Count);
        //If tiles below is empty
        if (tilesBelow.Count <= 0)
        {
            //Spawn new tile
            SpawnTileBelow();
        }
        //If tiles below is not empty
    
    }

    void SpawnTileBelow()
    {
        Debug.Log("Spawn Tile Below");
    }

    
}
