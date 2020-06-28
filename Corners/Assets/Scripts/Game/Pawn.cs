using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
namespace Game.Corners {
    public class Pawn : MonoBehaviour, IPawn {
        public Cell cell;
        [SerializeField]
        private LayerMask cellLayer;

        public GameObject selectedRect;

        public bool isSelected {
            get { return _isSelected; }
            set { _isSelected = value; }
        }
        private bool _isSelected = false;
        public void Init () {
            if (!cell) {
                SearchCellToGuestMe();
            }
        }

        public void MoveTo(Vector2 point){
            cell.guestPawn = null;
            cell = null;
            transform.position = point;
            SearchCellToGuestMe();
        }

        public void SearchCellToGuestMe () {
            var point = new Vector2 (transform.position.x, transform.position.y);
            var overlapCell = CheckOverlap (point);
            cell = overlapCell;
            cell.guestPawn = this;
            transform.position = cell.transform.position;
        }

        public void UpdateSelectedRect () {
            selectedRect.SetActive (_isSelected);
        }

        public string GetTag () {
            return gameObject.tag;
        }

        public Cell GetCell () {
            return cell;
        }

        public void SetSelected(bool value){
            _isSelected = value;
        }

        public bool GetSelected(){
            return _isSelected;
        }

        private Cell CheckOverlap (Vector2 point) {
            int mask = Utils.LayerMaskToInt (cellLayer);
            Cell res = null;
            var ray = Physics2D.Raycast (point, Vector2.up, 1f, 1 << mask);
            var collider = ray.collider;

            //var collider = Physics2D.OverlapBox (point, new Vector2 (0.0003f, 0.0003f), 0, 1 << mask);
            if (collider) {
                res = collider.gameObject.GetComponent<Cell> ();
            }

            return res;
        }
    }
}