using System;
using System.Collections.Generic;
using Assets.Framework.Entities;
using Assets.Framework.States;
using Assets.Framework.Systems;
using Assets.Scrips.States;
using UnityEngine;

namespace Assets.Scrips.Systems
{
    public class EntityLibrarySystem : IUpdateSystem
    {
        private readonly EntityLibraryState libraryState;

        public EntityLibrarySystem()
        {
            libraryState = StaticStates.Get<EntityLibraryState>();
        }

        //TODO: Make this unneeded.
        public List<Type> RequiredStates()
        {
            return new List<Type>();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                libraryState.UpdateSelectedLibraryIndex(libraryState.SelectedLibraryIndex - 1);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                libraryState.UpdateSelectedLibraryIndex(libraryState.SelectedLibraryIndex + 1);
            }
        }

        public void UpdateModulesFromDisk()
        {
            //            var moduleJson = DiskOperations.GetModules();
            //            foreach (var module in moduleJson)
            //            {
            //                AddEntityToLibrary(Entity.FromJson(module));
            //            }
        }


        private void AddEntityToLibrary(Entity entityToAdd)
        {
            throw new NotImplementedException();
            //            //Foreach support modification while iterating. 
            //            buildableEntityLibrary.ForEach(module =>
            //            {
            //                if (entityToAdd.Get<NameState>().Name == module.Get<NameState>().Name)
            //                {
            //                    buildableEntityLibrary.Remove(module);
            //                }
            //            });
            //            buildableEntityLibrary.Add(entityToAdd);
        }

    }
}