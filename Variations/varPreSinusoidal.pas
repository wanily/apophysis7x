{$Include 'delphiversion.pas'}

unit varPreSinusoidal;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationPreSinusoidal = class(TVariation)
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
procedure TVariationPreSinusoidal.CalcFunction;
begin
  FTx^ := vvar * sin(FTx^);
  FTy^ := vvar * sin(FTy^);
  {$ifndef Pre15c}
    FTz^ := VVAR * FTz^;
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
constructor TVariationPreSinusoidal.Create;
begin
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationPreSinusoidal.GetInstance: TVariation;
begin
  Result := TVariationPreSinusoidal.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationPreSinusoidal.GetName: string;
begin
  Result := 'pre_sinusoidal';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPreSinusoidal.GetVariableNameAt(const Index: integer): string;
begin
  Result := '';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPreSinusoidal.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPreSinusoidal.GetNrVariables: integer;
begin
  Result := 0
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPreSinusoidal.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationPreSinusoidal, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
