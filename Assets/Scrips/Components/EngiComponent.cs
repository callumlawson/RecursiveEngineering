using Assets.Scrips.Networks;
using Assets.Scrips.Util;
using UnityEngine;

namespace Assets.Scrips.Components
{
    public class EngiComponent
    {
        public ComponentGrid ComponentGrid;
        public int InteralHeight;
        public int InternalWidth;
        public string Name;

        public EngiComponent ParentComponent;

        public readonly SubstanceNetwork substanceNetwork;

        public bool WaterContainer;

        public EngiComponent()
        {
        }

        public EngiComponent(string name, EngiComponent parentComponent, int internalWidth, int interalHeight,
            bool waterContainer, SubstanceNetwork substanceNetwork)
        {
            Name = name;
            ParentComponent = parentComponent;
            InternalWidth = internalWidth;
            InteralHeight = interalHeight;
            this.substanceNetwork = substanceNetwork;
            ComponentGrid = new ComponentGrid(InternalWidth, InteralHeight);

            //TODO: Factor out of this class
            WaterContainer = waterContainer;
            if (waterContainer)
            {
                this.substanceNetwork.AddNode(new SubstanceNetworkNode(this));
            }
        }

        public bool AddComponent(EngiComponent component, GridCoordinate grid)
        {
            //TODO: Extract this logic.s
            if (component.WaterContainer)
            {
                foreach (var neigbour in ComponentGrid.GetNeigbouringComponents(grid))
                {
                    if (neigbour.WaterContainer)
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