## [2.2.0] - 04/07/2018

### Changed
- Removed newline after statements

### Fixed
- Common.DefaultTypeMemberHandler stack handling
- Iterations are now handled as while also when the init and increment statements are empty CodeSnippettStatement

## [2.1.0] - 26/06/2018

### Added
- Can now add extra context object when calling Generate methods

### Fixed
- VB generator not addin "_" after custom attributes in type declaration
- VB for handling

## [2.0.0] - 18/06/2018
Warning: This version may break some of your custom ICodeObjectHandler. If you were only using default implementations nothing should have been broken.

### Changed
- Changes to statement termination
- Changes to common ICodeObjectHandler implementation
- Lowered target framework version

### Added
- CodeDirectiveGeneration; right now only region directives are supported
- String ICodeWriter
- Static imports
- Option for redundant parentheses

### Fixed
- VB direction expression generation, now matches codedom
- Reduced number of redundant [] in vb identifiers generation