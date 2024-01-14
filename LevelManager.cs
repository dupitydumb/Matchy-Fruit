using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using System.Linq;
using System.Threading.Tasks;

public class LevelManager : MonoBehaviour
{

    public Tilemap[] tilemap;

    public GameObject[] specialTile;
    public GameObject TilePrefab;

    public UnityEvent OnTileClicked;
    public GameObject container;
    public List<Tile> itemContainer = new List<Tile>();

    public List<Tile> tiles = new List<Tile>();
    public static LevelManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GenerateTile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void GenerateTile()
    {
        int floorIndex = 0;
        foreach (Tilemap tilemap in tilemap)
        {   

            for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++)
            {
                for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
                {
                    Vector3Int localPlace = (new Vector3Int(x, y, (int)tilemap.transform.position.y));
                    Vector3 place = tilemap.CellToWorld(localPlace);
                    if (tilemap.HasTile(localPlace))
                    {
                        if (tilemap.GetTile(localPlace).name.Contains("tile"))
                        {
                            Debug.Log(tilemap.GetTile(localPlace).name);
                            int randType = Random.Range(0, 3);
                            //Hide the tile
                            tilemap.SetTile(localPlace, null);
                            GameObject tile = Instantiate(TilePrefab, place, Quaternion.identity);
                            tile.transform.parent = tilemap.transform;
                            tile.GetComponent<Tile>().floorIndex = floorIndex;
                            tile.GetComponent<SpriteRenderer>().sortingOrder = floorIndex;
                            SpriteRenderer shadow = tile.transform.GetChild(0).GetComponent<SpriteRenderer>();
                            shadow.sortingOrder = floorIndex + 2;
                            tile.GetComponent<Tile>().icons = Resources.Load<Sprite>("Sprites/Fruit/" + randType);
                            tile.GetComponent<Tile>().tileType = (TileType)randType;
                            //Set tile position
                            tile.GetComponent<Tile>().posX = x;
                            tile.GetComponent<Tile>().posY = y;
                            tiles.Add(tile.GetComponent<Tile>());
                        }

                        if (tilemap != null && tilemap.GetTile(localPlace) != null && tilemap.GetTile(localPlace).name.Contains("SpecialFrog"))
                        {
                            Debug.Log(tilemap.GetTile(localPlace).name);
                            //Hide the tile
                            tilemap.SetTile(localPlace, null);
                            GameObject tile = Instantiate(specialTile[0], place, Quaternion.identity);
                            tile.transform.parent = tilemap.transform;
                            tile.GetComponent<Tile>().floorIndex = floorIndex;
                            tile.GetComponent<SpriteRenderer>().sortingOrder = floorIndex;
                            SpriteRenderer shadow = tile.transform.GetChild(0).GetComponent<SpriteRenderer>();
                            shadow.sortingOrder = floorIndex + 2;
                            tile.GetComponent<Tile>().icons = Resources.Load<Sprite>("Sprites/Items/1");
                            tile.GetComponent<Tile>().tileType = TileType.Special_1;
                            //Set tile position
                            tile.GetComponent<Tile>().posX = x;
                            tile.GetComponent<Tile>().posY = y;
                            tiles.Add(tile.GetComponent<Tile>());
                        }
                    }

                    //if tilemap has special tile
                    
                }
            }
            floorIndex++;
            floorIndex += 2;
        }  
          
    }


    public async void CheckItemsContainer()
    {
        try
        {
            //Sort by tile type
            itemContainer = itemContainer.OrderBy(item => (int)item.tileType).ToList();
            
             //Set sibling
            
            
            foreach (Tile tile in itemContainer)
            {
                Debug.Log(tile.tileType);
                //if dont have rect transform then add
                if (tile.GetComponent<RectTransform>() == null)
                {
                    tile.gameObject.AddComponent<RectTransform>();
                }
                //Set tile postision to item container position
                tile.transform.position = container.transform.position;
                //Set tile parent to item container
    
                //Set Parent
                tile.transform.SetParent(container.transform);

                
                
            }
    
            foreach (Tile tile in itemContainer)
            {
                tile.transform.SetSiblingIndex(itemContainer.IndexOf(tile));
            }
           
    
            await Task.Delay(100);
    
            //Check if item has 3 same type
            foreach (Tile tile in itemContainer)
            {
                //Get the same type
                List<Tile> sameType = itemContainer.Where(item => item.tileType == tile.tileType).ToList();
                //If same type more than 3
                if (sameType.Count >= 3)
                {
                    //Remove all same type
                    foreach (Tile item in sameType)
                    {
                        itemContainer.Remove(item);
                        Destroy(item.gameObject);
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }


    }


}
