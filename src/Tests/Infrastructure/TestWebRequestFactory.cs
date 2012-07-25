﻿using System.Net;
using NUnit.Framework;
using Rhino.Mocks;
using AspUnitRunner.Infrastructure;

namespace AspUnitRunner.Tests.Infrastructure {
    [TestFixture]
    public class TestWebRequestFactory {
        private FakeRequestCreator _requestCreator;

        [SetUp]
        public void SetUp() {
            _requestCreator = FakeRequestCreator.Instance;
            WebRequest.RegisterPrefix("fake:", _requestCreator);
        }

        [TearDown]
        public void TearDown() {
            _requestCreator.Request = null;
        }

        [Test]
        public void Create_should_create_expected_web_request() {
            var expectedRequest = MockRepository.GenerateStub<WebRequest>();
            _requestCreator.Request = expectedRequest;

            var factory = new WebRequestFactory();
            var request = factory.Create("fake://host/path");
            Assert.That(request, Is.EqualTo(expectedRequest));
        }
    }
}