namespace Game.Corners {
    public interface IPawn {
        void Init();

        string GetTag();

        Cell GetCell();

        bool isSelected {
            get;
            set;
        }

        void UpdateSelectedRect();
    }
}