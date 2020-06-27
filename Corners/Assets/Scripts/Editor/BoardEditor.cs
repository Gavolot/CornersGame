using System;
using Game.Corners;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Processor;
namespace Game.Corners {
    [CustomEditor (typeof (Board))]
    public class BoardEditor : Editor {
        private Board _board = null;
        private void OnEnable () {
            _board = target as Board;
        }
        public override void OnInspectorGUI () {

            while (_board.horizontalCells % 2 == 1) {
                _board.horizontalCells--;
                if (_board.horizontalCells <= 1) {
                    _board.horizontalCells = 2;
                }

                _board.ResetBoard ();
            }
            if (_board.verticalCells % 2 == 1) {
                _board.verticalCells--;
                if (_board.verticalCells <= 1) {
                    _board.verticalCells = 2;
                }

                _board.ResetBoard ();
            }

            EditorGUI.BeginChangeCheck ();

            var floatFieldsWidth = 24f;

            EditorGUILayout.BeginHorizontal ();
            _board.horizontalCells = LayoutIntField ("Horizontal Cells: ", _board.horizontalCells, floatFieldsWidth);
            _board.verticalCells = LayoutIntField ("Vertical Cells: ", _board.verticalCells, floatFieldsWidth);
            EditorGUILayout.EndHorizontal ();

            _board.whiteCellSprite = EditorGUILayout.ObjectField ("White Cell Sprite: ", _board.whiteCellSprite, typeof (Sprite), true) as Sprite;
            _board.blackCellSprite = EditorGUILayout.ObjectField ("Black Cell Sprite: ", _board.blackCellSprite, typeof (Sprite), true) as Sprite;

            if (EditorGUI.EndChangeCheck () || GUILayout.Button ("Reset Cells")) {
                _board.ResetBoard ();
            }

            if (GUILayout.Button ("Clear Board")) {
                _board.ClearBoard ();
            }

            // if(GUILayout.Button("CenteredToChilds")){
            //     _board.CenteredToChilds();
            // }
            // if(GUILayout.Button("Unchild")){
            //     _board.transform.UnParentChilds();
            // }

            base.OnInspectorGUI ();

        }

        private int LayoutIntField (string label, int value, float width) {
            var res = value;
            EditorGUILayout.LabelField (label, GUILayout.Width (label.Length * 6));
            res = EditorGUILayout.IntField (value, GUILayout.Width (width));
            return res;
        }
    }
}