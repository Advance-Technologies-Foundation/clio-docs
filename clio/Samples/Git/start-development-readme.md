# Init project repository
Create git repository

# Init workspace
clio createw <Project_Name>
START tasks\git-init-workspace.cmd <Project_Repository_Url>

# Create application on dev environment

# Create development branch for environment
START tasks\git-create-branch.cmd <Environment_Name>, <PackagesName1,PackagesName2>

**Change default environment for clio**


# Send changes from development environment to git
START tasks\git-push.cmd
