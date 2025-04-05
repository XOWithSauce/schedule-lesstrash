using MelonLoader;
using System.Collections;
using System.Reflection;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Police;
using ScheduleOne.NPCs;
using UnityEngine;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.Trash;
[assembly: MelonInfo(typeof(LessTrash.LessTrash), LessTrash.BuildInfo.Name, LessTrash.BuildInfo.Version, LessTrash.BuildInfo.Author, LessTrash.BuildInfo.DownloadLink)]
[assembly: MelonColor()]
[assembly: MelonOptionalDependencies("FishNet.Runtime")]
[assembly: MelonGame(null, null)]

namespace LessTrash
{
    public static class BuildInfo
    {
        public const string Name = "LessTrash";
        public const string Description = "Trash and Garbage are now optimized! World generates 0 trash!";
        public const string Author = "XOWithSauce";
        public const string Company = null;
        public const string Version = "1.0";
        public const string DownloadLink = null;
    }
    public class LessTrash : MelonMod
    {
        TrashManager[] mgrs;
        List<object> coros = new();
        public override void OnApplicationStart()
        {
            MelonLogger.Msg("LessTrash Mod has been loaded");
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (buildIndex == 1)
            {
                if (this.mgrs == null || this.mgrs.Length == 0)
                    this.mgrs = UnityEngine.Object.FindObjectsOfType<TrashManager>(true);

                coros.Add(MelonCoroutines.Start(this.OverrideTrashManager()));
                coros.Add(MelonCoroutines.Start(this.TrashRoutine()));
            } else
            {
                foreach(object coro in coros)
                {
                    MelonCoroutines.Stop(coro);
                }
                coros.Clear();
            }
        }

        private IEnumerator OverrideTrashManager()
        {
            foreach (TrashManager mgmt in mgrs)
            {
                yield return new WaitForSeconds(1f);
                mgmt.GenerateableTrashItems = new TrashManager.TrashItemData[0];
            }
            yield return null;
        }

        private IEnumerator TrashRoutine()
        {
            for (; ; )
            {
                yield return new WaitForSeconds(60f);
                TrashItem[] trash = UnityEngine.Object.FindObjectsOfType<TrashItem>(true);
                //MelonLogger.Msg($"Running Trash cleanup routine for {trash.Length} items");
                foreach (TrashItem item in trash)
                {
                    yield return new WaitForSeconds(1f);
                    item.DestroyTrash();
                }
            }
        }
    }
}
