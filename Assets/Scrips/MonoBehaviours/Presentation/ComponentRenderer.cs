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

        private GameObject OuterRendererRoot;
        private GameObject InnerRendererRoot;
        private GameObject selectedGridIndicator;

        private EngiComponent LastRenderedComponent;

        [UsedImplicitly]
        public void Start()
        {
            selectedGridIndicator = Instantiate(Selector);
            selectedGridIndicator.transform.parent = transform;

            OuterRendererRoot = new GameObject();
            if (OuterRendererRoot != null)
            {
                OuterRendererRoot.name = "OuterComponent";
                OuterRendererRoot.transform.parent = transform;
            }

            InnerRendererRoot = new GameObject();
            if (InnerRendererRoot != null)
            {
                InnerRendererRoot.name = "InnerComponent";
                InnerRendererRoot.transform.parent = transform;
            }

            LastRenderedComponent = new EngiComponent();
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

        public void RenderComponent(EngiComponent outerComponent)
        {
            RenderOuterComponent(outerComponent);
            RenderInnerComponents(outerComponent);
            LastRenderedComponent = outerComponent;
        }

        private void RenderInnerComponents(EngiComponent outerComponent)
        {
            foreach (Transform child in InnerRendererRoot.transform)
            {
                Destroy(child.gameObject);
            }

            for (var x = 0; x < outerComponent.InnerComponents.GetLength(0); x++)
            {
                for (var y = 0; y < outerComponent.InnerComponents.GetLength(1); y++)
                {
                    var innerComponent = outerComponent.InnerComponents[x, y];
                    if (innerComponent != null)
                    {
                        var component = outerComponent.InnerComponents[x, y];
                        var componentAsset = Resources.Load<GameObject>(component.Name);
                        var componentGameObject = Instantiate(componentAsset);
                        componentGameObject.transform.parent = InnerRendererRoot.transform;
                        componentGameObject.transform.position = GridToPosition(x, y);

                    }
                }
            }
        }

        private void RenderOuterComponent(EngiComponent component)
        {
            if (component.Width == LastRenderedComponent.Width && component.Height == LastRenderedComponent.Height)
            {
                return;
            }
                
            foreach (Transform child in OuterRendererRoot.transform)
            {
                Destroy(child.gameObject);
            }

            for (var x = 0; x < component.Width; x++)
            {
                for (var y = 0; y < component.Height; y++)
                {
                    var tile = Instantiate(Tile);
                    tile.transform.parent = OuterRendererRoot.transform;
                    tile.transform.position = GridToPosition(x, y);
                }
            }
        }

        private static Vector3 GridToPosition(int x, int y)
        {
            return new Vector3(x * LayoutConstants.TileSizeInMeters, y * LayoutConstants.TileSizeInMeters);
        }
    }
}
