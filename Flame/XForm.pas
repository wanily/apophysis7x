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

{$Include 'delphiversion.pas'}

unit XForm;

interface

uses
{$ifdef Apo7X64}
{$else}
AsmRandom,
{$endif}
  XFormMan, BaseVariation;

const
  MAX_WEIGHT = 1000.0;
  {$ifndef Light}
    {$ifndef T500}
    NXFORMS = 100;
    {$else}
    NXFORMS = 500;
    {$endif}
  {$else}
    NXFORMS = 50;
  {$endif}

type
  TCPpoint = record
    x, y, z, c, o: double;
  end;
  PCPpoint = ^TCPpoint;

  TXYpoint = record
    x, y: double;
  end;
  PXYpoint = ^TXYpoint;

  T2Cpoint = record
    x, y, c1, c2: double;
  end;

  TMatrix = array[0..2, 0..2] of double;

{$ifdef Apo7X64}
{$else}
  //{$define _ASM_}
{$endif}

type
  TXForm = class
  public
    c: array[0..2, 0..1] of double;      // the coefs to the affine part of the function
    p: array[0..2, 0..1] of double;      // post-transform coefs!

    density: double;                     // prob is this function is chosen
    color: double;                       // color coord for this function. 0 - 1
    color2: double;                      // Second color coord for this function. 0 - 1
    vc: double;                          // Calculated color to be passed to the plugin
    vo: double;                          // Calculated opacity to be passed to the plugin
    symmetry: double;
    postXswap: boolean;

    TransformName : string;

    autoZscale: boolean;
    transOpacity: double;
    pluginColor: double;
    modWeights: array [0..NXFORMS] of double;
    PropTable: array of TXForm;

    Orientationtype: integer;
  private
    vars: array of double;
    varprops: TVarPropsList;

    FNrFunctions: Integer;
    FFunctionList: array of TCalcFunction;
    FCalcFunctionList: array of TCalcFunction;

    FTx, FTy: double; // must remain in this order
    FPx, FPy: double; // some asm code relies on this
    FTz, FPz: double;

    FAngle: double;
    FSinA: double;
    FCosA: double;
    FLength: double;
    colorC1, colorC2: double;
    polar_vpi, disc_vpi: double;

    gauss_rnd: array [0..3] of double;
    gauss_N: integer;

    rx_sin, rx_cos, ry_sin, ry_cos: double;
    px_sin, px_cos, py_sin, py_cos: double;

    FRegVariations: array of TBaseVariation;

    procedure PrecalcAngle;
    procedure PrecalcSinCos;
    procedure PrecalcAll;
    procedure DoPostTransform;
    procedure DoInvalidOperation;

    procedure Linear3D;
    procedure Sinusoidal;
    procedure Spherical;
    procedure Swirl;
    procedure Horseshoe;
    procedure Polar;
    procedure Disc;
    procedure Spiral;
    procedure hyperbolic;
    procedure Square;
    procedure Eyefish;
    procedure Bubble;
    procedure Cylinder;
    procedure Noise;
    procedure Blur;
    procedure Gaussian;
    procedure ZBlur;
    procedure Blur3D;

    procedure PreBlur;
    procedure PreZScale;
    procedure PreZTranslate;
    procedure PreRotateX;
    procedure PreRotateY;
{$ifdef Pre15c}
    procedure Linear;
{$else}
    procedure Flatten;
{$endif}
    procedure ZScale;
    procedure ZTranslate;
    procedure ZCone;

    procedure PostRotateX;
    procedure PostRotateY;      

    function Mul33(const M1, M2: TMatrix): TMatrix;
    function Identity: TMatrix;

    procedure BuildFunctionlist;
    procedure AddRegVariations;

  public
    constructor Create;
    destructor Destroy; override;
    procedure Clear;
    procedure Prepare;
    procedure PrepareInvalidXForm;

    procedure Assign(Xform: TXForm);

    procedure NextPoint(var CPpoint: TCPpoint);
    procedure NextPointTo(var CPpoint, ToPoint: TCPpoint);
    procedure NextPointXY(var px, py: double);
    procedure NextPoint2C(var p: T2CPoint);

    procedure Rotate(const degrees: double);
    procedure Translate(const x, y: double);
    procedure Multiply(const a, b, c, d: double);
    procedure Scale(const s: double);

    procedure GetVariable(const name: string; var Value: double);
    procedure SetVariable(const name: string; var Value: double);
    procedure ResetVariable(const name: string);

    function GetVariableStr(const name: string): string;
    procedure SetVariableStr(const name: string; var Value: string);

    function ToXMLString: string;
    function FinalToXMLString(IsEnabled: boolean): string;

    function GetVariation(index : integer) : double;
    procedure SetVariation(index : integer; value : double);
    function NumVariations : integer;
  end;

implementation

uses
  SysUtils, Math, StrUtils;

const
  EPS: double = 1E-300;

function TXForm.NumVariations : integer;
begin
  Result := length(vars);
end;
function TXForm.GetVariation(index : integer) : double;
begin
  Result := vars[index];
end;
procedure TXForm.SetVariation(index : integer; value : double);
begin
  if (vars[index] = 0) and (value <> 0) then begin
    // Activate var here
  end else begin
    // Deactivate var here
  end;
  vars[index] := value;
end;


{ TXForm }

///////////////////////////////////////////////////////////////////////////////
constructor TXForm.Create;
begin
  AddRegVariations;
  BuildFunctionlist;
  SetLength(vars, NRLOCVAR + Length(FRegVariations));
  varprops := GetVarProps;

  Clear;
