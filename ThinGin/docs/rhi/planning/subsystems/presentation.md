# Presentation and Swapchain

## Scope

Presentation handles how rendered images reach the screen and how the RHI interacts with windowing.

## Responsibilities

- Surface and swapchain creation.
- Frame pacing, vsync control, and resize handling.
- Multi-window support and headless rendering options.

## Architectural decisions

- Presentation is owned by the RHI, with a platform-provided surface and windowing abstraction.
- The swapchain/viewport model supports double buffering or better with explicit present timing.
- The platform layer supplies native window handles; the RHI controls present and sync.
- Presentation APIs should allow external window systems without rewriting the driver.
- Frame pacing supports vsync control and explicit timing policies.
- The platform surface contract includes size, DPI scaling, resize events, and present preferences.

## Swapchain policy

- Default to triple buffering when available; fall back to double buffering.
- Support vsync and immediate present modes where available.
- Prefer mailbox or low-latency present modes when supported.

## Color space and resize

- Default to sRGB color space; enable HDR when supported by the platform and backend.
- Recreate the swapchain on resize and rebind dependent resources.
- Handle minimize and zero-size surfaces gracefully by pausing present.

## Deliverables

- RHI viewport or swapchain interface with clear ownership.
- Presentation scheduling semantics and frame pacing options.
