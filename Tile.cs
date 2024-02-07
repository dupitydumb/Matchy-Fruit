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
    public bool isAvailable = true;

    
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
        
        if (tileType == TileType.Special_1)
        {
            isItem = true;
        }
    }


    void SetShadowFloor()
    {
        
        transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = floorIndex + 1;
        SpriteRenderer shadow = transform.GetChild(0).GetComponent<SpriteRenderer>();
        shadow.sortingOrder = floorIndex + 2;
    }
    
    void SetLayerSorting()
    {
        // Set the sorting layer to the parent's sorting layer
        SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = floorIndex;
    }
    

    // Update is called once per frame
    void Update()
    {
        SetShadowFloor();
        SetLayerSorting();
        if (collideWith.Count <= 0)
        {
            isAvailable = true;
        }
        if (!isAvailable)
        {
            shadow.SetActive(true);
        }
        if (isAvailable)
        {
            shadow.SetActive(false);
        }

        

        CheckCollide();

        
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

  
    public void ClearCollide()
    {
        collideWith.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        //If Collide with tile and tile floor index is lower than this tile
        if (collision.gameObject.tag == "Tile" && collision.gameObject.GetComponent<Tile>().floorIndex > floorIndex)
        {
            //if collide have not been added to the list
            if (!collideWith.Contains(collision.gameObject))
            {
                //Add to the list
                collideWith.Add(collision.gameObject);
                isAvailable = false;
            }
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
