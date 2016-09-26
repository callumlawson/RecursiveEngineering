using Assets.Scrips.Networks;
using Assets.Scrips.Util;
using UnityEngine;

namespace Assets.Scrips.Components
{
    public class EngiComponent
    {
        public readonly ComponentGrid ComponentGrid;
        public readonly int InteralHeight;
        public readonly int InternalWidth;
        public readonly string Name;

        public readonly EngiComponent ParentComponent;

        private readonly SubstanceNetwork substanceNetwork;
        private readonly bool waterContainer;

        public EngiComponent()
        {
        }

        public EngiComponent(
            string name, 
            EngiComponent parentComponent, 
            int internalWidth, 
            int interalHeight,
            bool waterContainer, 
            SubstanceNetwork substanceNetwork)
        {
            Name = name;
            ParentComponent = parentComponent;
            InternalWidth = internalWidth;
            InteralHeight = interalHeight;
            this.substanceNetwork = substanceNetwork;
            ComponentGrid = new ComponentGrid(InternalWidth, InteralHeight);

            //TODO: Factor out of this class
            this.waterContainer = waterContainer;
            if (waterContainer)
            {
                this.substanceNetwork.AddNode(new SubstanceNetworkNode(this));
            }
        }

        public bool AddComponent(EngiComponent component, GridCoordinate grid)
        {
            //TODO: Extract this logic.s
            if (component.waterContainer)
            {
                foreach (var neigbour in ComponentGrid.GetNeigbouringComponents(grid))
                {
                    if (neigbour.waterContainer)
                    {
                        substanceNetwork.AddBidirectionalConnection(
                            substanceNetwork.GetNodeForComponent(component),
                            substanceNetwork.GetNodeForComponent(neigbour) 
                        );
                    }
                }
            }

            return ComponentGrid.AddComponent(component, grid);
        }

        public EngiComponent GetComponent(GridCoordinate grid)
        {
            return ComponentGrid.GetComponent(grid);
        }

        public GridCoordinate GetGridForComponent(EngiComponent component)
        {
            return ComponentGrid.GetGridForComponent(component);
        }

        public GridCoordinate GetCenterGrid()
        {
            return new GridCoordinate(Mathf.RoundToInt(InternalWidth/2.0f), Mathf.RoundToInt(InteralHeight/2.0f));
        }

        public bool GridIsEmpty(GridCoordinate grid)
        {
            return ComponentGrid.GridIsEmpty(grid);
        }
    }
}