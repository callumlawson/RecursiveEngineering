using System;
using System.Collections.Generic;

namespace Assets.Framework.States
{
    //TODO: This isn't a great patern but can be replaced with injection later.
    //This is also a special case when saving... which is a disadvantage. 
    public static class StaticStates
    {
        private static readonly List<IState> States = new List<IState>();

        public static void Add(IState state)
        {
            States.Add(state);
        }
   
        public static T Get<T>() where T : IState
        {
            foreach (var state in States)
            {
                if (state.GetType() == typeof(T))
                {
                    return (T)state;
                }
            }
            throw new InvalidOperationException(string.Format("Static state with type {0} requested but not found!", typeof(T)));
        }
    }
}
