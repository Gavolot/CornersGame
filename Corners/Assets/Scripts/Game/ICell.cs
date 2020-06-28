namespace Game.Corners {
    public interface ICell : IInit
    {
        void UpdateSelected();
        void SetSelected(bool value);
        bool GetSelected();

        void ResetNeighbors ();

        bool GetCheckedUp();
        bool GetCheckedRightUp();
        bool GetCheckedLeftUp();
        bool GetCheckedDown();
        bool GetCheckedRightDown();
        bool GetCheckedLeftDown();
        bool GetCheckedLeft();
        bool GetCheckedRight();
        void SetCheckedUp(bool value);
        void SetCheckedRightUp(bool value);
        void SetCheckedLeftUp(bool value);
        void SetCheckedDown(bool value);
        void SetCheckedRightDown(bool value);
        void SetCheckedLeftDown(bool value);
        void SetCheckedLeft(bool value);
        void SetCheckedRight(bool value);

        void SetHumanCoordinate(string alphabet, int number);
        string GetHumanAlphaberCoordinate();
        int GetHumanNumberCoordinate();

    }
}