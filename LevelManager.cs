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

    public int maxItemsInContainer = 6;
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
        GetUseTile();
        GenerateTile();

        OnTileClicked.AddListener(CheckShuffleList);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ShuffleTile();
        }
    }

    public int availableSlot = 0;
    public int type0slot = 0;
    public int type1slot = 0;
    public int type2slot = 0;
    public int type3slot = 0;
    public int type4slot = 0;
    public int type5slot = 0;

    //Get all use tile
    void GetUseTile()
    {
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
                            availableSlot++;
                        }
                    }
                
                } 
            }
        }

        type0slot = availableSlot / 6;
        type1slot = availableSlot / 6;
        type2slot = availableSlot / 6;
        type3slot = availableSlot / 6;
        type4slot = availableSlot / 6;
        type5slot = availableSlot / 6;
    }

    List<GameObject> shuffleTiles = new List<GameObject>();

    void CheckShuffleList()
    {
        //If Null destroy
        for (int i = 0; i < shuffleTiles.Count; i++)
        {
            if (shuffleTiles[i] == null || shuffleTiles[i].GetComponent<Tile>().picked == true)
            {
                shuffleTiles.RemoveAt(i);
            }
        }
        
    }
    public void ShuffleTile()
    {
        //Swap Gameobejct tile postision and spawn tile value with other tile
        for (int i = 0; i < shuffleTiles.Count; i++)
        {
            int rand = Random.Range(0, shuffleTiles.Count);
            Vector3 temp = shuffleTiles[i].transform.position;
            shuffleTiles[i].transform.position = shuffleTiles[rand].transform.position;
            shuffleTiles[rand].transform.position = temp;

            //Swap value, posX, posY, floorIndex
            int tempX = shuffleTiles[i].GetComponent<Tile>().posX;
            int tempY = shuffleTiles[i].GetComponent<Tile>().posY;
            int tempFloorIndex = shuffleTiles[i].GetComponent<Tile>().floorIndex;
            shuffleTiles[i].GetComponent<Tile>().posX = shuffleTiles[rand].GetComponent<Tile>().posX;
            shuffleTiles[i].GetComponent<Tile>().posY = shuffleTiles[rand].GetComponent<Tile>().posY;
            shuffleTiles[i].GetComponent<Tile>().floorIndex = shuffleTiles[rand].GetComponent<Tile>().floorIndex;
            shuffleTiles[rand].GetComponent<Tile>().posX = tempX;
            shuffleTiles[rand].GetComponent<Tile>().posY = tempY;
            shuffleTiles[rand].GetComponent<Tile>().floorIndex = tempFloorIndex;

            //Swap parent if not same
            if (shuffleTiles[i].transform.parent != shuffleTiles[rand].transform.parent)
            {
                Transform tempParent = shuffleTiles[i].transform.parent;
                shuffleTiles[i].transform.parent = shuffleTiles[rand].transform.parent;
                shuffleTiles[rand].transform.parent = tempParent;
            }

            //Check collide of the tile
            shuffleTiles[i].GetComponent<Tile>().ClearCollide();
            shuffleTiles[rand].GetComponent<Tile>().ClearCollide();

            

        }
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
                            int randType;
                            if (type0slot > 0)
                            {
                                randType = 0;
                                type0slot--;
                            }
                            else if (type1slot > 0)
                            {
                                randType = 1;
                                type1slot--;
                            }
                            else if (type2slot > 0)
                            {
                                randType = 2;
                                type2slot--;
                            }
                            else if (type3slot > 0)
                            {
                                randType = 3;
                                type3slot--;
                            }
                            else if (type4slot > 0)
                            {
                                randType = 4;
                                type4slot--;
                            }
                            else if (type5slot > 0)
                            {
                                randType = 5;
                                type5slot--;
                            }
                            else
                            {
                                break;
                            }

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
                            shuffleTiles.Add(tile);
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

                        //if tilemap has special tile ice
                        if (tilemap != null && tilemap.GetTile(localPlace) != null && tilemap.GetTile(localPlace).name.Contains("IceTile"))
                        {
                            Debug.Log(tilemap.GetTile(localPlace).name);
                            //Hide the tile
                            int randType = Random.Range(0, 5);
                            tilemap.SetTile(localPlace, null);
                            GameObject tile = Instantiate(specialTile[1], place, Quaternion.identity);
                            tile.transform.parent = tilemap.transform;
                            tile.GetComponent<Tile>().floorIndex = floorIndex;
                            tile.GetComponent<SpriteRenderer>().sortingOrder = floorIndex;
                            SpriteRenderer shadow = tile.transform.GetChild(0).GetComponent<SpriteRenderer>();
                            shadow.sortingOrder = floorIndex + 2;
                            tile.GetComponent<Tile>().icons = Resources.Load<Sprite>("Sprites/Fruit/" + randType);
                            tile.GetComponent<Tile>().tileType = (TileType)randType;
                            //Set tile position
                            tile.GetComponent<Tile>().isItem = true;
                            tile.GetComponent<Tile>().posX = x;
                            tile.GetComponent<Tile>().posY = y;
                            tiles.Add(tile.GetComponent<Tile>());
                        }
                        //if tilemap has special tile chain
                        if (tilemap != null && tilemap.GetTile(localPlace) != null && tilemap.GetTile(localPlace).name.Contains("ChainTile"))
                        {
                            Debug.Log(tilemap.GetTile(localPlace).name);
                            //Hide the tile
                            int randType = Random.Range(0, 5);
                            tilemap.SetTile(localPlace, null);
                            GameObject tile = Instantiate(specialTile[2], place, Quaternion.identity);
                            tile.transform.parent = tilemap.transform;
                            tile.GetComponent<Tile>().floorIndex = floorIndex;
                            tile.GetComponent<SpriteRenderer>().sortingOrder = floorIndex;
                            SpriteRenderer shadow = tile.transform.GetChild(0).GetComponent<SpriteRenderer>();
                            shadow.sortingOrder = floorIndex + 2;
                            tile.GetComponent<Tile>().icons = Resources.Load<Sprite>("Sprites/Fruit/" + randType);
                            tile.GetComponent<Tile>().tileType = (TileType)randType;
                            //Set tile position
                            tile.GetComponent<Tile>().isItem = true;
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

    
    public void CheckItemsContainer()
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
    
            var itemContainerCopy = new List<Tile>(itemContainer);

            foreach (Tile tile in itemContainerCopy)
            {
                tile.transform.SetSiblingIndex(itemContainer.IndexOf(tile));
            }


            //Check if item has 3 same type
            foreach (Tile tile in itemContainerCopy)
            {
                //Get the same type
                List<Tile> sameType = itemContainer.Where(item => item.tileType == tile.tileType).ToList();
                //If same type more than 3
                if (sameType.Count >= 3)
                {
                    AudioManager.instance.PlayMatch();
                    //Remove all same type
                    foreach (Tile item in sameType)
                    {
                        itemContainer.Remove(item);
                        tiles.Remove(item);
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


    public async void SpawnTile(int posX, int posY, int floorIndex, Tilemap tilemap)
    {
        var place = tilemap.CellToWorld(new Vector3Int(posX, posY, (int)tilemap.transform.position.y));
        //Spawn new tile
        int randType = Random.Range(0, 3);
        //Hide the tile

        await Task.Delay(1000);
        GameObject tile = Instantiate(TilePrefab, place, Quaternion.identity);
        tile.transform.parent = tilemap.transform;
        tile.GetComponent<Tile>().floorIndex = floorIndex;
        tile.GetComponent<SpriteRenderer>().sortingOrder = floorIndex;
        SpriteRenderer shadow = tile.transform.GetChild(0).GetComponent<SpriteRenderer>();
        shadow.sortingOrder = floorIndex + 2;
        tile.GetComponent<Tile>().icons = Resources.Load<Sprite>("Sprites/Fruit/" + randType);
        tile.GetComponent<Tile>().tileType = (TileType)randType;
        //Set tile position
        tile.GetComponent<Tile>().posX = posX;
        tile.GetComponent<Tile>().posY = posY;
        tiles.Add(tile.GetComponent<Tile>());
    
    }

}
