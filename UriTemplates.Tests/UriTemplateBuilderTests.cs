﻿namespace Resta.UriTemplates.Tests
{
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class UriTemplateBuilderTests
    {
        [Test]
        public void BuildLiteralTest()
        {
            var builder = new UriTemplateBuilder();

            builder.Literal("abc");
            var actual = builder.Build().Template;
            Assert.AreEqual("abc", actual);

            builder.Literal("/def");
            actual = builder.Build().Template;
            Assert.AreEqual("abc/def", actual);
        }

        [Test]
        public void BuildExpressionTest()
        {
            var builder = new UriTemplateBuilder();
            builder.Simple(new VarSpec("test"));
            Assert.AreEqual("{test}", builder.Build().Template);
        }

        [Test]
        public void BuildExpressionsTest()
        {
            var builder = new UriTemplateBuilder();
            builder.Simple(new VarSpec("t1"), new VarSpec("t2"), new VarSpec("t3"));
            Assert.AreEqual("{t1,t2,t3}", builder.Build().Template);
        }

        [Test]
        public void BuildHighLevelExpressionTest(
            [Values('+', '#', '.', '/', ';', '?', '&')]
            char expressionOp)
        {
            var builder = new UriTemplateBuilder();
            builder.Expression(expressionOp, new VarSpec("test"));

            var expected = "{" + expressionOp + "test}";
            var actual = builder.Build().Template;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BuildHighLevelsExpressionTests()
        {
            var ops = new char[] { '+', '#', '.', '/', ';', '?', '&' };
            var builder = new UriTemplateBuilder();

            foreach (var op in ops)
            {
                builder.Expression(op, new VarSpec("test"));
            }

            var actual = string.Join(string.Empty, ops.Select(x => "{" + x + "test}").ToArray());

            Assert.AreEqual(actual, builder.Build().Template);
        }

        [Test]
        public void BuildExampleTest()
        {
            var template = new UriTemplateBuilder()
                .Literal("http://example.org/")
                .Simple(new VarSpec("area"))
                .Literal("/last-news")
                .Query("type", new VarSpec("count"))
                .Build();

            Assert.AreEqual("http://example.org/{area}/last-news{?type,count}", template.ToString());
        }
    }
}