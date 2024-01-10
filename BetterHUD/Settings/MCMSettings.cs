using BetterHUD.Localizations;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace BetterHUD.Settings {

    public class MCMSettings : AttributeGlobalSettings<MCMSettings> {

        [SettingPropertyGroup(RefValues.OptionText + "/" + RefValues.TroopInfoText)]
        [SettingPropertyBool(RefValues.ShowCountsText, IsToggle = true, Order = 0, RequireRestart = false, HintText = RefValues.ShowCountsHint)]
        public bool ShowTroopCounts { get; set; } = true;

        [SettingPropertyGroup(RefValues.OptionText + "/" + RefValues.TroopInfoText)]
        [SettingPropertyInteger(RefValues.UpdateIntText, 1, 30, "0 " + RefValues.SecondsText, Order = 0, RequireRestart = false, HintText = RefValues.UpdateIntHint)]
        public int TroopUpdateInterval { get; set; } = 5;



        [SettingPropertyGroup(RefValues.OptionText + "/" + RefValues.PlayerInfoText)]
        [SettingPropertyBool(RefValues.ShowPlayerInfoText, IsToggle = true, Order = 0, RequireRestart = false, HintText = RefValues.ShowPlayerInfoHint)]
        public bool ShowDetailedPlayerInfo { get; set; } = true;

        [SettingPropertyGroup(RefValues.OptionText + "/" + RefValues.PlayerInfoText)]
        [SettingPropertyInteger(RefValues.UpdateIntText, 1, 30, "0 " + RefValues.SecondsText, Order = 0, RequireRestart = false, HintText = RefValues.UpdateIntHint)]
        public int PlayerUpdateInterval { get; set; } = 1;

        [SettingPropertyGroup(RefValues.OptionText + "/" + RefValues.PlayerInfoText)]
        [SettingPropertyBool(RefValues.PercentText, Order = 0, RequireRestart = false, HintText = RefValues.PercentHint)]
        public bool MakePercent { get; set; } = false;



        [SettingPropertyGroup(RefValues.OptionText + "/" + RefValues.EnemyInfoText)]
        [SettingPropertyBool(RefValues.ShowEnemyText, IsToggle = true, Order = 0, RequireRestart = false, HintText = RefValues.ShowEnemyHint)]
        public bool ShowEnemyInfo { get; set; } = true;

        [SettingPropertyGroup(RefValues.OptionText + "/" + RefValues.EnemyInfoText)]
        [SettingPropertyInteger(RefValues.DisplayTimeText, 1, 300, "0 " + RefValues.SecondsText, Order = 0, RequireRestart = false, HintText = RefValues.DisplayTimeHint)]
        public int EnemyInfoDisplayTime { get; set; } = 30;


        public override string Id { get { return base.GetType().Assembly.GetName().Name; } }
        public override string DisplayName { get { return base.GetType().Assembly.GetName().Name; } }
        public override string FolderName { get { return base.GetType().Assembly.GetName().Name; } }
        public override string FormatType { get; } = "xml";
        public bool LoadMCMConfigFile { get; set; } = true;
    }
}
