# Resource System

## Scope

The resource system defines how buffers, textures, samplers, and views are described, created, and tracked.

See [Resource State Model](resource-state-model.md) for the explicit state and transition definitions.

## Responsibilities

- Resource descriptors and immutable creation parameters.
- Resource handles and backend-owned native objects.
- Lifetime management, including deferred init and release.
- Views and subresource ranges for shader and render bindings.
- Resource state tracking and transitions, including initial state on creation.

## Architectural decisions

- Resources are thin handles with backend-owned data.
- The RHI resource manager is the single source of lifetime control.
- Creation is explicit, and updates are done via command lists or staging resources.
- Resource states are explicit: access masks plus pipeline scope with subresource ranges for textures.
- Resource descriptors carry an explicit initial access state.
- Destruction is deferred and synchronized with GPU work via fences or frame-lag tracking.
- Data uploads use a hybrid model: staging resources for large/static data plus ring-buffer updates for dynamic data.
- Native handles live in backend resource implementations; the RHI exposes opaque references.
- GPU memory is allocated from pooled heaps with support for transient aliasing where safe.
- Backend tracks memory budgets and residency and exposes stats for diagnostics.
- Resources carry debug names for markers, validation, and crash capture.
- Resource creation is serialized through the RHI; updates are recorded on command lists.

## Memory and residency

- Use pooled allocators for long-lived resources and transient allocators for per-frame data.
- Track memory budgets in the backend and expose them via diagnostics.
- Fail fast on out-of-budget allocations unless a defined fallback exists.

## Deliverables

- Unified resource descriptor set covering buffers and textures.
- View descriptors for shader and render bindings, with subresource range definitions.
- Clear lifetime rules and ownership boundaries.
