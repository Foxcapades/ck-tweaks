using Unity.Entities;

#nullable enable
namespace FXPCDS.Content.MoreEquipment {
  [InternalBufferCapacity(2)]
  public struct ExtraEquipmentPresetsBuffer: IBufferElementData {
    public EquipmentCD equipment;
  }
}