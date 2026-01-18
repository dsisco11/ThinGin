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

## Execution model

- Record and replay command lists with task-based parallel recording.
- Submit through a centralized path with per-thread command allocators.
- Support graphics and async-compute contexts, with optional dedicated RHI thread.
- See [Command Model](subsystems/command-model.md).

## Resource and data flow

- Resource lifetime is RHI-owned with deferred destruction and fence/epoch tracking.
- Uploads use a hybrid model: staging resources for large/static data and ring-buffer updates for dynamic data.
- Resource descriptors include explicit initial access state and subresource ranges.
- See [Resource System](subsystems/resource-system.md) and [Resource State Model](subsystems/resource-state-model.md).

## Shader and binding model

- Author in HLSL and compile to SPIR-V with reflection-driven layouts.
- Descriptor tables are the primary binding model, grouped by update frequency.
- OpenGL uses slot translation for binding; bindless is capability-gated.
- See [Shader System](subsystems/shader-system.md) and [Descriptors and Binding](subsystems/descriptors-and-binding.md).

## Validation and diagnostics

- Validation is layered (RHI + backend/native where available).
- Debug markers, events, and profiling hooks are exposed with build/runtime gating.
- See [Diagnostics and Tooling](subsystems/diagnostics-and-tooling.md).

## Fallback and error handling

- Capability-gated features fail fast with explicit validation errors by default.
- Documented fallbacks are used only when they preserve correctness.
- Fallback usage is recorded in diagnostics to avoid silent behavior changes.

## Capability gating checklist

- Query limits and feature flags at device creation and treat them as immutable.
- Gate optional features at resource/pipeline creation and bind time.
- Enforce limits early with clear validation errors.
- Define explicit fallback paths for unsupported features.

## Integration boundary with higher-level rendering

- The renderer or frame graph schedules work and produces command lists.
- The RHI defines the submission and synchronization boundary, not the renderer.
- A render graph can be added without changing backend drivers if the RHI contracts are stable.

## Non-goals in this plan

- High-level renderer or frame graph design details.
- Platform windowing implementation details beyond the presentation boundary.
- Multi-GPU and multi-adapter scheduling (deferred; single-device focus).
