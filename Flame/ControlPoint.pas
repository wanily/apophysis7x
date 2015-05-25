unit ControlPoint;

interface

uses
  Classes, Windows, PaletteIO, XForm, XFormMan,
  SysUtils, math, ZLib;

const
  SUB_BATCH_SIZE = 10000;
  PROP_TABLE_SIZE = 1024;
  PREFILTER_WHITE = (1 shl 26);
  FILTER_CUTOFF = 1.8;
  BRIGHT_ADJUST = 2.3;
  FUSE = 15;

type
  TCoefsArray = array [0 .. 2, 0 .. 1] of double;
  pCoefsArray = ^TCoefsArray;

  TTriangle = record
    x: array [0 .. 2] of double;
    y: array [0 .. 2] of double;
  end;

  TTriangles = array [-1 .. NXFORMS] of TTriangle;

  TSPoint = record
    x: double;
    y: double;
  end;

  TSRect = record
    Left, Top, Right, Bottom: double;
  end;

  TMapPalette = record
    Red: array [0 .. 255] of byte;
    Green: array [0 .. 255] of byte;
    Blue: array [0 .. 255] of byte;
  end;

  TColorMaps = record
    Identifier: string;
    UGRFile: string;
  end;

  pPixArray = ^TPixArray;
  TPixArray = array [0 .. 1279, 0 .. 1023, 0 .. 3] of integer;
  pPreviewPixArray = ^TPreviewPixArray;
  TPreviewPixArray = array [0 .. 159, 0 .. 119, 0 .. 3] of integer;
  TFileType = (ftIfs, ftFla, ftXML);

type // ?
  PLongintArray = ^TLongintArray;
  TLongintArray = array [0 .. 8192] of Longint;

type
  TVariation = (vLinear, vSinusoidal, vSpherical, vSwirl, vHorseshoe, vPolar,
    vHandkerchief, vHeart, vDisc, vSpiral, vHyperbolic, vSquare, vEx, vJulia,
    vBent, vWaves, vFisheye, vPopcorn, vExponential, vPower, vCosine, vRings,
    vFan, vRandom);

type
  TPointsArray = array of TCPpoint;
  TPointsXYArray = array of TXYpoint;

  P2Cpoint = ^T2Cpoint;
  T2CPointsArray = array of T2Cpoint;

  TControlPoint = class
  public
    finalXform: TXForm;
    finalXformEnabled: boolean;
    useFinalXform: boolean;
    soloXform: integer;

    Transparency: boolean;

    cameraPitch, cameraYaw, cameraPersp, cameraDOF: double;
    cameraZpos: double;
    ProjectionFunc: procedure(pPoint: PCPPoint) of object;

    XForm: array [0 .. NXFORMS] of TXForm;

    noLinearFix: boolean;
    variation: TVariation;
    Cmap: TColorMap;
    cmapindex: integer;
    time: double;
    Fbrightness: double;
    contrast: double;
    gamma: double;
    Width: integer;
    Height: integer;
    spatial_oversample: integer;
    name, nick, url: string;
    center: array [0 .. 1] of double;
    vibrancy: double;
    hue_rotation: double;
    background: array [0 .. 3] of integer;
    zoom: double;
    pixels_per_unit: double;
    spatial_filter_radius: double;
    sample_density: extended;
    actual_density: extended;
    nbatches: integer;
    white_level: integer;
    cmap_inter: integer;
    symmetry: integer;
    pulse: array [0 .. 1, 0 .. 1] of double;
    wiggle: array [0 .. 1, 0 .. 1] of double;
    estimator, estimator_min, estimator_curve: double;
    jitters: integer;
    gamma_threshold: double;
    enable_de: boolean;
    used_plugins: TStringList;
    FAngle: double;
    FTwoColorDimensions: boolean;

    procedure FillUsedPlugins;

  private
    invalidXform: TXForm;

    CameraMatrix: array [0 .. 2, 0 .. 2] of double;
    DofCoef: double;
    gauss_rnd: array [0 .. 3] of double;
    gauss_N: integer;

    procedure ProjectNone(pPoint: PCPPoint);
    procedure ProjectPitch(pPoint: PCPPoint);
    procedure ProjectPitchYaw(pPoint: PCPPoint);
    procedure ProjectPitchDOF(pPoint: PCPPoint);
    procedure ProjectPitchYawDOF(pPoint: PCPPoint);

    function getppux: double;
    function getppuy: double;

    function GetBrightness: double;
    procedure SetBrightness(br: double);
    function GetRelativeGammaThreshold: double;
    procedure SetRelativeGammaThreshold(gtr: double);

  public
    xdata: string;

    procedure SaveToStringlist(sl: TStringList);
    procedure SaveToFile(Filename: string);

    procedure ParseString(aString: string);
    procedure ParseStringList(sl: TStringList);
    procedure RandomCP(min: integer = 2; max: integer = NXFORMS;
      calc: boolean = true);
    procedure RandomCP1;
    procedure CalcBoundbox;
    function BlowsUp(NrPoints: integer): boolean;

    procedure SetVariation(vari: TVariation);
    procedure Clear;

    procedure IterateXY(NrPoints: integer; var Points: TPointsXYArray);
    procedure IterateXYC(NrPoints: integer; var Points: TPointsArray);

    procedure Prepare;

    function Clone: TControlPoint;
    procedure Copy(cp1: TControlPoint; KeepSizes: boolean = false);

    function HasFinalXForm: boolean;

    function NumXForms: integer;
    function TrianglesFromCP(var Triangles: TTriangles): integer;
    procedure GetFromTriangles(const Triangles: TTriangles; const t: integer);

    procedure GetTriangle(var Triangle: TTriangle; const n: integer);
    procedure GetPostTriangle(var Triangle: TTriangle; const n: integer);

    procedure EqualizeWeights;
    procedure NormalizeWeights;
    procedure RandomizeWeights;
    procedure ComputeWeights(Triangles: TTriangles; t: integer);
    procedure AdjustScale(w, h: integer);

    constructor Create;
    destructor Destroy; override;

    procedure ZoomtoRect(R: TSRect);
    procedure ZoomOuttoRect(R: TSRect);
    procedure MoveRect(R: TSRect);
    procedure ZoomIn(Factor: double);
    procedure Rotate(Angle: double);

    property ppux: double read getppux;
    property ppuy: double read getppuy;

    property brightness: double read GetBrightness write SetBrightness;
    property gammaThreshRelative: double read GetRelativeGammaThreshold
      write SetRelativeGammaThreshold;
  end;

function add_symmetry_to_control_point(var cp: TControlPoint;
  sym: integer): integer;
function CalcUPRMagn(const cp: TControlPoint): double;
procedure FillVarDisturb;

implementation

uses global;

var
  var_distrib: array of integer;
  mixed_var_distrib: array of integer;

function sign(n: double): double;
begin
  if n < 0 then
    Result := -1
  else if n > 0 then
    Result := 1
  else
    Result := 0;
end;

procedure TControlPoint.FillUsedPlugins;
var
  i, j, k, f: integer;
  v: double;
  s: String;
begin
  used_plugins.Clear;

  f := -1;
  if self.finalXformEnabled then
    f := 0;

  for i := 0 to min(NumXForms + f, NXFORMS) do
    with XForm[i] do
    begin
      for j := 0 to NRVAR - 1 do
      begin
        v := self.XForm[i].GetVariation(j);
        if (v <> 0) and
          (used_plugins.IndexOf(Varnames(j)) < 0)
        then
        begin
          used_plugins.Add(Varnames(j));
          s := s + Varnames(j) + ' on TX #' + IntToStr(i + 1) + #13#10;
        end;
      end;
    end;
end;

constructor TControlPoint.Create;
var
  i: integer;
