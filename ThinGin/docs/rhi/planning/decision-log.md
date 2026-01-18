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
