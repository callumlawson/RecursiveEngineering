using System;
using System.Collections.Generic;
using Assets.Scrips.Components;
using Assets.Scrips.Modules;
using Assets.Scrips.Util;

namespace Assets.Scrips.Entities
{
    public class EntityLibrary
    {
        private static EntityLibrary instance;

        public static EntityLibrary Instance
        {
            get { return instance ?? (instance = new EntityLibrary()); }
        }

        private readonly List<List<IState>> entityLibrary = new List<List<IState>>
        {
            new List<IState>
            {
                new NameState("Box"),
                new PhysicalState()
            },
            new List<IState>
            {
                new NameState("Engine"),
                new PhysicalState()
            },
            new List<IState>
            {
                new NameState("VerticalWall"),
                new PhysicalState()
            },
            new List<IState>
            {
                new NameState("HorizontalWall"),
                new PhysicalState()
            },
            new List<IState>
            {
                new NameState("Tank"),
                new PhysicalState()
            },
//            new List<IState>
//            {
//                new NameState("HorizontalPipe"),
//                new SubstanceConnector(new List<Direction> {Direction.Left, Direction.Right})
//            },
//            new List<IState>
//            {
//                new NameState("VerticalPipe"),
//                new SubstanceConnector(new List<Direction> {Direction.Up, Direction.Down})
//            },
//            new List<IState>
//            {
//                new NameState("CrossPipe"),
//                new SubstanceConnector(new List<Direction>
//                {
//                    Direction.Up,
//                    Direction.Down,
//                    Direction.Left,
//                    Direction.Right
//                })
//            },
//            new List<IState>
//            {
//                new NameState("EngineInternals", 0, 0),
//                new SubstanceConnector(new List<Direction> {Direction.Left, Direction.Right}),
//                new EngineState(0)
//            }
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

        public List<IState> GetSelectedModule()
        {
            return entityLibrary[selectedLibraryIndex];
        }

        public List<IState> GetPreviousModule()
        {
            return entityLibrary[ClampToLibraryIndex(selectedLibraryIndex - 1)];
        }

        public List<IState> GetNextModule()
        {
            return entityLibrary[ClampToLibraryIndex(selectedLibraryIndex + 1)];
        }

        public void UpdateModulesFromDisk()
        {
//            var moduleJson = DiskOperations.GetModules();
//            foreach (var module in moduleJson)
//            {
//                AddEntityToLibrary(Module.FromJson(module));
//            }
        }

        private void AddEntityToLibrary(Entity entityToAdd)
        {
            throw new NotImplementedException();
            //            //Foreach support modification while iterating. 
            //            entityLibrary.ForEach(module =>
            //            {
            //                if (entityToAdd.GetState<NameState>().Name == module.GetState<NameState>().Name)
            //                {
            //                    entityLibrary.Remove(module);
            //                }
            //            });
            //            entityLibrary.Add(entityToAdd);
        }

        private int ClampToLibraryIndex(int value)
        {
            if (value >= entityLibrary.Count)
            {
                return 0;
            }
            if (value < 0)
            {
                return entityLibrary.Count - 1;
            }
            return value;
        }
    }
}
