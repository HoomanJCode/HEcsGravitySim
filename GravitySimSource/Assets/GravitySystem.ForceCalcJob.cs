using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public partial class GravitySystem
{
    [BurstCompile]
    private struct ForceCalcJob : IJobForEach<PlanetData, Translation, PhysicsVelocity>
    {
        private const float Gravity = 1f;
        [ReadOnly] public NativeArray<Translation> PlanetsTranslation;
        [ReadOnly] public NativeArray<PlanetData> PlanetsMasses;

        private static float3 GetForce(Vector3 myPosition, float myMass, Vector3 targetPosition, float targetMass)
        {
            var direction = targetPosition - myPosition;
            var distance = direction.magnitude;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (distance == 0) return float3.zero;
            return direction.normalized * (Gravity * (myMass * targetMass) / Mathf.Pow(distance, 2));
        }

        public void Execute([ReadOnly] ref PlanetData planetData, [ReadOnly] ref Translation translation,
            ref PhysicsVelocity physicsVelocity)
        {
            for (var i = 0; i < PlanetsTranslation.Length; i++)
                physicsVelocity.Linear += GetForce(translation.Value, planetData.Mass,
                    PlanetsTranslation[i].Value, PlanetsMasses[i].Mass);
        }
    }
}