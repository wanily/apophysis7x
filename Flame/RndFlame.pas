unit RndFlame;

interface

uses
  ControlPoint, Xform;

function RandomFlame(SourceCP: TControlPoint = nil; algorithm: integer = 0)
  : TControlPoint;

implementation

uses
  SysUtils, Global, cmap, GradientHlpr, XFormMan, Classes;

procedure RGBBlend(a, b: integer; var Palette: TColorMap);
var
  c, v: real;
  vrange, range: real;
  i: integer;
begin
  if a = b then
  begin
    Exit;
  end;
  range := b - a;
  vrange := Palette[b mod 256][0] - Palette[a mod 256][0];
  c := Palette[a mod 256][0];
  v := vrange / range;
  for i := (a + 1) to (b - 1) do
  begin
    c := c + v;
    Palette[i mod 256][0] := Round(c);
  end;
  vrange := Palette[b mod 256][1] - Palette[a mod 256][1];
  c := Palette[a mod 256][1];
  v := vrange / range;
  for i := a + 1 to b - 1 do
  begin
    c := c + v;
    Palette[i mod 256][1] := Round(c);
  end;
  vrange := Palette[b mod 256][2] - Palette[a mod 256][2];
  c := Palette[a mod 256][2];
  v := vrange / range;
  for i := a + 1 to b - 1 do
  begin
    c := c + v;
    Palette[i mod 256][2] := Round(c);
  end;
end;

function CreatePalette(strng: string): TColorMap;
var
  Strings: TStringList;
  index, i: integer;
  Tokens: TStringList;
  Indices, Colors: TStringList;
  a, b: integer;
begin
  Strings := TStringList.Create;
  Tokens := TStringList.Create;
  Indices := TStringList.Create;
  Colors := TStringList.Create;
  try
    try
      Strings.Text := strng;
      if Pos('}', Strings.Text) = 0 then
        raise EFormatInvalid.Create('No closing brace');
      if Pos('{', Strings[0]) = 0 then
        raise EFormatInvalid.Create('No opening brace.');
      GetTokens(ReplaceTabs(Strings.Text), Tokens);
      Tokens.Text := Trim(Tokens.Text);
      i := 0;
      while (Pos('}', Tokens[i]) = 0) and
        (Pos('opacity:', Lowercase(Tokens[i])) = 0) do
      begin
        if Pos('index=', Lowercase(Tokens[i])) <> 0 then
          Indices.Add(GetVal(Tokens[i]))
        else if Pos('color=', Lowercase(Tokens[i])) <> 0 then
          Colors.Add(GetVal(Tokens[i]));
        inc(i)
      end;
      for i := 0 to 255 do
      begin
        Result[i][0] := 0;
        Result[i][1] := 0;
        Result[i][2] := 0;
      end;
      if Indices.Count = 0 then
        raise EFormatInvalid.Create('No color info');
      for i := 0 to Indices.Count - 1 do
      begin
        try
          index := StrToInt(Indices[i]);
          while index < 0 do
            inc(index, 400);
          index := Round(Index * (255 / 399));
          Indices[i] := IntToStr(index);
          assert(index >= 0);
          assert(index < 256);
          Result[index][0] := StrToInt(Colors[i]) mod 256;
          Result[index][1] := trunc(StrToInt(Colors[i]) / 256) mod 256;
          Result[index][2] := trunc(StrToInt(Colors[i]) / 65536);
        except
        end;
      end;
      i := 1;
      repeat
        a := StrToInt(Trim(Indices[i - 1]));
        b := StrToInt(Trim(Indices[i]));
        RGBBlend(a, b, Result);
        inc(i);
      until i = Indices.Count;
      if (Indices[0] <> '0') or (Indices[Indices.Count - 1] <> '255') then
      begin
        a := StrToInt(Trim(Indices[Indices.Count - 1]));
        b := StrToInt(Trim(Indices[0])) + 256;
        RGBBlend(a, b, Result);
      end;
    except
      on EFormatInvalid do
      begin
        // Result := False;
      end;
    end;
  finally
    Tokens.Free;
    Strings.Free;
    Indices.Free;
    Colors.Free;
  end;
end;

procedure GetGradientFileGradientsNames(const filename: string;
  var NamesList: TStringList);
var
  i, p: integer;
  Title: string;
  FStrings: TStringList;
