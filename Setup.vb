Imports System
Imports EnvDTE
Imports EnvDTE80
Imports EnvDTE90
Imports System.Diagnostics

Public Module Setup
    Sub SetupToolbar()

        Dim MyToolbar As Microsoft.VisualStudio.CommandBars.CommandBar
        Const ToolbarName As String = "SolutionUnicaPowerToysToolbar"


        '
        ' Create the Toolbar
        '
        MyToolbar = DTE.Commands.AddCommandBar(ToolbarName, EnvDTE.vsCommandBarType.vsCommandBarTypeToolbar)
        MyToolbar.Enabled = True
        MyToolbar.Visible = True

        '
        ' Add comand buttons to the Toolbar
        '
        AddCommandButtonToTheTollbar(MyToolbar, "Macros.SolutionUnicaPowerToys.ConfigurationManagement.EnableOnlySelectedProjectAndItsBuildDependencies", " Enable Selected ")
        'AddCommandButtonToTheTollbar(MyToolbar, "Macros.SolutionUnicaPowerToys.ConfigurationManagement.DisableAllProjects", " Disable All ")
        AddCommandButtonToTheTollbar(MyToolbar, "Macros.SolutionUnicaPowerToys.ConfigurationManagement.EnableAllProjects", " Enable All ")




        '
        ' Define keyboard Shortcut fot the commands
        '
        'SetCommandShortcutKey("Macros.Samples.Accessibility.IncreaseTextEditorFontSize", "Global::Alt+M")
        'SetCommandShortcutKey("Macros.Samples.Accessibility.DecreaseTextEditorFontSize", "Global::Alt+S")
        'End If

        SetToolbarTooltips()

    End Sub

    Sub DeleteToolbar()

        Dim MyToolbar As Microsoft.VisualStudio.CommandBars.CommandBar
        Const ToolbarName As String = "SolutionUnicaPowerToysToolbar"

        '
        ' Delete the Toolbar 
        '
        Try
            MyToolbar = DTE.CommandBars.Item(ToolbarName)
            MyToolbar.Delete()
        Catch e As ArgumentException
        End Try
    End Sub


    Private Sub SetCommandShortcutKey(ByVal CommandName As String, ByVal Shortcut As String)

        Dim MacroCommand As Command = DTE.Commands.Item(CommandName)
        MacroCommand.Bindings = Shortcut

    End Sub

    Private Sub AddCommandButtonToTheTollbar(ByVal Toolbar As Microsoft.VisualStudio.CommandBars.CommandBar, ByVal CommandName As String, ByVal CommandCaption As String)

        Dim MacroCommand As Command = DTE.Commands.Item(CommandName)
        Dim ToolbarButton As Microsoft.VisualStudio.CommandBars.CommandBarButton = MacroCommand.AddControl(Toolbar)
        ToolbarButton.Visible = True
        ToolbarButton.Style = Microsoft.VisualStudio.CommandBars.MsoButtonStyle.msoButtonCaption
        ToolbarButton.Caption = CommandCaption
        'ToolbarButton.TooltipText = CommandTooltip

    End Sub

    Friend Sub SetToolbarTooltips()
        Try
            ' Esco se la toolbar non esiste
            Dim MyToolbar As Microsoft.VisualStudio.CommandBars.CommandBar
            Try
                MyToolbar = DTE.CommandBars("SolutionUnicaPowerToysToolbar")
            Catch e As ArgumentException
                Return
            End Try

            SetToolbarButtonTooltip(MyToolbar, " Enable Selected ", "Enable only selected projects")
            'SetToolbarButtonTooltip(MyToolbar, " Disable All ", "Disable all projects")
            SetToolbarButtonTooltip(MyToolbar, " Enable All ", "Enable all projects")
        Catch
        End Try


    End Sub

    Private Sub SetToolbarButtonTooltip(ByVal MyToolbar As Microsoft.VisualStudio.CommandBars.CommandBar, ByVal ButtonName As String, ByVal TooltipText As String)
        Try
            Dim ToolbarButton As Microsoft.VisualStudio.CommandBars.CommandBarButton = MyToolbar.Controls.Item(ButtonName)
            ToolbarButton.TooltipText = TooltipText
        Catch e As ArgumentException
        End Try

    End Sub

End Module
