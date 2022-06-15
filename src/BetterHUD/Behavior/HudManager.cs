using System;
using BetterHUD.Utils;
using BetterHUD.ViewModels;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.ScreenSystem;

namespace BetterHUD.Behavior {
	class HudManager : MissionBehavior {

		public override MissionBehaviorType BehaviorType => MissionBehaviorType.Other;

		MissionTime hitUpdateDisplay, noncriticalUpdateDiplay, enemyStatusDisplayTime, healthChecker;

		float lastHealth;
		float displayedDamage;

		private GauntletLayer layer;
		public ScreenBase sbase;

		private ImprovedHudViewModel datasource;

		public override void OnMissionTick(float dt) {
			base.OnMissionTick(dt);

			try {
				//Check if we are in a mission
				if (Mission.Current != null) {
					if (Mission.Current.MainAgent != null) {
						if (hitUpdateDisplay.IsPast) {
							displayedDamage = 0;
							HandleHealthUpdates();
						}

						if (enemyStatusDisplayTime.IsPast) {
							datasource.EnemyShowStatus = false;
                        }

						if (noncriticalUpdateDiplay.IsPast) {
							NonCriticalUpdate();
							noncriticalUpdateDiplay = MissionTime.SecondsFromNow(5);
                        }

						if (healthChecker.IsPast) {
							if (lastHealth != Mission.MainAgent.Health) {
								HandleHealthUpdates();
							}

							healthChecker = MissionTime.SecondsFromNow(1);
                        }
					}
				} else {
					hitUpdateDisplay = MissionTime.Zero;
					enemyStatusDisplayTime = MissionTime.Zero;
					noncriticalUpdateDiplay = MissionTime.Zero;
					healthChecker = MissionTime.Zero;
				}
			} catch (Exception e) {
				Helper.WriteToLog("Problem with hud manager, cause: " + e);
			}
		}

        public override void AfterStart() {

			this.layer = new GauntletLayer(100, "GauntletLayer", true);

			datasource = new ImprovedHudViewModel();

			//Name of GUI xmls.
			this.layer.LoadMovie("ImprovedHUD", datasource);

			this.sbase = ScreenManager.TopScreen;
			this.sbase.AddLayer(this.layer);
			ScreenManager.TrySetFocus(this.layer);
		}

		private void NonCriticalUpdate() {
			datasource.TroopCountText = TroopCountDisplay();
		}

        private void HitUpdateHUDElements(float dmg) {
			datasource.PlayerHealthText = HealthDisplay(Mission.Current.MainAgent);
			datasource.PlayerDamageText = PlayerDamageDisplay(dmg);
			datasource.PlayerShieldText = PlayerShieldDisplay();
			datasource.MountHealthText = MountHealthDisplay();
		}

		private string HealthDisplay(Agent a) {
			return a.Health + "/" + a.HealthLimit;
        }

		private string PlayerDamageDisplay(float dmg) {
			if (dmg != 0) {
				return ((dmg > 0) ? "+" : "") + dmg.ToString();
			} else {
				return "";
			}
		}

		private float HelthFromLastRun() {
			float healthChange = Mission.Current.MainAgent.Health - lastHealth;
			lastHealth = Mission.Current.MainAgent.Health;

			return healthChange;
		}

		private string PlayerShieldDisplay() {
			if (Mission.Current.MainAgent.WieldedOffhandWeapon.IsShield()) {
				return Mission.Current.MainAgent.WieldedOffhandWeapon.HitPoints + "/" + Mission.Current.MainAgent.WieldedOffhandWeapon.ModifiedMaxHitPoints;
			} else {
				return "";
            }
        }

		private string MountHealthDisplay() {
			if (Mission.MainAgent.HasMount) {
				return Mission.Current.MainAgent.MountAgent.Health + "/" + Mission.Current.MainAgent.MountAgent.HealthLimit;
			} else {
				return "";
            }
		}

		private string TroopCountDisplay() {
			if (Mission.Current.DefenderTeam.ActiveAgents.Count == 0 || Mission.Current.AttackerTeam.ActiveAgents.Count == 0) {
				return "";
			} else {
				return Mission.Current.AttackerTeam.ActiveAgents.Count + " vs " + Mission.Current.DefenderTeam.ActiveAgents.Count;
			}
        }

		public void HandleHealthUpdates() {
			displayedDamage = displayedDamage + HelthFromLastRun();

			HitUpdateHUDElements(displayedDamage);
			hitUpdateDisplay = MissionTime.SecondsFromNow(3);
		}

        public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, in MissionWeapon affectorWeapon, in Blow blow, in AttackCollisionData attackCollisionData) {
            base.OnAgentHit(affectedAgent, affectorAgent, affectorWeapon, blow, attackCollisionData);
			try {
				if (affectorAgent.Character != null && affectedAgent.Character != null) {
					if (affectorAgent == Agent.Main) {
						datasource.EnemyShowStatus = true;
						datasource.EnemyNameText = affectedAgent.Name;
						datasource.EnemyHealth = (int)Math.Round(affectedAgent.Health);
						datasource.EnemyMaxHealth = (int)Math.Round(affectedAgent.HealthLimit);
						datasource.EnemyHealthText = HealthDisplay(affectedAgent);

						enemyStatusDisplayTime = MissionTime.SecondsFromNow(30);
					}

					if (affectedAgent == Agent.Main || affectedAgent == Agent.Main.MountAgent) {

						if (Agent.Main.Health <= 0) {
							datasource.EnemyShowStatus = false;
							displayedDamage = 0;
						}

						HandleHealthUpdates();
					}
				}

			} catch (Exception e) {
				Helper.WriteToLog("Problem with health on hit, cause: " + e);
			}
		}
    }
}
