using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Transforms;

public partial class GravitySystem : JobComponentSystem
{
    private ForceCalcJob job;
    private JobHandle jobHandle;
    private EntityQuery worldPlanets;

    protected override void OnCreate()
    {
        worldPlanets = GetEntityQuery(
            ComponentType.ReadOnly<PlanetData>(),
            ComponentType.ReadOnly<Translation>(),
            ComponentType.ReadWrite<PhysicsVelocity>()
        );
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        job.PlanetsTranslation = worldPlanets.ToComponentDataArray<Translation>(Allocator.TempJob);
        job.PlanetsMasses = worldPlanets.ToComponentDataArray<PlanetData>(Allocator.TempJob);
        jobHandle = job.Schedule(worldPlanets, inputDeps);
        jobHandle = job.PlanetsTranslation.Dispose(jobHandle);
        jobHandle = job.PlanetsMasses.Dispose(jobHandle);
        return jobHandle;
    }
}