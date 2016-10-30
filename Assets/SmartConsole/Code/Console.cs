// Copyright (c) 2014 Cranium Software

// SmartConsole
//
// A Quake style debug console where you can add variables with the
// CreateVariable functions and commands with the RegisterCommand functions
// the variables wrap their underlying types, the commands are delegates
// taking a string with the line of console input and returning void
// TODO:
// * sort out spammy history and 'return' key handling on mobile platforms
// * improve cvar interface
// * allow history to scroll
// * improve autocomplete
// * allow executing console script from file
// SE: broadly patterned after the debug console implementation from GLToy...
// https://code.google.com/p/gltoy/source/browse/trunk/GLToy/Independent/Core/Console/GLToy_Console.h

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SmartConsole.Code
{
    /// <summary>
    ///     A Quake style debug console - should be added to an otherwise empty game object and have a font set in the
    ///     inspector
    /// </summary>
    public class Console : MonoBehaviour
    {
        public delegate void ConsoleCommandFunction(string parameters);

        // control the general layout here
        private const int HistoryLines = 34;
        private static readonly Color TextColor = new Color(1f, 0.8f, 0.05f);
        private static int currentCommandHistoryIndex;
        private static Font font;

        private static ConsoleVariable<bool> sLogging;

        private static GameObject textInput;
        private static GameObject[] sHistoryDisplay;

        private static readonly AutoCompleteDictionary<Command> CommandDictionary = new AutoCompleteDictionary<Command>();
        private static readonly AutoCompleteDictionary<Command> VariableDictionary = new AutoCompleteDictionary<Command>();
        private static readonly AutoCompleteDictionary<Command> MasterDictionary = new AutoCompleteDictionary<Command>();

        private static readonly List<string> CommandHistory = new List<string>();
        private static readonly List<string> OutputHistory = new List<string>();

        private static string sLastExceptionCallStack = "(none yet)";
        private static string sLastErrorCallStack = "(none yet)";
        private static string sLastWarningCallStack = "(none yet)";

        private static string currentInputLine = "";

        [UsedImplicitly] public Font Font;

        [UsedImplicitly]
        private void Awake()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            if (Font == null)
            {
                Debug.LogError("SmartConsole requires a font to be set in the inspector");
            }

            Initialise(this);
        }

        [UsedImplicitly]
        private void Update()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            HandleInput();

            if ((textInput != null)
                && (textInput.GetComponent<Text>() != null))
            {
                textInput.GetComponent<Text>().text = ">" + currentInputLine;
            }
        }

        /// <summary>
        ///     Clears out the console log
        /// </summary>
        /// <example>
        ///     <code>
        /// SmartConsole.Clear();
        /// </code>
        /// </example>
        private static void Clear()
        {
            OutputHistory.Clear();
            SetStringsOnHistoryElements();
        }

        /// <summary>
        ///     Register a console command with an example of usage and a help description
        ///     e.g. SmartConsole.RegisterCommand( "echo", "echo
        ///     <string>", "writes <string> to the console log", SmartConsole.Echo );
        /// </summary>
        public static void RegisterCommand(string name, string exampleUsage, string helpDescription,
            ConsoleCommandFunction callback)
        {
            var commandName = name.ToLower();

            var command = new Command
            {
                Name = commandName,
                ParamsExample = exampleUsage,
                Help = helpDescription,
                Callback = callback
            };

            CommandDictionary.Add(commandName, command);
            MasterDictionary.Add(commandName, command);
        }

        /// <summary>
        ///     Register a console command with a help description
        ///     e.g. SmartConsole.RegisterCommand( "help", "displays help information for console command where available",
        ///     SmartConsole.Help );
        /// </summary>
        public static void RegisterCommand(string name, string helpDescription, ConsoleCommandFunction callback)
        {
            RegisterCommand(name, "", helpDescription, callback);
        }

        /// <summary>
        ///     Register a console command
        ///     e.g. SmartConsole.RegisterCommand( "foo", Foo );
        /// </summary>
        public static void RegisterCommand(string name, ConsoleCommandFunction callback)
        {
            RegisterCommand(name, "", "(no description)", callback);
        }

        /// <summary>
        ///     Create a console variable
        ///     e.g. SmartConsole.Variable
        ///     < bool>
        ///         showFPS = SmartConsole.CreateVariable
        ///         < bool>( "show.fps", "whether to draw framerate counter or not", false );
        /// </summary>
        public static ConsoleVariable<T> CreateVariable<T>(string name, string description, T initialValue) where T : new()
        {
            if (VariableDictionary.ContainsKey(name))
            {
                Debug.LogError("Tried to add already existing console variable!");
                return null;
            }

            var returnValue = new ConsoleVariable<T>(name, description, initialValue);
            VariableDictionary.Add(name, returnValue);
            MasterDictionary.Add(name, returnValue);

            return returnValue;
        }

        /// <summary>
        ///     Create a console variable without specifying a default value
        ///     e.g. SmartConsole.Variable
        ///     < float> gameSpeed = SmartConsole.CreateVariable< float>( "game.speed", "the current speed of the game" );
        /// </summary>
        public static ConsoleVariable<T> CreateVariable<T>(string name, string description) where T : new()
        {
            return CreateVariable(name, description, new T());
        }

        /// <summary>
        ///     Create a console variable without specifying a description or default value
        ///     e.g. SmartConsole.Variable< string> someString = Console.CreateVariable< string>( "some.string" );
        /// </summary>
        public static ConsoleVariable<T> CreateVariable<T>(string name) where T : new()
        {
            return CreateVariable<T>(name, "");
        }

        /// <summary>
        ///     Destroy a console variable (so its name can be reused)
        /// </summary>
        public static void DestroyVariable<T>(ConsoleVariable<T> consoleVariable) where T : new()
        {
            VariableDictionary.Remove(consoleVariable.Name);
            MasterDictionary.Remove(consoleVariable.Name);
        }

        /// <summary>
        ///     Write a message to the debug console (only - not the log)
        /// </summary>
        /// <param name="message">
        ///     The message to display
        /// </param>
        /// <example>
        ///     <code>
        /// SmartConsole.Print( "Hello world!" );
        /// </code>
        /// </example>
        public static void Print(string message)
        {
            WriteLine(message);
        }

        /// <summary>
        ///     Write a message to the debug console (only - not the log)
        /// </summary>
        /// <param name="message">
        ///     The message to display
        /// </param>
        /// <example>
        ///     <code>
        /// SmartConsole.WriteLine( "Hello world!" );
        /// </code>
        /// </example>
        public static void WriteLine(string message)
        {
            OutputHistory.Add(DeNewLine(message));
            currentCommandHistoryIndex = OutputHistory.Count - 1;
            SetStringsOnHistoryElements();
        }

        /// <summary>
        ///     Execute a string as if it were a single line of input to the console
        /// </summary>
        public static void ExecuteLine(string inputLine)
        {
            WriteLine(">" + inputLine);
            var words = SplitParameters(inputLine);
            if (words.Length > 0)
            {
                if (MasterDictionary.ContainsKey(words[0]))
                {
                    CommandHistory.Add(inputLine);
                    MasterDictionary[words[0]].Callback(inputLine);
                }
                else
                {
                    WriteLine("Unrecognised command or variable name: " + words[0]);
                }
            }
        }

        // --- commands

        private static void Help(string parameters)
        {
            // try and lay it out nicely...
            const int nameLength = 25;
            const int exampleLength = 35;
            foreach (var command in CommandDictionary.Values)
            {
                var outputString = command.Name;
                for (var i = command.Name.Length; i < nameLength; ++i)
                {
                    outputString += " ";
                }

                if (command.ParamsExample.Length > 0)
                {
                    outputString += " example: " + command.ParamsExample;
                }
                else
                {
                    outputString += "          ";
                }

                for (var i = command.ParamsExample.Length; i < exampleLength; ++i)
                {
                    outputString += " ";
                }

                WriteLine(outputString + command.Help);
            }
        }

        private static void Echo(string parameters)
        {
            var outputMessage = "";
            var split = SplitParameters(parameters);
            for (var i = 1; i < split.Length; ++i)
            {
                outputMessage += split[i] + " ";
            }

            if (outputMessage.EndsWith(" "))
            {
                outputMessage.Substring(0, outputMessage.Length - 1);
            }

            WriteLine(outputMessage);
        }

        private static void Clear(string parameters)
        {
            Clear();
        }

        private static void LastExceptionCallStack(string parameters)
        {
            DumpCallStack(sLastExceptionCallStack);
        }

        private static void LastErrorCallStack(string parameters)
        {
            DumpCallStack(sLastErrorCallStack);
        }

        private static void LastWarningCallStack(string parameters)
        {
            DumpCallStack(sLastWarningCallStack);
        }

        private static void Quit(string parameters)
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
        }


        private static void ListCvars(string parameters)
        {
            // try and lay it out nicely...
            const int nameLength = 50;
            foreach (var variable in VariableDictionary.Values)
            {
                var outputString = variable.Name;
                for (var i = variable.Name.Length; i < nameLength; ++i)
                {
                    outputString += " ";
                }

                WriteLine(outputString + variable.Help);
            }
        }

        private static void Initialise(Console instance)
        {
            // run this only once...
            if (textInput != null)
            {
                return;
            }

            Application.logMessageReceived += LogHandler;

            InitialiseCommands();
            InitialiseVariables();
            InitialiseUi(instance);
        }

        private static void HandleInput()
        {
            if (CommandHistory.Count > 0)
            {
                var update = false;
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    update = true;
                    --currentCommandHistoryIndex;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    update = true;
                    ++currentCommandHistoryIndex;
                }

                if (update)
                {
                    currentCommandHistoryIndex = Mathf.Clamp(currentCommandHistoryIndex, 0,
                        CommandHistory.Count - 1);
                    currentInputLine = CommandHistory[currentCommandHistoryIndex];
                }
            }
            HandleTextInput();
        }

        private static void InitialiseCommands()
        {
            RegisterCommand("clear", "clear the console log", Clear);
            RegisterCommand("cls", "clear the console log (alias for Clear)", Clear);
            RegisterCommand("echo", "echo <string>", "writes <string> to the console log (alias for echo)", Echo);
            RegisterCommand("help", "displays help information for console command where available", Help);
            RegisterCommand("list", "lists all currently registered console variables", ListCvars);
            RegisterCommand("print", "print <string>", "writes <string> to the console log", Echo);
            RegisterCommand("quit", "quit the game (not sure this works with iOS/Android)", Quit);
            RegisterCommand("callstack.warning", "display the call stack for the last warning message", LastWarningCallStack);
            RegisterCommand("callstack.error", "display the call stack for the last error message", LastErrorCallStack);
            RegisterCommand("callstack.exception", "display the call stack for the last exception message", LastExceptionCallStack);
        }

        private static void InitialiseVariables()
        {
            sLogging = CreateVariable("console.log", "whether to redirect log to the console", true);
        }

        private static void InitialiseUi(Console instance)
        {
            font = instance.Font;
            if (font == null)
            {
                Debug.LogError("SmartConsole needs to have a font set on an instance in the editor!");
                font = new Font("Arial");
            }

            sHistoryDisplay = new GameObject[HistoryLines];
            for (var i = HistoryLines - 1; i >= 0; i--)
            {
                sHistoryDisplay[i] = instance.AddTextField("SmartConsoleHistoryDisplay" + i);
            }
            textInput = instance.AddTextField("SmartConsoleInputField");
        }

        private GameObject AddTextField(string guiName)
        {
            var returnObject = new GameObject();
            var text = returnObject.AddComponent<Text>();
            text.fontStyle = FontStyle.Normal;
            text.font = font;
            text.color = TextColor;
            text.fontSize = 18;
            var layout = returnObject.AddComponent<LayoutElement>();
            layout.minWidth = 1000;
            layout.minHeight = 15;
            returnObject.transform.SetParent(transform);
            returnObject.name = guiName;
            return returnObject;
        }

        private static void HandleTextInput()
        {
            var autoCompleteHandled = false;
            foreach (var c in Input.inputString)
            {
                switch (c)
                {
                    case '\b':
                        currentInputLine = currentInputLine.Length > 0
                            ? currentInputLine.Substring(0, currentInputLine.Length - 1)
                            : "";
                        break;
                    case '\n':
                    case '\r':
                        ExecuteCurrentLine();
                        currentInputLine = "";
                        break;
                    case '\t':
                        AutoComplete();
                        autoCompleteHandled = true;
                        break; // SE - unity doesn't seem to give this here so we check a keydown as well...
                    default:
                        currentInputLine = currentInputLine + c;
                        break;
                }
            }

            if (!autoCompleteHandled && Input.GetKeyDown(KeyCode.Tab))
            {
                AutoComplete();
            }
        }

        private static void ExecuteCurrentLine()
        {
            ExecuteLine(currentInputLine);
        }

        private static void AutoComplete()
        {
            var lookup = SplitParameters(currentInputLine);
            if (lookup.Length == 0)
            {
                // don't auto complete if we have typed any parameters so far or nothing at all...
                return;
            }

            var nearestMatch = MasterDictionary.AutoCompleteLookup(lookup[0]);

            // only complete to the next dot if there is one present in the completion string which
            // we don't already have in the lookup string
            var dotIndex = 0;
            do
            {
                dotIndex = nearestMatch.Name.IndexOf(".", dotIndex + 1);
            } while ((dotIndex > 0) && (dotIndex < lookup[0].Length));

            var insertion = nearestMatch.Name;
            if (dotIndex >= 0)
            {
                insertion = nearestMatch.Name.Substring(0, dotIndex + 1);
            }

            if (insertion.Length < currentInputLine.Length)
            {
                do
                {
                    if (AutoCompleteTailString("true")) break;
                    if (AutoCompleteTailString("false")) break;
                    if (AutoCompleteTailString("True")) break;
                    if (AutoCompleteTailString("False")) break;
                    if (AutoCompleteTailString("TRUE")) break;
                    if (AutoCompleteTailString("FALSE")) break;
                } while (false);
            }
            else if (insertion.Length >= currentInputLine.Length) // SE - is this really correct?
            {
                currentInputLine = insertion;
            }
        }

        private static bool AutoCompleteTailString(string tailString)
        {
            for (var i = 1; i < tailString.Length; ++i)
            {
                if (currentInputLine.EndsWith(" " + tailString.Substring(0, i)))
                {
                    currentInputLine = currentInputLine.Substring(0, currentInputLine.Length - 1) +
                                       tailString.Substring(i - 1);
                    return true;
                }
            }

            return false;
        }

        private static void SetStringsOnHistoryElements()
        {
            for (var i = 0; i < HistoryLines; ++i)
            {
                var historyIndex = OutputHistory.Count - 1 - i;
                sHistoryDisplay[i].GetComponent<Text>().text = historyIndex >= 0
                    ? OutputHistory[OutputHistory.Count - 1 - i]
                    : "";
            }
        }

        private static bool IsInputCoordInBounds(Vector2 inputCoordinate)
        {
            return (inputCoordinate.x < 0.05f*Screen.width) && (inputCoordinate.y > 0.95f*Screen.height);
        }

        private static void LogHandler(string message, string stack, LogType type)
        {
            if (!sLogging)
            {
                return;
            }

            const string assertPrefix = "[Assert]:             ";
            const string errorPrefix = "[Debug.LogError]:     ";
            const string exceptPrefix = "[Debug.LogException]: ";
            const string warningPrefix = "[Debug.LogWarning]:   ";
            const string otherPrefix = "[Debug.Log]:          ";

            var prefix = otherPrefix;
            switch (type)
            {
                case LogType.Assert:
                {
                    prefix = assertPrefix;
                    break;
                }

                case LogType.Warning:
                {
                    prefix = warningPrefix;
                    sLastWarningCallStack = stack;
                    break;
                }

                case LogType.Error:
                {
                    prefix = errorPrefix;
                    sLastErrorCallStack = stack;
                    break;
                }

                case LogType.Exception:
                {
                    prefix = exceptPrefix;
                    sLastExceptionCallStack = stack;
                    break;
                }
                default:
                {
                    break;
                }
            }

            WriteLine(prefix + message);

            switch (type)
            {
                case LogType.Assert:
                case LogType.Error:
                case LogType.Exception:
                {
                    //WriteLine ( "Call stack:\n" + stack );
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        public static string[] SplitParameters(string parameters)
        {
            return parameters.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] SplitParameters(string parameters, int requiredParameters)
        {
            var split = SplitParameters(parameters);
            if (split.Length < requiredParameters + 1)
            {
                WriteLine("Error: not enough parameters for command. Expected " + requiredParameters + " found " +
                          (split.Length - 1));
            }

            if (split.Length > requiredParameters + 1)
            {
                var extras = split.Length - 1 - requiredParameters;
                WriteLine("Warning: " + extras + "additional parameters will be dropped:");
                for (var i = split.Length - extras; i < split.Length; ++i)
                {
                    WriteLine("\"" + split[i] + "\"");
                }
            }

            return split;
        }

        public static string[] CVarParameterSplit(string parameters)
        {
            var split = SplitParameters(parameters);
            if (split.Length == 0)
            {
                WriteLine("Error: not enough parameters to set or display the value of a console variable.");
            }

            if (split.Length > 2)
            {
                var extras = split.Length - 3;
                WriteLine("Warning: " + extras + "additional parameters will be dropped:");
                for (var i = split.Length - extras; i < split.Length; ++i)
                {
                    WriteLine("\"" + split[i] + "\"");
                }
            }

            return split;
        }

        private static string DeNewLine(string message)
        {
            return message.Replace("\n", " | ");
        }

        private static void DumpCallStack(string stackString)
        {
            var lines = stackString.Split('\r', '\n');

            if (lines.Length == 0)
            {
                return;
            }

            var ignoreCount = 0;
            while ((lines[lines.Length - 1 - ignoreCount].Length == 0) && (ignoreCount < lines.Length))
            {
                ++ignoreCount;
            }
            var lineCount = lines.Length - ignoreCount;
            for (var i = 0; i < lineCount; ++i)
            {
                // SE - if the call stack is 100 deep without recursion you have much bigger problems than you can ever solve with a debugger...
                WriteLine(i + 1 + (i < 9 ? "  " : " ") + lines[i]);
            }
        }

        /// <summary>
        ///     A class representing a console variable
        /// </summary>
        public class ConsoleVariable<T> : Command where T : new()
        {
            private T value;

            public ConsoleVariable(string name)
            {
                Initialise(name, "", new T());
            }

            public ConsoleVariable(string name, string description)
            {
                Initialise(name, description, new T());
            }

            public ConsoleVariable(string name, T initialValue)
            {
                Initialise(name, "", initialValue);
            }

            public ConsoleVariable(string name, string description, T initalValue)
            {
                Initialise(name, description, initalValue);
            }

            public void Set(T val)
                // SE: I don't seem to know enough C# to provide a user friendly assignment operator solution
            {
                value = val;
            }

            public static implicit operator T(ConsoleVariable<T> var)
            {
                return var.value;
            }

            private void Initialise(string name, string description, T initalValue)
            {
                Name = name;
                Help = description;
                ParamsExample = "";
                value = initalValue;
                Callback = CommandFunction;
            }

            private static void CommandFunction(string parameters)
            {
                var split = CVarParameterSplit(parameters);
                if ((split.Length != 0) && VariableDictionary.ContainsKey(split[0]))
                {
                    var variable = VariableDictionary[split[0]] as ConsoleVariable<T>;
                    var conjunction = " is set to ";
                    if (split.Length == 2)
                    {
                        variable.SetFromString(split[1]);
                        conjunction = " has been set to ";
                    }

                    WriteLine(variable.Name + conjunction + variable.value);
                }
            }

            private void SetFromString(string value)
            {
                this.value = (T) Convert.ChangeType(value, typeof(T));
            }
        }
    }
}