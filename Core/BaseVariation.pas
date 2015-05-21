unit BaseVariation;

interface

type
  TCalcFunction = procedure of object;

type
  TBaseVariation = class

  protected
    procedure CalcFunction; virtual; abstract;

  public
    vvar: double;
    FTx, FTy: ^double;
    FPx, FPy: ^double;
    FTz, FPz: ^double;

    color, opacity: ^double;
    a, b, c, d, e, f: double;

    class function GetName: string; virtual; abstract;
    class function GetInstance: TBaseVariation; virtual; abstract;

    function GetNrVariables: integer; virtual;
    function GetVariableNameAt(const Index: integer): string; virtual;

    function GetVariable(const Name: string; var Value: double)
      : boolean; virtual;
    function SetVariable(const Name: string; var Value: double)
      : boolean; virtual;
    function ResetVariable(const Name: string): boolean; virtual;

    function GetVariableStr(const Name: string): string; virtual;
    function SetVariableStr(const Name: string; var strValue: string)
      : boolean; virtual;

    procedure Prepare; virtual;

    procedure GetCalcFunction(var Delphi_Suxx: TCalcFunction); virtual;
  end;

  TBaseVariationClass = class of TBaseVariation;

type
  TVariationLoader = class
  public
    Supports3D, SupportsDC: boolean;

    function GetName: string; virtual; abstract;
    function GetInstance: TBaseVariation; virtual; abstract;
    function GetNrVariables: integer; virtual; abstract;
    function GetVariableNameAt(const Index: integer): string; virtual; abstract;
  end;

type
  TVariationClassLoader = class(TVariationLoader)
  public
    constructor Create(varClass: TBaseVariationClass);
    function GetName: string; override;
    function GetInstance: TBaseVariation; override;
    function GetNrVariables: integer; override;
    function GetVariableNameAt(const Index: integer): string; override;

  private
    VariationClass: TBaseVariationClass;
  end;

function fmod(x, y: double): double;

implementation

uses SysUtils;

function fmod(x, y: double): double;
begin
  Result := frac(x / y) * y;
end;

function TBaseVariation.GetNrVariables: integer;
begin
  Result := 0;
end;

function TBaseVariation.GetVariable(const Name: string;
  var Value: double): boolean;
begin
  Result := False;
end;

function TBaseVariation.SetVariable(const Name: string;
  var Value: double): boolean;
begin
  Result := False;
end;

function TBaseVariation.ResetVariable(const Name: string): boolean;
var
  zero: double;
begin
  zero := 0;
  Result := SetVariable(Name, zero);
end;

function TBaseVariation.GetVariableStr(const Name: string): string;
var
  Value: double;
begin
  if GetVariable(Name, Value) then
    Result := Format('%.6g', [Value])
  else
    Result := '';
end;

function TBaseVariation.SetVariableStr(const Name: string;
  var strValue: string): boolean;
var
  v, oldv: double;
begin
  if GetVariable(Name, oldv) then
  begin
    try
      v := StrToFloat(strValue);
      SetVariable(Name, v);
    except
      v := oldv;
    end;
    strValue := Format('%.6g', [v]);
    Result := true;
  end
  else
    Result := False;
end;

function TBaseVariation.GetVariableNameAt(const Index: integer): string;
begin
  Result := ''
end;

procedure TBaseVariation.Prepare;
begin
end;

procedure TBaseVariation.GetCalcFunction(var Delphi_Suxx: TCalcFunction);
begin
  Delphi_Suxx := CalcFunction;
end;

constructor TVariationClassLoader.Create(varClass: TBaseVariationClass);
begin
  VariationClass := varClass;
end;

function TVariationClassLoader.GetName: string;
begin
  Result := VariationClass.GetName();
end;

function TVariationClassLoader.GetInstance: TBaseVariation;
begin
  Result := VariationClass.GetInstance();
end;

function TVariationClassLoader.GetNrVariables: integer;
var
  hack: TBaseVariation;
begin
  hack := GetInstance();
  Result := hack.GetNrVariables();
  hack.Free();
end;

function TVariationClassLoader.GetVariableNameAt(const Index: integer): string;
var
  hack: TBaseVariation;
begin
  hack := GetInstance();
  Result := hack.GetVariableNameAt(Index);
  hack.Free();
end;

end.
