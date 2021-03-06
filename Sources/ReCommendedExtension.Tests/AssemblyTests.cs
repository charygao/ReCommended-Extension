using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using JetBrains.Application.UI.Extensions;
using NUnit.Framework;

namespace ReCommendedExtension.Tests
{
    [TestFixture]
    public sealed class AssemblyTests
    {
        [Test]
        [SuppressMessage("ReSharper", "StringIndexOfIsCultureSpecific.1")]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public void TestCopyrightYear()
        {
            var assembly = typeof(IReCommendedExtensionZone).Assembly;
            var attribute = assembly.GetAttribute<AssemblyCopyrightAttribute>();

            Assert.IsTrue(attribute.Copyright.IndexOf(DateTime.Today.Year.ToString()) > -1);
        }

        [Test]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void TestVersions()
        {
            var assembly = typeof(IReCommendedExtensionZone).Assembly;

            var assemblyVersion = assembly.GetName().Version;
            Assert.AreEqual(0, assemblyVersion.Revision);

            var fileVersionAttribute = assembly.GetAttribute<AssemblyFileVersionAttribute>();

            Assert.AreEqual(
                new Version(assemblyVersion.Major, assemblyVersion.Minor, assemblyVersion.Build),
                new Version(fileVersionAttribute.Version));
        }
    }
}