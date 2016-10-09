using System;
using System.Collections.Generic;
using Assets.Scrips.States;

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
                new PhysicalState(),
                new SubstanceNetworkState(),
                new SubstanceConnectorState(new List<Direction> {Direction.Left, Direction.Right, Direction.Up, Direction.Down}),
            },
            new List<IState>
            {
                new NameState("HorizontalPipe"),
                new PhysicalState(),
                new SubstanceConnectorState(new List<Direction> {Direction.Left, Direction.Right}),
                new SubstanceNetworkState()
            },
            new List<IState>
            {
                new NameState("VerticalPipe"),
                new PhysicalState(),
                new SubstanceConnectorState(new List<Direction> {Direction.Up, Direction.Down}),
                new SubstanceNetworkState()
            },
            new List<IState>
            {
                new NameState("CrossPipe"),
                new PhysicalState(),
                new SubstanceConnectorState(new List<Direction>
                {
                    Direction.Up,
                    Direction.Down,
                    Direction.Left,
                    Direction.Right
                }),
                new SubstanceNetworkState()
            },
            new List<IState>
            {
                new NameState("EngineInternals"),
                new PhysicalState(),
                new SubstanceConnectorState(new List<Direction> {Direction.Left, Direction.Right, Direction.Up, Direction.Down}),
                new EngineState(0),
                new SubstanceNetworkState()
            },
            new List<IState>
            {
                new NameState("Environment"),
                new PhysicalState(0, 0),
                new SubstanceConnectorState(new List<Direction> {Direction.Left, Direction.Right, Direction.Up, Direction.Down}),
                new SubstanceNetworkState()
            }
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

        public static T GetState<T>(List<IState> states) where T : IState
        {
            foreach (var state in states)
            {
                if (state.GetType() == typeof(T))
                {
                    return (T)state;
                }
            }
            return default(T);
        }

        public void UpdateModulesFromDisk()
        {
//            var moduleJson = DiskOperations.GetModules();
//            foreach (var module in moduleJson)
//            {
//                AddEntityToLibrary(Entity.FromJson(module));
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
