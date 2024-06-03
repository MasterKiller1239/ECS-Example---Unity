using Unity.Burst;
using Unity.Entities;

namespace TMG.Zombies
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
    public partial struct ApplyBrainDamageSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency.Complete();   
            foreach (var brain in SystemAPI.Query<BrainAspect>())
            {
                brain.DamageBrain();
            }
        }
    }
}