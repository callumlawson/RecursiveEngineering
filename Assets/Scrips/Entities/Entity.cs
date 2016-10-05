using System;
using Assets.Scrips.Components;
using UnityEngine.Assertions;

namespace Assets.Scrips.Entities
{
    public class Entity
    {
        private EntityManager entityManager;
        public int EntityId { get; private set; }

        public Entity(EntityManager entityManager, int entityId)
        {
            this.entityManager = entityManager;
            EntityId = entityId;
        }

        public void AddState<T>(IState state) where T : IState
        {
            Assert.IsTrue(state != null, "State added must not be null.");
            entityManager.AddState<T>(this, state);
        }

        public T GetState<T>() where T : IState
        {
            return entityManager.GetState<T>(this);
        }

        public void Delete()
        {
            entityManager.DeleteEntity(this);
        }
    }
}
