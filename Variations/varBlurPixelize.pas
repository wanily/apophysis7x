{$Include 'delphiversion.pas'}

unit varBlurPixelize;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationBlurPixelize = class(TVariation)
  private
    blur_pixelize_size, blur_pixelize_scale: double;
    inv_size, v: double;
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
procedure TVariationBlurPixelize.Prepare;
begin
  inv_size := 1.0 / blur_pixelize_size;
  v := vvar * blur_pixelize_size;
end;

procedure TVariationBlurPixelize.CalcFunction;
var x, y: double;
begin
  x := floor(FTx^*(inv_size));
	y := floor(FTy^*(inv_size));

  FPx^ := FPx^ + (v) * (x + (blur_pixelize_scale) * (random - 0.5) + 0.5);
  FPy^ := FPy^ + (v) * (y + (blur_pixelize_scale) * (random - 0.5) + 0.5);
{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
constructor TVariationBlurPixelize.Create;
begin
  blur_pixelize_size := 0.1;
  blur_pixelize_scale := 1;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationBlurPixelize.GetInstance: TVariation;
begin
  Result := TVariationBlurPixelize.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationBlurPixelize.GetName: string;
begin
  Result := 'blur_pixelize';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationBlurPixelize.GetVariableNameAt(const Index: integer): string;
begin
  case Index Of
  0: Result := 'blur_pixelize_size';
  1: Result := 'blur_pixelize_scale';
  else
    Result := '';
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationBlurPixelize.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'blur_pixelize_size' then begin
    if (value < 1e-6) then value := 1e-6;
    blur_pixelize_size := Value;
    Result := True;
  end else if Name = 'blur_pixelize_scale' then begin
    blur_pixelize_scale := Value;
    Result := True;
  end 
end;
function TVariationBlurPixelize.ResetVariable(const Name: string): boolean;
begin
  Result := False;
  if Name = 'blur_pixelize_size' then begin
    blur_pixelize_size := 0.1;
    Result := True;
  end else if Name = 'blur_pixelize_scale' then begin
    blur_pixelize_size := 1;
    Result := True;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationBlurPixelize.GetNrVariables: integer;
begin
  Result := 2
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationBlurPixelize.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'blur_pixelize_size' then begin
    Value := blur_pixelize_size;
    Result := True;
  end else if Name = 'blur_pixelize_scale' then begin
    Value := blur_pixelize_scale;
    Result := True;
  end
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationBlurPixelize, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
