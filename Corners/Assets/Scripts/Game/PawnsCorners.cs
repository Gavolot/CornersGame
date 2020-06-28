using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
namespace Game.Corners {
    public enum ERules {
        Classic,
        NoJumpsFourDirs,
        NoJumpsAllDirs,
        NoJumpsDiagonals,
        Diagonals
    }
    public class PawnsCorners : MonoBehaviour {
        [HideInInspector]
        public string player_1_tag;
        [HideInInspector]
        public string player_2_tag;
        [HideInInspector]
        public string player_1_marker_tag;
        [HideInInspector]
        public string player_2_marker_tag;
        private IPawn selectedPawn = null;
        private List<IPawn> player1Pawns = null;
        private List<IPawn> player2Pawns = null;
        private Camera cameraMain;
        private string step;

        private IMarker[] player1Markers = null;
        private IMarker[] player2Markers = null;

        public ERules rules = ERules.Classic;

        IBoard board;
        [SerializeField]
        private LayerMask pawnsLayer = -1;
        [SerializeField]
        private LayerMask cellsLayer = -1;
        private void Start () {

            cameraMain = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
            board = FindObjectsOfType<MonoBehaviour> ().OfType<IBoard> ().First ();
            board.Init ();

            step = player_1_tag;
            StartCoroutine (PawnsInit ());
            StartCoroutine (MarkersInit ());
        }

        IEnumerator MarkersInit () {
            yield return new WaitForEndOfFrame ();

            var objects1 = GameObject.FindGameObjectsWithTag (player_1_marker_tag);
            var objects2 = GameObject.FindGameObjectsWithTag (player_2_marker_tag);
            player1Markers = new IMarker[objects1.Length];
            player2Markers = new IMarker[objects2.Length];

            for (var i = 0; i < objects1.Length; i++) {
                player1Markers[i] = objects1[i].GetComponent<IMarker> ();
                player1Markers[i].Init ();
            }
            for (var i = 0; i < objects2.Length; i++) {
                player2Markers[i] = objects2[i].GetComponent<IMarker> ();;
                player2Markers[i].Init ();
            }
        }
        IEnumerator PawnsInit () {
            yield return new WaitForEndOfFrame ();
            player1Pawns = new List<IPawn> ();
            player2Pawns = new List<IPawn> ();
            var pawns = FindObjectsOfType<MonoBehaviour> ().OfType<IPawn> ();
            foreach (var p in pawns) {
                p.Init ();
                if (p.GetTag () == player_1_tag) {
                    player1Pawns.Add (p);
                }
                if (p.GetTag () == player_2_tag) {
                    player2Pawns.Add (p);
                }
            }
        }

        private void Update () {
            var mousePos = Input.mousePosition;

            board.UpdateSelectedCells ();

            if (player1Pawns != null) {
                foreach (var pawn in player1Pawns) {
                    pawn.UpdateSelectedRect ();
                }
            }
            if (player2Pawns != null) {
                foreach (var pawn in player2Pawns) {
                    pawn.UpdateSelectedRect ();
                }
            }

            if (Input.GetMouseButtonDown (0)) {
                var clickPoint = cameraMain.ScreenToWorldPoint (mousePos);
                CheckMoveClick (clickPoint);
                CheckSelectedClick (clickPoint);
            }
        }

        private void CheckSelectedClick (Vector2 clickPoint) {
            Transform t;
            int maskPawns = Utils.LayerMaskToInt (pawnsLayer);
            t = CheckOverlap (clickPoint, maskPawns);
            if (t != null) {

                var pawn = t.gameObject.GetComponent<IPawn> ();
                if (pawn != null) {
                    if (pawn.GetTag () == step) {
                        DeselectAll ();
                        board.DeselectAll ();
                        board.UnCheckedAll ();
                        pawn.SetSelected (true);
                        selectedPawn = pawn;

                        var parentCell = selectedPawn.GetCell ();

                        if (rules == ERules.Classic) {
                            CheckSelectedNonDiagonalNeighbours (parentCell);
                            JumpPawnSearchClassic (pawn, parentCell);
                        } else
                        if (rules == ERules.NoJumpsFourDirs) {
                            CheckSelectedNonDiagonalNeighbours (parentCell);
                        } else
                        if (rules == ERules.NoJumpsAllDirs) {
                            CheckSelectedAllNeighbours (parentCell);
                        } else
                        if (rules == ERules.Diagonals) {
                            CheckSelectedDiagonalNeighbours (parentCell);
                            JumpPawnSearchDiagonals (pawn, parentCell);
                        } else
                        if (rules == ERules.NoJumpsDiagonals) {
                            CheckSelectedDiagonalNeighbours (parentCell);
                        }

                    }
                }
            }
        }

