{$Include 'delphiversion.pas'}

unit varElliptic;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationElliptic = class(TVariation)
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


    procedure CalcFunction; override;
	procedure Prepare; override;
  end;

implementation

uses
  Math;


///////////////////////////////////////////////////////////////////////////////
procedure TVariationElliptic.Prepare;
begin
	v := VVAR / (PI / 2.0)
end;
procedure TVariationElliptic.CalcFunction;
function sqrt_safe(x: double): double;
	begin
		if x < 0.0 then Result := 0.0
		else Result := sqrt(x);
	end;
var
  a, b, tmp, x2, xmax: double;
begin
  tmp := sqr(FTy^) + sqr(FTx^) + 1.0;
	x2 := 2.0 * FTx^;
	xmax := 0.5 * (sqrt(tmp + x2) + sqrt(tmp - x2));

	a := FTx^ / xmax;
	b := sqrt_safe(1.0 - sqr(a));

  {$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
  FPx^ := FPx^ + v * ArcTan2(a, b);
  
  if (FTy^ > 0) then FPy^ := FPy^ + v * Ln(xmax + sqrt_safe(xmax - 1.0))
  else FPy^ := FPy^ - v * Ln(xmax + sqrt_safe(xmax - 1.0))
end;

///////////////////////////////////////////////////////////////////////////////
constructor TVariationElliptic.Create;
begin
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationElliptic.GetInstance: TVariation;
begin
  Result := TVariationElliptic.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationElliptic.GetName: string;
begin
  Result := 'elliptic';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationElliptic.GetVariableNameAt(const Index: integer): string;
begin
  Result := '';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationElliptic.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationElliptic.GetNrVariables: integer;
begin
  Result := 0
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationElliptic.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationElliptic, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
