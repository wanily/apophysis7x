{$Include 'delphiversion.pas'}

unit varFoci;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationFoci = class(TVariation)
  public
    constructor Create;

    class function GetName: string; override;
    class function GetInstance: TVariation; override;

    function GetNrVariables: integer; override;
    function GetVariableNameAt(const Index: integer): string; override;

    function SetVariable(const Name: string; var value: double): boolean; override;
    function GetVariable(const Name: string; var value: double): boolean; override;


    procedure CalcFunction; override;
  end;

implementation

uses
  Math;


///////////////////////////////////////////////////////////////////////////////
procedure TVariationFoci.CalcFunction;
var
  expx, expnx, siny, cosy, tmp: double;
begin
  expx := exp(FTx^) * 0.5;
  expnx := 0.25 / expx;
  sincos(FTy^, siny, cosy);

  tmp := ( expx + expnx - cosy );
  if (tmp = 0) then tmp := 1e-6;
  tmp := VVAR / tmp;

  FPx^ := FPx^ + (expx - expnx) * tmp;
  FPy^ := FPy^ + siny * tmp;

{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
constructor TVariationFoci.Create;
begin
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationFoci.GetInstance: TVariation;
begin
  Result := TVariationFoci.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationFoci.GetName: string;
begin
  Result := 'foci';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationFoci.GetVariableNameAt(const Index: integer): string;
begin
  Result := '';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationFoci.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationFoci.GetNrVariables: integer;
begin
  Result := 0
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationFoci.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationFoci, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
