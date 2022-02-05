using HarmonyLib;
using Kingmaker;
using Kingmaker.Controllers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.FullScreenUITypes;
using Kingmaker.View;
using Kingmaker.View.MapObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPauser
{
    public class AutoPauseExtender : ISceneHandler, IDialogFinishHandler, IPartyCombatHandler, IFullScreenUIHandler, ILootInterractionHandler
    {
        public static void Load()
        {
            EventBus.Subscribe(new AutoPauseExtender());
            Log.Write("AutoPauseExtender Got Loaded.");
        }

        void IDialogFinishHandler.HandleDialogFinished(bool success)
        {
            try
            {
                if(Main.Settings.AutoPauseOnDialogFinished)
                    Game.Instance.IsPaused = true;
//                Traverse.Create<AutoPauseController>().Method("Pause", Main.Settings.AutoPauseOnDialogFinished, null).GetValue<bool>();
#if DEBUG
                Log.Write("Dialog finished, Pause should be " + (Main.Settings.AutoPauseOnDialogFinished ? "enabled" : "disabled"));
#endif
            }
            catch (Exception e)
            {
                Log.Write("Error At Dialog Finished:");
                Log.Error(e);
            }
        }

        void IFullScreenUIHandler.HandleFullScreenUiChanged(bool state, FullScreenUIType fullScreenUIType)
        {
            try
            {
                if (Main.Settings.AutoPauseOnCharacterScreenOpened && state && fullScreenUIType == FullScreenUIType.ChracterScreen && Game.Instance.CurrentMode == Kingmaker.GameModes.GameModeType.Default)
                {
                    Game.Instance.IsPaused = true;
#if DEBUG
                    Log.Write("CharacterScreen closed, Pause should be enabled");
#endif
                }
            }
            catch (Exception e)
            {
                Log.Write("Error At Character Screen Closed:");
                Log.Error(e);
            }

            try
            {
                if (Main.Settings.AutoPauseOnLocalMapOpened && state && fullScreenUIType == FullScreenUIType.LocalMap && Game.Instance.CurrentMode == Kingmaker.GameModes.GameModeType.Default)
                {
                    Game.Instance.IsPaused = true;
#if DEBUG
                    Log.Write("Map opened, Pause should be enabled");
#endif
                }
            }
            catch (Exception e)
            {
                Log.Write("Error At Map Opened:");
                Log.Error(e);
            }

            try
            {
                if (Main.Settings.AutoPauseOnInventoryScreenOpened && state && fullScreenUIType == FullScreenUIType.Inventory && Game.Instance.CurrentMode == Kingmaker.GameModes.GameModeType.Default)
                {
                    Game.Instance.IsPaused = true;
#if DEBUG
                    Log.Write("Inventory Screen closed, Pause should be enabled");
#endif
                }
            }
            catch (Exception e)
            {
                Log.Write("Error At Inventory Screen:");
                Log.Error(e);
            }

        }

        void ILootInterractionHandler.HandleLootInterraction(UnitEntityData unit, ItemsCollection loot, string lootContainerName)
        {
            try
            {
                if (Main.Settings.AutoPauseOnLootWindowOpened)
                {
                    Game.Instance.IsPaused = true;
#if DEBUG
                    Log.Write("LootWindow opened, Pause should be enabled");
#endif
                }
            }
            catch (Exception e)
            {
                Log.Write("Error At On Loot Window Opened:");
                Log.Error(e);
            }
        }

        void ILootInterractionHandler.HandleLootInterraction(UnitEntityData unit, EntityViewBase[] objects, LootContainerType containerType, Action closeCallback)
        {
            try
            {
                if (Main.Settings.AutoPauseOnLootWindowOpened)
                {
                    Game.Instance.IsPaused = true;
#if DEBUG
                    Log.Write("LootWindow opened, Pause should be enabled");
#endif
                }
            }
            catch (Exception e)
            {
                Log.Write("Error At On Loot Window Opened:");
                Log.Error(e);
            }
        }

        void IPartyCombatHandler.HandlePartyCombatStateChanged(bool inCombat)
        {
            try
            {
                if (!inCombat && Main.Settings.AutoPauseOnBattleEnd)
                {
                    Game.Instance.IsPaused = true;
                    //Traverse.Create<AutoPauseController>().Method("Pause", !inCombat && Main.Settings.AutoPauseOnBattleEnd, null).GetValue<bool>();
#if DEBUG
                    Log.Write("CombatStateChanged, Pause should be " + (!inCombat && Main.Settings.AutoPauseOnBattleEnd ? "enabled" : "disabled"));
#endif
                }
            }
            catch (Exception e)
            {
                Log.Write("Error At Combat Finished:");
                Log.Error(e);
            }
        }

        void ILootInterractionHandler.HandleZoneLootInterraction(AreaTransition areaTransition)
        {
        }

        void ISceneHandler.OnAreaBeginUnloading()
        {
        }

        void ISceneHandler.OnAreaDidLoad()
        {
            try
            {
                if (Main.Settings.AutoPauseOnAreaLoad && Game.Instance.CurrentMode == Kingmaker.GameModes.GameModeType.Default)
                {
                    Game.Instance.IsPaused = true;
                    // Traverse.Create<AutoPauseController>().Method("Pause",  true || Main.Settings.AutoPauseOnAreaLoad, null).GetValue<bool>();
#if DEBUG
                    Log.Write("Area Loaded, Pause should be " + (Main.Settings.AutoPauseOnAreaLoad && Game.Instance.CurrentMode == Kingmaker.GameModes.GameModeType.Default ? "enabled" : "disabled"));
#endif
                }
            }
            catch (Exception e)
            {
                Log.Write("Error At Area Load:");
                Log.Error(e);
            }
        }
    }
}
