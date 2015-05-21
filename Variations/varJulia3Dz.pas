{
     Apophysis Copyright (C) 2001-2004 Mark Townsend
     Apophysis Copyright (C) 2005-2006 Ronald Hordijk, Piotr Borys, Peter Sdobnov
     Apophysis Copyright (C) 2007-2008 Piotr Borys, Peter Sdobnov
     
     Apophysis "3D hack" Copyright (C) 2007-2008 Peter Sdobnov
     Apophysis "7X" Copyright (C) 2009-2010 Georg Kiehne

     This program is free software; you can redistribute it and/or modify
     it under the terms of the GNU General Public License as published by
     the Free Software Foundation; either version 2 of the License, or
     (at your option) any later version.

     This program is distributed in the hope that it will be useful,
     but WITHOUT ANY WARRANTY; without even the implied warranty of
     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
     GNU General Public License for more details.

     You should have received a copy of the GNU General Public License
     along with this program; if not, write to the Free Software
     Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
}

unit varJulia3Dz;

interface

uses
  BaseVariation, XFormMan;

const
  var_name = 'julia3Dz';
  var_n_name='julia3Dz_power';

type
  TVariationJulia3D = class(TBaseVariation)
  private
    N: integer;

    absN: integer;
    cN: double;

    procedure CalcPower1;
    procedure CalcPowerMinus1;
    procedure CalcPower2;
    procedure CalcPowerMinus2;

  public
    constructor Create;

    class function GetName: string; override;
    class function GetInstance: TBaseVariation; override;

    function GetNrVariables: integer; override;
    function GetVariableNameAt(const Index: integer): string; override;

    function SetVariable(const Name: string; var value: double): boolean; override;
    function GetVariable(const Name: string; var value: double): boolean; override;
    function ResetVariable(const Name: string): boolean; override;

    procedure Prepare; override;
    procedure CalcFunction; override;
    procedure GetCalcFunction(var f: TCalcFunction); override;
  end;

implementation

uses
  Math;

// TVariationJulia3D

///////////////////////////////////////////////////////////////////////////////
constructor TVariationJulia3D.Create;
begin
  N := random(5) + 2;
  if random(2) = 0 then N := -N;
end;

procedure TVariationJulia3D.Prepare;
begin
  absN := abs(N);
  cN := 1 / N / 2;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TVariationJulia3D.GetCalcFunction(var f: TCalcFunction);
begin
  if N = 2 then f := CalcPower2
  else if N = -2 then f := CalcPowerMinus2
  else if N = 1 then f := CalcPower1
  else if N = -1 then f := CalcPowerMinus1
  else f := CalcFunction;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TVariationJulia3D.CalcFunction;
var
  r, r2d: double;
  sina, cosa: extended;
begin
  r2d := sqr(FTx^) + sqr(FTy^);
  r := vvar * Math.Power(r2d, cN);

  FPz^ := FPz^ + r * FTz^ / (sqrt(r2d) * absN);

  sincos((arctan2(FTy^, FTx^) + 2*pi*random(absN)) / N, sina, cosa);
  FPx^ := FPx^ + r * cosa;
  FPy^ := FPy^ + r * sina;
end;

procedure TVariationJulia3D.CalcPower2;
var
  r, r2d: double;
  sina, cosa: extended;
begin
  r2d := sqrt(sqr(FTx^) + sqr(FTy^));
  r := vvar * sqrt(r2d);

  FPz^ := FPz^ + r * FTz^ / r2d / 2;

  sincos((arctan2(FTy^, FTx^)/2 + pi*random(2)), sina, cosa);
  FPx^ := FPx^ + r * cosa;
  FPy^ := FPy^ + r * sina;
end;

procedure TVariationJulia3D.CalcPowerMinus2;
var
  r, r2d: double;
  sina, cosa: extended;
begin
  r2d := sqrt(sqr(FTx^) + sqr(FTy^));
  r := vvar / sqrt(r2d);

  FPz^ := FPz^ + r * FTz^ / r2d / 2;

  sincos(pi*random(2) - arctan2(FTy^, FTx^)/2, sina, cosa);
  FPx^ := FPx^ + r * cosa;
  FPy^ := FPy^ + r * sina;
end;

procedure TVariationJulia3D.CalcPower1;
begin
  FPx^ := FPx^ + vvar * FTx^;
  FPy^ := FPy^ + vvar * FTy^;
  FPz^ := FPz^ + vvar * FTz^;
end;

procedure TVariationJulia3D.CalcPowerMinus1;
var
  r: double;
begin
  r := vvar / (sqr(FTx^) + sqr(FTy^));

  FPx^ := FPx^ + r * FTx^;
  FPy^ := FPy^ - r * FTy^;
  FPz^ := FPz^ + r * FTz^;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationJulia3D.GetInstance: TBaseVariation;
begin
  Result := TVariationJulia3D.Create;
end;

///////////////////////////////////////////////////////////////////////////////
class function TVariationJulia3D.GetName: string;
begin
  Result := var_name;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationJulia3D.GetVariableNameAt(const Index: integer): string;
begin
  case Index of
    0: Result := var_n_name;
  else
    Result := '';
  end
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationJulia3D.SetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = var_n_name then begin
    N := Round(Value);
    if N = 0 then N := 1;
    Value := N;
    Result := True;
  end;
end;

function TVariationJulia3D.ResetVariable(const Name: string): boolean;
begin
  Result := False;
  if Name = var_n_name then begin
    if N = 2 then N := -2
    else N := 2;
    Result := True;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationJulia3D.GetNrVariables: integer;
begin
  Result := 1;
end;

///////////////////////////////////////////////////////////////////////////////
function TVariationJulia3D.GetVariable(const Name: string; var value: double): boolean;
begin
  Result := False;
  if Name = var_n_name then begin
    Value := N;
    Result := true;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
initialization
  RegisterVariation(TVariationClassLoader.Create(TVariationJulia3D), true, false);
end.
