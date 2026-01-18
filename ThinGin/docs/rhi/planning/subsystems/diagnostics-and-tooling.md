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
- Validation is layered (RHI + backend/native) where available.
- Diagnostics are gated by build configuration with runtime toggles for key features.
- Optional GPU crash/debug capture is supported when the backend allows it.

## Deliverables

- RHI-level debug marker API.
- Validation policy and error reporting surface.
- Baseline GPU stats and memory tracking.
- Profiling hook surface for GPU events and counters.

## Severity and reporting

- Define validation severities: info, warning, error, fatal.
- Errors fail fast in development builds; shipping builds log minimal errors.
- Validation output includes resource names and command context when possible.

## GPU crash capture flow

- When supported, capture GPU breadcrumbs and markers on device loss.
- Emit a structured crash report with recent markers and resource names.
- Enable crash capture via build config and runtime toggle.
