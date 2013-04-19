@ECHO off

SET _ConfigSubversionRepositoryPath=https://apo-plugins.svn.sourceforge.net/svnroot/apo-plugins/personal/georgkiehne/updated_for_x64

SET CURRENT=%CD%
CD /D %~d0%~p0

CALL :GetVSCommonToolsDir
IF "%VS110COMNTOOLS%"=="" GOTO error_no_VS110COMNTOOLSDIR
CALL "%VS110COMNTOOLS%VCVarsQueryRegistry.bat" 32bit No64bit

IF "%VSINSTALLDIR%"=="" GOTO error_no_VSINSTALLDIR
IF "%FrameworkDir32%"=="" GOTO error_no_FrameworkDIR32
IF "%FrameworkVersion32%"=="" GOTO error_no_FrameworkVer32
IF "%Framework35Version%"=="" GOTO error_no_Framework35Version

SET FrameworkDir=%FrameworkDir32%
SET FrameworkVersion=%FrameworkVersion32%

IF NOT "%WindowsSdkDir_old%" == "" (
	SET "PATH=%WindowsSdkDir_old%bin\NETFX 4.0 Tools;%WindowsSdkDir_35%;%PATH%"
)

IF NOT "%WindowsSdkDir%" == "" (
	SET "PATH=%WindowsSdkDir%bin\x86;%PATH%"
	SET "INCLUDE=%WindowsSdkDir%include\shared;%WindowsSdkDir%include\um;%WindowsSdkDir%include\winrt;%INCLUDE%"
	SET "LIB=%WindowsSdkDir%lib\win8\um\x86;%LIB%"
	SET "LIBPATH=%WindowsSdkDir%References\CommonConfiguration\Neutral;%ExtensionSDKDir%\Microsoft.VCLibs\11.0\References\CommonConfiguration\neutral;%LIBPATH%"
)

SET DevEnvDir=%VSINSTALLDIR%Common7\IDE\

IF EXIST "%VSINSTALLDIR%Team Tools\Performance Tools" (
	SET "PATH=%VSINSTALLDIR%Team Tools\Performance Tools;%PATH%"
)
IF EXIST "%ProgramFiles%\HTML Help Workshop" SET PATH=%ProgramFiles%\HTML Help Workshop;%PATH%
IF EXIST "%ProgramFiles(x86)%\HTML Help Workshop" SET PATH=%ProgramFiles(x86)%\HTML Help Workshop;%PATH%
IF EXIST "%VCINSTALLDIR%VCPackages" SET PATH=%VCINSTALLDIR%VCPackages;%PATH%
SET PATH=%FrameworkDir%%Framework35Version%;%PATH%
SET PATH=%FrameworkDir%%FrameworkVersion%;%PATH%
SET PATH=%VSINSTALLDIR%Common7\Tools;%PATH%
IF EXIST "%VCINSTALLDIR%BIN" SET PATH=%VCINSTALLDIR%BIN;%PATH%
SET PATH=%DevEnvDir%;%PATH%

IF EXIST "%VSINSTALLDIR%VSTSDB\Deploy" (
	SET "PATH=%VSINSTALLDIR%VSTSDB\Deploy;%PATH%"
)

IF NOT "%FSHARPINSTALLDIR%" == "" (
	SET "PATH=%FSHARPINSTALLDIR%;%PATH%"
)

IF EXIST "%DevEnvDir%CommonExtensions\Microsoft\TestWindow" (
	SET "PATH=%DevEnvDir%CommonExtensions\Microsoft\TestWindow;%PATH%"
)

IF EXIST "%VCINSTALLDIR%ATLMFC\INCLUDE" SET INCLUDE=%VCINSTALLDIR%ATLMFC\INCLUDE;%INCLUDE%
IF EXIST "%VCINSTALLDIR%INCLUDE" SET INCLUDE=%VCINSTALLDIR%INCLUDE;%INCLUDE%

IF EXIST "%VCINSTALLDIR%ATLMFC\LIB" SET LIB=%VCINSTALLDIR%ATLMFC\LIB;%LIB%
IF EXIST "%VCINSTALLDIR%LIB" SET LIB=%VCINSTALLDIR%LIB;%LIB%

IF EXIST "%VCINSTALLDIR%ATLMFC\LIB" SET LIBPATH=%VCINSTALLDIR%ATLMFC\LIB;%LIBPATH%
IF EXIST "%VCINSTALLDIR%LIB" SET LIBPATH=%VCINSTALLDIR%LIB;%LIBPATH%

SET LIBPATH=%FrameworkDir%%Framework35Version%;%LIBPATH%
SET LIBPATH=%FrameworkDir%%FrameworkVersion%;%LIBPATH%

SET VisualStudioVersion=11.0

SET TODODIR=%~d0%~p0src
SET DONEDIR=%~d0%~p0out
SET LOGDIR=%~d0%~p0log
SET TEMPDIR=%~d0%~p0tmp
SET MKPLUGIN=%~d0%~p0bin\mkplugin.exe
SET SUBVERSION=%~d0%~p0bin\svn.exe

IF EXIST "%doneDir%" RMDIR /S /Q "%doneDir%" > NUL 2>&1
IF EXIST "%logDir%" RMDIR /S /Q "%logDir%" > NUL 2>&1
IF EXIST "%tempDir%" RMDIR /S /Q "%tempDir%" > NUL 2>&1