end;

procedure TXForm.Clear;
var
  i: Integer;
begin
  density := 0;
  color := 0;
  symmetry := 0;
  postXswap := false;
  autoZscale := false;

  c[0, 0] := 1;
  c[0, 1] := 0;
  c[1, 0] := 0;
  c[1, 1] := 1;
  c[2, 0] := 0;
  c[2, 1] := 0;

  p[0, 0] := 1;
  p[0, 1] := 0;
  p[1, 0] := 0;
  p[1, 1] := 1;
  p[2, 0] := 0;
  p[2, 1] := 0;

  for i := 0 to High(vars) do
    vars[i] := 0;
  vars[0] := 1;

  for i := 0 to NXFORMS do
    modWeights[i] := 1;

  SetLength(varprops, 0);
  varprops := GetVarProps;

  transOpacity := 1;
  pluginColor := 1;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.Prepare;
var
  i: integer;
  CalculateAngle, CalculateSinCos, CalculateLength: boolean;
begin
  c[0,0] := c[0][0];
  c[0,1] := c[0][1];
  c[1,0] := c[1][0];
  c[1,1] := c[1][1];
  c[2,0] := c[2][0];
  c[2,1] := c[2][1];

  colorC1 := (1 + symmetry)/2;
  colorC2 := color*(1 - symmetry)/2;

  FNrFunctions := 0;

  for i := 0 to High(FRegVariations) do begin
    FRegVariations[i].FPX := @FPX;
    FRegVariations[i].FPY := @FPY;
    FRegVariations[i].FPz := @FPz;
    FRegVariations[i].FTX := @FTX;
    FRegVariations[i].FTY := @FTY;
    FRegVariations[i].FTz := @FTz;
    FRegVariations[i].a := c[0,0];
    FRegVariations[i].b := c[0,1];
    FRegVariations[i].c := c[1,0];
    FRegVariations[i].d := c[1,1];
    FRegVariations[i].e := c[2,0];
    FRegVariations[i].f := c[2,1];
    FRegVariations[i].color := @vc;
    FRegVariations[i].opacity := @vo;

    FRegVariations[i].vvar := vars[i + NRLOCVAR];
    FRegVariations[i].Prepare;
    FRegVariations[i].GetCalcFunction(FFunctionList[NRLOCVAR + i]);
  end;

  SetLength(FCalcFunctionList, NrVar + 2);

  CalculateAngle := (vars[6] <> 0.0) or (vars[7] <> 0.0);
//  CalculateLength := False;
  CalculateSinCos := (vars[8] <> 0.0) or (vars[10] <> 0.0);

  // Pre- variations
  for i := 0 to NrVar - 1 do begin
    if (vars[i] <> 0.0) and (varprops[i].Priority = 0) then begin
      FCalcFunctionList[FNrFunctions] := FFunctionList[i];
      Inc(FNrFunctions);
    end;
  end;

  // Precalc must be called after pre- vars
  if CalculateAngle or CalculateSinCos then
  begin
    if CalculateAngle and CalculateSinCos then
      FCalcFunctionList[FNrFunctions] := PrecalcAll
    else if CalculateAngle then
      FCalcFunctionList[FNrFunctions] := PrecalcAngle
    else //if CalculateSinCos then
      FCalcFunctionList[FNrFunctions] := PrecalcSinCos;
    Inc(FNrFunctions);
  end;

  // Normal variations
  for i := 0 to NrVar - 1 do begin
    if (vars[i] <> 0.0) then begin
      if (varprops[i].Priority <> 1) then continue;

      FCalcFunctionList[FNrFunctions] := FFunctionList[i];
      Inc(FNrFunctions);
    end;
  end;

  // Post- variations
  for i := 0 to NrVar - 1 do begin
    if (vars[i] <> 0.0) and (varprops[i].Priority = 2) then begin
      FCalcFunctionList[FNrFunctions] := FFunctionList[i];
      Inc(FNrFunctions);
    end;
  end;

  // flatten ;-)
  for i := 0 to NrVar - 1 do begin
    if (vars[i] <> 0.0) and (varprops[i].Priority = 3) then begin
      FCalcFunctionList[FNrFunctions] := FFunctionList[i];
      Inc(FNrFunctions);
    end;
  end;

  polar_vpi := vars[6]/pi;
  disc_vpi := vars[7]/pi;

  gauss_rnd[0] := random;
  gauss_rnd[1] := random;
  gauss_rnd[2] := random;
  gauss_rnd[3] := random;
  gauss_N := 0;

  rx_sin := sin(vars[22] * pi/2);
  rx_cos := cos(vars[22] * pi/2);
  ry_sin := sin(vars[23] * pi/2);
  ry_cos := cos(vars[23] * pi/2);

  px_sin := sin(vars[27] * pi/2);
  px_cos := cos(vars[27] * pi/2);
  py_sin := sin(vars[28] * pi/2);
  py_cos := cos(vars[28] * pi/2);

  if (p[0,0]<>1) or (p[0,1]<>0) or(p[1,0]<>0) or (p[1,1]<>1) or (p[2,0]<>0) or (p[2,1]<>0) then
  begin
    p[0,0] := p[0][0];
    p[0,1] := p[0][1];
    p[1,0] := p[1][0];
    p[1,1] := p[1][1];
    p[2,0] := p[2][0];
    p[2,1] := p[2][1];

    FCalcFunctionList[FNrFunctions] := DoPostTransform;
    Inc(FNrFunctions);
  end;