begin
  FStrings := TStringList.Create;
  FStrings.LoadFromFile(filename);
  try
    if (Pos('{', FStrings.Text) <> 0) then
    begin
      for i := 0 to FStrings.Count - 1 do
      begin
        p := Pos('{', FStrings[i]);
        if (p <> 0) and (Pos('(3D)', FStrings[i]) = 0) then
        begin
          Title := Trim(Copy(FStrings[i], 1, p - 1));
          if Title <> '' then
            NamesList.Add(Trim(Copy(FStrings[i], 1, p - 1)));
        end;
      end;
    end;
  finally
    FStrings.Free;
  end;
end;

procedure RandomGradient(SourceCP, DestCP: TControlPoint);
var
  tmpGrad: string;
  tmpGrdList: TStringList;
begin
  cmap_index := Random(NRCMAPS);
  GetCMap(cmap_index, 1, DestCP.cmap);
  cmap_index := DestCP.cmapindex;
  DestCP.cmapindex := cmap_index;
end;

procedure RandomVariation(cp: TControlPoint);
var
  a, b, i, j: integer;
  VarPossible: boolean;
begin
  inc(MainSeed);
  RandSeed := MainSeed;

  VarPossible := false;
  for j := 0 to NRVAR - 1 do
  begin
    VarPossible := VarPossible or Variations[j];
  end;

  for i := 0 to cp.NumXForms - 1 do
  begin
    for j := 0 to NRVAR - 1 do
      cp.Xform[i].SetVariation(j, 0);

    if VarPossible then
    begin
      repeat
        a := Random(NRVAR);
      until Variations[a];

      repeat
        b := Random(NRVAR);
      until Variations[b];
    end
    else
    begin
      a := 0;
      b := 0;
    end;

    if (a = b) then
    begin
      cp.Xform[i].SetVariation(a, 1);
    end
    else
    begin
      cp.Xform[i].SetVariation(a, Random);
      cp.Xform[i].SetVariation(b, 1 - cp.Xform[i].GetVariation(a));
    end;
  end;
end;

procedure SetVariation(cp: TControlPoint);
begin
  RandomVariation(cp);
end;

procedure EqualizeWeights(var cp: TControlPoint);
var
  t, i: integer;
begin
  t := cp.NumXForms;
  for i := 0 to t - 1 do
    cp.Xform[i].density := 1.0 / t;
end;

procedure NormalizeWeights(var cp: TControlPoint);
var
  i: integer;
  td: double;
begin
  td := 0.0;
  for i := 0 to cp.NumXForms - 1 do
    td := td + cp.Xform[i].density;
  if (td < 0.001) then
    EqualizeWeights(cp)
  else
    for i := 0 to cp.NumXForms - 1 do
      cp.Xform[i].density := cp.Xform[i].density / td;
end;

procedure ComputeWeights(var cp1: TControlPoint; Triangles: TTriangles;
  t: integer);
var
  i: integer;
  total_area: double;
begin
  total_area := 0.0;
  for i := 0 to t - 1 do
  begin
    cp1.Xform[i].density := triangle_area(Triangles[i]);
    total_area := total_area + cp1.Xform[i].density;
  end;
  for i := 0 to t - 1 do
  begin
    cp1.Xform[i].density := cp1.Xform[i].density / total_area;
  end;
  NormalizeWeights(cp1);
end;

procedure RandomWeights(var cp1: TControlPoint);
var
  i: integer;
begin
  for i := 0 to Transforms - 1 do
    cp1.Xform[i].density := Random;
  NormalizeWeights(cp1);
end;

function RandomFlame(SourceCP: TControlPoint; algorithm: integer)
  : TControlPoint;
var
  Min, Max, i, j, rnd: integer;
  Triangles: TTriangles;
  r, s, theta, phi: double;
  skip: boolean;
