using UnityEngine;
namespace Game.Corners {
    public interface IMarker : IInit {
        int GetLayer ();
        string GetTag ();
    }
}