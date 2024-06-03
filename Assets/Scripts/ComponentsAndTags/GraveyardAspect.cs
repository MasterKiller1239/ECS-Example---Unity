using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.Zombies
{
    public readonly partial struct GraveyardAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<LocalTransform> _transform;
        private LocalTransform Transform => _transform.ValueRO;

        private readonly RefRO<GraveyardProperties> _graveyardProperties;
        private readonly RefRW<GraveyardRandom> _graveyardRandom;
        private readonly RefRW<ZombieSpawnPoints> _zombieSpawnPoints;
        private readonly RefRW<ZombieSpawnTimer> _zombieSpawnTimer;
        
        public int NumberTombstonesToSpawn => _graveyardProperties.ValueRO.NumberTombstonesToSpawn;
        public Entity TombstonePrefab => _graveyardProperties.ValueRO.TombstonePrefab;
        
        public bool ZombieSpawnPointInitialized()
        {
            return _zombieSpawnPoints.ValueRO.Value.IsCreated && ZombieSpawnPointCount > 0;
        }

        private int ZombieSpawnPointCount => _zombieSpawnPoints.ValueRO.Value.Value.Value.Length;

        public LocalTransform GetRandomTombstoneTransform()
        {
            return new LocalTransform
            {
                Position = GetRandomPosition(),
                Rotation = GetRandomRotation(),
                Scale = GetRandomScale(0.5f)
            };
        }

        private float3 GetRandomPosition()
        {
            float3 randomPosition;
            do
            {
                randomPosition = _graveyardRandom.ValueRW.Value.NextFloat3(MinCorner, MaxCorner);
            } while (math.distancesq(Transform.Position, randomPosition) <= BRAIN_SAFETY_RADIUS_SQ);

            return randomPosition;
        }
        
        private float3 MinCorner => Transform.Position - HalfDimensions;
        private float3 MaxCorner => Transform.Position + HalfDimensions;
        private float3 HalfDimensions => new()
        {
            x = _graveyardProperties.ValueRO.FieldDimensions.x * 0.5f,
            y = 0f,
            z = _graveyardProperties.ValueRO.FieldDimensions.y * 0.5f
        };
        private const float BRAIN_SAFETY_RADIUS_SQ = 100;

        private quaternion GetRandomRotation() => quaternion.RotateY(_graveyardRandom.ValueRW.Value.NextFloat(-0.25f, 0.25f));
        private float GetRandomScale(float min) => _graveyardRandom.ValueRW.Value.NextFloat(min, 1f);

        public float2 GetRandomOffset()
        {
            return _graveyardRandom.ValueRW.Value.NextFloat2();
        }

        public float ZombieSpawnTimer
        {
            get => _zombieSpawnTimer.ValueRO.Value;
            set => _zombieSpawnTimer.ValueRW.Value = value;
        }

        public bool TimeToSpawnZombie => ZombieSpawnTimer <= 0f;

        public float ZombieSpawnRate => _graveyardProperties.ValueRO.ZombieSpawnRate;

        public Entity ZombiePrefab => _graveyardProperties.ValueRO.ZombiePrefab;

        public LocalTransform GetZombieSpawnPoint()
        {
            var position = GetRandomZombieSpawnPoint();
            return new LocalTransform
            {
                Position = position,
                Rotation = quaternion.RotateY(MathHelpers.GetHeading(position, Transform.Position)),
                Scale = 1f
            };
        }

        private float3 GetRandomZombieSpawnPoint()
        {
            return GetZombieSpawnPoint(_graveyardRandom.ValueRW.Value.NextInt(ZombieSpawnPointCount));
        }

        private float3 GetZombieSpawnPoint(int i) => _zombieSpawnPoints.ValueRO.Value.Value.Value[i];

        public float3 Position => Transform.Position;
    }
}