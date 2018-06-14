using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using CodeDomExt.Generators.Csharp;
using CodeDomExt.Generators.VisualBasic;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators
{
    /// <summary>
    /// A class that will be passed through handlers; provides information about what has been generated.
    /// </summary>
    public class Context
    {
        /// <summary>
        /// The code writer that will be used to output code
        /// </summary>
        public ICodeWriter Writer { get; }
        /// <summary>
        /// The Handler provider (used by most handlers, when they need to delegate part of their handling to other handlers 
        /// </summary>
        public ICodeHandlerProvider HandlerProvider { get; }
        /// <summary>
        /// The code generator options
        /// </summary>
        public GeneratorOptions Options { get; }
        /// <summary>
        /// The current level of indentation
        /// </summary>
        public int Indentation { get; private set; } = 0;
        /// <summary>
        /// Stack containing information about the <see cref="CodeTypeDeclaration"/>s being handled
        /// </summary>
        public Stack<Tuple<DeclarationType, CodeTypeDeclaration>> TypeDeclarationStack { get; } = new Stack<Tuple<DeclarationType, CodeTypeDeclaration>>();
        /// <summary>
        /// The <see cref="DeclarationType"/> of the current <see cref="CodeTypeDeclaration"/>
        /// </summary>
        public DeclarationType CurrentDeclarationType => TypeDeclarationStack.Peek().Item1;
        /// <summary>
        /// The current <see cref="CodeTypeDeclaration"/> (of which members are being handled)
        /// </summary>
        public CodeTypeDeclaration CurrentCodeTypeDeclaration => TypeDeclarationStack.Peek().Item2;
        /// <summary>
        /// Stack containing information about the <see cref="MemberTypes"/> of the <see cref="CodeTypeMember"/>s being handled
        /// </summary>
        public Stack<MemberTypes> TypeMemberStack { get; } = new Stack<MemberTypes>();
        /// <summary>
        /// The <see cref="MemberTypes"/> of the <see cref="CodeTypeMember"/> currently being handled
        /// </summary>
        public MemberTypes CurrentTypeMember => TypeMemberStack.Peek();

        /// <summary>
        /// To be set before handling statements. If true the statement will halso handle its termination
        /// </summary>
        /// <seealso cref="CodeDomExt.Generators.Common.DefaultStatementHandler"/>
        public bool StatementShouldTerminate { get; set; } = true;
        
        /// <summary>
        /// The namespaces currently imported names (not escaped)
        /// </summary>
        public HashSet<string> ImportedNamespaces { get; } = new HashSet<string>();
        /// <summary>
        /// The current namespace name (not escaped)
        /// </summary>
        public string CurrentNamespace { get; set; }
        
        /// <summary>
        /// Set by member attributes handler, true if member is abstract
        /// </summary>
        public bool IsMemberAbstract { get; set; }
        
        /// <summary>
        /// CSharpContext, containing information used only by CSharpCodeProvider
        /// </summary>
        public CSharpContext CSharp { get; } = new CSharpContext();
        /// <summary>
        /// VisualBasicContext, containing information used only by VisualBasicCodeProvider
        /// </summary>
        public VisualBasicContext VisualBasic { get; } = new VisualBasicContext();
        
        /// <summary>
        /// Constructor for context; all arguments must be non null.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="options"></param>
        /// <param name="handlerProvider"></param>
        public Context(ICodeWriter writer, GeneratorOptions options, ICodeHandlerProvider handlerProvider)
        {
            Writer = writer ?? throw new ArgumentNullException(nameof(writer));
            Options = options ?? throw new ArgumentNullException(nameof(options));
            HandlerProvider = handlerProvider ?? throw new ArgumentNullException(nameof(handlerProvider));
        }

        /// <summary>
        /// Increases by one the level of indentation
        /// </summary>
        public void Indent()
        {
            Indentation += 1;
        }
        
        /// <summary>
        /// Decreases by one the level of indentation
        /// </summary>
        public void Unindent()
        {
            Indentation -= 1;
        }

        private readonly IDictionary<string, object> _userData = new Dictionary<string, object>();
        /// <summary>
        /// Stores an object of type T
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="ArgumentException">If an item of type T is already stored</exception>
        /// <typeparam name="T"></typeparam>
        public void AddUserData<T>(T data)
        {
            string key = typeof(T).FullName;
            if (_userData.ContainsKey(key))
            {
                throw new ArgumentException($"An object of type {key} is already present.");
            }

            _userData[key] = data;
        }

        /// <summary>
        /// Retrieves an object of type T
        /// </summary>
        /// <exception cref="ArgumentException">If no item of type T was previously added</exception>
        /// <typeparam name="T"></typeparam>
        public T GetUserData<T>()
        {
            string key = typeof(T).FullName;
            _userData.TryGetValue(key, out object res);
            if (res == null)
            {
                throw new ArgumentException($"No object of type {key} was found.");
            }

            return (T) res;
        }
    }
}