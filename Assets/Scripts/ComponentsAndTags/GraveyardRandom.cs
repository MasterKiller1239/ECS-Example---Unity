using Unity.Entities;
using Unity.Mathematics;

namespace TMG.Zombies
{
    public struct GraveyardRandom : IComponentData
    {
        public Random Value;
    }
}