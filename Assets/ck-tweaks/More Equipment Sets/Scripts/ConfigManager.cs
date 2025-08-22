using System;
using System.Runtime.CompilerServices;
using PugMod;
using Unity.Jobs;

namespace FXCPDS.Content.MoreEquipment {
  internal static class ConfigManager {
    private const string ConfigGroup = "player";
    private const string ConfigKey = "presets";

    private static string modName => ModMain.ModName;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void log(string message) {
      ModMain.Log(message);
    }

    public static void Save() {
      if (!Manager.sceneHandler.isInGame || Manager.main.player == null)
        return;

      var data = API.Config.Get<int[]>(modName, ConfigGroup, ConfigKey);
      data = ensureCorrectArray(ref data);
      data[Manager.saves.GetCharacterId()] = Manager.main.player.activeEquipmentPreset+1;

      if (Manager.main.player.activeEquipmentPreset > 2)
        Manager.main.player.SetActiveEquipmentPreset(2);

      API.Config.Set(modName, ConfigGroup, ConfigKey, data);
    }

    private struct SaveJob: IJob {
      public void Execute() {
        Save();
      }
    }

    private struct LoadJob: IJob {
      public void Execute() {
        ModMain.Log("load.execute");

        if (!Manager.sceneHandler.isInGame || Manager.main.player == null)
          return;

        if (!API.Config.TryGet<int[]>(modName, ConfigGroup, ConfigKey, out var data) || data == null)
          return;

        ModMain.Log("loading");

        data = ensureCorrectArray(ref data);

        var idx = data[Manager.saves.GetCharacterId()];

        if (idx < 1 || idx > 5)
          return;

        Manager.main.player.SetActiveEquipmentPreset(idx-1);
      }
    }

    internal static void register() =>
      API.Config.Register(modName, ConfigGroup, "player preset selections", ConfigKey, new int[SaveManager.totalNumberCharacters]);

    internal static JobHandle scheduleSave() =>
      new SaveJob().Schedule();

    internal static JobHandle scheduleLoad() =>
      new LoadJob().Schedule();

    private static int[] ensureCorrectArray(ref int[] data) {
      if (data == null)
        return new int[SaveManager.totalNumberCharacters];
      if (data.Length < SaveManager.totalNumberCharacters)
        Array.Resize(ref data, SaveManager.totalNumberCharacters);

      return data;
    }
  }
}