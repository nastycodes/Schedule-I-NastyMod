using NastyMod_v2.UI;
using MelonLoader;
using UnityEngine;

namespace NastyMod_v2
{
    /**
     * Core
     * 
     * This is the core class for the NastyMod project.
     * 
     * Author: nastycodes
     * Version: 1.0.0
     */
    public class NastyCore : MelonMod
    {
        // Harmony instance for patching
        private HarmonyLib.Harmony xHarmony;

        // Helper instance
        public static Core.Helper HelperInstance { get; private set; } = new Core.Helper();

        // Menu instance
        public Menu MenuInstance;

        /**
         * OnInitializeMelon
         * 
         * Called after the Melon was registered. This callback waits until MelonLoader has fully initialized
         * It is safe to make any game/Unity references from and after this callback.
         * 
         * @return void
         */
        public override void OnInitializeMelon()
        {
            // Log initialization message
            HelperInstance.SendLoggerMsg("Initializing NastyMod v2");

            // Set up Harmony
            xHarmony = new HarmonyLib.Harmony("com.nastymod.schedulei");
            xHarmony.PatchAll();
        }

        /**
         * OnSceneWasInitialized
         * 
         * Called once the active Scene is fully initialized.
         * 
         * @param buildIndex The build index of the scene.
         * @param sceneName The name of the scene.
         * @return void
         */
        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (sceneName == "Main")
            {
                MenuInstance = new Menu();
                MenuInstance.SetInitialized();
            }
        }

        /**
         * OnUpdate
         * 
         * Called once per frame.
         * 
         * @return void
         */
        public override void OnUpdate()
        {
            if (MenuInstance != null)
            {
                if (MenuInstance.IsInitialized()) MenuInstance.OnUpdate();

                if ((Input.GetKeyDown(MenuInstance.GetToggleHotkey()) || Input.GetKeyDown(MenuInstance.GetAltToggleHotkey())) && MenuInstance.IsInitialized()) MenuInstance.ToggleVisible();
            }
        }

        /**
         * OnGUI
         * 
         * Called once per frame to render and handle GUI events.
         * 
         * @return void
         */
        public override void OnGUI()
        {
            if (MenuInstance != null)
            {
                if (MenuInstance.IsInitialized()) MenuInstance.OnGUI();

                if (MenuInstance.IsVisible()) MenuInstance.Render();
            }
        }
    }
}
