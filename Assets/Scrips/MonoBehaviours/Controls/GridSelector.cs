using Assets.Scrips.Modules;
using Assets.Scrips.MonoBehaviours.Camera;
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
            selectedGridIndicator = Instantiate(Selector);
            selectedGridIndicator.transform.parent = transform;
        }

        [UsedImplicitly]
        public void Update()
        {
            var mousePosition = CameraController.ActiveCamera.ScreenToWorldPoint(Input.mousePosition);
            var gridx = Mathf.Round(mousePosition.x / LayoutConstants.TileSizeInMeters) * LayoutConstants.TileSizeInMeters;
            var gridy = Mathf.Round(mousePosition.y / LayoutConstants.TileSizeInMeters) * LayoutConstants.TileSizeInMeters;
            selectedGridIndicator.transform.position = new Vector3(gridx, gridy, 0);
        }

        public static GridCoordinate CurrentlySelectedGrid(Module activeComponent)
        {
            var gridOffset = new GridCoordinate(0, 0);
            if (activeComponent != null)
            {
                gridOffset = GridCoordinate.GetGridOffset(activeComponent);
            }
            var mousePosition = CameraController.ActiveCamera.ScreenToWorldPoint(Input.mousePosition);
            var gridx = Mathf.RoundToInt(mousePosition.x / LayoutConstants.TileSizeInMeters) - gridOffset.X;
            var gridy = Mathf.RoundToInt(mousePosition.y / LayoutConstants.TileSizeInMeters) - gridOffset.Y;
            return new GridCoordinate(gridx, gridy);
        }
    }
}
