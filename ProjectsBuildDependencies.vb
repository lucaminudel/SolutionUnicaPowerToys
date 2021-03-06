Imports System
Imports System.Diagnostics
Imports EnvDTE

Public Module ProjectsBuildDependencies


    Sub ListProjectsDependencies()

        ShowInfosForEachProject("dependencies", AddressOf ListProjectDependencies)

    End Sub

    Sub ListProjectsReferences()

        ShowInfosForEachProject("references", AddressOf ListProjectReferences)

    End Sub

    Sub ListAllProjectsNames()

        ShowInfosForEachProject("names", AddressOf ListProjectName)

    End Sub


    Sub ListAllProjectsUniqueNames()

        ShowInfosForEachProject("unique names", AddressOf ListProjectUniqueName)

    End Sub

    Private Delegate Sub ShowProjectInfo(ByVal Project As EnvDTE.Project)
    Private Sub ShowInfosForEachProject(ByVal ProjectsInfoDescription As String, ByVal ShowProjectInfoSub As ShowProjectInfo)
        If Not DTE.Solution.IsOpen Then
            MsgBox("Open a solution.", MsgBoxStyle.Exclamation)
            Return
        End If

        DTE.ExecuteCommand("Debug.Output")
        DTE.ExecuteCommand("View.Output")
        DTE.ToolWindows.OutputWindow.ActivePane.OutputString(Environment.NewLine + Environment.NewLine)

        DTE.ToolWindows.OutputWindow.ActivePane.OutputString("Projects " + ProjectsInfoDescription + ":" + Environment.NewLine)
        DTE.ToolWindows.OutputWindow.ActivePane.OutputString("----------------------------------" + Environment.NewLine)


        Dim Count As Int32 = 0
        For Each Project As EnvDTE.Project In DTE.Solution.Projects

            Count += VisitProjectsTree(Project, ShowProjectInfoSub)

        Next
        DTE.ToolWindows.OutputWindow.ActivePane.OutputString(Environment.NewLine + Count.ToString + " projects listed.")

    End Sub

    Private Function VisitProjectsTree(ByVal Project As EnvDTE.Project, ByVal ShowProjectInfoSub As ShowProjectInfo) As Int32
        Dim Count As Int32 = 0

        If Not Project.Kind.Contains("66A26720-8FB5-11D2-AA7E-00C04F688DDE") Then
            ShowProjectInfoSub(Project)
            Count = 1
        End If

        Dim ProjectItems As EnvDTE.ProjectItems = Project.ProjectItems
        If ProjectItems Is Nothing OrElse ProjectItems.Count <= 0 Then
            Return Count
        End If

        For Each ProjectItem As EnvDTE.ProjectItem In ProjectItems

            If Not ProjectItem.SubProject Is Nothing Then

                Count += VisitProjectsTree(ProjectItem.SubProject, ShowProjectInfoSub)
            End If

        Next

        Return Count
    End Function
    Private Sub ListProjectDependencies(ByVal Project As EnvDTE.Project)

        Dim Dependencies As Collections.Hashtable = New Collections.Hashtable()
        Try
            CollectProjectDependencies(Project, Dependencies)
        Catch
        End Try

        If Dependencies.Count = 0 Then
            DTE.ToolWindows.OutputWindow.ActivePane.OutputString(Environment.NewLine + "Project without dependencies: " + Project.UniqueName + Environment.NewLine)
            Return
        End If

        DTE.ToolWindows.OutputWindow.ActivePane.OutputString(Environment.NewLine + "Project dependencies of: " + Project.UniqueName + Environment.NewLine)

        For Each Dict As Collections.DictionaryEntry In Dependencies
            DTE.ToolWindows.OutputWindow.ActivePane.OutputString("   " + Dict.Key + Environment.NewLine)
        Next

        DTE.ToolWindows.OutputWindow.ActivePane.OutputString(Dependencies.Count.ToString + " dependencies listed." + Environment.NewLine)

    End Sub
    Private Sub ListProjectName(ByVal Project As EnvDTE.Project)

        DTE.ToolWindows.OutputWindow.ActivePane.OutputString(Project.Name() + " " + Environment.NewLine)

    End Sub


    Private Sub ListProjectUniqueName(ByVal Project As EnvDTE.Project)

        DTE.ToolWindows.OutputWindow.ActivePane.OutputString(Project.UniqueName() + Environment.NewLine)

    End Sub

    Private Sub ListProjectReferences(ByVal Project As EnvDTE.Project)
        DTE.ToolWindows.OutputWindow.ActivePane.OutputString(Environment.NewLine + "Project references of: " + Project.UniqueName + Environment.NewLine)

        Dim RealReferencesCount As Integer = 0
        Try
            Dim p As VSLangProj.VSProject = DTE.Solution.Projects.Item(Project.UniqueName).Object
            For Each Reference As VSLangProj.Reference In p.References
                If Reference.SourceProject Is Nothing Then
                    DTE.ToolWindows.OutputWindow.ActivePane.OutputString("   " + Reference.Path + Environment.NewLine)
                    RealReferencesCount = RealReferencesCount + 1
                End If
            Next
        Catch
        End Try

        If RealReferencesCount = 0 Then
            DTE.ToolWindows.OutputWindow.ActivePane.OutputString(Environment.NewLine + "Project without references: " + Project.UniqueName + Environment.NewLine)
            Return
        End If

        DTE.ToolWindows.OutputWindow.ActivePane.OutputString(RealReferencesCount.ToString + " references listed." + Environment.NewLine)

    End Sub

    'Friend Sub CollectProjectDependencies(ByVal ProjectUniqueName As String, ByVal Dependencies As Collections.Hashtable)
    '    Dim Project As EnvDTE.Project = GetProject(DTE.Solution.Projects.Item(ProjectUniqueName))
    '    CollectProjectDependencies(Project, Dependencies)
    'End Sub

    Friend Sub CollectProjectDependencies(ByVal Project As EnvDTE.Project, ByVal Dependencies As Collections.Hashtable)

        Dim BuildDependency As EnvDTE.BuildDependency = DTE.Solution.SolutionBuild.BuildDependencies.Item(Project.UniqueName)
        If BuildDependency Is Nothing Then
            Return
        End If

        Dim RequiredProjects As System.Array = BuildDependency.RequiredProjects

        For Each RequiredProjectObject As Object In RequiredProjects
            Dim RequiredProject As EnvDTE.Project
            Try
                RequiredProject = RequiredProjectObject
            Catch
                Continue For
            End Try

            If Not Dependencies.ContainsKey(RequiredProject.UniqueName) Then
                Dependencies.Add(RequiredProject.UniqueName, RequiredProject.Object)
                CollectProjectDependencies(RequiredProject, Dependencies)
            End If
        Next

    End Sub
    Friend Function GetProject(ByVal ProjectOrProjectIteam As Object) As EnvDTE.Project
        If TypeOf ProjectOrProjectIteam Is EnvDTE.Project Then
            Return CType(ProjectOrProjectIteam, EnvDTE.Project)
        ElseIf TypeOf ProjectOrProjectIteam Is EnvDTE.ProjectItem Then
            Return CType(ProjectOrProjectIteam, EnvDTE.ProjectItem).SubProject
        End If

        Throw New System.Exception("Not a Project nor a ProjectItem, what is it the ?!?!?!")
    End Function

End Module
