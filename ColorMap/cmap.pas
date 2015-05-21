unit Cmap;

interface

uses sysutils, classes;

type
  TColorMap = array [0 .. 255, 0 .. 3] of integer;

type
  EFormatInvalid = class(Exception);

const
  RANDOMCMAP = -1;
  NRCMAPS = 701;

procedure GetCmap(var Index: integer; const hue_rotation: double;
  out Cmap: TColorMap);
procedure GetCmapName(var Index: integer; out Name: string);
procedure rgb2hsv(const rgb: array of double; out hsv: array of double);
procedure hsv2rgb(const hsv: array of double; out rgb: array of double);
function GetGradient(FileName, Entry: string): string;
function GetPalette(strng: string; var Palette: TColorMap): boolean;
procedure GetTokens(s: string; var mlist: TStringList);

implementation

uses
  cmapdata, Math;

procedure rgb2hsv(const rgb: array of double; out hsv: array of double);
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

procedure hsv2rgb(const hsv: array of double; out rgb: array of double);
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

procedure GetCmap(var Index: integer; const hue_rotation: double;
  out Cmap: TColorMap);
var
  i: integer;
  rgb: array [0 .. 2] of double;
  hsv: array [0 .. 2] of double;
begin
  if Index = RANDOMCMAP then
    Index := Random(NRCMAPS);

  if (Index < 0) or (Index >= NRCMAPS) then
    Index := 0;

  for i := 0 to 255 do
  begin
    rgb[0] := cmaps[Index][i][0] / 255.0;
    rgb[1] := cmaps[Index][i][1] / 255.0;
    rgb[2] := cmaps[Index][i][2] / 255.0;

    rgb2hsv(rgb, hsv);
    hsv[0] := hsv[0] + hue_rotation * 6;
    hsv2rgb(hsv, rgb);

    Cmap[i][0] := Round(rgb[0] * 255);
    Cmap[i][1] := Round(rgb[1] * 255);
    Cmap[i][2] := Round(rgb[2] * 255);
  end;
end;

procedure GetCmapName(var Index: integer; out Name: string);
begin
  if Index = RANDOMCMAP then
    Index := Random(NRCMAPS);

  if (Index < 0) or (Index >= NRCMAPS) then
    Index := 0;

  Name := CMapNames[Index];
end;

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

function GetVal(token: string): string;
var
  p: integer;
begin
  p := Pos('=', token);
  Delete(token, 1, p);
  Result := token;
end;

function ReplaceTabs(str: string): string;
var
  i: integer;
begin
  for i := 1 to Length(str) do
  begin
    if str[i] = #9 then
    begin
      Delete(str, i, 1);
      Insert(#32, str, i);
    end;
  end;
  Result := str;
end;

procedure GetTokens(s: string; var mlist: TStringList);
var
  test, token: string;
begin
  mlist.clear;
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
    mlist.add(token);
  end;
end;

function GetPalette(strng: string; var Palette: TColorMap): boolean;
var
  Strings: TStringList;
  Index, i: integer;
  Tokens: TStringList;
  Indices, Colors: TStringList;
  a, b: integer;
begin
  GetPalette := True;
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
      i := 0;
      while (Pos('}', Tokens[i]) = 0) and
        (Pos('opacity:', Lowercase(Tokens[i])) = 0) do
      begin
        if Pos('index=', Lowercase(Tokens[i])) <> 0 then
          Indices.add(GetVal(Tokens[i]))
        else if Pos('color=', Lowercase(Tokens[i])) <> 0 then
          Colors.add(GetVal(Tokens[i]));
        inc(i)
      end;
      for i := 0 to 255 do
      begin
        Palette[i][0] := 0;
        Palette[i][1] := 0;
        Palette[i][2] := 0;
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
          Palette[index][0] := StrToInt(Colors[i]) mod 256;
          Palette[index][1] := trunc(StrToInt(Colors[i]) / 256) mod 256;
          Palette[index][2] := trunc(StrToInt(Colors[i]) / 65536);
        except
        end;
      end;
      i := 1;
      repeat
        a := StrToInt(Indices[i - 1]);
        b := StrToInt(Indices[i]);
        RGBBlend(a, b, Palette);
        inc(i);
      until i = Indices.Count;
      if (Indices[0] <> '0') or (Indices[Indices.Count - 1] <> '255') then
      begin
        a := StrToInt(Indices[Indices.Count - 1]);
        b := StrToInt(Indices[0]) + 256;
        RGBBlend(a, b, Palette);
      end;
    except
      on EFormatInvalid do
      begin
        Result := False;
      end;
    end;
  finally
    Tokens.Free;
    Strings.Free;
    Indices.Free;
    Colors.Free;
  end;
end;

function GetGradient(FileName, Entry: string): string;
var
  FileStrings: TStringList;
  GradStrings: TStringList;
  i: integer;
begin
  FileStrings := TStringList.Create;
  GradStrings := TStringList.Create;
  try
    try
      FileStrings.LoadFromFile(FileName);
      for i := 0 to FileStrings.Count - 1 do
        if Pos(Entry + ' ', Trim(FileStrings[i])) = 1 then
          break;
      GradStrings.add(FileStrings[i]);
      repeat
        inc(i);
        GradStrings.add(FileStrings[i]);
      until Pos('}', FileStrings[i]) <> 0;
      GetGradient := GradStrings.Text;
    except
      on Exception do
        Result := '';
    end;
  finally
    GradStrings.Free;
    FileStrings.Free;
  end;
end;

function LoadGradient(FileName, Entry: string; var gString: string;
  var Pal: TColorMap): boolean;
var
  FileStrings: TStringList;
  GradStrings: TStringList;
  i: integer;
begin
  FileStrings := TStringList.Create;
  GradStrings := TStringList.Create;
  try
    try
      FileStrings.LoadFromFile(FileName);
      for i := 0 to FileStrings.Count - 1 do
        if Pos(Entry + ' ', Trim(FileStrings[i])) = 1 then
          break;
      GradStrings.add(FileStrings[i]);
      repeat
        inc(i);
        GradStrings.add(FileStrings[i]);
      until Pos('}', FileStrings[i]) <> 0;
      gString := GradStrings.Text;
      Result := GetPalette(GradStrings.Text, Pal);
    except
      on Exception do
        Result := False;
    end;
  finally
    GradStrings.Free;
    FileStrings.Free;
  end;
end;

end.
