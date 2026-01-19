# RHI Planning Hierarchy

This directory defines the architecture-first plan for building a full driver-centric RHI in ArcRHI. It focuses on system design, boundaries, and milestones rather than code details.

## Document map

- [Architecture Overview](architecture-overview.md)
- [Roadmap](roadmap.md)
- [Replacement Plan (No Backward Compatibility)](replacement-plan.md)
- [Decision Log](decision-log.md)
- [Glossary](glossary.md)
- [Implementation Sequencing](../implementation-sequencing/README.todo)
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
- Command recording uses record and replay with multithreaded recording; immediate execution is an optional convenience.
- Cross-API concepts are explicit: resource states, pipeline layouts, descriptors, and synchronization.
- Single-device focus; multi-GPU is deferred.

## Capability gating

- The RHI queries backend limits and feature flags at device creation and treats them as immutable.
- Optional features are guarded by explicit capability checks at resource/pipeline creation and bind time.
- Unsupported feature usage fails fast (validation error) or routes to a defined fallback path.

## Relationship to existing docs

- [Implementation status](../implementation-status.md)
- [Gaps and roadmap snapshot](../gaps-and-roadmap.md)
