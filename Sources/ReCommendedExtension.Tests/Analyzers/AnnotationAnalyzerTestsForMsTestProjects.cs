using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.Properties.Flavours;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.FeaturesTestFramework.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;
using ReCommendedExtension.Highlightings;

namespace ReCommendedExtension.Tests.Analyzers
{
    [TestFlavours("3AC096D0-A1C2-E12C-1390-A8335801FDAB")] // todo: does not work
    [TestFixture]
    public sealed class AnnotationAnalyzerTestsForMsTestProjects : CSharpHighlightingTestBase
    {
        protected override string RelativeTestDataPath => @"Analyzers\Annotation";

        protected override bool HighlightingPredicate(IHighlighting highlighting, IPsiSourceFile sourceFile)
            => highlighting is MissingSuppressionJustificationHighlighting;

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        protected override void DoTest(IProject project)
        {
            // patch the project type guids (applying [TestFlavours("3AC096D0-A1C2-E12C-1390-A8335801FDAB")] doesn't work)
            var projectTypeGuids = new HashSet<Guid>(project.ProjectProperties.ProjectTypeGuids ?? Enumerable.Empty<Guid>());
            if (projectTypeGuids.Add(MsTestProjectFlavor.MsTestProjectFlavorGuid))
            {
                var field = project.ProjectProperties.GetType()
                    .BaseType.GetField("myProjectTypeGuids", BindingFlags.Instance | BindingFlags.NonPublic);
                field.SetValue(project.ProjectProperties, projectTypeGuids);
            }

            Assert.True(project.HasFlavour<MsTestProjectFlavor>());

            base.DoTest(project);
        }

        [Test]
        public void TestSuppressMessage_TestProject() => DoNamedTest2();
    }
}