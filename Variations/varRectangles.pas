{$Include 'delphiversion.pas'}

unit varRectangles;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationRectangles = class(TVariation)
  private
    FRectanglesX, FRectanglesY: double;
  public
    constructor Create;

    class function GetName: string; override;
    class function GetInstance: TVariation; override;

    function GetNrVariables: integer; override;
    function GetVariableNameAt(const Index: integer): string; override;

    function SetVariable(const Name: string; var value: double): boolean; override;
    function GetVariable(const Name: string; var value: double): boolean; override;

    procedure ObtainCalculateFunctionPtr(out f: TCalcFunction); override;
    procedure CalcFunction; override;
    procedure CalcZeroX;
    procedure CalcZeroY;
    procedure CalcZeroXY;

  end;

implementation

uses
  Math;

{ TVariationRectangles }

///////////////////////////////////////////////////////////////////////////////

procedure TVariationRectangles.ObtainCalculateFunctionPtr(out f: TCalcFunction);
begin
  if IsZero(FRectanglesX) then begin
    if IsZero(FRectanglesY) then
      f := CalcZeroXY
    else
      f := CalcZeroX;
  end
  else if IsZero(FRectanglesY) then
    f := CalcZeroY
  else f := CalcFunction;
end;

procedure TVariationRectangles.CalcFunction;
begin
  FPx^ := FPx^ + vvar * ((2*floor(FTx^/FRectanglesX) + 1)*FRectanglesX - FTx^);
  FPy^ := FPy^ + vvar * ((2*floor(FTy^/FRectanglesY) + 1)*FRectanglesY - FTy^);
{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

procedure TVariationRectangles.CalcZeroX;
begin
  FPx^ := FPx^ + vvar * FTx^;
  FPy^ := FPy^ + vvar * ((2*floor(FTy^/FRectanglesY) + 1)*FRectanglesY - FTy^);
{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

procedure TVariationRectangles.CalcZeroY;
begin
  FPx^ := FPx^ + vvar * ((2*floor(FTx^/FRectanglesX) + 1)*FRectanglesX - FTx^);
  FPy^ := FPy^ + vvar * FTy^;
{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

procedure TVariationRectangles.CalcZeroXY;
begin
  FPx^ := FPx^ + vvar * FTx^;
  FPy^ := FPy^ + vvar * FTy^;
{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationRectangles.GetName: string;
begin
  Result := 'rectangles';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationRectangles.GetVariableNameAt(const Index: integer): string;
begin
  case Index Of
  0: Result := 'rectangles_x';
  1: Result := 'rectangles_y';
  else
    Result := '';
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationRectangles.GetNrVariables: integer;
begin
  Result := 2;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationRectangles.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'rectangles_x' then begin
    FRectanglesX := Value;
    Result := True;
  end else if Name = 'rectangles_y' then begin
    FRectanglesY := Value;
    Result := True;
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationRectangles.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'rectangles_x' then begin
    Value := FRectanglesX;
    Result := True;
  end else if Name = 'rectangles_y' then begin
    Value := FRectanglesY;
    Result := True;
  end
end;

///////////////////////////////////////////////////////////////////////////////
constructor TVariationRectangles.Create;
begin
  inherited Create;

  FRectanglesX  := 1.0;
  FRectanglesY := 1.0;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationRectangles.GetInstance: TVariation;
begin
  Result := TVariationRectangles.Create;
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationRectangles, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
