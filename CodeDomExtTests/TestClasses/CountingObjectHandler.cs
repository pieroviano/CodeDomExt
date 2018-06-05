using CodeDomExt.Generators;

namespace CodeDomExtTests.TestClasses
{
    public abstract class CountingObjectHandler<T> : ICodeObjectHandler<T>
    {
        public int TimesCalled { get; private set; } = 0;
        
        public bool Handle(T obj, Context ctx)
        {
            TimesCalled++;
            return ActualHandle(obj, ctx);
        }

        protected abstract bool ActualHandle(T obj, Context context);
    }
}