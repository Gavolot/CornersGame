using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    public Sprite whiteCellSprite;
    public Sprite blackCellSprite;

    public int horizontalCells = 8;
    public int verticalCells = 8;


    private SpriteRenderer[] cellSprites;
    private GameObject[] gameObjects;
    void Start () {
        var prefab = Resources.Load ("Prefabs/Cell") as GameObject;
        var sizeH = whiteCellSprite.bounds.size.x;
        var sizeV = blackCellSprite.bounds.size.y;
        Vector2 pos = Vector2.zero;
        GameObject lastObject = null;
        int len = horizontalCells*verticalCells;
        Debug.Log(len);
        var next = 0;
        gameObjects = new GameObject[len];
        cellSprites = new SpriteRenderer[len];


        for (int i = 0; i < verticalCells; i++) {
            for (int j = 0; j < horizontalCells; j++) {
                pos = new Vector2 (sizeH * j, sizeV * i);
                lastObject = Instantiate (prefab, pos, Quaternion.identity, transform);
                gameObjects[next] = lastObject;
                cellSprites[next] = lastObject.GetComponent<SpriteRenderer>();
                next++;
            }
        }

        bool change = false;

        var backgroundColor = blackCellSprite;
        
        for(int i = 0; i<len; ++i){
            change = i % 8 == 0 || i == 0;
            backgroundColor = change ? backgroundColor : backgroundColor == whiteCellSprite ? blackCellSprite : whiteCellSprite;
            cellSprites[i].sprite = backgroundColor;
            change = false;
        }

        #region centeredToBoard
        var newPos = new Vector2 ();
        newPos.y = -(lastObject.transform.localPosition.y / 2);
        newPos.x = -(lastObject.transform.localPosition.x / 2);
        transform.position = newPos;
        #endregion
    }
    void Update () {

    }
}