end;

procedure TXForm.PrepareInvalidXForm;
begin
  c[0,0] := 1;
  c[0,1] := 0;
  c[1,0] := 0;
  c[1,1] := 1;
  c[2,0] := 0;
  c[2,1] := 0;

  colorC1 := 1;
  colorC2 := 0;

  FNrFunctions := 1;
  SetLength(FCalcFunctionList, 1);
  FCalcFunctionList[0] := DoInvalidOperation;
end;

procedure TXForm.PrecalcAngle;
{$ifndef _ASM_}
begin
  FAngle := arctan2(FTx, FTy);
{$else}
asm
    fld     qword ptr [eax + FTx]
    fld     qword ptr [eax + FTy]
    fpatan
    fstp    qword ptr [eax + FAngle]
    //fwait
{$endif}
end;

procedure TXForm.PrecalcSinCos;
{$ifndef _ASM_}
begin
  FLength := sqrt(sqr(FTx) + sqr(FTy)) + EPS;
  FSinA := FTx / FLength;
  FCosA := FTy / FLength;
{$else}
asm
    fld     qword ptr [eax + FTx]
    fld     qword ptr [eax + FTy]
    fld     st(1)
    fmul    st, st
    fld     st(1)
    fmul    st, st
    faddp
    fsqrt
    fadd    qword ptr [EPS] // avoid divide by zero...(?)
    fdiv    st(1), st
    fdiv    st(2), st
    fstp    qword ptr [eax + FLength]
    fstp    qword ptr [eax + FCosA]
    fstp    qword ptr [eax + FSinA]
    //fwait
{$endif}
end;

procedure TXForm.PrecalcAll;
{$ifndef _ASM_}
begin
  FLength := sqrt(sqr(FTx) + sqr(FTy)) + EPS;
  FSinA := FTx / FLength;
  FCosA := FTy / FLength;
  FAngle := arctan2(FTx, FTy);
{$else}
asm
    fld     qword ptr [eax + FTx]
    fld     qword ptr [eax + FTy]
    fld     st(1)
    fld     st(1)
    fpatan
    fstp    qword ptr [eax + FAngle]
    fld     st(1)
    fmul    st, st
    fld     st(1)
    fmul    st, st
    faddp
    fsqrt
    fadd    qword ptr [EPS] // avoid divide by zero...(?)
    fdiv    st(1), st
    fdiv    st(2), st
    fstp    qword ptr [eax + FLength]
    fstp    qword ptr [eax + FCosA]
    fstp    qword ptr [eax + FSinA]
    //fwait
{$endif}
end;

procedure TXForm.DoPostTransform;
var
  tmp: double;
begin
  tmp := FPx;
  FPx := p[0,0] * FPx + p[1,0] * FPy + p[2,0];
  FPy := p[0,1] * tmp + p[1,1] * FPy + p[2,1];
end;

procedure TXForm.DoInvalidOperation;
begin
  raise EMathError.Create('FCalcFunction not initialized!? Probably corrupted flame.');
end;

//--0--////////////////////////////////////////////////////////////////////////
procedure TXForm.Linear3D;
{$ifndef _ASM_}
begin
  FPx := FPx + vars[0] * FTx;
  FPy := FPy + vars[0] * FTy;
  FPz := FPz + vars[0] * FTz;
{$else}
asm
    mov     edx, [eax + vars]
    fld     qword ptr [edx]

    fld     qword ptr [eax + FTz]
    fmul    st, st(1)
    fadd    qword ptr [eax + FPz]
    fstp    qword ptr [eax + FPz]

    fld     qword ptr [eax + FTx]
    fmul    st, st(1)
    fadd    qword ptr [eax + FPx]
    fstp    qword ptr [eax + FPx]
    fld     qword ptr [eax + FTy]
    fmulp
    fadd    qword ptr [eax + FPy]
    fstp    qword ptr [eax + FPy]
    fwait
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
{$ifdef Pre15c}
procedure TXForm.Linear;
begin
  FPx := FPx + vars[1] * FTx;
  FPy := FPy + vars[1] * FTy;
end;
{$else}
procedure TXForm.Flatten;
begin
  FPz := 0;
end;
{$endif}

//--1--////////////////////////////////////////////////////////////////////////
procedure TXForm.Sinusoidal;
begin
  FPx := FPx + vars[2] * sin(FTx);
  FPy := FPy + vars[2] * sin(FTy);
  {$ifndef Pre15c}
  FPz := FPz + FTz * vars[2];
  {$endif}
end;

//--2--////////////////////////////////////////////////////////////////////////
procedure TXForm.Spherical;
var
  r: double;
begin
  r := vars[3] / (sqr(FTx) + sqr(FTy) + EPS);
  FPx := FPx + FTx * r;
  FPy := FPy + FTy * r;
  {$ifndef Pre15c}
  FPz := FPz + FTz * vars[3];
  {$endif}
end;

//--3--////////////////////////////////////////////////////////////////////////
procedure TXForm.Swirl;
var
  sinr, cosr: double;
begin
  SinCos(sqr(FTx) + sqr(FTy), sinr, cosr);
  FPx := FPx + vars[4] * (sinr * FTx - cosr * FTy);
  FPy := FPy + vars[4] * (cosr * FTx + sinr * FTy);
  {$ifndef Pre15c}
  FPz := FPz + FTz * vars[4];
  {$endif}
end;

//--4--////////////////////////////////////////////////////////////////////////
procedure TXForm.Horseshoe;
var
  r: double;
