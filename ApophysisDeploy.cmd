@echo off

setlocal EnableExtensions
if .%1 equ . goto usage

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:loop
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

if .%1 equ . goto main
set argNamed=%1
if "%argNamed:~0,1%" NEQ "/" goto usage
set argNamed=%argNamed:~1%
for /f "tokens=1,2 delims=:" %%i in ("%argNamed%") do set %%i=%%j
shift
goto loop
  
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:main
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

call :getreg HKLM\Software\7-Zip Path
if .%__getRegC% neq . (
set SevenZip=%__getRegC%
) else (
set SevenZip=%ProgramFiles%\7-Zip\
)

set path=%SevenZip%;%path%

if .%i% neq . set directory=%i%
if .%t% neq . set temp=%t%

if .%h% neq . set sftp-server=%h%
if .%u% neq . set sftp-user=%u%
if .%p% neq . set sftp-password=%p%
if .%o% neq . set sftp-target=%o%
if .%l% neq . set label=%l%

if .%directory% equ . set directory=..\..\out\deploy
if .%sftp-server% equ . goto usage

if .%temp% equ . set temp=%temp%\

set timestamp=%date:~-4%%date:~-10,2%%date:~-7,2%
set sftp-spath=sftp://%sftp-server%/

if .%sftp-target% neq . set sftp-spath=%sftp-spath%%sftp-target%/
if .%label% neq . set sftp-spath=%sftp-spath%%label%-

set sftp-tpath="%sftp-spath%latest"
set sftp-spath="%sftp-spath%%timestamp%.zip"

if .%sftp-user% neq . (
if .%sftp-password% neq . (
  set sftp-uarg=-u %sftp-user%:%sftp-password%
) else (
  set sftp-uarg=-u %sftp-user%
)
)

if "%sftp-uarg%" neq "" set sftp-spath=%sftp-uarg% %sftp-spath%
if "%sftp-uarg%" neq "" set sftp-tpath=%sftp-uarg% %sftp-tpath%

set zipfile=%temp%\~%timestamp%.zip
set infofile=%temp%\~%timestamp%.dat

echo.
echo ------------------------------------------------------------------------------
echo --- CREATING REMOTE ARCHIVE 
echo ------------------------------------------------------------------------------
7z a -y -r -bd "%zipfile%" "%cd%\%directory%\*.*"
curl -s -k -T "%zipfile%" %sftp-spath%
del /Q "%zipfile%"
echo Success!

echo.
echo ------------------------------------------------------------------------------
echo --- CREATING REMOTE METADATA FILES
echo ------------------------------------------------------------------------------
echo %timestamp% > %infofile%
curl -s -k -T "%infofile%" %sftp-tpath%
del /Q "%infofile%"
echo Success!

goto :eof
  
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:getreg
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
  set RegKeyName=%1
  set RegValueName=%2
  
  if .%RegValueName% equ . (
    set RegValueQuery=/ve
  ) else (
    set RegValueQuery=/v %RegValueName%
  )
  
  :: Needs to use 4 if Windows version < win7
  set RegTokenSkip=2
  
  for /f "usebackq skip=%RegTokenSkip% tokens=5*" %%A in (`REG QUERY %RegKeyName% %RegValueQuery% 2^>nul`) do (
    set __getRegA=%%A
    set __getRegB=%%B
    set __getRegC=%%C
  )
  
  goto :eof

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::  
:usage
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

  echo.
  echo Usage: xw-upload [/option:value, ...] /i:^<input directory^> /h:^<hostname^>
  echo Options:
  echo     /temp          ^| /t  Path to temporary directory
  echo     /sftp-server   ^| /h  Host name for remote SFTP connection
  echo     /sftp-user     ^| /u  User name for remote SFTP connection
  echo     /sftp-password ^| /p  Password for remote SFTP connection
  echo     /sftp-target   ^| /o  Target directory (on SFTP host)
  echo     /label         ^| /l  Payload label
  goto :eof