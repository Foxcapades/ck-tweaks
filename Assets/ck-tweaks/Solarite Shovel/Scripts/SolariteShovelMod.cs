using PugMod;
using UnityEngine;

namespace FXCPDS.Items.Solarite_Shovel {
  public class SolariteShovelMod: IMod {
    private const string Version = "1.0.0";
    private const string ModName = "solarite-shovel";

    public const ObjectID SolariteShovelID = (ObjectID)13333;

    private GameObject prefab;

    public static void Log(string message) {
      Debug.Log($"[{ModName}] {message}");
    }

    public void EarlyInit() {
      Log($"init v{Version}");
    }

    public void Init() {
      API.Authoring.OnObjectTypeAdded += (entity, _, manager) => {
        if (manager.GetComponentData<ObjectDataCD>(entity).objectID == ObjectID.SolariteWorkbench) {
          var recipeRef = new CanCraftObjectsBuffer {
            objectID = API.Authoring.GetObjectID("SolariteShovel:SolariteShovel"),
            amount = 1
          };

          var buffer = manager.GetBuffer<CanCraftObjectsBuffer>(entity);

          for (var i = 0; i < buffer.Length; i++) {
            if (buffer[i].objectID == ObjectID.None) {
              buffer[i] = recipeRef;
              return;
            }
          }

          buffer.Add(recipeRef);
        }
      };
    }

    public void Shutdown() {}

    public void ModObjectLoaded(Object obj) {
    }

    public void Update() {}
  }
}