using System;
using System.Collections.Generic;
using System.Reflection;
using Assets.Scrips.Datastructures;
using Assets.Scrips.States;
using Assets.Scrips.Systems;
using JetBrains.Annotations;

namespace Assets.Scrips.Entities
{
    public class EntityManager
    {
        private readonly Bag<Entity> entities;

        private readonly Dictionary<Type, Bag<IState>> statesByType;

        private int nextAvailableId;

        //Premature optimisation is the devil's work...
        private readonly MethodInfo addStateMethod = typeof(EntityManager).GetMethod("AddState");

        public EntityManager()
        {
            entities = new Bag<Entity>();    
            statesByType = new Dictionary<Type, Bag<IState>>();
            nextAvailableId = 0;
        }

        public Entity BuildEntity(List<IState> states)
        {
            var entity = CreateEmptyEntity();
            foreach (var state in states)
            {
                var genericAdd = addStateMethod.MakeGenericMethod(state.GetType());
                genericAdd.Invoke(this, new object[] {entity, state});
            }
            SystemManager.EntityAdded(entity);
            return entity;
        }

        //Should not be called directly - will mess up registration with systems
        private Entity CreateEmptyEntity()
        {
            var entityId = nextAvailableId;
            nextAvailableId++;
            var entity = new Entity(this, entityId);
            entities.Set(entityId, entity);
            return entity;
        }

        public Entity GetEntity(int entityId)
        {
            return entities[entityId];
        }

        public void DeleteEntity([NotNull] Entity entity)
        {
            SystemManager.EntityRemoved(entity);
            RemoveStatesForEntity(entity);
            entities[entity.EntityId] = null;
        }

        public void AddState<T>([NotNull] Entity entity, [NotNull] IState state) where T : IState
        {
            Bag<IState> componentsOfType;
            var listInitialized = statesByType.TryGetValue(typeof(T), out componentsOfType);

            if (!listInitialized)
            {
                componentsOfType = new Bag<IState>();
                statesByType.Add(typeof(T), componentsOfType);
            }

            componentsOfType.Set(entity.EntityId, state);
        }

        //For debug only!
        public List<IState> GetStates(Entity entity)
        {
            var results = new List<IState>();
            foreach (var states in statesByType.Values)
            {
                if (states.Get(entity.EntityId) != null)
                {
                    results.Add(states.Get(entity.EntityId));
                }
            }
            return results;
        }


        public T GetState<T>([NotNull] Entity entity)
        {
            Bag<IState> statesOfType;
            var hasType = statesByType.TryGetValue(typeof(T), out statesOfType);
            if (hasType)
            {
                return (T)statesByType[typeof(T)].Get(entity.EntityId);
            }
            return default(T);
        }

        private void RemoveStatesForEntity([NotNull] Entity entity)
        {
            foreach (var stateBag in statesByType.Values)
            {
                if (stateBag.Get(entity.EntityId) != null)
                {
                    stateBag.Set(entity.EntityId, null);
                }
            }
        }
    }
}
