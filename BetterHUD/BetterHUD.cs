using BetterCore.Utils;
using BetterHUD.Behavior;
using BetterHUD.Settings;
using HarmonyLib;
using System;
using TaleWorlds.MountAndBlade;

namespace BetterHUD {
    public class BetterHUD : MBSubModuleBase {

        public static MCMSettings Settings { get; private set; } = new MCMSettings();
        public static string ModName { get; private set; } = "BetterHUD";

        private bool isInitialized = false;
        private bool isLoaded = false;

        //FIRST
        protected override void OnSubModuleLoad() {
            try {
                base.OnSubModuleLoad();

                if (isInitialized)
                    return;

                Harmony h = new("Bannerlord.Shadow." + ModName);

                h.PatchAll();

                isInitialized = true;
            } catch (Exception e) {
                NotifyHelper.WriteError(ModName, "OnSubModuleLoad threw exception: " + e);
            }
        }

        //SECOND
        protected override void OnBeforeInitialModuleScreenSetAsRoot() {
            try {
                base.OnBeforeInitialModuleScreenSetAsRoot();

                if (isLoaded)
                    return;

                ModName = base.GetType().Assembly.GetName().Name;

                Settings = MCMSettings.Instance ?? throw new NullReferenceException("Settings are null");

                NotifyHelper.WriteMessage(ModName + " Loaded.", MsgType.Good);
                Integrations.BetterHealthLoaded = true;

                isLoaded = true;
            } catch (Exception e) {
                NotifyHelper.WriteError(ModName, "OnBeforeInitialModuleScreenSetAsRoot threw exception: " + e);
            }
        }

        public override void OnMissionBehaviorInitialize(Mission mission) {
            try {
                base.OnMissionBehaviorInitialize(mission);

                mission.AddMissionBehavior(new HudManager());
            } catch (Exception e) {
                NotifyHelper.WriteError(ModName, "OnMissionBehaviorInitialize threw exception: " + e);
            }
		}
    }
}
