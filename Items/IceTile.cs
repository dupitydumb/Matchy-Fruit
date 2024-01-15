using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Move 3 tile before can move this

public class IceTile : MonoBehaviour
{

    int moveLeft = 3;
    bool isActivated = false;
    bool isCanMove = false;

    public GameObject iceIcon;


    // Start is called before the first frame update
    void Start()
    {
        isActivated = gameObject.GetComponent<Tile>().isAvailable;
        LevelManager.instance.OnTileClicked.AddListener(isOtherTileMove);
        iceIcon.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder + 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveLeft <= 0)
        {
            iceIcon.SetActive(false);
            if (gameObject.GetComponentInParent<Tile>().isItem)
            {
                gameObject.GetComponentInParent<Tile>().isItem = false;
            }
        }
    }

    void isOtherTileMove()
    {
        if (isActivated)
        {
            if (moveLeft > 0)
            {
                moveLeft--;
            }
            else if (moveLeft <= 0)
            {
                isCanMove = true;
                gameObject.GetComponentInParent<Tile>().isItem = false;

            }
        }
    }
}
