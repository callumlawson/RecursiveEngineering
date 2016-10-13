using System;
using System.Collections.Generic;
using Assets.Framework.States;

namespace Assets.Framework.Entities
{
    [Serializable]
    public class Entity : IComparable<Entity>
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
            entityManager.AddState<T>(this, state);
        }

        public T GetState<T>() where T : IState
        {
            return entityManager.GetState<T>(this);
        }

        public bool HasState(Type stateType)
        {
            return entityManager.GetState(this, stateType) != null;
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
            throw new NotImplementedException();
        }

        public int CompareTo(Entity other)
        {
            throw new NotImplementedException();
        }
    }
}
