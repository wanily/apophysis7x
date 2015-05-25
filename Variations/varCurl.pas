{$Include 'delphiversion.pas'}

unit varCurl;

interface

uses
  Variation, VariationPoolManager;

const
  variation_name = 'curl';
  num_vars = 2;
  var_c1_name='curl_c1';
  var_c2_name='curl_c2';

//{$define _ASM_}

//                                 z
//  The formula is: f(z) = ------------------- , where z = complex (x + i*y)
//                        c2*(z^2) + c1*z + 1

type
  TVariationCurl = class(TVariation)
  private
    c2, c1: double;

    c2x2: double;

    procedure CalcZeroC2;
    procedure CalcZeroC1;
    procedure CalcZeroC2C1;

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
    procedure ObtainCalculateFunctionPtr(out f: TCalcFunction); override;
  end;

implementation

uses
  math;

// TVariationCurl

///////////////////////////////////////////////////////////////////////////////
constructor TVariationCurl.Create;
begin
  // seriously?
  (*c1 := random;
  c2 := random;

  case random(3) of
    0: c1 := 0;
    1: c2 := 0;
   {else: do nothing}
  end;*)
  c1 := 0;
  c2 := 0;
end;

procedure TVariationCurl.Prepare;
begin
  c2x2 := 2 * c2;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TVariationCurl.ObtainCalculateFunctionPtr(out f: TCalcFunction);
begin
  if IsZero(c1) then begin
    if IsZero(c2) then
      f := CalcZeroC2C1
    else
      f := CalcZeroC1
  end
  else begin
    if IsZero(c2) then
      f := CalcZeroC2
    else
      f := CalcFunction
  end;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TVariationCurl.CalcFunction;
var
  r: double;
  re, im: double;
begin
  re := 1 + c1*FTx^ + c2*(sqr(FTx^) - sqr(FTy^));
  im :=     c1*FTy^ + c2x2*FTx^*FTy^;

  r := vvar / (sqr(re) + sqr(im));

  FPx^ := FPx^  + (FTx^*re + FTy^*im) * r;
  FPy^ := FPy^  + (FTy^*re - FTx^*im) * r;
{$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FTz^;
{$endif}
end;

procedure TVariationCurl.CalcZeroC2;
var
  r: double;
  re, im: double;
begin
  re := 1 + c1*FTx^;
  im :=     c1*FTy^;

  r := vvar / (sqr(re) + sqr(im));

  FPx^ := FPx^  + (FTx^*re + FTy^*im) * r;
  FPy^ := FPy^  + (FTy^*re - FTx^*im) * r;
  FPz^ := FPz^  + vvar * FTz^;
end;

procedure TVariationCurl.CalcZeroC1;
var
  r: double;
  re, im: double;
begin
  re := 1 + c2*(sqr(FTx^) - sqr(FTy^));
  im :=   c2x2*FTx^*FTy^;

  r := vvar / (sqr(re) + sqr(im));

  FPx^ := FPx^  + (FTx^*re + FTy^*im) * r;
  FPy^ := FPy^  + (FTy^*re - FTx^*im) * r;
  FPz^ := FPz^  + vvar * FTz^;
end;

procedure TVariationCurl.CalcZeroC2C1;
var
  r: double;
begin
  FPx^ := FPx^  + vvar*FTx^;
  FPy^ := FPy^  + vvar*FTy^;
  FPz^ := FPz^  + vvar * FTz^;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationCurl.GetInstance: TVariation;
begin
  Result := TVariationCurl.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationCurl.GetName: string;
begin
  Result := variation_name;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationCurl.GetVariableNameAt(const Index: integer): string;
begin
  case Index of
    0: Result := var_c1_name;
    1: Result := var_c2_name;
  else
    Result := '';
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationCurl.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = var_c1_name then begin
    c1 := value;
    Result := True;
  end
  else if Name = var_c2_name then begin
    c2 := value;
    Result := True;
  end;
end;

function TVariationCurl.ResetVariable(const Name: string): boolean;
begin
  Result := False;
  if Name = var_c1_name then begin
    c1 := 0;
    Result := True;
  end
  else if Name = var_c2_name then begin
    c2 := 0;
    Result := True;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationCurl.GetNrVariables: integer;
begin
  Result := num_vars;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationCurl.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = var_c1_name then begin
    value := c1;
    Result := True;
  end
  else if Name = var_c2_name then begin
    value := c2;
    Result := True;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationCurl, {$ifndef Pre15c}false{$else}true{$endif}, false)) end.
