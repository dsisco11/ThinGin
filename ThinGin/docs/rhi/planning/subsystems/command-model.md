# Command Model

## Scope

The command model defines how the renderer records work and how the driver executes it.

## Goals

- Make the execution path explicit and backend-reachable.
- Support immediate execution first, with room for record and replay and multithreaded recording.
- Keep command semantics stable across backends.

## Key decisions

- Commands execute through a driver-owned command context.
- Immediate command lists are the baseline; deferred lists can be added without changing command semantics.
- Submission is an explicit boundary with fences for synchronization.

## Dependencies

- Driver backends must expose a context for executing commands.
- Resource system must define lifetime and state transitions.

## Deliverables

- Command list types and submission path defined at the RHI layer.
- Clear execution semantics and validation hooks.
- Optional RHI thread model described and reserved for later phases.
