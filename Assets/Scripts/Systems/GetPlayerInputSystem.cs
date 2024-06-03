using Unity.Entities;
using UnityEngine;

namespace TMG.Zombies
{
    public partial class GetPlayerInputSystem : SystemBase
    {
        protected override void OnCreate()
        {
            EntityManager.AddComponent<ForwardInput>(SystemHandle);
        }

        protected override void OnUpdate()
        {
            var currentInput = new ForwardInput
            {
                Value = Input.GetKey(KeyCode.W)
            };
            EntityManager.SetComponentData(SystemHandle, currentInput);
        }
    }
    
    public struct ForwardInput : IComponentData
    {
        public bool Value;
    }
}