using NUnit.Framework;

namespace Codewars.EsolangInterpreters.BefungeInterpreter
{
    public class BefungeInterpreterTests
    {
        [Test]
        public void Tests()
        {
            Assert.AreEqual("123456789", new BefungeInterpreter().Interpret(">987v>.v\nv456<  :\n>321 ^ _@"));
        }
        
        [Test]
        public void HelloWorld()
        {
            Assert.AreEqual("Hello World!\n", new BefungeInterpreter().Interpret(">25*\"!dlroW olleH\":v\n                v:,_@\n                >  ^"));
        }   
        
        [Test]
        public void Quine()
        {
            Assert.AreEqual("01->1# +# :# 0# g# ,# :# 5# 8# *# 4# +# -# _@", new BefungeInterpreter().Interpret("01->1# +# :# 0# g# ,# :# 5# 8# *# 4# +# -# _@"));
        }
    }
}
