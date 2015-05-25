unit PaletteIO;

interface uses
  SysUtils,
  Classes;

type TColorMap = array [0 .. 255, 0 .. 3] of integer;

const
  RANDOMCMAP = -1;
  NRCMAPS = 701;

{$I IntegratedPalettes.pas}

procedure GetIntegratedPaletteByIndex(const index: integer; out palette: TColorMap);
procedure GetIntegratedCmapNameByIndex(const index: integer; out name: string);

procedure ParsePaletteTokenString(s: string; var list: TStringList);

implementation uses
  Math;

procedure ConvertRgbToHsvArray(const rgb: array of double; out hsv: array of double);
var
  maxval, minval: double;
  del: double;
begin
  maxval := Max(rgb[0], Max(rgb[1], rgb[2]));
  minval := Min(rgb[0], Min(rgb[1], rgb[2]));

  hsv[2] := maxval; // v

  if (maxval > 0) and (maxval <> minval) then
  begin
    del := maxval - minval;
    hsv[1] := del / maxval; // s

    hsv[0] := 0;
    if (rgb[0] > rgb[1]) and (rgb[0] > rgb[2]) then
    begin
      hsv[0] := (rgb[1] - rgb[2]) / del;
    end
    else if (rgb[1] > rgb[2]) then
    begin
      hsv[0] := 2 + (rgb[2] - rgb[0]) / del;
    end
    else
    begin
      hsv[0] := 4 + (rgb[0] - rgb[1]) / del;
    end;

    if hsv[0] < 0 then
      hsv[0] := hsv[0] + 6;

  end
  else
  begin
    hsv[0] := 0;
    hsv[1] := 0;
  end;
end;

procedure ConvertHsvToRgbArray(const hsv: array of double; out rgb: array of double);
var
  j: integer;
  f, p, q, t, v: double;
begin
  try
    j := floor(hsv[0]);

    f := hsv[0] - j;
    v := hsv[2];
    p := hsv[2] * (1 - hsv[1]);
    q := hsv[2] * (1 - hsv[1] * f);
    t := hsv[2] * (1 - hsv[1] * (1 - f));
    case j of
      0:
        begin
          rgb[0] := v;
          rgb[1] := t;
          rgb[2] := p;
        end;
      1:
        begin
          rgb[0] := q;
          rgb[1] := v;
          rgb[2] := p;
        end;
      2:
        begin
          rgb[0] := p;
          rgb[1] := v;
          rgb[2] := t;
        end;
      3:
        begin
          rgb[0] := p;
          rgb[1] := q;
          rgb[2] := v;
        end;
      4:
        begin
          rgb[0] := t;
          rgb[1] := p;
          rgb[2] := v;
        end;
      5:
        begin
          rgb[0] := v;
          rgb[1] := p;
          rgb[2] := q;
        end;
    end;
  except
    on EMathError do
  end;
end;

procedure GetIntegratedPaletteByIndex(const index: integer; out palette: TColorMap);
var
  i, ii: integer;
  rgb: array [0 .. 2] of double;
  hsv: array [0 .. 2] of double;
begin
  ii := index;

  if ii = RANDOMCMAP then
    ii := Random(NRCMAPS);

  if (ii < 0) or (ii >= NRCMAPS) then
    ii := 0;

  for i := 0 to 255 do
  begin
    rgb[0] := IntegratedPalettes[ii][i][0] / 255.0;
    rgb[1] := IntegratedPalettes[ii][i][1] / 255.0;
    rgb[2] := IntegratedPalettes[ii][i][2] / 255.0;

    ConvertRgbToHsvArray(rgb, hsv);
    hsv[0] := hsv[0];
    ConvertHsvToRgbArray(hsv, rgb);

    palette[i][0] := Round(rgb[0] * 255);
    palette[i][1] := Round(rgb[1] * 255);
    palette[i][2] := Round(rgb[2] * 255);
  end;
end;

procedure GetIntegratedCmapNameByIndex(const Index: integer; out Name: string);
var
  ii: integer;
begin
  ii := index;
  if ii = RANDOMCMAP then
    ii := Random(NRCMAPS);

  if (ii < 0) or (ii >= NRCMAPS) then
    ii := 0;

  Name := CMapNames[ii];
end;

procedure ParsePaletteTokenString(s: string; var list: TStringList);
var
  test, token: string;
begin
  list.clear;
  test := s;
  while (Length(test) > 0) do
  begin
    while (Length(test) > 0) and CharInSet(test[1], [#32]) do
      Delete(test, 1, 1);
    if (Length(test) = 0) then
      Exit;
    token := '';
    while (Length(test) > 0) and (not CharInSet(test[1], [#32])) do
    begin
      token := token + test[1];
      Delete(test, 1, 1);
    end;
    list.add(token);
  end;
end;

end.
