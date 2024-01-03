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
				Logger.SendMessage("Problem with hud manager, cause: " + e, Severity.High);
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
			if (SubModule._settings.showTroopCounts) {
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
            datasource.PlayerDamageText = PlayerDamageDisplay(dmg);

			if (SubModule._settings.showDetailedPlayerInfo) {
				datasource.PlayerHealthText = HealthDisplay(Mission.Current.MainAgent, SubModule._settings.makePercent);

				datasource.PlayerShieldText = PlayerShieldDisplay();
				datasource.MountHealthText = MountHealthDisplay();
			}
		}

		private string HealthDisplay(Agent a, bool percent) {
			if (percent) {
                return Math.Floor(a.Health / a.HealthLimit * 100) + "%";
            }

			return a.Health + "/" + a.HealthLimit;
			
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

                if (SubModule._settings.makePercent) {
					

                    return Math.Floor( hitpoints / maxHitpoints * 100) + "%";

                }

				return hitpoints + "/" + maxHitpoints;
			} else {
				return "";
            }
        }

		private string MountHealthDisplay() {
			if (Mission.MainAgent.HasMount) {
				if (SubModule._settings.makePercent) {
                    return Math.Floor(Mission.Current.MainAgent.MountAgent.Health / Mission.Current.MainAgent.MountAgent.HealthLimit  * 100) + "%";
                }
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

        private string DefenderTroopCountDisplay() {
            if (Mission.Current.DefenderTeam.ActiveAgents.Count == 0) {
                return "";
            } else {
                return Mission.Current.DefenderTeam.ActiveAgents.Count.ToString();
            }
        }

        private string AttackerTroopCountDisplay() {
            if (Mission.Current.AttackerTeam.ActiveAgents.Count == 0) {
                return "";
            } else {
                return Mission.Current.AttackerTeam.ActiveAgents.Count.ToString();
            }
        }

        public void HandleHealthUpdates() {
			displayedDamage = displayedDamage + HealthFromLastRun();

			HitUpdateHUDElements(displayedDamage);
			hitUpdateDisplay = MissionTime.SecondsFromNow(2);
		}

        public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, in MissionWeapon affectorWeapon, in Blow blow, in AttackCollisionData attackCollisionData) {
            base.OnAgentHit(affectedAgent, affectorAgent, affectorWeapon, blow, attackCollisionData);
			try {
				if (affectorAgent.Character != null && affectedAgent.Character != null) {
					if (affectorAgent == Agent.Main) {
						if (SubModule._settings.showEnemyInfo) {
							datasource.EnemyShowStatus = true;
							datasource.EnemyNameText = affectedAgent.Name;
							datasource.EnemyHealth = (int)Math.Round(affectedAgent.Health);
							datasource.EnemyMaxHealth = (int)Math.Round(affectedAgent.HealthLimit);
							datasource.EnemyHealthText = HealthDisplay(affectedAgent, SubModule._settings.makePercent);

							enemyStatusDisplayTime = MissionTime.SecondsFromNow(30);
						}
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
				Logger.SendMessage("Problem with health on hit, cause: " + e, Severity.High);
			}
		}
    }
}