begin
  r := vars[5] / (sqrt(sqr(FTx) + sqr(FTy)) + EPS);
  FPx := FPx + (FTx - FTy) * (FTx + FTy) * r;
  FPy := FPy + (2*FTx*FTy) * r;
  {$ifndef Pre15c}
  FPz := FPz + FTz * vars[5];
  {$endif}
end;

//--5--////////////////////////////////////////////////////////////////////////
procedure TXForm.Polar;
begin
  FPx := FPx + polar_vpi * FAngle; //vars[5] * FAngle / PI;
  FPy := FPy + vars[6] * (sqrt(sqr(FTx) + sqr(FTy)) - 1.0);
  {$ifndef Pre15c}
  FPz := FPz + FTz * vars[6];
  {$endif}
end;

//--6--////////////////////////////////////////////////////////////////////////
procedure TXForm.Disc;
var
  r, sinr, cosr: double;
begin
  SinCos(PI * sqrt(sqr(FTx) + sqr(FTy)), sinr, cosr);
  r := disc_vpi * FAngle; //vars[7] * FAngle / PI;
  FPx := FPx + sinr * r;
  FPy := FPy + cosr * r;
  {$ifndef Pre15c}
  FPz := FPz + FTz * vars[7];
  {$endif}
end;

//--7--////////////////////////////////////////////////////////////////////////
procedure TXForm.Spiral;
var
  r, sinr, cosr: double;
begin
  r := Flength + 1E-6;
  SinCos(r, sinr, cosr);
  r := vars[8] / r;
  FPx := FPx + (FCosA + sinr) * r;
  FPy := FPy + (FsinA - cosr) * r;
  {$ifndef Pre15c}
  FPz := FPz + FTz * vars[8];
  {$endif}
end;

//--10--///////////////////////////////////////////////////////////////////////
procedure TXForm.Hyperbolic;
begin
  FPx := FPx + vars[9] * FTx / (sqr(FTx) + sqr(FTy) + EPS);
  FPy := FPy + vars[9] * FTy;
  {$ifndef Pre15c}
  FPz := FPz + FTz * vars[9];
  {$endif}
end;

//--11--///////////////////////////////////////////////////////////////////////
procedure TXForm.Square;
var
  sinr, cosr: double;
begin
  SinCos(FLength, sinr, cosr);
  FPx := FPx + vars[10] * FSinA * cosr;
  FPy := FPy + vars[10] * FCosA * sinr;
  {$ifndef Pre15c}
  FPz := FPz + FTz * vars[10];
  {$endif};
end;

//--12--///////////////////////////////////////////////////////////////////////
procedure TXForm.Eyefish;
var
  r: double;
begin
  r := 2 * vars[11] / (sqrt(sqr(FTx) + sqr(FTy)) + 1);
  FPx := FPx + r * FTx;
  FPy := FPy + r * FTy;
  {$ifndef Pre15c}
  FPz := FPz + FTz * vars[11];
  {$endif}
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.Bubble;
{$ifndef _ASM_}
var
  r: double;
begin
  r := (sqr(FTx) + sqr(FTy))/4 + 1;
  FPz := FPz + vars[12] * (2 / r - 1);

  r := vars[12] / r;

  FPx := FPx + r * FTx;
  FPy := FPy + r * FTy;
{$else}
asm
    fld     qword ptr [eax + FTy]
    fld     qword ptr [eax + FTx]
    fld     st(1)
    fmul    st, st
    fld     st(1)
    fmul    st, st
    faddp
    fld1
    fadd    st, st
    fadd    st, st
    fdivp   st(1), st

    mov     edx, [eax + vars]
    fld     qword ptr [edx + 12*8]

    fld1
    fadd    st(2), st
    fdivr   st(2), st

    fld     st(2)
    fadd    st, st
    fsubrp  st(1), st
    fmul    st, st(1)
    fadd    qword ptr [eax + FPz]
    fstp    qword ptr [eax + FPz]

    fmulp

    fmul    st(2), st
    fmulp
    fadd    qword ptr [eax + FPx]
    fstp    qword ptr [eax + FPx]
    fadd    qword ptr [eax + FPy]
    fstp    qword ptr [eax + FPy]
    fwait
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.Cylinder;
{$ifndef _ASM_}
begin
  FPx := FPx + vars[13] * sin(FTx);
  FPy := FPy + vars[13] * FTy;
  FPz := FPz + vars[13] * cos(FTx);
{$else}
asm
    mov     edx, [eax + vars]
    fld     qword ptr [edx + 13*8]
    fld     qword ptr [eax + FTx]
    fsincos
    fmul    st, st(2)
    fadd    qword ptr [eax + FPz]
    fstp    qword ptr [eax + FPz]
    fld     qword ptr [eax + FTy]
    fmul    st, st(2)
    fadd    qword ptr [eax + FPy]
    fstp    qword ptr [eax + FPy]
    fmulp
    fadd    qword ptr [eax + FPx]
    fstp    qword ptr [eax + FPx]
    fwait
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.Noise;
var
  r, sinr, cosr: double;
begin
  {$IfDef RandomizeInVarsHack}
  // Randomize here = HACK! Fix me...
  Randomize;
  {$EndIf}

  SinCos(random * 2*pi, sinr, cosr);
  r := vars[14] * random;
  FPx := FPx + FTx * r * cosr;
  FPy := FPy + FTy * r * sinr;
  {$ifndef Pre15c}
  FPz := FPz + FTz * r;
  {$endif}
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.Blur;
var
  r, s, z, sina, cosa: double;
