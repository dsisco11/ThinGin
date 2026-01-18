# Architecture Overview

## Goals

- Provide a driver-centric, explicit RHI that is C#-native and backend-agnostic.
- Separate renderer intent from backend API specifics.
- Keep resource and synchronization semantics explicit and portable.

## Layered model

Renderer and frame graph
  -> RHI frontend (resource descriptors, command lists, state models)
  -> RHI command contexts (execution boundary)
  -> RHI driver (backend translation and submission)
  -> Native API (OpenGL, Vulkan, D3D12, Metal)
  -> OS and windowing

## RHI frontend responsibilities

- Define resource descriptors, views, and lifetime semantics.
- Define command list and submission semantics.
- Define pipeline state, shader interfaces, and descriptor layouts.
- Track resource states and synchronization requirements at a logical level.

## Driver responsibilities

- Own device and context lifecycle and backend capability reporting.
- Create and destroy native resources from RHI descriptors.
- Translate pipeline state and descriptor layouts to backend-specific bindings.
- Execute command lists and manage submission, fences, and presentation.

## Cross-cutting architecture decisions

- Use handle-based resources with backend-managed native objects.
- Require explicit resource states and transitions in the RHI, even if some backends treat them as logical.
- Prefer immutable pipeline state objects with caching and hashing.
- Keep presentation as part of the RHI layer with a platform abstraction for surfaces.

## Integration boundary with higher-level rendering

- The renderer or frame graph schedules work and produces command lists.
- The RHI defines the submission and synchronization boundary, not the renderer.
- A render graph can be added without changing backend drivers if the RHI contracts are stable.

## Non-goals in this plan

- High-level renderer or frame graph design details.
- Platform windowing implementation details beyond the presentation boundary.
