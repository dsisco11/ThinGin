# Decision Log

This file records key architecture decisions that impact the RHI planning documents.

## Decision 001: No backward compatibility

- Status: accepted
- Rationale: the project is in design and has no downstream users.
- Implications: replace the legacy engine/provider path, avoid adapter layers, and keep the RHI clean-slate.

## Decision 002: Command model and execution boundary

- Status: accepted
- Decision: record and replay command lists with a driver-owned `IRHICommandContext` as the execution boundary.
- Rationale: clear submission boundary and validation path with a stable execution contract.

## Decision 003: Resource state model (explicit access and pipeline)

- Status: accepted
- Decision: use an explicit access and pipeline state model with subresource granularity and explicit transitions.
- Details: ERHIAccess-like bitmask states, per-pipeline tracking (graphics/async compute), subresource ranges for textures, and transition objects with extended flags (discard/clear/aliasing) and RHI-side validation.

## Decision 004: Shader pipeline and AST tooling

- Status: accepted
- Decision: use HLSL as the authoring language compiled to SPIR-V, with backend-specific translation as needed.
- Details: shader AST processing should leverage `TinyTokenizer`, `TinyPreprocessor`, and `TinyAst.Preprocessor` packages.

## Decision 005: Descriptor and binding model

- Status: accepted
- Decision: use reflection-driven descriptor layouts (descriptor tables) with per-frequency grouping, plus a slot-translation layer for OpenGL.
- Details: validate bindings against layouts at pipeline creation and bind time; use transient ring buffers for per-frame descriptors and persistent pools for long-lived descriptors; bindless is capability-gated and layered on top.

## Decision 006: Pipeline state model and caching

- Status: accepted
- Decision: use an immutable pipeline core (shaders + fixed-function state) with a defined dynamic state set (viewport/scissor/stencil ref/blend factors).
- Details: cache PSOs by a stable initializer hash; support runtime caches and offline/serialized PSO libraries; OpenGL maps PSOs to cached state bundles.

## Decision 007: Presentation ownership and frame pacing

- Status: accepted
- Decision: RHI owns presentation with a platform-provided surface and a swapchain/viewport abstraction.
- Details: RHI handles present/sync; platform layer supplies native window handles; frame pacing supports vsync control and explicit timing policies.

## Decision 008: Backend strategy and command contexts

- Status: accepted
- Decision: one active backend per platform/build, starting with OpenGL.
- Details: expose separate graphics and async-compute contexts in the API; backends that lack async compute map both pipelines to a single context and report capability flags.

## Decision 009: Resource lifetime and destruction

- Status: accepted
- Decision: use deferred destruction managed by the RHI with fence/epoch tracking, plus a frame-lag fallback when needed.
- Details: centralize lifetime control in the RHI resource manager; avoid immediate deletion while GPU work may still reference resources.

## Decision 010: Validation and diagnostics policy

- Status: accepted
- Decision: use layered validation (RHI validation + backend/native validation where available).
- Details: expose debug markers/events, error reporting, and GPU profiling hooks; gate by build configuration with runtime toggles for key diagnostics features; allow optional GPU crash/debug capture when supported.

## Decision 011: Upload and staging model

- Status: accepted
- Decision: hybrid upload model with staging resources for large/static data and ring-buffer updates for dynamic data.
- Details: use explicit copy commands for staging uploads; allow map/lock-style updates only where safe and backend-supported.

## Decision 012: Multithreaded recording and RHI thread

- Status: accepted
- Decision: support multithreaded command recording with a render thread and optional dedicated RHI thread.
- Details: use immediate and deferred command lists, allow task-based parallel recording, and submit through a centralized path with per-thread command allocators.

## Decision 013: Parallel recording API and validation timing

- Status: accepted
- Decision: use a task-based parallel recording model with explicit command list APIs available.
- Details: lightweight validation may run during recording in debug builds, with authoritative validation at submit/execute time.

