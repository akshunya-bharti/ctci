🚀 ctci

Minimal, deterministic, multi-language coding test runner for solving problems from “Cracking the Coding Interview”

🎯 Purpose

ctci is a lightweight coding test harness built in .NET.

It enables you to:

✅ Solve CTCI problems in C#, Python, or TypeScript

📄 Run structured test cases from a plain text file

🔁 Execute solutions using stdin / stdout

🧪 Automatically validate outputs

▶️ Continue execution on wrong answers

🛑 Fail fast on crashes or build errors

📊 Print a clear summary at the end

This is not a competitive programming judge.

It is a minimal interview training harness focused on:

Transparency

Determinism

Zero magic

Debuggability

Developer control

🧠 Design Philosophy

This project intentionally avoids:

❌ JSON serialization

❌ Test frameworks

❌ Reflection

❌ Dynamic loading

❌ Timeout handling (v1)

❌ Performance benchmarking

❌ Colored console output

❌ Automatic dependency detection

The goal is strict simplicity and predictability.

Each problem:

📦 Is isolated

🌐 Is language-specific

🔄 Uses raw stdin/stdout

📥 Has explicit test input

🎯 Executes deterministically

## Project Structure

```
ctci
├── CTCI.Runner        # .NET console runner
└── problems
    ├── Ch01_Strings
    │   ├── 01_IsUnique
    │   │   ├── csharp
    │   │   ├── python
    │   │   ├── typescript
    │   │   └── tests.txt
    │   └── 02_CheckPermutation
    ├── Ch02_LinkedLists
    └── Ch03_StacksAndQueues
```

## Problem Folder Structure

Each problem must follow this structure:
```
01_IsUnique
├── csharp
│   ├── Solution.csproj
│   └── Program.cs
├── python
│   └── solution.py
├── typescript
│   └── solution.ts
└── tests.txt
```
Each language implementation is completely isolated.

📝 Test File Format

The test file uses a length-prefixed format.

Structure

<number_of_tests>

For each test:
<number_of_input_lines>
<input_line_1>
<input_line_2>
...
<expected_output>

Example

2
1
5
25
2
1 2
3 4
10

This means:

Test 1 → Input: 5 → Expected: 25

Test 2 → Input: 1 2, 3 4 → Expected: 10

Why Length-Prefixed?

No separators

No ambiguity

Supports multi-line inputs

Works for matrices, graphs, arrays

▶️ CLI Usage

Run from inside CTCI.Runner:

dotnet run -- <language> <chapter.problem>

Examples

dotnet run -- csharp 1.1
dotnet run -- python 2.3
dotnet run -- typescript 3.2

🔤 Supported Languages

csharp

python

typescript

🔢 Chapter / Problem Format

Must follow:

<number>.<number>

Examples:

1.1
2.3
3.2

Both numbers must be positive integers.

⚙️ Execution Rules

🐍 Python

Executed as:

python solution.py

Python must be installed and available in PATH.

🟦 TypeScript

Executed as:

ts-node solution.ts

The runner forces CommonJS compilation to avoid ESM issues.

Requirements:

Node installed

ts-node installed globally

Install if needed:

npm install -g ts-node typescript

🟣 C#

Execution steps:

dotnet build

If build fails → print full error and stop

Locate compiled DLL in bin/Debug/netX.X

Execute using:

dotnet <compiled_dll>

The .exe file is never executed directly.

🧪 Execution Behavior

✅ On Correct Output

Test X: PASS

❌ On Wrong Output

Test X: FAIL
Expected: '...'
Actual:   '...'

Execution continues.

🛑 On Crash / Non-Zero Exit / Build Failure

Full stderr is printed

Execution stops immediately

📊 Final Summary

Passed X/Y tests

➕ Adding a New Problem

Step 1 — Create Chapter Folder (if needed)

Example:

problems/Ch04_TreesAndGraphs

Naming rule:

Ch{chapterNumber padded to 2 digits}_{Name}

Step 2 — Create Problem Folder

Example:

01_ValidateBST

Naming rule:

{problemNumber padded to 2 digits}_{Name}

Step 3 — Create Language Folders

csharp
python
typescript

Step 4 — Add tests.txt

Follow the strict length-prefixed format.

Step 5 — Implement Solution

Each solution must:

Read from standard input

Write to standard output

Print only the final answer

Avoid debug logs

🧩 Internal Components

The runner consists of:

CLI parser

Chapter & problem resolver

Test file parser

C# build manager

Process executor

Test runner

Output formatter

All implemented in a single Program.cs for clarity and minimalism.

🚫 Out of Scope (Version 1)

Timeout handling

Performance measurement

Parallel execution

Running entire chapter

Problem scaffolding generator

JSON-based test format

Automatic dependency installation

Colored console output

📌 Prerequisites

You must have:

.NET SDK

Python

Node

ts-node (installed globally)

Verify:

dotnet --version
python --version
node --version
ts-node --version

🏁 Summary

ctci is a minimal, deterministic, cross-language interview harness.

It prioritizes:

Simplicity

Control

Transparency

Extensibility

Developer discipline

Designed for serious interview preparation — not convenience.

