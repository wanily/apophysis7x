unit ParameterIO;

interface

uses Global, SysUtils, StrUtils, ControlPoint, XForm, PaletteIO,
  VariationPoolManager, RegularExpressionsCore, RegexHelper, Classes;

type
  TBatch = class
    private

      mNames: TStringList;
      mData: TStringList;

      procedure Parse(const path: string);
      function CountFlames: integer;

    public

      constructor CreateVolatileBatch;
      constructor LoadBatch(const path: string);
      destructor Destroy; override;

      property Count: integer read CountFlames;

      function GetFlameNameAt(i: integer): string;
      function GetFlameXmlAt(i: integer): string;

      procedure LoadControlPoint(i: integer; var cp: TControlPoint);
      procedure StoreControlPoint(i: integer; cp: TControlPoint);
      procedure AppendControlPoint(cp: TControlPoint);

      procedure AppendFlameAndSaveBatch(i: integer; filePath: string);
      procedure SaveBatch(filePath: string);

      procedure RemoveAt(i: integer);

      class procedure SaveControlPointToFile(cp: TControlPoint; filePath: string);

      class function CreateBatch(filePath: string): TBatch;
  end;

function IsRegisteredVariation(name: string): boolean;
function IsRegisteredVariable(name: string): boolean;

procedure EnumParameters(xml: string; var list: TStringList);
function NameOf(xml: string): string;
function FindFlameInBatch(xml, name: string): string;

procedure LoadPaletteFromXmlCompatible(xml: string; var cp: TControlPoint);
procedure LoadXFormFromXmlCompatible(xml: string; isFinalXForm: boolean; var xf: TXForm; var enabled: boolean);
function LoadCpFromXmlCompatible(xml: string; var cp: TControlPoint; var statusOutput: string): boolean;
function SaveCpToXmlCompatible(var xml: string; const cp1: TControlPoint): boolean;

implementation

constructor TBatch.LoadBatch(const path: string);
begin
  if not FileExists(path) then
    raise Exception.Create('Could not find file: "' + path + '"');

  mNames := TStringList.Create;
  mData := TStringList.Create;

  Parse(path);
end;

constructor TBatch.CreateVolatileBatch;
begin
  mNames := TStringList.Create;
  mData := TStringList.Create;
end;

procedure TBatch.LoadControlPoint(i: Integer; var cp: TControlPoint);
var
  str: string;
begin
  LoadCpFromXmlCompatible(self.GetFlameXmlAt(i), cp, str);
end;

procedure TBatch.StoreControlPoint(i: Integer; cp: TControlPoint);
var
  xml: string;
begin
  SaveCpToXmlCompatible(xml, cp);
  mData[i] := xml;
end;

procedure TBatch.AppendControlPoint(cp: TControlPoint);
var
  xml: string;
begin
  SaveCpToXmlCompatible(xml, cp);
  mData.Add(xml);
  mNames.Add(cp.Name);
end;

procedure TBatch.RemoveAt(i: Integer);
begin
  mData.Delete(i);
  mNames.Delete(i);
end;

procedure TBatch.AppendFlameAndSaveBatch(i: Integer; filePath: string);
var
  fileList: TStringList;
  tempBatch: TBatch;
begin
  if FileExists(filePath) then
  begin
    tempBatch := TBatch.LoadBatch(filePath);
    tempBatch.mNames.Add(mNames[i]);
    tempBatch.mData.Add(mData[i]);
    tempBatch.SaveBatch(filePath);
    tempBatch.Destroy;
  end else begin
    fileList := TStringList.Create;
    fileList.Text := mData[i];
    fileList.Insert(0, '<flames name="' + ChangeFileExt(ExtractFileName(filePath), '') + '">');
    fileList.Add('</flames>');
    fileList.SaveToFile(filePath);
    fileList.Destroy;
  end;
end;

procedure TBatch.SaveBatch(filePath: string);
var
  fileList: TStringList;
  flameList: TStringList;
  i, j: Integer;
