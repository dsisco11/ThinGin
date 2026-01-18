# Descriptors and Binding Model

## Scope

This plan defines how shaders bind textures, buffers, samplers, and other resources.

## Responsibilities

- Binding layout description that acts like a root signature or parameter layout.
- Descriptor allocation and update strategy.
- Compatibility with reflection data from shader compilation.
- Mapping to backend binding mechanisms.

## Architectural decisions

- Binding layouts are reflection-driven and validated at pipeline creation and bind time.
- Descriptors are grouped by frequency such as per-frame, per-draw, and per-material.
- Descriptor tables are the primary model; OpenGL uses a slot-translation layer.
- Bindless support is capability-gated and layered on top of the core model.
- Descriptor allocation uses transient ring buffers for per-frame data and persistent pools for long-lived resources.

## Deliverables

- Descriptor layout specification and validation rules.
- Allocation strategy for descriptors or binding tables.
- Backend mapping rules for OpenGL and future APIs.

## Binding layout object (concept)

The binding layout object represents the shader-visible resource contract for a pipeline. It is created from shader reflection, validated against resource usage, and used to drive binding and slot translation.

Key properties:

- Immutable description of resource slots grouped by frequency (per-frame/per-material/per-draw).
- Explicit resource types per slot (SRV/UAV/CBV/sampler) and visibility (graphics/compute).
- Stable layout hash for pipeline caching and descriptor allocation.
- Optional aliasing or merging rules for compatible layouts.

Operational flow:

- Reflection builds the layout for each shader stage.
- Layouts are merged into a pipeline layout with stage visibility.
- Descriptor tables are allocated per layout and populated by the renderer.
- OpenGL backend translates layout slots to texture units and binding points at bind time.

## Binding space conventions

- Space 0: per-frame or global resources.
- Space 1: per-view or per-camera resources.
- Space 2: per-material resources.
- Space 3: per-draw resources.

## Descriptor lifetime and ownership

- Per-frame descriptor tables are transient and reset each frame.
- Persistent tables are reference-counted and released by the RHI.
- Descriptor updates are validated against the binding layout at update time.
