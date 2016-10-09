using Assets.Scrips.Datastructures;
using Assets.Scrips.Modules;
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
            var gridx = Mathf.Round(mousePosition.x / GlobalConstants.TileSizeInMeters) * GlobalConstants.TileSizeInMeters;
            var gridy = Mathf.Round(mousePosition.y / GlobalConstants.TileSizeInMeters) * GlobalConstants.TileSizeInMeters;
            selectedGridIndicator.transform.position = new Vector3(gridx, gridy, 0);
        }

        public static GridCoordinate CurrentlySelectedGrid()
        {
            var gridOffset = new GridCoordinate(0, 0);
//            if (activeEntity != null)
//            {
//                gridOffset = GlobalConstants.GetGridOffset(activeEntity);
//            }
            var mousePosition = CameraController.ActiveCamera.ScreenToWorldPoint(Input.mousePosition);
            var gridx = Mathf.RoundToInt(mousePosition.x / GlobalConstants.TileSizeInMeters) - gridOffset.X;
            var gridy = Mathf.RoundToInt(mousePosition.y / GlobalConstants.TileSizeInMeters) - gridOffset.Y;
            return new GridCoordinate(gridx, gridy);
        }
    }
}
