using KitchenLib.Customs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplianceLib.Api;
using ApplianceLib.Api.References;
using Kitchen;
using KitchenData;
using static KitchenData.ItemGroup;
using UnityEngine;
using KitchenLib.Utils;
using KitchenLib.References;
using ApplianceLib.Api.Prefab;

namespace KitchenSloppySteaks
{
    class WaterCup : CustomItemGroup
    {
        public override string UniqueNameID => "Water Cup";
        public override GameObject Prefab => PrefabBuilder.CreateEmptyPrefab("Water Cup");
        public override ItemCategory ItemCategory => ItemCategory.Generic;
        public override ItemStorage ItemStorageFlags => ItemStorage.StackableFood;
        public override ItemValue ItemValue => ItemValue.Small;
        /*public override int SplitCount => 1;
        public override float SplitSpeed => 1;
        public override bool AllowSplitMerging => true;
        public override bool PreventExplicitSplit => true;
        public override bool SplitByComponents => true;
        public override Item SplitByComponentsHolder => ApplianceLibGDOs.Refs.Cup;*/


        public override List<ItemSet> Sets => new List<ItemSet>()
        {
            new ItemSet()
            {
                Max = 2,
                Min = 2,
                IsMandatory = true,
                Items = new List<Item>()
                {
                    (Item)GDOUtils.GetExistingGDO(ItemReferences.Water),
                    ApplianceLibGDOs.Refs.Cup
                }
            }
        };

        public override void OnRegister(GameDataObject gameDataObject)
        {
            Prefab.AttachCup(MaterialUtils.GetExistingMaterial("Water"), false);
        }
    }
}
