using Assets.Framework.Entities;
using Assets.Scrips.States;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scrips.MonoBehaviours.Presentation
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CrewRenderer : MonoBehaviour, IEntityRenderer
    {
        [UsedImplicitly] public Sprite AliveCrew;
        [UsedImplicitly] public Sprite DeadCrew;

        private SpriteRenderer spriteRenderer;

        [UsedImplicitly]
        public void Awake()
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
