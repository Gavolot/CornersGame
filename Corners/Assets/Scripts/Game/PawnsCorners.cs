using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Processor;
using UnityEngine;
namespace Game.Corners {

    public static class Rules {
        public const int Classic = 0;
    }
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

        public int rule = Rules.Classic;

        IBoard board;
        public LayerMask pawnsLayer = -1;
        private void Start () {
            cameraMain = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
            board = FindObjectsOfType<MonoBehaviour> ().OfType<IBoard> ().First();
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
            if (step == player_1_tag) {
                foreach (var pawn in player1Pawns) {
                    pawn.UpdateSelectedRect ();
                }
            } else
            if (step == player_2_tag) {
                foreach (var pawn in player2Pawns) {
                    pawn.UpdateSelectedRect ();
                }
            }

            if (Input.GetMouseButtonDown (0)) {
                var point = cameraMain.ScreenToWorldPoint (mousePos);
                var p = CheckOverlap (point);
                if (p != null) {
                    if (p.GetTag () == step) {
                        DeselectAll ();
                        board.DeselectAll();
                        p.isSelected = true;

                        var parentCell = p.GetCell ();
                        var neighbors = parentCell.neighborsCells;
                        foreach (var n in neighbors) {
                            if (n != null) {
                                if (n.guestPawn == null) {
                                    //n.selectedRect.SetActive(true);

                                    // if (rule == Rules.Classic) {
                                    if (parentCell.Up != null)
                                        if (n.transform.position == parentCell.Up.position) {
                                            n.selectedRect.SetActive (true);
                                        }
                                    if (parentCell.Left != null)
                                        if (n.transform.position == parentCell.Left.position) {
                                            n.selectedRect.SetActive (true);
                                        }
                                    if (parentCell.Right != null)
                                        if (n.transform.position == parentCell.Right.position) {
                                            n.selectedRect.SetActive (true);
                                        }
                                    if (parentCell.Down != null)
                                        if (n.transform.position == parentCell.Down.position) {
                                            n.selectedRect.SetActive (true);
                                        }
                                    // }

                                }
                            }
                        }
                    }
                }
            }
        }

        private void DeselectAll () {
            foreach (var pawn in player2Pawns) {
                pawn.isSelected = false;
            }
            foreach (var pawn in player1Pawns) {
                pawn.isSelected = false;
            }
        }

        private IPawn CheckOverlap (Vector2 point) {
            int mask = Utils.LayerMaskToInt (pawnsLayer);
            IPawn res = null;
            var collider = Physics2D.OverlapBox (point, new Vector2 (0.00003f, 0.00003f), 0, 1 << mask);
            if (collider) {
                res = collider.gameObject.GetComponent<IPawn> ();
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