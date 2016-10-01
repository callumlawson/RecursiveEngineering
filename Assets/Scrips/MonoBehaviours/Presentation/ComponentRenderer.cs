using System.Linq;
using Assets.Scrips.Components;
using Assets.Scrips.Modules;
using Assets.Scrips.Util;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.VR;

namespace Assets.Scrips.MonoBehaviours.Presentation
{
    public class ComponentRenderer : MonoBehaviour
    {
        [UsedImplicitly] public GameObject Tile;
        [UsedImplicitly] public GameObject Selector;

        private GameObject outerRendererRoot;
        private GameObject innerRendererRoot;
        private GameObject selectedGridIndicator;

        private Module lastRenderedComponent;

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

            lastRenderedComponent = new Module();
        }

        [UsedImplicitly]
        public void Update()
        {
            var mousePosition = CameraController.ActiveCamera.ScreenToWorldPoint(Input.mousePosition);
            var gridx = Mathf.Round(mousePosition.x/LayoutConstants.TileSizeInMeters)*LayoutConstants.TileSizeInMeters;
            var gridy = Mathf.Round(mousePosition.y/LayoutConstants.TileSizeInMeters)*LayoutConstants.TileSizeInMeters;
            selectedGridIndicator.transform.position = new Vector3(gridx, gridy, 0);
        }

        public GridCoordinate CurrentlySelectedGrid()
        {
            var gridOffset = GetGridOffset(lastRenderedComponent);
            var mousePosition = CameraController.ActiveCamera.ScreenToWorldPoint(Input.mousePosition);
            var gridx = Mathf.RoundToInt(mousePosition.x/LayoutConstants.TileSizeInMeters) - gridOffset.X;
            var gridy = Mathf.RoundToInt(mousePosition.y/LayoutConstants.TileSizeInMeters)- gridOffset.Y;
            return new GridCoordinate(gridx, gridy);
        }

        public void Render(Module activeModule)
        {
            Clear();

            RenderOuterComponent(activeModule);
            RenderInnerComponents(activeModule);

            if (!activeModule.IsTopLevelModule)
            {
                var modulesAtThisModulesLevel = activeModule.ParentModule.AsEnumerable();
                foreach (var module in modulesAtThisModulesLevel)
                {
                    RenderOuterComponent(module, 0.4f);
                    RenderInnerComponents(module, 0.4f);
                }
            }

            lastRenderedComponent = activeModule;
        }

        public GridCoordinate GetActiveComponentCenter()
        {
            var coreComponent = lastRenderedComponent.GetComponent<CoreComponent>();
            var midpoint = new GridCoordinate(
                Mathf.RoundToInt(coreComponent.InternalWidth/2.0f),
                Mathf.RoundToInt(coreComponent.InteralHeight/2.0f)
            );
            return midpoint + GetGridOffset(lastRenderedComponent);
        }

        private void Clear()
        {
            foreach (Transform child in innerRendererRoot.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in outerRendererRoot.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void RenderInnerComponents(Module moduleToRender, float opacity = 1.0f)
        {
            var gridOffset = GetGridOffset(moduleToRender);

            for (var x = 0; x < moduleToRender.ModuleGrid.Width; x++)
            {
                for (var y = 0; y < moduleToRender.ModuleGrid.Height; y++)
                {
                    var grid = new GridCoordinate(x, y);
                    var innerComponent = moduleToRender.GetModule(grid);
                    if (innerComponent != null)
                    {
                        var innerModule = moduleToRender.GetModule(grid);
                        var innerModuleAsset = Resources.Load<GameObject>(innerModule.GetComponent<CoreComponent>().Name);
                        var moduleGameObject = Instantiate(innerModuleAsset);
                        SetOpacity(moduleGameObject, opacity);
                        moduleGameObject.transform.parent = innerRendererRoot.transform;
                        moduleGameObject.transform.position = GridCoordinate.GridToPosition(grid + gridOffset);
                    }
                }
            }
        }

        private void RenderOuterComponent(Module module, float opacity = 1.0f)
        {
            var gridOffset = GetGridOffset(module);

            for (var x = 0; x < module.GetComponent<CoreComponent>().InternalWidth; x++)
            {
                for (var y = 0; y < module.GetComponent<CoreComponent>().InteralHeight; y++)
                {
                    var grid = new GridCoordinate(x, y);
                    var tile = Instantiate(Tile);
                    SetOpacity(tile, opacity);
                    tile.transform.parent = outerRendererRoot.transform;
                    tile.transform.position = GridCoordinate.GridToPosition(grid + gridOffset);
                }
            }
        }

        private static GridCoordinate GetGridOffset(Module module)
        {
            var moduleGridPosition = module.GetGridPosition();
            return new GridCoordinate(
                moduleGridPosition.X*LayoutConstants.MediumToLargeRatio,
                moduleGridPosition.Y*LayoutConstants.MediumToLargeRatio
            );
        }

        private void SetOpacity(GameObject go, float opacity)
        {
            var spriteRenderer = go.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, opacity);
        }
    }
}