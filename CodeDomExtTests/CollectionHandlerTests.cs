using CodeDomExt.Generators;
using CodeDomExtTests.TestClasses;
using Xunit;

namespace CodeDomExtTests
{
    public class CollectionHandlerTests
    {
        [Fact]
        public void Test()
        {
            var collectionHandler = new ChainOfResponsibilityHandler<DerivedClass>();
            var handler1 = new PredeterminedCodeObjectHandler<BaseClass>(false);
            var handler2 = new PredeterminedCodeObjectHandler<DerivedClass>(true);
            var handler3 = new PredeterminedCodeObjectHandler<DerivedClass>(false);
            var handler4 = new PredeterminedCodeObjectHandler<BaseClass>(true);
            var obj = new DerivedClass();
            
            collectionHandler.AddHandler(handler1);
            Assert.Throws<ObjectUnhandledException>(() => collectionHandler.Handle(obj, null));
            Assert.True(handler1.TimesCalled == 1);
            collectionHandler.AddHandler(handler2);
            Assert.True(collectionHandler.Handle(obj, null));
            Assert.True(handler1.TimesCalled == 1);
            Assert.True(handler2.TimesCalled == 1);
            collectionHandler.AddHandler(handler3);
            Assert.True(collectionHandler.Handle(obj, null));
            Assert.True(handler1.TimesCalled == 1);
            Assert.True(handler2.TimesCalled == 2);
            Assert.True(handler3.TimesCalled == 1);
            collectionHandler.AddHandler(handler4);
            Assert.True(collectionHandler.Handle(obj, null));
            Assert.True(handler1.TimesCalled == 1);
            Assert.True(handler2.TimesCalled == 2);
            Assert.True(handler3.TimesCalled == 1);
            Assert.True(handler3.TimesCalled == 1);

            Assert.False(new ChainOfResponsibilityHandler<BaseClass>(false).Handle(null, null));
        }
    }
}