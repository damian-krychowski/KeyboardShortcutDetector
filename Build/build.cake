var target = Argument("target", "Default");

Task("Restore-NuGet-Packages")
    .Does(() =>
{
    NuGetRestore("../Source/KeyboardShortcutDetector.sln");
});

Task("Clean")
    .Does(() =>
{
    CleanDirectories("../Source/**/bin");
    CleanDirectories("../Source/**/obj");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    MSBuild("../Source/KeyboardShortcutDetector.sln", settings => settings.SetConfiguration("Release"));
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit("../Source/KeyboardShortcutDetector.Tests/bin/Release/KeyboardShortcutDetector.Tests.dll", new NUnitSettings {
        ToolPath = "../Source/packages/NUnit.ConsoleRunner.3.2.1/tools/nunit3-console.exe"
    });
});

Task("Create-NuGet-Package")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    var nuGetPackSettings = new NuGetPackSettings {
        Id                      = "KeyboardShortcutDetector",
        Version                 = EnvironmentVariable("APPVEYOR_BUILD_VERSION"),
        Title                   = "KeyboardShortcutDetector",
        Authors                 = new[] {"Damian Krychowski"},
        Owners                  = new[] {"Damian Krychowski"},
        Description             = "Keyboard listener detecting shortcuts.",
        ProjectUrl              = new Uri("https://github.com/damian-krychowski/KeyboardShortcutDetector"),
        LicenseUrl              = new Uri("https://github.com/damian-krychowski/KeyboardShortcutDetector/blob/master/LICENSE"),
        Copyright               = "Damian Krychowski 2016",
        RequireLicenseAcceptance= false,
        Symbols                 = false,
        NoPackageAnalysis       = true,
        Files                   = new[] { new NuSpecContent {Source = "bin/Release/KeyboardShortcutDetector.dll", Target = "lib/net452"} },
        BasePath                = "../Source/KeyboardShortcutDetector",
        OutputDirectory         = "."
    };
    
    NuGetPack(nuGetPackSettings);
});

Task("Push-NuGet-Package")
    .IsDependentOn("Create-NuGet-Package")
    .Does(() =>
{
    var package = "./KeyboardShortcutDetector." + EnvironmentVariable("APPVEYOR_BUILD_VERSION") +".nupkg";
                
    NuGetPush(package, new NuGetPushSettings {
        ApiKey = EnvironmentVariable("NUGET_API_KEY")
    });
});


Task("Default")
	.IsDependentOn("Push-NuGet-Package")
    .Does(() =>
{
    Information("KeyboardShortcutDetector building finished.");
});

RunTarget(target);