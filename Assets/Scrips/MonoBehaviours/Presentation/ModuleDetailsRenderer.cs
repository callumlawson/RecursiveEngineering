using Assets.Scrips.Entities;
using Assets.Scrips.MonoBehaviours.Controls;
using Assets.Scrips.States;
using Assets.Scrips.Systems.Substance;
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
            var entityLibrary= EntityLibrary.Instance;
            SelectedComponent.sprite =
                Resources.Load<GameObject>(
                    EntityLibrary.GetState<NameState>(entityLibrary.GetSelectedModule()).Name)
                    .GetComponent<SpriteRenderer>()
                    .sprite;
            PreviousComponent.sprite =
                Resources.Load<GameObject>(
                    EntityLibrary.GetState<NameState>(entityLibrary.GetPreviousModule()).Name)
                    .GetComponent<SpriteRenderer>()
                    .sprite;
            NextComponent.sprite =
                Resources.Load<GameObject>(
                    EntityLibrary.GetState<NameState>(entityLibrary.GetNextModule()).Name)
                    .GetComponent<SpriteRenderer>()
                    .sprite;
        }

        //TODO: Use custom "toString" style pattern here.
        private void UpdateComponentDetails()
        {
            var selectedComponent = GameRunner.CurrentlySelectedEntity();
            if (selectedComponent != null)
            {
                SelectedComponentName.text = string.Format(
                    "Selected Grid: {0} Selected Entity: {1} Diesel: {2}",
                    GridSelector.CurrentlySelectedGrid(),
                    GameRunner.CurrentlySelectedEntity().GetState<NameState>().Name,
                    SubstanceNetwork.Instance.GetDiesel(selectedComponent)
                );
            }
            else
            {
                SelectedComponentName.text = string.Format(
                    "Selected Grid: {0}",
                    GridSelector.CurrentlySelectedGrid()
                );
            }
        }

        private void UpdateBreadcrumb()
        {
            var breadcrumb = "";
            foreach (var component in GameRunner.CurrentHeirarchy())
            {
                breadcrumb = ">" + component.GetState<NameState>().Name + breadcrumb;
            }
            Breadcrumb.text = breadcrumb;
        }
    }
}