        private void CheckMoveClick (Vector2 clickPoint) {
            Transform t;

            int maskCells = Utils.LayerMaskToInt (cellsLayer);
            t = CheckOverlap (clickPoint, maskCells);
            if (t != null) {

                var cell = t.gameObject.GetComponent<Cell> ();

                if (cell != null) {
                    if (cell.lastSeenPawn != null) {
                        if (cell.lastSeenPawn.GetSelected ()) {
                            cell.lastSeenPawn.MoveTo (t.position);

                            if (cell.lastSeenPawn.GetTag () == player_1_tag) {

                                step = player_2_tag;

                            } else {

                                step = player_1_tag;

                            }

                            DeselectAll ();
                            board.DeselectAll ();
                            board.UnCheckedAll ();
                            return;
                        }
                    }

                }
            }
        }

        private void CheckSelectedAllNeighbours (Cell root_cell) {
            var neighbours = root_cell.neighborsCells;
            foreach (var n in neighbours) {
                if (n != null) {
                    if (n.guestPawn == null) {
                        n.SetSelected (true);
                        n.lastSeenPawn = root_cell.guestPawn;
                    }
                }
            }
        }

        private void CheckSelectedNonDiagonalNeighbours (Cell root_cell) {
            var neighbours = root_cell.neighborsCells;
            foreach (var n in neighbours) {
                if (n != null) {
                    if (n.guestPawn == null) {
                        if (root_cell.Up != null)
                            if (n.transform.position == root_cell.Up.position) {
                                n.SetSelected (true);
                                n.lastSeenPawn = root_cell.guestPawn;
                            }
                        if (root_cell.Left != null)
                            if (n.transform.position == root_cell.Left.position) {
                                n.SetSelected (true);
                                n.lastSeenPawn = root_cell.guestPawn;
                            }
                        if (root_cell.Right != null)
                            if (n.transform.position == root_cell.Right.position) {
                                n.SetSelected (true);
                                n.lastSeenPawn = root_cell.guestPawn;
                            }
                        if (root_cell.Down != null)
                            if (n.transform.position == root_cell.Down.position) {
                                n.SetSelected (true);
                                n.lastSeenPawn = root_cell.guestPawn;
                            }
                    }
                }
            }
        }
        private void CheckSelectedDiagonalNeighbours (Cell root_cell) {
            var neighbours = root_cell.neighborsCells;
            foreach (var n in neighbours) {
                if (n != null) {
                    if (n.guestPawn == null) {
                        if (root_cell.UpRight != null)
                            if (n.transform.position == root_cell.UpRight.position) {
                                n.SetSelected (true);
                                n.lastSeenPawn = root_cell.guestPawn;
                            }
                        if (root_cell.UpLeft != null)
                            if (n.transform.position == root_cell.UpLeft.position) {
                                n.SetSelected (true);
                                n.lastSeenPawn = root_cell.guestPawn;
                            }
                        if (root_cell.DownRight != null)
                            if (n.transform.position == root_cell.DownRight.position) {
                                n.SetSelected (true);
                                n.lastSeenPawn = root_cell.guestPawn;
                            }
                        if (root_cell.DownLeft != null)
                            if (n.transform.position == root_cell.DownLeft.position) {
                                n.SetSelected (true);
                                n.lastSeenPawn = root_cell.guestPawn;
                            }
                    }
                }
            }
        }
        private void JumpPawnSearchClassic (IPawn root_pawn, Cell root_cell) {
            var neighbors = root_cell.neighborsCells;

            if (root_cell.LeftCell != null) {
                if (root_cell.LeftCell.guestPawn != null) {
                    var cell = root_cell.LeftCell;
                    var list = LeftSearch (root_pawn, cell);
                    foreach (var rc in list) {
                        JumpPawnSearchClassic (root_pawn, rc);
                    }
                }
            }
            if (root_cell.RightCell != null) {
                if (root_cell.RightCell.guestPawn != null) {
                    var cell = root_cell.RightCell;
                    var list = RightSearch (root_pawn, cell);
                    foreach (var rc in list) {
                        JumpPawnSearchClassic (root_pawn, rc);
                    }
                }
            }
            if (root_cell.UpCell != null) {
                if (root_cell.UpCell.guestPawn != null) {
                    var cell = root_cell.UpCell;
                    var list = UpSearch (root_pawn, cell);
                    foreach (var rc in list) {
                        JumpPawnSearchClassic (root_pawn, rc);
                    }
                }
            }
            if (root_cell.DownCell != null) {
                if (root_cell.DownCell.guestPawn != null) {
                    var cell = root_cell.DownCell;
                    var list = DownSearch (root_pawn, cell);
                    foreach (var rc in list) {
                        JumpPawnSearchClassic (root_pawn, rc);
                    }
                }
            }

        }
        private void JumpPawnSearchDiagonals (IPawn root_pawn, Cell root_cell) {
            var neighbors = root_cell.neighborsCells;

            if (root_cell.DownLeftCell != null) {
                if (root_cell.DownLeftCell.guestPawn != null) {
                    var cell = root_cell.DownLeftCell;
                    var list = LeftDownSearch (root_pawn, cell);
                    foreach (var rc in list) {
                        JumpPawnSearchDiagonals (root_pawn, rc);
                    }
                }
            }
            if (root_cell.DownRightCell != null) {
                if (root_cell.DownRightCell.guestPawn != null) {
                    var cell = root_cell.DownRightCell;
                    var list = RightDownSearch (root_pawn, cell);
                    foreach (var rc in list) {
                        JumpPawnSearchDiagonals (root_pawn, rc);
                    }
                }
            }
            if (root_cell.UpLeftCell != null) {
                if (root_cell.UpLeftCell.guestPawn != null) {
                    var cell = root_cell.UpLeftCell;
                    var list = LeftUpSearch (root_pawn, cell);
                    foreach (var rc in list) {
                        JumpPawnSearchDiagonals (root_pawn, rc);
                    }
                }
            }
            if (root_cell.UpRightCell != null) {
                if (root_cell.UpRightCell.guestPawn != null) {
                    var cell = root_cell.UpRightCell;
                    var list = RightUpSearch (root_pawn, cell);
                    foreach (var rc in list) {
                        JumpPawnSearchDiagonals (root_pawn, rc);
                    }
                }
            }

        }

