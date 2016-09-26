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
            var componentLibrary = GameRunner.ComponentLibrary;
            SelectedComponent.sprite = Resources.Load<GameObject>(componentLibrary.GetSelectedComponent().Name).GetComponent<SpriteRenderer>().sprite;
            PreviousComponent.sprite = Resources.Load<GameObject>(componentLibrary.GetPreviousComponent().Name).GetComponent<SpriteRenderer>().sprite;
            NextComponent.sprite = Resources.Load<GameObject>(componentLibrary.GetNextComponent().Name).GetComponent<SpriteRenderer>().sprite;
        }

        //TODO: Use custom "toString" style pattern here.
        private void UpdateComponentName()
        {
            var selectedComponent = GameRunner.CurrentlySelectedComponent();
            if (selectedComponent != null)
            {
                SelectedComponentName.text = string.Format(
                    "Selected Component: {0} Water: {1}",
                    GameRunner.CurrentlySelectedComponent().Name,
                    GameRunner.GlobalSubstanceNetwork.GetWater(selectedComponent)
                );
            }
            else
            {
                SelectedComponentName.text = "Selected Component: None";
            }
        }

        private void UpdateBreadcrumb()
        {
            var breadcrumb = "";
            foreach (var component in GameRunner.CurrentHeirarchy())
            {
                breadcrumb = ">" + component.Name + breadcrumb;
            }
            Breadcrumb.text = breadcrumb;
        }
    }
}
