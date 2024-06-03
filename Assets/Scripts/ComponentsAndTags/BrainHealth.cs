using Unity.Entities;

namespace TMG.Zombies
{
    public struct BrainHealth : IComponentData
    {
        public float Value;
        public float Max;
    }
}