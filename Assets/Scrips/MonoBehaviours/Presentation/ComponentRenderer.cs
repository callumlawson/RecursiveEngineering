using Assets.Framework.States;
using Assets.Scrips.Datastructures;
using Assets.Scrips.States;
using JetBrains.Annotations;
using UnityEngine;
using Entity = Assets.Framework.Entities.Entity;

namespace Assets.Scrips.MonoBehaviours.Presentation
{
    public class ComponentRenderer : MonoBehaviour
    {
        [UsedImplicitly] public GameObject Tile;

        private GameObject outerRendererRoot;
        private GameObject innerRendererRoot;
        private Entity lastRenderedEntity;

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

            lastRenderedEntity = new Entity(null, -1);
        }

        [UsedImplicitly]
        public void Update()
        {
            Clear();

            var activeEntity = StaticStates.Get<ActiveEntityState>().ActiveEntity;

            RenderOuterComponent(activeEntity);
            RenderInnerComponents(activeEntity);

            if (!activeEntity.GetState<PhysicalState>().IsRoot())
            {
                var entitiesAtThisEntitiesLevel = activeEntity.GetState<PhysicalState>().ParentEntity.GetState<PhysicalState>().ChildEntities;
                foreach (var entity in entitiesAtThisEntitiesLevel)
                {
                    RenderOuterComponent(entity, 0.4f);
                    RenderInnerComponents(entity, 0.4f);
                }
            }

            if (activeEntity != lastRenderedEntity)
            {
                CenterCamera(activeEntity);
            }

            lastRenderedEntity = activeEntity;
        }

        private void CenterCamera(Entity entity)
        {
            //CameraController.Instance.SetPosition(GridCoordinate.GridToPosition(GetComponentCenter(module)));
        }

        private GridCoordinate GetComponentCenter(Entity entity)
        {
            var coreComponent = entity.GetState<PhysicalState>();
            var midpoint = new GridCoordinate(
                Mathf.RoundToInt(coreComponent.InternalWidth/2.0f),
                Mathf.RoundToInt(coreComponent.InternalHeight/2.0f)
            );
            return midpoint;
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

        private void RenderInnerComponents(Entity entityToRender, float opacity = 1.0f)
        {
            for (var x = 0; x < entityToRender.GetState<PhysicalState>().InternalWidth; x++)
            {
                for (var y = 0; y < entityToRender.GetState<PhysicalState>().InternalHeight; y++)
                {
                    var grid = new GridCoordinate(x, y);
                    var innerEntities = entityToRender.GetState<PhysicalState>().GetEntitiesAtGrid(grid);
                    foreach (var innerEntity in innerEntities)
                    {
                        var innerModuleAsset = Resources.Load<GameObject>(innerEntity.GetState<NameState>().Name);
                        if (innerModuleAsset == null)
                        {
                            UnityEngine.Debug.LogError(innerEntity.GetState<NameState>().Name);
                        }
                        var moduleGameObject = Instantiate(innerModuleAsset);
                        //SetOpacity(moduleGameObject, opacity);
                        moduleGameObject.transform.parent = innerRendererRoot.transform;
                        moduleGameObject.transform.position = GridCoordinate.GridToPosition(grid);
                    }
                }
            }
        }

        private void RenderOuterComponent(Entity module, float opacity = 1.0f)
        {
            for (var x = 0; x < module.GetState<PhysicalState>().InternalWidth; x++)
            {
                for (var y = 0; y < module.GetState<PhysicalState>().InternalHeight; y++)
                {
                    var grid = new GridCoordinate(x, y);
                    var tile = Instantiate(Tile);
                    //SetOpacity(tile, opacity);
                    tile.transform.parent = outerRendererRoot.transform;
                    tile.transform.position = GridCoordinate.GridToPosition(grid);
                }
            }
        }

        private static void SetOpacity(GameObject go, float opacity)
        {
            var spriteRenderer = go.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, opacity);
        }
    }
}