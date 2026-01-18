# Decision Log

This file records key architecture decisions that impact the RHI planning documents.

## Decision 001: No backward compatibility

- Status: accepted
- Rationale: the project is in design and has no downstream users.
- Implications: replace the legacy engine/provider path, avoid adapter layers, and keep the RHI clean-slate.

## Decision 002: Command model and execution boundary

- Status: accepted
- Decision: record and replay command lists (single-threaded) with a driver-owned `IRHICommandContext` as the execution boundary.
- Rationale: clear submission boundary and validation path without the complexity of multi-threaded recording.

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
- Details: validate bindings against layouts at pipeline creation and bind time; use transient ring buffers for per-frame descriptors and persistent pools for long-lived descriptors; bindless is optional and layered on top.

## Decision 006: Pipeline state model and caching

- Status: accepted
- Decision: use an immutable pipeline core (shaders + fixed-function state) with a defined dynamic state set (viewport/scissor/stencil ref/blend factors).
- Details: cache PSOs by a stable initializer hash; start with runtime cache and allow an offline/serialized PSO library later; OpenGL maps PSOs to cached state bundles.

## Decision 007: Presentation ownership and frame pacing

- Status: accepted
- Decision: RHI owns presentation with a platform-provided surface and a swapchain/viewport abstraction.
- Details: RHI handles present/sync; platform layer supplies native window handles; frame pacing supports vsync control with room for explicit timing policies.

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