begin
  if Assigned(SourceCP) then
    Result := SourceCP.clone
  else
    Result := TControlPoint.Create;

  Min := randMinTransforms;
  Max := randMaxTransforms;

  inc(MainSeed);
  RandSeed := MainSeed;
  Transforms := Random(Max - (Min - 1)) + Min;
  repeat
    try
      inc(MainSeed);
      RandSeed := MainSeed;
      Result.clear;
      Result.RandomCP(Transforms, Transforms, false);
      RandomVariation(Result);
      inc(MainSeed);
      RandSeed := MainSeed;

      case algorithm of
        1:
          rnd := 0;
        2:
          rnd := 7;
      else
        rnd := 9;
      end;
      case rnd of
        0 .. 6:
          begin
            for i := 0 to Transforms - 1 do
            begin
              if Random(10) < 9 then
                Result.Xform[i].c[0, 0] := 1
              else
                Result.Xform[i].c[0, 0] := -1;
              Result.Xform[i].c[0, 1] := 0;
              Result.Xform[i].c[1, 0] := 0;
              Result.Xform[i].c[1, 1] := 1;
              Result.Xform[i].c[2, 0] := 0;
              Result.Xform[i].c[2, 1] := 0;
              Result.Xform[i].color := 0;
              Result.Xform[i].symmetry := 0;
              Result.Xform[i].SetVariation(0, 1);
              for j := 1 to NRVAR - 1 do
                Result.Xform[i].SetVariation(j, 0);
              Result.Xform[i].Translate(Random * 2 - 1, Random * 2 - 1);
              Result.Xform[i].Rotate(Random * 360);
              if i > 0 then
                Result.Xform[i].Scale(Random * 0.8 + 0.2)
              else
                Result.Xform[i].Scale(Random * 0.4 + 0.6);
              if Random(2) = 0 then
                Result.Xform[i].Multiply(1, Random - 0.5, Random - 0.5, 1);
            end;
            RandomVariation(Result);
          end;
        7, 8:
          begin
            for i := 0 to Transforms - 1 do
            begin
              r := Random * 2 - 1;
              if ((0 <= r) and (r < 0.2)) then
                r := r + 0.2;
              if ((r > -0.2) and (r <= 0)) then
                r := r - 0.2;
              s := Random * 2 - 1;
              if ((0 <= s) and (s < 0.2)) then
                s := s + 0.2;
              if ((s > -0.2) and (s <= 0)) then
                s := s - -0.2;
              theta := PI * Random;
              phi := (2 + Random) * PI / 4;
              Result.Xform[i].c[0][0] := r * cos(theta);
              Result.Xform[i].c[1][0] :=
                s * (cos(theta) * cos(phi) - sin(theta));
              Result.Xform[i].c[0][1] := r * sin(theta);
              Result.Xform[i].c[1][1] :=
                s * (sin(theta) * cos(phi) + cos(theta));

              Result.Xform[i].c[2][0] := Random * 2 - 1;
              Result.Xform[i].c[2][1] := Random * 2 - 1;
            end;
            for i := 0 to NXFORMS - 1 do
              Result.Xform[i].density := 0;
            for i := 0 to Transforms - 1 do
              Result.Xform[i].density := 1 / Transforms;
            RandomVariation(Result);
          end;
        9:
          begin
            for i := 0 to NXFORMS - 1 do
              Result.Xform[i].density := 0;
            for i := 0 to Transforms - 1 do
              Result.Xform[i].density := 1 / Transforms;
          end;
      end;
      Result.TrianglesFromCp(Triangles);
      if Random(2) > 0 then
        ComputeWeights(Result, Triangles, Transforms)
      else
        EqualizeWeights(Result);
    except
      on E: EmathError do
      begin
        Continue;
      end;
    end;
    for i := 0 to Transforms - 1 do
      Result.Xform[i].color := i / (Transforms - 1);
    if Result.Xform[0].density = 1 then
      Continue;
    case SymmetryType of
      { Bilateral }
      1:
        add_symmetry_to_control_point(Result, -1);
      { Rotational }
      2:
        add_symmetry_to_control_point(Result, SymmetryOrder);
      { Rotational and Reflective }
      3:
        add_symmetry_to_control_point(Result, -SymmetryOrder);
    end;
    skip := false;
    for i := 0 to Transforms - 1 do
    begin
      if not transform_affine(Triangles[i], Triangles) then
        skip := True;
    end;
    if skip then
      Continue;
  until not Result.BlowsUP(5000) and (Result.Xform[0].density <> 0);

  RandomGradient(SourceCP, Result);

  Result.brightness := defBrightness;
  Result.gamma := defGamma;
  Result.gamma_threshold := defGammaThreshold;
  Result.vibrancy := defVibrancy;
  Result.sample_density := defSampleDensity;
  Result.spatial_oversample := defOversample;
  Result.spatial_filter_radius := defFilterRadius;
  if KeepBackground and Assigned(SourceCP) then
  begin
    Result.background[0] := SourceCP.background[0];
    Result.background[1] := SourceCP.background[1];
    Result.background[2] := SourceCP.background[2];
  end
  else
  begin
    Result.background[0] := 0;
    Result.background[1] := 0;
    Result.background[2] := 0;
  end;
  Result.zoom := 0;
  Result.Xform[Result.NumXForms].clear;
  Result.Xform[Result.NumXForms].symmetry := 1;
end;

end.
