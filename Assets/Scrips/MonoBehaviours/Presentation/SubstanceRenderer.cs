using Assets.Scrips.Components;
using Assets.Scrips.Networks;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scrips.MonoBehaviours.Presentation
{
    public class SubstanceRenderer : MonoBehaviour {

        [UsedImplicitly] public GameObject WaterTile;

        private GameObject substanceRenderRoot;

        // Use this for initialization
        void Start () {
            substanceRenderRoot = new GameObject();
            if (substanceRenderRoot != null)
            {
                substanceRenderRoot.name = "SubstanceRenderRoot";
                substanceRenderRoot.transform.parent = transform;
            }
        }

        public void RenderSubstancesForComponent(EngiComponent component, SubstanceNetwork substanceNetwork)
        {
            var substanceNodes = substanceNetwork.GetNodesForComonent(component);
            foreach (var substanceNode in substanceNodes)
            {
                

            }
        }

        // Update is called once per frame
        void Update () {
	
        }
    }
}
