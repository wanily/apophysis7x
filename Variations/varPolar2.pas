{$Include 'delphiversion.pas'}

unit varPolar2;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationPolar2 = class(TVariation)
  private
    p2vv, p2vv2: double;
  public
    constructor Create;

    class function GetName: string; override;
    class function GetInstance: TVariation; override;

    function GetNrVariables: integer; override;
    function GetVariableNameAt(const Index: integer): string; override;

    function SetVariable(const Name: string; var value: double): boolean; override;
    function GetVariable(const Name: string; var value: double): boolean; override;

	  procedure Prepare; override;
    procedure CalcFunction; override;
  end;

implementation

uses
  Math;

///////////////////////////////////////////////////////////////////////////////
procedure TVariationPolar2.Prepare;
begin
  p2vv := VVAR / PI;
	p2vv2 := p2vv * 0.5;
end;

procedure TVariationPolar2.CalcFunction;
begin
  FPy^ := FPy^ + p2vv2 * Ln(sqr(FTx^) + sqr(FTy^));
  FPx^ := FPx^ + p2vv * ArcTan2(FTx^, FTy^);
{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
constructor TVariationPolar2.Create;
begin
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationPolar2.GetInstance: TVariation;
begin
  Result := TVariationPolar2.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationPolar2.GetName: string;
begin
  Result := 'polar2';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPolar2.GetVariableNameAt(const Index: integer): string;
begin
  Result := '';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPolar2.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPolar2.GetNrVariables: integer;
begin
  Result := 0
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPolar2.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationPolar2, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
