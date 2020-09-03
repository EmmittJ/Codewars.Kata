using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Codewars.EsolangInterpreters.Boolfuck
{
    [TestFixture]
    public class BoolfuckTests
    {
        [Test]
        public void testEmpty()
        {
            Assert.AreEqual("", Boolfuck.interpret("", ""));
            //Assert.AreEqual("", Boolfuck.interpret(Brainfuck.toBoolfuck(""), ""));
        }
        [Test]
        public void testSingleCommands()
        {
            Assert.AreEqual("", Boolfuck.interpret("<", ""));
            Assert.AreEqual("", Boolfuck.interpret(">", ""));
            Assert.AreEqual("", Boolfuck.interpret("+", ""));
            Assert.AreEqual("", Boolfuck.interpret(".", ""));
            Assert.AreEqual("\u0000", Boolfuck.interpret(";", ""));
        }
        [Test]
        public void testIO()
        {
            Assert.AreEqual("*", Boolfuck.interpret(">,>,>,>,>,>,>,>,<<<<<<<<>;>;>;>;>;>;>;>;<<<<<<<<", "*"));
        }
        [Test]
        public void testHelloWorld()
        {
            Assert.AreEqual("Hello, world!\n", Boolfuck.interpret(";;;+;+;;+;+;+;+;+;+;;+;;+;;;+;;+;+;;+;;;+;;+;+;;+;+;;;;+;+;;+;;;+;;+;+;+;;;;;;;+;+;;+;;;+;+;;;+;+;;;;+;+;;+;;+;+;;+;;;+;;;+;;+;+;;+;;;+;+;;+;;+;+;+;;;;+;+;;;+;+;+;", ""));
        }

        [Test]
        public void testBasic()
        {
            Assert.AreEqual("Codewars", Boolfuck.interpret(">,>,>,>,>,>,>,>,<<<<<<<[>]+<[+<]>>>>>>>>>[+]+<<<<<<<<+[>+]<[<]>>>>>>>>>[+<<<<<<<<[>]+<[+<]>>>>>>>>>+<<<<<<<<+[>+]<[<]>>>>>>>>>[+]<<<<<<<<;>;>;>;>;>;>;>;<<<<<<<,>,>,>,>,>,>,>,<<<<<<<[>]+<[+<]>>>>>>>>>[+]+<<<<<<<<+[>+]<[<]>>>>>>>>>]<[+<]", "Codewars\u00ff"));
            Assert.AreEqual("Codewars", Boolfuck.interpret(">,>,>,>,>,>,>,>,>+<<<<<<<<+[>+]<[<]>>>>>>>>>[+<<<<<<<<[>]+<[+<]>;>;>;>;>;>;>;>;>+<<<<<<<<+[>+]<[<]>>>>>>>>>[+<<<<<<<<[>]+<[+<]>>>>>>>>>+<<<<<<<<+[>+]<[<]>>>>>>>>>[+]+<<<<<<<<+[>+]<[<]>>>>>>>>>]<[+<]>,>,>,>,>,>,>,>,>+<<<<<<<<+[>+]<[<]>>>>>>>>>]<[+<]", "Codewars"));
            Assert.AreEqual("\u0048", Boolfuck.interpret(">,>,>,>,>,>,>,>,>>,>,>,>,>,>,>,>,<<<<<<<<+<<<<<<<<+[>+]<[<]>>>>>>>>>[+<<<<<<<<[>]+<[+<]>>>>>>>>>>>>>>>>>>+<<<<<<<<+[>+]<[<]>>>>>>>>>[+<<<<<<<<[>]+<[+<]>>>>>>>>>+<<<<<<<<+[>+]<[<]>>>>>>>>>[+]>[>]+<[+<]>>>>>>>>>[+]>[>]+<[+<]>>>>>>>>>[+]<<<<<<<<<<<<<<<<<<+<<<<<<<<+[>+]<[<]>>>>>>>>>]<[+<]>>>>>>>>>>>>>>>>>>>>>>>>>>>+<<<<<<<<+[>+]<[<]>>>>>>>>>[+<<<<<<<<[>]+<[+<]>>>>>>>>>+<<<<<<<<+[>+]<[<]>>>>>>>>>[+]<<<<<<<<<<<<<<<<<<<<<<<<<<[>]+<[+<]>>>>>>>>>[+]>>>>>>>>>>>>>>>>>>+<<<<<<<<+[>+]<[<]>>>>>>>>>]<[+<]<<<<<<<<<<<<<<<<<<+<<<<<<<<+[>+]<[<]>>>>>>>>>[+]+<<<<<<<<+[>+]<[<]>>>>>>>>>]<[+<]>>>>>>>>>>>>>>>>>>>;>;>;>;>;>;>;>;<<<<<<<<", "\u0008\u0009"));
        }
    }
}
