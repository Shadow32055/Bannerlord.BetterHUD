using BetterCore.Utils;
using BetterHUD.ViewModels;
using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ScreenSystem;

namespace BetterHUD.Behavior {
    class HudManager : MissionBehavior {

		public override MissionBehaviorType BehaviorType => MissionBehaviorType.Other;

		MissionTime hitUpdateDisplay, noncriticalUpdateDiplay, enemyStatusDisplayTime, healthChecker;

		float lastHealth;
		float displayedDamage;

		private GauntletLayer? layer;
		public ScreenBase? sbase;

		private ImprovedHudViewModel? datasource;

		public override void OnMissionTick(float dt) {
			base.OnMissionTick(dt);
            try {

                if (datasource is null)
				return;

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
							noncriticalUpdateDiplay = MissionTime.SecondsFromNow(BetterHUD.Settings.TroopUpdateInterval);
                        }

						if (healthChecker.IsPast) {
							if (lastHealth != Mission.MainAgent.Health) {
								HandleHealthUpdates();
							}

							healthChecker = MissionTime.SecondsFromNow(BetterHUD.Settings.PlayerUpdateInterval);
                        }
					}
				} else {
					hitUpdateDisplay = MissionTime.Zero;
					enemyStatusDisplayTime = MissionTime.Zero;
					noncriticalUpdateDiplay = MissionTime.Zero;
					healthChecker = MissionTime.Zero;
				}
			} catch (Exception e) {
				NotifyHelper.WriteError(BetterHUD.ModName, "Problem with hud manager, cause: " + e);
			}
		}

        public override void AfterStart() {

			this.layer = new GauntletLayer(49, "GauntletLayer", true);

			datasource = new ImprovedHudViewModel();

			//Name of GUI xmls.
			this.layer.LoadMovie("ImprovedHUD", datasource);
			

			this.sbase = ScreenManager.TopScreen;
			this.sbase.AddLayer(this.layer);
			ScreenManager.TrySetFocus(this.layer);
		}

		private void NonCriticalUpdate() {
			if (datasource is null)
				return;

			if (BetterHUD.Settings.ShowTroopCounts) {
				//datasource.TroopCountText = TroopCountDisplay();
				if (AttackerTroopCountDisplay() == "" || DefenderTroopCountDisplay() == "") {
                    datasource.AttackerTroopCountText = "";
                    datasource.DefenderTroopCountText = "";
                } else {
					datasource.AttackerTroopCountText = AttackerTroopCountDisplay();
					datasource.DefenderTroopCountText = DefenderTroopCountDisplay();
				}
			}
		}

        private void HitUpdateHUDElements(float dmg) {
			if (datasource is null)
				return;

            datasource.PlayerDamageText = PlayerDamageDisplay(dmg);

			if (BetterHUD.Settings.ShowDetailedPlayerInfo) {
				datasource.PlayerHealthText = HealthDisplay(Mission.Current.MainAgent, BetterHUD.Settings.MakePercent);

				datasource.PlayerShieldText = PlayerShieldDisplay();
				datasource.MountHealthText = MountHealthDisplay();
			}
		}

		private string HealthDisplay(Agent a, bool percent) {
			if (percent) {
                return Math.Floor(a.Health / a.HealthLimit * 100) + "%";
            }

			return Math.Round(a.Health) + "/" + Math.Round(a.HealthLimit);
			
        }

		private string PlayerDamageDisplay(float dmg) {
			if (dmg != 0) {
				return ((dmg > 0) ? "+" : "") + dmg.ToString();
			} else {
				return "";
			}
		}

		private float HealthFromLastRun() {
			float healthChange = Mission.Current.MainAgent.Health - lastHealth;
			lastHealth = Mission.Current.MainAgent.Health;

			return healthChange;
		}

		private string PlayerShieldDisplay() {
			if (Mission.Current.MainAgent.WieldedOffhandWeapon.IsShield()) {

                float hitpoints = (float)Mission.Current.MainAgent.WieldedOffhandWeapon.HitPoints;
                float maxHitpoints = (float)Mission.Current.MainAgent.WieldedOffhandWeapon.ModifiedMaxHitPoints;

                if (BetterHUD.Settings.MakePercent) {
					

                    return Math.Floor( hitpoints / maxHitpoints * 100) + "%";

                }

				return Math.Round(hitpoints) + "/" + Math.Round(maxHitpoints);
			} else {
				return "";
            }
        }

		private string MountHealthDisplay() {
			if (Mission.MainAgent.HasMount) {
				if (BetterHUD.Settings.MakePercent) {
                    return Math.Floor(Mission.Current.MainAgent.MountAgent.Health / Mission.Current.MainAgent.MountAgent.HealthLimit  * 100) + "%";
                }
				return Math.Round(Mission.Current.MainAgent.MountAgent.Health) + "/" + Math.Round(Mission.Current.MainAgent.MountAgent.HealthLimit);
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

        private string DefenderTroopCountDisplay() {
			if (Mission.Current.DefenderTeam == null)
				return "";

			if (Mission.Current.DefenderTeam.ActiveAgents == null)
				return "";

            if (Mission.Current.DefenderTeam.ActiveAgents.Count == 0)
                return "";

            
            return Mission.Current.DefenderTeam.ActiveAgents.Count.ToString();
        }

        private string AttackerTroopCountDisplay() {
            if (Mission.Current.AttackerTeam == null)
                return "";

            if (Mission.Current.AttackerTeam.ActiveAgents == null)
                return "";

            if (Mission.Current.AttackerTeam.ActiveAgents.Count == 0)
                return "";


            return Mission.Current.AttackerTeam.ActiveAgents.Count.ToString();
        }

        public void HandleHealthUpdates() {
			displayedDamage = displayedDamage + HealthFromLastRun();

			HitUpdateHUDElements(displayedDamage);
			hitUpdateDisplay = MissionTime.SecondsFromNow(2);
		}

        public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, in MissionWeapon affectorWeapon, in Blow blow, in AttackCollisionData attackCollisionData) {
            base.OnAgentHit(affectedAgent, affectorAgent, affectorWeapon, blow, attackCollisionData);
			try {

                if (affectorAgent is null)
                    return;

                if (affectedAgent is null)
                    return;

				if (datasource is null)
					return;

                if (BetterHUD.Settings.ShowEnemyInfo) {
					if (affectorAgent == Agent.Main) {

						datasource.EnemyShowStatus = true;
						datasource.EnemyNameText = affectedAgent.Name;
						datasource.EnemyHealth = (int)Math.Round(affectedAgent.Health);
						datasource.EnemyMaxHealth = (int)Math.Round(affectedAgent.HealthLimit);
						datasource.EnemyHealthText = HealthDisplay(affectedAgent, BetterHUD.Settings.MakePercent);

						enemyStatusDisplayTime = MissionTime.SecondsFromNow(BetterHUD.Settings.EnemyInfoDisplayTime);

						if (Agent.Main.Health <= 0) {
							datasource.EnemyShowStatus = false;
							displayedDamage = 0;
						}
					}
				}

                if (affectedAgent == Agent.Main) {

					if (Agent.Main.Health <= 0) {
						datasource.EnemyShowStatus = false;
						displayedDamage = 0;
					}

					HandleHealthUpdates();
				}

				if (Agent.Main == null)
					return;

				if (Agent.Main.HasMount) {
					if (affectedAgent == Agent.Main.MountAgent) {
						HandleHealthUpdates();
					}
				}
				

			} catch (Exception e) {
				NotifyHelper.WriteError(BetterHUD.ModName, "OnAgentHit threw exception: " + e);
			}
		}
    }
}
