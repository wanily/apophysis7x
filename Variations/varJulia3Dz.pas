unit varJulia3Dz;

interface

uses
  Variation, VariationPoolManager;

const
  var_name = 'julia3Dz';
  var_n_name='julia3Dz_power';

type
  TVariationJulia3D = class(TVariation)
  private
    N: integer;

    absN: integer;
    cN: double;

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
  Math;

// TVariationJulia3D

///////////////////////////////////////////////////////////////////////////////
constructor TVariationJulia3D.Create;
begin
  N := random(5) + 2;
  if random(2) = 0 then N := -N;
end;

procedure TVariationJulia3D.Prepare;
begin
  absN := abs(N);
  cN := 1 / N / 2;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TVariationJulia3D.ObtainCalculateFunctionPtr(out f: TCalcFunction);
begin
  if N = 2 then f := CalcPower2
  else if N = -2 then f := CalcPowerMinus2
  else if N = 1 then f := CalcPower1
  else if N = -1 then f := CalcPowerMinus1
  else f := CalcFunction;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TVariationJulia3D.CalcFunction;
var
  r, r2d: double;
  sina, cosa: extended;
begin
  r2d := sqr(FTx^) + sqr(FTy^);
  r := vvar * Math.Power(r2d, cN);

  FPz^ := FPz^ + r * FTz^ / (sqrt(r2d) * absN);

  sincos((arctan2(FTy^, FTx^) + 2*pi*random(absN)) / N, sina, cosa);
  FPx^ := FPx^ + r * cosa;
  FPy^ := FPy^ + r * sina;
end;

procedure TVariationJulia3D.CalcPower2;
var
  r, r2d: double;
  sina, cosa: extended;
begin
  r2d := sqrt(sqr(FTx^) + sqr(FTy^));
  r := vvar * sqrt(r2d);

  FPz^ := FPz^ + r * FTz^ / r2d / 2;

  sincos((arctan2(FTy^, FTx^)/2 + pi*random(2)), sina, cosa);
  FPx^ := FPx^ + r * cosa;
  FPy^ := FPy^ + r * sina;
end;

procedure TVariationJulia3D.CalcPowerMinus2;
var
  r, r2d: double;
  sina, cosa: extended;
begin
  r2d := sqrt(sqr(FTx^) + sqr(FTy^));
  r := vvar / sqrt(r2d);

  FPz^ := FPz^ + r * FTz^ / r2d / 2;

  sincos(pi*random(2) - arctan2(FTy^, FTx^)/2, sina, cosa);
  FPx^ := FPx^ + r * cosa;
  FPy^ := FPy^ + r * sina;
end;

procedure TVariationJulia3D.CalcPower1;
begin
  FPx^ := FPx^ + vvar * FTx^;
  FPy^ := FPy^ + vvar * FTy^;
  FPz^ := FPz^ + vvar * FTz^;
end;

procedure TVariationJulia3D.CalcPowerMinus1;
var
  r: double;
begin
  r := vvar / (sqr(FTx^) + sqr(FTy^));

  FPx^ := FPx^ + r * FTx^;
  FPy^ := FPy^ - r * FTy^;
  FPz^ := FPz^ + r * FTz^;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationJulia3D.GetInstance: TVariation;
begin
  Result := TVariationJulia3D.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationJulia3D.GetName: string;
begin
  Result := var_name;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationJulia3D.GetVariableNameAt(const Index: integer): string;
begin
  case Index of
    0: Result := var_n_name;
  else
    Result := '';
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationJulia3D.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = var_n_name then begin
    N := Round(Value);
    if N = 0 then N := 1;
    Value := N;
    Result := True;
  end;
end;

function TVariationJulia3D.ResetVariable(const Name: string): boolean;
begin
  Result := False;
  if Name = var_n_name then begin
    if N = 2 then N := -2
    else N := 2;
    Result := True;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationJulia3D.GetNrVariables: integer;
begin
  Result := 1;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationJulia3D.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = var_n_name then begin
    Value := N;
    Result := true;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationJulia3D, true, false)) end.
