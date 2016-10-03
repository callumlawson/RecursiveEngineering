﻿using Assets.Scrips.Modules;
using Assets.Scrips.Networks;
using Assets.Scrips.Networks.Substance;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scrips.MonoBehaviours.Presentation
{
    public class SubstanceRenderer : MonoBehaviour {

        [UsedImplicitly] public GameObject WaterTile;

        private GameObject substanceRenderRoot;
        private SpriteRenderer[,] tileGrid;

        // Use this for initialization
        [UsedImplicitly]
        void Start () {
            tileGrid = new SpriteRenderer[ModuleUtils.MaxWidth, ModuleUtils.MaxHeight];
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
            var activeComponent = GameRunner.Instance.ActiveModule;
            var substanceNetwork = SubstanceNetwork.Instance;

            for (var x = 0; x < ModuleUtils.MaxWidth; x++)
            {
                for (var y = 0; y < ModuleUtils.MaxHeight; y++)
                {
                    tileGrid[x, y].enabled = false;
                }
            }

            foreach (var innerComponent in activeComponent.GetContainedModules())
            {
                var substanceNode = substanceNetwork.GetNodeForComponent(innerComponent);
                var gridForSubstance = ModuleUtils.GetGridOffset(activeComponent) +
                                       activeComponent.GetGridForContainedModule(innerComponent);
                if (substanceNode != null)
                {
                    var water = substanceNode.GetSubstance(SubstanceTypes.WATER);
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

            for (var x = 0; x < ModuleUtils.MaxWidth; x++)
            {
                for (var y = 0; y < ModuleUtils.MaxHeight; y++)
                {
                    var grid = new GridCoordinate(x, y);
                    var tile = Instantiate(WaterTile);
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
