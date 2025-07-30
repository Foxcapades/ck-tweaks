using System.Diagnostics.CodeAnalysis;
using Unity.Entities;

namespace FXCPDS.Minion.Lifespan {
  [WorldSystemFilter(WorldSystemFilterFlags.All)]
  public partial class MinionSystem : PugSimulationSystemBase {
    private const int MinionLifespanConditionID = 103;
    protected override void OnCreate() {
      RequireForUpdate<MinionCD>();
      base.OnCreate();
    }

    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    protected override void OnUpdate() {
      var conditionLookup = GetBufferLookup<SummarizedConditionEffectsBuffer>();
      var lifespan = (float)ExtendedMinionLifespan.Lifespan;
    
      Entities.WithAll<MinionCD, OwnerReferenceCD>()
        .ForEach((ref MinionCD minion, ref OwnerReferenceCD owner) => {
          // Skip the minion data if it hasn't been initialized yet.
          if (minion.lifespan is 0)
            return;

          // If lifespan is infinite, always set it to 100%
          if (lifespan == -1) {
            minion.lifespan = 100;
            minion.lifespanTimer = 100;
            return;
          }

          // If we've already updated the lifespan, then no need to process this
          // minion again.
          if (minion.lifespan == lifespan)
            return;

          // Get the player's minion lifespan bonus to apply to the modded
          // lifespan value.
          var bonus = conditionLookup.TryGetBuffer(owner.owner, out var data) 
            ? data[MinionLifespanConditionID].value / 100f + 1f
            : 1f;

          // Get the amount of time that has already passed since the minion
          // data was initialized so we can subtract that from the updated
          // lifespan below.
          var delta = minion.lifespan - minion.lifespanTimer;

          minion.lifespan = lifespan * bonus;
          minion.lifespanTimer = minion.lifespan - delta;
        }).Schedule();

      base.OnUpdate();
    }
  }


}
