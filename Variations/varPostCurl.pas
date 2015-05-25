{$Include 'delphiversion.pas'}

unit varPostCurl;

interface

uses
  Variation, VariationPoolManager;

const
  variation_name = 'post_curl';
  num_vars = 2;

type
  TVariationPostCurl = class(TVariation)
  private
    c1, c2, c22: double;
  public
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

// TVariationCurl3D

procedure TVariationPostCurl.Prepare;
begin
  c1 := c1 * VVAR;
  c2 := c2 * VVAR;
  c22 := 2 * c2;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TVariationPostCurl.CalcFunction;
var
  x, y, r, re, im: double;
begin
  x := FPx^;
  y := FPy^;

  re := 1 + c1 * x + c2 * (sqr(x) - sqr(y));
  im := c1 * y + c22 * x * y;

  r := sqr(re) + sqr(im);
  FPx^ := (x * re + y * im) / r;
  FPy^ := (y * re - x * im) / r;
  {$ifndef Pre15c}
  FPz^ := FPz^ + vvar * FPz^;
{$endif}
end;
///////////////////////////////////////////////////////////////////////////////
class function TVariationPostCurl.GetInstance: TVariation;
begin
  Result := TVariationPostCurl.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationPostCurl.GetName: string;
begin
  Result := variation_name;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPostCurl.GetVariableNameAt(const Index: integer): string;
begin
  case Index of
    0: Result := 'post_curl_c1';
    1: Result := 'post_curl_c2';
  else
    Result := '';
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPostCurl.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'post_curl_c1' then begin
    c1 := value;
    Result := True;
  end
  else if Name = 'post_curl_c2' then begin
    c2 := value;
    Result := True;
  end;
end;

function TVariationPostCurl.ResetVariable(const Name: string): boolean;
begin
  Result := False;
  if Name = 'post_curl_c1' then begin
    c1 := 0;
    Result := True;
  end
  else if Name = 'post_curl_c2' then begin
    c2 := 0;
    Result := True;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPostCurl.GetNrVariables: integer;
begin
  Result := num_vars;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationPostCurl.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = 'post_curl_c1' then begin
    value := c1;
    Result := True;
  end
  else if Name = 'post_curl_c2' then begin
    value := c2;
    Result := True;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
initialization RegisterVariation(TIntegratedVariationLoader.Create(TVariationPostCurl, false, false)) end.
