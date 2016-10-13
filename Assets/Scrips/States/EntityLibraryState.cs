using System.Collections.Generic;
using Assets.Framework.States;

namespace Assets.Scrips.States
{
    public class EntityLibraryState : IState
    {
        public List<List<IState>> EntityLibrary;
        public int SelectedLibraryIndex { get; private set; }

        public EntityLibraryState(List<List<IState>> entityLibrary)
        {
            EntityLibrary = entityLibrary;
            SelectedLibraryIndex = 0;
        }

        public void UpdateSelectedLibraryIndex(int newIndex)
        {
            SelectedLibraryIndex = ClampToLibraryIndex(newIndex);
        }

        public List<IState> GetSelectedEntity()
        {
            return EntityLibrary[SelectedLibraryIndex];
        }

        public List<IState> GetPreviousEntity()
        {
            return EntityLibrary[ClampToLibraryIndex(SelectedLibraryIndex - 1)];
        }

        public List<IState> GetNextEntity()
        {
            return EntityLibrary[ClampToLibraryIndex(SelectedLibraryIndex + 1)];
        }

        private int ClampToLibraryIndex(int value)
        {
            if (value >= EntityLibrary.Count)
            {
                return 0;
            }
            if (value < 0)
            {
                return EntityLibrary.Count - 1;
            }
            return value;
        }
    }
}