## Decision 014: Platform surface contract for presentation

- Status: accepted
- Decision: use an extended surface contract provided by the platform/host layer.
- Details: host supplies native window handle plus sizing, DPI scaling, resize events, and present/vsync preferences; RHI owns swapchain/viewport and present.

## Decision 015: Backend ownership and context model

- Status: accepted
- Decision: backend owns device/context lifecycle and provides per-thread graphics and async-compute contexts.
- Details: contexts are managed by the driver with capability flags; backend debug layers integrate with layered validation.

## Decision 016: Native handle storage

- Status: accepted
- Decision: native GPU handles live in backend resource implementations; RHI exposes opaque references.

## Decision 017: Capability and feature reporting

- Status: accepted
- Decision: backends report structured capabilities and limits with feature flags used to gate optional functionality.
- Details: expose feature flags for async compute efficiency, bindless, ray tracing, and other optional systems; limits are explicit and queried via the driver.

## Decision 018: Implicit API barrier behavior

- Status: accepted
- Decision: APIs without explicit barriers treat transitions as logical ordering plus validation.
- Details: transitions are still recorded and validated; backends implement ordering and cache management as needed without native barrier primitives.

## Decision 019: Memory residency and allocator model

- Status: accepted
- Decision: use pooled allocators with transient aliasing for per-frame resources.
- Details: backend tracks budgets and residency; out-of-budget allocations fail fast unless a defined fallback exists.

## Decision 020: Fence and cross-queue synchronization

- Status: accepted
- Decision: expose timeline-style fences with binary fallback and explicit signal/wait ordering across queues.
- Details: cross-pipeline dependencies require explicit fences and pipeline-scoped transitions.

## Decision 021: Command list lifecycle and ordering

- Status: accepted
- Decision: command allocators are per-thread and reset after GPU completion or a frame fence.
- Details: submission order is preserved within each pipeline; cross-pipeline ordering requires explicit sync.

## Decision 022: Binding space conventions and descriptor lifetime

- Status: accepted
- Decision: reserve binding spaces by update frequency and define clear descriptor lifetimes.
- Details: space 0 per-frame, space 1 per-view, space 2 per-material, space 3 per-draw; transient tables reset each frame, persistent tables are reference-counted.

## Decision 023: Shader toolchain and permutation strategy

- Status: accepted
- Decision: compile HLSL with DXC to SPIR-V and use reflection-driven permutation keys.
- Details: permutations are driven by compile-time defines and stable hashes; debug info is included in development builds and minimized in release builds.

## Decision 024: Presentation swapchain policy

- Status: accepted
- Decision: default to triple buffering with fall back to double buffering and support vsync and immediate present modes.
- Details: prefer low-latency present modes when available; support sRGB by default with optional HDR; recreate swapchain on resize.

## Decision 025: Validation severity and crash capture

- Status: accepted
- Decision: define validation severities and capture GPU crash diagnostics where supported.
- Details: errors fail fast in development builds, shipping logs are minimal; crash capture records markers and resource names.

## Decision 026: Thread-safety and ownership

- Status: accepted
- Decision: command list recording is thread-safe; resource creation is serialized through the RHI.
- Details: backend contexts are not shared across threads without explicit synchronization.

## Decision 027: Cache invalidation policy

- Status: accepted
- Decision: shader and PSO cache artifacts are invalidated on backend, compiler, shader format, or driver version changes.

## Decision 028: RHI thread submission transport

- Status: accepted
- Decision: use `System.Threading.Channels` as the default transport for forwarding submissions to an optional dedicated RHI thread.
- Details: enqueue coarse-grained work items (command list batches, present, and execution-side housekeeping), not individual commands; use a bounded channel to provide backpressure; configure the queue as single-reader and prefer a centralized submit path to keep it single-writer where possible; keep the transport abstract so it can be replaced if profiling warrants.
