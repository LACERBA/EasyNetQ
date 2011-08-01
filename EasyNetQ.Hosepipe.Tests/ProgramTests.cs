﻿// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace EasyNetQ.Hosepipe.Tests
{
    [TestFixture]
    public class ProgramTests
    {
        private Program program;
        private MockMessageWriter messageWriter;
        private MockQueueRetrieval queueRetrieval;
        private MockMessageReader messageReader;
        private MockQueueInsertion queueInsertion;

        [SetUp]
        public void SetUp()
        {
            messageWriter = new MockMessageWriter();
            queueRetrieval = new MockQueueRetrieval();
            messageReader = new MockMessageReader();
            queueInsertion = new MockQueueInsertion();

            program = new Program(
                new ArgParser(), 
                queueRetrieval, 
                messageWriter,
                messageReader,
                queueInsertion);
        }

        private const string expectedDumpOutput =
@"2 Messages from queue 'EasyNetQ_Default_Error_Queue'
output to directory 'C:\Source\Mike.AmqpSpike\EasyNetQ.Hosepipe.Tests\bin\Debug'
";

        [Test]
        public void Should_output_messages_to_directory_with_dump()
        {
            var args = new[]
            {
                "dump",
                "s:localhost",
                "q:EasyNetQ_Default_Error_Queue"
            };

            var writer = new StringWriter();
            Console.SetOut(writer);

            program.Start(args);

            writer.GetStringBuilder().ToString().ShouldEqual(expectedDumpOutput);

            messageWriter.Parameters.QueueName.ShouldEqual("EasyNetQ_Default_Error_Queue");
            messageWriter.Parameters.HostName.ShouldEqual("localhost");
        }

        private const string expectedInsertOutput =
@"2 Messages from directory 'C:\Source\Mike.AmqpSpike\EasyNetQ.Hosepipe.Tests\bin\Debug'
inserted into queue 'test'
";

        [Test]
        public void Should_insert_messages_with_insert()
        {
            var args = new[]
            {
                "insert",
                "s:localhost",
                "q:test"
            };

            var writer = new StringWriter();
            Console.SetOut(writer);

            program.Start(args);

            writer.GetStringBuilder().ToString().ShouldEqual(expectedInsertOutput);

            messageReader.Parameters.QueueName.ShouldEqual("test");
            messageReader.Parameters.HostName.ShouldEqual("localhost");
        }
    }

    public class MockMessageWriter : IMessageWriter
    {
        public QueueParameters Parameters { get; set; }

        public void Write(IEnumerable<string> messages, QueueParameters queueParameters)
        {
            Parameters = queueParameters;
            foreach (var message in messages)
            {
                // Console.Out.WriteLine("message = {0}", message);
            }
        }
    }

    public class MockQueueRetrieval : IQueueRetreival
    {
        public IEnumerable<string> GetMessagesFromQueue(QueueParameters parameters)
        {
            yield return "some message";
            yield return "some message";
        }
    }

    public class MockMessageReader : IMessageReader
    {
        public QueueParameters Parameters { get; set; }

        public IEnumerable<string> ReadMessages(QueueParameters parameters)
        {
            Parameters = parameters;
            yield return "some message";
            yield return "some message";
        }
    }

    public class MockQueueInsertion : IQueueInsertion
    {
        public void PublishMessagesToQueue(IEnumerable<string> messages, QueueParameters parameters)
        {
            foreach (var message in messages)
            {
                // Console.Out.WriteLine("message = {0}", message);
            }
        }
    }
}

// ReSharper restore InconsistentNaming