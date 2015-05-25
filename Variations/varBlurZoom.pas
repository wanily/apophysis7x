{$Include 'delphiversion.pas'}

unit varBlurZoom;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationBlurZoom = class(TVariation)
  private
    blur_zoom_length, blur_zoom_x, blur_zoom_y: double; 
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
procedure TVariationBlurZoom.Prepare;
begin
end;

procedure TVariationBlurZoom.CalcFunction;
var z: double;
begin

  z := 1.0 + blur_zoom_length * random;
  FPx^ := FPx^ + vvar * ((FTx^ - blur_zoom_x) * z + blur_zoom_x);
  FPy^ := FPy^ + vvar * ((FTy^ - blur_zoom_y) * z - blur_zoom_y);
{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
constructor TVariationBlurZoom.Create;
begin
  blur_zoom_length := 0;
  blur_zoom_x := 0;
  blur_zoom_y := 0;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationBlurZoom.GetInstance: TVariation;
begin
  Result := TVariationBlurZoom.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationBlurZoom.GetName: string;
begin
  Result := 'blur_zoom';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationBlurZoom.GetVariableNameAt(const Index: integer): string;
begin
  case Index Of
  0: Result := 'blur_zoom_length';
  1: Result := 'blur_zoom_x';
  2: Result := 'blur_zoom_y';
  else
    Result := '';
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationBlurZoom.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'blur_zoom_length' then begin
    blur_zoom_length := Value;
    Result := True;
  end else if Name = 'blur_zoom_x' then begin
    blur_zoom_y := Value;
    Result := True;
  end else if Name = 'blur_zoom_y' then begin
    blur_zoom_y := Value;
    Result := True;
  end 
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationBlurZoom.GetNrVariables: integer;
begin
  Result := 3
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationBlurZoom.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'blur_zoom_length' then begin
    Value := blur_zoom_length;
    Result := True;
  end else if Name = 'blur_zoom_x' then begin
    Value := blur_zoom_x;
    Result := True;
  end else if Name = 'blur_zoom_y' then begin
    Value := blur_zoom_y;
    Result := True;
  end
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationBlurZoom, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
