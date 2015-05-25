
unit PluginVariation;

interface uses Variation;

const PLUGIN_PLATFORM = {$ifdef Apo7X64}$00000040{$else}$00000020{$endif};

type
  TPluginVariationClass = class of TPluginVariation;
  TPluginData = record
      Instance: Integer;
      PluginHandle: THandle;
      PluginClass: TPluginVariationClass;

      PluginVarGetName:           function: PAnsiChar; cdecl;
      PluginVarGetNrVariables:    function: Integer; cdecl;
      PluginVarGetVariableNameAt: function(const Index: integer): PAnsiChar; cdecl;

      PluginVarCreate:       function: Pointer; cdecl;
      PluginVarDestroy:      function(var MyVariation: Pointer): LongBool; cdecl;
      PluginVarInit:         function(MyVariation, FPx, FPy, FTx, FTy: Pointer; vvar: double): LongBool; cdecl;
      PluginVarInit3D:       function(MyVariation, FPx, FPy, FPz, FTx, FTy, FTz: Pointer; vvar: double): LongBool; cdecl;
      PluginVarInitDC:       function(MyVariation, FPx, FPy, FPz, FTx, FTy, FTz, color: Pointer; vvar, a, b, c, d, e, f: double): LongBool; cdecl;
      PluginVarInitDO:       function(MyVariation, FPx, FPy, FPz, FTx, FTy, FTz, color, opacity: Pointer; vvar, a, b, c, d, e, f: double): LongBool; cdecl;
      PluginVarPrepare:      function(MyVariation: Pointer): LongBool; cdecl;
      PluginVarCalc:         function(MyVariation: Pointer): LongBool; cdecl;
      PluginVarGetVariable:  function(MyVariation: Pointer; const Name: PAnsiChar; var value: double): LongBool; cdecl;
      PluginVarSetVariable:  function(MyVariation: Pointer; const Name: PAnsiChar; var value: double): LongBool; cdecl;
      PluginVarResetVariable:function(MyVariation: Pointer; const Name: PAnsiChar) : LongBool; cdecl;
    end;
    PPluginData = ^TPluginData;

  TPluginVariation = class(TVariation)

    private

      mData : TPluginData;
      mPtr : Pointer;

    public

      constructor Create(varData : TPluginData);
      destructor Destroy; override;

      class function GetName: string; override;
      class function GetInstance: TVariation; override;

      function GetNrVariables: integer; override;
      function GetVariableNameAt(const Index: integer): string; override;

      function SetVariable(const Name: string; var value: double): boolean; override;
      function GetVariable(const Name: string; var value: double): boolean; override;
      function ResetVariable(const Name: string): boolean; override;

      procedure Prepare; override;
      procedure CalcFunction; override;

    end;

type TVariationPluginLoader = class (TVariationLoader)

  public

    constructor Create(varData : TPluginData);
    destructor Destroy; override;
    
    function GetName: string; override;
    function GetInstance: TVariation; override;
    function GetNrVariables: integer; override;
    function GetVariableNameAt(const Index: integer): string; override;

    function Supports3D: boolean; override;
    function SupportsDC: boolean; override;

  private

    mData : TPluginData;

  end;

procedure InitializePlugins;

implementation uses
  Windows,
  Global,
  VariationPoolManager,
  Settings,
  SysUtils,
  Forms,
  Classes;

constructor TVariationPluginLoader.Create(varData : TPluginData);
begin
  mData := varData;
end;

function TVariationPluginLoader.Supports3D;
begin
  with mData do Result := @PluginVarInit3D <> nil;
end;

function TVariationPluginLoader.SupportsDC;
begin
  with mData do Result := @PluginVarInitDC <> nil;
end;

destructor TVariationPluginLoader.Destroy;
begin
  FreeLibrary(mData.PluginHandle);
end;

function TVariationPluginLoader.GetName : string;
begin
  Result := String(mData.PluginVarGetName);
end;

function TVariationPluginLoader.GetInstance: TVariation;
begin
  Result := TPluginVariation.Create(mData);
end;

function TVariationPluginLoader.GetNrVariables: integer;
begin
  Result := mData.PluginVarGetNrVariables;
end;

function TVariationPluginLoader.GetVariableNameAt(const Index: integer): string;
begin
  Result := String(mData.PluginVarGetVariableNameAt(Index));
end;

procedure TPluginVariation.Prepare;
begin
  with mData do begin
    if @PluginVarInitDO <> nil then
      PluginVarInitDO(mPtr, Pointer(FPX), Pointer(FPy), Pointer(FPz), Pointer(FTx), Pointer(FTy), Pointer(FTz), Pointer(color), Pointer(opacity), vvar, a, b, c, d, e, f)
    else if @PluginVarInitDC <> nil then
      PluginVarInitDC(mPtr, Pointer(FPX), Pointer(FPy), Pointer(FPz), Pointer(FTx), Pointer(FTy), Pointer(FTz), Pointer(color), vvar, a, b, c, d, e, f)
    else if @PluginVarInit3D <> nil then
      PluginVarInit3D(mPtr, Pointer(FPX), Pointer(FPy), Pointer(FPz), Pointer(FTx), Pointer(FTy), Pointer(FTz), vvar)
    else
      PluginVarInit(mPtr, Pointer(FPX), Pointer(FPy), Pointer(FTx), Pointer(FTy), vvar);
    PluginVarPrepare(mPtr);
  end;
end;

procedure TPluginVariation.CalcFunction;
begin
  mData.PluginVarCalc(mPtr);
end;

constructor TPluginVariation.Create(varData : TPluginData);
begin
  mData := varData;
  mPtr := mData.PluginVarCreate;
end;

destructor TPluginVariation.Destroy;
begin
  mData.PluginVarDestroy(mPtr);
  inherited;