begin
  for i := 0 to NXFORMS do
  begin
    XForm[i] := TXForm.Create;
  end;
  invalidXform := TXForm.Create;
  soloXform := -1;

  pulse[0][0] := 0;
  pulse[0][1] := 60;
  pulse[1][0] := 0;
  pulse[1][1] := 60;

  wiggle[0][0] := 0;
  wiggle[0][1] := 60;
  wiggle[1][0] := 0;
  wiggle[1][1] := 60;

  background[0] := 0;
  background[1] := 0;
  background[2] := 0;

  center[0] := 0;
  center[1] := 0;

  pixels_per_unit := 50;

  Width := 100;
  Height := 100;

  spatial_oversample := 1;
  spatial_filter_radius := 0.5;

  FAngle := 0;
  gamma := 1;
  vibrancy := 1;
  contrast := 1;
  Fbrightness := 1;

  sample_density := 50;
  zoom := 0;
  nbatches := 1;

  white_level := 200;

  estimator := 9.0;
  estimator_min := 0.0;
  estimator_curve := 0.4;
  enable_de := false;
  jitters := 1;
  gamma_threshold := 0.01;

  FTwoColorDimensions := false;

  finalXformEnabled := false;
  Transparency := false;

  cameraPitch := 0;
  cameraYaw := 0;
  cameraPersp := 0;
  cameraZpos := 0;
  cameraDOF := 0;

  used_plugins := TStringList.Create;
  xdata := '';
end;

destructor TControlPoint.Destroy;
var
  i: integer;
begin
  for i := 0 to NXFORMS do
    XForm[i].Free;
  invalidXform.Free;
  inherited;
end;

procedure TControlPoint.Prepare;
var
  i, n: integer;
  propsum: double;
  LoopValue: double;
  j: integer;
  TotValue: double;

  k: integer;
  tp: array [0 .. NXFORMS] of double;
begin
  n := NumXForms;
  assert(n > 0);

  finalXform := XForm[n];
  finalXform.Prepare;
  finalXForm.symmetry := 1;

  useFinalXform := finalXformEnabled and HasFinalXForm;
  for i := 0 to n - 1 do
  begin
    XForm[i].Prepare;
  end;
  invalidXform.PrepareInvalidXForm;

  if soloXform >= 0 then
  begin
    for i := 0 to n - 1 do
    begin
      XForm[i].transOpacity := 0;
    end;
    XForm[soloXform].transOpacity := 1;
  end;

  for k := 0 to n - 1 do
  begin
    TotValue := 0;
    SetLength(XForm[k].PropTable, PROP_TABLE_SIZE);

    for i := 0 to n - 1 do
    begin
      tp[i] := XForm[i].density * XForm[k].modWeights[i];
      TotValue := TotValue + tp[i];
    end;

    if TotValue > 0 then
    begin
      LoopValue := 0;
      for i := 0 to PROP_TABLE_SIZE - 1 do
      begin
        propsum := 0;
        j := -1;
        repeat
          inc(j);
          propsum := propsum + tp[j];
        until (propsum > LoopValue) or (j = n - 1);

        XForm[k].PropTable[i] := XForm[j];
        LoopValue := LoopValue + TotValue / PROP_TABLE_SIZE;
      end;
    end
    else
    begin
      for i := 0 to PROP_TABLE_SIZE - 1 do
        XForm[k].PropTable[i] := invalidXform;
    end;
  end;

  CameraMatrix[0, 0] := cos(-cameraYaw);
  CameraMatrix[1, 0] := -sin(-cameraYaw);
  CameraMatrix[2, 0] := 0;
  CameraMatrix[0, 1] := cos(cameraPitch) * sin(-cameraYaw);
  CameraMatrix[1, 1] := cos(cameraPitch) * cos(-cameraYaw);
  CameraMatrix[2, 1] := -sin(cameraPitch);
  CameraMatrix[0, 2] := sin(cameraPitch) * sin(-cameraYaw);
  CameraMatrix[1, 2] := sin(cameraPitch) * cos(-cameraYaw);
  CameraMatrix[2, 2] := cos(cameraPitch);
  DofCoef := 0.1 * cameraDOF;
  gauss_rnd[0] := random;
  gauss_rnd[1] := random;
  gauss_rnd[2] := random;
  gauss_rnd[3] := random;
  gauss_N := 0;

  if (cameraDOF <> 0) then
  begin
    if (cameraYaw <> 0) then
      ProjectionFunc := ProjectPitchYawDOF
    else
      ProjectionFunc := ProjectPitchDOF;
  end
  else if (cameraPitch <> 0) or (cameraYaw <> 0) then
  begin
    if (cameraYaw <> 0) then
      ProjectionFunc := ProjectPitchYaw
    else
      ProjectionFunc := ProjectPitch;
  end
  else
    ProjectionFunc := ProjectNone;
end;

procedure TControlPoint.IterateXY(NrPoints: integer;
  var Points: TPointsXYArray);
var
  i: integer;
  px, py: double;
  pPoint: PXYPoint;

  xf: TXForm;
begin
  px := 2 * random - 1;
  py := 2 * random - 1;

  try
    xf := XForm[0];
    for i := 0 to FUSE do
    begin
      xf := xf.PropTable[random(PROP_TABLE_SIZE)];
      xf.NextPointXY(px, py);
    end;

    pPoint := @Points[0];

    if useFinalXform then
      for i := 0 to NrPoints - 1 do
      begin
        xf := xf.PropTable[random(PROP_TABLE_SIZE)];
        xf.NextPointXY(px, py);
        if (xf.transOpacity = 0) then
          pPoint^.x := MaxDouble
        else
        begin
          pPoint^.x := px;
          pPoint^.y := py;
        end;
        finalXform.NextPointXY(pPoint^.x, pPoint^.y);
        inc(pPoint);
      end
    else
      for i := 0 to NrPoints - 1 do
      begin
        xf := xf.PropTable[random(PROP_TABLE_SIZE)];
        xf.NextPointXY(px, py);
        if (xf.transOpacity = 0) then
          pPoint^.x := MaxDouble
        else
        begin
          pPoint.x := px;
          pPoint.y := py;
        end;
        inc(pPoint);
      end;
  except
    on EMathError do
    begin
      exit;
    end;
  end;
end;

procedure TControlPoint.IterateXYC(NrPoints: integer; var Points: TPointsArray);
var
  i: integer;
  p: TCPpoint;
  pPoint: PCPPoint;
  depth: double;

  xf: TXForm;
begin
  p.x := 2 * random - 1;
  p.y := 2 * random - 1;
  p.c := random;

  try
    xf := XForm[0];
    for i := 0 to FUSE do
    begin
      xf := xf.PropTable[random(PROP_TABLE_SIZE)];
      xf.NextPoint(p);
    end;

    pPoint := @Points[0];

    if useFinalXform then
      for i := 0 to NrPoints - 1 do
      begin
        xf := xf.PropTable[random(PROP_TABLE_SIZE)];
        xf.NextPoint(p);

        finalXform.NextPointTo(p, pPoint^);

        ProjectionFunc(pPoint);
        pPoint^.o := xf.transOpacity;
        inc(pPoint);
      end
    else
      for i := 0 to NrPoints - 1 do
      begin
        xf := xf.PropTable[random(PROP_TABLE_SIZE)];
        xf.NextPoint(p);

        pPoint^ := p;
        ProjectionFunc(pPoint);
        pPoint^.o := xf.transOpacity;
        inc(pPoint);
      end;
  except
    on EMathError do
    begin
      exit;
    end;
  end;
end;

procedure TControlPoint.ProjectNone(pPoint: PCPPoint);
var
  zr: double;
begin
  zr := 1 - cameraPersp * (pPoint^.z - cameraZpos);

  pPoint^.x := pPoint^.x / zr;
  pPoint^.y := pPoint^.y / zr;
  pPoint^.z := pPoint^.z - cameraZpos;
end;

procedure TControlPoint.ProjectPitch(pPoint: PCPPoint);
var
  y, z, zr: double;
begin
  z := pPoint^.z - cameraZpos;
  y := CameraMatrix[1, 1] * pPoint^.y + CameraMatrix[2, 1] * z;
  zr := 1 - cameraPersp * (CameraMatrix[1, 2] * pPoint^.y + CameraMatrix
    [2, 2] * z);

  pPoint^.x := pPoint^.x / zr;
  pPoint^.y := y / zr;
  pPoint^.z := pPoint^.z - cameraZpos;
end;

procedure TControlPoint.ProjectPitchYaw(pPoint: PCPPoint);
var
  x, y, z, zr: double;
