using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Processor;
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
        void Start () {
            if (transform.childCount == 0) {
                Init ();
            }
        }
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
            //... Initialize
            cellsLayerMask = LayerMask.GetMask ("BoardCells");
            var prevPosition = transform.position;
            var prefab = Resources.Load ("Prefabs/Cell") as GameObject;
            var sizeH = whiteCellSprite.bounds.size.x;
            var sizeV = blackCellSprite.bounds.size.y;
            Vector2 pos = transform.position;
            boardArrayFullLength = horizontalCells * verticalCells;
            var next = 0;
            var gameObjects = new GameObject[boardArrayFullLength];
            cells = new Cell[boardArrayFullLength];
            bool change = false;
            var backgroundColor = blackCellSprite;
            //------------------------------------------------------------

            for (int i = 0; i < verticalCells; i++) {
                for (int j = 0; j < horizontalCells; j++) {
                    pos = new Vector2 (sizeH * j, sizeV * i);
                    var lastChild = Instantiate (prefab, pos, Quaternion.identity);
                    gameObjects[next] = lastChild;
                    var cell = lastChild.GetComponent<Cell> ();
                    cells[next] = cell;
                    cell.board = this;

                    #region sprite for cell
                    change = next % horizontalCells == 0 || next == 0;
                    backgroundColor = change ? backgroundColor : backgroundColor == whiteCellSprite ? blackCellSprite : whiteCellSprite;
                    lastChild.GetComponent<SpriteRenderer> ().sprite = backgroundColor;
                    change = false;
                    #endregion
                    next++;
                }
            }
            #region centering
            var sumPositions = Utils.GetMiddlePositionTransforms (gameObjects);
            for (var i = 0; i < gameObjects.Length; i++) {
                var child = gameObjects[i].transform;
                child.position = child.position - sumPositions + transform.position;
                child.SetParent (transform);
            }
            transform.position = prevPosition;
            #endregion

            //
        }

        public void ResetBoard () {
            ClearBoard ();
            Init ();
        }

        public void DeselectAll(){
            foreach(var c in cells){
                if(c != null){
                    c.selectedRect.SetActive(false);
                }
            }
        }
    }
}