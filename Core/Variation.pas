unit Variation;

interface

  type TCalcFunction = procedure of object;
  type TVariation = class

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
      class function GetInstance: TVariation; virtual; abstract;

      function GetNrVariables: integer; virtual;
      function GetVariableNameAt(const Index: integer): string; virtual;

      function GetVariable(const Name: string; var Value: double) : boolean; virtual;
      function SetVariable(const Name: string; var Value: double) : boolean; virtual;
      function ResetVariable(const Name: string): boolean; virtual;

      function GetVariableStr(const Name: string): string; virtual;
      function SetVariableStr(const Name: string; var strValue: string) : boolean; virtual;

      procedure Prepare; virtual;

      procedure ObtainCalculateFunctionPtr(out target: TCalcFunction); virtual;
  end;
  type TVariationLoader = class

    public

      Supports3D, SupportsDC: boolean;

      function GetName: string; virtual; abstract;
      function GetInstance: TVariation; virtual; abstract;
      function GetNrVariables: integer; virtual; abstract;
      function GetVariableNameAt(const Index: integer): string; virtual; abstract;

  end;

  type TIntegratedVariation = class of TVariation;
  type TIntegratedVariationLoader = class(TVariationLoader)

    public

      constructor Create(varClass: TIntegratedVariation);

      function GetName: string; override;
      function GetInstance: TVariation; override;
      function GetNrVariables: integer; override;
      function GetVariableNameAt(const Index: integer): string; override;

    private

      mImplementation: TIntegratedVariation;

  end;

implementation uses SysUtils;

function TVariation.GetNrVariables: integer;
begin
  Result := 0;
end;

function TVariation.GetVariable(const Name: string;
  var Value: double): boolean;
begin
  Result := False;
end;

function TVariation.SetVariable(const Name: string;
  var Value: double): boolean;
begin
  Result := False;
end;

function TVariation.ResetVariable(const Name: string): boolean;
var
  zero: double;
begin
  zero := 0;
  Result := SetVariable(Name, zero);
end;

function TVariation.GetVariableStr(const Name: string): string;
var
  Value: double;
begin
  if GetVariable(Name, Value) then
    Result := Format('%.6g', [Value])
  else
    Result := '';
end;

function TVariation.SetVariableStr(const Name: string; var strValue: string): boolean;
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

function TVariation.GetVariableNameAt(const Index: integer): string;
begin
  Result := ''
end;

procedure TVariation.Prepare;
begin
end;

procedure TVariation.ObtainCalculateFunctionPtr(out target: TCalcFunction);
begin
  target := CalcFunction;
end;

constructor TIntegratedVariationLoader.Create(varClass: TIntegratedVariation);
begin
  mImplementation := varClass;
end;

function TIntegratedVariationLoader.GetName: string;
begin
  Result := mImplementation.GetName();
end;

function TIntegratedVariationLoader.GetInstance: TVariation;
begin
  Result := mImplementation.GetInstance();
end;

function TIntegratedVariationLoader.GetNrVariables: integer;
var
  hack: TVariation;
begin
  hack := GetInstance();
  Result := hack.GetNrVariables();
  hack.Free();
end;

function TIntegratedVariationLoader.GetVariableNameAt(const Index: integer): string;
var
  hack: TVariation;
begin
  hack := GetInstance();
  Result := hack.GetVariableNameAt(Index);
  hack.Free();
end;

end.
