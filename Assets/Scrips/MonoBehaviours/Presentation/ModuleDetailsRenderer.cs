﻿using Assets.Scrips.Components;
using Assets.Scrips.MonoBehaviours.Controls;
using Assets.Scrips.Networks;
using Assets.Scrips.Util;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

//TODO: Replace this with reactive binds

namespace Assets.Scrips.MonoBehaviours.Presentation
{
    //TODO: Split into separate renderers
    public class ModuleDetailsRenderer : MonoBehaviour
    {
        public GameRunner GameRunner;
        public Text Breadcrumb;
        public Text SelectedComponentName;

        public Image SelectedComponent;
        public Image PreviousComponent;
        public Image NextComponent;

        [UsedImplicitly]
        public void Update()
        {
            UpdateBreadcrumb();
            UpdateComponentDetails();
            UpdateSelectedLibraryComponent();
        }

        private void UpdateSelectedLibraryComponent()
        {
            var moduleLibrary = ModuleLibrary.Instance;
            SelectedComponent.sprite =
                Resources.Load<GameObject>(
                    moduleLibrary.GetSelectedModule().GetComponent<CoreComponent>().Name)
                    .GetComponent<SpriteRenderer>()
                    .sprite;
            PreviousComponent.sprite =
                Resources.Load<GameObject>(
                    moduleLibrary.GetPreviousModule().GetComponent<CoreComponent>().Name)
                    .GetComponent<SpriteRenderer>()
                    .sprite;
            NextComponent.sprite =
                Resources.Load<GameObject>(
                    moduleLibrary.GetNextModule().GetComponent<CoreComponent>().Name)
                    .GetComponent<SpriteRenderer>()
                    .sprite;
        }

        //TODO: Use custom "toString" style pattern here.
        private void UpdateComponentDetails()
        {
            var selectedComponent = GameRunner.CurrentlySelectedModule();
            if (selectedComponent != null)
            {
                SelectedComponentName.text = string.Format(
                    "Selected Grid: {0} Selected Module: {1} Water: {2}",
                    GridSelector.CurrentlySelectedGrid(GameRunner.ActiveModule),
                    GameRunner.CurrentlySelectedModule().GetComponent<CoreComponent>().Name,
                    SubstanceNetwork.Instance.GetWater(selectedComponent)
                    );
            }
            else
            {
                SelectedComponentName.text = string.Format(
                    "Selected Grid: {0}",
                    GridSelector.CurrentlySelectedGrid(GameRunner.ActiveModule));
            }
        }

        private void UpdateBreadcrumb()
        {
            var breadcrumb = "";
            foreach (var component in GameRunner.CurrentHeirarchy())
            {
                breadcrumb = ">" + component.GetComponent<CoreComponent>().Name + breadcrumb;
            }
            Breadcrumb.text = breadcrumb;
        }
    }
}