using UnityEngine;
namespace Game.Corners {
    public interface IMarker : IInit {
        int GetLayer ();

        void Dispose();
        string GetTag ();
    }
}