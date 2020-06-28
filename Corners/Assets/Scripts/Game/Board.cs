using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
namespace Game.Corners {
    public class Board : MonoBehaviour, IBoard {
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

        public LayerMask cellsLayerMask;

        public int boardArrayFullLength = 0;
        public Cell[] cells;

        private GameObject[] gameObjectCells;
        public void ClearBoard () {
#if UNITY_EDITOR
            var tempList = transform.Cast<Transform> ().ToList ();
            foreach (var child in tempList) {
                DestroyImmediate (child.gameObject);
            }
#else
            foreach (Transform child in transform) {
                Destroy (child.gameObject);
            }
#endif
        }
        public void Init () {
            if (transform.childCount == 0) {
                MakeGrid();
            }
#if UNITY_EDITOR
            foreach (var cell in cells) {
                cell.Init ();
            }
#else
            StartCoroutine (CellsInit ());
#endif
        }

        private void MakeGrid () {
            //... InitializeData
            cellsLayerMask = LayerMask.GetMask ("BoardCells");
            var prevPosition = transform.position;
            var prefab = Resources.Load ("Prefabs/Cell") as GameObject;
            var sizeH = whiteCellSprite.bounds.size.x;
            var sizeV = blackCellSprite.bounds.size.y;
            Vector2 pos = transform.position;
            boardArrayFullLength = horizontalCells * verticalCells;
            var next = 0;
            gameObjectCells = new GameObject[boardArrayFullLength];
            cells = new Cell[boardArrayFullLength];
            bool change = false;
            var backgroundColor = blackCellSprite;
            //------------------------------------------------------------
            #region newCells
            for (int i = 0; i < verticalCells; i++) {
                for (int j = 0; j < horizontalCells; j++) {
                    pos = new Vector2 (sizeH * j, sizeV * i);

                    var lastChild = NewCell (prefab, pos, ref next);

                    #region sprite for cell
                    change = next % horizontalCells == 0 || next == 0;
                    backgroundColor = change ? backgroundColor : backgroundColor == whiteCellSprite ? blackCellSprite : whiteCellSprite;
                    lastChild.GetComponent<SpriteRenderer> ().sprite = backgroundColor;
                    change = false;
                    #endregion
                    next++;
                }
            }
            #endregion
            CenteringAndParent (gameObjectCells);
            transform.position = prevPosition;
        }
        private GameObject NewCell (GameObject prefab, Vector2 pos, ref int counter) {
            GameObject res = null;
            res = Instantiate (prefab, pos, Quaternion.identity);
            var cell = res.GetComponent<Cell> ();
            gameObjectCells[counter] = res;
            cells[counter] = cell;
            return res;
        }

        private void CenteringAndParent (GameObject[] gameObjects) {
            var sumPositions = Utils.GetMiddlePositionTransforms (gameObjects);
            for (var i = 0; i < gameObjects.Length; i++) {
                var child = gameObjects[i].transform;
                child.position = child.position - sumPositions + transform.position;
                child.SetParent (transform);
            }

        }

        IEnumerator CellsInit () {
            yield return new WaitForEndOfFrame ();
            foreach (var cell in cells) {
                cell.Init ();
            }
        }

        public void UpdateSelectedCells () {
            foreach (var c in cells) {
                c.UpdateSelected ();
            }
        }

        public void ResetBoard () {
            ClearBoard ();
            Init ();
        }

        public void UnCheckedAll () {
            foreach (var c in cells) {
                c.SetCheckedDown (false);
                c.SetCheckedUp (false);
                c.SetCheckedLeft (false);
                c.SetCheckedRight (false);
                c.SetCheckedRightDown (false);
                c.SetCheckedRightUp (false);
                c.SetCheckedLeftUp (false);
                c.SetCheckedLeftDown (false);
            }
        }

        public Cell[] GetCells () {
            return cells;
        }

        public void DeselectAll () {
            foreach (var c in cells) {
                if (c != null) {
                    c.SetSelected (false);
                    c.lastSeenPawn = null;
                }
            }
        }

        public int GetWidth () {
            return horizontalCells;
        }

        public int GetHeight () {
            return verticalCells;
        }
    }
}