begin
  {$IfDef RandomizeInVarsHack}
  // Randomize here = HACK! Fix me...
  Randomize;
  {$EndIf}
  SinCos(random * 2*pi, sina, cosa);
  r := vars[15] * random;
  FPx := FPx + r * cosa;
  FPy := FPy + r * sina;
  {$ifndef Pre15c}
  FPz := FPz + FTz * r;
  {$endif}
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.Gaussian;
var
  r, s, z, sina, cosa: double;
begin
  {$IfDef RandomizeInVarsHack}
  // Randomize here = HACK! Fix me...
  Randomize;
  {$EndIf}

  SinCos(random * 2*pi, sina, cosa);
  r := vars[16] * (gauss_rnd[0] + gauss_rnd[1] + gauss_rnd[2] + gauss_rnd[3] - 2);
  gauss_rnd[gauss_N] := random;
  gauss_N := (gauss_N+1) and $3;

  FPx := FPx + r * cosa;
  FPy := FPy + r * sina;
  {$ifndef Pre15c}
  FPz := FPz + vars[16] * FTz;
  {$endif}
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.ZBlur;
begin
  FPz := FPz + vars[17] * (gauss_rnd[0] + gauss_rnd[1] + gauss_rnd[2] + gauss_rnd[3] - 2);
  gauss_rnd[gauss_N] := random;
  gauss_N := (gauss_N+1) and $3;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.Blur3D;
var
  r, sina, cosa, sinb, cosb: double;
begin
  {$IfDef RandomizeInVarsHack}
  // Randomize here = HACK! Fix me...
  Randomize;
  {$EndIf}

  SinCos(random * 2*pi, sina, cosa);
  r := vars[18] * (gauss_rnd[0] + gauss_rnd[1] + gauss_rnd[2] + gauss_rnd[3] - 2);
  gauss_rnd[gauss_N] := random;
  gauss_N := (gauss_N+1) and $3;

  SinCos(random * pi, sinb, cosb);
  FPx := FPx + r * sinb * cosa;
  FPy := FPy + r * sinb * sina;
  FPz := FPz + r * cosb;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.PreBlur;
var
  r, sina, cosa: double;
begin
  {$IfDef RandomizeInVarsHack}
  // Randomize here = HACK! Fix me...
  Randomize;
  {$EndIf}

  SinCos(random * 2*pi, sina, cosa);
  r := vars[19] * (gauss_rnd[0] + gauss_rnd[1] + gauss_rnd[2] + gauss_rnd[3] - 2);
  gauss_rnd[gauss_N] := random;
  gauss_N := (gauss_N+1) and $3;

  FTx := FTx + r * cosa;
  FTy := FTy + r * sina;
end;


///////////////////////////////////////////////////////////////////////////////
procedure TXForm.PreZScale;
begin
  FTz := FTz * vars[20];
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.PreZTranslate;
begin
  FTz := FTz + vars[21];
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.PreRotateX;
var
  z: double;
begin
    z := rx_cos * FTz - rx_sin * FTy;
  FTy := rx_sin * FTz + rx_cos * FTy;
  FTz := z;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.PreRotateY;
var
  x: double;
begin
    x := ry_cos * FTx - ry_sin * FTz;
  FTz := ry_sin * FTx + ry_cos * FTz;
  FTx := x;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.ZScale;
{$ifndef _ASM_}
begin
  FPz := FPz + vars[24] * FTz;
{$else}
asm
    fld     qword ptr [eax + FTz]
    mov     edx, [ebx + vars]
    fmul    qword ptr [edx + 24*8]
    fadd    qword ptr [ebx + FPz]
    fstp    qword ptr [ebx + FPz]
    fwait
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.ZTranslate;
begin
  FPz := FPz + vars[25];
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.ZCone;
{$ifndef _ASM_}
begin
  FPz := FPz + vars[26] * sqrt(sqr(FTx) + sqr(FTy));
{$else}
asm
    fld     qword ptr [eax + FTx]
    fmul    st,st
    fld     qword ptr [eax + FTy]
    fmul    st,st
    faddp
    fsqrt
    mov     edx, [ebx + vars]
    fmul    qword ptr [edx + 26*8]
    fadd    qword ptr [ebx + FPz]
    fstp    qword ptr [ebx + FPz]
    fwait
{$endif}
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.PostRotateX;
var
  z: double;
begin
    z := px_cos * FPz - px_sin * FPy;
  FPy := px_sin * FPz + px_cos * FPy;
  FPz := z;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.PostRotateY;
var
  x: double;
begin
    x := py_cos * FPx - py_sin * FPz;
  FPz := py_sin * FPx + py_cos * FPz;
  FPx := x;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.NextPoint(var CPpoint: TCPpoint);
var
  i: Integer;
begin
  // first compute the color coord
//  CPpoint.c := (CPpoint.c + color) * 0.5 * (1 - symmetry) + symmetry * CPpoint.c;
  CPpoint.c := CPpoint.c * colorC1 + colorC2;
  CPpoint.o := transOpacity;

  vc := CPpoint.c;
  vo := CPpoint.o;

  FTx := c[0,0] * CPpoint.x + c[1,0] * CPpoint.y + c[2,0];
  FTy := c[0,1] * CPpoint.x + c[1,1] * CPpoint.y + c[2,1];
  FTz := CPpoint.z;

  Fpx := 0;
  Fpy := 0;
  Fpz := 0;

  for i:= 0 to FNrFunctions-1 do
    FCalcFunctionList[i];

  if (vo < 0) then vo := 0;
  if (vo > 1) then vo := 1;


  CPpoint.o := vo * CPpoint.o;
  CPpoint.c := CPpoint.c + pluginColor * (vc - CPpoint.c);
  CPpoint.x := FPx;
  CPpoint.y := FPy;
  CPPoint.z := FPz;
