{$Include 'delphiversion.pas'}

unit varBlurCircle;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationBlurCircle = class(TVariation)
  private
    VVAR4_PI: double;
    PI_4: double; 
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
procedure TVariationBlurCircle.Prepare;
begin
  VVAR4_PI := VVAR * 4.0 / PI;
  PI_4 := PI / 4.0;
end;

procedure TVariationBlurCircle.CalcFunction;
var
  x, y, absx, absy, side, perimeter, r, sina, cosa: double;
begin
  x := 2.0 * random - 1.0;
  y := 2.0 * random - 1.0;

  absx := x; if absx < 0 then absx := absx * -1.0;
  absy := y; if absy < 0 then absy := absy * -1.0;

  if (absx >= absy) then
  begin
    if (x >= absy) then
      perimeter := absx + y
    else perimeter := 5.0 * absx - y;
    side := absx;
  end else
  begin
    if (y >= absx) then
      perimeter := 3.0 * absy - x
    else perimeter := 7.0 * absy + x;
    side := absy;
  end;

  r := VVAR * side;
  SinCos(PI_4 * perimeter / side - PI_4, sina, cosa);

  FPx^ := FPx^ + r * cosa;
  FPy^ := FPy^ + r * sina;
{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;


///////////////////////////////////////////////////////////////////////////////
constructor TVariationBlurCircle.Create;
begin
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationBlurCircle.GetInstance: TVariation;
begin
  Result := TVariationBlurCircle.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationBlurCircle.GetName: string;
begin
  Result := 'blur_circle';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationBlurCircle.GetVariableNameAt(const Index: integer): string;
begin
  Result := '';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationBlurCircle.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationBlurCircle.GetNrVariables: integer;
begin
  Result := 0
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationBlurCircle.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationBlurCircle, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
