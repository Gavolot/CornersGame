using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Corners {
    public class Cell : MonoBehaviour, ICell {
        [SerializeField]
        private GameObject selectedRect;

        public IPawn guestPawn;
        public IPawn lastSeenPawn;

        [SerializeField]
        private string _coordinateАlphabet = "a";

        [SerializeField]
        private int _coordinateNumber = 1;

        private bool isCheckedUp = false;
        private bool isCheckedRight = false;
        private bool isCheckedLeft = false;
        private bool isCheckedDown = false;

        private bool isCheckedLeftUp = false;
        private bool isCheckedRightUp = false;
        private bool isCheckedLeftDown = false;
        private bool isCheckedRightDown = false;

        private bool isSelected = false;

        private float width;
        private float height;
        #region neighbours
        public Transform[] neighborsTransforms;
        public Cell[] neighborsCells;
        private Transform _up;
        private Cell _upCell;
        public Transform Up {
            get { return _up; }
            private set { _up = value; }
        }
        public Cell UpCell {
            get { return _upCell; }
            private set { _upCell = value; }
        }
        private Transform _down;
        private Cell _downCell;
        public Transform Down {
            get { return _down; }
            private set { _down = value; }
        }
        public Cell DownCell {
            get { return _downCell; }
            private set { _downCell = value; }
        }
        private Transform _left;
        private Cell _leftCell;
        public Transform Left {
            get { return _left; }
            private set { _left = value; }
        }
        public Cell LeftCell {
            get { return _leftCell; }
            private set { _leftCell = value; }
        }
        private Transform _right;
        private Cell _rightCell;
        public Transform Right {
            get { return _right; }
            private set { _right = value; }
        }
        public Cell RightCell {
            get { return _rightCell; }
            private set { _rightCell = value; }
        }
        private Transform _upRight;
        private Cell _upRightCell;
        public Transform UpRight {
            get { return _upRight; }
            private set { _upRight = value; }
        }
        public Cell UpRightCell {
            get { return _upRightCell; }
            private set { _upRightCell = value; }
        }
        private Transform _upLeft;
        private Cell _upLeftCell;
        public Transform UpLeft {
            get { return _upLeft; }
            private set { _upLeft = value; }
        }
        public Cell UpLeftCell {
            get { return _upLeftCell; }
            private set { _upLeftCell = value; }
        }
        private Transform _downRight;
        private Cell _downRightCell;
        public Transform DownRight {
            get { return _downRight; }
            private set { _downRight = value; }
        }
        public Cell DownRightCell {
            get { return _downRightCell; }
            private set { _downRightCell = value; }
        }
        private Transform _downLeft;
        private Cell _downLeftCell;
        public Transform DownLeft {
            get { return _downLeft; }
            private set { _downLeft = value; }
        }
        public Cell DownLeftCell {
            get { return _downLeftCell; }
            private set { _downLeftCell = value; }
        }
        #endregion
        SpriteRenderer spriteRenderer;

        public void Init () {
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
                _upRightCell = neighborsCells[0];
            }
            _upRight = neighborsTransforms[0];

            point = new Vector2 (transform.position.x - width, transform.position.y + height); //LeftUp
            neighborsTransforms[1] = CheckOverlap (point);
            if (neighborsTransforms[1]) {
                neighborsCells[1] = neighborsTransforms[1].gameObject.GetComponent<Cell> ();
                _upLeftCell = neighborsCells[1];
            }
            _upLeft = neighborsTransforms[1];

            point = new Vector2 (transform.position.x + width, transform.position.y - height); //RightDown
            neighborsTransforms[2] = CheckOverlap (point);
            if (neighborsTransforms[2]) {
                neighborsCells[2] = neighborsTransforms[2].gameObject.GetComponent<Cell> ();
                _downRightCell = neighborsCells[2];
            }
            _downRight = neighborsTransforms[2];

            point = new Vector2 (transform.position.x - width, transform.position.y - height); //LeftDown
            neighborsTransforms[3] = CheckOverlap (point);
            if (neighborsTransforms[3]) {
                neighborsCells[3] = neighborsTransforms[3].gameObject.GetComponent<Cell> ();
                _downLeftCell = neighborsCells[3];
            }
            _downLeft = neighborsTransforms[3];

            point = new Vector2 (transform.position.x + width, transform.position.y); //Right
            neighborsTransforms[4] = CheckOverlap (point);
            if (neighborsTransforms[4]) {
                neighborsCells[4] = neighborsTransforms[4].gameObject.GetComponent<Cell> ();
                _rightCell = neighborsCells[4];
            }
            _right = neighborsTransforms[4];

            point = new Vector2 (transform.position.x - width, transform.position.y); //Left
            neighborsTransforms[5] = CheckOverlap (point);
            if (neighborsTransforms[5]) {
                neighborsCells[5] = neighborsTransforms[5].gameObject.GetComponent<Cell> ();
                _leftCell = neighborsCells[5];
            }
            _left = neighborsTransforms[5];

            point = new Vector2 (transform.position.x, transform.position.y + height); //Up
            neighborsTransforms[6] = CheckOverlap (point);
            if (neighborsTransforms[6]) {
                neighborsCells[6] = neighborsTransforms[6].gameObject.GetComponent<Cell> ();
                _upCell = neighborsCells[6];
            }
            _up = neighborsTransforms[6];

            point = new Vector2 (transform.position.x, transform.position.y - height); //Down
            neighborsTransforms[7] = CheckOverlap (point);
            if (neighborsTransforms[7]) {
                neighborsCells[7] = neighborsTransforms[7].gameObject.GetComponent<Cell> ();
                _downCell = neighborsCells[7];
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

        public bool GetCheckedUp () {
            return isCheckedUp;
        }
        public bool GetCheckedRight () {
            return isCheckedRight;
        }
        public bool GetCheckedLeft () {
            return isCheckedLeft;
        }
        public bool GetCheckedDown () {
            return isCheckedDown;
        }
        public bool GetCheckedLeftDown () {
            return isCheckedLeftDown;
        }
        public bool GetCheckedRightDown () {
            return isCheckedRightDown;
        }
        public bool GetCheckedRightUp () {
            return isCheckedRightUp;
        }
        public bool GetCheckedLeftUp () {
            return isCheckedLeftUp;
        }

        public void SetCheckedUp (bool value) {
            isCheckedUp = value;
        }
        public void SetCheckedDown (bool value) {
            isCheckedDown = value;
        }
        public void SetCheckedLeft (bool value) {
            isCheckedLeft = value;
        }
        public void SetCheckedRight (bool value) {
            isCheckedRight = value;
        }
        public void SetCheckedLeftDown (bool value) {
            isCheckedLeftDown = value;
        }
        public void SetCheckedRightDown (bool value) {
            isCheckedRightDown = value;
        }
        public void SetCheckedLeftUp (bool value) {
            isCheckedLeftUp = value;
        }
        public void SetCheckedRightUp (bool value) {
            isCheckedRightUp = value;
        }

        public void SetSelected(bool value){
            isSelected = value;
        }

        public bool GetSelected(){
            return isSelected;
        }

        public void UpdateSelected(){
            selectedRect.SetActive(isSelected);
        }

        public void SetHumanCoordinate(string alphabet, int number){
            _coordinateАlphabet = alphabet;
            _coordinateNumber = number;
        }

        public string GetHumanAlphaberCoordinate(){
            return _coordinateАlphabet;
        }

        public int GetHumanNumberCoordinate(){
            return _coordinateNumber;
        }
    }
}