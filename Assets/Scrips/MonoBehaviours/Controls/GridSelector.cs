using System.Linq;
using Assets.Framework.States;
using Assets.Framework.Util;
using Assets.Scrips.Datastructures;
using Assets.Scrips.States;
using Assets.Scrips.Util;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scrips.MonoBehaviours.Controls
{
    class GridSelector : MonoBehaviour
    {
        [UsedImplicitly] public GameObject Selector;

        private GameObject selectedGridIndicator;

        [UsedImplicitly]
        public void Start()
        {
            selectedGridIndicator = SimplePool.Spawn(Selector);
            selectedGridIndicator.transform.parent = transform;
        }

        [UsedImplicitly]
        public void Update()
        {
            UpdateCurrentlySelectedGrid();
            UpdateSelectedGridIndicator();
        }

        private static void UpdateCurrentlySelectedGrid()
        {
            var gridOffset = new GridCoordinate(0, 0);
            var mousePosition = CameraController.ActiveCamera.ScreenToWorldPoint(Input.mousePosition);
            var gridx = Mathf.RoundToInt(mousePosition.x / GlobalConstants.TileSizeInMeters) - gridOffset.X;
            var gridy = Mathf.RoundToInt(mousePosition.y / GlobalConstants.TileSizeInMeters) - gridOffset.Y;

            var selectedGrid = new GridCoordinate(gridx, gridy);
            var physicalState = StaticStates.Get<ActiveEntityState>().ActiveEntity.GetState<PhysicalState>();
            StaticStates.Get<SelectedState>().Grid = selectedGrid;
            var entitiesAtGrid = physicalState.GetEntitiesAtGrid(selectedGrid);
            var tangableEntities = entitiesAtGrid.Where(entity => entity.HasState<PhysicalState>() && entity.GetState<PhysicalState>().IsTangible).ToList();
            if (tangableEntities.Count > 1)
            {
                UnityEngine.Debug.LogError("More than one tangable entity in a grid! This is not supported.");
            }
            StaticStates.Get<SelectedState>().Entity = tangableEntities.FirstOrDefault();
        }

        private void UpdateSelectedGridIndicator()
        {
            var mousePosition = CameraController.ActiveCamera.ScreenToWorldPoint(Input.mousePosition);
            var gridx = Mathf.Round(mousePosition.x/GlobalConstants.TileSizeInMeters)*GlobalConstants.TileSizeInMeters;
            var gridy = Mathf.Round(mousePosition.y/GlobalConstants.TileSizeInMeters)*GlobalConstants.TileSizeInMeters;
            selectedGridIndicator.transform.position = new Vector3(gridx, gridy, 0);
        }
    }
}
