using PugMod;
using UnityEngine;

namespace FXCPDS.Minion.Lifespan {
  public class ExtendedMinionLifespan : IMod {
    public const string ModVersion = "2.1.0";
    public const string ModKey = "fxcpds.minion.lifespan";
    public const string ModI8NPrefix = "FXCPDS/Minions/Lifespan/";

    public static short Lifespan {
      get => _instance.lifespan;
      set => _instance.updateLifespan(value);
    }

    public static GameObject MenuPrefab => _instance.menuOption;

    public static void Log(string msg) {
      Debug.Log($"[{ModKey}] {msg}");
    }

    private static ExtendedMinionLifespan _instance;
    private short lifespan = 120;
    private GameObject menuOption;

    public void EarlyInit() {
      _instance = this;
      API.Config.Register(ModKey, "lifespan", "", "value", 120);
      Log($"init v{ModVersion}");
    }

    public void Init() {
      lifespan = API.Config.Get<short>(ModKey, "lifespan", "value");
      LifespanMenuOption.AppendTo(Manager.menu.gameplayOptionsMenu, MenuPrefab);
    }

    public void Shutdown() {}

    public void ModObjectLoaded(Object obj) {
      if (obj is GameObject { name: "LifespanMenuOption" } o) {
        menuOption = o;
      }
    }

    public void Update() {}

    private void updateLifespan(short lifespan) {
      this.lifespan = lifespan;
      API.Config.Set(ModKey, "lifespan", "value", lifespan);
    }
  }
}