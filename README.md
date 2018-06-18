CodeDomExt aims to be an extension of [CodeDOM](https://docs.microsoft.com/en-GB/dotnet/framework/reflection-and-codedom/dynamic-source-code-generation-and-compilation), mainly focused on improving code generation from a CodeCompileUnit, representing the CodeDOM program as a graph.
# Extensibility
One of the focus of CodeDomExt is to allow users to define a new node, which can be used in the program graph, and specify how the code generator should handle it, without having to write a new generator nor having to modify CodeDomExt source code. This was reached through the use of the Chain-of-responsibility pattern.  
The main element of CodeDomExt is the generic interface `ICodeObjectHandler<T>`, which exposes a single method _Handle_, accepting an item of type T, and attempts to generate the code for it, returning if it was successfully generated.  
A `ChainOfResponsibilityHandler<T>` is an `ICodeObjectHandler<T>` which holds a collection of `ICodeObjectHandler<T>`, and in its _Handle_ method attempts to delegate the handling of an item to them, calling their _Handle_ method in a specific order, until one of them was able to handle the item.  
The code generators provided by the library contain multiple ChainOfResponsibilityHandlers, each capable of handling a specific group of nodes (e.g. one for type declaration, one for statements,...), and a user can add new handlers to any one of these.  
Read the wiki for more information on how to [customize a CodeGenerator](https://github.com/tremaluca/CodeDomExt/wiki/Customize-a-CodeGenerator)
# New nodes
There is a set of new CodeObjects providing support for generation of language features which are not supported by CodeDOM, like lambda expression and using statements.  
CodeDomExt provide support for generating these new nodes both in vb.net and c# programs.
# [Helpers](https://github.com/tremaluca/CodeDomExt/wiki/Helpers).
Helpers are utility classes which aim to improve the code responsible for the CodeDOM program graph generation, by making it more readable and easier to write.
# Download
CodeDomExt is available as a nuget package on [nuget.org](https://www.nuget.org/). CodeDomExt helpers are available as a separate package.