begin
  z := pPoint^.z - cameraZpos;
  x := CameraMatrix[0, 0] * pPoint^.x + CameraMatrix[1, 0] * pPoint^.y;
  y := CameraMatrix[0, 1] * pPoint^.x + CameraMatrix[1, 1] * pPoint^.y +
    CameraMatrix[2, 1] * z;
  zr := 1 - cameraPersp * (CameraMatrix[0, 2] * pPoint^.x + CameraMatrix[1, 2] *
    pPoint^.y + CameraMatrix[2, 2] * z);

  pPoint^.x := x / zr;
  pPoint^.y := y / zr;
  pPoint^.z := pPoint^.z - cameraZpos;
end;

procedure TControlPoint.ProjectPitchDOF(pPoint: PCPPoint);
var
  x, y, z, zr, dr: double;
  dsin, dcos: double;
  t: double;
begin
  z := pPoint^.z - cameraZpos;
  y := CameraMatrix[1, 1] * pPoint^.y + CameraMatrix[2, 1] * z;
  z := CameraMatrix[1, 2] * pPoint^.y + CameraMatrix[2, 2] * z;
  zr := 1 - cameraPersp * z;

{$IFDEF GAUSSIAN_DOF}
  asm
    fld     qword ptr [eax + gauss_rnd]
    fadd    qword ptr [eax + gauss_rnd+8]
    fadd    qword ptr [eax + gauss_rnd+16]
    fadd    qword ptr [eax + gauss_rnd+24]
    fld1
    fadd    st, st
    fsubp   st(1),st
    fmul    qword ptr [eax + dofCoef]
    fmul    qword ptr [z]
    fstp    qword ptr [dr]
    call    System.@RandExt
    mov     edx, [eax + gauss_N]
    fst     qword ptr [eax + gauss_rnd + edx*8]
    inc     edx
    and     edx,$03
    mov     [eax + gauss_N], edx
    fadd    st, st
    fldpi
    fmulp
    fsincos
    fstp    qword ptr [dcos]
    fstp    qword ptr [dsin]
  end;
{$ELSE}
  t := random * 2 * pi;
  dsin := sin(t);
  dcos := cos(t);
  dr := random * DofCoef * z;
{$ENDIF}

  pPoint^.x := (pPoint^.x + dr * dcos) / zr;
  pPoint^.y := (y + dr * dsin) / zr;
  pPoint^.z := pPoint^.z - cameraZpos;
end;

procedure TControlPoint.ProjectPitchYawDOF(pPoint: PCPPoint);
var
  x, y, z, zr, dr: double;
  dsin, dcos: double;
  t: double;
begin
  z := pPoint^.z - cameraZpos;
  x := CameraMatrix[0, 0] * pPoint^.x + CameraMatrix[1, 0] * pPoint^.y;
  y := CameraMatrix[0, 1] * pPoint^.x + CameraMatrix[1, 1] * pPoint^.y +
    CameraMatrix[2, 1] * z;
  z := CameraMatrix[0, 2] * pPoint^.x + CameraMatrix[1, 2] * pPoint^.y +
    CameraMatrix[2, 2] * z;
  zr := 1 - cameraPersp * z;

{$IFDEF GAUSSIAN_DOF}
  asm
    fld     qword ptr [eax + gauss_rnd]
    fadd    qword ptr [eax + gauss_rnd+8]
    fadd    qword ptr [eax + gauss_rnd+16]
    fadd    qword ptr [eax + gauss_rnd+24]
    fld1
    fadd    st, st
    fsubp   st(1),st
    fmul    qword ptr [eax + dofCoef]
    fmul    qword ptr [z]
    fstp    qword ptr [dr]
    call    System.@RandExt
    mov     edx, [eax + gauss_N]
    fst     qword ptr [eax + gauss_rnd + edx*8]
    inc     edx
    and     edx,$03
    mov     [eax + gauss_N], edx
    fadd    st, st
    fldpi
    fmulp
    fsincos
    fstp    qword ptr [dcos]
    fstp    qword ptr [dsin]
  end;
{$ELSE}
  t := random * 2 * pi;
  dsin := sin(t);
  dcos := cos(t);
  dr := random * DofCoef * z;
{$ENDIF}
  pPoint^.x := (x + dr * dcos) / zr;
  pPoint^.y := (y + dr * dsin) / zr;
  pPoint^.z := pPoint^.z - cameraZpos;
end;

function TControlPoint.BlowsUp(NrPoints: integer): boolean;
var
  i, n: integer;
  px, py: double;
  minx, maxx, miny, maxy: double;
  Points: TPointsXYArray;
  CurrentPoint: PXYPoint;

  xf: TXForm;
begin
  Result := false;

  n := min(SUB_BATCH_SIZE, NrPoints);
  SetLength(Points, n);

  px := 2 * random - 1;
  py := 2 * random - 1;

  Prepare;

  try
    xf := XForm[random(NumXForms)];
    for i := 0 to FUSE do
    begin
      xf := xf.PropTable[random(PROP_TABLE_SIZE)];
      xf.NextPointXY(px, py);
    end;

    CurrentPoint := @Points[0];
    for i := 0 to n - 1 do
    begin
      xf := xf.PropTable[random(PROP_TABLE_SIZE)];
      xf.NextPointXY(px, py);
      CurrentPoint.x := px;
      CurrentPoint.y := py;
      inc(CurrentPoint);
    end;
  except
    on EMathError do
    begin
      Result := true;
      exit;
    end;
  end;

  minx := 1E10;
  maxx := -1E10;
  miny := 1E10;
  maxy := -1E10;
  for i := 0 to n - 1 do
  begin
    minx := min(minx, Points[i].x);
    maxx := max(maxx, Points[i].x);
    miny := min(miny, Points[i].y);
    maxy := max(maxy, Points[i].y);
  end;

  if ((maxx - minx) > 1000) or ((maxy - miny) > 1000) then
    Result := true;
end;

procedure TControlPoint.ParseString(aString: string);
var
  ParseValues: TStringList;
  ParsePos: integer;
  CurrentToken: string;
  CurrentXForm: integer;
  i: integer;
  OldDecimalSperator: Char;
  v: double;
