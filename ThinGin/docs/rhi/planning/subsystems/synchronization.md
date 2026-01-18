# Synchronization and Resource Barriers

## Scope

Synchronization defines CPU to GPU and GPU to GPU coordination, plus resource state transitions.

## Responsibilities

- Fence and semaphore primitives for CPU to GPU coordination.
- Resource state model and transition commands.
- Barrier batching and hazard validation.
- Backend mapping of barriers and ordering constraints.

## Architectural decisions

- Resource states are explicit in the RHI.
- Backends may implement barriers as logical validation when native barriers are not available.
- Submission boundaries define visibility and execution ordering.

## Deliverables

- RHI fence and event semantics.
- Resource transition model with clear state definitions.
- Validation rules for incorrect state usage.