end;

procedure TXForm.NextPointTo(var CPpoint, ToPoint: TCPpoint);
var
  i: Integer;
begin
  ToPoint.c := CPpoint.c * colorC1 + colorC2;
  ToPoint.o := transOpacity;

  vc := ToPoint.c;
  vo := ToPoint.o;

  FTx := c[0,0] * CPpoint.x + c[1,0] * CPpoint.y + c[2,0];
  FTy := c[0,1] * CPpoint.x + c[1,1] * CPpoint.y + c[2,1];
  FTz := CPpoint.z;

  Fpx := 0;
  Fpy := 0;
  Fpz := 0;

  for i:= 0 to FNrFunctions-1 do
    FCalcFunctionList[i];

  if (vo < 0) then vo := 0;
  if (vo > 1) then vo := 1;

  ToPoint.o := vo * ToPoint.o;
  ToPoint.c := ToPoint.c + pluginColor * (vc - ToPoint.c);
  ToPoint.x := FPx;
  ToPoint.y := FPy;
  ToPoint.z := FPz; //?
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.NextPoint2C(var p: T2CPoint);
var
  i: Integer;
begin
  // first compute the color coord
//  pc1 := (pc1 + color) * 0.5 * (1 - symmetry) + symmetry * pc1;
//  pc2 := (pc2 + color) * 0.5 * (1 - symmetry) + symmetry * pc2;
  p.c1 := p.c1 * colorC1 + colorC2;
  p.c2 := p.c2 * colorC1 + colorC2;

  FTx := c[0,0] * p.x + c[1,0] * p.y + c[2,0];
  FTy := c[0,1] * p.x + c[1,1] * p.y + c[2,1];

  Fpx := 0;
  Fpy := 0;

  for i:= 0 to FNrFunctions-1 do
    FCalcFunctionList[i];

  p.x := FPx;
  p.y := FPy;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.NextPointXY(var px, py: double);
var
  i: integer;
begin
  FTx := c[0,0] * px + c[1,0] * py + c[2,0];
  FTy := c[0,1] * px + c[1,1] * py + c[2,1];
  FTz := 0;

  Fpx := 0;
  Fpy := 0;

  for i:= 0 to FNrFunctions-1 do
    FCalcFunctionList[i];

  px := FPx;
  py := FPy;
end;

///////////////////////////////////////////////////////////////////////////////
function TXForm.Mul33(const M1, M2: TMatrix): TMatrix;
begin
  result[0, 0] := M1[0][0] * M2[0][0] + M1[0][1] * M2[1][0] + M1[0][2] * M2[2][0];
  result[0, 1] := M1[0][0] * M2[0][1] + M1[0][1] * M2[1][1] + M1[0][2] * M2[2][1];
  result[0, 2] := M1[0][0] * M2[0][2] + M1[0][1] * M2[1][2] + M1[0][2] * M2[2][2];
  result[1, 0] := M1[1][0] * M2[0][0] + M1[1][1] * M2[1][0] + M1[1][2] * M2[2][0];
  result[1, 1] := M1[1][0] * M2[0][1] + M1[1][1] * M2[1][1] + M1[1][2] * M2[2][1];
  result[1, 2] := M1[1][0] * M2[0][2] + M1[1][1] * M2[1][2] + M1[1][2] * M2[2][2];
  result[2, 0] := M1[2][0] * M2[0][0] + M1[2][1] * M2[1][0] + M1[2][2] * M2[2][0];
  result[2, 0] := M1[2][0] * M2[0][1] + M1[2][1] * M2[1][1] + M1[2][2] * M2[2][1];
  result[2, 0] := M1[2][0] * M2[0][2] + M1[2][1] * M2[1][2] + M1[2][2] * M2[2][2];
end;

///////////////////////////////////////////////////////////////////////////////
function TXForm.Identity: TMatrix;
var
  i, j: integer;
begin
  for i := 0 to 2 do
    for j := 0 to 2 do
      Result[i, j] := 0;
  Result[0][0] := 1;
  Result[1][1] := 1;
  Result[2][2] := 1;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.Rotate(const degrees: double);
var
  r: double;
  Matrix, M1: TMatrix;
begin
  r := degrees * pi / 180;
  M1 := Identity;
  M1[0, 0] := cos(r);
  M1[0, 1] := -sin(r);
  M1[1, 0] := sin(r);
  M1[1, 1] := cos(r);
  Matrix := Identity;

  Matrix[0][0] := c[0, 0];
  Matrix[0][1] := c[0, 1];
  Matrix[1][0] := c[1, 0];
  Matrix[1][1] := c[1, 1];
  Matrix[0][2] := c[2, 0];
  Matrix[1][2] := c[2, 1];
  Matrix := Mul33(Matrix, M1);
  c[0, 0] := Matrix[0][0];
  c[0, 1] := Matrix[0][1];
  c[1, 0] := Matrix[1][0];
  c[1, 1] := Matrix[1][1];
  c[2, 0] := Matrix[0][2];
  c[2, 1] := Matrix[1][2];
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.Translate(const x, y: double);
var
  Matrix, M1: TMatrix;
