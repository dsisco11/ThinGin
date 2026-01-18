# Synchronization and Resource Barriers

## Scope

Synchronization defines CPU to GPU and GPU to GPU coordination, plus resource state transitions.

See [Resource State Model](resource-state-model.md) for the explicit state and transition definitions.

## Responsibilities

- Fence and semaphore primitives for CPU to GPU coordination.
- Resource state model and transition commands.
- Barrier batching and hazard validation.
- Backend mapping of barriers and ordering constraints.

## Architectural decisions

- Resource states are explicit: access masks plus pipeline scope (graphics/async compute).
- Transitions are explicit, subresource-aware, and validated by the RHI layer.
- Barriers include transition flags for discard/clear and aliasing, even if some backends treat them as logical.
- Submission boundaries define visibility and execution ordering across pipelines.

## Deliverables

- RHI fence and event semantics.
- Resource transition model with clear access, pipeline, and subresource definitions.
- Validation rules for incorrect state usage and hazard reporting.
