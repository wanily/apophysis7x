unit GradientHlpr;

interface

uses
  windows, Graphics, Cmap;

const
  PixelCountMax = 32768;

type
  pRGBTripleArray = ^TRGBTripleArray;
  TRGBTripleArray = array [0 .. PixelCountMax - 1] of TRGBTriple;

type
  TGradientHelper = class
  private
    procedure RGBBlend(a, b: integer; var Palette: TColorMap);
  public
    function GetGradientBitmap(Index: integer;
      const hue_rotation: double): TBitmap;
    function RandomGradient: TColorMap;
  end;

var
  GradientHelper: TGradientHelper;

implementation

uses
  Global;

function TGradientHelper.GetGradientBitmap(Index: integer;
  const hue_rotation: double): TBitmap;
var
  BitMap: TBitmap;
  i, j: integer;
  Row: pRGBTripleArray;
  pal: TColorMap;
begin
  GetCMap(index, hue_rotation, pal);

  BitMap := TBitmap.create;
  BitMap.PixelFormat := pf24bit;
  BitMap.Width := 256;
  BitMap.Height := 2;

  for j := 0 to BitMap.Height - 1 do
  begin
    Row := BitMap.Scanline[j];
    for i := 0 to BitMap.Width - 1 do
    begin
      Row[i].rgbtRed := pal[i][0];
      Row[i].rgbtGreen := pal[i][1];
      Row[i].rgbtBlue := pal[i][2];
    end
  end;

  Result := BitMap;
end;

function TGradientHelper.RandomGradient: TColorMap;
const
  maxNodes: integer = 10;
  minNodes: integer = 2;

  maxHue: integer = 600;
  maxSat: integer = 100;
  maxLum: integer = 100;

  minHue: integer = 0;
  minSat: integer = 0;
  minLum: integer = 0;
var
  a, b, n, nodes: integer;
  rgb: array [0 .. 2] of double;
  hsv: array [0 .. 2] of double;
  pal: TColorMap;
begin
  rgb[0] := 0;
  rgb[1] := 0;
  rgb[2] := 0;

  inc(MainSeed);
  RandSeed := MainSeed;
  nodes := random((maxNodes - 1) - (minNodes - 2)) + (minNodes - 1);
  n := 256 div nodes;
  b := 0;
  hsv[0] := 0.01 * (random(maxHue - (minHue - 1)) + minHue);
  hsv[1] := 0.01 * (random(maxSat - (minSat - 1)) + minSat);
  hsv[2] := 0.01 * (random(maxLum - (minLum - 1)) + minLum);
  hsv2rgb(hsv, rgb);
  pal[0][0] := Round(rgb[0] * 255);
  pal[0][1] := Round(rgb[1] * 255);
  pal[0][2] := Round(rgb[2] * 255);
  repeat
    a := b;
    b := b + n;
    hsv[0] := 0.01 * (random(maxHue - (minHue - 1)) + minHue);
    hsv[1] := 0.01 * (random(maxSat - (minSat - 1)) + minSat);
    hsv[2] := 0.01 * (random(maxLum - (minLum - 1)) + minLum);
    hsv2rgb(hsv, rgb);
    if b > 255 then
      b := 255;
    pal[b][0] := Round(rgb[0] * 255);
    pal[b][1] := Round(rgb[1] * 255);
    pal[b][2] := Round(rgb[2] * 255);
    RGBBlend(a, b, pal);
  until b = 255;
  Result := pal;
end;

procedure TGradientHelper.RGBBlend(a, b: integer; var Palette: TColorMap);
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

initialization

GradientHelper := TGradientHelper.create;

finalization

GradientHelper.Free;

end.