begin
  ParseValues := TStringList.Create;
  ParseValues.CommaText := aString;

  OldDecimalSperator := FormatSettings.DecimalSeparator;
  FormatSettings.DecimalSeparator := '.';

  CurrentXForm := 0;

  ParsePos := 0;
  while (ParsePos < ParseValues.Count) do
  begin
    CurrentToken := ParseValues[ParsePos];
    if AnsiCompareText(CurrentToken, 'xform') = 0 then
    begin
      inc(ParsePos);
      CurrentXForm := StrToInt(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'finalxformenabled') = 0 then
    begin
      inc(ParsePos);
      finalXformEnabled := StrToInt(ParseValues[ParsePos]) <> 0;
    end
    else if AnsiCompareText(CurrentToken, 'soloxform') = 0 then
    begin
      inc(ParsePos);
      soloXform := StrToInt(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'time') = 0 then
    begin
      inc(ParsePos);
      time := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'brightness') = 0 then
    begin
      inc(ParsePos);
      brightness := StrToFloat(ParseValues[ParsePos]) / BRIGHT_ADJUST;
    end
    else if AnsiCompareText(CurrentToken, 'zoom') = 0 then
    begin // mt
      inc(ParsePos); // mt
      zoom := StrToFloat(ParseValues[ParsePos]); // mt
    end
    else if AnsiCompareText(CurrentToken, 'angle') = 0 then
    begin
      inc(ParsePos);
      FAngle := StrToFloat(ParseValues[ParsePos]);
      // 3d camera stuff
    end
    else if AnsiCompareText(CurrentToken, 'cam_pitch') = 0 then
    begin
      inc(ParsePos);
      cameraPitch := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'cam_yaw') = 0 then
    begin
      inc(ParsePos);
      cameraYaw := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'cam_persp') = 0 then
    begin
      inc(ParsePos);
      cameraPersp := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'cam_zpos') = 0 then
    begin
      inc(ParsePos);
      cameraZpos := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'cam_dof') = 0 then
    begin
      inc(ParsePos);
      cameraDOF := abs(StrToFloat(ParseValues[ParsePos]));
      // end 3d
    end
    else if AnsiCompareText(CurrentToken, 'contrast') = 0 then
    begin
      inc(ParsePos);
      contrast := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'gamma') = 0 then
    begin
      inc(ParsePos);
      gamma := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'vibrancy') = 0 then
    begin
      inc(ParsePos);
      vibrancy := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'gamma_threshold') = 0 then
    begin
      inc(ParsePos);
      gamma_threshold := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'hue_rotation') = 0 then
    begin
      inc(ParsePos);
      hue_rotation := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'zoom') = 0 then
    begin
      inc(ParsePos);
      zoom := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'imagesize') = 0 then
    begin
      inc(ParsePos);
      Width := StrToInt(ParseValues[ParsePos]);
      inc(ParsePos);
      Height := StrToInt(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'image_size') = 0 then
    begin
      inc(ParsePos);
      Width := StrToInt(ParseValues[ParsePos]);
      inc(ParsePos);
      Height := StrToInt(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'center') = 0 then
    begin
      inc(ParsePos);
      center[0] := StrToFloat(ParseValues[ParsePos]);
      inc(ParsePos);
      center[1] := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'background') = 0 then
    begin
      inc(ParsePos);
      // Trap conversion errors for older parameters
      try
        background[0] := StrToInt(ParseValues[ParsePos]);
      except
        on EConvertError do
          background[0] := 0;
      end;
      inc(ParsePos);
      try
        background[1] := StrToInt(ParseValues[ParsePos]);
      except
        on EConvertError do
          background[1] := 0;
      end;
      inc(ParsePos);
      try
        background[2] := StrToInt(ParseValues[ParsePos]);
      except
        on EConvertError do
          background[2] := 0;
      end;
    end
    else if AnsiCompareText(CurrentToken, 'pulse') = 0 then
    begin
      inc(ParsePos);
      pulse[0, 0] := StrToFloat(ParseValues[ParsePos]);
      inc(ParsePos);
      pulse[0, 1] := StrToFloat(ParseValues[ParsePos]);
      inc(ParsePos);
      pulse[1, 0] := StrToFloat(ParseValues[ParsePos]);
      inc(ParsePos);
      pulse[1, 1] := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'wiggle') = 0 then
    begin
      inc(ParsePos);
      wiggle[0, 0] := StrToFloat(ParseValues[ParsePos]);
      inc(ParsePos);
      wiggle[0, 1] := StrToFloat(ParseValues[ParsePos]);
      inc(ParsePos);
      wiggle[1, 0] := StrToFloat(ParseValues[ParsePos]);
      inc(ParsePos);
      wiggle[1, 1] := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'pixels_per_unit') = 0 then
    begin
      inc(ParsePos);
      pixels_per_unit := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'spatial_filter_radius') = 0 then
    begin
      inc(ParsePos);
      spatial_filter_radius := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'spatial_oversample') = 0 then
    begin
      inc(ParsePos);
      spatial_oversample := StrToInt(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'sample_density') = 0 then
    begin
      inc(ParsePos);
      sample_density := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'nbatches') = 0 then
    begin
      inc(ParsePos);
      nbatches := StrToInt(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'white_level') = 0 then
    begin
      inc(ParsePos);
      white_level := StrToInt(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'cmap') = 0 then
    begin
      inc(ParsePos);
      cmapindex := StrToInt(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'cmap_inter') = 0 then
    begin
      inc(ParsePos);
      cmap_inter := StrToInt(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'palette') = 0 then
    begin
      // Inc(ParsePos);
      // cmapindex := StrToInt(ParseValues[ParsePos]);
      OutputDebugString(Pchar('NYI import Palette'));
    end
    else if AnsiCompareText(CurrentToken, 'density') = 0 then
    begin
      inc(ParsePos);
      XForm[CurrentXForm].density := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'color') = 0 then
    begin
      inc(ParsePos);
      XForm[CurrentXForm].color := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'symmetry') = 0 then
    begin
      inc(ParsePos);
      XForm[CurrentXForm].symmetry := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'coefs') = 0 then
    begin
      inc(ParsePos);
      XForm[CurrentXForm].c[0, 0] := StrToFloat(ParseValues[ParsePos]);
      inc(ParsePos);
      XForm[CurrentXForm].c[0, 1] := StrToFloat(ParseValues[ParsePos]);
      inc(ParsePos);
      XForm[CurrentXForm].c[1, 0] := StrToFloat(ParseValues[ParsePos]);
      inc(ParsePos);
      XForm[CurrentXForm].c[1, 1] := StrToFloat(ParseValues[ParsePos]);
      inc(ParsePos);
      XForm[CurrentXForm].c[2, 0] := StrToFloat(ParseValues[ParsePos]);
      inc(ParsePos);
      XForm[CurrentXForm].c[2, 1] := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'post') = 0 then
    begin
      inc(ParsePos);
      XForm[CurrentXForm].p[0, 0] := StrToFloat(ParseValues[ParsePos]);
      inc(ParsePos);
      XForm[CurrentXForm].p[0, 1] := StrToFloat(ParseValues[ParsePos]);
      inc(ParsePos);
      XForm[CurrentXForm].p[1, 0] := StrToFloat(ParseValues[ParsePos]);
      inc(ParsePos);
      XForm[CurrentXForm].p[1, 1] := StrToFloat(ParseValues[ParsePos]);
      inc(ParsePos);
      XForm[CurrentXForm].p[2, 0] := StrToFloat(ParseValues[ParsePos]);
      inc(ParsePos);
      XForm[CurrentXForm].p[2, 1] := StrToFloat(ParseValues[ParsePos]);
    end
    else if AnsiCompareText(CurrentToken, 'postxswap') = 0 then
    begin
      inc(ParsePos);
      XForm[CurrentXForm].postXswap := (ParseValues[ParsePos] = '1');
    end
    else if AnsiCompareText(CurrentToken, 'autozscale') = 0 then
    begin
      inc(ParsePos);
      XForm[CurrentXForm].autoZscale := (ParseValues[ParsePos] = '1');
    end
    else if AnsiCompareText(CurrentToken, 'vars') = 0 then
    begin
      for i := 0 to NRVAR - 1 do
      begin
        XForm[CurrentXForm].SetVariation(i, 0.0);
      end;

      i := 0;
      while true do
      begin
        if (ParsePos + 1) >= ParseValues.Count then
          break;
        if CharInSet(ParseValues[ParsePos + 1][1], ['a' .. 'z', 'A' .. 'Z'])
        then
          break;

        inc(ParsePos);
        XForm[CurrentXForm].SetVariation(i, StrToFloat(ParseValues[ParsePos]));
        inc(i);
      end;
    end
    else if AnsiCompareText(CurrentToken, 'variables') = 0 then
    begin
      for i := 0 to GetNrVariableNames - 1 do
      begin
        XForm[CurrentXForm].ResetVariable(GetVariableNameAt(i));
      end;

      i := 0;
      while true do
      begin
        if (ParsePos + 1) >= ParseValues.Count then
          break;
        if CharInSet(ParseValues[ParsePos + 1][1], ['a' .. 'z', 'A' .. 'Z'])
        then
          break;

        inc(ParsePos);
        v := StrToFloat(ParseValues[ParsePos]);
        XForm[CurrentXForm].SetVariable(GetVariableNameAt(i), v);
        inc(i);
      end;

    end
    else if AnsiCompareText(CurrentToken, 'chaos') = 0 then
    begin
      i := 0;
      while true do
      begin
        if (ParsePos + 1) >= ParseValues.Count then
          break;
        if CharInSet(ParseValues[ParsePos + 1][1], ['a' .. 'z', 'A' .. 'Z'])
        then
          break;

        inc(ParsePos);
        v := StrToFloat(ParseValues[ParsePos]);
        XForm[CurrentXForm].modWeights[i] := v;
        inc(i);
      end;

    end
    else if AnsiCompareText(CurrentToken, 'plotmode') = 0 then
    begin
      inc(ParsePos);
      if ((StrToInt(ParseValues[ParsePos]) = 1)) then
        XForm[CurrentXForm].transOpacity := 0;
    end
    else if AnsiCompareText(CurrentToken, 'opacity') = 0 then
    begin
      inc(ParsePos);
      XForm[CurrentXForm].transOpacity := (StrToFloat(ParseValues[ParsePos]));
    end
    else if AnsiCompareText(CurrentToken, 'var_color') = 0 then
    begin
      inc(ParsePos);
      XForm[CurrentXForm].pluginColor := (StrToFloat(ParseValues[ParsePos]));
    end
    else
    begin
      OutputDebugString(Pchar('Unknown Token: ' + CurrentToken));
    end;

    inc(ParsePos);
  end;
  GetIntegratedPaletteByIndex(cmapindex, Cmap);

  ParseValues.Free;

  FormatSettings.DecimalSeparator := OldDecimalSperator;
