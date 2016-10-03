using System;

namespace Assets.Scrips.Components
{
    [Serializable]
    public class CoreComponent : IComponent
    {
        public string Name { get; set; }
        public int InternalWidth { get; set; }
        public int InteralHeight { get; set; }

        //TODO(Refactor): Add Parent Component, child components[,], Grid coordinates. Delete width and height. 

        public CoreComponent(string name, int internalWidth, int interalHeight)
        {
            Name = name;
            InternalWidth = internalWidth;
            InteralHeight = interalHeight;
        }
    }
}
