﻿using System;
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
            //                if (entityToAdd.Get<EntityTypeState>().EntityType == module.Get<EntityTypeState>().EntityType)
            //                {
            //                    buildableEntityLibrary.Remove(module);
            //                }
            //            });
            //            buildableEntityLibrary.Add(entityToAdd);
        }

    }
}