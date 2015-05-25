{$Include 'delphiversion.pas'}

unit varPreSpherical;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationPreSpherical = class(TVariation)
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

{ TVariationPreSpherical }

///////////////////////////////////////////////////////////////////////////////
procedure TVariationPreSpherical.CalcFunction;
var r: double;
begin
  r := vvar / (sqr(FTx^) + sqr(FTy^) + 10e-300);
  FTx^ := FTx^ * r;
  FTy^ := FTy^ * r;
{$ifndef Pre15c}
    FTz^ := VVAR * FTz^;
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
constructor TVariationPreSpherical.Create;
begin
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationPreSpherical.GetInstance: TVariation;
begin
  Result := TVariationPreSpherical.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationPreSpherical.GetName: string;
begin
  Result := 'pre_spherical';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPreSpherical.GetVariableNameAt(const Index: integer): string;
begin
  Result := '';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPreSpherical.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPreSpherical.GetNrVariables: integer;
begin
  Result := 0
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPreSpherical.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationPreSpherical, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