end;

procedure TControlPoint.SetVariation(vari: TVariation);
var
  i, j, v: integer;
  rv: integer;
  VarPossible: boolean;
begin
  FillVarDisturb;
  VarPossible := false;
  for j := 0 to NRVAR - 1 do
  begin
    VarPossible := VarPossible or Variations[j];
  end;

  if VarPossible then
  begin
    repeat
      rv := var_distrib[random(Length(var_distrib))];
    until Variations[rv];
  end
  else
  begin
    rv := 0;
  end;

  for i := 0 to NXFORMS - 1 do
  begin
    for j := 0 to NRVAR - 1 do
    begin
      XForm[i].SetVariation(j, 0.0);
    end;

    if vari = vRandom then
    begin
      if rv < 0 then
      begin
        if VarPossible then
        begin
          repeat
            v := mixed_var_distrib[random(Length(mixed_var_distrib))];
          until Variations[v]; // Use only Variations set in options
        end
        else
        begin
          v := 0;
        end;
        XForm[i].SetVariation(v, 1.0);
      end
      else
        XForm[i].SetVariation(rv, 1.0);
    end
    else
      XForm[i].SetVariation(integer(vari), 1);
  end;
end;

procedure TControlPoint.RandomCP(min: integer = 2; max: integer = NXFORMS;
  calc: boolean = true);
var
  nrXforms: integer;
  i, j: integer;
  v, rv: integer;
  VarPossible: boolean;
begin
  // hue_rotation := random;
  hue_rotation := 1;
  cmapindex := RANDOMCMAP;
  GetIntegratedPaletteByIndex(cmapindex, Cmap);
  time := 0.0;

  // nrXforms := xform_distrib[random(13)];
  nrXforms := random(max - (min - 1)) + min;

  FillVarDisturb;
  VarPossible := false;
  for j := 0 to NRVAR - 1 do
  begin
    VarPossible := VarPossible or Variations[j];
  end;

  if VarPossible then
  begin
    repeat
      rv := var_distrib[random(Length(var_distrib))];
    until Variations[rv];
  end
  else
  begin
    rv := 0;
  end;

  for i := 0 to NXFORMS - 1 do
  begin
    XForm[i].density := 0;
  end;

  for i := 0 to nrXforms - 1 do
  begin
    XForm[i].density := 1.0 / nrXforms;
    XForm[i].color := i / (nrXforms - 1);

    XForm[i].c[0][0] := 2 * random - 1;
    XForm[i].c[0][1] := 2 * random - 1;
    XForm[i].c[1][0] := 2 * random - 1;
    XForm[i].c[1][1] := 2 * random - 1;
    XForm[i].c[2][0] := 4 * random - 2;
    XForm[i].c[2][1] := 4 * random - 2;

    for j := 0 to NRVAR - 1 do
    begin
      XForm[i].SetVariation(j, 0);
    end;

    for j := 0 to NRVAR - 1 do
    begin
      XForm[i].SetVariation(j, 0);
    end;

    if rv < 0 then
    begin
      if VarPossible then
      begin
        repeat
          v := mixed_var_distrib[random(Length(mixed_var_distrib))];
        until Variations[v]; // use only variations set in options
      end
      else
      begin
        v := 0;
      end;

      XForm[i].SetVariation(v, 1);
    end
    else
      XForm[i].SetVariation(rv, 1);

  end;
  if calc then
    CalcBoundbox;
end;

procedure TControlPoint.RandomCP1;
var
  i, j: integer;
begin
  RandomCP;
  for i := 0 to NXFORMS - 1 do
  begin
    for j := 0 to NRVAR - 1 do
    begin
      XForm[i].SetVariation(j, 0);
    end;
    XForm[i].SetVariation(0, 1);
  end;

  CalcBoundbox;
end;

procedure TControlPoint.CalcBoundbox;
var
  Points: TPointsXYArray;
  i, j: integer;
  deltax, minx, maxx: double;
  cntminx, cntmaxx: integer;
  deltay, miny, maxy: double;
  cntminy, cntmaxy: integer;
  LimitOutSidePoints: integer;
  px, py, sina, cosa: double;
begin
{$IFDEF TESTVARIANT}
  center[0] := 0;
  center[1] := 0;
  pixels_per_unit := 0.7 * min(Width / (6), Height / (6));
  exit;
{$ENDIF}
  // RandSeed := 1234567;
  try
    SetLength(Points, SUB_BATCH_SIZE);
    cosa := cos(FAngle);
    sina := sin(FAngle);

    Prepare;

    IterateXY(SUB_BATCH_SIZE, Points);

    LimitOutSidePoints := Round(0.05 * SUB_BATCH_SIZE);

    minx := 1E99;
    maxx := -1E99;
    miny := 1E99;
    maxy := -1E99;
    for i := 0 to SUB_BATCH_SIZE - 1 do
    begin
      if Points[i].x > 1E200 then
        continue;
      minx := min(minx, Points[i].x);
      maxx := max(maxx, Points[i].x);
      miny := min(miny, Points[i].y);
      maxy := max(maxy, Points[i].y);
    end;

    deltax := (maxx - minx) * 0.25;
    maxx := (maxx + minx) / 2;
    minx := maxx;

    deltay := (maxy - miny) * 0.25;
    maxy := (maxy + miny) / 2;
    miny := maxy;

    for j := 0 to 10 do
    begin
      cntminx := 0;
      cntmaxx := 0;
      cntminy := 0;
      cntmaxy := 0;
      for i := 0 to SUB_BATCH_SIZE - 1 do
      begin
        if Points[i].x > 1E200 then
          continue;
        px := Points[i].x * cosa + Points[i].y * sina;
        py := Points[i].y * cosa - Points[i].x * sina;
        if (Points[i].x < minx) then
          inc(cntminx);
        if (Points[i].x > maxx) then
          inc(cntmaxx);
        if (Points[i].y < miny) then
          inc(cntminy);
        if (Points[i].y > maxy) then
          inc(cntmaxy);
      end;

      if (cntminx < LimitOutSidePoints) then
      begin
        minx := minx + deltax;
      end
      else
      begin
        minx := minx - deltax;
      end;

      if (cntmaxx < LimitOutSidePoints) then
      begin
        maxx := maxx - deltax;
      end
      else
      begin
        maxx := maxx + deltax;
      end;

      deltax := deltax / 2;

      if (cntminy < LimitOutSidePoints) then
      begin
        miny := miny + deltay;
      end
      else
      begin
        miny := miny - deltay;
      end;

      if (cntmaxy < LimitOutSidePoints) then
      begin
        maxy := maxy - deltay;
      end
      else
      begin
        maxy := maxy + deltay;
      end;

      deltay := deltay / 2;
    end;

    if ((maxx - minx) > 1000) or ((maxy - miny) > 1000) then
      raise EMathError.Create('Flame area too large');

    center[0] := (minx + maxx) / 2;
    center[1] := (miny + maxy) / 2;
    if ((maxx - minx) > 0.001) and ((maxy - miny) > 0.001) then
      pixels_per_unit := 0.65 * min(Width / (maxx - minx),
        Height / (maxy - miny))
    else
      pixels_per_unit := 10;
  except
    on E: EMathError do
    begin // default
      center[0] := 0;
      center[1] := 0;
      pixels_per_unit := 10;
    end;
  end;
end;

function CalcUPRMagn(const cp: TControlPoint): double;
var
  Points: TPointsXYArray;
  i, j: integer;
  deltax, minx, maxx: double;
  cntminx, cntmaxx: integer;
  deltay, miny, maxy: double;
  cntminy, cntmaxy: integer;
  LimitOutSidePoints: integer;
  xLength, yLength: double;
