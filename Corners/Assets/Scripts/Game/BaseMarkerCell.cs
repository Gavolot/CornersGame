using UnityEngine;

namespace Game.Corners {
    public class BaseMarkerCell : MonoBehaviour, IMarker {
        [SerializeField]
        private LayerMask cellLayer;
        public void Init () {
            var point = new Vector2 (transform.position.x, transform.position.y);
            var overlapCell = CheckOverlap (point);
            transform.position = overlapCell.transform.position;
        }

        public int GetLayer(){
            return gameObject.layer;
        }
        public string GetTag(){
            return gameObject.tag;
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