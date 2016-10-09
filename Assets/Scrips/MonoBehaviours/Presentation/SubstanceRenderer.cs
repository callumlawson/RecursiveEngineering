using Assets.Scrips.Datastructures;
using Assets.Scrips.Modules;
using Assets.Scrips.States;
using Assets.Scrips.Systems.Substance;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scrips.MonoBehaviours.Presentation
{
    public class SubstanceRenderer : MonoBehaviour
    {
        [UsedImplicitly] public GameObject SubstanceTile;

        private GameObject substanceRenderRoot;
        private SpriteRenderer[,] tileGrid;

        [UsedImplicitly]
        private void Start()
        {
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
            var activeEnity = GameRunner.Instance.ActiveEntity;

            for (var x = 0; x < GlobalConstants.MaxWidth; x++)
            {
                for (var y = 0; y < GlobalConstants.MaxHeight; y++)
                {
                    tileGrid[x, y].enabled = false;
                }
            }

            foreach (var entity in activeEnity.GetState<PhysicalState>().ChildEntities)
            {
                var substanceState = entity.GetState<SubstanceNetworkState>();
                var gridForSubstance = entity.GetState<PhysicalState>().BottomLeftCoordinate;
                if (substanceState != null)
                {
                    var water = substanceState.GetSubstance(SubstanceType.Diesel);
                    if (water > 0.0f)
                    {
                        var tile = tileGrid[gridForSubstance.X, gridForSubstance.Y];
                        tile.enabled = true;
                        tile.color = new Color(1, 1, 1, Mathf.Clamp(water/100.0f, 0, 1));
                    }
                }
            }
        }

        private void InitSubstanceTiles()
        {
            foreach (Transform child in substanceRenderRoot.transform)
            {
                Destroy(child.gameObject);
            }

            for (var x = 0; x < GlobalConstants.MaxWidth; x++)
            {
                for (var y = 0; y < GlobalConstants.MaxHeight; y++)
                {
                    var grid = new GridCoordinate(x, y);
                    var tile = Instantiate(SubstanceTile);
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