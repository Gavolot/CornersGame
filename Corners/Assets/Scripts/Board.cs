using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    public Sprite whiteCellSprite;
    public Sprite blackCellSprite;

    public int horizontalCells = 8;
    public int verticalCells = 8;

    private int board_length = 0;

    private SpriteRenderer[] cellSprites;
    private GameObject[] gameObjects;
    void Start () {
        var prefab = Resources.Load ("Prefabs/Cell") as GameObject;
        var sizeH = whiteCellSprite.bounds.size.x;
        var sizeV = blackCellSprite.bounds.size.y;
        Vector2 pos = Vector2.zero;
        GameObject lastObject = null;
        board_length  = horizontalCells * verticalCells;
        var next = 0;
        gameObjects = new GameObject[board_length];
        cellSprites = new SpriteRenderer[board_length ];
        bool change = false;
        var backgroundColor = blackCellSprite;
        //var l = 0;

        for (int i = 0; i < verticalCells; i++) {
            for (int j = 0; j < horizontalCells; j++) {
                pos = new Vector2 (sizeH * j, sizeV * i);
                lastObject = Instantiate (prefab, pos, Quaternion.identity, transform);
                gameObjects[next] = lastObject;
                cellSprites[next] = lastObject.GetComponent<SpriteRenderer> ();

                change = next % 8 == 0 || next == 0;
                backgroundColor = change ? backgroundColor : backgroundColor == whiteCellSprite ? blackCellSprite : whiteCellSprite;
                lastObject.GetComponent<SpriteRenderer> ().sprite = backgroundColor;
                change = false;
                next++;
            }
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

    private void ColoredChessBoard () {
        bool change = false;
        var backgroundColor = blackCellSprite;
        for (int i = 0; i < board_length; ++i) {
            change = i % 8 == 0 || i == 0;
            backgroundColor = change ? backgroundColor : backgroundColor == whiteCellSprite ? blackCellSprite : whiteCellSprite;
            cellSprites[i].sprite = backgroundColor;
            change = false;
        }
    }
}