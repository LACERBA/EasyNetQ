﻿// ReSharper disable InconsistentNaming

using System.Threading;
using NUnit.Framework;

namespace EasyNetQ.Tests.Integration
{
    [TestFixture]
    public class ConnectionErrorConditionsTests
    {
        [SetUp]
        public void SetUp() {}

        [Test, Explicit("Tries to make a connection to a RabbitMQ Broker")]
        public void Should_write_a_useful_error_message_when_connetion_fails()
        {
            RabbitHutch.CreateBus("host=localhost_not");
            Thread.Sleep(2000);
        } 

        [Test, Explicit("Tries to make a connection to a RabbitMQ Broker")]
        public void Should_write_a_useful_error_message_when_VHost_does_not_exist()
        {
            RabbitHutch.CreateBus("host=localhost;virtualHost=not one I know");
            Thread.Sleep(2000);
        } 

        [Test, Explicit("Tries to make a connection to a RabbitMQ Broker")]
        public void Should_write_a_useful_error_message_when_credentials_are_incorrect()
        {
            RabbitHutch.CreateBus("host=localhost;password=wrong_password");
            Thread.Sleep(2000);
        }
    }
}

// ReSharper restore InconsistentNaming