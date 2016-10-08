using System;
using System.Collections.Generic;
using Assets.Scrips.Components;
using UnityEngine.Assertions;

namespace Assets.Scrips.Entities
{
    [Serializable]
    public class Entity
    {
        private readonly EntityManager entityManager;
        public int EntityId { get; private set; }

        public List<IState> DebugStates
        {
            get { return States(); }
        }

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

        public List<IState> States()
        {
           return entityManager.GetStates(this);
        }
    }
}
