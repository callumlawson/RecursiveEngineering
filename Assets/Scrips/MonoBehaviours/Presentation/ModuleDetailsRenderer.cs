using System.Collections.Generic;
using Assets.Framework.Entities;
using Assets.Framework.States;
using Assets.Scrips.States;
using Assets.Scrips.Systems;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

//TODO: Replace this with reactive binds
namespace Assets.Scrips.MonoBehaviours.Presentation
{
    //TODO: Split into separate renderers
    public class ModuleDetailsRenderer : MonoBehaviour
    {
        [UsedImplicitly] public GameRunner GameRunner;
        [UsedImplicitly] public Text Breadcrumb;
        [UsedImplicitly] public Text SelectedComponentName;

        [UsedImplicitly] public Image SelectedComponent;
        [UsedImplicitly] public Image PreviousComponent;
        [UsedImplicitly] public Image NextComponent;

        [UsedImplicitly]
        public void Update()
        {
            UpdateBreadcrumb();
            UpdateComponentDetails();
            UpdateSelectedLibraryComponent();
        }

        private void UpdateSelectedLibraryComponent()
        {
            var entityLibrary = StaticStates.Get<EntityLibraryState>();
            SelectedComponent.sprite =
                Resources.Load<GameObject>(
                    GetState<EntityTypeState>(entityLibrary.GetSelectedEntity()).EntityType)
                    .GetComponent<SpriteRenderer>()
                    .sprite;
            PreviousComponent.sprite =
                Resources.Load<GameObject>(
                    GetState<EntityTypeState>(entityLibrary.GetPreviousEntity()).EntityType)
                    .GetComponent<SpriteRenderer>()
                    .sprite;
            NextComponent.sprite =
                Resources.Load<GameObject>(
                    GetState<EntityTypeState>(entityLibrary.GetNextEntity()).EntityType)
                    .GetComponent<SpriteRenderer>()
                    .sprite;
        }

        //TODO: Use custom "toString" style pattern here.
        private void UpdateComponentDetails()
        {
            var selectedState = StaticStates.Get<SelectedState>();
            if (selectedState.Entity != null)
            {
                SelectedComponentName.text = string.Format(
                    "Selected Grid: {0} Diesel: {1}",
                    selectedState.Grid,
                    SubstanceNetworkSystem.Instance.GetDiesel(selectedState.Entity)
                );
            }
            else
            {
                SelectedComponentName.text = string.Format(
                    "Selected Grid: {0}",
                    selectedState.Grid
                );
            }
        }

        private void UpdateBreadcrumb()
        {
            var breadcrumb = "";
            foreach (var component in CurrentHeirarchy())
            {
                breadcrumb = ">" + component.GetState<EntityTypeState>().EntityType + breadcrumb;
            }
            Breadcrumb.text = breadcrumb;
        }

        private static IEnumerable<Entity> CurrentHeirarchy()
        {
            var heirarchy = new List<Entity>();
            var current = StaticStates.Get<ActiveEntityState>().ActiveEntity;
            do
            {
                heirarchy.Add(current);
                current = current.GetState<PhysicalState>().ParentEntity;
            } while (current != null);
            return heirarchy;
        }

        private static T GetState<T>(IEnumerable<IState> states) where T : IState
        {
            foreach (var state in states)
            {
                if (state.GetType() == typeof(T))
                {
                    return (T)state;
                }
            }
            return default(T);
        }
    }
}