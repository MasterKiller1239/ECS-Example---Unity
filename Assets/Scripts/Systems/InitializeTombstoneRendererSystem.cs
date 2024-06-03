using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace TMG.Zombies
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(SpawnTombstoneSystem))]
    public partial struct InitializeTombstoneRendererSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GraveyardProperties>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            var graveyard = SystemAPI.GetAspect<GraveyardAspect>(SystemAPI.GetSingletonEntity<GraveyardProperties>());
            
            foreach (var tombstoneRenderer in SystemAPI.Query<RefRW<TombstoneRenderer>>())
            {
                ecb.AddComponent(tombstoneRenderer.ValueRW.Value,
                    new TombstoneOffset { Value = graveyard.GetRandomOffset() });
            }
            ecb.Playback(state.EntityManager);
        }
    }
}