{$Include 'delphiversion.pas'}

unit varEscher;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationEscher = class(TVariation)
  private
    escher_beta, c, d: double;
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
procedure TVariationEscher.Prepare;
begin
  sincos(escher_beta, d, c);
  c := 0.5 * (1.0 + c);
  d := 0.5 * d;
end;

procedure TVariationEscher.CalcFunction;
var sn, cs, a, lnr, m : double;
begin
  a := arctan2(FTy^, FTx^); // Angular polar dimension
  lnr := 0.5 * ln(FTx^*FTx^ + FTy^*FTy^); // Natural logarithm of the radial polar dimension.

  m := VVAR * exp(c * lnr - d * a);

    sincos(c * a + d * lnr, sn, cs);

    FPx^ := FPx^ + m * cs;
    FPy^ := FPy^ + m * sn;

  {$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
constructor TVariationEscher.Create;
begin
  escher_beta := 0;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationEscher.GetInstance: TVariation;
begin
  Result := TVariationEscher.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationEscher.GetName: string;
begin
  Result := 'escher';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationEscher.GetVariableNameAt(const Index: integer): string;
begin
  case Index Of
  0: Result := 'escher_beta';
  else
    Result := '';
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationEscher.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'escher_beta' then begin
    value := frac((value + PI) / (2 * PI)) * 2 * PI - PI;
    escher_beta := Value;
    Result := True;
  end 
end;
function TVariationEscher.ResetVariable(const Name: string): boolean;
begin
  Result := False;
  if Name = 'escher_beta' then begin
    escher_beta := 0;
    Result := True;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationEscher.GetNrVariables: integer;
begin
  Result := 1
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationEscher.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'escher_beta' then begin
    Value := escher_beta;
    Result := True;
  end
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationEscher, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