begin
  try
    SetLength(Points, SUB_BATCH_SIZE);
    cp.IterateXY(SUB_BATCH_SIZE, Points);

    LimitOutSidePoints := Round(0.05 * SUB_BATCH_SIZE);

    minx := 1E99;
    maxx := -1E99;
    miny := 1E99;
    maxy := -1E99;
    for i := 0 to SUB_BATCH_SIZE - 1 do
    begin
      minx := min(minx, Points[i].x);
      maxx := max(maxx, Points[i].x);
      miny := min(miny, Points[i].y);
      maxy := max(maxy, Points[i].y);
    end;

    deltax := (maxx - minx) * 0.25;
    maxx := (maxx + minx) / 2;
    minx := maxx;

    deltay := (maxy - miny) * 0.25;
    maxy := (maxy + miny) / 2;
    miny := maxy;

    for j := 0 to 10 do
    begin
      cntminx := 0;
      cntmaxx := 0;
      cntminy := 0;
      cntmaxy := 0;
      for i := 0 to SUB_BATCH_SIZE - 1 do
      begin
        if (Points[i].x < minx) then
          inc(cntminx);
        if (Points[i].x > maxx) then
          inc(cntmaxx);
        if (Points[i].y < miny) then
          inc(cntminy);
        if (Points[i].y > maxy) then
          inc(cntmaxy);
      end;

      if (cntminx < LimitOutSidePoints) then
      begin
        minx := minx + deltax;
      end
      else
      begin
        minx := minx - deltax;
      end;

      if (cntmaxx < LimitOutSidePoints) then
      begin
        maxx := maxx - deltax;
      end
      else
      begin
        maxx := maxx + deltax;
      end;

      deltax := deltax / 2;

      if (cntminy < LimitOutSidePoints) then
      begin
        miny := miny + deltay;
      end
      else
      begin
        miny := miny - deltay;
      end;

      if (cntmaxy < LimitOutSidePoints) then
      begin
        maxy := maxy - deltay;
      end
      else
      begin
        maxy := maxy + deltay;
      end;

      deltay := deltay / 2;
    end;

    if ((maxx - minx) > 1000) or ((maxy - miny) > 1000) then
      raise EMathError.Create('Flame area too large');

    cp.center[0] := (minx + maxx) / 2;
    cp.center[1] := (miny + maxy) / 2;
    if ((maxx - minx) > 0.001) and ((maxy - miny) > 0.001) then
      cp.pixels_per_unit := 0.7 * min(cp.Width / (maxx - minx),
        cp.Height / (maxy - miny))
    else
      cp.pixels_per_unit := 10;

    // Calculate magn for UPRs
    xLength := maxx - minx;
    yLength := maxy - miny;
    if xLength >= yLength then
    begin
      Result := 1 / xLength * 2;
    end
    else
    begin
      Result := 1 / yLength * 2;
    end;

  except
    on E: EMathError do
    begin // default
      cp.center[0] := 0;
      cp.center[1] := 0;
      cp.pixels_per_unit := 10;
      raise Exception.Create('CalcUPRMagn: ' + E.Message);
    end;
  end;
end;

procedure TControlPoint.SaveToFile(Filename: string);
var
  sl: TStringList;
begin
  sl := TStringList.Create;

  SaveToStringlist(sl);

  sl.SaveToFile(Filename);
  sl.Free;
end;

procedure TControlPoint.SaveToStringlist(sl: TStringList);
var
  i, j, k: integer;
  s: string;
  OldDecimalSperator: Char;
  v: double;
  str: string;
begin
  OldDecimalSperator := FormatSettings.DecimalSeparator;
  FormatSettings.DecimalSeparator := '.';

  sl.Add(format('time %f', [time]));
  if cmapindex >= 0 then
    sl.Add(format('cmap %d', [cmapindex]));
  sl.Add(format('zoom %g', [zoom])); // mt
  sl.Add(format('angle %g', [FAngle]));

  sl.Add(format('cam_pitch %g', [cameraPitch]));
  sl.Add(format('cam_yaw %g', [cameraYaw]));
  sl.Add(format('cam_persp %g', [cameraPersp]));
  sl.Add(format('cam_zpos %g', [cameraZpos]));
  sl.Add(format('cam_dof %g', [cameraDOF]));

  sl.Add(format('image_size %d %d center %g %g pixels_per_unit %f',
    [Width, Height, center[0], center[1], pixels_per_unit]));
  sl.Add(format('spatial_oversample %d spatial_filter_radius %f',
    [spatial_oversample, spatial_filter_radius]));
  sl.Add(format('sample_density %g', [sample_density]));
  // sl.add(format('nbatches %d white_level %d background %f %f %f', - changed to integers - mt
  sl.Add(format('nbatches %d white_level %d background %d %d %d',
    [nbatches, white_level, background[0], background[1], background[2]]));
  sl.Add(format
    ('brightness %f gamma %f vibrancy %f gamma_threshold %f hue_rotation %f cmap_inter %d',
    [Fbrightness * BRIGHT_ADJUST, gamma, vibrancy, gamma_threshold,
    hue_rotation, cmap_inter]));
  sl.Add(format('finalxformenabled %d', [ifthen(finalXformEnabled, 1, 0)]));
  sl.Add(format('soloxform %d', [soloXform]));

  for i := 0 to min(NumXForms + 1, NXFORMS) do
    with XForm[i] do
    begin
      sl.Add(format('xform %d density %g color %g symmetry %g',
        [i, density, color, symmetry]));
      s := 'vars';
      for j := 0 to NRVAR - 1 do
      begin
        s := format('%s %g', [s, GetVariation(j)]);
      end;
      sl.Add(s);
      s := 'variables';
      for j := 0 to GetNrVariableNames - 1 do
      begin
{$IFNDEF VAR_STR}
        GetVariable(GetVariableNameAt(j), v);
        s := format('%s %g', [s, v]);
{$ELSE}
        s := s + ' ' + GetVariableStr(GetVariableNameAt(j));
{$ENDIF}
      end;
      sl.Add(s);
      sl.Add(format('coefs %.6f %.6f %.6f %.6f %.6f %.6f', [c[0][0], c[0][1],
        c[1][0], c[1][1], c[2][0], c[2][1]]));
      sl.Add(format('post %.6f %.6f %.6f %.6f %.6f %.6f', [p[0][0], p[0][1],
        p[1][0], p[1][1], p[2][0], p[2][1]]));
      if postXswap then
        sl.Add('postxswap 1')
      else
        sl.Add('postxswap 0');
      if autoZscale then
        sl.Add('autozscale 1')
      else
        sl.Add('autozscale 0');
      s := 'chaos';
      for j := 0 to NumXForms + 1 do
      begin
        s := s + format(' %g', [modWeights[j]]);
      end;
      sl.Add(s);

      sl.Add(format('opacity %g', [transOpacity]));
      sl.Add(format('var_color %g', [pluginColor]));

    end;
  FormatSettings.DecimalSeparator := OldDecimalSperator;
end;

function TControlPoint.Clone: TControlPoint;
var
  i, j: integer;
  sl: TStringList;
begin
  sl := TStringList.Create;
  SaveToStringlist(sl);
  Result := TControlPoint.Create;
  Result.ParseStringList(sl);
  Result.FAngle := FAngle;
  Result.Cmap := Cmap;
  Result.name := name;
  Result.nick := nick;
  Result.url := url;
  Result.Transparency := Transparency;
  Result.gamma_threshold := gamma_threshold;
  Result.estimator := estimator;
  Result.estimator_min := estimator_min;
  Result.estimator_curve := estimator_curve;
  Result.enable_de := enable_de;
  Result.xdata := xdata;

  Result.background[0] := background[0];
  Result.background[1] := background[1];
  Result.background[2] := background[2];

  Result.used_plugins.Clear;
  for i := 0 to used_plugins.Count - 1 do
    Result.used_plugins.Add(used_plugins[i]);

  for i := 0 to NXFORMS - 1 do
    Result.XForm[i].Assign(XForm[i]);

  sl.Free;
end;

procedure TControlPoint.Copy(cp1: TControlPoint; KeepSizes: boolean = false);
var
  i, j: integer;
  sl: TStringList;
  w, h: integer;
