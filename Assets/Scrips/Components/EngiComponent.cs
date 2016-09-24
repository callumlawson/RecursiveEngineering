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

        public readonly SubstanceNetwork globalSubstanceNetwork;

        public bool WaterContainer;

        public EngiComponent()
        {
        }

        public EngiComponent(string name, EngiComponent parentComponent, int internalWidth, int interalHeight,
            bool waterContainer, SubstanceNetwork globalSubstanceNetwork)
        {
            Name = name;
            ParentComponent = parentComponent;
            InternalWidth = internalWidth;
            InteralHeight = interalHeight;
            this.globalSubstanceNetwork = globalSubstanceNetwork;
            ComponentGrid = new ComponentGrid(InternalWidth, InteralHeight);

            //TODO: Factor out of this class
            WaterContainer = waterContainer;
            if (waterContainer)
            {
                this.globalSubstanceNetwork.AddNode(new SubstanceNetworkNode());
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
                        globalSubstanceNetwork.AddBidirectionalConnection(
                            component.globalSubstanceNetwork.InterfaceNode,
                            neigbour.globalSubstanceNetwork.InterfaceNode
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