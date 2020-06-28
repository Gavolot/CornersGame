using UnityEditor;
using UnityEngine;

namespace Game.Corners {

    [CustomEditor (typeof (PawnsCorners))]
    public class PawnsCornersEditor : Editor {
        PawnsCorners _pawnsCorners;
        private void OnEnable () {
            _pawnsCorners = target as PawnsCorners;
        }
        public override void OnInspectorGUI () {

            _pawnsCorners.player_1_tag = EditorGUILayout.TagField("Player_1: ", _pawnsCorners.player_1_tag);
            _pawnsCorners.player_2_tag = EditorGUILayout.TagField("Player_2: ", _pawnsCorners.player_2_tag);
            _pawnsCorners.player_1_marker_tag = EditorGUILayout.TagField("Player_1_Marker: ", _pawnsCorners.player_1_marker_tag);
            _pawnsCorners.player_2_marker_tag = EditorGUILayout.TagField("Player_2_Marker: ", _pawnsCorners.player_2_marker_tag);
            base.OnInspectorGUI ();

        }
    }
}