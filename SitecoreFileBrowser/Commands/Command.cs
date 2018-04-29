namespace SitecoreFileBrowser.Commands
{
    public abstract class Command 
    {
        protected Command(string commandName)
        {
            CommandName = commandName;
        }

        public string CommandName { get; }

        public abstract object Execute(CommandArguments args);
    }
}
