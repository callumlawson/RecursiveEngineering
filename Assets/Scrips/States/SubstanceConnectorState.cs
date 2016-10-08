using System;
using System.Collections.Generic;

namespace Assets.Scrips.States
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        None
    }

    [Serializable]
    public class SubstanceConnectorState : IState
    {
        public List<Direction> Diretions;

        public SubstanceConnectorState(List<Direction> diretions)
        {
            Diretions = diretions;
        }
    }
}
