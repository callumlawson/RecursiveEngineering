using System;
using System.Collections.Generic;
using Assets.Framework.States;

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

        public SubstanceConnectorState()
        {
            Diretions = new List<Direction>();
        }

        public SubstanceConnectorState(List<Direction> diretions)
        {
            Diretions = diretions;
        }
    }
}
