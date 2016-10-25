using Assets.Framework.Entities;
using Assets.Scrips.States;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scrips.MonoBehaviours.Presentation
{
    [RequireComponent(typeof(SpriteRenderer))]
    class CrewRenderer : MonoBehaviour, IEntityRenderer
    {
        [UsedImplicitly] public Sprite AliveCrew;
        [UsedImplicitly] public Sprite DeadCrew;

        private SpriteRenderer spriteRenderer;

        [UsedImplicitly]
        public void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void OnRenderEntity(Entity entity)
        {
            var healthState = entity.GetState<HealthState>();
            spriteRenderer.sprite = healthState.CurrentHealth > 0 ? AliveCrew : DeadCrew;
        }
    }
}
