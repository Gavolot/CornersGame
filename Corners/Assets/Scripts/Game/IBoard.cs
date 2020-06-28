namespace Game.Corners {
    public interface IBoard : IInit{
        void ClearBoard ();

        void ResetBoard();

        void DeselectAll();

        void UnCheckedAll();

        void UpdateSelectedCells();

        int GetWidth();
        int GetHeight();

        //Cell[] GetCells();
    }
}