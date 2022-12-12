using Core;
using NUnit.Framework;
using System;

namespace Tests
{
    public class Tests
    {
        public User user = new User("Petya", "Ivanov", new string[] { "utyug" });
        public string res;

        [Test]
        public void TestCorrectString()
        {
            
            res = StringFormatter.Shared.Format("User {FirstName} {LastName} order {Orders[0]}", user);
            Assert.That($"User {user.FirstName} {user.LastName} order {user.Orders[0]}".Equals(res));
        }

        [Test]
        public void TestIncorrectString()
        {
            Assert.Multiple(() =>
            {
                Assert.Catch<ArgumentException>(() =>
                {
                    res = StringFormatter.Shared.Format("User {FirstName}} {LastName} order {Orders[0]}", user);
                });

                Assert.Catch<ArgumentException>(() =>
                {
                    res = StringFormatter.Shared.Format("User {{FirstName} {LastName} order {Orders[0]}", user);
                });

            });
        }

        [Test]
        public void TestMultipleBrackets()
        {
            res = StringFormatter.Shared.Format("User {{{FirstName}}} {{{LastName}}} order {{{Orders[0]}}}", user);
            Assert.That($"User {{{user.FirstName}}} {{{user.LastName}}} order {{{user.Orders[0]}}}".Equals(res));
        }

    }
}