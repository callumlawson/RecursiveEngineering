using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
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
        public readonly List<Direction> Diretions;

        public SubstanceConnectorState()
        {
            Diretions = new List<Direction>();
        }

        public SubstanceConnectorState(List<Direction> diretions)
        {
            Diretions = diretions;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Connection directions: ");
            foreach (var direction in Diretions)
            {
                stringBuilder.Append(direction + " ");
            }
            return stringBuilder.ToString();
        }
    }
}
