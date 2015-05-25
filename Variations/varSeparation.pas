{$Include 'delphiversion.pas'}
unit varSeparation;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationSeparation = class(TVariation)
  private
    separation_x, separation_y: double;
    separation_xinside, separation_yinside: double;
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
procedure TVariationSeparation.Prepare;
begin
end;

procedure TVariationSeparation.CalcFunction;
begin
  if(FTx^ > 0.0) then
		FPx^ := FPx^ + VVAR * (sqrt(sqr(FTx^) + sqr(separation_x))- FTx^ * (separation_xinside))
	else
		FPx^ := FPx^ - VVAR * (sqrt(sqr(FTx^) + sqr(separation_x))+ FTx^ * (separation_xinside)) ;
	if(FTy^ > 0.0) then
		FPy^ := FPy^ + VVAR * (sqrt(sqr(FTy^) + sqr(separation_y))- FTy^ * (separation_yinside))
	else
		FPy^ := FPy^ - VVAR * (sqrt(sqr(FTy^) + sqr(separation_y))+ FTy^ * (separation_yinside)) ;

{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
constructor TVariationSeparation.Create;
begin
  separation_x := 1;
  separation_y := 1;
  separation_xinside := 0;
  separation_yinside := 0;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationSeparation.GetInstance: TVariation;
begin
  Result := TVariationSeparation.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationSeparation.GetName: string;
begin
  Result := 'separation';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationSeparation.GetVariableNameAt(const Index: integer): string;
begin
  case Index Of
  0: Result := 'separation_x';
  1: Result := 'separation_y';
  2: Result := 'separation_xinside';
  3: Result := 'separation_yinside';
  else
    Result := '';
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationSeparation.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'separation_x' then begin
    separation_x := Value;
    Result := True;
  end else if Name = 'separation_y' then begin
    separation_y := Value;
    Result := True;
  end else if Name = 'separation_xinside' then begin
    separation_xinside := Value;
    Result := True;
  end else if Name = 'separation_yinside' then begin
    separation_yinside := Value;
    Result := True;
  end 
end;
function TVariationSeparation.ResetVariable(const Name: string): boolean;
begin
  Result := False;
  if Name = 'separation_x' then begin
    separation_x := 1;
    Result := True;
  end else if Name = 'separation_y' then begin
    separation_y := 1;
    Result := True;
  end else if Name = 'separation_xinside' then begin
    separation_xinside := 0;
    Result := True;
  end else if Name = 'separation_yinside' then begin
    separation_yinside := 0;
    Result := True;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationSeparation.GetNrVariables: integer;
begin
  Result := 4
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationSeparation.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'separation_x' then begin
    Value := separation_x;
    Result := True;
  end else if Name = 'separation_y' then begin
    Value := separation_y;
    Result := True;
  end else if Name = 'separation_xinside' then begin
    Value := separation_xinside;
    Result := True;
  end else if Name = 'separation_yinside' then begin
    Value := separation_yinside;
    Result := True;
  end
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationSeparation, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
