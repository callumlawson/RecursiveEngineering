using System.Collections.Generic;
using Assets.Scrips.Components;
using Assets.Scrips.Util;
using UnityEngine;

namespace Assets.Scrips.Modules
{
    public class Module
    {
        public readonly ModuleGrid ModuleGrid;
        public readonly Module ParentModule;
        private readonly List<IComponent> components;

        public Module()
        {
            components = new List<IComponent> { new CoreComponent("Dummy", 0 ,0 , ModuleType.Container)};
        }

        public Module(Module parentModule, List<IComponent> components)
        {
            this.components = components;
            ParentModule = parentModule;
            ModuleGrid = new ModuleGrid(GetComponent<CoreComponent>().InternalWidth, GetComponent<CoreComponent>().InteralHeight);
        }

        public bool IsTerminalModule
        {
            get { return GetComponent<CoreComponent>().InternalWidth == 0 || GetComponent<CoreComponent>().InteralHeight == 0; }
        }

        public bool AddModule(Module module, GridCoordinate grid)
        {
            return ModuleGrid.AddComponent(module, grid);
        }

        public Module GetModule(GridCoordinate grid)
        {
            return ModuleGrid.GetComponent(grid);
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

        public GridCoordinate GetGridForModule(Module module)
        {
            return ModuleGrid.GetGridForComponent(module);
        }

        public GridCoordinate GetCenterGrid()
        {
            return new GridCoordinate(Mathf.RoundToInt(GetComponent<CoreComponent>().InternalWidth / 2.0f), Mathf.RoundToInt(GetComponent<CoreComponent>().InteralHeight / 2.0f));
        }

        public bool GridIsEmpty(GridCoordinate grid)
        {
            return ModuleGrid.GridIsEmpty(grid);
        }
    }
}