begin

  fileList := TStringList.Create;
  fileList.Add('<flames name="' + mNames[0] + '">');

  for i := 0 to Count - 1 do
  begin
    flameList := TStringList.Create;
    flameList.Text := mData[i];

    for j := 0 to flameList.Count do
      fileList.Add(flameList[j]);

    flameList.Destroy;
  end;

  fileList.Add('</flames>');
  fileList.SaveToFile(filePath);
  fileList.Destroy;
end;

class function TBatch.CreateBatch(filePath: string): TBatch;
var
  fileList: TStringList;
begin
  fileList := TStringList.Create;
  fileList.Add('<flames name="' + ChangeFileExt(ExtractFileName(filePath), '') + '">');
  fileList.Add('</flames>');
  fileList.SaveToFile(filePath);
  fileList.Destroy;

  Result := TBatch.LoadBatch(filePath);
end;

class procedure TBatch.SaveControlPointToFile(cp: TControlPoint; filePath: string);
var
  fileList: TStringList;
  tempBatch: TBatch;
  xml: string;
begin
  SaveCpToXmlCompatible(xml, cp);

  if FileExists(filePath) then
  begin
    tempBatch := TBatch.LoadBatch(filePath);
    tempBatch.mNames.Add(cp.name);
    tempBatch.mData.Add(xml);
    tempBatch.SaveBatch(filePath);
    tempBatch.Destroy;
  end else begin
    fileList := TStringList.Create;
    fileList.Text := xml;
    fileList.Insert(0, '<flames name="' + ChangeFileExt(ExtractFileName(filePath), '') + '">');
    fileList.Add('</flames>');
    fileList.SaveToFile(filePath);
    fileList.Destroy;
  end;
end;

procedure TBatch.Parse(const path: string);
var
  fileContent: TStringList;
  flames: TStringList;

  i: integer;
  name: string;
begin
  fileContent := TStringList.Create;
  fileContent.LoadFromFile(path, TEncoding.Default);  // -x- todo: evaluate effects of using UTF8

  flames := TStringList.Create;
  EnumParameters(fileContent.Text, flames);

  mNames.Clear;
  mData.Clear;

  for i := 0 to flames.Count - 1 do
  begin
    name := NameOf(flames[i]);
    mNames.Add(name);
    mData.Add(flames[i]);
  end;

  flames.Destroy;
  fileContent.Destroy;
end;

function TBatch.CountFlames: integer;
begin
  Assert(mNames.Count = mData.Count);
  Result := mNames.Count;
end;

function TBatch.GetFlameNameAt(i: Integer): string;
begin
  Result := mNames[i];
end;

function TBatch.GetFlameXmlAt(i: Integer): string;
begin
  Result := mData[i];
end;

destructor TBatch.Destroy;
begin
  mNames.Destroy;
  mData.Destroy;
end;

(* *************************** Validation functions ***************************** *)
function IsRegisteredVariation(name: string): boolean;
var
  i, count: integer;
  vname: string;
  xf: TXForm;
begin
  xf := TXForm.Create;
  xf.Destroy;
  count := GetTotalVariationCount;
  for i := 0 to count - 1 do
  begin
    vname := GetVariationNameByIndex(i);
    if (lowercase(vname) = lowercase(name)) then
    begin
      Result := true;
      exit;
    end;
  end;
  Result := false;
end;

function IsRegisteredVariable(name: string): boolean;
var
  i, count: integer;
begin
  count := GetTotalVariableCount;
  for i := 0 to count - 1 do
  begin
    if (lowercase(GetVariableNameByGlobalVariableIndex(i)) = lowercase(name)) then
    begin
      Result := true;
      exit;
    end;
  end;
  Result := false;
end;

(* ***************************** Loading functions ******************************* *)
function NameOf(xml: string): string;
var
  Regex: TPerlRegEx;
