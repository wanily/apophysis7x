{$INCLUDE 'delphiversion.pas'}
unit XFormMan;

interface

uses
  Variation, SysUtils, Forms, Windows, StrUtils;

const
  NRLOCVAR = 29;

var
  NumBuiltinVars: integer;

type
  TFNToVN = record
    FileName: string;
    VarName: string;
  end;

  TVarProps = class
  public
    Index: integer;
    VarName: string;
    Priority: integer;
    Order: integer;
  end;

  TVarPropsList = array of TVarProps;

function NrVar: integer;
function Varnames(const Index: integer): String;
procedure RegisterVariation(Variation: TVariationLoader;
  supports3D, supportsDC: boolean);
function GetNrRegisteredVariations: integer;
function GetRegisteredVariation(const Index: integer): TVariationLoader;
function GetNrVariableNames: integer;
function GetVariableNameAt(const Index: integer): string;
function GetVariationIndex(const str: string): integer;
function GetVariationIndexFromVariableNameIndex(const Index: integer): integer;
procedure VarSupports(Index: integer; var supports3D: boolean;
  var supportsDC: boolean);
procedure InitializeXFormMan;
procedure DestroyXFormMan;
procedure RegisterVariationFile(FileName, name: string);
function GetFileNameOfVariation(name: string): string;
function GetVarProps: TVarPropsList;

implementation

uses
  Classes;

var
  VariationList: TList;
  VariableNames: TStringlist;
  loaderNum: integer;
  Variable2VariationIndex: array of integer;
  FNToVNList: array of TFNToVN;
  FNToVNCount: integer;

procedure InitializeXFormMan;
begin
  VariationList := TList.Create;
  VariableNames := TStringlist.Create;
  SetLength(Variable2VariationIndex, 0);
  SetLength(FNToVNList, 0);
  FNToVNCount := 0;
end;

procedure VarSupports(Index: integer; var supports3D: boolean;
  var supportsDC: boolean);
const
  supports3D_arr: array [0 .. NRLOCVAR - 1] of boolean = (
{$IFDEF Pre15c}
    true, // 'linear3D',
    false, // 'linear',
    false, // 'sinusoidal',
    false, // 'spherical',
    false, // 'swirl',
    false, // 'horseshoe',
    false, // 'polar',
    false, // 'disc',
    false, // 'spiral',
    false, // 'hyperbolic',
    false, // 'diamond',
    false, // 'eyefish',
    true, // 'bubble',
    true, // 'cylinder',
    false, // 'noise',
    false, // 'blur',
    false, // 'gaussian_blur',
    true, // 'zblur',
    true, // 'blur3D',
    true, // 'pre_blur',
    true, // 'pre_zscale',
    true, // 'pre_ztranslate',
    true, // 'pre_rotate_x',
    true, // 'pre_rotate_y',
    true, // 'zscale',
    true, // 'ztranslate',
    true, // 'zcone',
    true, // 'post_rotate_x',
    true // 'post_rotate_y',
{$ELSE}
    true, // 'linear',
    true, // 'flatten',
    true, // 'sinusoidal',
    true, // 'spherical',
    true, // 'swirl',
    true, // 'horseshoe',
    true, // 'polar',
    true, // 'disc',
    true, // 'spiral',
    true, // 'hyperbolic',
    true, // 'diamond',
    true, // 'eyefish',
    true, // 'bubble',
    true, // 'cylinder',
    true, // 'noise',
    true, // 'blur',
    true, // 'gaussian_blur',
    true, // 'zblur',
    true, // 'blur3D',
    true, // 'pre_blur',
    true, // 'pre_zscale',
    true, // 'pre_ztranslate',
    true, // 'pre_rotate_x',
    true, // 'pre_rotate_y',
    true, // 'zscale',
    true, // 'ztranslate',
    true, // 'zcone',
    true, // 'post_rotate_x',
    true // 'post_rotate_y',
{$ENDIF}
    );
  supportsDC_arr: array [0 .. NRLOCVAR - 1] of boolean = (false,
    // 'linear3D/linear',
    false, // 'linear/flatten',
    false, // 'sinusoidal',
    false, // 'spherical',
    false, // 'swirl',
    false, // 'horseshoe',
    false, // 'polar',
    false, // 'disc',
    false, // 'spiral',
    false, // 'hyperbolic',
    false, // 'diamond',
    false, // 'eyefish',
    false, // 'bubble',
    false, // 'cylinder',
    false, // 'noise',
    false, // 'blur',
    false, // 'gaussian_blur',
    false, // 'zblur',
    false, // 'blur3D',

    false, // 'pre_blur',
    false, // 'pre_zscale',
    false, // 'pre_ztranslate',
    false, // 'pre_rotate_x',
    false, // 'pre_rotate_y',

    false, // 'zscale',
    false, // 'ztranslate',
    false, // 'zcone',

    false, // 'post_rotate_x',
    false // 'post_rotate_y'
    );
