using System;

namespace Assets.Scrips.Components
{
    [Serializable]
    public class CoreComponent : IComponent
    {
        public string Name { get; set; }
        public int InternalWidth { get; set; }
        public int InteralHeight { get; set; }

        public CoreComponent(string name, int internalWidth, int interalHeight)
        {
            Name = name;
            InternalWidth = internalWidth;
            InteralHeight = interalHeight;
        }
    }
}
