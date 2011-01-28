#SolutionUnicaPowerToys for Visual Studio 2008 & 2010

Is a set of macros do deal with Solutions that contain a huge number ofProjects.
Macros in SolutionUnicaPowerToys let you analyze projects references and dependencies
and let you selectively enable the build of a limited number of Projects and only their
references.

##Here is the list of available commands:
-**Solution Projects**
  - ListAllProjectsNames:  recursively lists and counts all the projects in the solution
  - ListAllProjectsUniqueNames:  the same as ListAllProjectsNames with project unique names
  - ListProjectsDependencies:  for every project lists the projects referenced
  - ListProjectsReferences:  for every project lists the binary references
-**Configuration Management**
  - ListProjectsStateInCurrentConfiguration:  list all projects active in the current solution 
    configuration (i.e. Debug and Release)
  - EnableAllProjects:  enable in the current solution configuration the build for all projects
  - DisableAllProjects:  disable in the current solution configuration the build for all projects
  - **EnableOnlySelectedProjectAndItsBuildDependencies**:  enable in the current solution configuration 
    the build for only the selected projects and all the projects referenced by them
  

##To install the Macro:
Alt+F8 will open the macro explorer. Right click and select Load Macro
Keep the macro explorer and click the commands you like to exxecute. 
Then read the output in the VS Output windows for the Debug.

