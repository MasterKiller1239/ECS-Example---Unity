using Unity.Entities;
using UnityEngine;

namespace TMG.Zombies
{
    public class BrainMono : MonoBehaviour
    {
        public float BrainHealth;
    }

    public class BrainBaker : Baker<BrainMono>
    {
        public override void Bake(BrainMono authoring)
        {
            var brainEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<BrainTag>(brainEntity);
            AddComponent(brainEntity, new BrainHealth { Value = authoring.BrainHealth, Max = authoring.BrainHealth });
            AddBuffer<BrainDamageBufferElement>(brainEntity);
        }
    }
    
    public class DataMono : MonoBehaviour
    {
        public float BrainHealth;
    }

    public class DataBaker : Baker<DataMono>
    {
        public override void Bake(DataMono authoring)
        {
            var dataEntity = GetEntity(TransformUsageFlags.None);
            AddComponent<DataTag>(dataEntity);
        }
    }
    
    public struct DataTag : IComponentData{}
}