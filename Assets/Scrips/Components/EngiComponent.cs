using System.Collections.Generic;
using Assets.Scrips.Networks;
using Assets.Scrips.Util;
using UnityEngine;

namespace Assets.Scrips.Components
{
    public struct ComponentData
    {
        public string Name;
        public int InternalWidth;
        public int InteralHeight;
        public bool WaterContainer;
    }

    public class EngiComponent
    {
        public readonly ComponentGrid ComponentGrid;
        public readonly int InteralHeight;
        public readonly int InternalWidth;
        public readonly string Name;
        public readonly EngiComponent ParentComponent;

        private readonly SubstanceNetwork substanceNetwork;
        private readonly bool waterContainer;
        private List<SubstanceConnection> SubstanceConnections;

        public EngiComponent()
        {
        }

        public EngiComponent(
            ComponentData componentData,
            EngiComponent parentComponent, 
            SubstanceNetwork substanceNetwork, 
            List<SubstanceConnection> substanceConnections = null)
        {
            Name = componentData.Name;
            ParentComponent = parentComponent;
            InternalWidth = componentData.InternalWidth;
            InteralHeight = componentData.InteralHeight;
            this.substanceNetwork = substanceNetwork;
            SubstanceConnections = substanceConnections;
            ComponentGrid = new ComponentGrid(InternalWidth, InteralHeight);

            //TODO: Factor out of this class
            waterContainer = componentData.WaterContainer;
            if (waterContainer)
            {
                this.substanceNetwork.AddNode(new SubstanceNetworkNode(this));
            }
        }

        public bool AddComponent(EngiComponent component, GridCoordinate grid)
        {
            var addSucsess = ComponentGrid.AddComponent(component, grid);
            if (addSucsess)
            {
                UpdateSubstanceNetwork(component, grid);
            }
            return addSucsess;
        }

        private void UpdateSubstanceNetwork(EngiComponent component, GridCoordinate grid)
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