﻿using System;
using System.Collections.Generic;
using Assets.Framework.Entities;

namespace Assets.Framework.Systems
{
    public interface ITickEntitySystem : IFilteredSystem
    {
        void Tick(List<Entity> matchingEntities);
    }
}
