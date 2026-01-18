# Resource System

## Scope

The resource system defines how buffers, textures, samplers, and views are described, created, and tracked.

## Responsibilities

- Resource descriptors and immutable creation parameters.
- Resource handles and backend-owned native objects.
- Lifetime management, including deferred init and release.
- Views and subresource ranges for shader and render bindings.
- Resource state tracking and transitions.

## Architectural decisions

- Resources are thin handles with backend-owned data.
- The RHI resource manager is the single source of lifetime control.
- Creation is explicit, and updates are done via command lists or staging resources.
- Resource states are part of the RHI contract even for APIs that lack explicit barriers.

## Deliverables

- Unified resource descriptor set covering buffers and textures.
- View descriptors for shader and render bindings.
- Clear lifetime rules and ownership boundaries.