begin
  Regex := TPerlRegEx.Create;
  Regex.Regex := '<flame.*?name="(.*?)".*?>.*?</flame>';
  Regex.Options := [preSingleLine, preCaseless];
  Regex.Subject := (xml);
  if Regex.Match then
  begin
    Result := String(Regex.Groups[1]);
  end
  else
    Result := '';
  Regex.Free;
end;

procedure EnumParameters(xml: string; var list: TStringList);
var
  Regex: TPerlRegEx;
begin
  Regex := TPerlRegEx.Create;
  Regex.Regex := '<flame.*?>.*?</flame>';
  Regex.Options := [preSingleLine, preCaseless];
  Regex.Subject := (xml);
  if Regex.Match then
  begin
    repeat
      list.Add(String(Regex.MatchedText));
    until not Regex.MatchAgain;
  end;
  Regex.Free;
end;

function FindFlameInBatch(xml, name: string): string;
var
  Regex: TPerlRegEx;
begin
  Regex := TPerlRegEx.Create;
  Regex.Regex := '<flame.*?name="(.*?)".*?>.*?</flame>';
  Regex.Options := [preSingleLine, preCaseless];
  Regex.Subject := (xml);
  if Regex.Match then
  begin
    repeat
      if ((name) = Regex.Groups[1]) then
      begin
        Result := String(Regex.MatchedText);
        Regex.Free;
        exit;
      end;
    until not Regex.MatchAgain;
  end;
  Result := '';
  Regex.Free;
end;

function LoadCpFromXmlCompatible(xml: string; var cp: TControlPoint;
  var statusOutput: string): boolean;
const
  re_flame: string = '<flame(.*?)>(.*?)</flame>';
  re_xform: string = '<((?:final)?xform)(.*?)/>';
  re_palette: string = '<palette(.*?)>([a-f0-9\s]+)</palette>';
  re_attrib: string = '([0-9a-z_]+)="(.*?)"';
  re_strtoken: string = '([a-z0-9_]+)';
var
  flame_attribs: string;
  flame_content: string;
  xform_type: string;
  xform_attribs: string;
  palette_attribs: string;
  palette_content: string;

  find_attribs: TPerlRegEx;
  found_attrib: boolean;
  attrib_name: string;
  attrib_match: string;

  find_xforms: TPerlRegEx;
  found_xform: boolean;
  xform_index: integer;

  find_strtokens: TPerlRegEx;
  found_strtoken: boolean;
  strtoken_index: integer;
  strtoken_value: string;

  find_palette: TPerlRegEx;

  temp2i: T2Int;
  temp2f: T2Float;
  temprgb: TRGB;

  dummy: boolean;
  attrib_success: boolean;
  i: integer;
