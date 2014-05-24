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
//{$D-,L-,O+,Q-,R-,Y-,S-}
unit Options;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  StdCtrls, ComCtrls, ExtCtrls, Buttons, Registry, Mask, CheckLst,
  MMSystem, Translation, RegexHelper, FileCtrl, StrUtils, ShellAPI, ShlObj;

type
  TOptionsForm = class(TForm)
    btnOK: TButton;
    btnCancel: TButton;
    OpenDialog: TOpenDialog;
    Label45: TLabel;
    GroupBox15: TGroupBox;
    btnBrowseSound: TSpeedButton;
    btnPlay: TSpeedButton;
    Label44: TLabel;
    txtSoundFile: TEdit;
    chkPlaysound: TCheckBox;
    Tabs: TPageControl;
    GeneralPage: TTabSheet;
    SpeedButton1: TSpeedButton;
    pnlJPEGQuality: TPanel;
    chkConfirmDel: TCheckBox;
    chkOldPaletteFormat: TCheckBox;
    chkConfirmExit: TCheckBox;
    chkConfirmStopRender: TCheckBox;
    cbUseTemplate: TCheckBox;
    cbMissingPlugin: TCheckBox;
    cbEmbedThumbs: TCheckBox;
    chkShowRenderStats: TCheckBox;
    pnlMultithreading: TPanel;
    cbNrTheads: TComboBox;
    pnlPNGTransparency: TPanel;
    grpGuidelines: TGroupBox;
    cbGL: TCheckBox;
    pnlCenterLine: TPanel;
    shCenterLine: TShape;
    pnlThirdsLine: TPanel;
    shThirdsLine: TShape;
    pnlGRLine: TPanel;
    shGRLine: TShape;
    pnlCenter: TPanel;
    pnlThirds: TPanel;
    pnlGoldenRatio: TPanel;
    rgRotationMode: TRadioGroup;
    rgZoomingMode: TRadioGroup;
    Panel46: TPanel;
    txtLanguageFile: TComboBox;
    cbPNGTransparency: TComboBox;
    txtJPEGquality: TComboBox;
    cbSinglePrecision: TCheckBox;
    EditorPage: TTabSheet;
    GroupBox1: TGroupBox;
    chkUseXFormColor: TCheckBox;
    chkHelpers: TCheckBox;
    rgReferenceMode: TRadioGroup;
    GroupBox21: TGroupBox;
    chkAxisLock: TCheckBox;
    chkExtendedEdit: TCheckBox;
    chkXaosRebuild: TCheckBox;
    grpEditorColors: TGroupBox;
    pnlBackground: TPanel;
    pnlReferenceC: TPanel;
    pnlHelpers: TPanel;
    pnlGrid: TPanel;
    pnlBackColor: TPanel;
    shBackground: TShape;
    pnlReference: TPanel;
    shRef: TShape;
    pnlHelpersColor: TPanel;
    shHelpers: TShape;
    pnlGridColor1: TPanel;
    shGC1: TShape;
    pnlGridColor2: TPanel;
    shGC2: TShape;
    chkShowAllXforms: TCheckBox;
    chkEnableEditorPreview: TCheckBox;
    Panel48: TPanel;
    tbEPTransparency: TTrackBar;
    DisplayPage: TTabSheet;
    GroupBox2: TGroupBox;
    Panel8: TPanel;
    Panel9: TPanel;
    Panel10: TPanel;
    txtHighQuality: TEdit;
    txtMediumQuality: TEdit;
    txtLowQuality: TEdit;
    grpRendering: TGroupBox;
    Panel1: TPanel;
    Panel2: TPanel;
    Panel3: TPanel;
    Panel4: TPanel;
    Panel5: TPanel;
    Panel6: TPanel;
    Panel7: TPanel;
    txtGammaThreshold: TEdit;
    txtFilterRadius: TEdit;
    txtOversample: TEdit;
    txtVibrancy: TEdit;
    txtBrightness: TEdit;
    txtGamma: TEdit;
    txtSampleDensity: TEdit;
    GroupBox20: TGroupBox;
    Label48: TLabel;
    chkShowTransparency: TCheckBox;
    chkExtendMainPreview: TCheckBox;
    pnlExtension: TPanel;
    cbExtendPercent: TComboBox;
    chkUseSmallThumbs: TCheckBox;
    PathsPage: TTabSheet;
    btnDefGradient: TSpeedButton;
    btnSmooth: TSpeedButton;
    SpeedButton2: TSpeedButton;
    btnHelp: TSpeedButton;
    Label49: TLabel;
    btnFindDefaultSaveFile: TSpeedButton;
    chkRememberLastOpen: TCheckBox;
    Panel39: TPanel;
    txtDefParameterFile: TEdit;
    Panel40: TPanel;
    txtDefSmoothFile: TEdit;
    Panel41: TPanel;
    Panel43: TPanel;
    txtLibrary: TEdit;
    txtHelp: TEdit;
    cbEnableAutosave: TCheckBox;
    Panel44: TPanel;
    txtDefaultSaveFile: TEdit;
    Panel45: TPanel;
    cbFreq: TComboBox;
    btnPluginPath: TSpeedButton;
    Panel50: TPanel;
    txtPluginFolder: TEdit;
    procedure chkEnableEditorPreviewClick(Sender: TObject);
    procedure SpeedButton1Click(Sender: TObject);
    procedure btnCancelClick(Sender: TObject);
    procedure FormShow(Sender: TObject);
    procedure btnOKClick(Sender: TObject);
    procedure btnDefGradientClick(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure btnSmoothClick(Sender: TObject);
    procedure SpeedButton2Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure pnlBackColorClick(Sender: TObject);
    procedure pnlReferenceClick(Sender: TObject);
    procedure pnlGridColor1Click(Sender: TObject);
    procedure pnlGridColor2Click(Sender: TObject);
    procedure pnlHelpersColorClick(Sender: TObject);
    procedure btnBrowseSoundClick(Sender: TObject);
    procedure pnlCenterLineClick(Sender: TObject);
    procedure pnlThirdsLineClick(Sender: TObject);
    procedure pnlGRLineClick(Sender: TObject);
    procedure btnPlayClick(Sender: TObject);
    procedure shCenterLineMouseUp(Sender: TObject; Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
    procedure shThirdsLineMouseUp(Sender: TObject; Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
    procedure shGRLineMouseUp(Sender: TObject; Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
    procedure UpdateShapeColors;
    procedure shBackgroundMouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure shGC1MouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure shGC2MouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure shRefMouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure shHelpersMouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure chkRememberLastOpenClick(Sender: TObject);
    procedure chkUseSmallThumbsClick(Sender: TObject);
    procedure btnFindDefaultSaveFileClick(Sender: TObject);
    procedure cbEnableAutosaveClick(Sender: TObject);
    procedure btnHelpClick(Sender: TObject);
    procedure cbGLClick(Sender: TObject);
    procedure btnPluginPathClick(Sender: TObject);

  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  OptionsForm: TOptionsForm;

implementation

{$R *.DFM}

uses
  Main, Global, Editor, ControlPoint, XFormMan, Adjust;

procedure TOptionsForm.btnCancelClick(Sender: TObject);
begin
  Close;
end;

procedure TOptionsForm.UpdateShapeColors;
begin
  shBackground.Brush.Color := pnlBackColor.Color;
  shGC1.Brush.Color := pnlGridColor1.Color;
  shGC2.Brush.Color := pnlGridColor2.Color;
  shRef.Brush.Color := pnlReference.Color;
  shHelpers.Brush.Color := pnlHelpersColor.Color;
  shCenterLine.Brush.Color := pnlCenterLine.Color;
  shThirdsLine.Brush.Color := pnlThirdsLine.Color;
  shGRLine.Brush.Color := pnlGRLine.Color;
end;

procedure TOptionsForm.FormShow(Sender: TObject);
var
  Registry: TRegistry;
  i, j: integer;
  s1, s2: string;
begin
  { Read position from registry }
  Registry := TRegistry.Create;
  try
    Registry.RootKey := HKEY_CURRENT_USER;
    if Registry.OpenKey('Software\' + APP_NAME + '\Forms\Options', False) then
    begin
      if Registry.ValueExists('Left') then
        OptionsForm.Left := Registry.ReadInteger('Left');
      if Registry.ValueExists('Top') then
        OptionsForm.Top := Registry.ReadInteger('Top');
    end;
    Registry.CloseKey;
  finally
    Registry.Free;
  end;

  { General tab }
  txtDefParameterFile.Text := DefFlameFile;
  txtDefSmoothFile.Text := defSmoothPaletteFile;
//  chkResize.checked := ResizeOnLoad;
  if NrTreads <= 1 then
    cbNrTheads.ItemIndex := 0
  else begin
    // not with fucking Delphi 2005... :(
    //cbNrTheads.text := intTostr(NrTreads);

    // Hack
    cbNrTheads.ItemIndex := 0;
    case NrTreads of
      2: cbNrTheads.ItemIndex := 1;
      3: cbNrTheads.ItemIndex := 2;
      4: cbNrTheads.ItemIndex := 3;
      5: cbNrTheads.ItemIndex := 4;
      6: cbNrTheads.ItemIndex := 5;
      7: cbNrTheads.ItemIndex := 6;
      8: cbNrTheads.ItemIndex := 7;
      9: cbNrTheads.ItemIndex := 8;
      10: cbNrTheads.ItemIndex := 9;
      11: cbNrTheads.ItemIndex := 10;
      12: cbNrTheads.ItemIndex := 11;
    end;
  end;

  chkConfirmDel.Checked := ConfirmDelete;
  chkOldPaletteFormat.Checked := OldPaletteFormat;
  chkConfirmExit.Checked := ConfirmExit;
  chkConfirmStopRender.Checked := ConfirmStopRender;
  chkRememberLastOpen.Checked := RememberLastOpenFile;
  chkUseSmallThumbs.Checked := UseSmallThumbnails;
  cbUseTemplate.Checked := AlwaysCreateBlankFlame;
  cbMissingPlugin.Checked := WarnOnMissingPlugin;
  cbEmbedThumbs.Checked := EmbedThumbnails;
  //cbSinglePrecision.Checked := SingleBuffer;

  rgRotationMode.ItemIndex := MainForm_RotationMode;
  if PreserveQuality then
    rgZoomingMode.ItemIndex := 0
  else
    rgZoomingMode.ItemIndex := 1;
  txtJPEGQuality.text := IntToStr(JPEGQuality);

  chkPlaySound.Checked := PlaySoundOnRenderComplete;
  txtSoundFile.Text := RenderCompleteSoundFile;

  //cbInternalBitsPerSample.ItemIndex := InternalBitsPerSample;


  { Editor }
//  rgReferenceMode.ItemIndex := ReferenceMode;
  chkUseXFormColor.checked := UseTransformColors;
  chkHelpers.Checked := HelpersEnabled;
  chkExtendedEdit.Checked := ExtEditEnabled;
  chkAxisLock.Checked := TransformAxisLock;
  chkXaosRebuild.Checked := RebuildXaosLinks;
  chkShowAllXforms.Checked := ShowAllXforms;
  chkEnableEditorPreview.Checked := EnableEditorPreview;
  tbEPTransparency.Position := EditorPreviewTransparency;
  chkEnableEditorPreviewClick(self);

  { Display tab }
  txtSampleDensity.Text := FloatToStr(defSampleDensity);
  txtGamma.Text := FloatToStr(defGamma);
  txtBrightness.Text := FloatToStr(defBrightness);
  txtVibrancy.Text := FloatToStr(defVibrancy);
  txtOversample.Text := IntToStr(defOversample);
  txtFilterRadius.Text := FloatToStr(defFilterRadius);
  txtGammaThreshold.Text := FloatToStr(defGammaThreshold);

  txtLowQuality.Text := FloatToStr(prevLowQuality);
  txtMediumQuality.Text := FloatToStr(prevMediumQuality);
  txtHighQuality.Text := FloatToStr(prevHighQuality);

  pnlBackColor.Color := TColor(EditorBkgColor);
  pnlGridColor1.Color := GridColor1;
  pnlGridColor2.Color := GridColor2;
  pnlReference.color := TColor(ReferenceTriangleColor);

  cbPNGTransparency.ItemIndex :=  PNGTransparency;
  chkShowTransparency.Checked := ShowTransparency;
  cbExtendPercent.Text := FloatToStr((MainPreviewScale - 1) / 0.02);
  chkExtendMainPreview.Checked := ExtendMainPreview;

  chkShowRenderStats.Checked := ShowRenderStats;

  { Environment tab }
  txtHelp.Text := HelpPath;
  txtLibrary.text := defLibrary;
  Label45.Visible := false;

  cbEnableAutosave.Checked := AutoSaveEnabled;
  txtDefaultSaveFile.Text := AutoSavePath;
  cbFreq.ItemIndex := AutoSaveFreq;

  cbEnableAutosaveClick(nil);

  pnlCenterLine.Color := TColor(LineCenterColor);
  pnlThirdsLine.Color := TColor(LineThirdsColor);
  pnlGRLine.Color := TColor(LineGRColor);
  cbGL.Checked := EnableGuides;
  cbGLClick(nil);
  txtPluginFolder.Text := PluginPath;

  UpdateShapeColors;

  j := -1;
  txtLanguageFile.Items.Clear;
  for i := 0 to AvailableLanguages.Count-1 do begin
    if AvailableLanguages.Strings[i] = '' then begin
      txtLanguageFile.Items.Add('Default (English)');
    end else begin
      LanguageInfo(AvailableLanguages.Strings[i], s1, s2);
      if (s2 <> '') then s1 := s2 + ' (' + s1 + ')';
      txtLanguageFile.Items.Add(s1);
    end;
    if (lowercase(AvailableLanguages.Strings[i]) = lowercase(languagefile)) then
      j := i;

  end;
  txtLanguageFile.ItemIndex := j;
end;

procedure TOptionsForm.btnOKClick(Sender: TObject);
var
  i: integer;
  warn: boolean;
begin
  warn := (LanguageFile <> AvailableLanguages[txtLanguageFile.ItemIndex]) or (UseSmallThumbnails <> chkUseSmallThumbs.Checked);

  { General tab }
  JPEGQuality := StrToInt(txtJPEGQuality.text);
  if NumTries < 1 then Numtries := 1;
  if Trylength < 100 then trylength := 100;
  if JPEGQuality > 100 then JPEGQuality := 100;
  if JPEGQuality < 1 then JPEGQuality := 100;
  if BatchSize < 1 then BatchSize := 1;
  if BatchSize > 300 then BatchSize := 300;

  PNGTransparency := cbPNGTransparency.ItemIndex;
  ShowTransparency := chkShowTransparency.Checked;

  NrTreads := StrToIntDef(cbNrTheads.text, 0);
  ConfirmDelete := chkConfirmDel.Checked;
  OldPaletteFormat := chkOldPaletteFormat.Checked;
  ConfirmExit := chkConfirmExit.Checked;
  ConfirmStopRender := chkConfirmStopRender.Checked;
  RememberLastOpenFile := chkRememberLastOpen.Checked;
  UseSmallThumbnails := chkUseSmallThumbs.Checked;
  AlwaysCreateBlankFlame := cbUseTemplate.Checked;
  EmbedThumbnails := cbEmbedThumbs.Checked;
  WarnOnMissingPlugin := cbMissingPlugin.Checked;
  LanguageFile := AvailableLanguages.Strings[txtLanguageFile.ItemIndex];
  //SingleBuffer := cbSinglePrecision.Checked;

  MainForm_RotationMode := rgRotationMode.ItemIndex;
  PreserveQuality := (rgZoomingMode.ItemIndex = 0);
//  ResizeOnLoad := chkResize.checked;

  //InternalBitsPerSample := cbInternalBitsPerSample.ItemIndex;
  LineCenterColor := pnlCenterLine.Color;
  LineThirdsColor := pnlThirdsLine.Color;
  LineGRColor := pnlGRLine.Color;
  EnableGuides := cbGL.Checked;

  // Editor
//  ReferenceMode := rgReferenceMode.ItemIndex;
  UseTransformColors := chkUseXFormColor.checked;
  HelpersEnabled := chkHelpers.Checked;
  ShowAllXforms := chkShowAllXforms.Checked;

  ExtEditEnabled := chkExtendedEdit.Checked;
  TransformAxisLock := chkAxisLock.Checked;
  RebuildXaosLinks := chkXaosRebuild.Checked;
  EnableEditorPreview := chkEnableEditorPreview.Checked;
  EditorPreviewTransparency := tbEPTransparency.Position;

  { Display tab }
  defSampleDensity := StrToFloat(txtSampleDensity.Text);
  if defSampleDensity > 100 then defSampleDensity := 100;
  if defSampleDensity <= 0 then defSampleDensity := 0.1;
  defGamma := StrToFloat(txtGamma.Text);
  if defGamma < 0.1 then defGamma := 0.1;
  defBrightness := StrToFloat(txtBrightness.Text);
  if defBrightness < 0.1 then defBrightness := 0.1;
  defVibrancy := StrToFloat(txtVibrancy.Text);
  if defVibrancy < 0 then defVibrancy := 0.1;
  defFilterRadius := StrToFloat(txtFilterRadius.Text);
  if defFilterRadius <= 0 then defFilterRadius := 0.1;
  defGammaThreshold := StrToFloat(txtGammaThreshold.Text);
  if defGammaThreshold < 0 then defGammaThreshold := 0;
  defOversample := StrToInt(txtOversample.Text);
  if defOversample > 4 then defOversample := 4;
  if defOversample < 1 then defOversample := 1;
  prevLowQuality := StrToFloat(txtLowQuality.Text);
  if prevLowQuality > 100 then prevLowQuality := 100;
  if prevLowQuality < 0.01 then prevLowQuality := 0.01;
  prevMediumQuality := StrToFloat(txtMediumQuality.Text);
  if prevMediumQuality > 1000 then prevMediumQuality := 1000;
  if prevMediumQuality < 0.01 then prevMediumQuality := 0.01;
  prevHighQuality := StrToFloat(txtHighQuality.Text);
  if prevHighQuality > 10000 then prevHighQuality := 10000;
  if prevHighQuality < 0.01 then prevHighQuality := 0.01;

  MainPreviewScale := 1 + 0.02 * StrToFloatDef(cbExtendPercent.Text, 0);
  if MainPreviewScale < 1 then MainPreviewScale := 1
  else if MainPreviewScale > 5 then MainPreviewScale := 5;
  ExtendMainPreview := chkExtendMainPreview.Checked;

  ShowRenderStats := chkShowRenderStats.Checked;

  {Paths}
  defLibrary := txtLibrary.text;
  if (not RememberLastOpenFile) then defFlameFile := txtDefParameterFile.Text;
  defSmoothPaletteFile := txtDefSmoothFile.Text;
  PlaySoundOnRenderComplete := chkPlaySound.Checked;
  RenderCompleteSoundFile := txtSoundFile.Text;
  HelpPath := txtHelp.Text;

  PluginPath := txtPluginFolder.Text;
  if (RightStr(PluginPath, 1) <> '\') and (PluginPath <> '') then
    PluginPath := PluginPath + '\';

  AutoSaveEnabled := cbEnableAutosave.Checked;
  AutoSavePath := txtDefaultSaveFile.Text;
  AutoSaveFreq := cbFreq.ItemIndex;

  if (warn) then
    Application.MessageBox(PChar(TextByKey('options-restartnotice')), PChar('Apophysis'), MB_ICONWARNING);

  Close;
end;

procedure TOptionsForm.btnDefGradientClick(Sender: TObject);
var
  fn:string;
begin
  OpenDialog.Filter := TextByKey('common-filter-flamefiles') + '|*.flame|' + TextBykey('common-filter-allfiles') + '|*.*';
  OpenDialog.FileName := '';
  if OpenSaveFileDialog(OptionsForm, '.flame', OpenDialog.Filter, OpenDialog.InitialDir, TextByKey('common-browse'), fn, true, false, false, true) then
  //if OpenDialog.Execute then
  begin
    txtDefParameterFile.text := fn;
  end;
end;

procedure TOptionsForm.FormClose(Sender: TObject; var Action: TCloseAction);
var
  Registry: TRegistry;
begin
  { Write position to registry }
  Registry := TRegistry.Create;
  try
    Registry.RootKey := HKEY_CURRENT_USER;
    if Registry.OpenKey('\Software\' + APP_NAME + '\Forms\Options', True) then
    begin
      Registry.WriteInteger('Top', OptionsForm.Top);
      Registry.WriteInteger('Left', OptionsForm.Left);
    end;
  finally
    Registry.Free;
  end;

end;

procedure TOptionsForm.btnSmoothClick(Sender: TObject);
var
  fn:string;
begin
  OpenDialog.Filter := TextByKey('common-filter-gradientfiles') + '|*.gradient;*.ugr|' + TextBykey('common-filter-allfiles') + '|*.*';
  OpenDialog.InitialDir := ExtractFilePath(defSmoothPaletteFile);
  OpenDialog.FileName := '';
  OpenDialog.DefaultExt := 'ugr';
  if OpenSaveFileDialog(OptionsForm, OpenDialog.DefaultExt, OpenDialog.Filter, OpenDialog.InitialDir, TextByKey('common-browse'), fn, true, false, false, true) then
  //if OpenDialog.Execute then
  begin
    txtDefSmoothFile.text := fn;
  end;
end;

procedure TOptionsForm.SpeedButton2Click(Sender: TObject);
var
  fn:string;
begin
  OpenDialog.Filter := TextByKey('common-filter-scriptfiles') + '|*.aposcript;*.asc|' + TextBykey('common-filter-allfiles') + '|*.*';;
  OpenDialog.InitialDir := ExtractFilePath(defLibrary);
  OpenDialog.FileName := '';
  if OpenSaveFileDialog(OptionsForm, '.asc', OpenDialog.Filter, OpenDialog.InitialDir, TextByKey('common-browse'), fn, true, false, false, true) then
  //if OpenDialog.Execute then
  begin
    txtLibrary.text := fn;
  end;
end;

procedure TOptionsForm.FormCreate(Sender: TObject);
var
  i: integer;
begin
  btnOK.Caption := TextByKey('common-ok');
	btnCancel.Caption := TextByKey('common-cancel');
	Panel8.Caption := TextByKey('common-lowquality');
	Panel9.Caption := TextByKey('common-mediumquality');
	Panel10.Caption := TextByKey('common-highquality');
	Panel44.Caption := TextByKey('common-filename');
	Panel7.Caption := TextByKey('common-filterradius');
	Panel1.Caption := TextByKey('common-density');
	Panel6.Caption := TextByKey('common-oversample');
	Panel2.Caption := TextByKey('common-gamma');
	Panel3.Caption := TextByKey('common-brightness');
	Panel4.Caption := TextByKey('common-vibrancy');
	Panel5.Caption := TextByKey('common-gammathreshold');
	cbPNGTransparency.Items[1] := TextByKey('common-enabled');
	cbPNGTransparency.Items[0] := TextByKey('common-disabled');
	Label49.Caption := TextByKey('common-minutes');
  //Panel49.Caption := TextByKey('common-filename') + ' (x64)';
  Panel48.Caption := TextByKey('options-tab-editor-previewtransparency');
  chkEnableEditorPreview.Caption := TextByKey('options-tab-editor-enablepreview');
	self.Caption := TextByKey('options-title');
	GeneralPage.Caption := TextByKey('options-tab-general-title');
  Panel46.Caption := TextByKey('options-tab-general-language');
	pnlMultithreading.Caption := TextByKey('options-tab-general-multithreading');
	cbNrTheads.Items[0] := TextByKey('options-tab-general-multithreading-off');
	//pnlBufferDepth.Caption := TextByKey('options-tab-general-bufferdepth');
	pnlJPEGQuality.Caption := TextByKey('options-tab-general-jpegquality');
	pnlPNGTransparency.Caption := TextByKey('options-tab-general-pngtransparency');
	chkShowRenderStats.Caption := TextByKey('options-tab-general-showextendedstatistics');
	chkConfirmDel.Caption := TextByKey('options-tab-general-confirmdelete');
	chkConfirmExit.Caption := TextByKey('options-tab-general-confirmexit');
	chkconfirmStopRender.Caption := TextByKey('options-tab-general-confirmrenderstop');
	chkOldPaletteFormat.Caption := TextByKey('options-tab-general-oldgradientformat');
	cbUseTemplate.Caption := TextByKey('options-tab-general-alwaysblankflame');
	cbMissingplugin.Caption := TextByKey('options-tab-general-enablemissingpluginswarning');
	cbEmbedThumbs.Caption := TextByKey('options-tab-general-enablethumbnailembedding');
	rgRotationMode.Caption := TextByKey('options-tab-general-rotatemode');
	rgRotationMode.Items[0] := TextByKey('options-tab-general-rotateimage');
	rgRotationMode.Items[1] := TextByKey('options-tab-general-rotateframe');
	rgZoomingMode.Caption := TextByKey('options-tab-general-zoommode');
	rgZoomingMode.Items[0] := TextByKey('options-tab-general-preservequality');
	rgZoomingMode.Items[1] := TextByKey('options-tab-general-preservespeed');
	grpGuidelines.Caption := TextByKey('options-tab-general-guides');
	cbGl.Caption := TextByKey('options-tab-general-enableguides');
	pnlCenter.Caption := TextByKey('options-tab-general-guidecentercolor');
	pnlThirds.Caption := TextByKey('options-tab-general-guidethirdscolor');
	pnlGoldenRatio.Caption := TextByKey('options-tab-general-guidegoldenratiocolor');
	EditorPage.Caption := TextByKey('options-tab-editor-title');
	GroupBox1.Caption := TextByKey('options-tab-editor-editorgraph');
	GroupBox21.Caption := TextByKey('options-tab-editor-editordefaults');
	rgReferenceMode.Caption := TextByKey('options-tab-editor-referencetriangle');
	chkUseXFormColor.Caption := TextByKey('options-tab-editor-usetransformcolor');
	chkHelpers.Caption := TextByKey('options-tab-editor-helperlines');
	chkShowAllXForms.Caption := TextByKey('options-tab-editor-alwaysshowbothtransformtypes');
	pnlBackground.Caption := TextByKey('options-tab-editor-backgroundcolor');
	pnlGrid.Caption := TextByKey('options-tab-editor-gridcolors');
	pnlReferenceC.Caption := TextByKey('options-tab-editor-referencecolor');
	pnlHelpers.Caption := TextByKey('options-tab-editor-helpercolors');
	chkExtendedEdit.Caption := TextByKey('options-tab-editor-extendededit');
	chkAxisLock.Caption := TextByKey('options-tab-editor-locktransformaxes');
	chkXaosRebuild.Caption := TextByKey('options-tab-editor-rebuildxaoslinks');
	rgReferenceMode.Items[0] := TextByKey('options-tab-editor-normalreference');
	rgReferenceMode.Items[1] := TextByKey('options-tab-editor-proportionalreference');
	rgReferenceMode.Items[2] := TextByKey('options-tab-editor-wanderingreference');
	DisplayPage.Caption := TextByKey('options-tab-display-title');
	grpRendering.Caption := TextByKey('options-tab-display-rendering');
	GroupBox2.Caption := TextByKey('options-tab-display-previewdensity');
	GroupBox20.Caption := TextByKey('options-tab-display-mainpreview');
	chkExtendMainPreview.Caption := TextByKey('options-tab-display-extendpreviewbuffer');
	pnlExtension.Caption := TextByKey('options-tab-display-extenspreviewbufferlabel');
	chkShowTransparency.Caption := TextByKey('options-tab-display-showtransparency');
	chkUseSmallThumbs.Caption := TextByKey('options-tab-display-usesmallthumbs');
  PathsPage.Caption := TextByKey('options-tab-environment-title');
	Panel39.Caption := TextByKey('options-tab-environment-defaultparams');
	Panel40.Caption := TextByKey('options-tab-environment-smoothpalette');
	Panel41.Caption := TextByKey('options-tab-environment-functionlib');
	Panel43.Caption := TextByKey('options-tab-environment-helpfile');
	chkRememberLastOpen.Caption := TextByKey('options-tab-environment-rememberlastopen');
	cbEnableAutosave.Caption := TextByKey('options-tab-environment-autosave');
	panel45.Caption := TextByKey('options-tab-environment-savefrequency');
  cbSinglePrecision.Caption := TextByKey('options-tab-general-singleprecision');
  grpEditorColors.Caption := TextByKey('editor-tab-color-title');
end;

procedure TOptionsForm.pnlCenterLineClick(Sender: TObject);
begin
  if (not cbGL.Checked) then exit;
  AdjustForm.ColorDialog.Color := pnlCenterLine.Color;
  if AdjustForm.ColorDialog.Execute then
  begin
    pnlCenterLine.Color := AdjustForm.ColorDialog.Color;
    LineCenterColor := Integer(pnlCenterLine.color);
    UpdateShapeColors;
  end;
end;

procedure TOptionsForm.pnlThirdsLineClick(Sender: TObject);
begin
  if (not cbGL.Checked) then exit;
  AdjustForm.ColorDialog.Color := pnlThirdsLine.Color;
  if AdjustForm.ColorDialog.Execute then
  begin
    pnlThirdsLine.Color := AdjustForm.ColorDialog.Color;
    LineThirdsColor := Integer(pnlThirdsLine.color);
    UpdateShapeColors;
  end;
end;

procedure TOptionsForm.pnlGRLineClick(Sender: TObject);
begin
  if (not cbGL.Checked) then exit;
  AdjustForm.ColorDialog.Color := pnlGRLine.Color;
  if AdjustForm.ColorDialog.Execute then
  begin
    pnlGRLine.Color := AdjustForm.ColorDialog.Color;
    LineGRColor := Integer(pnlGRLine.color);
    UpdateShapeColors;
  end;
end;

procedure TOptionsForm.pnlBackColorClick(Sender: TObject);
begin
  AdjustForm.ColorDialog.Color := pnlBackColor.Color;
  if AdjustForm.ColorDialog.Execute then
  begin
    pnlBackColor.Color := AdjustForm.ColorDialog.Color;
    EditorBkgColor := Integer(pnlBackColor.color);
    UpdateShapeColors;
  end;
end;

procedure TOptionsForm.pnlReferenceClick(Sender: TObject);
begin
  AdjustForm.ColorDialog.Color := pnlReference.Color;
  if AdjustForm.ColorDialog.Execute then
  begin
    pnlReference.Color := AdjustForm.ColorDialog.Color;
    ReferenceTriangleColor := Integer(pnlReference.color);
    UpdateShapeColors;
  end;
end;

procedure TOptionsForm.pnlGridColor1Click(Sender: TObject);
begin
  AdjustForm.ColorDialog.Color := pnlGridColor1.Color;
  if AdjustForm.ColorDialog.Execute then
  begin
    pnlGridColor1.Color := AdjustForm.ColorDialog.Color;
    GridColor1 := Integer(pnlGridColor1.color);
    UpdateShapeColors;
  end;
end;

procedure TOptionsForm.pnlGridColor2Click(Sender: TObject);
begin
  AdjustForm.ColorDialog.Color := pnlGridColor2.Color;
  if AdjustForm.ColorDialog.Execute then
  begin
    pnlGridColor2.Color := AdjustForm.ColorDialog.Color;
    GridColor2 := Integer(pnlGridColor2.color);
    UpdateShapeColors;
  end;
end;

procedure TOptionsForm.pnlHelpersColorClick(Sender: TObject);
begin
  AdjustForm.ColorDialog.Color := pnlHelpersColor.Color;
  if AdjustForm.ColorDialog.Execute then
  begin
    pnlHelpersColor.Color := AdjustForm.ColorDialog.Color;
    HelpersColor := Integer(pnlHelpersColor.color);
    UpdateShapeColors;
  end;
end;

procedure TOptionsForm.btnBrowseSoundClick(Sender: TObject);
var
  fn:string;
begin
  OpenDialog.InitialDir := ExtractFilePath(RenderCompleteSoundFile);
  OpenDialog.Filter := 'Waveform files (*.wav)|*.wav';
  OpenDialog.FileName := '';
  if OpenSaveFileDialog(OptionsForm, '.wav', OpenDialog.Filter, OpenDialog.InitialDir, 'Open file...', fn, true, false, false, true) then
  //if OpenDialog.Execute then
  begin
    txtSoundFile.text := fn;
  end;
end;

procedure TOptionsForm.btnPlayClick(Sender: TObject);
begin
  if txtSoundFile.text <> '' then
    sndPlaySound(PChar(txtSoundFile.text), SND_FILENAME or SND_ASYNC)
  else
    sndPlaySound(pchar(SND_ALIAS_SYSTEMASTERISK), SND_ALIAS_ID or SND_NOSTOP or SND_ASYNC);
end;

procedure TOptionsForm.btnPluginPathClick(Sender: TObject);
var
  TitleName : string;
  lpItemID : PItemIDList;
  BrowseInfo : TBrowseInfo;
  DisplayName : array[0..MAX_PATH] of char;
  TempPath : array[0..MAX_PATH] of char;
begin
  FillChar(BrowseInfo, sizeof(TBrowseInfo), #0);
  BrowseInfo.hwndOwner := self.Handle;
  BrowseInfo.pszDisplayName := @DisplayName;
  TitleName := 'Please specify the plugin folder';
  BrowseInfo.lpszTitle := PChar(TitleName);
  BrowseInfo.ulFlags := BIF_RETURNONLYFSDIRS;
  lpItemID := SHBrowseForFolder(BrowseInfo);
  if lpItemId <> nil then begin
    SHGetPathFromIDList(lpItemID, TempPath);
    txtPluginFolder.Text := TempPath;
    GlobalFreePtr(lpItemID);
  end;
end;

procedure TOptionsForm.shBackgroundMouseUp(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
  pnlBackColorClick(Sender);
end;

procedure TOptionsForm.shCenterLineMouseUp(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
  pnlCenterLineClick(Sender);
end;

procedure TOptionsForm.shThirdsLineMouseUp(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
  pnlThirdsLineClick(Sender);
end;

procedure TOptionsForm.shGRLineMouseUp(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
  pnlGRLineClick(Sender);
end;

procedure TOptionsForm.shGC1MouseUp(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
begin
  pnlGridColor1Click(Sender);
end;

procedure TOptionsForm.shGC2MouseUp(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
begin
  pnlGridColor2Click(Sender);
end;

procedure TOptionsForm.shRefMouseUp(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
begin
  pnlReferenceClick(Sender);
end;

procedure TOptionsForm.shHelpersMouseUp(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
  pnlHelpersColorClick(Sender);
end;

procedure TOptionsForm.chkRememberLastOpenClick(Sender: TObject);
begin
  if chkRememberLastOpen.Checked then begin
    txtDefParameterFile.Enabled := false;
    txtDefParameterFile.Text := ''; //LastOpenFile;
    btnDefGradient.Enabled := false;
  end else begin
    txtDefParameterFile.Enabled := true;
    btnDefGradient.Enabled := true;
  end;

  Panel39.Enabled := txtDefParameterFile.Enabled;

  if (Panel39.Enabled) then Panel39.Font.Color := clWindowText
  else Panel39.Font.Color := clGrayText;
end;

procedure TOptionsForm.chkUseSmallThumbsClick(Sender: TObject);
begin
  Label45.Visible := true;
end;

procedure TOptionsForm.btnFindDefaultSaveFileClick(Sender: TObject);
var fn:string;
begin
  OpenDialog.Filter := TextByKey('common-filter-flamefiles') + '|*.flame|' + TextBykey('common-filter-allfiles') + '|*.*';
  OpenDialog.FileName := '';
  if OpenSaveFileDialog(OptionsForm, '.flame', OpenDialog.Filter, OpenDialog.InitialDir, TextByKey('common-browse'), fn, false, false, false, false) then
  //if OpenDialog.Execute then
  begin
    txtDefaultSaveFile.text := fn;
  end;
end;

procedure TOptionsForm.cbEnableAutosaveClick(Sender: TObject);
begin
  Panel44.Enabled := cbEnableAutoSave.Checked;
  Panel45.Enabled := cbEnableAutoSave.Checked;
  Label49.Enabled := cbEnableAutoSave.Checked;
  if (Panel44.Enabled) then Panel44.Font.Color := clWindowText
  else Panel44.Font.Color := clGrayText;
  if (Panel45.Enabled) then Panel45.Font.Color := clWindowText
  else Panel45.Font.Color := clGrayText;
  txtDefaultSaveFile.Enabled := cbEnableAutoSave.Checked;
  btnFindDefaultSaveFile.Enabled := cbEnableAutoSave.Checked;
  cbFreq.Enabled := cbEnableAutoSave.Checked;
end;

procedure TOptionsForm.btnHelpClick(Sender: TObject);
var
  fn:string;
begin
  OpenDialog.Filter := TextBykey('common-filter-allfiles') + '|*.*';
  OpenDialog.InitialDir := ExtractFilePath(helpPath);
  OpenDialog.FileName := '';
  if OpenSaveFileDialog(OptionsForm, '', OpenDialog.Filter, OpenDialog.InitialDir, TextByKey('common-browse'), fn, true, false, false, true) then
  //if OpenDialog.Execute then
  begin
    txtHelp.text := fn;
  end;
end;

procedure TOptionsForm.cbGLClick(Sender: TObject);
begin
  pnlCenter.Enabled := cbGL.Checked;
  pnlThirds.Enabled := cbGL.Checked;
  pnlGoldenRatio.Enabled := cbGL.Checked;

  if (pnlCenter.Enabled) then pnlCenter.Font.Color := clWindowText
  else pnlCenter.Font.Color := clGrayText;

  if (pnlThirds.Enabled) then pnlThirds.Font.Color := clWindowText
  else pnlThirds.Font.Color := clGrayText;

  if (pnlGoldenRatio.Enabled) then pnlGoldenRatio.Font.Color := clWindowText
  else pnlGoldenRatio.Font.Color := clGrayText;
end;

procedure TOptionsForm.SpeedButton1Click(Sender: TObject);
var fn, fn2, s1, s2:string; i : integer;
begin
  OpenDialog.Filter := 'Extensible Markup Language Files (*.xml)|*.xml';
  OpenDialog.InitialDir := ExtractFilePath(helpPath);
  OpenDialog.FileName := '';
  if OpenSaveFileDialog(OptionsForm, '', OpenDialog.Filter, OpenDialog.InitialDir, TextByKey('common-browse'), fn, true, false, false, true) then
  //if OpenDialog.Execute then
  begin
    fn2 := ExtractFilePath(Application.ExeName) + 'Languages\' + ExtractFileName(fn);
    LanguageInfo(fn, s1, s2);
    if s1 <> '' then begin
      if not DirectoryExists(ExtractFilePath(Application.ExeName) + 'Languages\') then
        CreateDirectory(PChar(ExtractFilePath(Application.ExeName) + 'Languages\'), nil);
      if (lowercase(ExtractFilePath(fn)) <> lowercase(ExtractFilePath(Application.ExeName) + 'Languages\')) then
        CopyFile(PChar(fn), PChar(fn2), False);
      AvailableLanguages.Add(fn2);
      i := AvailableLanguages.Count - 1;
      if (s2 <> '') then
        s1 := s2 + ' (' + s1 + ')';
      txtLanguageFile.Items.Add(s1);
      txtLanguageFile.ItemIndex := txtLanguageFile.Items.Count - 1;
    end else begin
      Application.MessageBox(PChar(TextByKey('common-invalidformat')), PChar('Apophysis'), MB_ICONERROR);
    end;
  end;
end;

procedure TOptionsForm.chkEnableEditorPreviewClick(Sender: TObject);
begin
  Panel48.Enabled := chkEnableEditorPreview.Checked;
  if chkEnableEditorPreview.Checked then
    Panel48.Font.Color := clWindowText
  else Panel48.Font.Color := clGrayText;
  tbEPTransparency.Enabled := chkEnableEditorPreview.Checked;
end;

end.

