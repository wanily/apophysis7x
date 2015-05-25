{$Include 'delphiversion.pas'}

unit varAuger;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationAuger = class(TVariation)
  private
    auger_freq, auger_weight, auger_scale, auger_sym: double;

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
procedure TVariationAuger.Prepare;
begin
end;

procedure TVariationAuger.CalcFunction;
var x, y, s, t, dx, dy: double;
begin
  x := FTx^;
  y := FTy^;

  s := sin(auger_freq * x);
  t := sin(auger_freq * y);

  dx := x + auger_weight * (0.5 * auger_scale * t + abs(x) * t);
  dy := y + auger_weight * (0.5 * auger_scale * s + abs(y) * s);

  FPx^ := FPx^ + VVAR * (x + auger_sym * (dx - x));
  FPy^ := FPy^ + VVAR * dy;
{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
constructor TVariationAuger.Create;
begin
  auger_freq := 5; auger_weight := 0.5;
  auger_scale := 0.1; auger_sym := 0;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationAuger.GetInstance: TVariation;
begin
  Result := TVariationAuger.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationAuger.GetName: string;
begin
  Result := 'auger';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationAuger.GetVariableNameAt(const Index: integer): string;
begin
  case Index Of
  0: Result := 'auger_freq';
  1: Result := 'auger_weight';
  2: Result := 'auger_scale';
  3: Result := 'auger_sym';
  else
    Result := '';
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationAuger.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'auger_freq' then begin
    auger_freq := Value;
    Result := True;
  end else if Name = 'auger_weight' then begin
    auger_weight := Value;
    Result := True;
  end else if Name = 'auger_scale' then begin
    auger_scale := Value;
    Result := True;
  end else if Name = 'auger_sym' then begin
    auger_sym := Value;
    Result := True;
  end
end;
function TVariationAuger.ResetVariable(const Name: string): boolean;
begin
  Result := False;
  if Name = 'auger_freq' then begin
    auger_freq := 5;
    Result := True;
  end else if Name = 'auger_weight' then begin
    auger_weight := 0.5;
    Result := True;
  end else if Name = 'auger_scale' then begin
    auger_sym := 0.1;
    Result := True;
  end else if Name = 'auger_sym' then begin
    auger_sym := 0;
    Result := True;
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationAuger.GetNrVariables: integer;
begin
  Result := 4
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationAuger.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'auger_freq' then begin
    Value := auger_freq;
    Result := True;
  end else if Name = 'auger_weight' then begin
    Value := auger_weight;
    Result := True;
  end else if Name = 'auger_scale' then begin
    Value := auger_scale;
    Result := True;
  end else if Name = 'auger_sym' then begin
    Value := auger_sym;
    Result := True;
  end
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationAuger, true, false)) end.
