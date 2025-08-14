using System;
using System.Reflection;
using PugMod;
using UnityEngine;
using HarmonyLib;
using PugConversion;
using Unity.Entities;
using Object = UnityEngine.Object;

namespace FXPCDS.Content.MoreEquipment {

  [HarmonyPatch]
  public class MoreEquipmentSets: IMod {
    public void EarlyInit() {}

    public void Init() {
      Manager.ui.characterWindow.presetTabs[]
    }

    public void Shutdown() {}

    public void ModObjectLoaded(Object obj) {}

    public void Update() {}

    [HarmonyPrefix]
    [HarmonyPatch(typeof(EquipmentHandler), "GetActiveEquipment")]
    static bool GetActiveEquipment(
      ref int activePreset,
      ref Entity ___ownerEntity,
      ref World ___world,
      ref EquipmentCD __result
    ) {
      if (activePreset < 3)
        return true;

      if (EntityUtility.TryGetBuffer<ExtraEquipmentPresetsBuffer>(___ownerEntity, ___world, out var buffer)) {
        __result = buffer[activePreset - 2].equipment;
        return false;
      }

      activePreset = 2;
      return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(InventoryConverter), nameof(InventoryConverter.Convert))]
    static void Convert(GameObject authoring, InventoryConverter __instance, ref Entity ___PrimaryEntity) {
      if (!__instance.TryGetActiveComponent<EquipmentAuthoring>(authoring, out _))
        return;

      __instance.EnsureHasBuffer<ExtraEquipmentPresetsBuffer>();

      // ConversionManager is sadly hidden.
      var convMan = (ConversionManager) __instance.GetType()
        .GetMethod("get_ConversionManager", BindingFlags.NonPublic | BindingFlags.Instance)!
        .Invoke(__instance, Array.Empty<object>());

      var refInstance = convMan.EntityManager
        .GetBuffer<EquipmentPresetsBuffer>(___PrimaryEntity)[0].equipment;

      for (var i = 0; i < 2; i++) {
        __instance.AddToBuffer(new ExtraEquipmentPresetsBuffer {
          equipment = new EquipmentCD {
            helmSlotIndex = __instance.AddToBuffer(new ContainedObjectsBuffer()),
            necklaceSlotIndex = __instance.AddToBuffer(new ContainedObjectsBuffer()),
            breastSlotIndex = __instance.AddToBuffer(new ContainedObjectsBuffer()),
            pantsSlotIndex = __instance.AddToBuffer(new ContainedObjectsBuffer()),
            ring1SlotIndex = __instance.AddToBuffer(new ContainedObjectsBuffer()),
            ring2SlotIndex = __instance.AddToBuffer(new ContainedObjectsBuffer()),
            offHandIndex = __instance.AddToBuffer(new ContainedObjectsBuffer()),

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
