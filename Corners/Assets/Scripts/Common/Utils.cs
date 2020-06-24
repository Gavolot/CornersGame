using System.Linq;
using UnityEngine;
namespace Processor {

    public static class Utils {
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
    }
}