# RHI Planning Hierarchy

This directory defines the architecture-first plan for building a full UE5-like RHI in ThinGin. It focuses on system design, boundaries, and milestones rather than code details.

## Document map

- [Architecture Overview](architecture-overview.md)
- [Roadmap](roadmap.md)
- [Replacement Plan (No Backward Compatibility)](replacement-plan.md)
- [Decision Log](decision-log.md)
- Subsystem Plans
  - [Driver Backends](subsystems/driver-backends.md)
  - [Command Model](subsystems/command-model.md)
  - [Resource System](subsystems/resource-system.md)
  - [Resource State Model](subsystems/resource-state-model.md)
  - [Pipeline State](subsystems/pipeline-state.md)
  - [Synchronization and Barriers](subsystems/synchronization.md)
  - [Descriptors and Binding](subsystems/descriptors-and-binding.md)
  - [Shader System](subsystems/shader-system.md)
  - [Presentation and Swapchain](subsystems/presentation.md)
  - [Diagnostics and Tooling](subsystems/diagnostics-and-tooling.md)

## Guiding decisions

- Driver-centric RHI with `IRHIDriver` as the backend boundary.
- One RHI frontend that the renderer and engine rely on; legacy paths are removed rather than bridged.
- No backward compatibility requirements; avoid adapter layers.
- Resources are described at creation and owned by the RHI, with backend-managed native objects.
- Command recording starts with immediate execution and evolves toward record and replay with optional multithreaded recording.
- Cross-API concepts are explicit: resource states, pipeline layouts, descriptors, and synchronization.

## Relationship to existing docs

- [Implementation status](../implementation-status.md)
- [Gaps and roadmap snapshot](../gaps-and-roadmap.md)
