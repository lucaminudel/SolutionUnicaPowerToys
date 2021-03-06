Imports System
Imports EnvDTE
Imports EnvDTE80
Imports EnvDTE90
Imports System.Diagnostics

Public Module ConfigurationManagement

    Public Sub ListProjectsStateInCurrentConfiguration()
        If Not DTE.Solution.IsOpen Then
            MsgBox("Open a solution.", MsgBoxStyle.Exclamation)
            Return
        End If


        DTE.ExecuteCommand("Debug.Output")
        DTE.ExecuteCommand("View.Output")
        DTE.ToolWindows.OutputWindow.ActivePane.OutputString(Environment.NewLine + Environment.NewLine)

        DTE.ToolWindows.OutputWindow.ActivePane.OutputString("Active Projects in current Configuration: " + DTE.Solution.SolutionBuild.ActiveConfiguration.Name + Environment.NewLine)
        DTE.ToolWindows.OutputWindow.ActivePane.OutputString("--------------------------------------------------------" + Environment.NewLine)

        Dim ActiveProjectsCount As Integer = 0
        For Each ProjectActiveConfiguration As EnvDTE.SolutionContext In DTE.Solution.SolutionBuild.ActiveConfiguration.SolutionContexts
            DTE.ToolWindows.OutputWindow.ActivePane.OutputString(IIf(ProjectActiveConfiguration.ShouldBuild, "[x] ", "[ ] ") + ProjectActiveConfiguration.ProjectName + Environment.NewLine)

            If ProjectActiveConfiguration.ShouldBuild Then
                ActiveProjectsCount += 1
            End If
        Next

        DTE.ToolWindows.OutputWindow.ActivePane.OutputString(Environment.NewLine + ActiveProjectsCount.ToString + " projects are active.")


    End Sub

    Public Sub EnableAllProjects()
        SetToAllProjectsShouldBuild(True)

        DTE.ExecuteCommand("Debug.Output")
        DTE.ExecuteCommand("View.Output")
        DTE.ToolWindows.OutputWindow.ActivePane.OutputString(Environment.NewLine + Environment.NewLine)
        DTE.ToolWindows.OutputWindow.ActivePane.OutputString("All projects enable." + Environment.NewLine)

    End Sub
    Public Sub DisableAllProjects()
        SetToAllProjectsShouldBuild(False)

        DTE.ExecuteCommand("Debug.Output")
        DTE.ExecuteCommand("View.Output")
        DTE.ToolWindows.OutputWindow.ActivePane.OutputString(Environment.NewLine + Environment.NewLine)
        DTE.ToolWindows.OutputWindow.ActivePane.OutputString("All projects enable." + Environment.NewLine)

    End Sub
    Public Sub EnableOnlySelectedProjectAndItsBuildDependencies()
        If Not DTE.Solution.IsOpen Then
            MsgBox("Open a solution.", MsgBoxStyle.Exclamation)
            Return
        End If



        DTE.ExecuteCommand("Debug.Output")
        DTE.ExecuteCommand("View.Output")
        DTE.ToolWindows.OutputWindow.ActivePane.OutputString(Environment.NewLine + Environment.NewLine)

        DTE.ToolWindows.OutputWindow.ActivePane.OutputString("Enable selected projects" + Environment.NewLine)
        DTE.ToolWindows.OutputWindow.ActivePane.OutputString("-----------------------------" + Environment.NewLine)

        Dim SolutionExplorer As UIHierarchy = DTE.Windows.Item(Constants.vsWindowKindSolutionExplorer).Object
        Dim SelectedItems As System.Array = SolutionExplorer.SelectedItems

        Dim ProjectsToEnable As Collections.Hashtable = New Collections.Hashtable

        For Each SelectedItem As UIHierarchyItem In SelectedItems

            Dim Project As EnvDTE.Project
            Try
                Project = ProjectsBuildDependencies.GetProject(SelectedItem.Object)
            Catch ex As Exception
                Return
            End Try

            Dim Dependencies As Collections.Hashtable = New Collections.Hashtable
            ProjectsBuildDependencies.CollectProjectDependencies(Project, Dependencies)
            Dependencies.Add(Project.UniqueName, Nothing)

            For Each Element As Collections.DictionaryEntry In Dependencies
                If Not ProjectsToEnable.Contains(Element.Key) Then
                    ProjectsToEnable.Add(Element.Key, Element.Value)
                End If
            Next

            DTE.ToolWindows.OutputWindow.ActivePane.OutputString(Project.Name + " project and its build dependencies enabled" + Environment.NewLine)
        Next

        EnableOnlyProjectsIn(ProjectsToEnable)

        DTE.ToolWindows.OutputWindow.ActivePane.OutputString(Environment.NewLine + ProjectsToEnable.Count.ToString + " projects enabled." + Environment.NewLine)

    End Sub


    Private Sub SetToAllProjectsShouldBuild(ByVal ShouldBuild As Boolean)
        If Not DTE.Solution.IsOpen Then
            MsgBox("Open a solution.", MsgBoxStyle.Exclamation)
            Return
        End If

        For Each SolutionConfiguration As EnvDTE.SolutionConfiguration In DTE.Solution.SolutionBuild.SolutionConfigurations
            For Each ProjectActiveConfiguration As EnvDTE.SolutionContext In SolutionConfiguration.SolutionContexts
                ProjectActiveConfiguration.ShouldBuild = ShouldBuild
            Next
        Next

    End Sub

    Private Sub EnableOnlyProjectsIn(ByVal OnlyProjectsToBeEnabled As Collections.Hashtable)
        For Each SolutionConfiguration As EnvDTE.SolutionConfiguration In DTE.Solution.SolutionBuild.SolutionConfigurations
            For Each ProjectActiveConfiguration As EnvDTE.SolutionContext In SolutionConfiguration.SolutionContexts

                ProjectActiveConfiguration.ShouldBuild = OnlyProjectsToBeEnabled.Contains(ProjectActiveConfiguration.ProjectName)

            Next
        Next
    End Sub

    Private Sub EnableProjectsIn(ByVal ProjectsToBeEnabled As Collections.Hashtable)
        For Each SolutionConfiguration As EnvDTE.SolutionConfiguration In DTE.Solution.SolutionBuild.SolutionConfigurations
            For Each ProjectActiveConfiguration As EnvDTE.SolutionContext In SolutionConfiguration.SolutionContexts
                If ProjectsToBeEnabled.Contains(ProjectActiveConfiguration.ProjectName) Then
                    ProjectActiveConfiguration.ShouldBuild = True
                End If
            Next
        Next
    End Sub

End Module
