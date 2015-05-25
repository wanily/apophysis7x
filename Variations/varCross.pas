{$Include 'delphiversion.pas'}

unit varCross;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationCross = class(TVariation)
  public
    constructor Create;

    class function GetName: string; override;
    class function GetInstance: TVariation; override;

    function GetNrVariables: integer; override;
    function GetVariableNameAt(const Index: integer): string; override;

    function SetVariable(const Name: string; var value: double): boolean; override;
    function GetVariable(const Name: string; var value: double): boolean; override;

    procedure CalcFunction; override;
  end;

implementation

uses
  Math;

///////////////////////////////////////////////////////////////////////////////
procedure TVariationCross.CalcFunction;
var
  r: double;
begin
  r := Abs((FTx^ - FTy^) * (FTx^ + FTy^) + 1e-6);
  if (r < 0) then r := r * -1.0;
  r := VVAR / r;

  FPx^ := FPx^ + FTx^ * r;
  FPy^ := FPy^ + FTy^ * r;
{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
constructor TVariationCross.Create;
begin
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationCross.GetInstance: TVariation;
begin
  Result := TVariationCross.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationCross.GetName: string;
begin
  Result := 'cross';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationCross.GetVariableNameAt(const Index: integer): string;
begin
  Result := '';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationCross.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationCross.GetNrVariables: integer;
begin
  Result := 0
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationCross.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationCross, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