end;

class function TPluginVariation.GetInstance: TVariation;
begin
  Result := nil;
end;

class function TPluginVariation.GetName: string;
begin
  Result := '';
end;

function TPluginVariation.GetNrVariables: integer;
begin
  Result := mData.PluginVarGetNrVariables;
end;

function TPluginVariation.GetVariableNameAt(const Index: integer): string;
begin
  Result := String(mData.PluginVarGetVariableNameAt(Index));
end;

function TPluginVariation.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := mData.PluginVarSetVariable(mPtr,PAnsiChar(AnsiString(Name)),value);
end;

function TPluginVariation.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := mData.PluginVarGetVariable(mPtr,PAnsiChar(AnsiString(Name)),value);
end;

function TPluginVariation.ResetVariable(const Name: string) : boolean;
var
  dummy: double;
begin
  if @mData.PluginVarResetVariable <> nil then
    Result := mData.PluginVarResetVariable(mPtr, PAnsiChar(AnsiString(Name)))
  else begin
    dummy := 0;
    Result := mData.PluginVarSetVariable(mPtr,PAnsiChar(AnsiString(Name)), dummy);
  end;
end;

function GetPlatformOf(dllPath: string): integer;
var
  fs: TFilestream;
  signature: DWORD;
  dos_header: IMAGE_DOS_HEADER;
  pe_header: IMAGE_FILE_HEADER;
  opt_header: IMAGE_OPTIONAL_HEADER;
begin
  fs := TFilestream.Create(dllPath, fmOpenread or fmShareDenyNone);
  try
    fs.read(dos_header, SizeOf(dos_header));
    if dos_header.e_magic <> IMAGE_DOS_SIGNATURE then
    begin
      Result := 0;
      Exit;
    end;

    fs.seek(dos_header._lfanew, soFromBeginning);
    fs.read(signature, SizeOf(signature));
    if signature <> IMAGE_NT_SIGNATURE then
    begin
      Result := 0;
      Exit;
    end;

    fs.read(pe_header, SizeOf(pe_header));
    case pe_header.Machine of
      IMAGE_FILE_MACHINE_I386: Result := $00000020;
      IMAGE_FILE_MACHINE_AMD64: Result := $00000040;
    else
      Result := 0;
  end; { Case }

  finally
    fs.Free;
  end;
end;

procedure InitializePlugins;
var
  searchResult: TSearchRec;
  dllPath, name, msg: string;
  PluginData : TPluginData;
  errno:integer;
  errstr:string;
begin
  PluginPath := ReadPluginDir;

  // Try to find regular files matching *.dll in the plugins dir
  if FindFirst(PluginPath + '*.dll', faAnyFile, searchResult) = 0 then
  begin
    repeat
      with PluginData do begin
        dllPath := PluginPath + searchResult.Name;

        //Check plugin platform
         if PLUGIN_PLATFORM <> GetPlatformOf(dllPath)
         then continue;

        //Load DLL and initialize plugins!
        PluginHandle := LoadLibrary(PChar(dllPath));
        if PluginHandle<>0 then begin
          @PluginVarGetName := GetProcAddress(PluginHandle,'PluginVarGetName');
          if @PluginVarGetName = nil then begin  // Must not be a valid plugin!
            FreeLibrary(PluginHandle);
            msg := msg + 'Invalid plugin type: "' + searchResult.Name + '" is not a plugin' + #13#10;
            continue;
          end;
          name := String(PluginVarGetName);
          if GetVariationIndexByName(name) >= 0 then begin
            FreeLibrary(PluginHandle);
            msg := msg + 'Cannot load plugin from ' + searchResult.Name + ': variation "' + name + '" already exists!' + #13#10;
          end
          else begin
            @PluginVarGetNrVariables    := GetProcAddress(PluginHandle,'PluginVarGetNrVariables');
            @PluginVarGetVariableNameAt := GetProcAddress(PluginHandle,'PluginVarGetVariableNameAt');
            @PluginVarCreate            := GetProcAddress(PluginHandle,'PluginVarCreate');
            @PluginVarDestroy           := GetProcAddress(PluginHandle,'PluginVarDestroy');
            @PluginVarInit              := GetProcAddress(PluginHandle,'PluginVarInit');
            @PluginVarInit3D            := GetProcAddress(PluginHandle,'PluginVarInit3D');
            @PluginVarInitDC            := GetProcAddress(PluginHandle,'PluginVarInitDC');
            @PluginVarInitDO            := GetProcAddress(PluginHandle,'PluginVarInitDO');
            @PluginVarPrepare           := GetProcAddress(PluginHandle,'PluginVarPrepare');
            @PluginVarCalc              := GetProcAddress(PluginHandle,'PluginVarCalc');
            @PluginVarGetVariable       := GetProcAddress(PluginHandle,'PluginVarGetVariable');
            @PluginVarSetVariable       := GetProcAddress(PluginHandle,'PluginVarSetVariable');
            @PluginVarResetVariable     := GetProcAddress(PluginHandle,'PluginVarResetVariable');

            RegisterVariation(TVariationPluginLoader.Create(PluginData));
          end;
        end else begin
          errno := GetLastError;
          errstr := SysErrorMessage(errno);
          msg := msg + 'Cannot open plugin file: ' + searchResult.Name + ' (Error #' + IntToStr(GetLastError) + ' - ' + errstr + ')' + #13#10;
        end;
      end;
    until (FindNext(searchResult) <> 0);

    SysUtils.FindClose(searchResult);

    if msg <> '' then
      Application.MessageBox(
        PChar('There were problems with some of the plugins:' + #13#10#13#10 + msg),
        'Warning', MB_ICONWARNING or MB_OK);
  end;
end;

end.

