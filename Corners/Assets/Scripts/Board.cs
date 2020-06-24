using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Processor;
namespace Game.Corners {
    public class Board : MonoBehaviour {

        #region CustomInspector
        [HideInInspector]
        public int horizontalCells = 8;
        [HideInInspector]
        public int verticalCells = 8;
        [HideInInspector]
        public Sprite whiteCellSprite;
        [HideInInspector]
        public Sprite blackCellSprite;
        #endregion

        public int boardArrayFullLength = 0;
        public SpriteRenderer[] cellSprites;
        public GameObject[] gameObjects;
        public GameObject lastChild;
        void Start () {
            if (transform.childCount == 0) {
                InitBoard ();
            } else {
                if (!lastChild) {
                    lastChild = transform.GetChild (transform.childCount - 1).gameObject;
                }
            }
        }

        public void CenteredBoardInWorld () {
            if (lastChild) {
                var newPos = new Vector2 ();
                newPos.y = -(lastChild.transform.localPosition.y / 2);
                newPos.x = -(lastChild.transform.localPosition.x / 2);
                transform.position = newPos;
            }
        }

        public void ClearBoard () {
#if UNITY_EDITOR
            var tempList = transform.Cast<Transform> ().ToList ();
            foreach (var child in tempList) {
                DestroyImmediate (child.gameObject);
            }
            // if (isForceCenteringBoard) {
            //     transform.position = new Vector3 (0f, 0f, 0f);
            // }

#else
            foreach (Transform child in transform) {
                Destroy (child.gameObject);
            }
            if (isForceCenteringBoard) {
                transform.position = new Vector3 (0f, 0f, 0f);
            }
#endif
        }

        public void CenteredToChilds () {
            var sumPositions = new Vector3 (0f, 0f, 0f);
            Transform[] tempChilds = new Transform[transform.childCount];
            var i = 0;
            var childCount = transform.childCount;
#if UNITY_EDITOR
            var tempList = transform.Cast<Transform> ().ToList ();
            foreach (var child in tempList) {
                sumPositions += child.transform.position;
                tempChilds[i] = child;
                child.parent = null;
                i++;
            }
#else
            foreach (Transform child in transform) {
                sumPositions += child.position;
                child.parent = null;
                tempChilds[i] = child;
                i++;
            }
#endif
            sumPositions /= childCount;
            transform.position = sumPositions;
            for (var j = 0; j < tempChilds.Length; j++) {
                tempChilds[j].SetParent (transform);
            }

        }

        public void InitBoard () {
            var prevPosition = transform.position;
            var prefab = Resources.Load ("Prefabs/Cell") as GameObject;
            var sizeH = whiteCellSprite.bounds.size.x;
            var sizeV = blackCellSprite.bounds.size.y;
            Vector2 pos = transform.position;
            boardArrayFullLength = horizontalCells * verticalCells;
            var next = 0;
            gameObjects = new GameObject[boardArrayFullLength];
            cellSprites = new SpriteRenderer[boardArrayFullLength];
            bool change = false;
            var backgroundColor = blackCellSprite;
            //------------------------------------------------------------

            for (int i = 0; i < verticalCells; i++) {
                for (int j = 0; j < horizontalCells; j++) {
                    pos = new Vector2 (sizeH * j, sizeV * i);
                    lastChild = Instantiate (prefab, pos, Quaternion.identity);
                    gameObjects[next] = lastChild;
                    cellSprites[next] = lastChild.GetComponent<SpriteRenderer> ();

                    change = next % horizontalCells == 0 || next == 0;
                    backgroundColor = change ? backgroundColor : backgroundColor == whiteCellSprite ? blackCellSprite : whiteCellSprite;
                    lastChild.GetComponent<SpriteRenderer> ().sprite = backgroundColor;
                    change = false;
                    next++;
                }
            }

            // var sumPositions = new Vector3 (0f, 0f, 0f);
            // for (var i = 0; i < gameObjects.Length; i++) {
            //     var child = gameObjects[i].transform;
            //     sumPositions += child.position;
            // }
            // sumPositions /= gameObjects.Length;



            var sumPositions = Utils.GetMiddlePositionTransforms(gameObjects);
            for (var i = 0; i < gameObjects.Length; i++) {
                var child = gameObjects[i].transform;
                child.position = child.position - sumPositions + transform.position;
                child.SetParent (transform);
            }

            transform.position = prevPosition;
        }

        public void ColoredChessBoard () {
            bool change = false;
            var backgroundColor = blackCellSprite;
            for (int i = 0; i < boardArrayFullLength; ++i) {
                change = i % horizontalCells == 0 || i == 0;
                backgroundColor = change ? backgroundColor : backgroundColor == whiteCellSprite ? blackCellSprite : whiteCellSprite;
                cellSprites[i].sprite = backgroundColor;
                change = false;
            }
        }

        public void ResetBoard () {
            ClearBoard ();
            InitBoard ();
            // if (isForceCenteringBoard) {
            //     CenteredBoardInWorld ();
            // }
        }
    }
}