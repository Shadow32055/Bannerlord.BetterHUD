
namespace BetterHUD.Settings {
    public class DefaultSettings : ISettings {
        public bool showTroopCounts { get; set; } = true;
        public bool showDetailedPlayerInfo { get; set; } = true;
        public bool showEnemyInfo { get; set; } = true;
        public bool makePercent { get; set; } = false;
    }
}
