{$Include 'delphiversion.pas'}

unit varPreDisc;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationPreDisc = class(TVariation)
  private
    vvar_by_pi: double;
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

{ TVariationPreSpherical }

///////////////////////////////////////////////////////////////////////////////
procedure TVariationPreDisc.Prepare;
begin
  vvar_by_pi := vvar / PI;
end;

procedure TVariationPreDisc.CalcFunction;
var
  r, sinr, cosr: double;
begin
  SinCos(PI * sqrt(sqr(FTx^) + sqr(FTy^)), sinr, cosr);
  r := vvar_by_pi * arctan2(FTx^, FTy^);
  FTx^ := sinr * r;
  FTy^ := cosr * r;
  {$ifndef Pre15c}
    FTz^ := VVAR * FTz^;
{$endif}

end;

///////////////////////////////////////////////////////////////////////////////
constructor TVariationPreDisc.Create;
begin
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationPreDisc.GetInstance: TVariation;
begin
  Result := TVariationPreDisc.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationPreDisc.GetName: string;
begin
  Result := 'pre_disc';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPreDisc.GetVariableNameAt(const Index: integer): string;
begin
  Result := '';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPreDisc.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPreDisc.GetNrVariables: integer;
begin
  Result := 0
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPreDisc.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationPreDisc, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
