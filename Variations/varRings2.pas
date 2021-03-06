{$Include 'delphiversion.pas'}
unit varRings2;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationRings2 = class(TVariation)
  private
    FVal, dx: double;
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

{ TVariationRings2 }

///////////////////////////////////////////////////////////////////////////////
procedure TVariationRings2.Prepare;
const
  EPS = 1E-10;
begin
  dx := sqr(FVal) + EPS;
end;

procedure TVariationRings2.CalcFunction;
var
  r: double;
  Length: double;
  Angle: double;
begin
  Length := sqrt(sqr(FTx^) + sqr(FTy^));
{ // all this range-checking crap only slows us down...
  if (FTx^ < -EPS) or (FTx^ > EPS) or (FTy^ < -EPS) or (FTy^ > EPS) then
    Angle := arctan2(FTx^, FTy^)
  else
    Angle := 0.0;
} // ...and besides, we don't need arctan() if we have Length!

//  dx := sqr(FVal) + EPS; - we can precalc it!!!
//  r := Length + dx - System.Int((Length + dx)/(2 * dx)) * 2 * dx - dx + Length * (1-dx);
//              ^^^^......he he, lots of useless calculations......^^^^
  r := vvar * (2 - dx * (System.Int((Length/dx + 1)/2) * 2 / Length + 1));

  FPx^ := FPx^ + r * FTx^;
  FPy^ := FPy^ + r * FTy^;
{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
constructor TVariationRings2.Create;
begin
  FVal := Random * 2;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationRings2.GetInstance: TVariation;
begin
  Result := TVariationRings2.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationRings2.GetName: string;
begin
  Result := 'rings2';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationRings2.GetVariableNameAt(const Index: integer): string;
begin
  case Index Of
  0: Result := 'rings2_val';
  else
    Result := '';
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationRings2.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'rings2_val' then begin
    FVal := Value;
    Result := True;
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationRings2.GetNrVariables: integer;
begin
  Result := 1
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationRings2.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'rings2_val' then begin
    Value := FVal;
    Result := True;
  end
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationRings2, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