begin
  w := Width;
  h := Height;

  Clear;
  sl := TStringList.Create;

  // --Z-- this is quite a weird and unoptimal way to copy things:
  cp1.SaveToStringlist(sl);
  ParseStringList(sl);

  FAngle := cp1.FAngle;
  center[0] := cp1.center[0];
  center[1] := cp1.center[1];
  pixels_per_unit := cp1.pixels_per_unit;
  Cmap := cp1.Cmap;
  name := cp1.name;
  nick := cp1.nick;
  url := cp1.url;
  gamma_threshold := cp1.gamma_threshold;
  estimator := cp1.estimator;
  estimator_min := cp1.estimator_min;
  estimator_curve := cp1.estimator_curve;
  enable_de := cp1.enable_de;
  used_plugins := cp1.used_plugins;
  xdata := cp1.xdata;

  background[0] := cp1.background[0];
  background[1] := cp1.background[1];
  background[2] := cp1.background[2];

  if KeepSizes then
    AdjustScale(w, h);

  used_plugins.Clear;
  for i := 0 to cp1.used_plugins.Count - 1 do
    used_plugins.Add(cp1.used_plugins[i]);

  for i := 0 to NXFORMS do // was: NXFORMS-1
    XForm[i].Assign(cp1.XForm[i]);

  finalXformEnabled := cp1.finalXformEnabled;

  sl.Free;
end;

procedure TControlPoint.ParseStringList(sl: TStringList);
var
  s: string;
  i: integer;
begin
  finalXformEnabled := false;
  for i := 0 to sl.Count - 1 do
  begin
    s := s + sl[i] + ' ';
  end;
  ParseString(s);
end;

procedure TControlPoint.Clear;
var
  i, j: integer;
begin
  symmetry := 0;
  cmapindex := -1;
  zoom := 0;
  xdata := '';
  for i := 0 to NXFORMS do
    XForm[i].Clear;
  finalXformEnabled := false;
  soloXform := -1;

  try
    if (used_plugins <> nil) then
      used_plugins.Clear
    else
      used_plugins := TStringList.Create;
  except
    // hack
    used_plugins := TStringList.Create;
  end;
end;

function TControlPoint.HasFinalXForm: boolean;
var
  i: integer;
begin
  with XForm[NumXForms] do
  begin
    Result := (c[0, 0] <> 1) or (c[0, 1] <> 0) or (c[1, 0] <> 0) or
      (c[1, 1] <> 1) or (c[2, 0] <> 0) or (c[2, 1] <> 0) or (p[0, 0] <> 1) or
      (p[0, 1] <> 0) or (p[1, 0] <> 0) or (p[1, 1] <> 1) or (p[2, 0] <> 0) or
      (p[2, 1] <> 0) or (symmetry <> 1) or (GetVariation(0) <> 1);
    if Result = false then
    begin
      for i := 1 to NRVAR - 1 do
        Result := Result or (GetVariation(i) <> 0);
    end;
  end;
end;

function add_symmetry_to_control_point(var cp: TControlPoint;
  sym: integer): integer;
const
  sym_distrib: array [0 .. 14] of integer = (-4, -3, -2, -2, -2, -1, -1, -1, 2,
    2, 2, 3, 3, 4, 4);
var
  i, j, k: integer;
  a: double;
begin
  Result := 0;
  if (0 = sym) then
    if (random(1) <> 0) then
      sym := sym_distrib[random(14)]
    else if (random(32) <> 0) then // not correct
      sym := random(13) - 6
    else
      sym := random(51) - 25;

  if (1 = sym) or (0 = sym) then
  begin
    Result := 0;
    exit;
  end;

  for i := 0 to NXFORMS - 1 do
    if (cp.XForm[i].density = 0.0) then
      break;

  if (i = NXFORMS) then
  begin
    Result := 0;
    exit;
  end;
  cp.symmetry := sym;

  if (sym < 0) then
  begin
    cp.XForm[i].density := 1.0;
    cp.XForm[i].symmetry := 1;
    cp.XForm[i].SetVariation(0, 1.0);
    for j := 1 to NRVAR - 1 do
      cp.XForm[i].SetVariation(j, 0.0);
    cp.XForm[i].color := 1.0;
    cp.XForm[i].c[0][0] := -1.0;
    cp.XForm[i].c[0][1] := 0.0;
    cp.XForm[i].c[1][0] := 0.0;
    cp.XForm[i].c[1][1] := 1.0;
    cp.XForm[i].c[2][0] := 0.0;
    cp.XForm[i].c[2][1] := 0.0;

    inc(i);
    inc(Result);
    sym := -sym;
  end;

  a := 2 * pi / sym;

  k := 1;

  while (k < sym) and (i < SymmetryNVars) do
  begin
    cp.XForm[i].density := 1.0;
    cp.XForm[i].SetVariation(0, 1);
    cp.XForm[i].symmetry := 1;
    for j := 1 to NRVAR - 1 do
      cp.XForm[i].SetVariation(j, 0);
    if sym < 3 then
      cp.XForm[i].color := 0
    else
      cp.XForm[i].color := (k - 1) / (sym - 2);

    if cp.XForm[i].color > 1 then
    begin
      repeat
        cp.XForm[i].color := cp.XForm[i].color - 1
      until cp.XForm[i].color <= 1;
    end;

    cp.XForm[i].c[0][0] := cos(k * a);
    cp.XForm[i].c[0][1] := sin(k * a);
    cp.XForm[i].c[1][0] := -cp.XForm[i].c[0][1];
    cp.XForm[i].c[1][1] := cp.XForm[i].c[0][0];
    cp.XForm[i].c[2][0] := 0.0;
    cp.XForm[i].c[2][1] := 0.0;

    inc(i);
    inc(Result);
    inc(k);
  end;
end;

procedure TControlPoint.ZoomtoRect(R: TSRect);
var
  scale, ppu: double;
  dx, dy: double;
begin
  scale := power(2, zoom);
  ppu := pixels_per_unit * scale;

  dx := ((R.Left + R.Right) / 2 - Width / 2) / ppu;
  dy := ((R.Top + R.Bottom) / 2 - Height / 2) / ppu;

  center[0] := center[0] + cos(FAngle) * dx - sin(FAngle) * dy;
  center[1] := center[1] + sin(FAngle) * dx + cos(FAngle) * dy;

  if PreserveQuality then
    zoom := Log2(scale * (Width / (abs(R.Right - R.Left) + 1)))
  else
    pixels_per_unit := pixels_per_unit * Width / abs(R.Right - R.Left);
end;

procedure TControlPoint.ZoomOuttoRect(R: TSRect);
var
  ppu: double;
  dx, dy: double;
begin

  if PreserveQuality then
    zoom := Log2(power(2, zoom) / (Width / (abs(R.Right - R.Left) + 1)))
  else
    pixels_per_unit := pixels_per_unit / Width * abs(R.Right - R.Left);
  ppu := pixels_per_unit * power(2, zoom);

  dx := ((R.Left + R.Right) / 2 - Width / 2) / ppu;
  dy := ((R.Top + R.Bottom) / 2 - Height / 2) / ppu;

  center[0] := center[0] - cos(FAngle) * dx + sin(FAngle) * dy;
  center[1] := center[1] - sin(FAngle) * dx - cos(FAngle) * dy;
end;

procedure TControlPoint.ZoomIn(Factor: double);
var
  scale: double;
begin
  scale := power(2, zoom);

  scale := scale / Factor;
  zoom := Log2(scale);
end;

procedure TControlPoint.MoveRect(R: TSRect);
var
  scale: double;
  ppux, ppuy: double;
  dx, dy: double;
begin
  scale := power(2, zoom);
  ppux := pixels_per_unit * scale;
  ppuy := pixels_per_unit * scale;

  dx := (R.Left - R.Right) / ppux;
  dy := (R.Top - R.Bottom) / ppuy;

  center[0] := center[0] + cos(FAngle) * dx - sin(FAngle) * dy;
  center[1] := center[1] + sin(FAngle) * dx + cos(FAngle) * dy;
end;

procedure TControlPoint.Rotate(Angle: double);
begin
  FAngle := FAngle + Angle;
end;

function TControlPoint.getppux: double;
begin
  Result := pixels_per_unit * power(2, zoom)
end;

function TControlPoint.getppuy: double;
begin
  Result := pixels_per_unit * power(2, zoom)
end;

function TControlPoint.GetBrightness: double;
begin
  Result := Fbrightness;
end;

