# Shader System

## Scope

The shader system covers compilation, reflection, caching, and libraries.

## Responsibilities

- Define supported shader source languages and profiles.
- Provide reflection data for resources and inputs.
- Build a stable library and cache keyed by hashes.
- Integrate shader metadata with pipeline state creation.

## Architectural decisions

- Author in HLSL and compile to SPIR-V as the canonical intermediate.
- Reflection is mandatory for binding validation and descriptor layout.
- Shader libraries are versioned and backend-aware.
- Shader AST processing leverages `TinyTokenizer`, `TinyPreprocessor`, and `TinyAst.Preprocessor`.

## Pipeline flow (high level)

- Author HLSL source.
- Run preprocessing and tokenization via the Tiny* packages.
- Build a lightweight AST for analysis and metadata extraction.
- Compile to SPIR-V and generate reflection data.
- Cache compiled binaries and reflection metadata by hash and backend.

## Deliverables

- Shader compilation pipeline specification.
- Reflection schema and binding metadata.
- Library and cache strategy aligned with the pipeline cache.
- AST processing workflow based on the Tiny* packages.
