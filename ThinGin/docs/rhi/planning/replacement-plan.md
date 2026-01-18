# Replacement Plan (No Backward Compatibility)

## Current state

- Legacy engine and provider path drives OpenGL directly.
- New RHI core exists as types and scaffolding with no driver implementation.
- Rendering code mixes both ownership models.

## Target state

- Driver-centric RHI is the sole graphics entry point.
- Backend drivers implement `IRHIDriver` and own native resources.
- Legacy engine and provider path is removed from the active build.

## Strategy

- Implement the RHI as a clean-slate path; do not wrap the legacy engine.
- Use legacy code only as reference when useful; do not maintain compatibility adapters.

## Replacement sequence

- Establish the new RHI boundary and command model.
- Implement a minimal OpenGL driver and prove a vertical slice.
- Migrate rendering entry points such as GBuffer, texture creation, and draw submission.
- Remove the legacy provider pathway from the build once sample content renders through the RHI.
- Consolidate resource managers into the RHI-owned system.

## Removal policy for legacy path

- Remove or archive legacy rendering code as soon as the RHI path can render basic scenes.
- Avoid dual systems; no compatibility layer or fallback path remains in production.

## Risks and mitigations

- Risk: duplicated systems increase churn.
  - Mitigation: keep a strict boundary and delete legacy paths early.
- Risk: loss of proven behavior during replacement.
  - Mitigation: validate each vertical slice with simple render tests before adding more features.