var
  varl: TVariationLoader;
begin

  if (index >= NRLOCVAR) then
  begin
    supports3D := TVariationLoader(VariationList.Items[index - NRLOCVAR])
      .supports3D;
    supportsDC := TVariationLoader(VariationList.Items[index - NRLOCVAR])
      .supportsDC;
  end
  else
  begin
    supports3D := supports3D_arr[index];
    supportsDC := supportsDC_arr[index];
  end;
end;

procedure DestroyXFormMan;
var
  i: integer;
begin
  VariableNames.Free;

  for i := 0 to VariationList.Count - 1 do
    TVariationLoader(VariationList[i]).Free;
  VariationList.Free;

  Finalize(Variable2VariationIndex);
  Finalize(FNToVNList);
end;

function NrVar: integer;
begin
  Result := NRLOCVAR + VariationList.Count;
end;

function GetVariationIndexFromVariableNameIndex(const Index: integer): integer;
begin
  if (Index < 0) or (Index > High(Variable2VariationIndex)) then
    Result := -1
  else
    Result := Variable2VariationIndex[Index];
end;

function Varnames(const Index: integer): String;
const
  cvarnames: array [0 .. NRLOCVAR - 1] of string = (
{$IFDEF Pre15c}
    'linear3D', 'linear',
{$ELSE}
    'linear', 'flatten',
{$ENDIF}
    'sinusoidal', 'spherical', 'swirl', 'horseshoe', 'polar', 'disc', 'spiral',
    'hyperbolic', 'diamond', 'eyefish', 'bubble', 'cylinder', 'noise', 'blur',
    'gaussian_blur', 'zblur', 'blur3D', 'pre_blur', 'pre_zscale',
    'pre_ztranslate', 'pre_rotate_x', 'pre_rotate_y', 'zscale', 'ztranslate',
    'zcone', 'post_rotate_x', 'post_rotate_y');
begin
  if Index < NRLOCVAR then
    Result := cvarnames[Index]
  else
    Result := TVariationLoader(VariationList[Index - NRLOCVAR]).GetName;
end;

function GetVarProps: TVarPropsList;
var
  i: integer;
  r: TVarPropsList;
  x: TVarProps;
begin
  SetLength(r, NrVar);

  for i := 0 to NrVar - 1 do
  begin
    x := TVarProps.Create;
    x.Index := i;
    x.VarName := Varnames(i);
    x.Priority := 1;
    x.Order := i + 1;

    if LeftStr(x.VarName, 4) = 'pre_' then
      x.Priority := 0;

    if LeftStr(x.VarName, 4) = 'post_' then
      x.Priority := 2;

    if x.VarName = 'flatten' then
      x.Priority := 3;

    r[i] := x;
  end;

  Result := r;
end;

function GetVariationIndex(const str: string): integer;
var
  i: integer;
begin
  i := NrVar - 1;
  while (i >= 0) and (Varnames(i) <> str) do
    Dec(i);
  Result := i;
end;

procedure RegisterVariationFile(FileName, name: string);
begin
  FNToVNCount := FNToVNCount + 1;
  SetLength(FNToVNList, FNToVNCount);
  FNToVNList[FNToVNCount - 1].FileName := FileName;
  FNToVNList[FNToVNCount - 1].VarName := name;
end;

function GetFileNameOfVariation(name: string): string;
var
  i: integer;
begin
  for i := 0 to FNToVNCount - 1 do
  begin
    if FNToVNList[i].VarName = name then
    begin
      Result := FNToVNList[i].FileName;
      Exit;
    end;
  end;
  Result := '';
end;

procedure RegisterVariation(Variation: TVariationLoader;
  supports3D, supportsDC: boolean);
var
  i: integer;
  prevNumVariables: integer;
begin
  OutputDebugString(PChar(Variation.GetName));

  VariationList.Add(Variation);
  Variation.supports3D := supports3D;
  Variation.supportsDC := supportsDC;

  prevNumVariables := GetNrVariableNames;
  SetLength(Variable2VariationIndex, prevNumVariables +
    Variation.GetNrVariables);
  for i := 0 to Variation.GetNrVariables - 1 do
  begin
    VariableNames.Add(Variation.GetVariableNameAt(i));
    Variable2VariationIndex[prevNumVariables + i] := NrVar - 1;
  end;
end;

function GetNrRegisteredVariations: integer;
begin
  Result := VariationList.Count;
end;

function GetRegisteredVariation(const Index: integer): TVariationLoader;
begin
  Result := TVariationLoader(VariationList[Index]);
end;

function GetNrVariableNames: integer;
begin
  Result := VariableNames.Count;
end;

function GetVariableNameAt(const Index: integer): string;
begin
  Result := VariableNames[Index];
end;

initialization

InitializeXFormMan;

finalization

DestroyXFormMan;

end.
