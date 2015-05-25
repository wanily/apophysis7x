{$Include 'delphiversion.pas'}

unit varWaves2;

interface

uses
  Variation, VariationPoolManager;

type
  TVariationWaves2 = class(TVariation)
  private
    waves2_freqx, waves2_freqy, waves2_freqz: double;
    waves2_scalex, waves2_scaley, waves2_scalez: double;

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
procedure TVariationWaves2.Prepare;
begin
end;

procedure TVariationWaves2.CalcFunction;
begin
  FPx^ := FPx^ + VVAR * (FTx^ + waves2_scalex * sin(FTy^ * waves2_freqx));
  FPy^ := FPy^ + VVAR * (FTy^ + waves2_scaley * sin(FTx^ * waves2_freqy));
  {$ifndef Pre15c}
  FPz^ := FPz^ + VVAR * (FTz^ + waves2_scalez * sin(sqrt(sqr(FTx^)+sqr(FTy^)) * waves2_freqz));
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
constructor TVariationWaves2.Create;
begin
  waves2_freqx := 2; waves2_scalex := 1;
  waves2_freqy := 2; waves2_scaley := 1;
  waves2_freqz := 0; waves2_scalez := 0;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationWaves2.GetInstance: TVariation;
begin
  Result := TVariationWaves2.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationWaves2.GetName: string;
begin
  Result := 'waves2';
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationWaves2.GetVariableNameAt(const Index: integer): string;
begin
  case Index Of
  {$ifndef Pre15c}
  0: Result := 'waves2_freqx';
  1: Result := 'waves2_freqy';
  2: Result := 'waves2_freqz';
  3: Result := 'waves2_scalex';
  4: Result := 'waves2_scaley';
  5: Result := 'waves2_scalez';
  {$else}
  0: Result := 'waves2_freqx';
  1: Result := 'waves2_freqy';
  2: Result := 'waves2_scalex';
  3: Result := 'waves2_scaley';
  {$endif}
  else
    Result := '';
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationWaves2.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'waves2_freqx' then begin
    waves2_freqx := Value;
    Result := True;
  end else if Name = 'waves2_freqy' then begin
    waves2_freqy := Value;
    Result := True;
 {$ifndef Pre15c}
  end else if Name = 'waves2_freqz' then begin
    waves2_freqz := Value;
    Result := True;
 {$endif}
  end else if Name = 'waves2_scalex' then begin
    waves2_scalex := Value;
    Result := True;
  end else if Name = 'waves2_scaley' then begin
    waves2_scaley := Value;
    Result := True;
  {$ifndef Pre15c}
  end else if Name = 'waves2_scalez' then begin
    waves2_scalez := Value;
    Result := True;
  {$endif}
  end
end;
function TVariationWaves2.ResetVariable(const Name: string): boolean;
begin
  Result := False;
  if Name = 'waves2_freqx' then begin
    waves2_freqx := 2;
    Result := True;
  end else if Name = 'waves2_freqy' then begin
    waves2_freqy := 2;
    Result := True;
 {$ifndef Pre15c}
  end else if Name = 'waves2_freqz' then begin
    waves2_freqz := 0;
    Result := True;
 {$endif}
  end else if Name = 'waves2_scalex' then begin
    waves2_scalex := 1;
    Result := True;
  end else if Name = 'waves2_scaley' then begin
    waves2_scaley := 1;
    Result := True;
  {$ifndef Pre15c}
  end else if Name = 'waves2_scalez' then begin
    waves2_scalez := 0;
    Result := True;
  {$endif}
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationWaves2.GetNrVariables: integer;
begin
  {$ifndef Pre15c}
  Result := 6
{$else}
  Result := 4
  {$endif}
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationWaves2.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'waves2_freqx' then begin
    Value := waves2_freqx;
    Result := True;
  end else if Name = 'waves2_freqy' then begin
    Value := waves2_freqy;
    Result := True;
    {$ifndef Pre15c}
  end else if Name = 'waves2_freqz' then begin
    Value := waves2_freqz;
    Result := True;
  {$endif}
  end else if Name = 'waves2_scalex' then begin
    Value := waves2_scalex;
    Result := True;
  end else if Name = 'waves2_scaley' then begin
    Value := waves2_scaley;
    Result := True;
      {$ifndef Pre15c}
  end else if Name = 'waves2_scalez' then begin
    Value := waves2_scalez;
    Result := True;
  {$endif}
  end
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationWaves2, false, false)) end.
