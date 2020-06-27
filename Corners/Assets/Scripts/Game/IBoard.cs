namespace Game.Corners {
    public interface IBoard{
        void ClearBoard ();
        void Init ();

        void ResetBoard();

        void DeselectAll();
    }
}