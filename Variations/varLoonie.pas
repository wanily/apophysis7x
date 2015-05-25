{$Include 'delphiversion.pas'}

unit varLoonie;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationLoonie = class(TVariation)
  private
    sqrvar: double;
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
procedure TVariationLoonie.Prepare;
begin
  sqrvar := VVAR * VVAR;
end;

procedure TVariationLoonie.CalcFunction;
var r, r2 : double;
begin
  r2 := sqr(FTx^) + sqr(FTy^);

	if (r2 < (sqrvar)) and (r2 <> 0) then
	begin
		r := VVAR * sqrt((sqrvar) / r2 - 1.0);
		FPx^ := FPx^ + r * FTx^;
		FPy^ := FPy^ + r * FTy^;
	end else begin
		FPx^ := FPx^ + VVAR * FTx^;
		FPy^ := FPy^ + VVAR * FTy^;
	end;

{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
constructor TVariationLoonie.Create;
begin
  sqrvar := 0;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationLoonie.GetInstance: TVariation;
begin
  Result := TVariationLoonie.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationLoonie.GetName: string;
begin
  Result := 'loonie';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationLoonie.GetVariableNameAt(const Index: integer): string;
begin
  Result := '';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationLoonie.SetVariable(const Name: string; var value: double): boolean;
var temp: double;
begin
  Result := False;
end;
function TVariationLoonie.ResetVariable(const Name: string): boolean;
begin
  Result := False;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationLoonie.GetNrVariables: integer;
begin
  Result := 0
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationLoonie.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationLoonie, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
