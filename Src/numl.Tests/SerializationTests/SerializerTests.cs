﻿using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using numl.Serialization;
using System.Collections.Generic;
using System.Text;

namespace numl.Tests.SerializationTests
{
    [TestFixture]
    public class SerializerTests
    {
        /// <summary>
        /// Wraps string into Reader
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>StreamReader.</returns>
        public static StreamReader FromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            var sr = new StreamReader(stream);
            return sr;
        }

        [Test]
        public void LiteralTest()
        {
            var fl = Serializer.Parse(FromString("false"));
            Assert.AreEqual(false, fl);
            var tr = Serializer.Parse(FromString("true"));
            Assert.AreEqual(true, tr);
            var nl = Serializer.Parse(FromString("null"));
            Assert.AreEqual(null, nl);
        }

        [Test]
        public void NumberTest()
        {
            var a = Serializer.Parse(FromString(System.Math.PI.ToString("r")));
            Assert.AreEqual(System.Math.PI, a);
            var b = Serializer.Parse(FromString((-1 * System.Math.PI).ToString("r")));
            Assert.AreEqual(-1 * System.Math.PI, b);
            var c = Serializer.Parse(FromString((4354).ToString()));
            Assert.AreEqual(4354, c);
            var d = Serializer.Parse(FromString((-4354).ToString()));
            Assert.AreEqual(-4354, d);
            var e = Serializer.Parse(FromString((double.MinValue).ToString("r")));
            Assert.AreEqual(double.MinValue, e);
            var f = Serializer.Parse(FromString((double.MaxValue).ToString("r")));
            Assert.AreEqual(double.MaxValue, f);
        }

        [Test]
        public void StringTest()
        {
            Action<string, string> test = (s, a) =>
            {
                var experiment = Serializer.Parse(FromString($"\"{s}\""));
                Assert.AreEqual(a, experiment);
            };

            test("little string example", "little string example");
            test("with \\\"escape\\\" thingy", "with \"escape\" thingy");
            test("others \\t \\n \\r \\b \\f test", "others \t \n \r \b \f test");
            test("unicode \\u00A3 test", "unicode \u00A3 test");
            test("slashes \\\\ \\/ test", "slashes \\ / test");
        }

        [Test]
        public void ArrayTest()
        {
            Action<string, object[]> test = (s, a) =>
            {
                var experiment = Serializer.Parse(FromString(s));
                Assert.AreEqual(a, experiment);
            };

            test("[1   , 2,   3,   4,    5]", new object[] { 1, 2, 3, 4, 5});
            test($"[{System.Math.PI.ToString("r")},{(-1 * System.Math.PI).ToString("r")}]",
                   new object[] { System.Math.PI, -1 * System.Math.PI });

            var arr = "[\"little string example\",";
            arr += "\"with \\\"escape\\\" thingy\",";
            arr += "\"others \\t \\n \\r \\b \\f test\",";
            arr += "\"unicode \\u00A3 test\",";
            arr += "\"slashes \\\\ \\/ test\"]";

            var truth = new object[]
            {
                "little string example",
                "with \"escape\" thingy",
                "others \t \n \r \b \f test",
                "unicode \u00A3 test",
                "slashes \\ / test"
            };

            test(arr, truth);

            test("[true   , false,   null,   true ]", new object[] { true, false, null, true });
        }

        [Test]
        public void SimpleObjectTest()
        {
            Action<string, Dictionary<string, object>> test = (s, a) =>
            {
                var experiment = Serializer.Parse(FromString(s));
                Assert.AreEqual(a, experiment);
            };


            var s1 = "{\n\t\"prop1\"  :  123213,\n\t\"prop2\" : \"simple string\"\n}";
            var d1 = new Dictionary<string, object>()
            {
                {"prop1", 123213 },
                {"prop2", "simple string" } 
            };

            test(s1, d1);

            var s2 = "{\n\t\"prop1\"  :  123213,\n\t\"prop2\" : \"simple string\",\n\t\"prop3\" : [1 ,  2]\n}";
            var d2 = new Dictionary<string, object>()
            {
                {"prop1", 123213 },
                {"prop2", "simple string" },
                {"prop3", new object[] { 1, 2 } }
            };

            test(s2, d2);
        }

        [Test]
        public void SimpleSerializationTests()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            var s = "Super Interest String!";
            Serializer.Serialize(sw, s);
            Assert.AreEqual($"\"{s}\"", sb.ToString());


            sb.Clear();

            double x = double.MinValue;
            Serializer.Serialize(sw, x);
            Assert.AreEqual(x.ToString("r"), sb.ToString());
        }

        [Test]
        public void SimpleArraySerializationTests()
        {
            var x1 = new[] { 1, 2, 3, 4, 5, 6, 7 };


            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            
            Serializer.Serialize(sw, x1);


            sb.Clear();

            var x2 = new[] { "a", "b", "c", "d", "e", "f", "g" };
            Serializer.Serialize(sw, x2);
        }

        [Test]
        public void SimpleObjectSerializationTests()
        {
            var x1 = new { a = "one", b = double.MaxValue, c = false };


            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);


            Serializer.Serialize(sw, x1);


            sb.Clear();

            var x2 = new { a = "one", b = double.MaxValue, c = false, x = x1 };
            Serializer.Serialize(sw, x2);
        }
    }
}