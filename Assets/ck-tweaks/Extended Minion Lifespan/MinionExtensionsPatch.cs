using System.Collections.Generic;
using HarmonyLib;
using I2.Loc;

namespace FXCPDS.Minion.Lifespan {
  [HarmonyPatch(typeof(MinionExtensions))]
  public class MinionExtensionsPatch {
    private const string TargetTerm = "WeaponSecondary/MinionLifespan";

    [HarmonyPostfix]
    [HarmonyPatch(nameof(MinionExtensions.GetSummonMinionStatText))]
    static void StatTextPostfix(ref List<TextAndFormatFields> __result) {
      var target = __result.Find(t => t.text == TargetTerm);
      if (target != null)
        target.formatFields[0] = ExtendedMinionLifespan.Lifespan == -1
          ? LocalizationManager.GetTranslation(ExtendedMinionLifespan.ModI8NPrefix + LifespanMenuOption.InfiniteKey)
          : ExtendedMinionLifespan.Lifespan.ToString();
    }
  }  
}
