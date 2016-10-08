using Assets.Scrips.Datatypes;
using Assets.Scrips.Entities;
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
            var gridx = Mathf.Round(mousePosition.x / EntityUtils.TileSizeInMeters) * EntityUtils.TileSizeInMeters;
            var gridy = Mathf.Round(mousePosition.y / EntityUtils.TileSizeInMeters) * EntityUtils.TileSizeInMeters;
            selectedGridIndicator.transform.position = new Vector3(gridx, gridy, 0);
        }

        public static GridCoordinate CurrentlySelectedGrid(Entity activeEntity)
        {
            var gridOffset = new GridCoordinate(0, 0);
//            if (activeEntity != null)
//            {
//                gridOffset = EntityUtils.GetGridOffset(activeEntity);
//            }
            var mousePosition = CameraController.ActiveCamera.ScreenToWorldPoint(Input.mousePosition);
            var gridx = Mathf.RoundToInt(mousePosition.x / EntityUtils.TileSizeInMeters) - gridOffset.X;
            var gridy = Mathf.RoundToInt(mousePosition.y / EntityUtils.TileSizeInMeters) - gridOffset.Y;
            return new GridCoordinate(gridx, gridy);
        }
    }
}
