using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace BetterHUD.Settings {

    public class MCMSettings : AttributeGlobalSettings<MCMSettings> {

        const string optionText = "{=BH_oPw9hh}Options";

        const string enabledText = "{=BA_V8krxN}Enable";
        const string enabledDesText = "{=BA_sVRwEA}Should this bonus be applied.";

     

        [SettingPropertyGroup(optionText)]
        [SettingPropertyBool("Show Troop Counts", Order = 0, RequireRestart = false, HintText = "Show the number of troops on the attacking and defending side.")]
        public bool showTroopCounts { get; set; } = true;

        [SettingPropertyGroup(optionText)]
        [SettingPropertyBool("Show Detailed PLayer Information", Order = 0, RequireRestart = false, HintText = "Show numeric values for player health, shield durabiltiy and mount heath.")]
        public bool showDetailedPlayerInfo { get; set; } = true;

        [SettingPropertyGroup(optionText)]
        [SettingPropertyBool("Show Enemy Information", Order = 0, RequireRestart = false, HintText = "Show the last damaged enemy name and health at the bottom of the screen.")]
        public bool showEnemyInfo { get; set; } = true;

        [SettingPropertyGroup(optionText)]
        [SettingPropertyBool("Display as Percent",Order = 0,RequireRestart = false,HintText = "Show all numeric values as percent.")]
        public bool makePercent { get; set; } = false;






        public override string Id { get { return base.GetType().Assembly.GetName().Name; } }
        public override string DisplayName { get; } = "Better HUD";
        public override string FolderName { get { return base.GetType().Assembly.GetName().Name; } }
        public override string FormatType { get; } = "xml";
        public bool LoadMCMConfigFile { get; set; } = true;
    }
}
