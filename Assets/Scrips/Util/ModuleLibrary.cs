using System.Collections.Generic;
using Assets.Scrips.Components;
using Assets.Scrips.Modules;

namespace Assets.Scrips.Util
{
    public class ModuleLibrary
    {
        private readonly List<List<IComponent>> componentLibrary = new List<List<IComponent>>
        {
            new List<IComponent> { new CoreComponent("Box", 5, 5, ModuleType.Container)},
            new List<IComponent> { new CoreComponent("Tank", 0, 0, ModuleType.WaterTank)},
            new List<IComponent> { new CoreComponent("HorizontalPipe", 0, 0, ModuleType.NotNeeded), new SubstanceConnector(new List<Direction> {Direction.Left, Direction.Right})},
            new List<IComponent> { new CoreComponent("VerticalPipe", 0, 0, ModuleType.NotNeeded), new SubstanceConnector(new List<Direction> {Direction.Up, Direction.Down})},
            new List<IComponent> { new CoreComponent("CrossPipe", 0, 0, ModuleType.NotNeeded), new SubstanceConnector(new List<Direction> {Direction.Up, Direction.Down, Direction.Left, Direction.Right})}
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

        public List<IComponent> GetSelectedComponent()
        {
            return componentLibrary[selectedLibraryIndex];
        }

        public List<IComponent> GetPreviousComponent()
        {
            return componentLibrary[ClampToLibraryIndex(selectedLibraryIndex - 1)];
        }

        public List<IComponent> GetNextComponent()
        {
            return componentLibrary[ClampToLibraryIndex(selectedLibraryIndex + 1)];
        }

        public T GetComponent<T>(List<IComponent> components) where T : IComponent
        {
            foreach (var component in components)
            {
                if (component.GetType() == typeof(T))
                {
                    return component as T;
                }
            }
            return null;
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
