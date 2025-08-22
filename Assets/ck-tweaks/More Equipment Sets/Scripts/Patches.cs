using HarmonyLib;

namespace FXCPDS.Content.MoreEquipment {
  [HarmonyPatch]
  internal class Patches {
    [HarmonyPostfix]
    [HarmonyPatch(typeof(LoadManager), nameof(LoadManager.ExitGame), typeof(CameraSceneFader.FadeSettings))]
    static void exitPostfix() {
      if (Manager.sceneHandler.playerWantsToExitToTitle)
        ConfigManager.Save();
    }
  }
}