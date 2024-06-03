using Unity.Entities;
using UnityEngine;

namespace TMG.Zombies
{
    public class ZombieMono : MonoBehaviour
    {
        public float RiseRate;
        public float WalkSpeed;
        public float WalkAmplitude;
        public float WalkFrequency;
        
        public float EatDamage;
        public float EatAmplitude;
        public float EatFrequency;
    }

    public class ZombieBaker : Baker<ZombieMono>
    {
        public override void Bake(ZombieMono authoring)
        {
            var zombieEntity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent(zombieEntity, new ZombieRiseRate { Value = authoring.RiseRate });
            AddComponent(zombieEntity, new ZombieWalkProperties
            {
                WalkSpeed = authoring.WalkSpeed,
                WalkAmplitude = authoring.WalkAmplitude,
                WalkFrequency = authoring.WalkFrequency
            });
            AddComponent(zombieEntity, new ZombieEatProperties
            {
                EatDamagePerSecond = authoring.EatDamage,
                EatAmplitude = authoring.EatAmplitude,
                EatFrequency = authoring.EatFrequency
            });
            AddComponent<ZombieTimer>(zombieEntity);
            AddComponent<ZombieHeading>(zombieEntity);
            AddComponent<NewZombieTag>(zombieEntity);
            
        }
    }
}