        #region NoDiagsSearchFunctions
        private List<Cell> LeftSearch (IPawn root_pawn, Cell cell) {
            var res = new List<Cell> ();
            for (var i = 0; i < board.GetWidth (); i++) {

                if (cell != null) {
                    if (cell.GetCheckedLeft ()) break;

                    if (cell.guestPawn == null && cell.RightCell.guestPawn != null) {
                        cell.SetSelected (true);
                        cell.SetCheckedLeft (true);
                        cell.lastSeenPawn = root_pawn;
                        res.Add (cell);
                        cell = cell.LeftCell;
                    } else
                    if (cell.LeftCell != null) {
                        if (cell.LeftCell.guestPawn == null && cell.guestPawn != null) {
                            cell = cell.LeftCell;
                        }
                    } else {
                        break;
                    }

                } else {
                    break;
                }
            }
            return res;
        }
        private List<Cell> RightSearch (IPawn root_pawn, Cell cell) {
            var res = new List<Cell> ();
            for (var i = 0; i < board.GetWidth (); i++) {

                if (cell != null) {
                    if (cell.GetCheckedRight ()) break;

                    if (cell.guestPawn == null && cell.LeftCell.guestPawn != null) {
                        cell.SetSelected (true);
                        cell.SetCheckedRight (true);
                        cell.lastSeenPawn = root_pawn;
                        res.Add (cell);
                        cell = cell.RightCell;
                    } else
                    if (cell.RightCell != null) {
                        if (cell.RightCell.guestPawn == null && cell.guestPawn != null) {
                            cell = cell.RightCell;
                        }
                    } else {
                        break;
                    }

                } else {
                    break;
                }
            }
            return res;
        }
        private List<Cell> UpSearch (IPawn root_pawn, Cell cell) {
            var res = new List<Cell> ();
            for (var i = 0; i < board.GetWidth (); i++) {

                if (cell != null) {
                    if (cell.GetCheckedUp ()) break;

                    if (cell.guestPawn == null && cell.DownCell.guestPawn != null) {
                        cell.SetSelected (true);
                        cell.SetCheckedUp (true);
                        cell.lastSeenPawn = root_pawn;
                        res.Add (cell);
                        cell = cell.UpCell;
                    } else
                    if (cell.UpCell != null) {
                        if (cell.UpCell.guestPawn == null && cell.guestPawn != null) {
                            cell = cell.UpCell;
                        }
                    } else {
                        break;
                    }

                } else {
                    break;
                }
            }
            return res;
        }
        private List<Cell> DownSearch (IPawn root_pawn, Cell cell) {
            var res = new List<Cell> ();
            for (var i = 0; i < board.GetWidth (); i++) {

                if (cell != null) {
                    if (cell.GetCheckedDown ()) break;

                    if (cell.guestPawn == null && cell.UpCell.guestPawn != null) {
                        cell.SetSelected (true);
                        cell.lastSeenPawn = root_pawn;
                        cell.SetCheckedDown (true);
                        res.Add (cell);
                        cell = cell.DownCell;
                    } else
                    if (cell.DownCell != null) {
                        if (cell.DownCell.guestPawn == null && cell.guestPawn != null) {
                            cell = cell.DownCell;
                        }
                    } else {
                        break;
                    }

                } else {
                    break;
                }
            }
            return res;
        }
        #endregion

