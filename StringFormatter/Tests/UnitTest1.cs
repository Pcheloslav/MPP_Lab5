using Core;
using NUnit.Framework;
using System;

namespace Tests
{
    public class Tests
    {
        public User user = new("Petya", "Ivanov", new string[] { "utyug" });

        [Test]
        public void TestCorrectString()
        {
            
            var res = StringFormatter.Format("User {FirstName} {LastName} order {Orders[0]}", user);
            Assert.That($"User {user.FirstName} {user.LastName} order {user.Orders[0]}".Equals(res));
        }

        [Test]
        public void TestIncorrectString()
        {
            Assert.Multiple(() =>
            {
                Assert.Catch<ArgumentException>(() =>
                {
                    var res = StringFormatter.Format("User {FirstName}} {LastName} order {Orders[0]}", user);
                });

                Assert.Catch<ArgumentException>(() =>
                {
                    var res = StringFormatter.Format("User {{FirstName} {LastName} order {Orders[0]}", user);
                });
            });
        }

        [Test]
        public void TestMultipleBrackets()
        {
            var res = StringFormatter.Format("User {{{FirstName}}} {{{LastName}}} order {{{Orders[0]}}}", user);
            Assert.That($"User {{{user.FirstName}}} {{{user.LastName}}} order {{{user.Orders[0]}}}".Equals(res));
        }
    }
}
