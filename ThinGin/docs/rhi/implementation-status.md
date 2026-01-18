# Implementation Status (as of 2026-01-17)

This document inventories RHI-related systems currently present in the repository, and describes (a) what is implemented, (b) what appears partially implemented, and (c) what is present as API scaffolding.

## High-level architecture: two parallel tracks

### Track A: Legacy engine/provider system (works today)

The OpenGL backend includes a full “engine” implementation that binds OpenGL directly via OpenTK:

- `ThinGin.OpenGL/Common/Engine/GLEngineBase.cs`
  - Owns OpenGL state setup, capability toggles, framebuffer binding, and error checks.
  - Implements `EngineInstance` (core engine abstraction).

This track is wired into the `IGraphicsImplementation` provider model:

- `ThinGin/Core/Common/Interfaces/IGraphicsImplementation.cs`
- `ThinGin.OpenGL/GL2/Implementation.cs`
- `ThinGin.OpenGL/GL3/Implementation.cs`

This system is **engine-centric** (the engine calls GL directly via provider classes), rather than **driver-centric** (RHI commands recorded and submitted to a driver).

### Track B: New driver-centric RHI core (in progress)

The newer RHI effort lives under:

- `ThinGin/Core/RenderHardware/`

Key observations:

- The codebase contains explicit driver-centric terminology (e.g., the `IRHIDriver` comment referencing a dynamic RHI concept).
- Many types are present (textures, buffers, pipeline init structs, fences), but **most implementations are still placeholders**.
- The new RHI core is **not fully integrated** with the legacy engine/provider runtime.

## Core RHI layer (ThinGin/Core/RenderHardware)

### Root interfaces and entry points

- `IRHI` (`ThinGin/Core/RenderHardware/Core/IRHI.cs`)
  - Minimal: `CurrentFrame`, `Resources`, `Shutdown()`.

- `RenderHardwareInterface` (`ThinGin/Core/RenderHardware/Core/RenderHardwareInterface.cs`)
  - Intended as the “facade” over an RHI driver.
  - Current implementation is extremely minimal (stores driver, creates a resource manager, `Shutdown()` stub).

- `IRHIDriver` (`ThinGin/Core/RenderHardware/Interfaces/IRHIDriver.cs`)
  - Intended as the driver interface surface.
  - Already contains real API commitments:
    - Feature/limit reporting (`RHIDriverFeatures`)
    - Extension queries
    - Pixel format support/translation
    - Error checking
    - Binding (shader/sampler/uniform buffer/GBuffer/buffer)
    - Buffer upload and mapping (lock/unlock)
    - Texture creation APIs (many variants)
    - Clearing operations
  - **Important:** no concrete `IRHIDriver` implementation was found in `ThinGin.OpenGL` yet.

### Resource lifetime and management

- `RHIResource` (`ThinGin/Core/RenderHardware/Resources/RHIResource.cs`)
  - This is one of the more complete pieces:
    - Implements `IDisposable` and a finalizer.
    - Supports lazy init/update/release through delegates (`RHIDelegate`).
    - Integrates with an `RHIResourceManager` queue for deferred work.

- `RHIResourceManager` (`ThinGin/Core/RenderHardware/Core/RHIResourceManager.cs`)
  - Tracks resources and provides `LazyInit`, `LazyUpdate`, `LazyRelease` queues.
  - `Process()` drains queues and calls `TryInitialize/TryUpdate/TryRelease`.
  - **Status:** functional queueing logic, but depends on resources providing delegates or their own init path.

### Resource type hierarchy

The RHI resource taxonomy is largely present (even if many methods are placeholders):

- Handles and native object association
  - `RHIHandle` (`ThinGin/Core/RenderHardware/Resources/RHIHandle.cs`) is a base abstraction.

- Textures
  - Base: `RHITexture` + typed derivatives for 1D/2D/3D/cube/arrays.
  - `RHITexture` includes a native-handle field and helpers (`Get_NativeResource`, `Set_NativeResource`).
  - Many lifetime delegates return `null` currently (implying self-managed init), which is likely temporary.

- Buffers
  - `RHIVertexBuffer`, `RHIIndexBuffer`, `RHIUniformBuffer`, `RHIStructuredBuffer`, `RHIStagingBuffer`.
  - Most are “shape-only” abstractions today.

- Views
  - `RHIShaderResourceView`, `RHIUnorderedAccessView`, `RHIViewObject`.
  - Currently minimal shells.

- Rasterizer state
  - `RHIRasterizerState` and `RasterizerStateConfig` exist.

- Queries
  - `RHIRenderQuery` and pool types exist.

- Ray tracing
  - `RHIRayTracingGeometry`, `RHIRayTracingScene` are present as shells.

### Pipelines and state objects

Modern pipeline init types exist:

