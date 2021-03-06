﻿using System;
using System.Collections.Generic;
using Assets.Framework.States;
using Newtonsoft.Json;

//TODO: Delete this. Not needed as an actual object! 
namespace Assets.Scrips.Modules
{
    [Serializable]
    public class Module
    {
        [JsonProperty]
        private readonly GridOperations gridOperations;
        [JsonProperty]
        public readonly List<IState> Components;

//        public void AddModule(Module module, GridCoordinate grid)
//        {
//            if (GridOperations.AddModule(module, grid))
//            {
//                AddModuleToSubstanceNetwork(module);
//                CheckForConnections(module);
//            }
//        }
//
//        public void RemoveModule(Module moduleToRemove)
//        {
//            var removedModule = GridOperations.RemoveModule(moduleToRemove);
//            if (removedModule != null)
//            {
//                RemoveModuleFromSubstanceNetwork(removedModule);
//            }
//        }
//
//        public void RemoveModule(GridCoordinate grid)
//        {
//            var removedModule = GridOperations.RemoveModule(grid);
//            if (removedModule != null)
//            {
//                RemoveModuleFromSubstanceNetwork(removedModule);
//            }
//        }

//        public Module GetModule(GridCoordinate grid)
//        {
//            return gridOperations.GetModule(grid);
//        }

        //VERY SLOW
        public T GetState<T>() where T : IState
        {
            foreach (var component in Components)
            {
                if (component.GetType() == typeof(T))
                {
                    return (T)component;
                }
            }
            return default(T);
        }

//        public GridCoordinate GetGridPosition()
//        {
//            return IsTopLevelModule ? new GridCoordinate(0, 0) : ParentModule.GetGridForContainedModule(this);
//        }

//        public GridCoordinate GetGridForContainedModule(Module module)
//        {
//            return gridOperations.GetGridForModule(module);
//        }

//        public static string ToJson(Module module)
//        {
//            var settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects };
//            return JsonConvert.SerializeObject(module, Formatting.Indented, settings);
//        }
//
//        //TODO: Sort out this horror. Use a custom deserializer perhaps to add things the "Correct way".
//        public static Module FromJson(string json)
//        {
//            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
//            var module = JsonConvert.DeserializeObject<Module>(json, settings);
//            FixupChildParents(module);
//            AddModuleToSubstanceNetwork(module);
//            CheckForConnections(module);
//            return module;
//        }

//        private static void FixupChildParents(Module module)
//        {
//            foreach (var innerModule in module.GetContainedModules())
//            {
//                innerModule.ParentModule = module;
//                FixupChildParents(innerModule);
//            }
//        }

//        private static void AddModuleToSubstanceNetwork(Module module)
//        {
//            SubstanceNetworkSystem.Instance.AddEntityToNetwork(module);
//            foreach (var innerModule in module.GetContainedModules())
//            {
//                AddModuleToSubstanceNetwork(innerModule);
//            }
//        }
//
//        private static void RemoveModuleFromSubstanceNetwork(Module module)
//        {
//            SubstanceNetworkSystem.Instance.RemoveModuleFromNetwork(module);
//            foreach (var innerModule in module.GetContainedModules())
//            {
//                RemoveModuleFromSubstanceNetwork(innerModule);
//            }
//        }
//
//        private static void CheckForConnections(Module module)
//        {
//            SubstanceNetworkSystem.Instance.ConnectModule(module);
//            foreach (var innerModule in module.GetContainedModules())
//            {
//                CheckForConnections(innerModule);
//            }
//        }

//        public List<Module> GetContainedModules()
//        {
//            return GridOperations.GetContainedModules();
//        }
//
//        public List<Module> GetNeighbouringModules(GridCoordinate grid)
//        {
//            return GridOperations.GetNeigbouringEntities(grid).ToList();
//        }
    }
}