begin
  find_strtokens := TPerlRegEx.Create;
  find_attribs := TPerlRegEx.Create;
  find_xforms := TPerlRegEx.Create;
  find_palette := TPerlRegEx.Create;

  find_attribs.Regex := (re_attrib);
  find_strtokens.Regex := (re_strtoken);
  find_xforms.Regex := (re_xform);
  find_palette.Regex := (re_palette);

  find_attribs.Options := [preSingleLine, preCaseless];
  find_strtokens.Options := [preSingleLine, preCaseless];
  find_xforms.Options := [preSingleLine, preCaseless];
  find_palette.Options := [preSingleLine, preCaseless];

  flame_attribs := (GetStringPart(xml, re_flame, 1, ''));
  flame_content := (GetStringPart(xml, re_flame, 2, ''));

  find_attribs.Subject := (flame_attribs);
  found_attrib := find_attribs.Match;

  Result := true;

  while found_attrib do
  begin
    attrib_match := find_attribs.MatchedText;
    attrib_name := (lowercase(String(find_attribs.Groups[1])));
    attrib_success := true;

    if attrib_name = 'name' then
      cp.name := GetStringPart(String(attrib_match), re_attrib, 2, '')
    else if attrib_name = 'vibrancy' then
      cp.vibrancy := GetFloatPart(String(attrib_match), re_attrib, 2,
        defVibrancy)
    else if attrib_name = 'brightness' then
      cp.brightness := GetFloatPart(String(attrib_match), re_attrib, 2,
        defBrightness)
    else if attrib_name = 'gamma' then
      cp.gamma := GetFloatPart(String(attrib_match), re_attrib, 2, defGamma)
    else if attrib_name = 'gamma_threshold' then
      cp.gamma_threshold := GetFloatPart(String(attrib_match), re_attrib, 2,
        defGammaThreshold)
    else if attrib_name = 'oversample' then
      cp.spatial_oversample := GetIntPart(String(attrib_match), re_attrib, 2,
        defOversample)
    else if attrib_name = 'filter' then
      cp.spatial_filter_radius := GetFloatPart(String(attrib_match), re_attrib,
        2, defFilterRadius)
    else if attrib_name = 'zoom' then
      cp.zoom := GetFloatPart(String(attrib_match), re_attrib, 2, 0)
    else if attrib_name = 'scale' then
      cp.pixels_per_unit := GetFloatPart(String(attrib_match), re_attrib, 2, 25)
    else if attrib_name = 'quality' then
      cp.sample_density := GetFloatPart(String(attrib_match), re_attrib, 2, 5)
    else if attrib_name = 'angle' then
      cp.fangle := GetFloatPart(String(attrib_match), re_attrib, 2, 0)
    else if attrib_name = 'rotate' then // angle = -pi*x/180
      cp.vibrancy := -PI * GetFloatPart(String(attrib_match), re_attrib,
        2, 0) / 180
    else if attrib_name = 'cam_pitch' then
      cp.cameraPitch := GetFloatPart(String(attrib_match), re_attrib, 2, 0)
    else if attrib_name = 'cam_yaw' then
      cp.cameraYaw := GetFloatPart(String(attrib_match), re_attrib, 2, 0)
    else if attrib_name = 'cam_perspective' then
      cp.cameraPersp := GetFloatPart(String(attrib_match), re_attrib, 2, 1)
    else if attrib_name = 'cam_dist' then // perspective = 1/x
    begin
      cp.cameraPersp := GetFloatPart(String(attrib_match), re_attrib, 2, 1);
      if cp.cameraPersp = 0 then
        cp.cameraPersp := EPS;
      cp.cameraPersp := 1 / cp.cameraPersp;
    end
    else if attrib_name = 'cam_zpos' then
      cp.cameraZpos := GetFloatPart(String(attrib_match), re_attrib, 2, 0)
    else if attrib_name = 'cam_dof' then
      cp.cameraDOF := GetFloatPart(String(attrib_match), re_attrib, 2, 0)

    else if attrib_name = 'estimator_radius' then
      cp.estimator := GetFloatPart(String(attrib_match), re_attrib, 2, 0)
    else if attrib_name = 'estimator_minimum' then
      cp.estimator_min := GetFloatPart(String(attrib_match), re_attrib, 2, 0)
    else if attrib_name = 'estimator_curve' then
      cp.estimator_curve := GetFloatPart(String(attrib_match), re_attrib, 2, 0)
    else if attrib_name = 'enable_de' then
      cp.enable_de := GetBoolPart(String(attrib_match), re_attrib, 2, false)

    else if attrib_name = 'center' then
    begin
      temp2f := Get2FloatPart(String(attrib_match), re_attrib, 2, 0);
      cp.center[0] := temp2f.f1;
      cp.center[1] := temp2f.f2;
    end
    else if attrib_name = 'size' then
    begin
      temp2i := Get2IntPart(String(attrib_match), re_attrib, 2, 0);
      cp.Width := temp2i.i1;
      cp.Height := temp2i.i2;
    end
    else if attrib_name = 'background' then
    begin
      temprgb := GetRGBPart(String(attrib_match), re_attrib, 2, 0);
      cp.background[0] := temprgb.r;
      cp.background[1] := temprgb.g;
      cp.background[2] := temprgb.b;
    end

    else if attrib_name = 'soloxform' then
      cp.soloXform := GetIntPart(String(attrib_match), re_attrib, 2, 0);

    found_attrib := find_attribs.MatchAgain;
  end;

  if LimitVibrancy and (cp.vibrancy > 1) then
    cp.vibrancy := 1;
  cp.cmapindex := -1;

  find_xforms.Subject := flame_content;
  found_xform := find_xforms.Match;
  xform_index := 0;

  for i := 0 TO NXFORMS - 1 do
  begin
    cp.XForm[i].Clear;
    cp.XForm[i].transOpacity := 1;
    cp.XForm[i].symmetry := 1;
    cp.XForm[i].density := 0;
  end;

  // -x- why we need 3 flags to indicate whether there is a final xform is subject of the next episode of "Ancient Aliens"...
  cp.finalXformEnabled := false;
  cp.useFinalXform := false;

  while found_xform do
  begin
    xform_type := find_xforms.Groups[1];
    xform_attribs := find_xforms.Groups[2];
    if (lowercase(String(xform_type)) = 'xform') then
    begin
      LoadXFormFromXmlCompatible(find_xforms.MatchedText, false, cp.XForm[xform_index], dummy);
      xform_index := xform_index + 1;
    end
    else
    begin
      cp.finalXform := TXForm.Create;
      LoadXFormFromXmlCompatible(find_xforms.MatchedText, true, cp.finalXform, dummy);
      cp.finalXformEnabled := true;
      cp.useFinalXform := true;
      xform_index := xform_index + 1;
      cp.XForm[cp.NumXForms] := cp.finalXform;
      cp.XForm[cp.NumXForms].symmetry := 1;
      cp.XForm[cp.NumXForms].transOpacity := 1;
      cp.XForm[cp.NumXForms].density := 0;
    end;
    found_xform := find_xforms.MatchAgain;
  end;

  find_palette.Subject := (xml);
  if (find_palette.Match) then
    LoadPaletteFromXmlCompatible(find_palette.MatchedText, cp);

  find_strtokens.Free;
  find_attribs.Free;
  find_xforms.Free;
  find_palette.Free;
