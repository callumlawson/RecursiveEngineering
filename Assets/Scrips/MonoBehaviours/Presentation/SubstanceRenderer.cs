using Assets.Framework.States;
using Assets.Framework.Util;
using Assets.Scrips.Datastructures;
using Assets.Scrips.States;
using Assets.Scrips.Util;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scrips.MonoBehaviours.Presentation
{
    public class SubstanceRenderer : MonoBehaviour
    {
        private Color dieselColor = new Color(1.0f, 0.6f, 0.3f);

        private GameObject substanceRenderRoot;
        [UsedImplicitly] public GameObject SubstanceTile;
        private SpriteRenderer[,] tileGrid;
        private Color waterColor = new Color(0f, 0f, 1f);

        [UsedImplicitly]
        private void Start()
        {
            SimplePool.Preload(SubstanceTile, 200);

            tileGrid = new SpriteRenderer[GlobalConstants.MaxWidth, GlobalConstants.MaxHeight];
            substanceRenderRoot = new GameObject();
            if (substanceRenderRoot != null)
            {
                substanceRenderRoot.name = "SubstanceRenderRoot";
                substanceRenderRoot.transform.parent = transform;
            }

            InitSubstanceTiles();
        }

        [UsedImplicitly]
        public void Update()
        {
            var activeEntity = StaticStates.Get<ActiveEntityState>().ActiveEntity;

            for (var x = 0; x < GlobalConstants.MaxWidth; x++)
            {
                for (var y = 0; y < GlobalConstants.MaxHeight; y++)
                {
                    tileGrid[x, y].enabled = false;
                }
            }

            //TODO: Make this not rubbish
            foreach (var entity in activeEntity.GetState<PhysicalState>().ChildEntities)
            {
                var substanceState = entity.GetState<SubstanceNetworkState>();
                var gridForSubstance = entity.GetState<PhysicalState>().BottomLeftCoordinate;
                if (substanceState != null)
                {
                    var diesel = substanceState.GetSubstance(SubstanceType.Diesel);
                    if (diesel > 0.0f)
                    {
                        var tile = tileGrid[gridForSubstance.X, gridForSubstance.Y];
                        tile.enabled = true;
                        dieselColor.a = Mathf.Clamp(diesel / 100.0f - 0.3f, 0, 1);
                        tile.color = dieselColor;
                    }

                    var water = substanceState.GetSubstance(SubstanceType.SeaWater);
                    if (water > 0.0f)
                    {
                        var tile = tileGrid[gridForSubstance.X, gridForSubstance.Y];
                        tile.enabled = true;
                        waterColor.a = Mathf.Clamp(water / 100.0f - 0.3f, 0, 1);
                        tile.color = waterColor;
                    }
                }
            }
        }

        private void InitSubstanceTiles()
        {
            foreach (Transform child in substanceRenderRoot.transform)
            {
                SimplePool.Despawn(child.gameObject);
            }

            for (var x = 0; x < GlobalConstants.MaxWidth; x++)
            {
                for (var y = 0; y < GlobalConstants.MaxHeight; y++)
                {
                    var grid = new GridCoordinate(x, y);
                    var tile = SimplePool.Spawn(SubstanceTile);
                    var spriteRenderer = tile.GetComponent<SpriteRenderer>();
                    spriteRenderer.enabled = false;
                    tileGrid[x, y] = spriteRenderer;
                    tile.transform.parent = substanceRenderRoot.transform;
                    tile.transform.position = GridCoordinate.GridToPosition(grid);
                }
            }
        }
    }
}