using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

namespace TMG.Zombies
{
    public class TombstoneMono : MonoBehaviour
    {
        public GameObject Renderer;
    }

    public class TombstoneBaker : Baker<TombstoneMono>
    {
        public override void Bake(TombstoneMono authoring)
        {
            var tombstoneEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(tombstoneEntity, new TombstoneRenderer
            {
                Value = GetEntity(authoring.Renderer, TransformUsageFlags.Dynamic)
            });
        }
    }
    
    [MaterialProperty("TombstoneOffset")]
    public struct TombstoneOffset : IComponentData
    {
        public float2 Value;
    }

    public struct TombstoneRenderer : IComponentData
    {
        public Entity Value;
    }
}