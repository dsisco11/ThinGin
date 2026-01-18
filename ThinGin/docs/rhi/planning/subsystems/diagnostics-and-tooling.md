# Diagnostics and Tooling

## Scope

Diagnostics provide visibility into GPU work, errors, and performance.

## Responsibilities

- Debug markers and event scopes for GPU tools.
- Validation hooks and error reporting.
- Performance counters and memory reporting.
- Backend debug and validation integration.

## Architectural decisions

- Diagnostics are optional but consistent across backends.
- Validation is layered and can be enabled per build configuration.

## Deliverables

- RHI-level debug marker API.
- Validation policy and error reporting surface.
- Baseline GPU stats and memory tracking.
