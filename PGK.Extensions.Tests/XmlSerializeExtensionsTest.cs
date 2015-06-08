using System;
using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PGK.Extensions.Tests
{
    [TestClass]
    public class XmlSerializeExtensionsTest
    {
        public class Person
        {
            // parameter less constructor required by XmlSerializer
            public Person()
            {
            }

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Identifier { get; set; }
            public DateTime Birthday { get; set; }

            public static Person GetJalalx()
            {
                return new Person()
                {
                    Identifier = 1,
                    FirstName = "Jalal",
                    LastName = "Amini Robaty",
                    Birthday = new DateTime(1990, 6, 14)
                };
            }

            public override bool Equals(object obj)
            {
                var instance = obj as Person;
                return (instance.FirstName == this.FirstName) &&
                       (instance.LastName == this.LastName) &&
                       (instance.Identifier == this.Identifier) &&
                       (instance.Birthday == this.Birthday);
            }
        }

        [TestMethod]
        public void ArePersonsEqual()
        {
            var jalalx1 = Person.GetJalalx();
            var jalalx2 = Person.GetJalalx();

            Assert.AreEqual(jalalx1, jalalx2);
        }

        [TestMethod]
        public void CanXmlSerializeOrNot()
        {
            var jalalx = Person.GetJalalx();

            Assert.IsTrue(jalalx.CanXmlSerialize<Person>(), "Object Can Xml Serialize!");
        }

        [TestMethod]
        public void XmlSerializePerson()
        {
            var jalalx = Person.GetJalalx();
            var stream = new System.IO.MemoryStream();
            jalalx.XmlSerialize<Person>(stream);
            Assert.IsTrue(stream.Length > 0, "Object Xml Serialized successful!");
        }

        [TestMethod]
        public void XmlDeserializePerson()
        {
            var jalalx = Person.GetJalalx();
            var stream = new System.IO.MemoryStream();
            jalalx.XmlSerialize<Person>(stream);

            var newJalalx = stream.XmlDeserialize<Person>();

            Assert.AreEqual(jalalx, newJalalx);
        }
    }
}
