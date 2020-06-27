using System.Linq;
using UnityEngine;
namespace Processor {

    public static class Utils {

        public static int LayerMaskToInt(LayerMask mask){
            return Mathf.RoundToInt(Mathf.Log(mask, 2));
        }
        public static Transform[] UnParentChilds (this Transform me) {
            var res = new Transform[me.childCount];
            var i = 0;
#if UNITY_EDITOR
            var tempList = me.Cast<Transform> ().ToList ();
            foreach (var c in tempList) {
                c.parent = null;
                res[i++] = c;
            }
#else
            foreach (Transform c in me) {
                c.parent = null;
                res[i++] = c;
            }
#endif
            return res;
        }

        #region getMiddlePosition

        public static Vector3 GetMiddlePositionChilds (this Transform me) {
            var sumPositions = Vector3.zero;
            var len = 0;
            #if UNITY_EDITOR
            var tempList = me.Cast<Transform> ().ToList ();
            foreach (var child in tempList) {
                sumPositions += child.position;
                len++;
            }
            #else
            foreach (Transform child in me) {
                sumPositions += child.position;
                len++;
            }
            #endif
            sumPositions /= len;
            return sumPositions;
        }

        public static Vector3 GetMiddlePositionTransforms (GameObject[] gameObjects) {
            var sumPositions = Vector3.zero;
            for (var i = 0; i < gameObjects.Length; i++) {
                var child = gameObjects[i].transform;
                sumPositions += child.position;
            }

            sumPositions /= gameObjects.Length;

            return sumPositions;
        }
        public static Vector3 GetMiddlePositionTransforms (Transform[] transforms) {
            var sumPositions = Vector3.zero;
            for (var i = 0; i < transforms.Length; i++) {
                var child = transforms[i];
                sumPositions += child.position;
            }
            sumPositions /= transforms.Length;

            return sumPositions;
        }
        #endregion

        public static void CenteredToChilds (Transform me) {
            var sumPositions = new Vector3 (0f, 0f, 0f);
            Transform[] tempChilds = new Transform[me.childCount];
            var i = 0;
            var childCount = me.childCount;
#if UNITY_EDITOR
            var tempList = me.Cast<Transform> ().ToList ();
            foreach (var child in tempList) {
                sumPositions += child.transform.position;
                tempChilds[i] = child;
                child.parent = null;
                i++;
            }
#else
            foreach (Transform child in transform) {
                sumPositions += child.position;
                child.parent = null;
                tempChilds[i] = child;
                i++;
            }
#endif
            sumPositions /= childCount;
            me.position = sumPositions;
            for (var j = 0; j < tempChilds.Length; j++) {
                tempChilds[j].SetParent (me);
            }

        }
    }
}