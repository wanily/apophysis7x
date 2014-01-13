using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Xyrus.Tools.TemplateExpander
{
	class Program
	{
		enum ExpandMode
		{
			Default,
			Tag
		}

		struct Arguments
		{
			public string Name;
			public string TargetDir;
			public string TemplateDir;
			public string Guid;
			public string OutputFile;

			private static Arguments New()
			{
				var result = new Arguments();

				result.Guid = System.Guid.NewGuid().ToString();
				result.TemplateDir = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ?? Environment.CurrentDirectory, "Template");
				result.TargetDir = Environment.CurrentDirectory;

				return result;
			}

			public static Arguments Parse(string[] args)
			{
				string key = null;
				bool done = false;

				var result = New();

				foreach (var arg in args ?? new string[0])
				{
					if (done)
						break;

					switch (arg.ToLower().Trim())
					{
						case "-t":
						case "--template":
							key = "t";
							break;
						case "-o":
						case "--output":
							key = "o";
							break;
						case "-w":
						case "--write-name":
							key = "w";
							break;
						default:
							switch (key)
							{
								case "t":
									result.TemplateDir = arg.Trim();
									key = null;
									break;
								case "o":
									result.TargetDir = arg.Trim();
									key = null;
									break;
								case "w":
									result.OutputFile = arg.Trim();
									key = null;
									break;
								default:
									result.Name = arg.Trim();
									done = true;
									break;
							}
							break;
					}
				}

				return result;
			}
		}

		static void EnsureName(ref Arguments arguments)
		{
			if (string.IsNullOrEmpty(arguments.Name))
			{
				Console.Write(Resources.PromptString);
				arguments.Name = Console.ReadLine() ?? string.Empty;

				if (string.IsNullOrEmpty(arguments.Name))
				{
					Console.Write(Resources.FailedString);
				}
			}

			arguments.Name = Sanitize(arguments.Name);
		}

		static string Sanitize(string str)
		{
			var forbiddenChars = Path.GetInvalidFileNameChars();
			foreach (var @char in forbiddenChars)
				str = str.Replace(@char, '_');

			return str;
		}

		static string StringToRegex(string str)
		{
			var literalChars = new List<char>
			{
				'a', 'A', 'b', 'B', 'c', 'C', 'd', 'D', 'e', 'E', 'f', 'F', 'g', 'G', 'h', 'H', 'i', 'I', 'j',
				'J', 'k', 'K', 'l', 'L', 'm', 'M', 'n', 'N', 'o', 'O', 'p', 'P', 'q', 'Q', 'r', 'R', 's', 'S',
				't', 'T', 'u', 'U', 'v', 'V', 'w', 'W', 'x', 'X', 'y', 'Y', 'z', 'Z',
				'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
			};

			var regex = string.Empty;

			for (int i = 0; i < str.Length; i++)
			{
				if (literalChars.Contains(str[i]))
				{
					regex += str[i];
					continue;
				}

				regex += ("\\u" + ((int) str[i]).ToString("X4"));
			}

			return regex;
		}

		static string ExpandVars(string str, Arguments args, ExpandMode mode)
		{
			string tag;

			switch (mode)
			{
				case ExpandMode.Default:
					tag = "${0}";
					break;
				case ExpandMode.Tag:
					tag = "<%{0}%>";
					break;
				default:
					throw new ArgumentOutOfRangeException("mode");
			}

			string[][] vars =
			{
				new[] {"name", args.Name},
				new[] {"guid", args.Guid}
			};

			foreach (var v in vars)
			{
				var regex = new Regex(StringToRegex(string.Format(tag, v[0])), RegexOptions.IgnoreCase);
				str = regex.Replace(str, v[1]);
			}

			return str;
		}

		static void Main(string[] args)
		{
			var result = Arguments.Parse(args);
			EnsureName(ref result);

			if (!Directory.Exists(result.TemplateDir))
			{
				Console.WriteLine(Resources.ErrorFormatString, string.Format(Resources.TemplateDirError, result.TemplateDir));
				return;
			}

			try
			{
				WriteProject(result);
				Console.WriteLine(Resources.SuccessfulString);

				if (!string.IsNullOrEmpty(result.OutputFile))
				{
					File.WriteAllText(result.OutputFile, result.Name);
				}
			} 
			catch (Exception exception)
			{
				Console.WriteLine(Resources.ErrorFormatString, exception.Message);
				Console.WriteLine(Resources.FailedString);
			}
		}

		static void WriteProject(Arguments arguments)
		{
			var targetDirectory = Path.Combine(arguments.TargetDir, arguments.Name);
			var files = Directory.GetFiles(arguments.TemplateDir);

			if (!Directory.Exists(targetDirectory))
				Directory.CreateDirectory(targetDirectory);

			foreach (var file in files)
			{
				var sourceFileName = Path.GetFileName(file) ?? ("~f" + (new Random().Next(1000,9999)) + ".dat");
				var sourcePath = Path.Combine(arguments.TemplateDir, sourceFileName);

				var targetFileName = Sanitize(ExpandVars(sourceFileName, arguments, ExpandMode.Default));
				var targetPath = Path.Combine(targetDirectory, targetFileName);

				Console.WriteLine(Resources.ReadingString, sourceFileName);

				var content = File.ReadAllText(sourcePath);
				var updatedContent = ExpandVars(content, arguments, ExpandMode.Tag);

				Console.WriteLine(Resources.WritingString, targetFileName);
				File.WriteAllText(targetPath, updatedContent);
			}
		}
	}
}