end;

procedure LoadPaletteFromXmlCompatible(xml: string; var cp: TControlPoint);
const
  re_palette: string = '<palette(.*?)>([a-f0-9\s]+)</palette>';
  re_attrib: string = '([0-9a-z_]+)="(.*?)"';
var
  i, pos, len, count: integer;
  c: char;
  data, attr, hexdata, format: string;
  alpha: boolean;

  find_attribs: TPerlRegEx;
  found_attrib: boolean;
  attrib_name: string;
  attrib_match: string;
  attrib_success: boolean;
  function HexChar(c: char): Byte;
  begin
    case c of
      '0' .. '9':
        Result := (Byte(c) - Byte('0'));
      'a' .. 'f':
        Result := (Byte(c) - Byte('a')) + 10;
      'A' .. 'F':
        Result := (Byte(c) - Byte('A')) + 10;
    else
      Result := 0;
    end;
  end;

begin
  hexdata := GetStringPart(String(xml), re_palette, 2, '');
  attr := GetStringPart(String(xml), re_palette, 1, '');

  find_attribs := TPerlRegEx.Create;
  find_attribs.Regex := (re_attrib);
  find_attribs.Options := [preSingleLine, preCaseless];
  find_attribs.Subject := (attr);
  found_attrib := find_attribs.Match;

  count := 0;

  while found_attrib do
  begin
    attrib_match := find_attribs.MatchedText;
    attrib_name := (lowercase(String(find_attribs.Groups[1])));
    attrib_success := true;

    if (attrib_name = 'count') then
      count := GetIntPart(String(attrib_match), re_attrib, 2, 256)
    else if (attrib_name = 'format') then
      format := GetStringPart(String(attrib_match), re_attrib, 2, 'RGB');

    found_attrib := find_attribs.MatchAgain;
  end;

  find_attribs.Free;

  alpha := (lowercase(format) = 'rgba');
  data := '';

  for i := 1 to Length(hexdata) do
  begin
    c := hexdata[i];
    if CharInSet(c, ['0' .. '9'] + ['A' .. 'F'] + ['a' .. 'f']) then
      data := data + c;
  end;

  if alpha then
    len := count * 8
  else
    len := count * 6;

  for i := 0 to count - 1 do
  begin
    if alpha then
      pos := i * 8 + 2
    else
      pos := i * 6;
    cp.cmap[i][0] := 16 * HexChar(data[pos + 1]) + HexChar(data[pos + 2]);
    cp.cmap[i][1] := 16 * HexChar(data[pos + 3]) + HexChar(data[pos + 4]);
    cp.cmap[i][2] := 16 * HexChar(data[pos + 5]) + HexChar(data[pos + 6]);
  end;
