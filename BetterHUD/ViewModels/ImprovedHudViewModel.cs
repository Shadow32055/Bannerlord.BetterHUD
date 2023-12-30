using TaleWorlds.Library;

namespace BetterHUD.ViewModels {
	public class ImprovedHudViewModel : ViewModel {

		private string playerDamage = "";
		private string playerHealth = "";
		private string playerShield = "";

		private string mountHealth = "";

		private string troopCount = "";
        private string attackerTroopCount = "";
        private string defenderTroopCount = "";

        private string enemyHealth = "";

		private string enemyName = "";
		private int enemyHealthInt = 0;
		private int enemyMaxHealthInt = 0;
		private bool enemyShowStatus = false;
		private bool displayHud = true;

		[DataSourceProperty]
		public string EnemyHealthText {
			get {
				return enemyHealth;
			}
			set {
				this.enemyHealth = value;
				base.OnPropertyChangedWithValue(value, "EnemyHealthText");
			}
		}


		[DataSourceProperty]
		public bool EnemyShowStatus {
			get {
				return enemyShowStatus;
			}
			set {
				this.enemyShowStatus = value;
				base.OnPropertyChangedWithValue(value, "EnemyShowStatus");
			}
		}

		[DataSourceProperty]
		public bool DisplayHUD {
			get {
				return displayHud;
			}
			set {
				this.displayHud = value;
				base.OnPropertyChangedWithValue(value, "DisplayHUD");
			}
		}


		[DataSourceProperty]
		public string EnemyNameText {
			get {
				return enemyName;
			}
			set {
				this.enemyName = value;
				base.OnPropertyChangedWithValue(value, "EnemyNameText");
			}
		}

		[DataSourceProperty]
		public int EnemyHealth {
			get {
				return enemyHealthInt;
			}
			set {
				this.enemyHealthInt = value;
				base.OnPropertyChangedWithValue(value, "EnemyHealth");
			}
		}

		[DataSourceProperty]
		public int EnemyMaxHealth {
			get {
				return enemyMaxHealthInt;
			}
			set {
				this.enemyMaxHealthInt = value;
				base.OnPropertyChangedWithValue(value, "EnemyMaxHealth");
			}
		}

		[DataSourceProperty]
		public string PlayerDamageText {
			get {
				return playerDamage;
			}
			set {
				this.playerDamage = value;
				base.OnPropertyChangedWithValue(value, "PlayerDamageText");
			}
		}

		[DataSourceProperty]
		public string PlayerHealthText {
			get {
				return playerHealth;
			}
			set {
				this.playerHealth = value;
				base.OnPropertyChangedWithValue(value, "PlayerHealthText");
			}
		}

		[DataSourceProperty]
		public string PlayerShieldText {
			get {
				return playerShield;
			}
			set {
				this.playerShield = value;
				base.OnPropertyChangedWithValue(value, "PlayerShieldText");
			}
		}

		[DataSourceProperty]
		public string MountHealthText {
			get {
				return mountHealth;
			}
			set {
				this.mountHealth = value;
				base.OnPropertyChangedWithValue(value, "MountHealthText");
			}
		}

		[DataSourceProperty]
		public string TroopCountText {
			get {
				return troopCount;
			}
			set {
				this.troopCount = value;
				base.OnPropertyChangedWithValue(value, "TroopCountText");
			}
		}

        [DataSourceProperty]
        public string AttackerTroopCountText {
            get {
                return attackerTroopCount;
            }
            set {
                this.attackerTroopCount = value;
                base.OnPropertyChangedWithValue(value, "AttackerTroopCountText");
            }
        }

        [DataSourceProperty]
        public string DefenderTroopCountText {
            get {
                return defenderTroopCount;
            }
            set {
                this.defenderTroopCount = value;
                base.OnPropertyChangedWithValue(value, "DefenderTroopCountText");
            }
        }
    }
}