procedure TControlPoint.SetBrightness(br: double);
begin
  if br > 0 then
  begin
    if Fbrightness <> 0 then
      gamma_threshold := (gamma_threshold / Fbrightness) * br;
    Fbrightness := br;
  end;
end;
function TControlPoint.GetRelativeGammaThreshold: double;
begin
  if Fbrightness <> 0 then
    Result := gamma_threshold / Fbrightness
  else
    Result := gamma_threshold;
end;

procedure TControlPoint.SetRelativeGammaThreshold(gtr: double);
begin
  gamma_threshold := gtr * Fbrightness;
end;

var
  vdfilled: boolean = false;

procedure FillVarDisturb;
const
  startvar_distrib: array [0 .. 26] of integer = (-1, -1, -1, -1, -1, -1, -1, 0,
    0, 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7);
  startmixed_var_distrib: array [0 .. 16] of integer = (0, 0, 0, 1, 1, 1, 2, 2,
    2, 3, 3, 4, 4, 5, 6, 6, 7);
var
  i: integer;
begin
  if vdfilled then
    exit;

  SetLength(var_distrib, NRVAR + 19);
  SetLength(mixed_var_distrib, NRVAR + 9);

  for i := 0 to High(startvar_distrib) do
    var_distrib[i] := startvar_distrib[i];

  for i := High(startvar_distrib) + 1 to high(var_distrib) do
    var_distrib[i] := 8 + i - High(startvar_distrib) - 1;

  for i := 0 to High(startmixed_var_distrib) do
    mixed_var_distrib[i] := startmixed_var_distrib[i];

  for i := High(startmixed_var_distrib) + 1 to high(mixed_var_distrib) do
    mixed_var_distrib[i] := 8 + i - High(startmixed_var_distrib) - 1;

  vdfilled := true;
end;

function TControlPoint.NumXForms: integer;
var
  i: integer;
begin
  // ...
  Result := NXFORMS;
  for i := 0 to NXFORMS - 1 do
  begin
    if XForm[i].density = 0 then
    begin
      Result := i;
      break;
    end;
  end;
end;

function TControlPoint.TrianglesFromCP(var Triangles: TTriangles): integer;
var
  i, j: integer;
  temp_x, temp_y, xset, yset: double;
  Left, Top, Bottom, Right: double;
begin
  Top := 0;
  Bottom := 0;
  Right := 0;
  Left := 0;
  Result := NumXForms;

  begin
    Triangles[-1].x[0] := 1;
    Triangles[-1].y[0] := 0; // "x"
    Triangles[-1].x[1] := 0;
    Triangles[-1].y[1] := 0; // "0"
    Triangles[-1].x[2] := 0;
    Triangles[-1].y[2] := -1; // "y"
  end;

  for j := 0 to Result do
  begin
    for i := 0 to 2 do
      with XForm[j] do
      begin
        if postXswap then
        begin
          Triangles[j].x[i] := Triangles[-1].x[i] * p[0][0] + Triangles[-1].y[i]
            * p[1][0] + p[2][0];
          Triangles[j].y[i] := Triangles[-1].x[i] * p[0][1] + Triangles[-1].y[i]
            * p[1][1] + p[2][1];
        end
        else
        begin
          Triangles[j].x[i] := Triangles[-1].x[i] * c[0][0] + Triangles[-1].y[i]
            * c[1][0] + c[2][0];
          Triangles[j].y[i] := Triangles[-1].x[i] * c[0][1] + Triangles[-1].y[i]
            * c[1][1] + c[2][1];
        end;
      end;
  end;
  EnableFinalXform := finalXformEnabled;

  for j := -1 to Result do
    for i := 0 to 2 do
      Triangles[j].y[i] := -Triangles[j].y[i];
end;

procedure TControlPoint.EqualizeWeights;
var
  t, i: integer;
begin
  t := NumXForms;
  for i := 0 to t - 1 do
    XForm[i].density := 0.5;
end;

procedure TControlPoint.NormalizeWeights;
var
  i: integer;
  td: double;
begin
  td := 0.0;
  for i := 0 to NumXForms - 1 do
    td := td + XForm[i].density;
  if (td < 0.001) then
    EqualizeWeights
  else
    for i := 0 to NumXForms - 1 do
      XForm[i].density := XForm[i].density / td;
end;

procedure TControlPoint.RandomizeWeights;
var
  i: integer;
begin
  for i := 0 to Transforms - 1 do
    XForm[i].density := random;
end;

procedure TControlPoint.ComputeWeights(Triangles: TTriangles; t: integer);
var
  i: integer;
  total_area: double;
begin
  total_area := 0;
  for i := 0 to t - 1 do
  begin
    XForm[i].density := triangle_area(Triangles[i]);
    total_area := total_area + XForm[i].density;
  end;
  for i := 0 to t - 1 do
  begin
    XForm[i].density := XForm[i].density / total_area;
  end;
end;

procedure TControlPoint.GetFromTriangles(const Triangles: TTriangles;
  const t: integer);
var
  i: integer;
  v: double;
begin
  for i := 0 to t do
    if XForm[i].postXswap then
    begin
      solve3(Triangles[-1].x[0], -Triangles[-1].y[0], Triangles[i].x[0],
        Triangles[-1].x[1], -Triangles[-1].y[1], Triangles[i].x[1],
        Triangles[-1].x[2], -Triangles[-1].y[2], Triangles[i].x[2],
        XForm[i].p[0][0], XForm[i].p[1][0], XForm[i].p[2][0]);

      solve3(Triangles[-1].x[0], -Triangles[-1].y[0], -Triangles[i].y[0],
        Triangles[-1].x[1], -Triangles[-1].y[1], -Triangles[i].y[1],
        Triangles[-1].x[2], -Triangles[-1].y[2], -Triangles[i].y[2],
        XForm[i].p[0][1], XForm[i].p[1][1], XForm[i].p[2][1]);
    end
    else
    begin
      solve3(Triangles[-1].x[0], -Triangles[-1].y[0], Triangles[i].x[0],
        Triangles[-1].x[1], -Triangles[-1].y[1], Triangles[i].x[1],
        Triangles[-1].x[2], -Triangles[-1].y[2], Triangles[i].x[2],
        XForm[i].c[0][0], XForm[i].c[1][0], XForm[i].c[2][0]);

      solve3(Triangles[-1].x[0], -Triangles[-1].y[0], -Triangles[i].y[0],
        Triangles[-1].x[1], -Triangles[-1].y[1], -Triangles[i].y[1],
        Triangles[-1].x[2], -Triangles[-1].y[2], -Triangles[i].y[2],
        XForm[i].c[0][1], XForm[i].c[1][1], XForm[i].c[2][1]);
      if XForm[i].autoZscale then
        with XForm[i] do
        begin
          v := c[0][0] * c[1][1] - c[0][1] * c[1][0];

          if v = 1 then
            SetVariation(20, 0.0)
          else
            SetVariation(20, sign(v) * sqrt(abs(v)));
        end;
    end;
  finalXformEnabled := EnableFinalXform;
end;

procedure TControlPoint.GetTriangle(var Triangle: TTriangle; const n: integer);
var
  i, j: integer;
begin
  for i := 0 to 2 do
    with XForm[n] do
    begin
      Triangle.x[i] := MainTriangles[-1].x[i] * c[0][0] - MainTriangles[-1].y[i]
        * c[1][0] + c[2][0];
      Triangle.y[i] := -MainTriangles[-1].x[i] * c[0][1] + MainTriangles[-1].y
        [i] * c[1][1] - c[2][1];
    end;
end;

procedure TControlPoint.GetPostTriangle(var Triangle: TTriangle;
  const n: integer);
var
  i, j: integer;
begin
  for i := 0 to 2 do
    with XForm[n] do
    begin
      Triangle.x[i] := MainTriangles[-1].x[i] * p[0][0] - MainTriangles[-1].y[i]
        * p[1][0] + p[2][0];
      Triangle.y[i] := -MainTriangles[-1].x[i] * p[0][1] + MainTriangles[-1].y
        [i] * p[1][1] - p[2][1];
    end;
end;

procedure TControlPoint.AdjustScale(w, h: integer);
begin
  pixels_per_unit := pixels_per_unit * w / Width;

  Width := w;
  Height := h;
end;

end.
