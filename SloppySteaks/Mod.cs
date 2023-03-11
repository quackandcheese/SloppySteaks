using Kitchen;
using KitchenData;
using KitchenLib;
using KitchenLib.Event;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMods;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System.Collections;
using HarmonyLib;
using UnityExplorer.CacheObject;

// Namespace should have "Kitchen" in the beginning
namespace KitchenSloppySteaks
{
    public class Mod : BaseMod, IModSystem
    {
        // GUID must be unique and is recommended to be in reverse domain name notation
        // Mod Name is displayed to the player and listed in the mods menu
        // Mod Version must follow semver notation e.g. "1.2.3"
        public const string MOD_GUID = "QuackAndCheese.PlateUp.SloppySteaks";
        public const string MOD_NAME = "Sloppy Steaks";
        public const string MOD_VERSION = "0.1.0";
        public const string MOD_AUTHOR = "QuackAndCheese";
        public const string MOD_GAMEVERSION = ">=1.1.4";
        // Game version this mod is designed for in semver
        // e.g. ">=1.1.3" current and all future
        // e.g. ">=1.1.3 <=1.2.3" for all from/until

        // Boolean constant whose value depends on whether you built with DEBUG or RELEASE mode, useful for testing
#if DEBUG
        public const bool DEBUG_MODE = true;
#else
        public const bool DEBUG_MODE = false;
#endif

        public static AssetBundle Bundle;
        public static Item platedSteak = (Item)GDOUtils.GetExistingGDO(ItemReferences.SteakPlated);


        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        protected override void OnInitialise()
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
        }

        private void AddGameData()
        {
            LogInfo("Attempting to register game data...");

            AddGameDataObject<WaterCup>();
            AddGameDataObject<SloppySteakCard>();

            LogInfo("Done loading game data.");
        }

        
        protected override void OnUpdate()
        {

        }

        private bool colorblindSetup = false;
        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            LogInfo("Attempting to load asset bundle...");
            Bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).First();
            LogInfo("Done loading asset bundle.");

            // Register custom GDOs
            AddGameData();

            // Perform actions when game data is built
            Events.BuildGameDataEvent += delegate (object s, BuildGameDataEventArgs args)
            {
                ItemGroup steakPlated = args.gamedata.Get<ItemGroup>(ItemGroupReferences.SteakPlated);
                GameObject waterPrefab = Mod.Bundle.LoadAsset<GameObject>("Water");

                waterPrefab.transform.parent = steakPlated.Prefab.GetChild("Sauce").transform;
                waterPrefab.SetActive(false);

                steakPlated.Prefab.GetChildFromPath("Sauce/Water/Water").ApplyMaterialToChild("avocado", "Water");
                

                if (!colorblindSetup)
                {
                    steakPlated.Prefab.GetChildFromPath("Sauce/Water/Colorblind Parent").AddItemColorblindLabel("Wa");

                    colorblindSetup = true;
                }




                // Itemsets
                ItemGroupView SteakView = steakPlated.Prefab.GetComponent<ItemGroupView>();
                ItemGroup WaterCup = GDOUtils.GetCastedGDO<ItemGroup, WaterCup>();
                Item Water = args.gamedata.Get<Item>(ItemReferences.Water);

                steakPlated.DerivedSets.ElementAt(3).Items.Add(WaterCup);
                args.gamedata.Get<ItemGroup>(ItemGroupReferences.ThickSteakPlated).DerivedSets.ElementAt(3).Items.Add(WaterCup);
                args.gamedata.Get<ItemGroup>(ItemGroupReferences.ThinSteakPlated).DerivedSets.ElementAt(3).Items.Add(WaterCup);
                args.gamedata.Get<ItemGroup>(ItemGroupReferences.BonedSteakPlated).DerivedSets.ElementAt(3).Items.Add(WaterCup);

                ComponentAccesserUtil.AddComponent(SteakView, (WaterCup, steakPlated.Prefab.GetChildFromPath("Sauce/Water")));
            };
        }

        internal class ComponentAccesserUtil : ItemGroupView
        {
            private static FieldInfo componentGroupField = ReflectionUtils.GetField<ItemGroupView>("ComponentGroups");

            public static void AddComponent(ItemGroupView viewToAddTo, params (Item item, GameObject gameObject)[] addedGroups)
            {
                List<ComponentGroup> componentGroups = componentGroupField.GetValue(viewToAddTo) as List<ComponentGroup>;
                foreach (var group in addedGroups)
                {
                    componentGroups.Add(new()
                    {
                        Item = group.item,
                        GameObject = group.gameObject
                    });
                }
                componentGroupField.SetValue(viewToAddTo, componentGroups);
            }
        }

        #region Logging
        public static void LogInfo(string _log) { Debug.Log($"[{MOD_NAME}] " + _log); }
        public static void LogWarning(string _log) { Debug.LogWarning($"[{MOD_NAME}] " + _log); }
        public static void LogError(string _log) { Debug.LogError($"[{MOD_NAME}] " + _log); }
        public static void LogInfo(object _log) { LogInfo(_log.ToString()); }
        public static void LogWarning(object _log) { LogWarning(_log.ToString()); }
        public static void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }
}