end;

procedure LoadXFormFromXmlCompatible(xml: string; isFinalXForm: boolean; var xf: TXForm; var enabled: boolean);
const
  re_attrib: string = '([0-9a-z_]+)="(.*?)"';
  re_xform: string = '<((?:final)?xform)(.*?)/>';
  re_coefs: string =
    '([\d.eE+-]+)\s+([\d.eE+-]+)\s+([\d.eE+-]+)\s+([\d.eE+-]+)\s+([\d.eE+-]+)\s+([\d.eE+-]+)';
var
  xform_attribs: string;
  find_attribs: TPerlRegEx;
  found_attrib: boolean;
  attrib_name: string;
  attrib_match: string;
  token_part: string;
  i, j: integer;
  d: double;
  t: TStringList;
  v_set: boolean;
  attrib_success: boolean;
begin
  enabled := true;
  xform_attribs := GetStringPart(String(xml), re_xform, 2, '');

  find_attribs := TPerlRegEx.Create;
  find_attribs.Regex := (re_attrib);
  find_attribs.Options := [preSingleLine, preCaseless];
  find_attribs.Subject := (xform_attribs);
  found_attrib := find_attribs.Match;

  for i := 0 to GetTotalVariationCount - 1 do
    xf.SetVariation(i, 0);

  while found_attrib do
  begin
    attrib_match := find_attribs.MatchedText;
    attrib_name := (find_attribs.Groups[1]);
    attrib_success := true;

    if (attrib_name = 'enabled') and isFinalXForm then
      enabled := GetBoolPart(String(attrib_match), re_attrib, 2, true)
    else if (attrib_name = 'weight') and (not isFinalXForm) then
      xf.density := GetFloatPart(String(attrib_match), re_attrib, 2, 0.5)
    else if (attrib_name = 'symmetry') and (not isFinalXForm) then
      xf.symmetry := GetFloatPart(String(attrib_match), re_attrib, 2, 0)
    else if (attrib_name = 'color_speed') and (not isFinalXForm) then
      xf.symmetry := GetFloatPart(String(attrib_match), re_attrib, 2, 0)
    else if (attrib_name = 'chaos') and (not isFinalXForm) then
    begin
      token_part := GetStringPart(String(attrib_match), re_attrib, 2, '');
      if token_part <> '' then
      begin
        t := TStringList.Create;
        ParsePaletteTokenString(token_part, t);
        for i := 0 to t.count - 1 do
          xf.modWeights[i] := Abs(StrToFloat(t[i]));
        t.Destroy;
      end;
    end
    else if (attrib_name = 'opacity') and (not isFinalXForm) then
      xf.transOpacity := GetFloatPart(String(attrib_match), re_attrib, 2, 1)
    else if (attrib_name = 'name') and (not isFinalXForm) then
      xf.TransformName := GetStringPart(String(attrib_match), re_attrib, 2, '')
    else if (attrib_name = 'plotmode') and (not isFinalXForm) then
      xf.transOpacity := StrToFloat(IfThen(lowercase(GetStringPart(String(attrib_match), re_attrib, 2, '')) = 'off', '0', '1'))
    else if (attrib_name = 'coefs') then
    begin
      token_part := GetStringPart(String(attrib_match), re_attrib, 2, '1 0 0 1 0 0');
      xf.c[0][0] := GetFloatPart(token_part, re_coefs, 1, 1);
      xf.c[0][1] := GetFloatPart(token_part, re_coefs, 2, 0);
      xf.c[1][0] := GetFloatPart(token_part, re_coefs, 3, 0);
      xf.c[1][1] := GetFloatPart(token_part, re_coefs, 4, 1);
      xf.c[2][0] := GetFloatPart(token_part, re_coefs, 5, 0);
      xf.c[2][1] := GetFloatPart(token_part, re_coefs, 6, 0);
    end
    else if (attrib_name = 'post') then
    begin
      token_part := GetStringPart(String(attrib_match), re_attrib, 2, '1 0 0 1 0 0');
      xf.p[0][0] := GetFloatPart(token_part, re_coefs, 1, 1);
      xf.p[0][1] := GetFloatPart(token_part, re_coefs, 2, 0);
      xf.p[1][0] := GetFloatPart(token_part, re_coefs, 3, 0);
      xf.p[1][1] := GetFloatPart(token_part, re_coefs, 4, 1);
      xf.p[2][0] := GetFloatPart(token_part, re_coefs, 5, 0);
      xf.p[2][1] := GetFloatPart(token_part, re_coefs, 6, 0);
    end
    else if (attrib_name = 'color') then
      xf.color := GetFloatPart(String(attrib_match), re_attrib, 2, 0)
    else if (attrib_name = 'var_color') then
      xf.vc := GetFloatPart(String(attrib_match), re_attrib, 2, 1)
    else if ((String(attrib_name) = 'symmetry') or
      (String(attrib_name) = 'weight') or (String(attrib_name) = 'color_speed')
      or (String(attrib_name) = 'chaos') or (String(attrib_name) = 'opacity') or
      (String(attrib_name) = 'name') or (String(attrib_name) = 'plotmode')) and
      (isFinalXForm) then
    begin
      // EmitWarning('Malformed attribute "xform.' + attrib_name + '" - ignoring');
      // LogWrite('WARNING|' +'Malformed attribute "xform.' + attrib_name + '" - ignoring', 'parser.log');
      attrib_success := false;
    end
    else if (IsRegisteredVariation(String(attrib_name))) then
    begin
      for i := 0 to GetTotalVariationCount - 1 do
      begin
        if lowercase(GetVariationNameByIndex(i)) = lowercase(String(attrib_name)) then
        begin
          xf.SetVariation(i, GetFloatPart(String(attrib_match), re_attrib, 2, 0));
          v_set := true;
          break;
        end;
      end;
    end
    else if (IsRegisteredVariable(String(attrib_name))) then
    begin
      d := GetFloatPart(String(attrib_match), re_attrib, 2, 0);
      xf.SetVariable(String(attrib_name), d);
    end
    else attrib_success := false;

    found_attrib := find_attribs.MatchAgain;
  end;

  if (isFinalXForm) then
  begin
    xf.symmetry := 1;
    xf.color := 0;
  end;

  find_attribs.Free;
