using System.Collections.Generic;
using Assets.Scrips.Components;
using Assets.Scrips.Modules;
using Assets.Scrips.MonoBehaviours.Camera;
using Assets.Scrips.Util;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scrips.MonoBehaviours.Presentation
{
    public class ComponentRenderer : MonoBehaviour
    {
        [UsedImplicitly] public GameObject Tile;

        private GameObject outerRendererRoot;
        private GameObject innerRendererRoot;
        private Module lastRenderedComponent;

        [UsedImplicitly]
        public void Start()
        {
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

            lastRenderedComponent = new Module(null, new List<IComponent> { new CoreComponent("Dummy", 0, 0) });
        }

        [UsedImplicitly]
        public void Update()
        {
            Clear();

            var activeModule = GameRunner.Instance.ActiveModule;

            RenderOuterComponent(activeModule);
            RenderInnerComponents(activeModule);

            if (!activeModule.IsTopLevelModule)
            {
                var modulesAtThisModulesLevel = activeModule.ParentModule.GetContainedModules();
                foreach (var module in modulesAtThisModulesLevel)
                {
                    RenderOuterComponent(module, 0.4f);
                    RenderInnerComponents(module, 0.4f);
                }
            }

            if (activeModule != lastRenderedComponent)
            {
                CenterCamera(activeModule);
            }

            lastRenderedComponent = activeModule;
        }

        private void CenterCamera(Module module)
        {
            CameraController.Instance.SetPosition(GridCoordinate.GridToPosition(GetComponentCenter(module)));
        }

        private GridCoordinate GetComponentCenter(Module module)
        {
            var coreComponent = module.GetComponent<CoreComponent>();
            var midpoint = new GridCoordinate(
                Mathf.RoundToInt(coreComponent.InternalWidth/2.0f),
                Mathf.RoundToInt(coreComponent.InteralHeight/2.0f)
            );
            return midpoint + GridCoordinate.GetGridOffset(module);
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
            var gridOffset = GridCoordinate.GetGridOffset(moduleToRender);

            for (var x = 0; x < moduleToRender.GetComponent<CoreComponent>().InternalWidth; x++)
            {
                for (var y = 0; y < moduleToRender.GetComponent<CoreComponent>().InteralHeight; y++)
                {
                    var grid = new GridCoordinate(x, y);
                    var innerComponent = moduleToRender.GetModule(grid);
                    if (innerComponent != null)
                    {
                        var innerModule = moduleToRender.GetModule(grid);
                        var innerModuleAsset = Resources.Load<GameObject>(innerModule.GetComponent<CoreComponent>().Name);
                        if (innerModuleAsset == null)
                        {
                            UnityEngine.Debug.LogError(innerModule.GetComponent<CoreComponent>().Name);
                        }
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
            var gridOffset = GridCoordinate.GetGridOffset(module);

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

        private void SetOpacity(GameObject go, float opacity)
        {
            var spriteRenderer = go.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, opacity);
        }
    }
}