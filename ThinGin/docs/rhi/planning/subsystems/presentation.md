# Presentation and Swapchain

## Scope

Presentation handles how rendered images reach the screen and how the RHI interacts with windowing.

## Responsibilities

- Surface and swapchain creation.
- Frame pacing, vsync control, and resize handling.
- Multi-window support and headless rendering options.

## Architectural decisions

- Presentation is part of the RHI, with a platform abstraction for windowing.
- The swapchain model supports at least double buffering with explicit present timing.
- Presentation APIs should allow external window systems without rewriting the driver.

## Deliverables

- RHI viewport or swapchain interface with clear ownership.
- Presentation scheduling semantics and frame pacing options.