- `GraphicsPipelineStateInit` (`ThinGin/Core/RenderHardware/Pipelines/GraphicsPipelineStateInit.cs`)
- `BoundShaderStateInput`, `DepthStencilStateInit`, `ExclusiveDepthStencilAccess`, etc.

Pipeline state classes exist but are largely unimplemented:

- `RHIGraphicsPipelineState` (abstract shell)
- `RHIGraphicsPipelineStateFallback` (present)
- `RHIBlendState`, `RHIDepthStencilState` (present)

### Commands and submission model

- Command abstraction: `RHICommand` with `Execute()`.
- Command lists:
  - `RHICommandList` stores a list.
  - `RHICommandListImmediate` stores a concurrent queue and `Flush()` executes commands.

Current gap:

- `RHICommand.Execute()` has **no driver/context parameter**, so commands cannot naturally call into an `IRHIDriver` instance without capturing it externally.
- There is no “record then submit” boundary yet (no `Submit`, no command allocator model, no parallel command lists).

### Synchronization

- `RHIFenceObject` and related classes exist.
- `RHIResourceTransition` exists but is currently an empty abstraction.

### DMA / mapping

- `DMAInterface` exists as the start of a buffer mapping / staging interface.
- Several methods are not implemented yet.

## OpenGL backend: where it intersects with the new RHI

### OpenGL RHI handle type

- `RHIOpenGLResourceHandle` (`ThinGin.OpenGL/Common/RenderHardware/RHIOpenGLResourceHandle.cs`)
  - Wraps an `int` OpenGL object handle.

### OpenGL RHI resources (partial)

Under `ThinGin.OpenGL/Common/RenderHardware/Resources/`:

- `OpenGLTexture2D`, `OpenGLTexture3D`, array/cube variants
- `OpenGLSamplerState`

These primarily:

- Subclass the new RHI texture/sampler types.
- Store an `OpenGLTextureContext` (target, flags, etc.).

Current gap:

- These classes do not currently supply initializer/releaser delegates, so they do not actually allocate GL objects via the new RHI lifecycle.

### OpenGL fences (stub)

- `OpenGLFenceObject` (`ThinGin.OpenGL/OpenGL/RenderHardware/Synchronization/OpenGLFenceObject.cs`)
  - Exists but throws `NotImplementedException` for initializer/releaser/poll.

## Rendering-adjacent code that appears to predate the new IRHI

Several rendering-facing types inherit from `RHIResource` but are constructed with an engine object (not an `IRHI`), indicating the RHI integration is still mid-refactor.

Example:

- `ThinGin/Core/Rendering/GBuffer.cs` extends `RHIResource` and calls methods like `RHI.Bind_Framebuffer(...)`.
- `EngineInstance` exposes framebuffer binding methods (`Bind_Framebuffer`, `Unbind_Framebuffer`), but `IRHI` does not.

This is a strong signal that the repo currently has:

- A legacy engine API (framebuffers, binding, etc.)
- A new RHI API with different ownership boundaries

…and call-sites are not yet consistently migrated.

## Notable “incomplete by construction” indicators

The repository contains many stubs or placeholders relevant to RHI completion:

- Methods with empty bodies or missing returns (e.g., `RHIShaderLibrary`, `DMAInterface`).
- `NotImplementedException` thrown by RHI-related OpenGL types (e.g., `OpenGLFenceObject`).

This matters because it affects what can realistically be considered “implemented” vs “API skeleton”.

## Build status snapshot

A `dotnet build` of `ThinGin.sln` currently fails in the `ThinGin` project with errors unrelated to the new RHI driver work, but directly relevant to the ongoing “old texture API → RHITexture” migration:

- `ThinGin/Core/Common/Textures/Types/Texture.cs`: `Texture` does not implement `ITexture.Handle` and `ITexture.Metadata`.

There are also multiple warnings indicating `ITexture` is obsolete in favor of `RHITexture`, which aligns with the repo’s direction but confirms that the migration is incomplete.

## Summary of what is truly implemented today

**Implemented (usable as-is):**

- Legacy OpenGL engine path (`GLEngineBase`) and provider-based rendering infrastructure.
- Core `RHIResource` lifecycle state machine + queued processing via `RHIResourceManager`.

**Partially implemented:**

- Command-list scaffolding (queue/list + immediate flush).
- RHI type taxonomy (textures/buffers/views/states) as a set of abstractions.
- OpenGL-specific RHI resource classes that currently store context/metadata.

**Primarily scaffolding:**

- `RenderHardwareInterface` facade.
- `IRHIDriver` contract (large surface area, but no backend implementation yet).
- Resource transitions/barriers.
- Pipeline state objects and caches.
- Shader library and pipeline binary library functionality.
- DMA mapping/staging helpers.

For "what to implement next" in a recommended order, see [Gaps and Roadmap](gaps-and-roadmap.md).
