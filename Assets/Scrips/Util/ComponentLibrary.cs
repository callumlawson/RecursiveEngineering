using System.Collections.Generic;
using Assets.Scrips.Components;

namespace Assets.Scrips.Util
{
    public class ComponentLibrary
    {
        private readonly List<ComponentData> componentLibrary = new List<ComponentData>
        {
            new ComponentData { Name = "Box", InternalWidth = 5, InteralHeight = 5, WaterContainer = false },
            new ComponentData { Name = "Tank", InternalWidth = 2, InteralHeight = 2, WaterContainer = true }
        };

        private int selectedLibraryIndex;

        public void IncrementSelectedComponent()
        {
            selectedLibraryIndex = ClampToLibraryIndex(selectedLibraryIndex + 1);
        }

        public void DecrementSelectedComponent()
        {
            selectedLibraryIndex = ClampToLibraryIndex(selectedLibraryIndex - 1);
        }

        public ComponentData GetSelectedComponent()
        {
            return componentLibrary[selectedLibraryIndex];
        }

        public ComponentData GetPreviousComponent()
        {
            return componentLibrary[ClampToLibraryIndex(selectedLibraryIndex - 1)];
        }

        public ComponentData GetNextComponent()
        {
            return componentLibrary[ClampToLibraryIndex(selectedLibraryIndex + 1)];
        }

        private int ClampToLibraryIndex(int value)
        {
            if (value >= componentLibrary.Count)
            {
                return 0;
            }
            if (value < 0)
            {
                return componentLibrary.Count - 1;
            }
            return value;
        }
    }
}
