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

## Deliverables

- Unified resource descriptor set covering buffers and textures.
- View descriptors for shader and render bindings, with subresource range definitions.
- Clear lifetime rules and ownership boundaries.
