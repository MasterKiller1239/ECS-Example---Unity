using Unity.Entities;

namespace TMG.Zombies
{
    [InternalBufferCapacity(8)]
    public struct BrainDamageBufferElement : IBufferElementData
    {
        public float Value;
    }
}