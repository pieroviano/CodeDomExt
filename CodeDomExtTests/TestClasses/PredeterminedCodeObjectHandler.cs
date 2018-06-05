using CodeDomExt.Generators;

namespace CodeDomExtTests.TestClasses
{
    public class PredeterminedCodeObjectHandler<T> : CountingObjectHandler<T>
    {
        private bool result;
        
        public PredeterminedCodeObjectHandler(bool result)
        {
            this.result = result;
        }
        
        protected override bool ActualHandle(T obj, Context context)
        {
            return result;
        }
    }
}