using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace Ljk.Dapper.Mode.Generator {
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class ToolCommand {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int lanuch_CommandId = 0x0100;
        public const int about_CommandId = 0x0107;
        public const int nuGet_SubCommand = 0x0105;
        public const int market_SubCommand = 0x0106;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("e5ffb04b-a6df-47ee-9ba9-700c03d0edd0");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private ToolCommand(AsyncPackage package,OleMenuCommandService commandService) {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var lanuch_MenuCommandID = new CommandID(CommandSet,lanuch_CommandId);
            var about_MenuCommandID = new CommandID(CommandSet,about_CommandId);
            var nuget_MenuCommandID = new CommandID(CommandSet,nuGet_SubCommand);
            var market_MenuCommandID = new CommandID(CommandSet,market_SubCommand);

            var lanuch_MenuItem = new MenuCommand(this.Execute,lanuch_MenuCommandID);
            var about_MenuItem = new MenuCommand(this.Execute,about_MenuCommandID);
            var nuget_MenuItem = new MenuCommand(this.Execute,nuget_MenuCommandID);
            var market_MenuItem = new MenuCommand(this.Execute,market_MenuCommandID);

            commandService.AddCommand(lanuch_MenuItem);
            commandService.AddCommand(about_MenuItem);
            commandService.AddCommand(nuget_MenuItem);
            commandService.AddCommand(market_MenuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static ToolCommand Instance {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider {
            get {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package) {
            // Switch to the main thread - the call to AddCommand in TestCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            Instance = new ToolCommand(package,commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender,EventArgs e) {
            MenuCommand menuCommand = (MenuCommand)sender;
            switch(menuCommand.CommandID.ID) {
                case 0x0100:    // lanuch_Command
                    Process.Start("com.yuzz.DbGenerator.exe");
                    break;
                case 0x0105:    // nuGet_SubCommand
                    Process.Start("https://www.nuget.org/packages/Ljk.Dapper");
                    break;
                case 0x0106:    // market_SubCommand
                    Process.Start("https://marketplace.visualstudio.com/items?itemName=helloyuzz.LjkDapper");
                    break;
                case 0x0107:    // about_SubCommand
                    ThreadHelper.ThrowIfNotOnUIThread();                    
                    string message = "Ljk.Dapper.ModeGenerator v1.1";
                    string title = "About";

                    // Show a message box to prove we were here
                    VsShellUtilities.ShowMessageBox(
                        this.package,
                        message,
                        title,
                        OLEMSGICON.OLEMSGICON_INFO,
                        OLEMSGBUTTON.OLEMSGBUTTON_OK,
                        OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                    break;
            }
        }
    }
}
