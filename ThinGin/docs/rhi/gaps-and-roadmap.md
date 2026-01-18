# Gaps and Roadmap

This document describes what still needs to be implemented to reach a driver-centric, explicit RHI system in C#, based on what exists in the repository today.

## Target architecture (driver-centric)

A driver-centric RHI typically stabilizes around these layers:

1. **High-level rendering (renderer, frame graph, passes)**
2. **RHI command recording API** (command lists/contexts)
3. **Backend driver interface** (D3D12/Vulkan/Metal/OpenGL)
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
- Expose graphics and async-compute contexts in the API, mapping both to a single context on backends without async compute.

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

**Stretch goal:** separate "RHI thread" / render thread model.
**Decision:** multithreaded recording with an optional RHI thread is part of the target model.

### 3) Resource lifecycle (real init/release)

**Status:** `RHIResource` lifecycle machinery exists and is solid; most concrete resources don’t yet allocate real GPU objects.

**Needed:**

- For each GPU-backed resource type (buffers, textures, samplers, shader programs):
  - Provide `Get_Initializer` / `Get_Releaser` delegates that call into the backend driver.
  - Decide whether resources store a native handle directly or via driver-owned tables.
- Centralize lifetime control in the RHI resource manager with deferred destruction and fence/epoch tracking.
- Implement the hybrid upload path: staging resources for large/static data and ring-buffer updates for dynamic data.

**Important design choice:**

- A driver-centric approach tends to keep RHI resources as small handles and pushes heavier data into driver-managed structures.
- ThinGin currently has both patterns starting (e.g., `RHIHandle` + OpenGL handle wrapper).

### 4) Pipeline State Objects (PSO) / fixed-function state

**Status:** Pipeline init structs and state class shells exist.

**Needed:**

- Define a real `RHIGraphicsPipelineState` with an immutable core and defined dynamic state set.
- Implement hashing and runtime PSO caching, with a path to offline/serialized caches.
- Translate PSOs to cached state bundles on OpenGL.

**Deliverable milestone:** A cached graphics pipeline state for a “simple mesh draw” (vertex+pixel shader, blend/depth/raster state).

### 5) Resource transitions / barriers

**Status:** `RHIResourceTransition` exists but is empty.

**Needed:**

- Implement an explicit access and pipeline state model:
  - ERHIAccess-like bitmask states (SRV/UAV/RTV/DSV/Copy/Present/etc).
  - Pipeline scope for graphics and async compute.
  - Subresource ranges for textures (mip/array/plane).
- Provide explicit transition commands with AccessBefore/After and extended flags (discard/clear/aliasing).

**Backend note:**

- OpenGL has fewer explicit barriers than D3D12/Vulkan. Your OpenGL driver may implement "logical barriers" as validation + ordering only.

### 6) Views, descriptors, and binding model

**Status:** SRV/UAV types exist as shells.

**Needed:**

- Implement the chosen binding model:
  - Reflection-driven descriptor layouts grouped by frequency.
  - Descriptor tables as the primary model, with OpenGL slot translation.
  - Bindless as an optional layer.
  - Transient ring buffers for per-frame descriptors and persistent pools for long-lived descriptors.

For a cross-API RHI, consider defining:

- `RHIShaderParameterBindings` / root signature equivalent
- "descriptor" types (sampler/texture/buffer binding)

### 7) Shader system: compilation, reflection, libraries

**Status:** Shader types exist, plus `RHIShaderLibrary` / `RHIPipelineBinaryLibrary` placeholders.

**Needed:**

- Implement the chosen shader pipeline:
  - HLSL authoring compiled to SPIR-V, with backend-specific translation as needed.
  - Use `TinyTokenizer`, `TinyPreprocessor`, and `TinyAst.Preprocessor` for AST processing.

Core missing pieces:

- Shader compilation/build step
- Reflection (uniforms, blocks, textures)
- Caching keys (`ShaHash` exists)
- "Library" implementation or replacement (currently has incomplete methods)

### 8) Presentation / swapchain / viewport

**Status:** RHI viewport types exist (`RHIViewport`, `RHIViewportPresenter`) but no clear runtime wiring.

**Needed:**

- Define a platform surface abstraction and RHI-owned swapchain/viewport.
- Implement present scheduling and frame pacing (vsync first, explicit timing later).

If the current examples rely on OpenTK's window/context management, align the host layer to supply window handles while the RHI owns presentation.

### 9) Diagnostics and validation

**Status:** Diagnostics types exist but lack a unified policy.

**Needed:**

- Layered validation (RHI + backend/native) with consistent error reporting.
- Debug markers/events, GPU profiling hooks, and baseline stats.
- Build-config gating with runtime toggles for key diagnostics features.

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

### Milestone 4: "Explicit RHI features"

- Resource barriers and transitions (explicit access+pipeline + validation).
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

If you want fastest forward progress toward the target RHI semantics:

1. Implement `IRHIDriver` for OpenGL (GL3).
2. Make command execution able to call the driver.
3. Convert one vertical slice end-to-end (shader + vertex buffer + draw) using the new RHI.

Once that slice exists, remove the legacy provider-based renderer and build forward exclusively on the new RHI.
