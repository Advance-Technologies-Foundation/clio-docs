# Architecture Circle

## Global Settings

| Item           | Value                                                                |
|:---------------|:---------------------------------------------------------------------|
| Git Repository | https://gitlab.example.com/Creatio/architecturecircle        |
| SonarQube      | http://sonarqube.example.com/dashboard?id=architecturecircle&branch=master |
| AppName        | RecordBoost                                                          |
| PkgName        | RecordBoost                                                          |
| Stage_Net472   | http://stage-net472-example.com:8060                                         |
| Stage_Net6     | http://stage-net6-example.com:8080                                 |
| UAT_Net472     | N/A                                                                  |
| UAT_Net6       | N/A                                                                  |
| Username       | MyUser                                                           |
| Password       | MyPassword                                                           |

### Dev Sys Settings

| SysSetting Code   | SysSetting Name                       | Value                            |
|:------------------|:--------------------------------------|:---------------------------------|
| SchemaNamePrefix	 | Prefix for schemas and packages name	 | ATF                              |
| Maintainer         | Publisher                             | Advanced Technologies Foundation |

Some scenario files have been included in the repository for convenience.

| File                      | Purpose                                             |
|:--------------------------|:----------------------------------------------------|
| init-dev-settings.yaml    | Initialize settings for the development environment |
| install-required-pkg.yaml | Installs Customer360                                |

### [Link Workspace to File Design Mode](https://github.com/Advance-Technologies-Foundation/clio?tab=readme-ov-file#link-workspace-to-file-design-mode)

- Net6
  ```powershell
  # Install dev_net6, run from repository root folder
  clio l4r -r . -e D:\Projects\inetpub\wwwroot\dev_net6\Terrasoft.Configuration\Pkg -p *;
  
  # Update db on dev_net6 from file system
  clio 2db -e dev_net6;
  ```

- Net472
  ```powershell
  # Install App on dev_net472, run from repository root folder
  clio l4r -r . -e D:\Projects\inetpub\wwwroot\dev_net472\Terrasoft.WebApp\Terrasoft.Configuration\Pkg -p *;
  
  # Update db on dev_net472 from file system
  clio 2db -e dev_net472;
  ```

## Open Solution for development

To facilitate Test-Driven Development (TDD),
you might want to open the solution that includes both unit tests and the package.
Follow these steps to open the appropriate solution:

1. Navigate to the tasks directory:
   ```powershell
   cd tasks\
   ```
2. Open the test solution:
    - Net6
   ```powershell
   .\open-test-solution-netcore.cmd
   ```
    - Net472
   ```powershell
   .\open-test-solution-netframework.cmd
   ```

## Run Tests and Generate Reports

To run tests and generate reports, follow these steps:

1. Install required tools to run tests
    ```powershell
    dotnet tool update dotnet-reportgenerator-globaltool -g
    dotnet tool update LiveReloadServer -g
    ```
   > Note: If you intend to server/host reports with a real server such as (IIS, Apache, Nginx, etc.),
   you can skip the installation of **LiveReloadServer**.

2. Navigate to the tasks directory:
    ```powershell
    cd tasks\
    ```
3. Run the tests:
    - Net6
    ```powershell
    .\run-tests-netcore.cmd
    ```
    - Net472
    ```powershell
    .\run-tests-netframework.cmd
    ```

