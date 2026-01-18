# Pipeline State

## Scope

Pipeline state objects encapsulate shader programs and fixed-function state into immutable, cacheable objects.

## Responsibilities

- Graphics and compute pipeline descriptors.
- Pipeline layout or root signature equivalent.
- State validation, hashing, and caching.
- Backend translation of pipeline states.

## Architectural decisions

- Pipeline state objects have an immutable core and a defined dynamic state set.
- The immutable core (shaders + fixed-function state) is keyed by a stable descriptor hash.
- Dynamic state includes viewport, scissor, stencil reference, and blend factors.
- Shader reflection informs pipeline layout and binding validation.
- OpenGL uses pipeline state as a cached state bundle rather than a native object.
- Cache supports runtime and offline/serialized PSO libraries.

## Deliverables

- Graphics pipeline state definition with a cache strategy.
- Compute pipeline state definition even if backend support is phased.
- Pipeline layout contract shared with the descriptor system.

## Cache ownership

- Pipeline caches are owned by the RHI and keyed by backend and PSO hash.
- Offline cache artifacts are versioned and invalidated on shader or layout changes.
- Cache artifacts are also invalidated on backend or driver version changes.
