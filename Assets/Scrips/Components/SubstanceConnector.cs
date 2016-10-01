using System;
using System.Collections.Generic;

namespace Assets.Scrips.Components
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
    public class SubstanceConnector : IComponent
    {
        public List<Direction> Diretions;

        public SubstanceConnector(List<Direction> diretions)
        {
            Diretions = diretions;
        }
    }
}
