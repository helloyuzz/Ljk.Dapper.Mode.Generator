# Ljk.Dapper.Mode.Generator VISX Project

See also:  
https://marketplace.visualstudio.com/items?itemName=helloyuzz.LjkDapper

```csharp
public const int lanuch_CommandId = 0x0100;
public const int about_CommandId = 0x0107;
public const int nuGet_SubCommand = 0x0105;
public const int market_SubCommand = 0x0106;
```

```csharp
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
```

```csharp
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
```
Screent shot:  
<img src="https://github.com/helloyuzz/Ljk.Dapper.Mode.Generator/blob/master/Ljk.Dapper.Mode.Generator/Screenshot/pic1.png" width="600px" height="300px">

<img src="https://github.com/helloyuzz/Ljk.Dapper.Mode.Generator/blob/master/Ljk.Dapper.Mode.Generator/Screenshot/pic2.png" width="600px" height="300px">

