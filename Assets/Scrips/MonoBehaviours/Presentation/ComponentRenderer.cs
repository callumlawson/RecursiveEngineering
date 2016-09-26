using System;
using Assets.Scrips.Components;
using Assets.Scrips.Util;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scrips.MonoBehaviours.Presentation
{
    public class ComponentRenderer : MonoBehaviour
    {
        [UsedImplicitly] public GameObject Tile;
        [UsedImplicitly] public GameObject Selector;

        private GameObject outerRendererRoot;
        private GameObject innerRendererRoot;
        private GameObject selectedGridIndicator;

        private EngiComponent lastRenderedComponent;

        [UsedImplicitly]
        public void Start()
        {
            selectedGridIndicator = Instantiate(Selector);
            selectedGridIndicator.transform.parent = transform;

            outerRendererRoot = new GameObject();
            if (outerRendererRoot != null)
            {
                outerRendererRoot.name = "OuterComponent";
                outerRendererRoot.transform.parent = transform;
            }

            innerRendererRoot = new GameObject();
            if (innerRendererRoot != null)
            {
                innerRendererRoot.name = "InnerComponent";
                innerRendererRoot.transform.parent = transform;
            }

            lastRenderedComponent = new EngiComponent();
        }

        [UsedImplicitly]
        public void Update()
        {
            var mousePosition = CameraController.ActiveCamera.ScreenToWorldPoint(Input.mousePosition);
            var gridx = Mathf.Round(mousePosition.x / LayoutConstants.TileSizeInMeters) * LayoutConstants.TileSizeInMeters;
            var gridy = Mathf.Round(mousePosition.y / LayoutConstants.TileSizeInMeters) * LayoutConstants.TileSizeInMeters;
            selectedGridIndicator.transform.position = new Vector3(gridx, gridy, 0);
        }

        public static GridCoordinate CurrentlySelectedGrid()
        {
            var mousePosition = CameraController.ActiveCamera.ScreenToWorldPoint(Input.mousePosition);
            var gridx = Mathf.RoundToInt(mousePosition.x / LayoutConstants.TileSizeInMeters);
            var gridy = Mathf.RoundToInt(mousePosition.y / LayoutConstants.TileSizeInMeters);
            return new GridCoordinate(gridx, gridy);
        }

        public void Render(EngiComponent outerComponent)
        {
            RenderOuterComponent(outerComponent);
            RenderInnerComponents(outerComponent);
            lastRenderedComponent = outerComponent;
        }

        private void RenderInnerComponents(EngiComponent outerComponent)
        {
            foreach (Transform child in innerRendererRoot.transform)
            {
                Destroy(child.gameObject);
            }

            for (var x = 0; x < outerComponent.ComponentGrid.Width; x++)
            {
                for (var y = 0; y < outerComponent.ComponentGrid.Height; y++)
                {
                    var grid = new GridCoordinate(x, y);
                    var innerComponent = outerComponent.GetComponent(grid);
                    if (innerComponent != null)
                    {
                        var component = outerComponent.GetComponent(grid);
                        var componentAsset = Resources.Load<GameObject>(component.Name);
                        var componentGameObject = Instantiate(componentAsset);
                        componentGameObject.transform.parent = innerRendererRoot.transform;
                        componentGameObject.transform.position = GridCoordinate.GridToPosition(grid);

                    }
                }
            }
        }

        private void RenderOuterComponent(EngiComponent component)
        {
            if (component.InternalWidth == lastRenderedComponent.InternalWidth && component.InteralHeight == lastRenderedComponent.InteralHeight)
            {
                return;
            }
                
            foreach (Transform child in outerRendererRoot.transform)
            {
                Destroy(child.gameObject);
            }

            for (var x = 0; x < component.InternalWidth; x++)
            {
                for (var y = 0; y < component.InteralHeight; y++)
                {
                    var grid = new GridCoordinate(x, y);
                    var tile = Instantiate(Tile);
                    tile.transform.parent = outerRendererRoot.transform;
                    tile.transform.position = GridCoordinate.GridToPosition(grid);
                }
            }
        }
    }
}
