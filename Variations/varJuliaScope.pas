{$Include 'delphiversion.pas'}

unit varJuliaScope;

interface

uses
  Variation, VariationPoolManager;

const
  variation_name='juliascope';
  var_n_name='juliascope_power';
  var_c_name='juliascope_dist';

type
  TVariationJuliaScope = class(TVariation)
  private
    N: integer;
    c: double;

    rN: integer;
    cn: double;

    procedure CalcPower1;
    procedure CalcPowerMinus1;
    procedure CalcPower2;
    procedure CalcPowerMinus2;

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
    procedure ObtainCalculateFunctionPtr(out f: TCalcFunction); override;
  end;

implementation

uses
  math;

// TVariationJuliaScope

///////////////////////////////////////////////////////////////////////////////
constructor TVariationJuliaScope.Create;
begin
  N := random(5) + 2;
  c := 1.0;
end;

procedure TVariationJuliaScope.Prepare;
begin
  rN := abs(N);
  cn := c / N / 2;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TVariationJuliaScope.ObtainCalculateFunctionPtr(out f: TCalcFunction);
begin
  if c = 1 then begin
    if N = 2 then f := CalcPower2
    else if N = -2 then f := CalcPowerMinus2
    else if N = 1 then f := CalcPower1
    else if N = -1 then f := CalcPowerMinus1
    else f := CalcFunction;
  end
  else f := CalcFunction;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TVariationJuliaScope.CalcFunction;
var
  rnd: integer;
  r: double;
  sina, cosa: extended;
begin
  rnd := random(rN);
  if (rnd and 1) = 0 then
    sincos( (2*pi*rnd + arctan2(FTy^, FTx^)) / N, sina, cosa)
  else
    sincos( (2*pi*rnd - arctan2(FTy^, FTx^)) / N, sina, cosa);
  r := vvar * Math.Power(sqr(FTx^) + sqr(FTy^), cn);
  FPx^ := FPx^ + r * cosa;
  FPy^ := FPy^ + r * sina;
{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

procedure TVariationJuliaScope.CalcPower2;
var
  r: double;
  sina, cosa: extended;
begin
  if random(2) = 0 then
    sincos(arctan2(FTy^, FTx^)/2, sina, cosa)
  else
    sincos(pi - arctan2(FTy^, FTx^)/2, sina, cosa);

  r := vvar * sqrt(sqrt(sqr(FTx^) + sqr(FTy^)));

  FPx^ := FPx^ + r * cosa;
  FPy^ := FPy^ + r * sina;
{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

procedure TVariationJuliaScope.CalcPowerMinus2;
var
  r: double;
  sina, cosa: extended;
begin
  if random(2) = 0 then
    sincos(arctan2(FTy^, FTx^)/2, sina, cosa)
  else
    sincos(pi - arctan2(FTy^, FTx^)/2, sina, cosa);
  r := vvar / sqrt(sqrt(sqr(FTx^) + sqr(FTy^)));

  FPx^ := FPx^ + r * cosa;
  FPy^ := FPy^ - r * sina;
{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

procedure TVariationJuliaScope.CalcPower1;
begin
  FPx^ := FPx^ + vvar * FTx^;
  FPy^ := FPy^ + vvar * FTy^;
{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

procedure TVariationJuliaScope.CalcPowerMinus1;
var
  r: double;
begin
  r := vvar / (sqr(FTx^) + sqr(FTy^));

  FPx^ := FPx^ + r * FTx^;
  FPy^ := FPy^ - r * FTy^;
{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationJuliaScope.GetInstance: TVariation;
begin
  Result := TVariationJuliaScope.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationJuliaScope.GetName: string;
begin
  Result := variation_name;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationJuliaScope.GetVariableNameAt(const Index: integer): string;
begin
  case Index of
  0: Result := var_n_name;
  1: Result := var_c_name;
  else
    Result := '';
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationJuliaScope.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = var_n_name then begin
    N := Round(Value);
    if N = 0 then N := 1;
    Value := N;
    Result := True;
  end
  else if Name = var_c_name then begin
    c := value;
    Result := True;
  end;
end;

function TVariationJuliaScope.ResetVariable(const Name: string): boolean;
begin
  Result := False;
  if Name = var_n_name then begin
    if N = 2 then N := -2
    else N := 2;
    Result := True;
  end
  else if Name = var_c_name then begin
    c := 1;
    Result := True;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationJuliaScope.GetNrVariables: integer;
begin
  Result := 2;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationJuliaScope.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = var_n_name then begin
    Value := N;
    Result := true;
  end
  else if Name = var_c_name then begin
    Value := c;
    Result := true;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationJuliaScope, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
