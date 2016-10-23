using System.Collections.Generic;
using Assets.Framework.States;
using Assets.Scrips.States;

namespace Assets.Scrips.Util
{
    public static class InitialBuildableEntities
    {
        public static readonly List<IState> Environment = new List<IState>
        {
            new EntityTypeState("Environment"),
            new PhysicalState(0, 0, false),
            new SubstanceConnectorState(new List<Direction>
            {
                Direction.Left,
                Direction.Right,
                Direction.Up,
                Direction.Down
            }),
            new SubstanceNetworkState()
        };

        public static readonly List<List<IState>> BuildableEntityLibrary = new List<List<IState>>
        {
            new List<IState>
            {
                new EntityTypeState("Box"),
                new PhysicalState()
            },
            new List<IState>
            {
                new EntityTypeState("Crew"),
                new NameState("Jess"),
                new HealthState(100.0f),
                new PhysicalState(),
                new CrewState()
            },
            new List<IState>
            {
                new EntityTypeState("Engine"),
                new PhysicalState()
            },
            new List<IState>
            {
                new EntityTypeState("VerticalWall"),
                new PhysicalState()
            },
            new List<IState>
            {
                new EntityTypeState("HorizontalWall"),
                new PhysicalState()
            },
            new List<IState>
            {
                new EntityTypeState("Tank"),
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
                new EntityTypeState("HorizontalPipe"),
                new PhysicalState(),
                new SubstanceConnectorState(new List<Direction> {Direction.Left, Direction.Right}),
                new SubstanceNetworkState()
            },
            new List<IState>
            {
                new EntityTypeState("VerticalPipe"),
                new PhysicalState(),
                new SubstanceConnectorState(new List<Direction> {Direction.Up, Direction.Down}),
                new SubstanceNetworkState()
            },
            new List<IState>
            {
                new EntityTypeState("CrossPipe"),
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
                new EntityTypeState("EngineInternals"),
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
            }
        };
    }
}
