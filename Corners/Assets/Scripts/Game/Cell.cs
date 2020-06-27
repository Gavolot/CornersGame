using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Corners {
    public class Cell : MonoBehaviour {
        public GameObject selectedRect;
        public Board board;

        public Pawn guestPawn;

        private float width;
        private float height;
        #region neighbours
        public Transform[] neighborsTransforms;
        public Cell[] neighborsCells;
        private Transform _up;
        public Transform Up {
            get { return _up; }
            private set { _up = value; }
        }
        private Transform _down;
        public Transform Down {
            get { return _down; }
            private set { _down = value; }
        }
        private Transform _left;
        public Transform Left {
            get { return _left; }
            private set { _left = value; }
        }
        private Transform _right;
        public Transform Right {
            get { return _right; }
            private set { _right = value; }
        }
        private Transform _upRight;
        public Transform UpRight {
            get { return _upRight; }
            private set { _upRight = value; }
        }
        private Transform _upLeft;
        public Transform UpLeft {
            get { return _upLeft; }
            private set { _upLeft = value; }
        }
        private Transform _downRight;
        public Transform DownRight {
            get { return _downRight; }
            private set { _downRight = value; }
        }
        private Transform _downLeft;
        public Transform DownLeft {
            get { return _downLeft; }
            private set { _downLeft = value; }
        }
        #endregion
        SpriteRenderer spriteRenderer;

        void Start () {
            spriteRenderer = GetComponent<SpriteRenderer> ();
            width = spriteRenderer.sprite.bounds.size.x;
            height = spriteRenderer.sprite.bounds.size.y;
            ResetNeighbors ();
        }
        public void ResetNeighbors () {
            neighborsTransforms = new Transform[8];
            neighborsCells = new Cell[8];
            var point = Vector2.zero;
            point = new Vector2 (transform.position.x + width, transform.position.y + height); //RightUp
            neighborsTransforms[0] = CheckOverlap (point);
            if (neighborsTransforms[0]) {
                neighborsCells[0] = neighborsTransforms[0].gameObject.GetComponent<Cell> ();
            }
            _upRight = neighborsTransforms[0];

            point = new Vector2 (transform.position.x - width, transform.position.y + height); //LeftUp
            neighborsTransforms[1] = CheckOverlap (point);
            if (neighborsTransforms[1]) {
                neighborsCells[1] = neighborsTransforms[1].gameObject.GetComponent<Cell> ();
            }
            _upLeft = neighborsTransforms[1];

            point = new Vector2 (transform.position.x + width, transform.position.y - height); //RightDown
            neighborsTransforms[2] = CheckOverlap (point);
            if (neighborsTransforms[2]) {
                neighborsCells[2] = neighborsTransforms[2].gameObject.GetComponent<Cell> ();
            }
            _downRight = neighborsTransforms[2];

            point = new Vector2 (transform.position.x - width, transform.position.y - height); //LeftDown
            neighborsTransforms[3] = CheckOverlap (point);
            if (neighborsTransforms[3]) {
                neighborsCells[3] = neighborsTransforms[3].gameObject.GetComponent<Cell> ();
            }
            _downLeft = neighborsTransforms[3];

            point = new Vector2 (transform.position.x + width, transform.position.y); //Right
            neighborsTransforms[4] = CheckOverlap (point);
            if (neighborsTransforms[4]) {
                neighborsCells[4] = neighborsTransforms[4].gameObject.GetComponent<Cell> ();
            }
            _right = neighborsTransforms[4];

            point = new Vector2 (transform.position.x - width, transform.position.y); //Left
            neighborsTransforms[5] = CheckOverlap (point);
            if (neighborsTransforms[5]) {
                neighborsCells[5] = neighborsTransforms[5].gameObject.GetComponent<Cell> ();
            }
            _left = neighborsTransforms[5];

            point = new Vector2 (transform.position.x, transform.position.y + height); //Up
            neighborsTransforms[6] = CheckOverlap (point);
            if (neighborsTransforms[6]) {
                neighborsCells[6] = neighborsTransforms[6].gameObject.GetComponent<Cell> ();
            }
            _up = neighborsTransforms[6];

            point = new Vector2 (transform.position.x, transform.position.y - height); //Down
            neighborsTransforms[7] = CheckOverlap (point);
            if (neighborsTransforms[7]) {
                neighborsCells[7] = neighborsTransforms[7].gameObject.GetComponent<Cell> ();
            }
            _down = neighborsTransforms[7];
        }
        private Transform CheckOverlap (Vector2 point) {
            Transform res = null;
            var collider = Physics2D.OverlapBox (point, new Vector2 (0.0003f, 0.0003f), 0, 1 << gameObject.layer);
            if (collider) {
                res = collider.gameObject.transform;
            }

            return res;
        }
    }
}