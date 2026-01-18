# Gaps and Roadmap

This document describes what still needs to be implemented to reach a UE5-like RHI system in C#, based on what exists in the repository today.

## Target architecture (UE5-inspired)

A UE5-like RHI typically stabilizes around these layers:

1. **High-level rendering (renderer, frame graph, passes)**
2. **RHI command recording API** (command lists/contexts)
3. **DynamicRHI / backend driver** (D3D12/Vulkan/Metal/OpenGL)
4. **Device objects** (buffers/textures/samplers/PSOs)
5. **Synchronization** (fences, semaphores, resource barriers)
6. **Shader system** (compilation, reflection, libraries/caches)

ThinGin currently has pieces of (2), (3) (interface only), (4) (type shells), and (5) (type shells), plus a legacy OpenGL engine that already does (3) directly.

## Biggest current gap: integration boundary

Right now the codebase appears to mix two ownership models:

- **Engine-centric model**: the engine (OpenGL engine) owns API state and directly issues GL calls.
- **RHI-centric model**: the engine records high-level RHI work, and a driver submits it.

With no backward compatibility requirements, avoid bridging. Choose the RHI-centric model and replace the legacy engine/provider path rather than wrapping it. Legacy code can be used as reference, but it should not be kept as an adapter layer.

## Gaps by subsystem

### 1) `IRHIDriver` backend(s)

**Status:** Interface exists; no concrete implementations found.

**Needed:**

- Implement `IRHIDriver` for at least one backend.
  - Pragmatic starting point: OpenGL (GL3) using OpenTK.
- Decide how the backend owns:
  - device/context lifecycle
  - per-thread contexts (if any)
  - debug/validation behavior

**Deliverable milestone:** A minimal backend that can:

- Create a `RHITexture2D` + bind it.
- Create a vertex buffer + upload data.
- Bind a shader + submit a draw.

### 2) Command recording and submission

**Status:** Command list classes exist, but the execution model can’t yet reach a driver cleanly.

**Decision:** record and replay command lists (single-threaded) with optional immediate execution layered on top.

**Needed:**

Concrete fixes required either way:

- Update `RHICommand.Execute()` to be able to operate on a driver/context.
  - Example shape: `Execute(IRHIDriver driver)` or `Execute(IRHICommandContext ctx)`.
- Add a submission boundary:
  - `Submit(RHICommandListBase list)` (or similar)
  - `Flush()` semantics for immediate lists vs deferred lists

**Stretch goal:** separate “RHI thread” / render thread model.

### 3) Resource lifecycle (real init/release)

**Status:** `RHIResource` lifecycle machinery exists and is solid; most concrete resources don’t yet allocate real GPU objects.

**Needed:**

- For each GPU-backed resource type (buffers, textures, samplers, shader programs):
  - Provide `Get_Initializer` / `Get_Releaser` delegates that call into the backend driver.
  - Decide whether resources store a native handle directly or via driver-owned tables.

**Important design choice:**

- UE5 tends to keep RHI resources “small handles” and pushes heavier data into driver-managed structures.
- ThinGin currently has both patterns starting (e.g., `RHIHandle` + OpenGL handle wrapper).

### 4) Pipeline State Objects (PSO) / fixed-function state

**Status:** Pipeline init structs and state class shells exist.

**Needed:**

- Define a real `RHIGraphicsPipelineState` that can be created/validated/cached.
- Decide how PSOs map onto OpenGL:
  - OpenGL does not have native PSOs like D3D12/Vulkan, so the driver will translate PSOs into a set of bound states.

**Deliverable milestone:** A cached graphics pipeline state for a “simple mesh draw” (vertex+pixel shader, blend/depth/raster state).

### 5) Resource transitions / barriers

**Status:** `RHIResourceTransition` exists but is empty.

**Needed:**

- Introduce a resource state model (at least logical states):
  - Common states: `RenderTarget`, `DepthWrite`, `ShaderRead`, `CopySrc`, `CopyDst`, etc.
- Provide transition commands / barrier batching.

**Backend note:**

- OpenGL has fewer explicit barriers than D3D12/Vulkan. Your OpenGL driver may implement “logical barriers” as validation + ordering only.

### 6) Views, descriptors, and binding model

**Status:** SRV/UAV types exist as shells.

**Needed:**

- Define how shaders bind resources:
  - UE5-style: descriptor tables / bindless options.
  - OpenGL-style: texture units + uniform locations + UBO binding points.

For a cross-API RHI, consider defining:

- `RHIShaderParameterBindings` / root signature equivalent
- “descriptor” types (sampler/texture/buffer binding)

### 7) Shader system: compilation, reflection, libraries

**Status:** Shader types exist, plus `RHIShaderLibrary` / `RHIPipelineBinaryLibrary` placeholders.

**Needed:**

- Decide on shader source and compilation pipeline:
  - GLSL only (initially), or
  - HLSL -> SPIR-V -> GLSL (or native Vulkan) later.

Core missing pieces:

- Shader compilation/build step
- Reflection (uniforms, blocks, textures)
- Caching keys (`ShaHash` exists)
- “Library” implementation or replacement (currently has incomplete methods)

### 8) Presentation / swapchain / viewport

**Status:** RHI viewport types exist (`RHIViewport`, `RHIViewportPresenter`) but no clear runtime wiring.

**Needed:**

- Define a platform abstraction for windowing/swapchain.
- Implement present scheduling and frame pacing.

If the current examples rely on OpenTK’s window/context management, decide whether:

- presentation belongs to the engine host (outside RHI), or
- presentation is an RHI responsibility (UE5-like).

## Recommended milestone plan (pragmatic)

### Milestone 1: “RHI can draw a triangle”

- Implement a minimal OpenGL `IRHIDriver`.
- Implement minimal resource creation (shader program, vertex buffer).
- Add a minimal command path (even if immediate-only) that reaches the driver.

### Milestone 2: “RHI can render a textured mesh”

- Textures: create/upload/bind.
- Samplers: implement binding.
- Add a tiny PSO object or equivalent cached state bundle.

### Milestone 3: “RHI supports render targets + depth”

- Implement `GBuffer`/render target creation through the RHI backend.
- Ensure consistent binding semantics (read/write, clear, blit/copy).

### Milestone 4: “UE-style features”

- Resource barriers (logical + validation).
- SRV/UAV and descriptor binding model.
- Asynchronous uploads via staging/DMA.
- Shader libraries / pipeline binary caching.
- Optional: multithreaded command recording.

## Known refactor hotspots (plan for churn)

- The type name `IRHI` currently represents the new RHI facade, but many call-sites appear to treat the engine instance as "the RHI". Update those call-sites to depend on a new RHI-owned object and remove the legacy engine pathway.

- There are multiple resource managers:
  - `ThinGin/Core/RenderHardware/Core/RHIResourceManager`
  - `ThinGin/Core/Engine/RenderManager.Objects`

Consider converging on one to avoid split lifetime control.

## What to do next

If you want fastest forward progress toward UE5-like RHI semantics:

1. Implement `IRHIDriver` for OpenGL (GL3).
2. Make command execution able to call the driver.
3. Convert one vertical slice end-to-end (shader + vertex buffer + draw) using the new RHI.

Once that slice exists, remove the legacy provider-based renderer and build forward exclusively on the new RHI.
