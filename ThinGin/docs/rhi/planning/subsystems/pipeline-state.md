# Pipeline State

## Scope

Pipeline state objects encapsulate shader programs and fixed-function state into immutable, cacheable objects.

## Responsibilities

- Graphics and compute pipeline descriptors.
- Pipeline layout or root signature equivalent.
- State validation, hashing, and caching.
- Backend translation of pipeline states.

## Architectural decisions

- Pipeline state objects are immutable and keyed by a stable descriptor.
- Shader reflection informs pipeline layout and binding validation.
- OpenGL uses pipeline state as a cached state bundle rather than a native object.

## Deliverables

- Graphics pipeline state definition with a cache strategy.
- Compute pipeline state definition even if backend support is phased.
- Pipeline layout contract shared with the descriptor system.
