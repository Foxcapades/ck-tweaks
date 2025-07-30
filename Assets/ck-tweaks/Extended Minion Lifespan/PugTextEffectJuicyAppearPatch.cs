using HarmonyLib;

namespace FXCPDS.Minion.Lifespan {
  [HarmonyPatch(typeof(PugTextEffectJuicyAppear))]
  public class PugTextEffectJuicyAppearPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PugTextEffectJuicyAppear.PugTextEffectLateUpdate))]
    static bool Prefix(PugTextEffectJuicyAppear __instance) {
      return __instance.centerPoint?.transform != null;
    }
  }
}