begin
  M1 := Identity;
  M1[0, 2] := x;
  M1[1, 2] := y;
  Matrix := Identity;

  Matrix[0][0] := c[0, 0];
  Matrix[0][1] := c[0, 1];
  Matrix[1][0] := c[1, 0];
  Matrix[1][1] := c[1, 1];
  Matrix[0][2] := c[2, 0];
  Matrix[1][2] := c[2, 1];
  Matrix := Mul33(Matrix, M1);
  c[0, 0] := Matrix[0][0];
  c[0, 1] := Matrix[0][1];
  c[1, 0] := Matrix[1][0];
  c[1, 1] := Matrix[1][1];
  c[2, 0] := Matrix[0][2];
  c[2, 1] := Matrix[1][2];
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.Multiply(const a, b, c, d: double);
var
  Matrix, M1: TMatrix;
begin
  M1 := Identity;
  M1[0, 0] := a;
  M1[0, 1] := b;
  M1[1, 0] := c;
  M1[1, 1] := d;
  Matrix := Identity;
  Matrix[0][0] := Self.c[0, 0];
  Matrix[0][1] := Self.c[0, 1];
  Matrix[1][0] := Self.c[1, 0];
  Matrix[1][1] := Self.c[1, 1];
  Matrix[0][2] := Self.c[2, 0];
  Matrix[1][2] := Self.c[2, 1];
  Matrix := Mul33(Matrix, M1);
  Self.c[0, 0] := Matrix[0][0];
  Self.c[0, 1] := Matrix[0][1];
  Self.c[1, 0] := Matrix[1][0];
  Self.c[1, 1] := Matrix[1][1];
  Self.c[2, 0] := Matrix[0][2];
  Self.c[2, 1] := Matrix[1][2];
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.Scale(const s: double);
var
  Matrix, M1: TMatrix;
begin
  M1 := Identity;
  M1[0, 0] := s;
  M1[1, 1] := s;
  Matrix := Identity;
  Matrix[0][0] := c[0, 0];
  Matrix[0][1] := c[0, 1];
  Matrix[1][0] := c[1, 0];
  Matrix[1][1] := c[1, 1];
  Matrix[0][2] := c[2, 0];
  Matrix[1][2] := c[2, 1];
  Matrix := Mul33(Matrix, M1);
  c[0, 0] := Matrix[0][0];
  c[0, 1] := Matrix[0][1];
  c[1, 0] := Matrix[1][0];
  c[1, 1] := Matrix[1][1];
  c[2, 0] := Matrix[0][2];
  c[2, 1] := Matrix[1][2];
end;

///////////////////////////////////////////////////////////////////////////////
destructor TXForm.Destroy;
var
  i: integer;
begin
//  if assigned(Script) then
//    Script.Free;

  for i := 0 to High(FRegVariations) do
    FRegVariations[i].Free;

  inherited;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.BuildFunctionlist;
begin
  SetLength(FFunctionList, NrVar + Length(FRegVariations));

  //fixed
  FFunctionList[0] := Linear3D;
{$ifdef Pre15c}
  FFunctionList[1] := Linear;
{$else}
  FFunctionList[1] := Flatten;
{$endif}
  FFunctionList[2] := Sinusoidal;
  FFunctionList[3] := Spherical;
  FFunctionList[4] := Swirl;
  FFunctionList[5] := Horseshoe;
  FFunctionList[6] := Polar;
  FFunctionList[7] := Disc;
  FFunctionList[8] := Spiral;
  FFunctionList[9] := Hyperbolic;
  FFunctionList[10] := Square;
  FFunctionList[11] := Eyefish;
  FFunctionList[12] := Bubble;
  FFunctionList[13] := Cylinder;
  FFunctionList[14] := Noise;
  FFunctionList[15] := Blur;
  FFunctionList[16] := Gaussian;
  FFunctionList[17] := ZBlur;
  FFunctionList[18] := Blur3D;

  FFunctionList[19] := PreBlur;
  FFunctionList[20] := PreZScale;
  FFunctionList[21] := PreZTranslate;
  FFunctionList[22] := PreRotateX;
  FFunctionList[23] := PreRotateY;

  FFunctionList[24] := ZScale;
  FFunctionList[25] := ZTranslate;
  FFunctionList[26] := ZCone;

  FFunctionList[27] := PostRotateX;
  FFunctionList[28] := PostRotateY;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.AddRegVariations;
var
  i: integer;
begin
  SetLength(FRegVariations, GetNrRegisteredVariations);
  for i := 0 to GetNrRegisteredVariations - 1 do begin
    FRegVariations[i] := GetRegisteredVariation(i).GetInstance;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.Assign(XForm: TXForm);
var
  i,j: integer;
  Name: string;
  Value: double;
begin
  if Not assigned(XForm) then
    Exit;

  for i := 0 to High(vars) do
    vars[i] := XForm.vars[i];

  c := Xform.c;
  p := Xform.p;
  density := XForm.density;
  color := XForm.color;
  color2 := XForm.color2;
  symmetry := XForm.symmetry;
  Orientationtype := XForm.Orientationtype;
  TransformName := XForm.TransformName;

  postXswap := Xform.postXswap;
  autoZscale := Xform.autoZscale;

  for i := 0 to High(FRegVariations)  do begin
    for j := 0 to FRegVariations[i].GetNrVariables - 1 do begin
      Name := FRegVariations[i].GetVariableNameAt(j);
      XForm.FRegVariations[i].GetVariable(Name, Value);
      FRegVariations[i].SetVariable(Name, Value);
    end;
  end;

  for i := 0 to High(modWeights) do
    modWeights[i] := xform.modWeights[i];

  transOpacity := xform.transOpacity;
  pluginColor := xform.pluginColor;