MKDIR "%doneDir%"
MKDIR "%logDir%"
MKDIR "%tempDir%"

ECHO Downloading current source code...
IF NOT EXIST "%todoDir%" MKDIR "%todoDir%"
"%SUBVERSION%" co %_ConfigSubversionRepositoryPath% "%todoDir%" > NUL 2>&1
IF %ERRORLEVEL% EQU 0 (
	ECHO SUCCESS: Source code is up-to-date!
	GOTO contsvn
)
ECHO ERROR: Failed to update source code. Continuing with local copy...
:contsvn
ECHO.

ECHO Building binaries...
FOR /F %%a IN ('DIR /b "%todoDir%\*.c"') DO (
	"%mkplugin%" "%%~na" "%tempDir%\%%~na" > NUL 2>&1
	ATTRIB -H "%tempDir%\%%~na\plugin.h" > NUL 2>&1
	REN "%tempDir%\%%~na\plugin.h" "apoplugin.h" > NUL 2>&1
	COPY /Y "%todoDir%\%%a" "%tempDir%\%%~na\%%~na.cpp" > NUL 2>&1

	MsBuild /nologo /verbosity:quiet /p:Configuration=Release;Platform=Win32 "%tempDir%\%%~na\%%~na.vcxproj" > "%logDir%\%%~na.x86.log"
	MsBuild /nologo /verbosity:quiet /p:Configuration=Release;Platform=x64 "%tempDir%\%%~na\%%~na.vcxproj" > "%logDir%\%%~na.x64.log"
	
	IF EXIST "%tempDir%\%%~na\bin\x86\%%~na.dll" (
		COPY /Y "%tempDir%\%%~na\bin\x86\%%~na.dll" "%doneDir%\%%~na.x86.dll" > NUL 2>&1
		DEL /Q "%logDir%\%%~na.x86.log" > NUL
		
		IF EXIST "%tempDir%\%%~na\bin\x64\%%~na.dll" (
			COPY /Y "%tempDir%\%%~na\bin\x64\%%~na.dll" "%doneDir%\%%~na.x64.dll" > NUL 2>&1
			DEL /Q "%logDir%\%%~na.x64.log" > NUL
			
			ECHO SUCCESS: Build of "%%~na" completed!
		)
		
		IF NOT EXIST "%tempDir%\%%~na\bin\x64\%%~na.dll" (
			ECHO ERROR: Build of "%%~na" failed!
			TYPE "%logDir%\%%~na.x64.log"
			ECHO.
		)
	)
	
	IF NOT EXIST "%tempDir%\%%~na\bin\x86\%%~na.dll" (
		ECHO ERROR: Build of "%%~na" failed!
		TYPE "%logDir%\%%~na.x86.log"
		ECHO.
	)
	
	RMDIR /S /Q "%tempDir%\%%~na" > NUL 2>&1
)

RMDIR /S /Q "%tempDir%" > NUL 2>&1
GOTO end

::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: HELPER METHODS
::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

:GetVSCommonToolsDir
@SET VS110COMNTOOLS=
@CALL :GetVSCommonToolsDirHelper32 HKLM > NUL 2>&1
@IF ERRORLEVEL 1 CALL :GetVSCommonToolsDirHelper32 HKCU > NUL 2>&1
@IF ERRORLEVEL 1 CALL :GetVSCommonToolsDirHelper64  HKLM > NUL 2>&1
@IF ERRORLEVEL 1 CALL :GetVSCommonToolsDirHelper64  HKCU > NUL 2>&1
@EXIT /B 0

:GetVSCommonToolsDirHelper32
@FOR /F "tokens=1,2*" %%i IN ('reg query "%1\SOFTWARE\Microsoft\VisualStudio\SxS\VS7" /v "11.0"') DO (
	@IF "%%i"=="11.0" (
		@SET "VS110COMNTOOLS=%%k"
	)
)
@IF "%VS110COMNTOOLS%"=="" EXIT /B 1
@SET "VS110COMNTOOLS=%VS110COMNTOOLS%Common7\Tools\"
@EXIT /B 0

:GetVSCommonToolsDirHelper64
@FOR /F "tokens=1,2*" %%i IN ('reg query "%1\SOFTWARE\Wow6432Node\Microsoft\VisualStudio\SxS\VS7" /v "11.0"') DO (
	@IF "%%i"=="11.0" (
		@SET "VS110COMNTOOLS=%%k"
	)
)
@IF "%VS110COMNTOOLS%"=="" EXIT /B 1
@SET "VS110COMNTOOLS=%VS110COMNTOOLS%Common7\Tools\"
@EXIT /B 0

:error_no_VS110COMNTOOLSDIR
ECHO ERROR: Cannot determine the location of the VS Common Tools folder.
GOTO end

:error_no_VSINSTALLDIR
ECHO ERROR: Cannot determine the location of the VS installation.
GOTO end

:error_no_FrameworkDIR32
ECHO ERROR: Cannot determine the location of the .NET Framework 32bit installation.
GOTO end

:error_no_FrameworkVer32
ECHO ERROR: Cannot determine the version of the .NET Framework 32bit installation.
GOTO end

:error_no_Framework35Version
ECHO ERROR: Cannot determine the .NET Framework 3.5 version.
GOTO end

:end
CD /D %CURRENT%
ECHO.
ECHO Press any key to continue . . .
PAUSE > NUL