using TaleWorlds.MountAndBlade;
using BetterHUD.Behavior;
using BetterHUD.Utils;

namespace BetterHUD {
	public class SubModule : MBSubModuleBase {

		protected override void OnSubModuleLoad() {
			base.OnSubModuleLoad();
		}

		protected override void OnBeforeInitialModuleScreenSetAsRoot() {
			base.OnBeforeInitialModuleScreenSetAsRoot();
            //new Harmony("Bannerlord.Shadow.BetterCombat").PatchAll();
            Helper.SetModName("BetterHUD");
		}

        public override void OnMissionBehaviorInitialize(Mission mission) {
            base.OnMissionBehaviorInitialize(mission);

			mission.AddMissionBehavior(new HudManager());
		}
    }
}
