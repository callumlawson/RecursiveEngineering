using System;
using System.Collections.Generic;
using Assets.Scrips.Components;
using Assets.Scrips.Util;
using Newtonsoft.Json;

namespace Assets.Scrips.Modules
{
    [Serializable]
    public class Module
    {
        [JsonProperty]
        public ModuleGrid ModuleGrid;
        [JsonProperty]
        private readonly List<IComponent> components;
        [JsonIgnore]
        public Module ParentModule;

        [JsonIgnore]
        public bool IsTerminalModule
        {
            get { return GetComponent<CoreComponent>().InternalWidth == 0 || GetComponent<CoreComponent>().InteralHeight == 0; }
        }

        [JsonIgnore]
        public bool IsTopLevelModule
        {
            get { return ParentModule == null; }
        }

        [JsonConstructor]
        public Module()
        {
            
        }

        public Module(Module parentModule, List<IComponent> components)
        {
            this.components = components;
            ParentModule = parentModule;
            ModuleGrid = new ModuleGrid(GetComponent<CoreComponent>().InternalWidth, GetComponent<CoreComponent>().InteralHeight);
        }

        public bool AddModule(Module module, GridCoordinate grid)
        {
            return ModuleGrid.AddModule(module, grid);
        }

        public void RemoveModule(GridCoordinate grid)
        {
            ModuleGrid.RemoveModule(grid);
        }

        public Module GetModule(GridCoordinate grid)
        {
            return ModuleGrid.GetModule(grid);
        }

        public T GetComponent<T>() where T : IComponent
        {
            foreach (var component in components)
            {
                if (component.GetType() == typeof(T))
                {
                    return component as T;
                }
            }
            return null;
        }

        public GridCoordinate GetGridPosition()
        {
            return IsTopLevelModule ? new GridCoordinate(0, 0) : ParentModule.GetGridForContainedModule(this);
        }

        public GridCoordinate GetGridForContainedModule(Module module)
        {
            return ModuleGrid.GetGridForModule(module);
        }

        public bool GridIsEmpty(GridCoordinate grid)
        {
            return ModuleGrid.GridIsEmpty(grid);
        }

        public static string ToJson(Module module)
        {
            var settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects };
            return JsonConvert.SerializeObject(module, Formatting.Indented, settings);
        }

        public static Module FromJson(string json)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            var module = JsonConvert.DeserializeObject<Module>(json, settings);
            FixupChildParents(module);
            return module;
        }

        private static void FixupChildParents(Module module)
        {
            foreach (var innerModule in module.GetContainedModules())
            {
                innerModule.ParentModule = module;
                FixupChildParents(innerModule);
            }
        }

        public List<Module> GetContainedModules()
        {
            return ModuleGrid.GetContainedModules();
        }
    }
}