using UnityEngine;
namespace Game.Corners {
    public interface IPawn {
        void Init();

        string GetTag();

        Cell GetCell();

        void SearchCellToGuestMe();

        void MoveTo(Vector2 point);

        bool isSelected {
            get;
            set;
        }

        void UpdateSelectedRect();
    }
}