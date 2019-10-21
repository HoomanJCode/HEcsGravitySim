# HEcsGravitySim
###### gravity simulation by ECS and job system at unity.

In this simple project, we calculated the force of each of the planets on all other planets, each frame.

So we have two big nested loops over 1000 planets and we can not process all of them on a single thread when we need realtime calculation.

to fix this problem we need CPU multi-threading. Data-Oriented Programming and Unity3d can help us!

[Rendered Video](https://www.aparat.com/v/GpcBv)

at this example, we used Unity, JobSystem(multi-threading system), ECS Architecture (Data-oriented programming), Hybrid Renderer Package (Fast rendering Approach) and UnityPhysics package(Havok Physics at this case).

## Documentation:
at this simple project, we have Job structure, Gravity System Class, Planet Component Data and Random Generator Class.
Random Generator Class, generating planets at random positions.

Gravity system Gathering Planets Entities Each Frame by EntityQuery.to process force of them and you can detect entities by Planet Component Data.

The gravity system executes the job each frame. at this job, we have nested "for"s and when we add calculated forces to Velocity Component, the physic engine calculates positions automatically.
