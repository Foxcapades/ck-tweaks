using PugMod;
using Unity.Entities;
using UnityEngine;

#nullable enable
namespace FXCPDS.Content.MoreEquipment {
  public class MoreEquipmentSets: IMod {
    private const string ModName = "extra-equipment";
    private const string version = "v1.0.0";
    private static GameObject? prefab;

    internal static void Log(string message) =>
      Debug.Log($"[{ModName}] {message}");

    public void EarlyInit() {
      Log($"init {version}");
    }

    public void Init() {
      ExtendedTab.appendTabs(Manager.ui.characterWindow, prefab!);
      API.Authoring.OnObjectTypeAdded += OnObjectTypeAdded;
    }

    public void Shutdown() {
      // Safety measure to prevent NP crash on mod uninstallation.
      Manager.main.allPlayers.ForEach(p => {
        if (p.activeEquipmentPreset > 2)
          p.SetActiveEquipmentPreset(0);
      });
    }

    public void ModObjectLoaded(Object obj) {
      if (obj is GameObject { name: "ExtraEquipmentTabs" } o)
        prefab = o;
    }

    public void Update() {}

    private static void OnObjectTypeAdded(Entity entity, GameObject _, EntityManager manager) {
      if (!manager.HasBuffer<EquipmentPresetsBuffer>(entity))
        return;

      var presetsBuffer = manager.GetBuffer<EquipmentPresetsBuffer>(entity);

      // already configured.
      if (presetsBuffer.Length > 3)
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
