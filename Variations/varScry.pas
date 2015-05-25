{$Include 'delphiversion.pas'}
unit varScry;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationScry = class(TVariation)
  private
    v: double;
  public
    constructor Create;

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

implementation

uses
  Math;

///////////////////////////////////////////////////////////////////////////////
procedure TVariationScry.Prepare;
begin
  if (VVAR = 0) then
    v := 1.0 / 1e-6
  else v := 1.0 / vvar;
end;

procedure TVariationScry.CalcFunction;
var t, r : double;
begin
  t := sqr(FTx^) + sqr(FTy^);
	r := 1.0 / (sqrt(t) * (t + v));

	FPx^ := FPx^ + FTx^ * r;
	FPy^ := FPy^ + FTy^ * r;

{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
constructor TVariationScry.Create;
begin
  v := 0;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationScry.GetInstance: TVariation;
begin
  Result := TVariationScry.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationScry.GetName: string;
begin
  Result := 'scry';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationScry.GetVariableNameAt(const Index: integer): string;
begin
  Result := '';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationScry.SetVariable(const Name: string; var value: double): boolean;
var temp: double;
begin
  Result := False;
end;
function TVariationScry.ResetVariable(const Name: string): boolean;
begin
  Result := False;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationScry.GetNrVariables: integer;
begin
  Result := 0
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationScry.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationScry, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
