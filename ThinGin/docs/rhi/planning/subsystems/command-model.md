# Command Model

## Scope

The command model defines how the renderer records work and how the driver executes it.

## Goals

- Make the execution path explicit and backend-reachable.
- Support record and replay with multithreaded recording and optional immediate convenience.
- Keep command semantics stable across backends.

## Key decisions

- Commands execute through a driver-owned command context.
- Command lists are recorded then submitted; execution happens through `IRHICommandContext`.
- Immediate execution is optional and layered on top of the same command semantics.
- Submission is an explicit boundary with fences for synchronization.
- Multithreaded recording is supported via per-thread command allocators and a centralized submit path.
- An optional dedicated RHI thread can be enabled per platform/configuration.
- Parallel recording uses a task-based model with explicit command list APIs available.
- Lightweight validation may run during recording, with authoritative validation at submit/execute time.

## Dependencies

- Driver backends must expose a context for executing commands.
- Resource system must define lifetime and state transitions.

## Deliverables

- Command list types and submission path defined at the RHI layer.
- Clear execution semantics and validation hooks.
- RHI thread model and parallel recording guidelines described.
