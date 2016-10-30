// SE: annoying having to leak this out publicly - basically to facilitate the weird and wonderful cvar implementation
namespace Assets.SmartConsole.Code
{
    /// <summary>
    ///     A class representing a console command - WARNING: this is only exposed as a hack!
    /// </summary>
    public class Command
    {
        public Console.ConsoleCommandFunction Callback;
        public string Help = "(no description)";
        public string Name;
        public string ParamsExample = "";
    }
}