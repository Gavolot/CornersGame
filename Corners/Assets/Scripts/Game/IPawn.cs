using UnityEngine;
namespace Game.Corners {
    public interface IPawn : IInit{

        string GetTag();

        Cell GetCell();

        void SearchCellToGuestMe();

        void MoveTo(Vector2 point);


        bool GetSelected();
        void SetSelected(bool value);

        void UpdateSelectedRect();
    }
}