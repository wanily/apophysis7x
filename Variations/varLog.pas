{$Include 'delphiversion.pas'}

unit varLog;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationLog = class(TVariation)
  private
    base, denom: double;
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

{ TVariationPreSpherical }


///////////////////////////////////////////////////////////////////////////////
procedure TVariationLog.Prepare;
begin
  denom := 0.5 / Ln(base);
end;
procedure TVariationLog.CalcFunction;
begin
  FPx^ := FPx^ + vvar * Ln(sqr(FTx^) + sqr(FTy^)) * denom;
  FPy^ := FPy^ + vvar * ArcTan2(FTy^, FTx^);
{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
constructor TVariationLog.Create;
begin
  base := 2.71828182845905;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationLog.GetInstance: TVariation;
begin
  Result := TVariationLog.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationLog.GetName: string;
begin
  Result := 'log';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationLog.GetVariableNameAt(const Index: integer): string;
begin
  case Index Of
  0: Result := 'log_base';
  else
    Result := '';
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationLog.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'log_base' then begin
    base := Value;
    if (base < 1E-6) then
      base := 1E-6;
    Result := True;
  end;
end;
function TVariationLog.ResetVariable(const Name: string): boolean;
begin
  Result := False;
  if Name = 'log_base' then begin
    base := 2.71828182845905;
    Result := True;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationLog.GetNrVariables: integer;
begin
  Result := 1
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationLog.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'log_base' then begin
    Value := base;
    Result := True;
  end
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationLog, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
