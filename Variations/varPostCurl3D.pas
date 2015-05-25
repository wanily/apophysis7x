unit varPostCurl3D;

interface

uses
  Variation, VariationPoolManager;

const
  variation_name = 'post_curl3D';
  num_vars = 3;
  var_cx_name = 'post_curl3D_cx';
  var_cy_name = 'post_curl3D_cy';
  var_cz_name = 'post_curl3D_cz';

type
  TVariationPostCurl3D = class(TVariation)
  private
    cx, cy, cz: double;

    _cx, _cy, _cz,
    cx2, cy2, cz2, c_2,
    c2x, c2y, c2z: double;
  public
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

// TVariationCurl3D

procedure TVariationPostCurl3D.Prepare;
begin
  _cx := VVAR * cx;
  _cy := VVAR * cy;
  _cz := VVAR * cz;

  c2x := 2 * _cx;
  c2y := 2 * _cy;
  c2z := 2 * _cz;

  cx2 := sqr(_cx);
  cy2 := sqr(_cy);
  cz2 := sqr(_cz);

  c_2 := cx2 + cy2 + cz2;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TVariationPostCurl3D.CalcFunction;
var
  x, y, z, r, r2: double;
begin
  x := Max(-1e100, Min(FPx^, 1e100)); // <--- got weird FP overflow there...
  y := Max(-1e100, Min(FPy^, 1e100));
  z := Max(-1e100, Min(FPz^, 1e100));

  r2 := sqr(x) + sqr(y) + sqr(z);
  r := 1.0 / (r2*c_2 + c2x*x - c2y*y + c2z*z + 1);

  FPx^ := r * (x + _cx*r2);
  FPy^ := r * (y + _cy*r2);
  FPz^ := r * (z + _cz*r2);
end;
///////////////////////////////////////////////////////////////////////////////
class function TVariationPostCurl3D.GetInstance: TVariation;
begin
  Result := TVariationPostCurl3D.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationPostCurl3D.GetName: string;
begin
  Result := variation_name;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPostCurl3D.GetVariableNameAt(const Index: integer): string;
begin
  case Index of
    0: Result := var_cx_name;
    1: Result := var_cy_name;
    2: Result := var_cz_name;
  else
    Result := '';
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPostCurl3D.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = var_cx_name then begin
    cx := value;
    Result := True;
  end
  else if Name = var_cy_name then begin
    cy := value;
    Result := True;
  end
  else if Name = var_cz_name then begin
    cz := value;
    Result := True;
  end;
end;

function TVariationPostCurl3D.ResetVariable(const Name: string): boolean;
begin
  Result := False;
  if Name = var_cx_name then begin
    cx := 0;
    Result := True;
  end
  else if Name = var_cy_name then begin
    cy := 0;
    Result := True;
  end
  else if Name = var_cz_name then begin
    cz := 0;
    Result := True;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPostCurl3D.GetNrVariables: integer;
begin
  Result := num_vars;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPostCurl3D.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = var_cx_name then begin
    value := cx;
    Result := True;
  end
  else if Name = var_cy_name then begin
    value := cy;
    Result := True;
  end
  else if Name = var_cz_name then begin
    value := cz;
    Result := True;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationPostCurl3D, true, false)) end.
