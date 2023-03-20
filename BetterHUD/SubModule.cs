using TaleWorlds.MountAndBlade;
using BetterHUD.Behavior;
using BetterHUD.Utils;
using BetterHUD.Settings;

namespace BetterHUD {
	public class SubModule : MBSubModuleBase {

		protected override void OnSubModuleLoad() {
			base.OnSubModuleLoad();
		}

		protected override void OnBeforeInitialModuleScreenSetAsRoot() {
			base.OnBeforeInitialModuleScreenSetAsRoot();

            string modName = base.GetType().Assembly.GetName().Name;

            Helper.SetModName(modName);
            Helper.settings = SettingsManager.Instance;
        }

        public override void OnMissionBehaviorInitialize(Mission mission) {
            base.OnMissionBehaviorInitialize(mission);

			mission.AddMissionBehavior(new HudManager());
		}
    }
}