end;

// Replace...
function SaveCpToXmlCompatible(var xml: string;
  const cp1: TControlPoint): boolean;
  function ColorToXmlCompact(cp1: TControlPoint): string;
  var
    i: integer;
  begin
    Result := '   <palette count="256" format="RGB">';
    for i := 0 to 255 do
    begin
      if ((i and 7) = 0) then
        Result := Result + #13#10 + '      ';
      Result := Result + IntToHex(cp1.cmap[i, 0], 2) + IntToHex(cp1.cmap[i, 1],
        2) + IntToHex(cp1.cmap[i, 2], 2);
    end;
    Result := Result + #13#10 + '   </palette>';
  end;

var
  t, i { , j } : integer;
  FileList: TStringList;
  x, y: double;
  parameters: string;
  str: string;
begin
  FileList := TStringList.Create;
  x := cp1.center[0];
  y := cp1.center[1];

  // if cp1.cmapindex >= 0 then pal := pal + 'gradient="' + IntToStr(cp1.cmapindex) + '" ';

  try
    parameters := 'version="Apophysis 7X" ';
    if cp1.time <> 0 then
      parameters := parameters + format('time="%g" ', [cp1.time]);

    parameters := parameters + 'size="' + IntToStr(cp1.Width) + ' ' +
      IntToStr(cp1.Height) + format('" center="%g %g" ', [x, y]) +
      format('scale="%g" ', [cp1.pixels_per_unit]);

    if cp1.fangle <> 0 then
      parameters := parameters + format('angle="%g" ', [cp1.fangle]) +
        format('rotate="%g" ', [-180 * cp1.fangle / PI]);
    if cp1.zoom <> 0 then
      parameters := parameters + format('zoom="%g" ', [cp1.zoom]);

    // 3d
    if cp1.cameraPitch <> 0 then
      parameters := parameters + format('cam_pitch="%g" ', [cp1.cameraPitch]);
    if cp1.cameraYaw <> 0 then
      parameters := parameters + format('cam_yaw="%g" ', [cp1.cameraYaw]);
    if cp1.cameraPersp <> 0 then
      parameters := parameters + format('cam_perspective="%g" ',
        [cp1.cameraPersp]);
    if cp1.cameraZpos <> 0 then
      parameters := parameters + format('cam_zpos="%g" ', [cp1.cameraZpos]);
    if cp1.cameraDOF <> 0 then
      parameters := parameters + format('cam_dof="%g" ', [cp1.cameraDOF]);
    //
    parameters := parameters +
      format('oversample="%d" filter="%g" quality="%g" ',
      [cp1.spatial_oversample, cp1.spatial_filter_radius, cp1.sample_density]);
    if cp1.nbatches <> 1 then
      parameters := parameters + 'batches="' + IntToStr(cp1.nbatches) + '" ';

    parameters := parameters + format('background="%g %g %g" ',
      [cp1.background[0] / 255, cp1.background[1] / 255,
      cp1.background[2] / 255]) + format('brightness="%g" ', [cp1.brightness]) +
      format('gamma="%g" ', [cp1.gamma]);

    if cp1.vibrancy <> 1 then
      parameters := parameters + format('vibrancy="%g" ', [cp1.vibrancy]);

    if cp1.gamma_threshold <> 0 then
      parameters := parameters + format('gamma_threshold="%g" ',
        [cp1.gamma_threshold]);

    if cp1.soloXform >= 0 then
      parameters := parameters + format('soloxform="%d" ', [cp1.soloXform]);

    parameters := parameters + format('estimator_radius="%g" ', [cp1.estimator])
      + format('estimator_minimum="%g" ', [cp1.estimator_min]) +
      format('estimator_curve="%g" ', [cp1.estimator_curve]);
    if (cp1.enable_de) then
      parameters := parameters + ('enable_de="1" ')
    else
      parameters := parameters + ('enable_de="0" ');

    str := '';
    for i := 0 to cp1.used_plugins.count - 1 do
    begin
      str := str + cp1.used_plugins[i];
      if (i = cp1.used_plugins.count - 1) then
        break;
      str := str + ' ';
    end;
    parameters := parameters + format('plugins="%s" ', [str]);

    FileList.Add('<flame name="' + cp1.name + '" ' + parameters + '>');
    { Write transform parameters }
    t := cp1.NumXForms;
    for i := 0 to t - 1 do
      FileList.Add(cp1.XForm[i].ToXMLString);
    if cp1.HasFinalXForm then
    begin
      // 'enabled' flag disabled in this release
      FileList.Add(cp1.XForm[t].FinalToXMLString(cp1.finalXformEnabled));
    end;

    { Write palette data }
    // if exporting or OldPaletteFormat then
    // FileList.Add(ColorToXml(cp1))
    // else
    FileList.Add(ColorToXmlCompact(cp1));

    FileList.Add('</flame>');
    xml := FileList.text;
    Result := true;
  finally
    FileList.Free
  end;
end;

end.
