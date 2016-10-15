using System.Collections;
using System.Collections.Generic;
using Assets.Framework.Entities;
using Assets.Framework.States;
using Assets.Framework.Systems;
using Assets.Scrips.Datastructures;
using Assets.Scrips.Modules;
using Assets.Scrips.States;
using Assets.Scrips.Systems;
using Assets.Scrips.Util;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scrips
{
    //TODO: Extract input handler from game runner.
    public class GameRunner : MonoBehaviour
    {
        private EntityStateSystem entitySystem;

        [UsedImplicitly]
        public void Start()
        {
            entitySystem = new EntityStateSystem();

            var activeEntity = entitySystem.BuildEntity(
                new List<IState>
                {
                    new NameState("Sub Pen"),
                    new PhysicalState(null, new List<Entity>(), new GridCoordinate(0, 0), 1, 1, 28, 13)
                }
            );

            //TODO: this should be via entity System
            StaticStates.Add(new ActiveEntityState(activeEntity));
            StaticStates.Add(new GameModeState(GameMode.Design));
            StaticStates.Add(new EntityLibraryState(InitialBuildableEntities.BuildableEntityLibrary));
            StaticStates.Add(new SelectedState());

            entitySystem.AddSystem(new PlayerEntityModificationSystem());
            entitySystem.AddSystem(new EngineSystem());
            entitySystem.AddSystem(new SubstanceNetworkSystem());
            entitySystem.AddSystem(new EntityLibrarySystem());
            entitySystem.AddSystem(new SaveLoadSystem());

            StartCoroutine(Ticker());
        }

        [UsedImplicitly]
        public void Update()
        {
            entitySystem.Update();
        }

        private IEnumerator Ticker()
        {
            while (true)
            {
                entitySystem.Tick();
                yield return new WaitForSeconds(GlobalConstants.TickPeriodInSeconds);
            }
        }
    }
}