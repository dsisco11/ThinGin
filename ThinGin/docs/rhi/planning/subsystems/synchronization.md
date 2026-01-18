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
- Fence semantics are timeline-based where supported, with binary fallback in the backend.
- Cross-queue synchronization uses explicit signal/wait ordering between graphics and async compute.

## Deliverables

- RHI fence and event semantics.
- Resource transition model with clear access, pipeline, and subresource definitions.
- Validation rules for incorrect state usage and hazard reporting.

## Fence and semaphore semantics

- Fences expose signal and wait with monotonic values at the RHI layer.
- Backends without timeline support emulate via binary fences and internal counters.
- GPU-to-GPU sync uses explicit signal/wait pairs across queues.

## Cross-queue ordering

- Command lists are ordered within a pipeline; cross-pipeline dependencies require explicit fences.
- Transitions specify source and destination pipeline scopes for visibility and ordering.
