using HarmonyLib;
using Kingmaker;
using Kingmaker.Controllers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPauser
{
    public class AutoPauseExtender : ISceneHandler, IDialogFinishHandler, IPartyCombatHandler
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

        void IPartyCombatHandler.HandlePartyCombatStateChanged(bool inCombat)
        {
            try
            {
                if (!inCombat && Main.Settings.AutoPauseOnBattleEnd)
                    Game.Instance.IsPaused = true;
                //Traverse.Create<AutoPauseController>().Method("Pause", !inCombat && Main.Settings.AutoPauseOnBattleEnd, null).GetValue<bool>();
#if DEBUG
                Log.Write("CombatStateChanged, Pause should be " + (!inCombat && Main.Settings.AutoPauseOnBattleEnd ? "enabled" : "disabled"));
#endif
            }
            catch (Exception e)
            {
                Log.Write("Error At Combat Finished:");
                Log.Error(e);
            }
        }

        void ISceneHandler.OnAreaBeginUnloading()
        {
        }

        void ISceneHandler.OnAreaDidLoad()
        {
            try
            {
                if (Main.Settings.AutoPauseOnAreaLoad)
                    Game.Instance.IsPaused = true;
                // Traverse.Create<AutoPauseController>().Method("Pause",  true || Main.Settings.AutoPauseOnAreaLoad, null).GetValue<bool>();
#if DEBUG
                Log.Write("Area Loaded, Pause should be " +(Main.Settings.AutoPauseOnAreaLoad ? "enabled" : "disabled"));
#endif
            }
            catch (Exception e)
            {
                Log.Write("Error At Area Load:");
                Log.Error(e);
            }
        }
    }
}
