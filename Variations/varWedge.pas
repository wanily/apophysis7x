{$Include 'delphiversion.pas'}

unit varWedge;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationWedge = class(TVariation)
  private
    wedge_angle, wedge_hole, wedge_swirl: double;
    wedge_count : integer;
    C1_2PI, comp_fac: double;
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
procedure TVariationWedge.Prepare;
begin
   C1_2PI := 0.15915494309189533576888376337251;
   comp_fac := 1.0 - wedge_angle * wedge_count * C1_2PI;
end;
procedure TVariationWedge.CalcFunction;
var
  r, a, cosa, sina: double;
  c: integer;
begin

  r := sqrt(sqr(FTx^) + sqr(FTy^));
  a := ArcTan2(FTy^, FTx^) + wedge_swirl * r;
  c := floor((wedge_count * a + PI) * C1_2PI);
  a := a * comp_fac + c * wedge_angle;
  SinCos(a, sina, cosa);

  r := vvar * (r + wedge_hole);
  FPx^ := FPx^ + r * cosa;
  FPy^ := FPy^ + r * sina;
    {$ifndef Pre15c}
  FPz^ := FPz^ + VVAR * FTz^;
  {$endif}
end;

///////////////////////////////////////////////////////////////////////////////
constructor TVariationWedge.Create;
begin
  wedge_angle := PI / 2.0;
  wedge_hole := 0;
  wedge_count := 2;
  wedge_swirl := 0;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationWedge.GetInstance: TVariation;
begin
  Result := TVariationWedge.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationWedge.GetName: string;
begin
  Result := 'wedge';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationWedge.GetVariableNameAt(const Index: integer): string;
begin
  case Index Of
  0: Result := 'wedge_angle';
  1: Result := 'wedge_hole';
  2: Result := 'wedge_count';
  3: Result := 'wedge_swirl';
  else
    Result := '';
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationWedge.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'wedge_angle' then begin
    wedge_angle := Value;
    Result := True;
  end else if Name = 'wedge_hole' then begin
    wedge_hole := Value;
    Result := True;
  end else if Name = 'wedge_count' then begin
    if (Value < 1) then Value := 1;
    Value := Round(value);
    wedge_count := Round(Value);
    Result := True;
  end else if Name = 'wedge_swirl' then begin
    wedge_swirl := Value;
    Result := True;
  end;
end;
function TVariationWedge.ResetVariable(const Name: string): boolean;
begin
  Result := False;
  if Name = 'wedge_angle' then begin
    wedge_angle := PI / 2;
    Result := True;
  end else if Name = 'wedge_hole' then begin
    wedge_hole := 0;
    Result := True;
  end else if Name = 'wedge_count' then begin
    wedge_count := 2;
    Result := True;
  end else if Name = 'wedge_swirl' then begin
    wedge_swirl := 0;
    Result := True;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationWedge.GetNrVariables: integer;
begin
  Result := 4
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationWedge.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'wedge_angle' then begin
    Value := wedge_angle;
    Result := True;
  end else if Name = 'wedge_hole' then begin
    Value := wedge_hole;
    Result := True;
  end else if Name = 'wedge_count' then begin
    Value := wedge_count;
    Result := True;
  end else if Name = 'wedge_swirl' then begin
    Value := wedge_swirl;
    Result := True;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationWedge, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
