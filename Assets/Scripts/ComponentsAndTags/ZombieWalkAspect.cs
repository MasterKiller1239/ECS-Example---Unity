using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.Zombies
{
    public readonly partial struct ZombieWalkAspect : IAspect
    {
        public readonly Entity Entity;
        
        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRW<ZombieTimer> _walkTimer;
        private readonly RefRO<ZombieWalkProperties> _walkProperties;
        private readonly RefRO<ZombieHeading> _heading;

        private float WalkSpeed => _walkProperties.ValueRO.WalkSpeed;
        private float WalkAmplitude => _walkProperties.ValueRO.WalkAmplitude;
        private float WalkFrequency => _walkProperties.ValueRO.WalkFrequency;
        private float Heading => _heading.ValueRO.Value;

        private float WalkTimer
        {
            get => _walkTimer.ValueRO.Value;
            set => _walkTimer.ValueRW.Value = value;
        }

        public void Walk(float deltaTime)
        {
            WalkTimer += deltaTime;
            _transform.ValueRW.Position += _transform.ValueRO.Forward() * WalkSpeed * deltaTime;
            
            var swayAngle = WalkAmplitude * math.sin(WalkFrequency * WalkTimer);
            _transform.ValueRW.Rotation = quaternion.Euler(0, Heading, swayAngle);
        }
        
        public bool IsInStoppingRange(float3 brainPosition, float brainRadiusSq)
        {
            return math.distancesq(brainPosition, _transform.ValueRO.Position) <= brainRadiusSq;
        }
    }
}