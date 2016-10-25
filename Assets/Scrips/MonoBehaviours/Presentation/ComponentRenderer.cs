using Assets.Framework.States;
using Assets.Framework.Util;
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
            SimplePool.Preload(Tile, 200);

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

            Profiler.BeginSample("Component Renderer outer");
            RenderOuterComponent(activeEntity);
            Profiler.EndSample();

            Profiler.BeginSample("Component Renderer inner");
            RenderInnerComponents(activeEntity);
            Profiler.EndSample();

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
                SimplePool.Despawn(child.gameObject);
            }

            foreach (Transform child in outerRendererRoot.transform)
            {
                SimplePool.Despawn(child.gameObject);
            }
        }

        private void RenderInnerComponents(Entity entityToRender, float opacity = 1.0f)
        {
            for (var x = 0; x < entityToRender.GetState<PhysicalState>().InternalWidth; x++)
            {
                for (var y = 0; y < entityToRender.GetState<PhysicalState>().InternalHeight; y++)
                {
                    var grid = new GridCoordinate(x, y);

                    Profiler.BeginSample("Get entities at grid");
                    var innerEntities = entityToRender.GetState<PhysicalState>().GetEntitiesAtGrid(grid);
                    Profiler.EndSample();

                    foreach (var innerEntity in innerEntities)
                    {
                        Profiler.BeginSample("Resources Loading");
                        var innerModuleAsset = Resources.Load<GameObject>(innerEntity.GetState<EntityTypeState>().EntityType);
                        Profiler.EndSample();

                        if (innerModuleAsset == null)
                        {
                            UnityEngine.Debug.LogError(innerEntity.GetState<EntityTypeState>().EntityType);
                        }

                        var moduleGameObject = SimplePool.Spawn(innerModuleAsset);
                        //SetOpacity(moduleGameObject, opacity);
                        moduleGameObject.transform.parent = innerRendererRoot.transform;
                        moduleGameObject.transform.position = GridCoordinate.GridToPosition(grid);
                        var possibleRenderer = moduleGameObject.GetComponent<IEntityRenderer>();
                        if (possibleRenderer != null)
                        {
                            possibleRenderer.OnRenderEntity(innerEntity);       
                        }
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
                    var tile = SimplePool.Spawn(Tile);
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