{$Include 'delphiversion.pas'}
unit varRadialBlur;

interface

uses
  Variation, VariationPoolManager;

const
  var_name = 'radial_blur';
  var_a_name = 'radial_blur_angle';

type
  TVariationRadialBlur = class(TVariation)
  private
    angle,
    spin_var, zoom_var: double;

    rnd: array[0..3] of double;
    N: integer;

    procedure CalcZoom;
    procedure CalcSpin;

  public
    constructor Create;

    class function GetName: string; override;
    class function GetInstance: TVariation; override;

    function GetNrVariables: integer; override;
    function GetVariableNameAt(const Index: integer): string; override;

    function GetVariable(const Name: string; var value: double): boolean; override;
    function SetVariable(const Name: string; var value: double): boolean; override;
    function ResetVariable(const Name: string): boolean; override;

    procedure Prepare; override;
    procedure CalcFunction; override;
    procedure ObtainCalculateFunctionPtr(out f: TCalcFunction); override;
  end;

implementation

uses
  math;

// TVariationRadialBlur

///////////////////////////////////////////////////////////////////////////////
constructor TVariationRadialBlur.Create;
begin
  angle := random * 2 - 1;
end;

procedure TVariationRadialBlur.Prepare;
begin
  spin_var := vvar * sin(angle * pi/2);
  zoom_var := vvar * cos(angle * pi/2);

  N := 0;
  rnd[0] := random;
  rnd[1] := random;
  rnd[2] := random;
  rnd[3] := random;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TVariationRadialBlur.ObtainCalculateFunctionPtr(out f: TCalcFunction);
begin
  if IsZero(spin_var) then f := CalcZoom
  else if IsZero(zoom_var) then f := CalcSpin
  else f := CalcFunction;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TVariationRadialBlur.CalcFunction;
var
  rndG, rz, ra: double;
  sina, cosa: extended;
begin
  rndG := (rnd[0] + rnd[1] + rnd[2] + rnd[3] - 2);
  rnd[N] := random;
  N := (N+1) and $3;

  ra := sqrt(sqr(FTx^) + sqr(FTy^));
  SinCos(arctan2(FTy^, FTx^) + spin_var * rndG, sina, cosa);
  rz := zoom_var * rndG - 1;

  FPx^ := FPx^ + ra * cosa + rz * FTx^;
  FPy^ := FPy^ + ra * sina + rz * FTy^;
{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
procedure TVariationRadialBlur.CalcZoom;
var
  r: double;
begin
  r := zoom_var * (rnd[0] + rnd[1] + rnd[2] + rnd[3] - 2);

  rnd[N] := random;
  N := (N+1) and $3;

  FPx^ := FPx^ + r * FTx^;
  FPy^ := FPy^ + r * FTy^;
  FPz^ := FPz^ + vvar * FTz^;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TVariationRadialBlur.CalcSpin;
var
  r: double;
  sina, cosa: extended;
begin
  SinCos(arctan2(FTy^, FTx^) + spin_var * (rnd[0] + rnd[1] + rnd[2] + rnd[3] - 2),
         sina, cosa);
  r := sqrt(sqr(FTx^) + sqr(FTy^));

  rnd[N] := random;
  N := (N+1) and $3;

  FPx^ := FPx^ + r * cosa - FTx^;
  FPy^ := FPy^ + r * sina - FTy^;
  FPz^ := FPz^ + vvar * FTz^;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationRadialBlur.GetInstance: TVariation;
begin
  Result := TVariationRadialBlur.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationRadialBlur.GetName: string;
begin
  Result := var_name;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationRadialBlur.GetVariableNameAt(const Index: integer): string;
begin
  case Index of
    0: Result := var_a_name;
  else
    Result := '';
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationRadialBlur.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = var_a_name then begin
    Value := angle;
    Result := true;
  end;
end;

function TVariationRadialBlur.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = var_a_name then begin
    angle := Value;
    Result := True;
  end;
end;

function TVariationRadialBlur.ResetVariable(const Name: string): boolean;
begin
  Result := False;
  if Name = var_a_name then begin
    if angle <> 0 then angle := 0
    else if angle = 0 then angle := 1;
    Result := True;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationRadialBlur.GetNrVariables: integer;
begin
  Result := 1;
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationRadialBlur, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
