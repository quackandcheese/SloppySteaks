using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ApplianceLib.Api.References.ApplianceLibGDOs;
using KitchenLib.References;
using ApplianceLib.Api.References;

namespace KitchenSloppySteaks
{
    class SloppySteakCard : CustomDish
    {
        public override string UniqueNameID => "Sloppy Steaks";
        public override DishType Type => DishType.Extra;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override CardType CardType => CardType.Default;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Medium;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsSpecificFranchiseTier => false;
        public override bool DestroyAfterModUninstall => false;
        public override bool IsUnlockable => true;

        public override List<Unlock> HardcodedRequirements => new()
        {
            (Dish)GDOUtils.GetExistingGDO(DishReferences.Steak)
        };

        public override HashSet<Dish.IngredientUnlock> IngredientsUnlocks => new HashSet<Dish.IngredientUnlock>
        {
            new Dish.IngredientUnlock
            {
                Ingredient = GDOUtils.GetCastedGDO<ItemGroup, WaterCup>(),
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.SteakPlated)
            },
            new Dish.IngredientUnlock
            {
                Ingredient = GDOUtils.GetCastedGDO < ItemGroup, WaterCup >(),
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.ThinSteakPlated)
            },
            new Dish.IngredientUnlock
            {
                Ingredient = GDOUtils.GetCastedGDO < ItemGroup, WaterCup >(),
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.ThickSteakPlated)
            },
            new Dish.IngredientUnlock
            {
                Ingredient = GDOUtils.GetCastedGDO < ItemGroup, WaterCup >(),
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.BonedSteakPlated)
            }
        };

        public override HashSet<Item> MinimumIngredients => new HashSet<Item>
        {
            ApplianceLibGDOs.Refs.Cup,
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Water)
        };
        public override HashSet<Process> RequiredProcesses => new HashSet<Process>
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Cook)
        };

        public override Dictionary<Locale, string> Recipe => new Dictionary<Locale, string>
        {
            { Locale.English, "Dump water on a big rare cut of meat." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            ( Locale.English, LocalisationUtils.CreateUnlockInfo("Sloppy Steaks", "It's a steak with water dumped on it, it's really really good", "Let's slop 'em up!") )
        };
    }
}
