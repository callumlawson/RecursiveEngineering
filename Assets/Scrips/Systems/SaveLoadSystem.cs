using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Framework.Systems;

namespace Assets.Scrips.Systems
{
    class SaveLoadSystem : IUpdateSystem
    {
        public void Update()
        {
            //            if (Input.GetKeyDown(KeyCode.O))
            //            {
            //                acceptingInput = false;
            //                UserTextQuery.Instance.GetTextResponse("Entity to load...", LoadModule);
            //            }
            //
            //            if (Input.GetKeyDown(KeyCode.P))
            //            {
            //                acceptingInput = false;
            //                UserTextQuery.Instance.GetTextResponse("Part to save...", SaveModule);
            //            }
        }

        //        //TODO: Actual error handling.
        //        private void LoadModule(string moduleName)
        //        {
        //            acceptingInput = true;
        //            var module = Entity.FromJson(DiskOperations.ReadText(moduleName));
        //            ActiveEntity = entityManager.BuildEntity(module.Components);
        //            ActiveModule = module;
        //        }
        //
        //        private void SaveModule(string moduleName)
        //        {
        //            acceptingInput = true;
        //            DiskOperations.SaveText(moduleName, Entity.ToJson(ActiveModule));
        //            EntityLibrarySystem.Instance.UpdateModulesFromDisk();
        //        }
    }
}
