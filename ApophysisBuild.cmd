@echo off

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:setup
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

setlocal EnableExtensions

:: Needs to use 4 if Windows version < win7
set RegTokenSkip=2

:: Remaining fields
set ProjectDir=%~d0%~p0
set LangDir=EN
set Platform=
set PlatformSDK=

set BdsVersion=16.0
set FrameworkVersion=v3.5

set BdsRegKeyName=HKCU\Software\Embarcadero\BDS\%BdsVersion%
set BdsRegValueName=RootDir

set InnoRegKeyName=HKEY_LOCAL_MACHINE\SOFTWARE\Classes\InnoSetupScriptFile\shell\open\command

for /f "usebackq skip=%RegTokenSkip% tokens=1-3" %%A in (`REG QUERY %BdsRegKeyName% /v %BdsRegValueName% 2^>nul`) do (
    set __BDSA=%%A
    set __BDSB=%%B
    set __BDSC=%%C
)

set Bds=%ProgramFiles(x86)%\Embarcadero\Studio\%BdsVersion%
if defined __BDSC set Bds=%__BDSC%

set BdsInclude=%Bds%\include
set BdsCommonDir=%Public%\Documents\Embarcadero\Studio\%BdsVersion%
set BdsInterbase=%BdsCommonDir%\..\..\InterBase\redist\InterBaseXE7\IDE_spoof

set FrameworkDir=%WinDir%\Microsoft.NET\Framework\%FrameworkVersion%
set FrameworkSdkDir=

for /f "usebackq skip=%RegTokenSkip% tokens=3*" %%A in (`REG QUERY %InnoRegKeyName% /ve 2^>nul`) do (
    set __INSC=%%A %%B
)

set Inno=%ProgramFiles(x86)%\Inno Setup 5
if defined __INSC call :getInnoPathFromRegShellOpen %__INSC%

set Path=%FrameworkDir%;%FrameworkSDKDir%;%Bds%\bin;%Bds%\bin64;%BdsInterbase%;%Inno%;%Path%

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:build_app
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

echo.
echo ------------------------------------------------------------------------------
echo --- BUILDING 32 BIT BINARY
echo ------------------------------------------------------------------------------
msbuild /v:m /nologo /t:build /p:config=Release /p:platform=Win32 %ProjectDir%\Apophysis7x.groupproj
if %ErrorLevel% neq 0 goto error

echo.
echo ------------------------------------------------------------------------------
echo --- BUILDING 64 BIT BINARY
echo ------------------------------------------------------------------------------
msbuild /v:m /nologo /t:build /p:config=Release /p:platform=Win64 %ProjectDir%\Apophysis7x.groupproj
if %ErrorLevel% neq 0 goto error

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:build_setup
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

echo.
echo ------------------------------------------------------------------------------
echo --- BUILDING 32 BIT SETUP
echo ------------------------------------------------------------------------------
iscc /q ApophysisSetup.iss
if %ErrorLevel% neq 0 goto error
echo Success!

echo.
echo ------------------------------------------------------------------------------
echo --- BUILDING 64 BIT SETUP
echo ------------------------------------------------------------------------------
iscc /q ApophysisSetup64.iss
if %ErrorLevel% neq 0 goto error
echo Success!

goto shutdown

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:error
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
echo Fatal error in build step. Exiting...
goto shutdown

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:getInnoPathFromRegShellOpen
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
set Inno=%~d1%~p1
exit /b

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:shutdown
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
endlocal