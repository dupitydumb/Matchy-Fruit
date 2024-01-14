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
}


public class Tile : MonoBehaviour
{

    public Sprite icons;
    public int floorIndex;
    bool isAvailable = true;

    public GameObject shadow;

    public TileType tileType;

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

        LevelManager.instance.OnTileClicked.AddListener(CheckCollide);
    }

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
    }

    /// <summary>
    /// Called when the mouse enters the GUIElement or Collider.
    /// </summary>
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isAvailable)
            {
                Debug.Log("Clicked");
                levelManager.itemContainer.Add(this);
                floorIndex = 0;
                picked = true;
                try {
                    levelManager.CheckItemsContainer();
                }
                catch (System.Exception e)
                {
                    Debug.Log(e);
                }
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
        foreach (GameObject tile in collideWith)
        {
            if (tile == null || tile.GetComponent<Tile>().picked == true)
            {
                collideWith.Remove(tile);
            }
        }
    }

    
}
