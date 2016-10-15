using System.Collections.Generic;
using Assets.Framework.States;
using Assets.Scrips.States;

namespace Assets.Scrips.Util
{
    public static class InitialBuildableEntities
    {
        public static List<List<IState>> BuildableEntityLibrary = new List<List<IState>>
        {
            new List<IState>
            {
                new NameState("Box"),
                new PhysicalState()
            },
            new List<IState>
            {
                new NameState("Crewman"),
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
                new SubstanceConnectorState(new List<Direction>
                {
                    Direction.Left,
                    Direction.Right,
                    Direction.Up,
                    Direction.Down
                })
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
                new SubstanceConnectorState(new List<Direction>
                {
                    Direction.Left,
                    Direction.Right,
                    Direction.Up,
                    Direction.Down
                }),
                new EngineState(0),
                new SubstanceNetworkState()
            },
            new List<IState>
            {
                new NameState("Environment"),
                new PhysicalState(0, 0),
                new SubstanceConnectorState(new List<Direction>
                {
                    Direction.Left,
                    Direction.Right,
                    Direction.Up,
                    Direction.Down
                }),
                new SubstanceNetworkState()
            }
        };
    }
}
