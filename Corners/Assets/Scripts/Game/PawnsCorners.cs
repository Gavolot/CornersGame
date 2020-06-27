using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Processor;
using UnityEngine;
namespace Game.Corners {

    public enum ERules {
        Classic,
        NoJumpsFourDirs,
        NoJumpsAllDirs
    }

    public enum EDirs4 {
        Up,
        Left,
        Right,
        Down,
        None
    }

    // public static class Rules {
    //     public const int Classic = 0;
    //     public const int NoJumpsFourDirs = 1;
    //     public const int NoJumpsAllDirs = 2;
    // }
    public class PawnsCorners : MonoBehaviour {
        [HideInInspector]
        public string player_1_tag;
        [HideInInspector]
        public string player_2_tag;
        private IPawn selectedPawn = null;
        private List<IPawn> player1Pawns;
        private List<IPawn> player2Pawns;
        private Camera cameraMain;
        private string step;

        private int attemptsJumping = 0;

        //public int rule = Rules.Classic;

        public ERules rules = ERules.Classic;

        IBoard board;
        public LayerMask pawnsLayer = -1;
        public LayerMask cellsLayer = -1;
        private void Start () {
            cameraMain = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
            board = FindObjectsOfType<MonoBehaviour> ().OfType<IBoard> ().First ();
            board.Init ();
            player1Pawns = new List<IPawn> ();
            player2Pawns = new List<IPawn> ();
            step = player_1_tag;
            StartCoroutine (PawnsInit ());
        }
        IEnumerator PawnsInit () {
            //lazy timer
            yield return new WaitForEndOfFrame ();
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

            foreach (var pawn in player1Pawns) {
                pawn.UpdateSelectedRect ();
            }
            foreach (var pawn in player2Pawns) {
                pawn.UpdateSelectedRect ();
            }

            if (Input.GetMouseButtonDown (0)) {
                var point = cameraMain.ScreenToWorldPoint (mousePos);
                Transform t;

                int maskCells = Utils.LayerMaskToInt (cellsLayer);
                t = CheckOverlap (point, maskCells);
                if (t != null) {

                    var cell = t.gameObject.GetComponent<Cell> ();

                    if (cell != null) {
                        if (cell.lastSeenPawn != null) {
                            if (cell.lastSeenPawn.isSelected) {
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
                                // if (selectedPawn.Equals (cell.lastSeenPawn)) {
                                //     selectedPawn.MoveTo (t.position);
                                //     selectedPawn = null;
                                //     DeselectAll ();
                                //     board.DeselectAll ();
                                //     board.LastSeenClearAll ();
                                // }
                            }
                        }

                    }
                }

                int maskPawns = Utils.LayerMaskToInt (pawnsLayer);
                t = CheckOverlap (point, maskPawns);
                if (t != null) {

                    var pawn = t.gameObject.GetComponent<IPawn> ();
                    //var pawn = CheckOverlapPawn (point, mask);
                    if (pawn != null) {
                        if (pawn.GetTag () == step) {
                            DeselectAll ();
                            board.DeselectAll ();
                            board.UnCheckedAll ();
                            //board.LastSeenClearAll ();
                            pawn.isSelected = true;
                            selectedPawn = pawn;

                            var parentCell = selectedPawn.GetCell ();

                            if (rules == ERules.Classic) {
                                var neighbors = parentCell.neighborsCells;
                                foreach (var n in neighbors) {
                                    if (n != null) {
                                        if (n.guestPawn == null) {
                                            if (parentCell.Up != null)
                                                if (n.transform.position == parentCell.Up.position) {
                                                    n.selectedRect.SetActive (true);
                                                    n.lastSeenPawn = parentCell.guestPawn;
                                                }
                                            if (parentCell.Left != null)
                                                if (n.transform.position == parentCell.Left.position) {
                                                    n.selectedRect.SetActive (true);
                                                    n.lastSeenPawn = parentCell.guestPawn;
                                                }
                                            if (parentCell.Right != null)
                                                if (n.transform.position == parentCell.Right.position) {
                                                    n.selectedRect.SetActive (true);
                                                    n.lastSeenPawn = parentCell.guestPawn;
                                                }
                                            if (parentCell.Down != null)
                                                if (n.transform.position == parentCell.Down.position) {
                                                    n.selectedRect.SetActive (true);
                                                    n.lastSeenPawn = parentCell.guestPawn;
                                                }

                                        }
                                    }
                                }
                                JumpPawnSearchClassic (pawn, parentCell);
                            } else
                            if (rules == ERules.NoJumpsFourDirs || rules == ERules.NoJumpsAllDirs) {
                                var neighbors = parentCell.neighborsCells;
                                foreach (var n in neighbors) {
                                    if (n != null) {
                                        if (n.guestPawn == null) {
                                            //n.selectedRect.SetActive(true);

                                            if (rules == ERules.NoJumpsFourDirs) {
                                                if (parentCell.Up != null)
                                                    if (n.transform.position == parentCell.Up.position) {
                                                        n.selectedRect.SetActive (true);
                                                        n.lastSeenPawn = parentCell.guestPawn;
                                                    }
                                                if (parentCell.Left != null)
                                                    if (n.transform.position == parentCell.Left.position) {
                                                        n.selectedRect.SetActive (true);
                                                        n.lastSeenPawn = parentCell.guestPawn;
                                                    }
                                                if (parentCell.Right != null)
                                                    if (n.transform.position == parentCell.Right.position) {
                                                        n.selectedRect.SetActive (true);
                                                        n.lastSeenPawn = parentCell.guestPawn;
                                                    }
                                                if (parentCell.Down != null)
                                                    if (n.transform.position == parentCell.Down.position) {
                                                        n.selectedRect.SetActive (true);
                                                        n.lastSeenPawn = parentCell.guestPawn;
                                                    }
                                            } else if (rules == ERules.NoJumpsAllDirs) {
                                                n.selectedRect.SetActive (true);
                                                n.lastSeenPawn = parentCell.guestPawn;
                                            }

                                        }
                                    }
                                }
                            }

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
        private List<Cell> LeftSearch (IPawn root_pawn, Cell cell) {
            var res = new List<Cell> ();
            for (var i = 0; i < board.GetWidth (); i++) {

                if (cell != null) {
                    if (cell.isCheckedLeft) break;

                    if (cell.guestPawn == null && cell.RightCell.guestPawn != null) {
                        cell.selectedRect.SetActive (true);
                        cell.isCheckedLeft = true;
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
                    if (cell.isCheckedRight) break;

                    if (cell.guestPawn == null && cell.LeftCell.guestPawn != null) {
                        cell.selectedRect.SetActive (true);
                        cell.isCheckedRight = true;
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
                    if (cell.isCheckedUp) break;

                    if (cell.guestPawn == null && cell.DownCell.guestPawn != null) {
                        cell.selectedRect.SetActive (true);
                        cell.isCheckedUp = true;
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
                    if (cell.isCheckedDown) break;

                    if (cell.guestPawn == null && cell.UpCell.guestPawn != null) {
                        cell.selectedRect.SetActive (true);
                        cell.lastSeenPawn = root_pawn;
                        cell.isCheckedDown = true;
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

        private void DeselectAll () {
            foreach (var pawn in player2Pawns) {
                pawn.isSelected = false;
            }
            foreach (var pawn in player1Pawns) {
                pawn.isSelected = false;
            }
        }

        private Transform CheckOverlap (Vector2 point, int layerMask) {

            Transform res = null;
            //var collider = Physics2D.OverlapBox (point, new Vector2 (0.00003f, 0.00003f), 0, 1 << layerMask);
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