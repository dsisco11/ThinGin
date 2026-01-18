# Glossary

Short definitions for terms used in the RHI planning documents.

- HLSL: High-Level Shading Language used to author GPU shaders.
- SPIR-V: A binary intermediate representation for shaders used for compilation, reflection, and backend translation.
- TinyTokenizer: Tokenizer used for shader source analysis and AST construction.
- TinyPreprocessor: Preprocessor used to expand macros and includes before AST processing.
- TinyAst.Preprocessor: Preprocessor AST utilities used to build and analyze shader ASTs.
- Timeline fence: A fence that uses monotonically increasing values for signal/wait.
- Present mode: The swapchain presentation policy (vsync, immediate, mailbox).
- Binding space: A logical descriptor namespace used to group resources by update frequency.
- Mailbox present: A low-latency present mode that replaces queued frames with the latest.
- Descriptor table: A grouped set of resource bindings described by a layout and updated together.
