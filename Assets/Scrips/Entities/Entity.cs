using System;
using System.Collections.Generic;
using Assets.Scrips.States;
using UnityEngine.Assertions;

namespace Assets.Scrips.Entities
{
    [Serializable]
    public class Entity
    {
        private readonly EntityManager entityManager;
        public int EntityId { get; private set; }

        //For debugging only!
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

        public bool HasState<T>() where T : IState
        {
            return GetState<T>() != null;
        }

        public void Delete()
        {
            entityManager.DeleteEntity(this);
        }

        private List<IState> States()
        {
           return entityManager.GetStates(this);
        }

        public string ToJson()
        {
            return "";
        }
    }
}
