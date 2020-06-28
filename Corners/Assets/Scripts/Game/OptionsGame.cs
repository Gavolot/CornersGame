using System.Collections;
using Game;
using UnityEngine;
using UnityEngine.UI;
namespace Game.Corners {
    public enum ERules {
        Classic,
        NoJumpsFourDirs,
        NoJumpsAllDirs,
        NoJumpsDiagonals,
        Diagonals
    }
    public class OptionsGame : MonoBehaviour {

        public GameObject optionsContainer;
        public GameObject uiGameContainer;
        public Dropdown rulesMenu;
        public Button buttonStartGame;

        public Button buttonRestartGame;

        public Button buttonExitGame;

        public PawnsCorners pawnsCorners;
        public Board board;
        private ERules rules;

        [SerializeField]
        private GameObject prefabPlayer1Pawn = null;
        [SerializeField]
        private GameObject prefabPlayer2Pawn = null;
        [SerializeField]
        private GameObject prefabPlayer1Marker = null;
        [SerializeField]
        private GameObject prefabPlayer2Marker = null;
        [SerializeField]
        private GameObject uiGameWinPlayer_1 = null;

        [SerializeField]
        private GameObject uiGameWinPlayer_2 = null;

        private void Start () {
            rules = ERules.Classic;
            rulesMenu.onValueChanged.AddListener (delegate {
                RulesMenuUpdate (rulesMenu);
            });
            buttonStartGame.onClick.AddListener (delegate {
                ButtonStartGameClick ();
            });

            buttonRestartGame.onClick.AddListener (delegate {
                Restart ();
            });
            buttonExitGame.onClick.AddListener (delegate {
                ButtonExitGame ();
            });
        }

        private void ButtonExitGame () {
            board.ResetBoard ();
            pawnsCorners.ClearAll ();
            optionsContainer.SetActive (true);
            uiGameContainer.SetActive (false);
            uiGameWinPlayer_1.SetActive (false);
            uiGameWinPlayer_2.SetActive (false);
        }

        private void ButtonStartGameClick () {
            optionsContainer.SetActive (false);
            InitPawns ();
            StartCoroutine (CornersInit ());
            uiGameContainer.SetActive (true);
        }

        private void InitPawns () {
            for (var x = 0; x < 3; x++) {
                for (var y = 0; y < 3; y++) {
                    var cell = board.GetCellFromGrid (x, y);
                    var pawn = Instantiate (prefabPlayer1Pawn, cell.transform.position, Quaternion.identity);
                    var marker = Instantiate (prefabPlayer1Marker, cell.transform.position, Quaternion.identity);
                }
            }
            for (var x = 5; x < 8; x++) {
                for (var y = 5; y < 8; y++) {
                    var cell = board.GetCellFromGrid (x, y);
                    var pawn = Instantiate (prefabPlayer2Pawn, cell.transform.position, Quaternion.identity);
                    var marker = Instantiate (prefabPlayer2Marker, cell.transform.position, Quaternion.identity);
                }
            }
        }

        public void Restart () {
            board.ResetBoard ();
            pawnsCorners.ClearAll ();
            ButtonStartGameClick ();
            uiGameWinPlayer_1.SetActive (false);
            uiGameWinPlayer_2.SetActive (false);
        }

        IEnumerator CornersInit () {
            yield return new WaitForEndOfFrame ();
            pawnsCorners.Init ();
        }

        private void RulesMenuUpdate (Dropdown mDropdown) {
            rules = (ERules) mDropdown.value;
            pawnsCorners.rules = rules;
        }
    }
}