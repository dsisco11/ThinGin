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

## Submission transport (RHI thread inbox)

When a dedicated RHI thread is enabled, the renderer's centralized submit path forwards work to that thread through an internal queue.

- The queue transports coarse-grained work items (submission batches), not individual draw/dispatch commands.
- The primary unit of transfer is an immutable recorded command list (or a bundle of command lists) plus metadata (queue selection, fence signaling, and optional presentation).
- Resource lifetime actions that must occur on the execution side (deferred destruction drains, backend-only housekeeping) are also enqueued as work items when needed.
- The default implementation uses `System.Threading.Channels` with a bounded channel to provide backpressure (configurable by frames-in-flight and/or memory budget).
- The queue is a single-reader (the RHI thread). Writers should be serialized through the centralized submit path so the implementation can use a single-writer configuration where possible.
- The transport mechanism is an internal detail and should be abstracted behind a small interface so it can be swapped later if profiling shows it is a bottleneck.

## Dependencies

- Driver backends must expose a context for executing commands.
- Resource system must define lifetime and state transitions.

## Deliverables

- Command list types and submission path defined at the RHI layer.
- Clear execution semantics and validation hooks.
- RHI thread model and parallel recording guidelines described.

## Lifecycle and ordering

- Command allocators are per-thread and reset after GPU completion or a frame fence.
- Submission order is preserved within each pipeline.
- Cross-pipeline ordering requires explicit fences or transitions.
- Frame boundaries are defined by present and RHI frame counters.

## Thread-safety and ownership

- Command list recording is thread-safe by design; resource creation is serialized through the RHI.
- RHI objects are owned by the RHI and released through deferred destruction.
- Backend contexts are not shared across threads without explicit synchronization.
