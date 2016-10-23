using System.Collections;
using System.Collections.Generic;
using Assets.Framework.States;
using Assets.Framework.Systems;
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
        public void Awake()
        {
            entitySystem = new EntityStateSystem();

            //TODO: this should be via the entity System. Fix this before save load.
            StaticStates.Add(new WorldEntityState(entitySystem.BuildEntity(new List<IState>())));
            StaticStates.Add(new ActiveEntityState(entitySystem.BuildEntity(new List<IState>())));
            StaticStates.Add(new GameModeState(GameMode.Design));
            StaticStates.Add(new EntityLibraryState(InitialBuildableEntities.BuildableEntityLibrary));
            StaticStates.Add(new SelectedState());

            entitySystem.AddSystem(new WorldInitSystem());
            entitySystem.AddSystem(new GlobalControlsSystem());
            entitySystem.AddSystem(new PlayerEntityModificationSystem());
            entitySystem.AddSystem(new EngineSystem());
            entitySystem.AddSystem(new SubstanceNetworkSystem());
            entitySystem.AddSystem(new EntityLibrarySystem());
            entitySystem.AddSystem(new SaveLoadSystem());
            entitySystem.AddSystem(new CrewMovementSystem());
            entitySystem.AddSystem(new CrewHealthSystem());
            entitySystem.AddSystem(new SeaSystem());

            entitySystem.Init();
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