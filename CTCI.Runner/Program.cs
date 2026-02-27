using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var parsed = ParseArguments(args);
        if (parsed == null) return;

        var (language, chapterNumber, problemNumber) = parsed.Value;

        var problemFolder = ResolveProblemFolder(chapterNumber, problemNumber);
        if (problemFolder == null) return;

        Console.WriteLine($"Resolved problem folder: {problemFolder}");

        var tests = LoadTests(problemFolder);
        if (tests == null) return;

        Console.WriteLine($"Loaded {tests.Count} tests.");

        var languageFolder = Path.Combine(problemFolder, language);

        if (!Directory.Exists(languageFolder))
        {
            Console.WriteLine($"Language folder '{language}' not found.");
            return;
        }

        string fileName = "";
        string arguments = "";

        if (language == "csharp")
        {
            var dllPath = BuildCSharp(languageFolder);
            if (dllPath == null) return;

            fileName = "dotnet";
            arguments = $"\"{dllPath}\"";
        }
        else if (language == "python")
        {
            var solution = Path.Combine(languageFolder, "solution.py");
            if (!File.Exists(solution))
            {
                Console.WriteLine("solution.py not found.");
                return;
            }

            fileName = "python";
            arguments = "solution.py";
        }
        else if (language == "typescript")
        {
            var solution = Path.Combine(languageFolder, "solution.ts");
            if (!File.Exists(solution))
            {
                Console.WriteLine("solution.ts not found.");
                return;
            }

            if (OperatingSystem.IsWindows())
            {
                fileName = "cmd.exe";
                arguments =
                    "/c ts-node --compiler-options \"{\\\"module\\\":\\\"CommonJS\\\"}\" solution.ts";
            }
            else
            {
                fileName = "ts-node";
                arguments =
                    "--compiler-options '{\"module\":\"CommonJS\"}' solution.ts";
            }
        }
        else
        {
            Console.WriteLine("Unsupported language.");
            return;
        }

        RunAllTests(tests, fileName, arguments, languageFolder);
    }

    static (string Language, int Chapter, int Problem)? ParseArguments(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: ctci <language> <chapter.problem>");
            return null;
        }

        var language = args[0].ToLowerInvariant();

        if (language != "csharp" && language != "python" && language != "typescript")
        {
            Console.WriteLine("Language must be one of: csharp, python, typescript");
            return null;
        }

        var parts = args[1].Split('.');

        if (parts.Length != 2 ||
            !int.TryParse(parts[0], out var chapter) ||
            !int.TryParse(parts[1], out var problem) ||
            chapter <= 0 || problem <= 0)
        {
            Console.WriteLine("Problem must be in format <chapter.problem>");
            return null;
        }

        return (language, chapter, problem);
    }

    static string? ResolveProblemFolder(int chapterNumber, int problemNumber)
    {
        var runnerDir = Directory.GetCurrentDirectory();
        var rootDir = Directory.GetParent(runnerDir)?.FullName;

        if (rootDir == null)
        {
            Console.WriteLine("Cannot determine root directory.");
            return null;
        }

        var problemsDir = Path.Combine(rootDir, "problems");

        if (!Directory.Exists(problemsDir))
        {
            Console.WriteLine("Problems directory not found.");
            return null;
        }

        var chapterPrefix = $"Ch{chapterNumber:D2}_";

        var chapterFolder = Directory.GetDirectories(problemsDir)
            .FirstOrDefault(d => Path.GetFileName(d).StartsWith(chapterPrefix));

        if (chapterFolder == null)
        {
            Console.WriteLine("Chapter folder not found.");
            return null;
        }

        var problemPrefix = $"{problemNumber:D2}_";

        var problemFolder = Directory.GetDirectories(chapterFolder)
            .FirstOrDefault(d => Path.GetFileName(d).StartsWith(problemPrefix));

        if (problemFolder == null)
        {
            Console.WriteLine("Problem folder not found.");
            return null;
        }

        return problemFolder;
    }

    static List<TestCase>? LoadTests(string problemFolder)
    {
        var testFile = Path.Combine(problemFolder, "tests.txt");

        if (!File.Exists(testFile))
        {
            Console.WriteLine("tests.txt not found.");
            return null;
        }

        var lines = File.ReadAllLines(testFile);
        var index = 0;

        if (lines.Length == 0 || !int.TryParse(lines[index++], out var testCount))
        {
            Console.WriteLine("Invalid test count.");
            return null;
        }

        var tests = new List<TestCase>();

        for (int t = 0; t < testCount; t++)
        {
            if (index >= lines.Length ||
                !int.TryParse(lines[index++], out var inputCount))
            {
                Console.WriteLine("Invalid input line count.");
                return null;
            }

            var inputs = new List<string>();

            for (int i = 0; i < inputCount; i++)
            {
                if (index >= lines.Length)
                {
                    Console.WriteLine("Unexpected end of file while reading inputs.");
                    return null;
                }

                inputs.Add(lines[index++]);
            }

            if (index >= lines.Length)
            {
                Console.WriteLine("Unexpected end of file while reading expected output.");
                return null;
            }

            var expected = lines[index++];

            tests.Add(new TestCase(inputs, expected));
        }

        return tests;
    }

    static string? BuildCSharp(string languageFolder)
    {
        var csproj = Directory.GetFiles(languageFolder, "*.csproj")
                               .FirstOrDefault();

        if (csproj == null)
        {
            Console.WriteLine("No .csproj found.");
            return null;
        }

        Console.WriteLine("Building C# project...");

        var (exitCode, _, stderr) =
            RunProcess("dotnet", "build", languageFolder, new List<string>());

        if (exitCode != 0)
        {
            Console.WriteLine("Build failed:");
            Console.WriteLine(stderr);
            return null;
        }

        Console.WriteLine("Build succeeded.");

        var binFolder = Path.Combine(languageFolder, "bin", "Debug");

        if (!Directory.Exists(binFolder))
        {
            Console.WriteLine("Build output folder not found.");
            return null;
        }

        var dll = Directory.GetFiles(binFolder, "*.dll", SearchOption.AllDirectories)
            .FirstOrDefault(f => !f.EndsWith(".deps.dll"));

        if (dll == null)
        {
            Console.WriteLine("Compiled DLL not found.");
            return null;
        }

        return dll;
    }

    static void RunAllTests(
        List<TestCase> tests,
        string fileName,
        string arguments,
        string workingDirectory)
    {
        int passed = 0;

        for (int i = 0; i < tests.Count; i++)
        {
            var test = tests[i];

            var (exitCode, stdout, stderr) =
                RunProcess(fileName, arguments, workingDirectory, test.InputLines);

            if (exitCode != 0)
            {
                Console.WriteLine($"Test {i + 1}: CRASH");
                Console.WriteLine(stderr);
                return;
            }

            var expected = test.Expected.Trim();
            var actual = stdout.Trim();

            if (expected == actual)
            {
                Console.WriteLine($"Test {i + 1}: PASS");
                passed++;
            }
            else
            {
                Console.WriteLine($"Test {i + 1}: FAIL");
                Console.WriteLine($"Expected: '{expected}'");
                Console.WriteLine($"Actual:   '{stdout}'");
            }
        }

        Console.WriteLine($"Passed {passed}/{tests.Count} tests");
    }

    static (int ExitCode, string StdOut, string StdErr) RunProcess(
        string fileName,
        string arguments,
        string workingDirectory,
        List<string> inputLines)
    {
        var psi = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            WorkingDirectory = workingDirectory,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        using var process = new Process { StartInfo = psi };

        process.Start();

        foreach (var line in inputLines)
        {
            process.StandardInput.WriteLine(line);
        }

        process.StandardInput.Close();

        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();

        process.WaitForExit();

        return (
            process.ExitCode,
            outputTask.Result,
            errorTask.Result
        );
    }
}

class TestCase
{
    public List<string> InputLines { get; }
    public string Expected { get; }

    public TestCase(List<string> inputLines, string expected)
    {
        InputLines = inputLines;
        Expected = expected;
    }
}