        #region DiagSearchFunctions
        private List<Cell> LeftDownSearch (IPawn root_pawn, Cell cell) {
            var res = new List<Cell> ();
            for (var i = 0; i < board.GetWidth (); i++) {

                if (cell != null) {
                    if (cell.GetCheckedLeftDown ()) break;

                    if (cell.guestPawn == null && cell.UpRightCell.guestPawn != null) {
                        cell.SetSelected (true);
                        cell.SetCheckedLeftDown (true);
                        cell.lastSeenPawn = root_pawn;
                        res.Add (cell);
                        cell = cell.DownLeftCell;
                    } else
                    if (cell.DownLeftCell != null) {
                        if (cell.DownLeftCell.guestPawn == null && cell.guestPawn != null) {
                            cell = cell.DownLeftCell;
                        }
                    } else {
                        break;
                    }

                } else {
                    break;
                }
            }
            return res;
        }
        private List<Cell> RightDownSearch (IPawn root_pawn, Cell cell) {
            var res = new List<Cell> ();
            for (var i = 0; i < board.GetWidth (); i++) {

                if (cell != null) {
                    if (cell.GetCheckedRightDown ()) break;

                    if (cell.guestPawn == null && cell.UpLeftCell.guestPawn != null) {
                        cell.SetSelected (true);
                        cell.SetCheckedRightDown (true);
                        cell.lastSeenPawn = root_pawn;
                        res.Add (cell);
                        cell = cell.DownRightCell;
                    } else
                    if (cell.DownRightCell != null) {
                        if (cell.DownRightCell.guestPawn == null && cell.guestPawn != null) {
                            cell = cell.DownRightCell;
                        }
                    } else {
                        break;
                    }

                } else {
                    break;
                }
            }
            return res;
        }
        private List<Cell> LeftUpSearch (IPawn root_pawn, Cell cell) {
            var res = new List<Cell> ();
            for (var i = 0; i < board.GetWidth (); i++) {

                if (cell != null) {
                    if (cell.GetCheckedLeftUp ()) break;

                    if (cell.guestPawn == null && cell.DownRightCell.guestPawn != null) {
                        cell.SetSelected (true);
                        cell.SetCheckedLeftUp (true);
                        cell.lastSeenPawn = root_pawn;
                        res.Add (cell);
                        cell = cell.UpLeftCell;
                    } else
                    if (cell.UpLeftCell != null) {
                        if (cell.UpLeftCell.guestPawn == null && cell.guestPawn != null) {
                            cell = cell.UpLeftCell;
                        }
                    } else {
                        break;
                    }

                } else {
                    break;
                }
            }
            return res;
        }
        private List<Cell> RightUpSearch (IPawn root_pawn, Cell cell) {
            var res = new List<Cell> ();
            for (var i = 0; i < board.GetWidth (); i++) {

                if (cell != null) {
                    if (cell.GetCheckedRightUp ()) break;

                    if (cell.guestPawn == null && cell.DownLeftCell.guestPawn != null) {
                        cell.SetSelected (true);
                        cell.SetCheckedRightUp (true);
                        cell.lastSeenPawn = root_pawn;
                        res.Add (cell);
                        cell = cell.UpRightCell;
                    } else
                    if (cell.UpRightCell != null) {
                        if (cell.UpRightCell.guestPawn == null && cell.guestPawn != null) {
                            cell = cell.UpRightCell;
                        }
                    } else {
                        break;
                    }

                } else {
                    break;
                }
            }
            return res;
        }
        #endregion
        private void DeselectAll () {
            foreach (var pawn in player2Pawns) {
                pawn.SetSelected (false);
            }
            foreach (var pawn in player1Pawns) {
                pawn.SetSelected (false);
            }
        }

        private Transform CheckOverlap (Vector2 point, int layerMask) {
            Transform res = null;
            var ray = Physics2D.Raycast (point, Vector2.up, 1f, 1 << layerMask);
            var collider = ray.collider;
            if (collider) {
                res = collider.gameObject.transform;
            }

            return res;
        }

        private void OnDestroy () {
            player1Pawns = null;
            player2Pawns = null;
            selectedPawn = null;
        }
    }
}