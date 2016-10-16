using Assets.Framework.States;
using Assets.Framework.Util;
using Assets.Scrips.Datastructures;
using Assets.Scrips.Modules;
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

        private void UpdateCurrentlySelectedGrid()
        {
            var gridOffset = new GridCoordinate(0, 0);
            //            if (activeEntity != null)
            //            {
            //                gridOffset = GlobalConstants.GetGridOffset(activeEntity);
            //            }
            var mousePosition = CameraController.ActiveCamera.ScreenToWorldPoint(Input.mousePosition);
            var gridx = Mathf.RoundToInt(mousePosition.x / GlobalConstants.TileSizeInMeters) - gridOffset.X;
            var gridy = Mathf.RoundToInt(mousePosition.y / GlobalConstants.TileSizeInMeters) - gridOffset.Y;

            var selectedGrid = new GridCoordinate(gridx, gridy);
            var physicalState = StaticStates.Get<ActiveEntityState>().ActiveEntity.GetState<PhysicalState>();
            StaticStates.Get<SelectedState>().Grid = selectedGrid;
            StaticStates.Get<SelectedState>().Entity = physicalState.GetEntityAtGrid(selectedGrid);
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
