unit varJulia3Djf; // original variation code by Joel Faber, modified & optimized by Peter Sdobnov

interface

uses
  Variation, VariationPoolManager;

const
  var_name = 'julia3D';
  var_n_name='julia3D_power';

type
  TVariationJulia3DJF = class(TVariation)
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

// TVariationJulia3DJF

///////////////////////////////////////////////////////////////////////////////
constructor TVariationJulia3DJF.Create;
begin
  N := random(5) + 2;
  if random(2) = 0 then N := -N;
end;

procedure TVariationJulia3DJF.Prepare;
begin
  absN := abs(N);
  cN := (1/N - 1) / 2;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TVariationJulia3DJF.ObtainCalculateFunctionPtr(out f: TCalcFunction);
begin
  if N = 2 then f := CalcPower2
  else if N = -2 then f := CalcPowerMinus2
  else if N = 1 then f := CalcPower1
  else if N = -1 then f := CalcPowerMinus1
  else f := CalcFunction;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TVariationJulia3DJF.CalcFunction;
var
  r, r2d, z, tmp: double;
  sina, cosa: extended;
begin
  z := FTz^ / absN;
  r2d := sqr(FTx^) + sqr(FTy^);
  r := vvar * Math.Power(r2d + sqr(z), cN); // r^n / sqrt(r)  -->  r^(n-0.5)

  FPz^ := FPz^ + r * z;

  tmp := r * sqrt(r2d);
  sincos((arctan2(FTy^, FTx^) + 2*pi*random(absN)) / N, sina, cosa);

  FPx^ := FPx^ + tmp * cosa;
  FPy^ := FPy^ + tmp * sina;
end;

procedure TVariationJulia3DJF.CalcPower2;
var
  r, r2d, z, tmp: double;
  sina, cosa: extended;
begin
  z := FTz^ / 2;
  r2d := sqr(FTx^) + sqr(FTy^);
  r := vvar / sqrt(sqrt(r2d + sqr(z))); // vvar * sqrt(r3d) / r3d  -->  vvar / sqrt(r3d)

  FPz^ := FPz^ + r * z;

  tmp := r * sqrt(r2d);
  sincos(arctan2(FTy^, FTx^) / 2 + pi*random(2), sina, cosa);

  FPx^ := FPx^ + tmp * cosa;
  FPy^ := FPy^ + tmp * sina;
end;

procedure TVariationJulia3DJF.CalcPowerMinus2;
var
  r, r2d, r3d, z, tmp: double;
  sina, cosa: extended;
begin
  z := FTz^ / 2;
  r2d := sqr(FTx^) + sqr(FTy^);
  r3d := sqrt(r2d + sqr(z));
  r := vvar / (sqrt(r3d) * r3d);

  FPz^ := FPz^ + r * z;

  tmp := r * sqrt(r2d);
  sincos(arctan2(FTy^, FTx^) / 2 + pi*random(2), sina, cosa);

  FPx^ := FPx^ + tmp * cosa;
  FPy^ := FPy^ - tmp * sina;
end;

procedure TVariationJulia3DJF.CalcPower1;
begin
  FPx^ := FPx^ + vvar * FTx^;
  FPy^ := FPy^ + vvar * FTy^;
  FPz^ := FPz^ + vvar * FTz^;
end;

procedure TVariationJulia3DJF.CalcPowerMinus1;
var
  r: double;
begin
  r := vvar / (sqr(FTx^) + sqr(FTy^) + sqr(FTz^));

  FPx^ := FPx^ + r * FTx^;
  FPy^ := FPy^ - r * FTy^;
  FPz^ := FPz^ + r * FTz^;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationJulia3DJF.GetInstance: TVariation;
begin
  Result := TVariationJulia3DJF.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationJulia3DJF.GetName: string;
begin
  Result := var_name;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationJulia3DJF.GetVariableNameAt(const Index: integer): string;
begin
  case Index of
    0: Result := var_n_name;
  else
    Result := '';
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationJulia3DJF.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = var_n_name then begin
    N := Round(Value);
    if N = 0 then N := 1;
    Value := N;
    Result := True;
  end;
end;

function TVariationJulia3DJF.ResetVariable(const Name: string): boolean;
begin
  Result := False;
  if Name = var_n_name then begin
    if N = 2 then N := -2
    else N := 2;
    Result := True;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationJulia3DJF.GetNrVariables: integer;
begin
  Result := 1;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationJulia3DJF.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = var_n_name then begin
    Value := N;
    Result := true;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationJulia3DJF, true, false)) end.
