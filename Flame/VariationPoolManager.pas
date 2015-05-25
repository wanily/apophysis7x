{$INCLUDE 'delphiversion.pas'}

unit VariationPoolManager;

interface

uses
  Variation, SysUtils, Forms, Windows, StrUtils, CoreVariation;

type
  TVariationPriority = (vpPre = 0, vpRegular = 1, vpPost = 2, vpSuperior = 3);

procedure RegisterVariation(Variation: TVariationLoader);

function GetRegisteredVariation(const Index: integer): TVariationLoader;

function GetTotalVariableCount: integer;
function GetVariableNameByGlobalVariableIndex(const Index: integer): string;
function GetVariationIndexByVariableIndex(const Index: integer): integer;

function GetCoreVariationCount: integer;
function GetIntegratedNonCoreVariationCount: integer;
function GetTotalVariationCount: integer;
function GetVariationNameByIndex(const index: integer): string;
function GetVariationIndexByName(const str: string): integer;
function GetVariationSupportFlagsByIndex(Index: integer; var supports3D: boolean; var supportsDC: boolean): boolean;
function GetVariationPriorityByIndex(const index: integer): TVariationPriority;

procedure InitializeXFormMan;
procedure DestroyXFormMan;

implementation

uses
  Classes;

var
  mVariations: TList;
  mVariables: TStringlist;
  mVariableIndexToVariationIndexArray: array of integer;

function GetCoreVariationCount: integer;
begin
  Result := TCoreVariation.Count;
end;

procedure InitializeXFormMan;
begin
  mVariations := TList.Create;
  mVariables := TStringlist.Create;
  SetLength(mVariableIndexToVariationIndexArray, 0);
end;

function GetVariationPriorityByIndex(const index: integer): TVariationPriority;
// -x- this is actually making my hair fall out
var
  name: string;
begin

  name := GetVariationNameByIndex(index);

  if (Lowercase(name) = 'flatten') then
    Result := vpSuperior
  else if (StrUtils.StartsText('pre_', Lowercase(name))) then
    Result := vpPre
  else if (StrUtils.StartsText('post_', Lowercase(name))) then
    Result := vpPost
  else
    Result := vpRegular;

end;

function GetVariationSupportFlagsByIndex(Index: integer; var supports3D: boolean; var supportsDC: boolean) : boolean;
var
  varl: TVariationLoader;
begin

  if (index >= GetCoreVariationCount) then
  begin
    supports3D := TVariationLoader(mVariations.Items[index - GetCoreVariationCount]).supports3D;
    supportsDC := TVariationLoader(mVariations.Items[index - GetCoreVariationCount]).supportsDC;
  end
  else
  begin
    supports3D := TCoreVariation.GetInfoByIndex(index).supports3D;
    supportsDC := false;
  end;

  Result := true;
end;

procedure DestroyXFormMan;
var
  i: integer;
begin
  mVariables.Free;

  for i := 0 to mVariations.Count - 1 do
    TVariationLoader(mVariations[i]).Free;
  mVariations.Free;

  Finalize(mVariableIndexToVariationIndexArray);
end;

function GetTotalVariationCount: integer;
begin
  Result := GetCoreVariationCount + mVariations.Count;
end;

function GetVariationIndexByVariableIndex(const Index: integer): integer;
begin
  if (Index < 0) or (Index > High(mVariableIndexToVariationIndexArray)) then
    Result := -1
  else
    Result := mVariableIndexToVariationIndexArray[Index];
end;

function GetVariationNameByIndex(const Index: integer): String;
begin
  if Index < GetCoreVariationCount then
    Result := TCoreVariation.GetInfoByIndex(index).name
  else
    Result := TVariationLoader(mVariations[Index - GetCoreVariationCount]).GetName;
end;

function GetVariationIndexByName(const str: string): integer;
var
  i: integer;
begin
  i := GetTotalVariationCount - 1;
  while (i >= 0) and (GetVariationNameByIndex(i) <> str) do
    Dec(i);
  Result := i;
end;

procedure RegisterVariation(Variation: TVariationLoader);
var
  i: integer;
  prevNumVariables: integer;
begin
  mVariations.Add(Variation);

  prevNumVariables := GetTotalVariableCount;
  SetLength(mVariableIndexToVariationIndexArray, prevNumVariables + Variation.GetNrVariables);

  for i := 0 to Variation.GetNrVariables - 1 do
  begin
    mVariables.Add(Variation.GetVariableNameAt(i));
    mVariableIndexToVariationIndexArray[prevNumVariables + i] := GetTotalVariationCount - 1;
  end;
end;

function GetIntegratedNonCoreVariationCount: integer;
begin
  Result := mVariations.Count;
end;

function GetRegisteredVariation(const Index: integer): TVariationLoader;
begin
  Result := TVariationLoader(mVariations[Index]);
end;

function GetTotalVariableCount: integer;
begin
  Result := mVariables.Count;
end;

function GetVariableNameByGlobalVariableIndex(const Index: integer): string;
begin
  Result := mVariables[Index];
end;

initialization

InitializeXFormMan;

TCoreVariation.RegisterCoreVariation({$ifdef Pre15c}'linear3D', true{$else} 'linear',  true{$endif});
TCoreVariation.RegisterCoreVariation({$ifdef Pre15c}'linear',   false{$else}'flatten', true{$endif});

TCoreVariation.RegisterCoreVariation('sinusoidal',    {$ifdef Pre15c}false{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('spherical',     {$ifdef Pre15c}false{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('swirl',         {$ifdef Pre15c}false{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('horseshoe',     {$ifdef Pre15c}false{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('polar',         {$ifdef Pre15c}false{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('disc',          {$ifdef Pre15c}false{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('spiral',        {$ifdef Pre15c}false{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('hyperbolic',    {$ifdef Pre15c}false{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('diamond',       {$ifdef Pre15c}false{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('eyefish',       {$ifdef Pre15c}false{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('bubble',        {$ifdef Pre15c}true{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('cylinder',      {$ifdef Pre15c}true{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('noise',         {$ifdef Pre15c}false{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('blur',          {$ifdef Pre15c}false{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('gaussian_blur', {$ifdef Pre15c}false{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('zblur',         {$ifdef Pre15c}true{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('blur3D',        {$ifdef Pre15c}true{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('pre_blur',      {$ifdef Pre15c}true{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('pre_zscale',    {$ifdef Pre15c}true{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('pre_ztranslate',{$ifdef Pre15c}true{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('pre_rotate_x',  {$ifdef Pre15c}true{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('pre_rotate_y',  {$ifdef Pre15c}true{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('zscale',        {$ifdef Pre15c}true{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('ztranslate',    {$ifdef Pre15c}true{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('zcone',         {$ifdef Pre15c}true{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('post_rotate_x', {$ifdef Pre15c}true{$else}true{$endif});
TCoreVariation.RegisterCoreVariation('post_rotate_y', {$ifdef Pre15c}true{$else}true{$endif});

finalization

DestroyXFormMan;

end.
