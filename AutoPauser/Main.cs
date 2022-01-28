using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;
using HarmonyLib;
using UnityEngine;

namespace AutoPauser
{
#if DEBUG
    [EnableReloading]
#endif

    public class Main
    {
        public static UnityModManager.ModEntry.ModLogger logger;

        static Settings settings;

        public static Settings Settings { get => settings; }

        public static bool enabled;


        static Harmony harmonyInstance;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            logger = modEntry.Logger;
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
#if DEBUG
            modEntry.OnUnload = Unload;
#endif

            settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
            harmonyInstance = new Harmony(modEntry.Info.Id);
            
            StartMod();
            return true;
        }

#if DEBUG

        private static bool Unload(UnityModManager.ModEntry modEntry)
        {
            Settings.Save(modEntry);
            return true;
        }
#endif

        private static void StartMod()
        {
            SafeLoad(AutoPauseExtender.Load);
        }

        static void SafeLoad(Action load)
        {
            try
            {
                load();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Settings.Save(modEntry);
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            if (!enabled) 
                return;

            var fixedWidth = new GUILayoutOption[1] { GUILayout.ExpandWidth(false) };

            GUILayout.BeginVertical();
            GUILayout.Label("<b>Current Settings:</b>", fixedWidth);
            settings.AutoPauseOnAreaLoad = GUILayout.Toggle(settings.AutoPauseOnAreaLoad, "Auto Pause on Area Load", fixedWidth);
            settings.AutoPauseOnBattleEnd = GUILayout.Toggle(settings.AutoPauseOnBattleEnd, "Auto Pause on Battle End", fixedWidth);
            settings.AutoPauseOnDialogFinished = GUILayout.Toggle(settings.AutoPauseOnDialogFinished, "Auto Pause on finished Dialog", fixedWidth);
            GUILayout.EndVertical();
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }
    }
}
