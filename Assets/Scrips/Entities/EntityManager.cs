using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Assets.Scrips.Components;
using Assets.Scrips.Util;
using JetBrains.Annotations;

namespace Assets.Scrips.Entities
{
    public class EntityManager
    {
        private readonly Bag<Entity> entities;

        private readonly Dictionary<Type, Bag<IState>> statesByType;

        private int nextAvailableId;

        //Premature optimisation is the devils work...
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
            return entity;
        }

        public Entity CreateEmptyEntity()
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

        public T GetState<T>(Entity entity)
        {
            var componentsOfType = statesByType[typeof(T)];
            return componentsOfType != null ? (T)componentsOfType.Get(entity.EntityId) : default(T);
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
