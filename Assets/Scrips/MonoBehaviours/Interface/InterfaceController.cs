using Assets.Scrips.Components;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

//TODO: Replace this with reactive binds
namespace Assets.Scrips.MonoBehaviours.Interface
{
    //TODO: Split into separate renderers
    public class InterfaceController : MonoBehaviour
    {
        public GameRunner GameRunner;
        public Text Breadcrumb;
        public Text SelectedComponentName;

        public Image SelectedComponent;
        public Image PreviousComponent;
        public Image NextComponent;

        [UsedImplicitly]
        public void Update ()
        {
            UpdateBreadcrumb();
            UpdateComponentName();
            UpdateSelectedLibraryComponent();
        }

        private void UpdateSelectedLibraryComponent()
        {
            var componentLibrary = GameRunner.ModuleLibrary;
            SelectedComponent.sprite = Resources.Load<GameObject>(componentLibrary.GetComponent<CoreComponent>(componentLibrary.GetSelectedComponent()).Name).GetComponent<SpriteRenderer>().sprite;
            PreviousComponent.sprite = Resources.Load<GameObject>(componentLibrary.GetComponent<CoreComponent>(componentLibrary.GetPreviousComponent()).Name).GetComponent<SpriteRenderer>().sprite;
            NextComponent.sprite = Resources.Load<GameObject>(componentLibrary.GetComponent<CoreComponent>(componentLibrary.GetNextComponent()).Name).GetComponent<SpriteRenderer>().sprite;
        }

        //TODO: Use custom "toString" style pattern here.
        private void UpdateComponentName()
        {
            var selectedComponent = GameRunner.CurrentlySelectedComponent();
            if (selectedComponent != null)
            {
                SelectedComponentName.text = string.Format(
                    "Selected Module: {0} Water: {1}",
                    GameRunner.CurrentlySelectedComponent().GetComponent<CoreComponent>().Name,
                    GameRunner.GlobalSubstanceNetwork.GetWater(selectedComponent)
                );
            }
            else
            {
                SelectedComponentName.text = "Selected Module: None";
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
