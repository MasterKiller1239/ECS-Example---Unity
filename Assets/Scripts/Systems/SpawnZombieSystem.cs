using Unity.Burst;
using Unity.Entities;

namespace TMG.Zombies
{
    [BurstCompile]
    public partial struct SpawnZombieSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ZombieSpawnTimer>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

            new SpawnZombieJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged)
            }.Run();
        }
    }
    
    [BurstCompile]
    public partial struct SpawnZombieJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer ECB;
        
        [BurstCompile]
        private void Execute(GraveyardAspect graveyard)
        {
            graveyard.ZombieSpawnTimer -= DeltaTime;
            if(!graveyard.TimeToSpawnZombie) return;
            if(!graveyard.ZombieSpawnPointInitialized()) return;
            
            graveyard.ZombieSpawnTimer = graveyard.ZombieSpawnRate;
            var newZombie = ECB.Instantiate(graveyard.ZombiePrefab);

            var newZombieTransform = graveyard.GetZombieSpawnPoint();
            ECB.SetComponent(newZombie, newZombieTransform);
            
            var zombieHeading = MathHelpers.GetHeading(newZombieTransform.Position, graveyard.Position);
            ECB.SetComponent(newZombie, new ZombieHeading{Value = zombieHeading});
        }
    }
}