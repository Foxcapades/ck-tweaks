using HarmonyLib;
using PugMod;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace FXCPDS.Content.MoreEquipment {
  [HarmonyPatch]
  public class ModMain: IMod {
    public const string ModName = "extra-equipment";
    public const long ModID = 5176439;
    private const string Version = "v1.0.0";

    private static GameObject prefab;

    internal static void Log(string message) =>
      Debug.Log($"[{ModName}] {message}");

    public void EarlyInit() {
      Log($"init {Version}");
      ConfigManager.register();

      // API.ModLoader.ApplyHarmonyPatch(ModID, typeof(Patches));
    }

    public void Init() {
      ExtendedTab.appendTabs(Manager.ui.characterWindow, prefab!);
      API.Authoring.OnObjectTypeAdded += OnObjectTypeAdded;
    }

    public void Shutdown() {}

    public void ModObjectLoaded(Object obj) {
      if (obj is GameObject { name: "ExtraEquipmentTabs" } o)
        prefab = o;
    }

    public void Update() {}

    private static void OnObjectTypeAdded(Entity entity, GameObject go, EntityManager manager) {
      if (!manager.HasBuffer<EquipmentPresetsBuffer>(entity))
        return;

      var presetsBuffer = manager.GetBuffer<EquipmentPresetsBuffer>(entity);

      // already configured.
      if (presetsBuffer.Length >= 5)
        return;

      var objectsBuffer = manager.GetBuffer<ContainedObjectsBuffer>(entity);

      var refInstance = presetsBuffer[0].equipment;

      for (var i = 0; i < 2; i++) {
        presetsBuffer.Add(new EquipmentPresetsBuffer {
          equipment = new EquipmentCD {
            helmSlotIndex = objectsBuffer.Add(new ContainedObjectsBuffer()),
            necklaceSlotIndex = objectsBuffer.Add(new ContainedObjectsBuffer()),
            breastSlotIndex = objectsBuffer.Add(new ContainedObjectsBuffer()),
            pantsSlotIndex = objectsBuffer.Add(new ContainedObjectsBuffer()),
            ring1SlotIndex = objectsBuffer.Add(new ContainedObjectsBuffer()),
            ring2SlotIndex = objectsBuffer.Add(new ContainedObjectsBuffer()),
            offHandIndex = objectsBuffer.Add(new ContainedObjectsBuffer()),

            // Use the existing shared buffers.  The base implementation also does
            // this.
            bagIndex = refInstance.bagIndex,
            lanternIndex = refInstance.lanternIndex,
            pouch1Index = refInstance.pouch1Index,
            pouch2Index = refInstance.pouch2Index,
            pouch3Index = refInstance.pouch3Index,
            pouch4Index = refInstance.pouch4Index,
          }
        });
      }
    }
  }
}
