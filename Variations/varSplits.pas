{$Include 'delphiversion.pas'}

unit varSplits;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationSplits = class(TVariation)
  private
    splits_x, splits_y: double; 
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

///////////////////////////////////////////////////////////////////////////////
procedure TVariationSplits.Prepare;
begin
end;

procedure TVariationSplits.CalcFunction;
begin
  if(FTx^ >= 0.0) then
		FPx^ := FPx^ + VVAR * (FTx^ + splits_x)
	else
		FPx^ := FPx^ + VVAR * (FTx^ - splits_x);

  if(FTy^ >= 0.0) then
		FPy^ := FPy^ + VVAR * (FTy^ + splits_y)
	else
		FPy^ := FPy^ + VVAR * (FTy^ - splits_y);

{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
constructor TVariationSplits.Create;
begin
  splits_x := 0;
  splits_y := 0;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationSplits.GetInstance: TVariation;
begin
  Result := TVariationSplits.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationSplits.GetName: string;
begin
  Result := 'splits';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationSplits.GetVariableNameAt(const Index: integer): string;
begin
  case Index Of
  0: Result := 'splits_x';
  1: Result := 'splits_y';
  else
    Result := '';
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationSplits.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'splits_x' then begin
    splits_x := Value;
    Result := True;
  end else if Name = 'splits_y' then begin
    splits_y := Value;
    Result := True;
  end 
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationSplits.GetNrVariables: integer;
begin
  Result := 2
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationSplits.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'splits_x' then begin
    Value := splits_x;
    Result := True;
  end else if Name = 'splits_y' then begin
    Value := splits_y;
    Result := True;
  end
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationSplits, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
