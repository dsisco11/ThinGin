# Command Model

## Scope

The command model defines how the renderer records work and how the driver executes it.

## Goals

- Make the execution path explicit and backend-reachable.
- Support record and replay first, with room for immediate convenience and multithreaded recording later.
- Keep command semantics stable across backends.

## Key decisions

- Commands execute through a driver-owned command context.
- Command lists are recorded then submitted; execution happens through `IRHICommandContext`.
- Immediate execution is optional and layered on top of the same command semantics.
- Submission is an explicit boundary with fences for synchronization.

## Dependencies

- Driver backends must expose a context for executing commands.
- Resource system must define lifetime and state transitions.

## Deliverables

- Command list types and submission path defined at the RHI layer.
- Clear execution semantics and validation hooks.
- Optional RHI thread model described and reserved for later phases.
