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
