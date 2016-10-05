using System.Collections.Generic;
using Assets.Scrips.Components;
using Assets.Scrips.Util;

namespace Assets.Scrips.Modules
{
    public class EntityLibrary
    {
        private static EntityLibrary instance;

        public static EntityLibrary Instance
        {
            get { return instance ?? (instance = new EntityLibrary()); }
        }

        private readonly List<Module> moduleLibrary = new List<Module>
        {
            new Module(new List<IState> {new NameState("Box", 7, 7)}),
            new Module(new List<IState> {new NameState("Engine", 7, 7)}),
            new Module(new List<IState> {new NameState("VerticalWall", 0, 0)}),
            new Module(new List<IState> {new NameState("HorizontalWall", 0, 0)}),
            new Module(new List<IState> {new NameState("Tank", 0, 0)}),
            new Module(new List<IState>
            {
                new NameState("HorizontalPipe", 0, 0),
                new SubstanceConnector(new List<Direction> {Direction.Left, Direction.Right})
            }),
            new Module(new List<IState>
            {
                new NameState("VerticalPipe", 0, 0),
                new SubstanceConnector(new List<Direction> {Direction.Up, Direction.Down})
            }),
            new Module(new List<IState>
            {
                new NameState("CrossPipe", 0, 0),
                new SubstanceConnector(new List<Direction>
                {
                    Direction.Up,
                    Direction.Down,
                    Direction.Left,
                    Direction.Right
                })
            }),
            new Module(new List<IState>
            {
                new NameState("EngineInternals", 0, 0),
                new SubstanceConnector(new List<Direction> {Direction.Left, Direction.Right}),
                new EngineState(0)
            })
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

        public Module GetSelectedModule()
        {
            return moduleLibrary[selectedLibraryIndex];
        }

        public Module GetPreviousModule()
        {
            return moduleLibrary[ClampToLibraryIndex(selectedLibraryIndex - 1)];
        }

        public Module GetNextModule()
        {
            return moduleLibrary[ClampToLibraryIndex(selectedLibraryIndex + 1)];
        }

        public void UpdateModulesFromDisk()
        {
            var moduleJson = DiskOperations.GetModules();
            foreach (var module in moduleJson)
            {
                AddModuleToLibrary(Module.FromJson(module));
            }
        }

        private void AddModuleToLibrary(Module moduleToAdd)
        {
            //Foreach support modification while iterating. 
            moduleLibrary.ForEach(module =>
            {
                if (moduleToAdd.GetState<NameState>().Name == module.GetState<NameState>().Name)
                {
                    moduleLibrary.Remove(module);
                }
            });
            moduleLibrary.Add(moduleToAdd);
        }

        private int ClampToLibraryIndex(int value)
        {
            if (value >= moduleLibrary.Count)
            {
                return 0;
            }
            if (value < 0)
            {
                return moduleLibrary.Count - 1;
            }
            return value;
        }
    }
}