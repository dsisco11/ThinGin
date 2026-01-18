# Descriptors and Binding Model

## Scope

This plan defines how shaders bind textures, buffers, samplers, and other resources.

## Responsibilities

- Binding layout description that acts like a root signature or parameter layout.
- Descriptor allocation and update strategy.
- Compatibility with reflection data from shader compilation.
- Mapping to backend binding mechanisms.

## Architectural decisions

- Binding layouts are explicit and validated at pipeline creation.
- Descriptors are grouped by frequency such as per-frame, per-draw, and per-material.
- Bindless support is optional and layered on top of the core model.

## Deliverables

- Descriptor layout specification and validation rules.
- Allocation strategy for descriptors or binding tables.
- Backend mapping rules for OpenGL and future APIs.