end;

///////////////////////////////////////////////////////////////////////////////
function TXForm.ToXMLString: string;
var
  i, j: integer;
  Name: string;
  Value: double;
  numChaos: integer;
begin
  result := Format('   <xform weight="%g" color="%g" ', [density, color]);
  if symmetry <> 0 then result := result + format('symmetry="%g" ', [symmetry]);

  for i := 0 to nrvar - 1 do begin
    if vars[i] <> 0 then
      Result := Result + varnames(i) + format('="%g" ', [vars[i]]);
  end;
  Result := Result + Format('coefs="%g %g %g %g %g %g" ', [c[0,0], c[0,1], c[1,0], c[1,1], c[2,0], c[2,1]]);
  if (p[0,0]<>1) or (p[0,1]<>0) or(p[1,0]<>0) or (p[1,1]<>1) or (p[2,0]<>0) or (p[2,1]<>0) then
    Result := Result + Format('post="%g %g %g %g %g %g" ', [p[0,0], p[0,1], p[1,0], p[1,1], p[2,0], p[2,1]]);

  for i := 0 to High(FRegVariations)  do begin
    if vars[i+NRLOCVAR] <> 0 then
      for j := 0 to FRegVariations[i].GetNrVariables - 1 do begin
        Name := FRegVariations[i].GetVariableNameAt(j);
//        FRegVariations[i].GetVariable(Name,Value);
//        Result := Result + Format('%s="%g" ', [name, value]);
        Result := Result + Format('%s="%s" ', [name, FRegVariations[i].GetVariableStr(Name)]);
      end;
  end;

  numChaos := -1;
  for i := NXFORMS-1 downto 0 do
    if modWeights[i] <> 1 then begin
      numChaos := i;
      break;
    end;
  if numChaos >= 0 then begin
    Result := Result + 'chaos="';
    for i := 0 to numChaos do
      Result := Result + Format('%g ', [modWeights[i]]);
    Result := Result + '" ';
  end;

  Result := Result + Format('opacity="%g" ', [transOpacity]);
  
  if TransformName <> '' then
    Result := Result + 'name="' + TransformName + '"';

  if pluginColor <> 1 then
    Result := Result + Format('var_color="%g" ', [pluginColor]);

  Result := Result + '/>';
end;

function TXForm.FinalToXMLString(IsEnabled: boolean): string;
var
  i, j: integer;
  Name: string;
  Value: double;
begin
  //  result := Format('   <finalxform enabled="%d" color="%g" symmetry="%g" ',
//                   [ifthen(IsEnabled, 1, 0), color, symmetry]);
  result := Format('   <finalxform color="%g" ', [color]);
  if symmetry <> 0 then result := result + format('symmetry="%g" ', [symmetry]);

  for i := 0 to nrvar - 1 do begin
    if vars[i] <> 0 then
      Result := Result + varnames(i) + format('="%g" ', [vars[i]]);
  end;
  Result := Result + Format('coefs="%g %g %g %g %g %g" ', [c[0,0], c[0,1], c[1,0], c[1,1], c[2,0], c[2,1]]);
  if (p[0,0]<>1) or (p[0,1]<>0) or(p[1,0]<>0) or (p[1,1]<>1) or (p[2,0]<>0) or (p[2,1]<>0) then
    Result := Result + Format('post="%g %g %g %g %g %g" ', [p[0,0], p[0,1], p[1,0], p[1,1], p[2,0], p[2,1]]);
  if pluginColor <> 1 then
    Result := Result + Format('var_color="%g" ', [pluginColor]);

  for i := 0 to High(FRegVariations)  do begin
    if vars[i+NRLOCVAR] <> 0 then
      for j := 0 to FRegVariations[i].GetNrVariables - 1 do begin
        Name := FRegVariations[i].GetVariableNameAt(j);
//        FRegVariations[i].GetVariable(Name,Value);
//        Result := Result + Format('%s="%g" ', [name, value]);
        Result := Result + Format('%s="%s" ', [name, FRegVariations[i].GetVariableStr(Name)]);
      end;
  end;

  Result := Result + '/>';
end;

///////////////////////////////////////////////////////////////////////////////
procedure TXForm.GetVariable(const name: string; var Value: double);
var
  i: integer;
begin
  for i := 0 to High(FRegVariations) do
    if FRegVariations[i].GetVariable(name, value) then
      break;
end;

procedure TXForm.SetVariable(const name: string; var Value: double);
var
  i: integer;
begin
  for i := 0 to High(FRegVariations) do
    if FRegVariations[i].SetVariable(name, value) then
      break;
end;

procedure TXForm.ResetVariable(const name: string);
var
  i: integer;
begin
  for i := 0 to High(FRegVariations) do
    if FRegVariations[i].ResetVariable(name) then
      break;
end;

///////////////////////////////////////////////////////////////////////////////
function TXForm.GetVariableStr(const name: string): string;
var
  i: integer;
begin
  for i := 0 to High(FRegVariations) do begin
    Result := FRegVariations[i].GetVariableStr(name);
    if Result <> '' then break;
  end;
end;

procedure TXForm.SetVariableStr(const name: string; var Value: string);
var
  i: integer;
begin
  for i := 0 to High(FRegVariations) do begin
    if FRegVariations[i].SetVariableStr(name, value) then break;
  end;
end;

end.
