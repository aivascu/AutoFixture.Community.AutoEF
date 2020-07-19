using System;
using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.ControlFlow;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Git.GitTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
[DotNetVerbosityMapping]
[GitHubActions("pull_request",
    GitHubActionsImage.WindowsLatest,
    OnPullRequestBranches = new[] { MasterBranch, DevelopBranch },
    InvokedTargets = new[] { nameof(Test) },
    ImportGitHubTokenAs = nameof(GitHubToken),
    ImportSecrets = new[] { nameof(CoverallsToken) })]
[GitHubActions("continuous",
    GitHubActionsImage.WindowsLatest,
    OnPushBranches = new[] { MasterBranch },
    InvokedTargets = new[] { nameof(Test) },
    ImportGitHubTokenAs = nameof(GitHubToken),
    ImportSecrets = new[] { nameof(CoverallsToken) })]
internal partial class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Compile);

    private const string MasterBranch = "master";
    private const string DevelopBranch = "develop";
    private const string ReleaseBranchPrefix = "release";
    private const string HotfixBranchPrefix = "hotfix";

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    private readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("NuGet API Key")] private readonly string NuGetApiKey;

    [Parameter("NuGet Source for Packages")]
    private readonly string NuGetSource = "https://api.nuget.org/v3/index.json";

    [Required] [Solution] private readonly Solution Solution;

    [Required] [GitRepository] private readonly GitRepository GitRepository;

    [Required] [GitVersion] readonly GitVersion GitVersion;

    [Partition(2)] private readonly Partition TestPartition;

    private readonly IEnumerable<string> NonLibProjects = new[] { "_build", "TestClasses" };

    private IEnumerable<Project> TestProjects => TestPartition.GetCurrent(Solution.GetProjects("*.Tests"));

    private IEnumerable<Project> LibProjects => Solution.Projects
        .Where(p => !p.Name.EndsWith(".Tests")
                    && !NonLibProjects.Contains(p.Name, StringComparer.OrdinalIgnoreCase));

    private List<AbsolutePath> PackageFiles => ArtifactsDirectory.GlobFiles("*.nupkg").ToList();

    private AbsolutePath SourceDirectory => RootDirectory / "src";
    private AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    private AbsolutePath TestResultsDirectory => RootDirectory / "testresults";
    private AbsolutePath CoverageDirectory => RootDirectory / "coverage";

    private Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory
                .GlobDirectories("**/bin", "**/obj")
                .ForEach(DeleteDirectory);

            EnsureCleanDirectory(ArtifactsDirectory);
            EnsureCleanDirectory(TestResultsDirectory);
            EnsureCleanDirectory(CoverageDirectory);
        });

    private Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    private Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .EnableNoLogo()
                .SetProjectFile(Solution)
                .SetNoRestore(InvokedTargets.Contains(Restore))
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion));
        });

    private Target Test => _ => _
        .DependsOn(Compile)
        .Produces(TestResultsDirectory / "*.trx")
        .Produces(TestResultsDirectory / "*.xml")
        .Partition(() => TestPartition)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetConfiguration(Configuration)
                .SetNoBuild(InvokedTargets.Contains(Compile))
                .ResetVerbosity()
                .SetResultsDirectory(TestResultsDirectory)
                .When(InvokedTargets.Contains(Cover), _ => _
                    .EnableCollectCoverage()
                    .SetCoverletOutputFormat(CoverletOutputFormat.cobertura)
                    .SetExcludeByFile("**/TestClasses/**")
                    .When(IsServerBuild, _ => _.EnableUseSourceLink()))
                .CombineWith(TestProjects, (_, v) => _
                    .SetProjectFile(v)
                    .SetLogger($"trx;LogFileName={v.Name}.trx")
                    .When(InvokedTargets.Contains(Cover), _ => _
                        .SetCoverletOutput(TestResultsDirectory / $"{v.Name}.xml"))));
        });

    private Target Cover => _ => _
        .DependsOn(Test)
        .Consumes(Test)
        .Produces(CoverageDirectory / "lcov.info")
        .Executes(() =>
        {
            ReportGenerator(_ => _
                .SetFramework("netcoreapp3.0")
                .SetReports(TestResultsDirectory / "*.xml")
                .SetTargetDirectory(CoverageDirectory)
                .SetReportTypes((ReportTypes)"lcov")
                .When(IsLocalBuild, _ => _
                    .AddReportTypes(ReportTypes.HtmlInline)));
        });

    private Target Pack => _ => _
        .DependsOn(Compile)
        .Produces(ArtifactsDirectory / "*.nupkg")
        .Produces(ArtifactsDirectory / "*.snupkg")
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetConfiguration(Configuration)
                .SetNoBuild(InvokedTargets.Contains(Compile))
                .SetOutputDirectory(ArtifactsDirectory)
                .SetVersion(GitVersion.NuGetVersionV2)
                .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
                .EnableIncludeSymbols()
                .CombineWith(LibProjects, (_, p) => _.SetProject(p)));
        });

    private Target Publish => _ => _
        .ProceedAfterFailure()
        .DependsOn(Test, Pack)
        .Consumes(Pack)
        .Requires(() => NuGetApiKey)
        .Requires(() => GitHasCleanWorkingCopy())
        .Requires(() => Configuration.Equals(Configuration.Release))
        .Requires(() => GitRepository.Branch.EqualsOrdinalIgnoreCase(MasterBranch)
                        || GitRepository.Branch.EqualsOrdinalIgnoreCase(DevelopBranch)
                        || GitRepository.Branch.StartsWithOrdinalIgnoreCase(ReleaseBranchPrefix)
                        || GitRepository.Branch.StartsWithOrdinalIgnoreCase(HotfixBranchPrefix))
        .Executes(() =>
        {
            Assert(PackageFiles.Count == 4, "Unexpected number of packages.");

            DotNetNuGetPush(s => s
                    .SetSource(NuGetSource)
                    .SetApiKey(NuGetApiKey)
                    .EnableSkipDuplicate()
                    .CombineWith(PackageFiles, (_, p) => _.SetTargetPath(p)),
                degreeOfParallelism: 5,
                completeOnFailure: true);
        });
}
