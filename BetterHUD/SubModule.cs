using TaleWorlds.MountAndBlade;
using BetterHUD.Behavior;
using BetterCore.Utils;
using BetterHUD.Settings;

namespace BetterHUD {
	public class SubModule : MBSubModuleBase {

		public static MCMSettings _settings;
	

		protected override void OnBeforeInitialModuleScreenSetAsRoot() {
			base.OnBeforeInitialModuleScreenSetAsRoot();

            string modName = base.GetType().Assembly.GetName().Name;

            Helper.SetModName(modName);
            _settings = MCMSettings.Instance;
        }

        public override void OnMissionBehaviorInitialize(Mission mission) {
            base.OnMissionBehaviorInitialize(mission);

			mission.AddMissionBehavior(new HudManager());
		}
    }
}
