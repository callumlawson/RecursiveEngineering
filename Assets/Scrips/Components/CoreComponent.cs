using System;
using Assets.Scrips.Modules;

namespace Assets.Scrips.Components
{
    [Serializable]
    public class CoreComponent : IComponent
    {
        public string Name { get; set; }
        public int InternalWidth { get; set; }
        public int InteralHeight { get; set; }
        public ModuleType Type { get; set; }

        public CoreComponent(string name, int internalWidth, int interalHeight, ModuleType type)
        {
            Name = name;
            InternalWidth = internalWidth;
            InteralHeight = interalHeight;
            Type = type;
        }
    }
}
