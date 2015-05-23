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

unit Main;

{$Include 'delphiversion.pas'}

interface

uses
  Windows, Forms, Dialogs, Menus, Controls, ComCtrls,
  ToolWin, StdCtrls, Classes, Messages, ExtCtrls, ImgList,
  Jpeg, SyncObjs, SysUtils, ClipBrd, Graphics, Math,
  ExtDlgs, AppEvnts, ShellAPI, Registry,
  Global, Xform, XFormMan, ControlPoint, CMap,
  RenderThread, RenderingCommon, RenderingInterface, (*ParameterIO,*)
  LibXmlParser, LibXmlComps, PngImage, XPMan,
  StrUtils, LoadTracker, CheckLst,
  CommandLine, RegularExpressionsCore, MissingPlugin, Translation,
  RegexHelper, Vcl.RibbonLunaStyleActnCtrls, Vcl.Ribbon,
  Vcl.PlatformDefaultStyleActnCtrls, System.Actions, Vcl.ActnList, Vcl.ActnMan,
  Vcl.RibbonSilverStyleActnCtrls, Vcl.ActnCtrls, Vcl.ActnMenus,
  Vcl.RibbonActnMenus, Vcl.StdActns, System.ImageList,
  Vcl.RibbonObsidianStyleActnCtrls, UIRibbonForm, UIRibbon, UIRibbonCommands,
  ApophysisRibbon, FlameListView;//, WinInet;

const
  PixelCountMax = 32768;
  RS_A1 = 0;
  RS_DR = 1;
  RS_XO = 2;
  RS_VO = 3;

  randFilename = 'Apophysis7X.rand';
  undoFilename = 'Apophysis7X.undo';
  templateFilename = 'Apophysis7X.temp';
  templatePath = '\templates';
  scriptPath = '\scripts';

type
  TMouseMoveState = (msUsual, msZoomWindow, msZoomOutWindow, msZoomWindowMove,
                     msZoomOutWindowMove, msDrag, msDragMove, msRotate, msRotateMove, msPitchYaw, msHeight);

type
  TWin32Version = (wvUnknown, wvWin95, wvWin98, wvWinNT, wvWin2000, wvWinXP, wvWinVista, wvWin7, wvWinFutureFromOuterSpace);

type
  pRGBTripleArray = ^TRGBTripleArray;
  TRGBTripleArray = array[0..PixelCountMax - 1] of TRGBTriple;
  TMatrix = array[0..1, 0..1] of double;

  TMainForm = class(TUIRibbonForm)
   // Menus
    PreviewPanelPopupMenu: TPopupMenu;
      PreviewPanelMenuUndoItem: TMenuItem;
      PreviewPanelMenuRedoItem: TMenuItem;
      PreviewPanelMenuSeparator0: TMenuItem;
      PreviewPanelMenuResetLocationItem: TMenuItem;
      PreviewPanelMenuSeparator1: TMenuItem;
      PreviewPanelMenuFullscreenItem : TMenuItem;

    ListViewPopupMenu: TPopupMenu;
      ListViewMenuRenameItem: TMenuItem;
      ListViewMenuDeleteItem: TMenuItem;

   // Threads
    AutoSaveTimer: TTimer;
    PreviewRedrawDelayTimer: TTimer;
    ClipboardWatcherEvents: TApplicationEvents;

   // Dialogs
    OpenDialog: TOpenDialog;
    SaveDialog: TSaveDialog;
    ColorDialog: TColorDialog;

   // Fields
    LargeFlameThumbnailsList: TImageList;
    SmallFlameThumbnailsList: TImageList;

   // Vieuals
    ListViewPanel: TPanel;
      ListViewPanelStyleShape: TShape;
      ListView: TListView;

    BetweenListAndPreviewPanelSplitter: TSplitter;

    PreviewPanel: TPanel;
      PreviewImage: TImage;

    StatusPanel: TPanel;
      StatusBar: TStatusBar;
      StatusProgressBar: TProgressBar;

   // Internals
    procedure BeginUpdatePreview;

    procedure AutoSaveTimerCallback(Sender: TObject);
    procedure PreviewRedrawDelayTimerCallback(Sender: TObject);
    procedure ClipboardWatcherEventsCallback(Sender: TObject);

   // Ribbon infrastructure
    procedure RibbonLoaded; override;
    procedure CommandCreated(const Sender: TUIRibbon; const Command: TUICommand); override;

   // Event handlers;
    procedure OnFormCreated(Sender: TObject);
    procedure OnFormShown(Sender: TObject);
    procedure OnFormGotFocus(Sender: TObject);
    procedure OnFormLostFocus(Sender: TObject);
    procedure OnFormKeyStateChanged(Sender: TObject; var Key: Word; Shift: TShiftState);
    procedure OnFormKeyPressed(Sender: TObject; var Key: Char);
    procedure OnFormClosed(Sender: TObject; var Action: TCloseAction);
    procedure OnFormDestroyed(Sender: TObject);

    procedure OnListViewSelectedItemChanging(Sender: TObject; Item: TListItem; Change: TItemChange; var AllowChangeListViewItem: Boolean);
    procedure OnListViewSelectedItemChanged(Sender: TObject; Item: TListItem; Change: TItemChange);
    procedure OnListViewEditCompleted(Sender: TObject; Item: TListItem; var S: string);
    procedure OnListViewMenuRenameClick(Sender: TObject);
    procedure OnListViewMenuDeleteClick(Sender: TObject);

    procedure OnPreviewPanelResized(Sender: TObject);
    procedure OnPreviewPanelMenuUndoClick(Sender: TObject);
    procedure OnPreviewPanelMenuRedoClick(Sender: TObject);
    procedure OnPreviewPanelMenuResetLocationClick(Sender: TObject);
    procedure OnPreviewPanelMenuFullscreenClick(Sender: TObject);
    procedure OnPreviewImageMouseDown(Sender: TObject; Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
    procedure OnPreviewImageMouseMove(Sender: TObject; Shift: TShiftState; X, Y: Integer);
    procedure OnPreviewImageMouseUp(Sender: TObject; Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
    procedure OnPreviewImageDoubleClick(Sender: TObject);

    procedure OnBatchReaderTagEncountered(Sender: TObject; TagName: string; Attributes: TAttrList);
    procedure OnFlameReaderTagEncountered(Sender: TObject; TagName: string; Attributes: TAttrList);
    procedure OnFlameReaderEmptyTagEncountered(Sender: TObject; TagName: string; Attributes: TAttrList);
    procedure OnFlameReaderDataEncountered(Sender: TObject; Content: String);
    procedure OnFlameReaderClosingTagEncountered(Sender: TObject; TagName: String);

  private
  
   // Fields
    PreviewThreadCount: integer;
    PreviewThreadChangingContext: boolean;
    TransparentPreviewImage: TPngObject;

    IsDrawingSelection: boolean;
    CurrentCursorMode: TMouseMoveState;
    CursorCurrent: TRect;
    CursorStart: TRect;
    CursorCurrentAngle: double;
    CursorStartAngle: double;

   // No idea
    SubstSource: TStringList;
    SubstTarget: TStringList;

    FViewPos, FViewOldPos: TSPoint;
    FViewScale: double;
    FClickPitch, FNewPitch: double;
    FClickYaw, FNewYaw: double;
    FShiftState: TShiftState;
    DoNotAskAboutChange: boolean;
    ParseHandledPluginList : boolean;

    // For parsing:
    FinalXformLoaded: boolean;
    ActiveXformSet: integer;
    XMLPaletteFormat: string;
    XMLPaletteCount: integer;

    camDragMode, camDragged, camMM: boolean;
    camDragPos, camDragOld: TPoint;
    camDragValueX, camDragValueY: double;

    procedure CreateSubstMap;
    procedure InsertStrings;
    procedure DrawImageView;
    procedure DrawZoomWindow;
    procedure DrawRotatelines(Angle: double);
    procedure DrawPitchYawLines(YawAngle: double; PitchAngle:double);

    procedure FavoriteClick(Sender: TObject);
    procedure ScriptItemClick(Sender: TObject);
    procedure HandleThreadCompletion(var Message: TMessage);
      message WM_THREAD_COMPLETE;
    procedure HandleThreadTermination(var Message: TMessage);
      message WM_THREAD_TERMINATE;

    procedure UpdateCursorModeCommandStates;

  public

   // Commands
    NewFlameCommand: TUICommandAction;
    OpenBatchCommand: TUICommandAction;
    SaveFlameCommand: TUICommandAction;
    SaveBatchCommand: TUICommandAction;
    RestoreLastAutosaveCommand: TUICommandAction;

    CopyCommand: TUICommandAction;
    PasteCommand: TUICommandAction;
    UndoCommand: TUICommandAction;
    RedoCommand: TUICommandAction;

    RenderFlameCommand: TUICommandAction;
    RenderBatchCommand: TUICommandAction;

    FullscreenPreviewCommand: TUICommandAction;
    ShowEditorCommand: TUICommandAction;
    ShowCameraEditorCommand: TUICommandAction;
    ShowOutputPropertiesCommand: TUICommandAction;
    ShowPaletteCommand: TUICommandAction;
    ShowCanvasCommand: TUICommandAction;
    PaletteFromImageCommand: TUICommandAction;

    RunScriptCommand: TUICommandAction;
    StopScriptCommand: TUICommandAction;
    OpenScriptCommand: TUICommandAction;
    EditScriptCommand: TUICommandAction;
    ManageScriptFavoritesCommand: TUICommandAction;

    PanModeCommand: TUICommandBoolean;
    RotateModeCommand: TUICommandBoolean;
    ZoomInModeCommand: TUICommandBoolean;
    ZoomOutModeCommand: TUICommandBoolean;
    ShowTransparencyCommand: TUICommandBoolean;
    ShowGuidelinesCommand: TUICommandBoolean;
    ShowIconsInListViewCommand: TUICommandBoolean;

    AboutCommand: TUICommandAction;
    HelpCommand: TUICommandAction;
    FractalFlamePublicationCommand: TUICommandAction;
    DonateCommand: TUICommandAction;
    RecentItems: TUICommandAction;
    ShowSettingsCommand: TUICommandAction;
    ExitCommand: TUICommandAction;
  

  // Fields
    CommandLine : TCommandLine;
    Renderer: TRenderThread;
    ListViewManager: TFlameListView;
    Batch: TBatch;

     { Public declarations }
    UndoIndex, UndoMax: integer;
    Center: array[0..1] of double;
    MainZoom: double;
    StartTime: TDateTime;
    AnimPal: TColorMap;
    PrevListItem: TListItem;
    LockListChangeUpdate: boolean;
    CurrentFileName: string;
    UsedThumbnails: TImageList;
    IsLoadingBatch : boolean;
    SurpressHandleMissingPlugins : boolean;
    SelectedFlameNameMemento, FocusedFlameNameMemento: string;
    AllowChangeListViewItemMemento: boolean;

    VarMenus: array of TMenuItem;

    ListXmlScanner : TEasyXmlScanner;
    XmlScanner : TXmlScanner;

    procedure Undo(const args : TUiCommandActionEventArgs);
    procedure Redo(const args : TUiCommandActionEventArgs);

    procedure OpenFractalFlamePublication(const args : TUiCommandActionEventArgs);
    procedure OpenManual(const args : TUiCommandActionEventArgs);
    procedure OpenHelp(const args : TUiCommandActionEventArgs);

    procedure SetCursorMode(const args : TUiCommandBooleanEventArgs);
    procedure SetCursorModePan;
    procedure SetCursorModeRotate;
    procedure SetCursorModeZoomIn;
    procedure SetCursorModeZoomOut;

    procedure SetCanUndo(value: boolean);
    procedure SetCanRedo(value: boolean);
    procedure SetCanPaste(value: boolean);

    procedure OpenBatchFromDisk(const args : TUiCommandActionEventArgs);
    procedure OpenLastAutomaticallySavedBatch(const args : TUiCommandActionEventArgs);
    procedure SaveBatchToDisk(const args : TUiCommandActionEventArgs);
    procedure RenderBatch(const args : TUiCommandActionEventArgs);

    procedure CreateAndSelectNewFlame(const args : TUiCommandActionEventArgs);
    procedure SaveSelectedFlameToBatch(const args : TUiCommandActionEventArgs);
    procedure RenderSelectedFlame(const args : TUiCommandActionEventArgs);
    procedure ResetSelectedFlameCamera(const args : TUiCommandActionEventArgs);
    procedure CopySelectedFlameToClipboard(const args : TUiCommandActionEventArgs);
    procedure ReplaceSelectedFlameWithClipboard(const args : TUiCommandActionEventArgs);
    procedure DeleteSelectedFlame(const args : TUiCommandActionEventArgs);

    procedure RunCurrentScript(const args : TUiCommandActionEventArgs);
    procedure StopCurrentScript(const args : TUiCommandActionEventArgs);
    procedure OpenScriptFromDisk(const args : TUiCommandActionEventArgs);

    procedure CreatePaletteFromImage(const Args: TUiCommandActionEventArgs);

    procedure SetShowGuidelinesInPreview(const Args: TUiCommandBooleanEventArgs);
    procedure SetShowTransparencyInPreview(const Args: TUiCommandBooleanEventArgs);
    procedure SetShowIconsInListView(const Args: TUiCommandBooleanEventArgs);
    procedure SetListViewVisibility(const Args: TUiCommandBooleanEventArgs);
    procedure SetPreviewDensity(const Args: TUiCommandActionEventArgs);
    procedure SavePreviewImageToDisk(const Args: TUiCommandActionEventArgs);

    procedure ShowFullscreenPreviewWindow(const args : TUiCommandActionEventArgs);
    procedure ShowEditorWindow(const args : TUiCommandActionEventArgs);
    procedure ShowCameraWindow(const args : TUiCommandActionEventArgs);
    procedure ShowPaletteWindow(const args : TUiCommandActionEventArgs);
    procedure ShowOutputPropertiesWindow(const args : TUiCommandActionEventArgs);
    procedure ShowCanvasSizeWindow(const args : TUiCommandActionEventArgs);
    procedure ShowScriptEditorWindow(const args : TUiCommandActionEventArgs);
    procedure ShowScriptFavoritesWindow(const args : TUiCommandActionEventArgs);
    procedure ShowOptionsWindow(const args : TUiCommandActionEventArgs);
    procedure ShowTraceWindow(const args : TUiCommandActionEventArgs);
    procedure ShowAboutWindow(const args : TUiCommandActionEventArgs);

    procedure Close(const args : TUiCommandActionEventArgs);

    ////

    function ReadWithSubst(Attributes: TAttrList; attrname: string): string;
    procedure InvokeLoadXML(xmltext:string);
    procedure LoadXMLFlame(filename, name: string);
    procedure DisableFavorites;
    procedure EnableFavorites;
    procedure ParseXML(var cp1: TControlPoint; const params: string; const ignoreErrors : boolean);
    function SaveFlame(cp1: TControlPoint; title, filename: string): boolean;
    function SaveXMLFlame(const cp1: TControlPoint; title, filename: string): boolean;
    procedure DisplayHint(Sender: TObject);
    procedure OnProgress(prog: double);
    procedure ResizeImage;
    procedure DrawPreview;
    procedure DrawFlame;
    procedure UpdateUndo;
    procedure LoadUndoFlame(index: integer; filename: string);
    procedure ClearCp(var cp: TControlPoint);
    procedure RandomizeCP(var cp1: TControlPoint; alg: integer = 0);
    function SaveGradient(Gradient, Title, FileName: string): boolean;
    function GradientFromPalette(const pal: TColorMap; const title: string): string;
    procedure StopThread;
    procedure UpdateWindows;
    procedure ResetLocation;
    procedure EmptyBatch;
    procedure RandomBatch;
    procedure GetScripts;
    function ApplicationOnHelp(Command: Word; Data: Integer; var CallHelp: Boolean): Boolean;
    function SystemErrorMessage: string;
    function SystemErrorMessage2(errno:cardinal): string;
    function RetrieveXML(cp : TControlPoint):string;
  end;

procedure ListXML(FileName: string; sel: integer);
function EntryExists(En, Fl: string): boolean;
function XMLEntryExists(title, filename: string): boolean;
//procedure ComputeWeights(var cp1: TControlPoint; Triangles: TTriangles; t: integer);
function DeleteEntry(Entry, FileName: string): boolean;
function CleanIdentifier(ident: string): string;
function CleanUPRTitle(ident: string): string;
function GradientString(c: TColorMap): string;
//function PackVariations: int64;
//procedure UnpackVariations(v: int64);
//procedure NormalizeWeights(var cp: TControlPoint);
//procedure EqualizeWeights(var cp: TControlPoint);
procedure MultMatrix(var s: TMatrix; const m: TMatrix);
procedure NormalizeVariations(var cp1: TControlPoint);
function GetWinVersion: TWin32Version;
function LoadXMLFlameText(filename, name: string) : string;

var
  MainForm: TMainForm;
  pname, ptime: String;
  nxform: integer;
  TbBreakWidth: integer;

  EnumPlugins: Boolean;
  MainCp: TControlPoint;
  ParseCp: TControlPoint;
  CurrentFlame: string;
  ThumbnailSize:integer;
  UpdateList:TStringList;
  UpdateError:boolean;
  AboutToExit:boolean;

  ApophysisSVN:string; //APP_VERSION;
  AppVersionString:string; //APP_NAME+'.'+APP_VERSION;

implementation

uses
  Editor, Options, Settings, Template,
  FullScreen, FormRender, Adjust, Browser, Save, About, CmapData,
  {$ifdef DisableScripting}
  {$else}
    ScriptForm, FormFavorites,
  {$endif}
  RndFlame, Tracer, Types, varGenericPlugin;

{$R *.DFM}
{$R 'Ribbon\ApophysisRibbon.res'}

procedure AssignBitmapProperly(var Bitmap:TBitmap; Source:TBitmap);
begin
  Bitmap.Dormant;
  Bitmap.FreeImage;
  Bitmap.Width := 0;
  Bitmap.Assign(Source);
end;

procedure FreeBitmapProperly(var Bitmap:TBitmap);
begin
  try
   Bitmap.Dormant;
   Bitmap.FreeImage;
  finally
   Bitmap.Free;
  end;
end;

procedure NormalizeVariations(var cp1: TControlPoint);
var
  totvar: double;
  i, j: integer;
begin
  for i := 0 to NXFORMS - 1 do
  begin
    totvar := 0;
    for j := 0 to NRVAR - 1 do
    begin
      if cp1.xform[i].GetVariation(j) < 0 then
        cp1.xform[i].SetVariation(j, cp1.xform[i].GetVariation(j) * -1);
      totvar := totvar + cp1.xform[i].GetVariation(j);
    end;
    if totVar = 0 then
    begin
      cp1.xform[i].SetVariation(0, 1)
    end
    else
      for j := 0 to NRVAR - 1 do begin
        if totVar <> 0 then
          cp1.xform[i].SetVariation(j, cp1.xform[i].GetVariation(j) / totvar);
      end;
  end;
end;

function FlameInClipboard: boolean;
var
  flamestr: string;
  isstart, isend: integer;
begin
  { returns true if a flame in clipboard - can be tricked }
  result := false;
  if Clipboard.HasFormat(CF_TEXT) then
  begin
    flamestr := Clipboard.AsText;
    isstart := Pos('<flame', flamestr);
    isend := Pos('</flame>', flamestr);
    if (isstart > 0) and (isend > 0) and (isstart < isend) then Result := true;
  end
end;

procedure MultMatrix(var s: TMatrix; const m: TMatrix);
var
  a, b, c, d, e, f, g, h: double;
begin
  a := s[0, 0];
  b := s[0, 1];
  c := s[1, 0];
  d := s[1, 1];
  e := m[0, 0];
  f := m[0, 1];
  g := m[1, 0];
  h := m[1, 1];
{
    [a, b][e ,f]   [a*e+b*g, a*f+b*h]
    [    ][    ] = [                ]
    [c, d][g, h]   [c*e+d*g, c*f+d*h]
}
  s[0, 0] := a * e + b * g;
  s[0, 1] := a * f + b * h;
  s[1, 0] := c * e + d * g;
  s[1, 1] := c * f + d * h;

end;

(*
function PackVariations: int64;
{ Packs the variation options into an integer with Linear as lowest bit }
var
  i: integer;
begin
  result := 0;
  for i := NRVAR-1 downto 0 do
  begin
    result := (result shl 1) or integer(Variations[i]);
  end;
end;

procedure UnpackVariations(v: int64);
{ Unpacks the variation options form an integer }
var
  i: integer;
begin
  for i := 0 to NRVAR - 1 do
    Variations[i] := boolean(v shr i and 1);
end;
*)

function GetWinVersion: TWin32Version;
{ Returns current version of a host Win32 platform }
begin
  Result := wvUnknown;
  if Win32Platform = VER_PLATFORM_WIN32_WINDOWS then
    if (Win32MajorVersion > 4) or ((Win32MajorVersion = 4) and (Win32MinorVersion > 0)) then
      Result := wvWin98
    else
      Result := wvWin95
  else
    if Win32MajorVersion <= 4 then
      Result := wvWinNT
    else if Win32MajorVersion = 5 then
      if Win32MinorVersion = 0 then
        Result := wvWin2000
      else if Win32MinorVersion >= 1 then
        Result := wvWinXP
    else if Win32MajorVersion = 6 then
      if Win32MinorVersion = 0 then
        Result := wvWinVista
      else if Win32MinorVersion >= 1 then
        Result := wvWin7
    else if Win32MajorVersion >= 7 then
      Result := wvWinFutureFromOuterSpace;
end;

{ ************************************* Help ********************************* }

procedure ShowHelp(Pt: TPoint; ContextId: Integer);
//var
  //Popup: THHPopup;
begin
 (*  -X- context help not longer supported
  FillChar(Popup, SizeOf(Popup), 0);
  Popup.cbStruct := SizeOf(Popup);
  Popup.hinst := 0;
  Popup.idString := ContextId;
  Popup.pszText := nil;
  GetCursorPos(Pt);
  Popup.pt := Pt;
  Popup.clrForeGround := TColorRef(-1);
  Popup.clrBackground := TColorRef(-1);
  Popup.rcMargins := Rect(-1, -1, -1, -1);
  Popup.pszFont := '';
  HtmlHelp(0, PChar(AppPath + 'Apophysis7x.chm::/Popups.txt'), HH_DISPLAY_TEXT_POPUP, DWORD(@Popup));
  *)
end;

procedure TMainForm.InsertStrings;
begin
//  CopyAction.Caption := TextByKey('common-copy');
//	PasteAction.Caption := TextByKey('common-paste');
//	mnuItemDelete.Caption := TextByKey('common-delete');
//	mnuListRename.Caption := TextByKey('common-rename');
//	UndoAction.Caption := TextByKey('common-undo');
//	mnuPopUndo.Caption := TextByKey('common-undo');
//	btnUndo.Hint := TextByKey('common-undo');
//	RedoAction.Caption := TextByKey('common-redo');
//	mnuPopRedo.Caption := TextByKey('common-redo');
//	btnRedo.Hint := TextByKey('common-redo');
//	FileMenuGroup.Caption := TextByKey('main-menu-file-title');
//	NewFlameAction.Caption := TextByKey('main-menu-file-new');
//	ToolButton8.Hint := TextByKey('main-menu-file-new');
//	OpenBatchAction.Caption := TextByKey('main-menu-file-open');
//	btnOpen.Hint := TextByKey('main-menu-file-open');
//	RestoreAutosaveAction.Caption := TextByKey('main-menu-file-restoreautosave');
//	SaveFlameAction.Caption := TextByKey('main-menu-file-saveparams');
//	btnSave.Hint := TextByKey('main-menu-file-saveparams');
//	SaveBatchAction.Caption := TextByKey('main-menu-file-saveallparams');
//	//mnuExit.Caption := TextByKey('main-menu-file-exit');
//	EditMenuGroup.Caption := TextByKey('main-menu-edit-title');
//	ViewMenuGroup.Caption := TextByKey('main-menu-view-title');
//	FullScreenPreviewAction.Caption := TextByKey('main-menu-view-fullscreen');
//	mnuPopFullscreen.Caption := TextByKey('main-menu-view-fullscreen');
//	btnFullScreen.Hint := TextByKey('main-menu-view-fullscreen');
//	ShowEditorAction.Caption := TextByKey('main-menu-view-editor');
//	ToolButton5.Hint := TextByKey('main-menu-view-editor');
//	ShowAdjustmentAction.Caption := TextByKey('main-menu-view-adjustment');
//	ToolButton6.Hint := TextByKey('main-menu-view-adjustment');
//	//mnuMessages.Caption := TextByKey('main-menu-view-messages');
//	toolButton13.Hint := TextByKey('main-menu-view-messages');
//	//mnuResetLocation.Caption := TextByKey('main-menu-flame-reset');
//	mnuPopResetLocation.Caption := TextByKey('main-menu-flame-reset');
//	btnReset.Hint := TextByKey('main-menu-flame-reset');
//	RenderFlameAction.Caption := TextByKey('main-menu-flame-rendertodisk');
//	btnRender.Hint := TextByKey('main-menu-flame-rendertodisk');
//	RenderBatchAction.Caption := TextByKey('main-menu-flame-renderallflames');
//	tbRenderAll.Hint := TextByKey('main-menu-flame-renderallflames');
//	//mnuReportFlame.Caption := TextByKey('main-menu-flame-generatereport');
//	ScriptMenuGroup.Caption := TextByKey('main-menu-script-title');
//	RunScriptAction.Caption := TextByKey('main-menu-script-run');
//	btnRunScript.Hint := TextByKey('main-menu-script-run');
//	StopScriptAction.Caption := TextByKey('main-menu-script-stop');
//	btnStopScript.Hint := TextByKey('main-menu-script-stop');
//	OpenScriptAction.Caption := TextByKey('main-menu-script-open');
//	EditScriptAction.Caption := TextByKey('main-menu-script-edit');
//	ToolButton17.Hint := TextByKey('main-menu-script-edit');
//	ManageScriptFavoritesAction.Caption := TextByKey('main-menu-script-managefaves');
//	//mnuTurnFlameToScript.Caption := TextByKey('main-menu-script-flametoscript');
//	ToolsMenuGroup.Caption := TextByKey('main-menu-options-title');
//	//mnuToolbar.Caption := TextByKey('main-menu-options-togglemaintoolbar');
//	//mnuStatusBar.Caption := TextByKey('main-menu-options-togglestatusbar');
//	//mnuFileContents.Caption := TextByKey('main-menu-options-togglefilelist');
//	//mnuResetUI.Caption := TextByKey('main-menu-options-resetfilelistwidth');
//	ShowSettingsAction.Caption := TextByKey('main-menu-options-showoptions');
//	ToolButton14.Hint := TextByKey('main-menu-options-showoptions');
//	//MainHelp.Caption := TextByKey('main-menu-help-title');
//	//mnuHelpTopics.Caption := TextByKey('main-menu-help-contents');
//	//mnuFlamePDF.Caption := TextByKey('main-menu-help-aboutalgorithm');
//	//mnuAbout.Caption := TextByKey('main-menu-help-aboutapophysis');
//	btnViewList.Hint := TextByKey('main-toolbar-listviewmode-classic');
//	btnViewIcons.Hint := TextByKey('main-toolbar-listviewmode-icons');
//	tbShowAlpha.Hint := TextByKey('main-toolbar-togglealpha');
//	tbGuides.Hint := TextByKey('main-toolbar-toggleguides');
//	tbDraw.Hint := TextByKey('main-toolbar-modemove');
//	ToolButton20.Hint := TextByKey('main-toolbar-moderotate');
//	ToolButton21.Hint := TextByKey('main-toolbar-modezoomin');
//	ToolButton22.Hint := TextByKey('main-toolbar-modezoomout');
//  ListView1.Columns[0].Caption := TextByKey('save-name');
//  //mnuResumeRender.Caption := TextByKey('main-menu-flame-resumeunfinished');
end;

procedure TMainForm.InvokeLoadXML(xmltext:string);
begin
  ParseXML(MainCP, PCHAR(xmltext), false);
end;

function TMainForm.ApplicationOnHelp(Command: Word; Data: Integer; var CallHelp: Boolean): Boolean;
var
  Pos: TPoint;
begin
  Pos.x := 0;
  Pos.y := 0;

  CallHelp := False;
  Result := True;
  case Command of
    HELP_SETPOPUP_POS: Pos := SmallPointToPoint(TSmallPoint(Data));
    HELP_CONTEXTPOPUP: ShowHelp(Pos, Data);
  else Result := False;
  end;
end;

{ **************************************************************************** }

procedure TMainForm.StopThread;
begin
  PreviewRedrawDelayTimer.Enabled := False;
  if Assigned(Renderer) then begin
    assert(Renderer.Suspended = false);
    PreviewThreadChangingContext := true;
    Renderer.BreakRender;
    Renderer.WaitFor;
  end;
end;

procedure EqualizeVars(const x: integer);
var
  i: integer;
begin
  for i := 0 to Transforms - 1 do
    MainCp.xform[x].SetVariation(i, 1.0 / NRVAR);
end;

procedure NormalVars(const x: integer);
var
  i: integer;
  td: double;
begin
  td := 0.0;
  for i := 0 to 6 do
    td := td + Maincp.xform[x].GetVariation(i);
  if (td < 0.001) then
    EqualizeVars(x)
  else
    for i := 0 to 6 do
      MainCp.xform[x].SetVariation(i, MainCp.xform[x].GetVariation(i) / td);
end;

procedure RandomVariation(cp: TControlPoint);
{ Randomise variation parameters }
var
  a, b, i, j: integer;
begin
  inc(MainSeed);
  RandSeed := MainSeed;
  for i := 0 to cp.NumXForms - 1 do
  begin
    for j := 0 to NRVAR - 1 do
      cp.xform[i].SetVariation(j, 0);
    repeat
      a := random(NRVAR);
    until Variations[a];
    repeat
      b := random(NRVAR);
    until Variations[b];
    if (a = b) then
    begin
      cp.xform[i].SetVariation(a, 1);
    end
    else
    begin
      cp.xform[i].SetVariation(a, random);
      cp.xform[i].SetVariation(b, 1 - cp.xform[i].GetVariation(a));
    end;
  end;
end;

procedure TMainForm.ClearCp(var cp: TControlPoint);
var
  i: integer;
begin
  if assigned(MainCP) then
    cp := MainCP.Clone
  else
    cp := TControlPoint.Create;

  for i := 0 to Transforms do cp.xform[i].Clear;
  cp.xform[0].SetVariation(0, 1);
  cp.xform[0].density := 0.5;
  cp.xform[1].symmetry := 1;

  cp.center[0] := 0;
  cp.center[1] := 0;
  cp.zoom := 0;
  cp.pixels_per_unit := cp.Width/4;
  cp.FAngle := 0;
end;

procedure TMainForm.RandomizeCP(var cp1: TControlPoint; alg: integer = 0);
(*
var
  vrnd, Min, Max, i, j, rnd: integer;
  Triangles: TTriangles;
  cmap: TColorMap;
  r, s, theta, phi: double;
  skip: boolean;
*)
var
  sourceCP: TControlPoint;
begin
  if assigned(MainCP) then
    sourceCP := MainCP.Clone
  else
    SourceCP := nil;

  if assigned(cp1) then begin
    cp1.Free;
    cp1 := nil;
  end;
  cp1 := RandomFlame(sourceCP, alg);

  if assigned(sourceCP) then
    sourceCP.Free;

(*
  Min := randMinTransforms;
  Max := randMaxTransforms;
  case randGradient of
    0:
      begin
        cp1.CmapIndex := Random(NRCMAPS);
        GetCMap(cmap_index, 1, cp1.cmap);
        cmap_index := cp1.cmapindex;
      end;
    1: cmap := DefaultPalette;
    2: cmap := MainCp.cmap;
    3: cmap := GradientForm.RandomGradient;
  end;
  inc(MainSeed);
  RandSeed := MainSeed;
  transforms := random(Max - (Min - 1)) + Min;
  repeat
    try
      inc(MainSeed);
      RandSeed := MainSeed;
      cp1.clear;
      cp1.RandomCP(transforms, transforms, false);
      cp1.SetVariation(Variation);
      inc(MainSeed);
      RandSeed := MainSeed;

      case alg of
        1: rnd := 0;
        2: rnd := 7;
        3: rnd := 9;
      else
        if (Variation = vLinear) or (Variation = vRandom) then
          rnd := random(10)
        else
          rnd := 9;
      end;
      case rnd of
        0..6:
          begin
            for i := 0 to Transforms - 1 do
            begin
              if Random(10) < 9 then
                cp1.xform[i].c[0, 0] := 1
              else
                cp1.xform[i].c[0, 0] := -1;
              cp1.xform[i].c[0, 1] := 0;
              cp1.xform[i].c[1, 0] := 0;
              cp1.xform[i].c[1, 1] := 1;
              cp1.xform[i].c[2, 0] := 0;
              cp1.xform[i].c[2, 1] := 0;
              cp1.xform[i].color := 0;
              cp1.xform[i].symmetry := 0;
              cp1.xform[i].vars[0] := 1;
              for j := 1 to NVARS - 1 do
                cp1.xform[i].vars[j] := 0;
              Translate(cp1.xform[i], random * 2 - 1, random * 2 - 1);
              Rotate(cp1.xform[i], random * 360);
              if i > 0 then Scale(cp1.xform[i], random * 0.8 + 0.2)
              else Scale(cp1.xform[i], random * 0.4 + 0.6);
              if Random(2) = 0 then
                Multiply(cp1.xform[i], 1, random - 0.5, random - 0.5, 1);
            end;
            SetVariation(cp1);
          end;
        7, 8:
          begin
          { From the source to Chaos: The Software }
            for i := 0 to Transforms - 1 do
            begin
              r := random * 2 - 1;
              if ((0 <= r) and (r < 0.2)) then
                r := r + 0.2;
              if ((r > -0.2) and (r <= 0)) then
                r := r - 0.2;
              s := random * 2 - 1;
              if ((0 <= s) and (s < 0.2)) then
                s := s + 0.2;
              if ((s > -0.2) and (s <= 0)) then
                s := s - -0.2;
              theta := PI * random;
              phi := (2 + random) * PI / 4;
              cp1.xform[i].c[0][0] := r * cos(theta);
              cp1.xform[i].c[1][0] := s * (cos(theta) * cos(phi) - sin(theta));
              cp1.xform[i].c[0][1] := r * sin(theta);
              cp1.xform[i].c[1][1] := s * (sin(theta) * cos(phi) + cos(theta));
            { the next bit didn't translate so well, so I fudge it}
              cp1.xform[i].c[2][0] := random * 2 - 1;
              cp1.xform[i].c[2][1] := random * 2 - 1;
            end;
            for i := 0 to NXFORMS - 1 do
              cp1.xform[i].density := 0;
            for i := 0 to Transforms - 1 do
              cp1.xform[i].density := 1 / Transforms;
            SetVariation(cp1);
          end;
        9: begin
            for i := 0 to NXFORMS - 1 do
              cp1.xform[i].density := 0;
            for i := 0 to Transforms - 1 do
              cp1.xform[i].density := 1 / Transforms;
          end;
      end; // case
      MainForm.TrianglesFromCp(cp1, Triangles);
      vrnd := Random(2);
      if vrnd > 0 then
        ComputeWeights(cp1, Triangles, transforms)
      else
        EqualizeWeights(cp1);
    except on E: EmathError do
      begin
        Continue;
      end;
    end;
    for i := 0 to Transforms - 1 do
      cp1.xform[i].color := i / (transforms - 1);
    if cp1.xform[0].density = 1 then Continue;
    case SymmetryType of
      { Bilateral }
      1: add_symmetry_to_control_point(cp1, -1);
      { Rotational }
      2: add_symmetry_to_control_point(cp1, SymmetryOrder);
      { Rotational and Reflective }
      3: add_symmetry_to_control_point(cp1, -SymmetryOrder);
    end;
    { elimate flames with transforms that aren't affine }
    skip := false;
    for i := 0 to Transforms - 1 do
      if not transform_affine(Triangles[i], Triangles) then
        skip := True;
    if skip then continue;
  until not cp1.BlowsUP(5000) and (cp1.xform[0].density <> 0);
  cp1.brightness := defBrightness;
  cp1.gamma := defGamma;
  cp1.vibrancy := defVibrancy;
  cp1.sample_density := defSampleDensity;
  cp1.spatial_oversample := defOversample;
  cp1.spatial_filter_radius := defFilterRadius;
  cp1.cmapIndex := MainCp.cmapindex;
  if not KeepBackground then begin
    cp1.background[0] := 0;
    cp1.background[1] := 0;
    cp1.background[2] := 0;
  end;
  if randGradient = 0 then
  else cp1.cmap := cmap;
  cp1.zoom := 0;
  cp1.Nick := SheepNick;
  cp1.URl := SheepURL;
*)
end;

function TMainForm.GradientFromPalette(const pal: TColorMap; const title: string): string;
var
  c, i, j: integer;
  strings: TStringList;
begin
  strings := TStringList.Create;
  try
    strings.add('gradient:');
    strings.add(' title="' + CleanUPRTitle(title) + '" smooth=no');
    for i := 0 to 255 do
    begin
      j := round(i * (399 / 255));
      c := pal[i][2] shl 16 + pal[i][1] shl 8 + pal[i][0];
      strings.Add(' index=' + IntToStr(j) + ' color=' + intToStr(c));
    end;
    result := strings.text;
  finally
    strings.free;
  end;
end;

function CleanIdentifier(ident: string): string;
{ Strips unwanted characters from an identifier}
var
  i: integer;
begin
  for i := 0 to Length(ident) do
  begin
    if ident[i] = #32 then
      ident[i] := '_'
    else if ident[i] = '}' then
      ident[i] := '_'
    else if ident[i] = '{' then
      ident[i] := '_';
  end;
  Result := ident;
end;

procedure TMainForm.OnProgress(prog: double);
var
  Elapsed, Remaining: TDateTime;
  IntProg: Integer;
begin
  IntProg := (round(prog * 100));
  //pnlLSPFrame.Visible := true;
  StatusProgressBar.Position := IntProg;
  if (IntProg = 100) then StatusProgressBar.Position := 0;
  Elapsed := Now - StartTime;
  StatusBar.Panels[1].Text := Format(TextByKey('render-status-elapsed') + ' %2.2d:%2.2d:%2.2d.%2.2d',
    [Trunc(Elapsed * 24),
    Trunc((Elapsed * 24 - Trunc(Elapsed * 24)) * 60),
      Trunc((Elapsed * 24 * 60 - Trunc(Elapsed * 24 * 60)) * 60),
      Trunc((Elapsed * 24 * 60 * 60 - Trunc(Elapsed * 24 * 60 * 60)) * 100)]);
  if prog > 0 then
    Remaining := Elapsed/prog - Elapsed
  else
    Remaining := 0;

  StatusBar.Panels[2].Text := Format(TextByKey('render-status-remaining') + ' %2.2d:%2.2d:%2.2d.%2.2d',
    [Trunc(Remaining * 24),
    Trunc((Remaining * 24 - Trunc(Remaining * 24)) * 60),
      Trunc((Remaining * 24 * 60 - Trunc(Remaining * 24 * 60)) * 60),
      Trunc((Remaining * 24 * 60 * 60 - Trunc(Remaining * 24 * 60 * 60)) * 100)]);
  StatusBar.Panels[3].Text := MainCp.name;
  Application.ProcessMessages;
end;

procedure TMainForm.UpdateUndo;
begin
  MainCp.FillUsedPlugins;
  SaveFlame(MainCp, Format('%.4d-', [UndoIndex]) + MainCp.name,
    GetEnvVarValue('APPDATA') + '\' + undoFilename);
  Inc(UndoIndex);
  UndoMax := UndoIndex; //Inc(UndoMax);
  SetCanUndo(true);
  PreviewPanelMenuUndoItem.Enabled := True;
  SetCanRedo(false);
  PreviewPanelMenuRedoItem.Enabled := false;
  EditForm.mnuUndo.Enabled := True;
  EditForm.mnuRedo.Enabled := false;
  EditForm.tbUndo.enabled := true;
  EditForm.tbRedo.enabled := false;
  AdjustForm.btnUndo.enabled := true;
  AdjustForm.btnRedo.enabled := false;
end;

function GradientEntries(gFilename: string): string;
var
  i, p: integer;
  Title: string;
  FileStrings: TStringList;
  NewStrings: TStringList;
begin
  FileStrings := TStringList.Create;
  NewStrings := TStringList.Create;
  NewStrings.Text := '';
  FileStrings.LoadFromFile(gFilename);
  try
    if (Pos('{', FileStrings.Text) <> 0) then
    begin
      for i := 0 to FileStrings.Count - 1 do
      begin
        p := Pos('{', FileStrings[i]);
        if (p <> 0) then
        begin
          Title := Trim(Copy(FileStrings[i], 1, p - 1));
          if (Title <> '') and (LowerCase(Title) <> 'comment') then
          begin { Otherwise bad format }
            NewStrings.Add(Title);
          end;
        end;
      end;
      GradientEntries := NewStrings.Text;
    end;
  finally
    FileStrings.Free;
    NewStrings.Free;
  end;
end;

{ ********************************* File ************************************* }

function EntryExists(En, Fl: string): boolean;
{ Searches for existing identifier in parameter files }
var
  FStrings: TStringList;
  i: integer;
begin
  Result := False;
  if FileExists(Fl) then
  begin
    FStrings := TStringList.Create;
    try
      FStrings.LoadFromFile(Fl);
      for i := 0 to FStrings.Count - 1 do
        if Pos(LowerCase(En) + ' {', Lowercase(FStrings[i])) <> 0 then
          Result := True;
    finally
      FStrings.Free;
    end
  end
  else
    Result := False;
end;

function CleanEntry(ident: string): string;
{ Strips unwanted characters from an identifier}
var
  i: integer;
begin
  for i := 1 to Length(ident) do
  begin
    if ident[i] = #32 then
      ident[i] := '_'
    else if ident[i] = '}' then
      ident[i] := '_'
    else if ident[i] = '{' then
      ident[i] := '_';
  end;
  Result := ident;
end;

function CleanXMLName(ident: string): string;
var
  i: integer;
begin
  for i := 1 to Length(ident) do
  begin
    if ident[i] = '*' then
      ident[i] := '_'
    else if ident[i] = '"' then
      ident[i] := #39;
  end;
  Result := ident;
end;


function CleanUPRTitle(ident: string): string;
{ Strips braces but leave spaces }
var
  i: integer;
begin
  for i := 1 to Length(ident) do
  begin
    if ident[i] = '}' then
      ident[i] := '_'
    else if ident[i] = '{' then
      ident[i] := '_';
  end;
  Result := ident;
end;

function DeleteEntry(Entry, FileName: string): boolean;
{ Deletes an entry from a multi-entry file }
var
  Strings: TStringList;
  p, i: integer;
begin
  Result := True;
  Strings := TStringList.Create;
  try
    i := 0;
    Strings.LoadFromFile(FileName);
    while Pos(Entry + ' ', Trim(Strings[i])) <> 1 do
    begin
      inc(i);
    end;
    repeat
      p := Pos('}', Strings[i]);
      Strings.Delete(i);
    until p <> 0;
    if (i < Strings.Count) and (Trim(Strings[i]) = '') then Strings.Delete(i);
    Strings.SaveToFile(FileName);
  finally
    Strings.Free;
  end;
end;

function IFSToString(cp: TControlPoint; Title: string): string;
{ Creates a string containing a formated IFS parameter set }
var
  i: integer;
  a, b, c, d, e, f, p: double;
  Strings: TStringList;
begin
  Strings := TStringList.Create;
  try
    Strings.Add(CleanEntry(Title) + ' {');
    for i := 0 to Transforms - 1 do
    begin
      a := cp.xform[i].c[0][0];
      b := cp.xform[i].c[0][1];
      c := cp.xform[i].c[1][0];
      d := cp.xform[i].c[1][1];
      e := cp.xform[i].c[2][0];
      f := cp.xform[i].c[2][1];
      p := cp.xform[i].density;
      Strings.Add(Format('%.6g %.6g %.6g %.6g %.6g %.6g %.6g',
        [a, b, c, d, e, f, p]));
    end;
    Strings.Add('}');
    IFSToString := Strings.Text;
  finally
    Strings.Free;
  end;
end;

function GetTitle(str: string): string;
var
  p: integer;
begin
  str := Trim(str);
  p := Pos(' ', str);
  GetTitle := Trim(Copy(str, 1, p));
end;

function GetComment(str: string): string;
{ Extracts comment form line of IFS file }
var
  p: integer;
begin
  str := Trim(str);
  p := Pos(';', str);
  if p <> 0 then
    GetComment := Trim(Copy(str, p + 1, Length(str) - p))
  else
    GetComment := '';
end;

function GetParameters(str: string; var a, b, c, d, e, f, p: double): boolean;
var
  Tokens: TStringList;
begin
  GetParameters := False;
  Tokens := TStringList.Create;
  try
    try
      GetTokens(str, tokens);
      if Tokens.Count >= 7 then {enough tokens}
      begin
        a := StrToFloat(Tokens[0]);
        b := StrToFloat(Tokens[1]);
        c := StrToFloat(Tokens[2]);
        d := StrToFloat(Tokens[3]);
        e := StrToFloat(Tokens[4]);
        f := StrToFloat(Tokens[5]);
        p := StrToFloat(Tokens[6]);
        Result := True;
      end;
    except on E: EConvertError do
      begin
        Result := False
      end;
    end;
  finally
    Tokens.Free;
  end;
end;

function StringToIFS(strng: string): boolean;
{ Loads an IFS parameter set from string}
var
  Strings: TStringList;
  Comments: TStringList;
  i, sTransforms: integer;
  cmnt, sTitle: string;
  a, b, c, d: double;
  e, f, p: double;
begin
  MainCp.clear;
  StringToIFS := True;
  sTransforms := 0;
  Strings := TStringList.Create;
  Comments := TStringList.Create;
  try
    try
      Strings.Text := strng;
      if Pos('}', Strings.Text) = 0 then
        raise EFormatInvalid.Create('No closing brace');
      if Pos('{', Strings[0]) = 0 then
        raise EFormatInvalid.Create('No opening brace.');
      {To Do ... !!!!}
      sTitle := GetTitle(Strings[0]);
      if sTitle = '' then raise EFormatInvalid.Create('No identifier.');
      cmnt := GetComment(Strings[0]);
      if cmnt <> '' then Comments.Add(cmnt);
      i := 1;
      try
        repeat
          cmnt := GetComment(Strings[i]);
          if cmnt <> '' then Comments.Add(cmnt);
          if (Pos(';', Trim(Strings[i])) <> 1) and (Trim(Strings[i]) <> '') then
            if GetParameters(Strings[i], a, b, c, d, e, f, p) then
            begin
              MainCp.xform[sTransforms].c[0][0] := a;
              MainCp.xform[sTransforms].c[0][1] := c;
              MainCp.xform[sTransforms].c[1][0] := b;
              MainCp.xform[sTransforms].c[1][1] := d;
              MainCp.xform[sTransforms].c[2][0] := e;
              MainCp.xform[sTransforms].c[2][1] := f;
              MainCp.xform[sTransforms].density := p;
              inc(sTransforms);
            end
            else
              EFormatInvalid.Create('Insufficient parameters.');
          inc(i);
        until (Pos('}', Strings[i]) <> 0) or (sTransforms = NXFORMS);
      except on E: EMathError do
      end;
      if sTransforms < 2 then
        raise EFormatInvalid.Create('Insufficient parameters.');
      MainCp.name := sTitle;
      Transforms := sTransforms;
      for i := 1 to Transforms - 1 do
        MainCp.xform[i].color := 0;
      MainCp.xform[0].color := 1;

    except on E: EFormatInvalid do
      begin
        Application.MessageBox(PChar(TextByKey('common-invalidformat')), PChar('Apophysis'), 16);
      end;
    end;
  finally
    Strings.Free;
    Comments.Free;
  end;
end;


function SaveIFS(cp: TControlPoint; Title, FileName: string): boolean;
{ Saves IFS parameters to end of file }
var
  a, b, c: double;
  d, e, f, p: double;
  m: integer;
  IFile: TextFile;
begin
  Result := True;
  try
    AssignFile(IFile, FileName);
    if FileExists(FileName) then
    begin
      if EntryExists(Title, FileName) then DeleteEntry(Title, FileName);
      Append(IFile);
    end
    else
      ReWrite(IFile);
    WriteLn(IFile, Title + ' {');
    for m := 0 to Transforms - 1 do
    begin
      a := cp.xform[m].c[0][0];
      c := cp.xform[m].c[0][1];
      b := cp.xform[m].c[1][0];
      d := cp.xform[m].c[1][1];
      e := cp.xform[m].c[2][0];
      f := cp.xform[m].c[2][1];
      p := cp.xform[m].density;
      Write(IFile, Format('%.6g %.6g %.6g %.6g %.6g %.6g %.6g',
        [a, b, c, d, e, f, p]));
      WriteLn(IFile, '');
    end;
    WriteLn(IFile, '}');
    WriteLn(IFile, ' ');
    CloseFile(IFile);
  except on E: EInOutError do
    begin
      Application.MessageBox(PChar(Format(TextByKey('common-genericsavefailure'), [FileName])), 'Apophysis', 16);
      Result := False;
    end;
  end;
end;

function TMainForm.SaveFlame(cp1: TControlPoint; title, filename: string): boolean;
{ Saves Flame parameters to end of file }
var
  IFile: TextFile;
  sl: TStringList;
  i: integer;
begin
  Result := True;
  try
    AssignFile(IFile, filename);
    if FileExists(filename) then
    begin
      if EntryExists(title, filename) then DeleteEntry(title, fileName);
      Append(IFile);
    end
    else ReWrite(IFile);

    sl := TStringList.Create;
    try
      cp1.SaveToStringList(sl);
      WriteLn(IFile, title + ' {');
      write(IFile, sl.Text);
      WriteLn(IFile, 'palette:');
      for i := 0 to 255 do
      begin
        WriteLn(IFile, IntToStr(cp1.cmap[i][0]) + ' ' +
                       IntToStr(cp1.cmap[i][1]) + ' ' +
                       IntToStr(cp1.cmap[i][2]))
      end;
      WriteLn(IFile, ' }');
    finally
      sl.free
    end;
    WriteLn(IFile, ' ');
    CloseFile(IFile);

  except on EInOutError do
    begin
      Application.MessageBox(PChar(Format(TextByKey('common-genericsavefailure'), [FileName])), 'Apophysis', 16);
      Result := False;
    end;
  end;
end;

function ColorToXmlCompact(cp1: TControlPoint): string;
var
  i: integer;
begin
  Result := '   <palette count="256" format="RGB">';
  for i := 0 to 255 do  begin
    if ((i and 7) = 0) then Result := Result + #13#10 + '      ';
    Result := Result + IntToHex(cp1.cmap[i, 0],2)
                     + IntToHex(cp1.cmap[i, 1],2)
                     + IntToHex(cp1.cmap[i, 2],2);
  end;
  Result := Result + #13#10 + '   </palette>';
end;


function ColorToXml(cp1: TControlPoint): string;
var
  i: integer;
begin
  Result := '';
  for i := 0 to 255 do  begin
    Result := Result + '   <color index="' + IntToStr(i) +
      '" rgb="' + IntToStr(cp1.cmap[i, 0]) + ' ' +
                  IntToStr(cp1.cmap[i, 1]) + ' ' +
                  IntToStr(cp1.cmap[i, 2]) + '"/>' + #13#10;
  end;
end;


function FlameToXMLAS(const cp1: TControlPoint; title: string; exporting: boolean): string;
var
  t, i{, j}: integer;
  FileList: TStringList;
  x, y: double;
  parameters: string;
  str: string;
begin
  FileList := TStringList.create;
  x := cp1.center[0];
  y := cp1.center[1];

//  if cp1.cmapindex >= 0 then pal := pal + 'gradient="' + IntToStr(cp1.cmapindex) + '" ';

  try
    parameters := 'version="' + AppVersionString + '" ';
    if cp1.time <> 0 then
      parameters := parameters + format('time="%g" ', [cp1.time]);

    parameters := parameters +
      'size="' + IntToStr(cp1.width) + ' ' + IntToStr(cp1.height) +
      format('" center="%g %g" ', [x, y]) +
      format('scale="%g" ', [cp1.pixels_per_unit]);

    if cp1.FAngle <> 0 then
      parameters := parameters + format('angle="%g" ', [cp1.FAngle]) +
                                 format('rotate="%g" ', [-180 * cp1.FAngle/Pi]);
    if cp1.zoom <> 0 then
      parameters := parameters + format('zoom="%g" ', [cp1.zoom]);

// 3d
    if cp1.cameraPitch <> 0 then
      parameters := parameters + format('cam_pitch="%g" ', [cp1.cameraPitch]);
    if cp1.cameraYaw <> 0 then
      parameters := parameters + format('cam_yaw="%g" ', [cp1.cameraYaw]);
    if cp1.cameraPersp <> 0 then
      parameters := parameters + format('cam_perspective="%g" ', [cp1.cameraPersp]);
    if cp1.cameraZpos <> 0 then
      parameters := parameters + format('cam_zpos="%g" ', [cp1.cameraZpos]);
    if cp1.cameraDOF <> 0 then
      parameters := parameters + format('cam_dof="%g" ', [cp1.cameraDOF]);
//
    parameters := parameters + format(
             'oversample="%d" filter="%g" quality="%g" ',
             [cp1.spatial_oversample,
              cp1.spatial_filter_radius,
              cp1.sample_density]
             );
    if cp1.nbatches <> 1 then parameters := parameters + 'batches="' + IntToStr(cp1.nbatches) + '" ';

    parameters := parameters +
      format('background="%g %g %g" ', [cp1.background[0] / 255, cp1.background[1] / 255, cp1.background[2] / 255]) +
      format('brightness="%g" ', [cp1.brightness]) +
      format('gamma="%g" ', [cp1.gamma]);

    if cp1.vibrancy <> 1 then
      parameters := parameters + format('vibrancy="%g" ', [cp1.vibrancy]);

    if cp1.gamma_threshold <> 0 then
      parameters := parameters + format('gamma_threshold="%g" ', [cp1.gamma_threshold]);

    if cp1.soloXform >= 0 then
      parameters := parameters + format('soloxform="%d" ', [cp1.soloXform]);

    parameters := parameters +
      format('estimator_radius="%g" ', [cp1.estimator]) +
      format('estimator_minimum="%g" ', [cp1.estimator_min]) +
      format('estimator_curve="%g" ', [cp1.estimator_curve]);
    if (cp1.enable_de) then
      parameters := parameters + ('enable_de="1" ')
    else parameters := parameters + ('enable_de="0" ');

    str := '';
    for i := 0 to cp1.used_plugins.Count-1 do begin
      str := str + cp1.used_plugins[i];
      if (i = cp1.used_plugins.Count-1) then break;
      str := str + ' ';
    end;
    parameters := parameters + format('plugins="%s" new_linear="1" ', [str]);

    FileList.Add('<flame name="' + title + '" ' + parameters + '>');
   { Write transform parameters }
    t := cp1.NumXForms;
    for i := 0 to t - 1 do
      FileList.Add(cp1.xform[i].ToXMLString);
    if cp1.HasFinalXForm then
    begin
      // 'enabled' flag disabled in this release
      FileList.Add(cp1.xform[t].FinalToXMLString(cp1.finalXformEnabled));
    end;

    { Write palette data }
    if exporting then
      FileList.Add(ColorToXml(cp1))
    else
      FileList.Add(ColorToXmlCompact(cp1));

    FileList.Add('</flame>');
    result := FileList.text;
  finally
    FileList.free
  end;
end;

function FlameToXML(const cp1: TControlPoint; exporting, embedthumb: boolean): String;
var
  t, i{, j}, pos: integer;
  FileList: TStringList;
  x, y: double;
  parameters: string;
  str, buf, xdata: string;
begin
  FileList := TStringList.create;
  x := cp1.center[0];
  y := cp1.center[1];

  try
    parameters := 'version="' + AppVersionString + '" ';
    if cp1.time <> 0 then
      parameters := parameters + format('time="%g" ', [cp1.time]);

    parameters := parameters +
      'size="' + IntToStr(cp1.width) + ' ' + IntToStr(cp1.height) +
      format('" center="%g %g" ', [x, y]) +
      format('scale="%g" ', [cp1.pixels_per_unit]);

    if cp1.FAngle <> 0 then
      parameters := parameters + format('angle="%g" ', [cp1.FAngle]) +  // !?!?!?
                                 format('rotate="%g" ', [-180 * cp1.FAngle/Pi]);
    if cp1.zoom <> 0 then
      parameters := parameters + format('zoom="%g" ', [cp1.zoom]);

// 3d
    if cp1.cameraPitch <> 0 then
      parameters := parameters + format('cam_pitch="%g" ', [cp1.cameraPitch]);
    if cp1.cameraYaw <> 0 then
      parameters := parameters + format('cam_yaw="%g" ', [cp1.cameraYaw]);
    if cp1.cameraPersp <> 0 then
      parameters := parameters + format('cam_perspective="%g" ', [cp1.cameraPersp]);
    if cp1.cameraZpos <> 0 then
      parameters := parameters + format('cam_zpos="%g" ', [cp1.cameraZpos]);
    if cp1.cameraDOF <> 0 then
      parameters := parameters + format('cam_dof="%g" ', [cp1.cameraDOF]);
//
    parameters := parameters + format(
             'oversample="%d" filter="%g" quality="%g" ',
             [cp1.spatial_oversample,
              cp1.spatial_filter_radius,
              cp1.sample_density]
             );
    if cp1.nbatches <> 1 then parameters := parameters + 'batches="' + IntToStr(cp1.nbatches) + '" ';

    parameters := parameters +
      format('background="%g %g %g" ', [cp1.background[0] / 255, cp1.background[1] / 255, cp1.background[2] / 255]) +
      format('brightness="%g" ', [cp1.brightness]) +
      format('gamma="%g" ', [cp1.gamma]);

    if cp1.vibrancy <> 1 then
      parameters := parameters + format('vibrancy="%g" ', [cp1.vibrancy]);

    if cp1.gamma_threshold <> 0 then
      parameters := parameters + format('gamma_threshold="%g" ', [cp1.gamma_threshold]);

    if cp1.soloXform >= 0 then
      parameters := parameters + format('soloxform="%d" ', [cp1.soloXform]);

    //
    parameters := parameters +
      format('estimator_radius="%g" ', [cp1.estimator]) +
      format('estimator_minimum="%g" ', [cp1.estimator_min]) +
      format('estimator_curve="%g" ', [cp1.estimator_curve]);
    if exporting then parameters := parameters +
      format('temporal_samples="%d" ', [cp1.jitters]);
    if (cp1.enable_de) then
      parameters := parameters + ('enable_de="1" ')
    else parameters := parameters + ('enable_de="0" ');

    str := '';
    for i := 0 to cp1.used_plugins.Count-1 do begin
      str := str + cp1.used_plugins[i];
      if (i = cp1.used_plugins.Count-1) then break;
      str := str + ' ';
    end;
    parameters := parameters + format('plugins="%s" new_linear="1" ', [str]);

    FileList.Add('<flame name="' + CleanXMLName(cp1.name) + '" ' + parameters + '>');
   { Write transform parameters }
    t := cp1.NumXForms;
    for i := 0 to t - 1 do
      FileList.Add(cp1.xform[i].ToXMLString);
    if cp1.HasFinalXForm then
    begin
      // 'enabled' flag disabled in this release
      FileList.Add(cp1.xform[t].FinalToXMLString(cp1.finalXformEnabled));
    end;

    { Write palette data }
    if exporting then
      FileList.Add(ColorToXml(cp1))
    else
      FileList.Add(ColorToXmlCompact(cp1));

    FileList.Add('</flame>');
    result := FileList.text;
  finally
    FileList.free
  end;
end;

function RemoveExt(filename: string): string;
var
  ext: string;
  p: integer;
begin
  filename := ExtractFileName(filename);
  ext := ExtractFileExt(filename);
  p := Pos(ext, filename);
  Result := Copy(filename, 0, p - 1);
end;

function XMLEntryExists(title, filename: string): boolean;
var
  FileList: TStringList;
begin

  Result := false;
  if FileExists(filename) then
  begin
    FileList := TStringList.Create;
    try
      FileList.LoadFromFile(filename);
      if pos('<flame name="' + title + '"', FileList.Text) <> 0 then Result := true;
    finally
      FileList.Free;
    end
  end else
    result := false;
end;

procedure DeleteXMLEntry(title, filename: string);
var
  Strings: TStringList;
  p, i: integer;
begin
  Strings := TStringList.Create;
  try
    i := 0;
    Strings.LoadFromFile(FileName);
    while Pos('name="' + title + '"', Trim(Strings[i])) = 0 do
      inc(i);

    p := 0;
    while p = 0 do
    begin
      p := Pos('</flame>', Strings[i]);
      Strings.Delete(i);
    end;
    Strings.SaveToFile(FileName);
  finally
    Strings.Free;
  end;
end;


function TMainForm.SaveXMLFlame(const cp1: TControlPoint; title, filename: string): boolean;
{ Saves Flame parameters to end of file }
var
  Tag: string;
  IFile: File;
  FileList: TStringList;
  RB: RawByteString;

  i, p: integer;
  bakname: string;
begin
  Tag := RemoveExt(filename);
  Result := True;
  try
    if FileExists(filename) then
    begin
      bakname := ChangeFileExt(filename, '.bak');
      if FileExists(bakname) then DeleteFile(bakname);
      RenameFile(filename, bakname);

      FileList := TStringList.create;
      try
        FileList.LoadFromFile(bakname);

        if Pos('<flame name="' + title + '"', FileList.Text) <> 0 then
        begin
          i := 0;
          while Pos('<flame name="' + title + '"', Trim(FileList[i])) = 0 do
            inc(i);

          p := 0;
          while p = 0 do
          begin
            p := Pos('</flame>', FileList[i]);
            FileList.Delete(i);
          end;
        end;

//      FileList := TStringList.create;
//      try
//        FileList.LoadFromFile(filename);

        // fix first line
        if (FileList.Count > 0) then begin
          FileList[0] := '<flames name="' + Tag + '">';
        end;

        if FileList.Count > 2 then
        begin
          if pos('<flame ', FileList.text) <> 0 then
            repeat
              FileList.Delete(FileList.Count - 1);
            until (Pos('</flame>', FileList[FileList.count - 1]) <> 0)
          else
            repeat
              if FileList.Count > 0 then
                FileList.Delete(FileList.Count - 1);
            until (Pos('<' + Tag + '>', FileList[FileList.count - 1]) <> 0) or
                  (Pos('</flames>', FileList[FileList.count - 1]) <> 0) or
                  (FileList.Count = 0);
        end else
        begin
          FileList.Delete(FileList.Count - 1);
        end;

        FileList.Add(Trim(FlameToXML(cp1, false, true)));
        FileList.Add('</flames>');
        FileList.SaveToFile(filename);

      finally
        if FileExists(bakname) and not FileExists(filename) then
          RenameFile(bakname, filename);

        FileList.Free;
      end;
    end
    else
    begin
    // New file ... easy
      FileList := TStringList.Create;
      FileList.Text := '<flames name="' + Tag + '">' + #$0D#$0A +
        FlameToXML(cp1, false, true) + #$0D#$0A + '</flames>';
      FileList.SaveToFile(filename, TEncoding.UTF8);
      FileList.Destroy;
    end;
  except 
    begin
      Application.MessageBox(PChar(Format(TextByKey('common-genericsavefailure'), [FileName])), 'Apophysis', 16);
      Result := False;
    end;
  end;
end;

function TMainForm.SaveGradient(Gradient, Title, FileName: string): boolean;
{ Saves gradient parameters to end of file }
var
  IFile: TextFile;
begin
  Result := True;
  try
    AssignFile(IFile, FileName);
    if FileExists(FileName) then
    begin
      if EntryExists(Title, FileName) then DeleteEntry(Title, FileName);
      Append(IFile);
    end
    else
      ReWrite(IFile);
    Write(IFile, Gradient);
    WriteLn(IFile, ' ');
    CloseFile(IFile);
  except on EInOutError do
    begin
      Application.MessageBox(PChar(Format(TextByKey('common-genericsavefailure'), [FileName])), 'Apophysis', 16);
      Result := False;
    end;
  end;
end;

function RenameIFS(OldIdent: string; var NewIdent: string): boolean;
{ Renames an IFS parameter set in a file }
var
  Strings: TStringList;
  p, i: integer;
  s: string;
begin
  Result := True;
  NewIdent := CleanEntry(NewIdent);
  Strings := TStringList.Create;
  try
    try
      i := 0;
      Strings.LoadFromFile(OpenFile);
      if Pos(OldIdent + ' ', Trim(Strings.Text)) <> 0 then
      begin
        while Pos(OldIdent + ' ', Trim(Strings[i])) <> 1 do
        begin
          inc(i);
        end;
        p := Pos('{', Strings[i]);
        s := Copy(Strings[i], p, Length(Strings[i]) - p + 1);
        Strings[i] := NewIdent + ' ' + s;
        Strings.SaveToFile(OpenFile);
      end
      else
        Result := False;
    except on Exception do Result := False;
    end;
  finally
    Strings.Free;
  end;
end;

function RenameXML(OldIdent: string; var NewIdent: string): boolean;
{ Renames an XML parameter set in a file }
var
  Strings: TStringList;
  i: integer;
  bakname: string;
begin
  Result := True;
  Strings := TStringList.Create;
  try
    try
      i := 0;
      Strings.LoadFromFile(OpenFile);
      if Pos('name="' + OldIdent + '"', Strings.Text) <> 0 then
      begin
        while Pos('name="' + OldIdent + '"', Strings[i]) = 0 do
        begin
          inc(i);
        end;
        Strings[i] := StringReplace(Strings[i], OldIdent, NewIdent, []);

        bakname := ChangeFileExt(OpenFile, '.bak');
        if FileExists(bakname) then DeleteFile(bakname);
        RenameFile(OpenFile, bakname);

        Strings.SaveToFile(OpenFile);
      end
      else
        Result := False;
    except on Exception do Result := False;
    end;
  finally
    Strings.Free;
  end;
end;

{ ****************************** Display ************************************ }

procedure Trace1(const str: string);
begin
  if TraceLevel >= 1 then
    TraceForm.MainTrace.Lines.Add('. ' + str);
end;

procedure Trace2(const str: string);
begin
  if TraceLevel >= 2 then
    TraceForm.MainTrace.Lines.Add('. . ' + str);
end;

procedure TMainForm.HandleThreadCompletion(var Message: TMessage);
var
  oldscale: double;
begin
  if PreviewThreadChangingContext then
  begin
    HandleThreadTermination(Message);
    PreviewThreadChangingContext := false;
    Exit;
  end;

  Trace2(MsgComplete + IntToStr(message.LParam));
  if not Assigned(Renderer) then begin
    Trace2(MsgNotAssigned);
    exit;
  end;
  if Renderer.ThreadID <> message.LParam then begin
    Trace2(MsgAnotherRunning);
    exit;
  end;
  PreviewImage.Cursor := crDefault;

  if assigned(TransparentPreviewImage) then begin
    oldscale := TransparentPreviewImage.Width / PreviewImage.Width;
    TransparentPreviewImage.Free;
  end
  else oldscale := FViewScale;

  TransparentPreviewImage := Renderer.GetTransparentImage;

  if (TransparentPreviewImage <> nil) and (TransparentPreviewImage.Width > 0) then begin
    FViewScale := TransparentPreviewImage.Width / PreviewImage.Width;

    FViewPos.X := FViewScale/oldscale * (FViewPos.X - FViewOldPos.X);
    FViewPos.Y := FViewScale/oldscale * (FViewPos.Y - FViewOldPos.Y);

    DrawImageView;
{
    case FMouseMoveState of
      msZoomWindowMove: FMouseMoveState := msZoomWindow;
      msZoomOutWindowMove: FMouseMoveState := msZoomOutWindow;
//    msDragMove: FMouseMoveState := msDrag;
      msRotateMove: FMouseMoveState := msRotate;
    end;
}
    if CurrentCursorMode in [msZoomWindowMove, msZoomOutWindowMove, msRotateMove] then
      IsDrawingSelection := false;

    Trace1(TimeToStr(Now) + ' : Render complete');
    Renderer.ShowSmallStats;
  end
  else Trace2('WARNING: No image rendered!');

  Renderer.WaitFor;
  Trace2('Destroying RenderThread #' + IntToStr(Renderer.ThreadID));
  Renderer.Free;
  Renderer := nil;
  Trace1('');
end;

procedure TMainForm.HandleThreadTermination(var Message: TMessage);
begin
  Trace2(MsgTerminated + IntToStr(message.LParam));
  if not Assigned(Renderer) then begin
    Trace2(MsgNotAssigned);
    exit;
  end;
  if Renderer.ThreadID <> message.LParam then begin
    Trace2(MsgAnotherRunning);
    exit;
  end;
  PreviewImage.Cursor := crDefault;
  Trace2('  Render aborted');

  Trace2('Destroying RenderThread #' + IntToStr(Renderer.ThreadID));
  Renderer.Free;
  Renderer := nil;
  Trace1('');
end;

procedure TMainForm.DrawPreview;
var
  cp : TControlPoint;
  Render : TRenderer;
  BM:TBitmap;
begin
  Render := TRenderer.Create;
  bm := TBitmap.Create;
  Render := TRenderer.Create;

  cp := MainCP.Clone;

  cp.sample_density := 1;
  cp.spatial_oversample := 1;
  cp.spatial_filter_radius := 1;

  Render.NrThreads := NrTreads;
  Render.SetCP(cp);
  Render.Render;
  BM.Assign(Render.GetImage);
  PreviewImage.Picture.Graphic := bm;
end;

procedure TMainForm.DrawFlame;
var
  GlobalMemoryInfo: TMemoryStatus; // holds the global memory status information
  RenderCP: TControlPoint;
  Mem, ApproxMem: cardinal;
  bs: integer;
begin
  PreviewRedrawDelayTimer.Enabled := False;
  if Assigned(Renderer) then begin
    assert(Renderer.Suspended = false);

    Trace2('Killing previous RenderThread #' + inttostr(Renderer.ThreadID));
    Renderer.Terminate;
    Renderer.WaitFor;
    Trace2('Destroying RenderThread #' + IntToStr(Renderer.ThreadID));

    Renderer.Free;
    Renderer := nil;
  end;

  if not Assigned(Renderer) then
  begin
    if EditForm.Visible and ((MainCP.Width / MainCP.Height) <> (EditForm.cp.Width / EditForm.cp.Height))
      then EditForm.UpdateDisplay(true); // preview only?
    if AdjustForm.Visible then AdjustForm.UpdateDisplay(true); // preview only!

    RenderCP := MainCP.Clone;
    RenderCp.AdjustScale(PreviewImage.width, PreviewImage.height);

    // following needed ?
//    cp.Zoom := Zoom;
//    cp.center[0] := center[0];
//    cp.center[1] := center[1];

    RenderCP.sample_density := defSampleDensity;
    // oversample and filter are just slowing us down here...
    RenderCP.spatial_oversample := 1; // defOversample;
    RenderCP.spatial_filter_radius := 0.001; {?} //defFilterRadius;
    RenderCP.Transparency := true; // always generate transparency here

    GlobalMemoryInfo.dwLength := SizeOf(GlobalMemoryInfo);
    GlobalMemoryStatus(GlobalMemoryInfo);
    Mem := GlobalMemoryInfo.dwAvailPhys;

    if (singleBuffer) then bs := 16
    else bs := 32;

//    if Output.Lines.Count >= 1000 then Output.Lines.Clear;
    Trace1('--- Previewing "' + RenderCP.name + '" ---');
    Trace1(Format('  Available memory: %f Mb', [Mem / (1024*1024)]));
    ApproxMem := int64(RenderCp.Width) * int64(RenderCp.Height) {* sqr(Oversample)}
                 * (bs + 4 + 4); // +4 for temp image(s)...?
    assert(MainPreviewScale <> 0);
    if ApproxMem * sqr(MainPreviewScale) < Mem then begin
      if ExtendMainPreview then begin
        RenderCP.sample_density := RenderCP.sample_density / sqr(MainPreviewScale);
        RenderCP.Width := round(RenderCp.Width * MainPreviewScale);
        RenderCP.Height := round(RenderCp.Height * MainPreviewScale);
      end;
    end
    else Trace1('WARNING: Not enough memory for extended preview!');
    if ApproxMem > Mem then
      Trace1('OUTRAGEOUS: Not enough memory even for normal preview! :-(');
    Trace1(Format('  Size: %dx%d, Quality: %f',
                  [RenderCP.Width, RenderCP.Height, RenderCP.sample_density]));
    FViewOldPos.x := FViewPos.x;
    FViewOldPos.y := FViewPos.y;
    StartTime := Now;
    try
      Renderer := TRenderThread.Create;
      Renderer.TargetHandle := MainForm.Handle;
      if TraceLevel > 0 then Renderer.Output := TraceForm.MainTrace.Lines;
      Renderer.OnProgress := OnProgress;
      Renderer.SetCP(RenderCP);
      Renderer.NrThreads := PreviewThreadCount;

      Trace2('Starting RenderThread #' + inttostr(Renderer.ThreadID));
      Renderer.Resume;

      PreviewImage.Cursor := crAppStart;
    except
      Trace1('ERROR: Cannot start renderer!');
    end;
    RenderCP.Free;

  end;
end;

{ ************************** IFS and triangle stuff ************************* }

function FlameToString(Title: string): string;
{ Creates a string containing the formated flame parameter set }
var
  I: integer;
  sl, Strings: TStringList;
begin
  Strings := TStringList.Create;
  sl := TStringList.Create;
  try
    Strings.Add(CleanEntry(Title) + ' {');
    MainCp.SaveToStringList(sl);
    Strings.Add(sl.text);
    Strings.Add('palette:');
    for i := 0 to 255 do
    begin
      Strings.Add(IntToStr(MainCp.cmap[i][0]) + ' ' +
        IntToStr(MainCp.cmap[i][1]) + ' ' +
        IntToStr(MainCp.cmap[i][2]))
    end;
    Strings.Add('}');
    Result := Strings.Text;
  finally
    sl.Free;
    Strings.Free;
  end;
end;

procedure TMainForm.EmptyBatch;
var
  F: TextFile;
  b, RandFile: string;
  cmap_index,ci: integer;
begin
  b := IntToStr(1);
  inc(MainSeed);
  RandSeed := MainSeed;
  try
    AssignFile(F, GetEnvVarValue('APPDATA') + '\' + randFilename);
    OpenFile := GetEnvVarValue('APPDATA') + '\' + randFilename;
    ReWrite(F);
    WriteLn(F, '<random_batch>');

    inc(RandomIndex);
    Statusbar.SimpleText := Format(TextByKey('main-status-batchgenerate'), [1, b]);
    RandSeed := MainSeed;
    cmap_index := random(NRCMAPS);
    inc(MainSeed);
    RandSeed := MainSeed;
    ClearCp(MainCp);
    //MainCp.CalcBoundbox;

    MainCp.name := RandomPrefix + RandomDate + '-' + IntToStr(RandomIndex);
    ci := Random(256); //Random(NRCMAPS);
    GetCMap(ci, 1, MainCp.cmap);
    MainCp.cmapIndex := ci;
    Write(F, FlameToXML(MainCp, False, false));

    Write(F, '</random_batch>');
    CloseFile(F);
  except
    on EInOutError do Application.MessageBox(PChar(TextByKey('main-status-batcherror')), PChar('Apophysis'), 16);
  end;
  RandFile := GetEnvVarValue('APPDATA') + '\' + randFilename;
  MainCp.name := '';
end;

procedure TMainForm.RandomBatch;
{ Write a series of random ifs to a file }
var
  i: integer;
  F: TextFile;
  b, RandFile: string;
  cmap_index,ci: integer;
begin
  b := IntToStr(BatchSize);
  inc(MainSeed);
  RandSeed := MainSeed;
  try
    AssignFile(F, GetEnvVarValue('APPDATA') + '\' + randFilename);
    OpenFile := GetEnvVarValue('APPDATA') + '\' + randFilename;
    ReWrite(F);
    WriteLn(F, '<random_batch>');
    for i := 0 to BatchSize - 1 do
    begin
      inc(RandomIndex);
      Statusbar.SimpleText := Format(TextByKey('main-status-batchgenerate'), [(i+1), b]);
      RandSeed := MainSeed;
      cmap_index := random(NRCMAPS);
      inc(MainSeed);
      RandSeed := MainSeed;
      RandomizeCP(MainCp);
      MainCp.CalcBoundbox;

(*     Title := RandomPrefix + RandomDate + '-' +
        IntToStr(RandomIndex);
  *)
      MainCp.name := RandomPrefix + RandomDate + '-' +
        IntToStr(RandomIndex);
      Write(F, FlameToXML(MainCp, False, false));
//      Write(F, FlameToString(Title));
//      WriteLn(F, ' ');
    end;
    Write(F, '</random_batch>');
    CloseFile(F);
  except
    on EInOutError do Application.MessageBox(PChar(TextByKey('main-status-batcherror')), PChar('Apophysis'), 16);
  end;
  RandFile := GetEnvVarValue('APPDATA') + '\' + randFilename;
  MainCp.name := '';
end;

{ ******************************** Menu ************************************ }

function LoadXMLFlameText(filename, name: string) : string;
var
  i, p: integer;
  FileStrings: TStringList;
  ParamStrings: TStringList;
  Tokens: TStringList;
  time: integer;
begin
  time := -1;
  FileStrings := TStringList.Create;
  ParamStrings := TStringList.Create;
  Result := '';

  if pos('*untitled', name) <> 0 then
  begin
    Tokens := TStringList.Create;
    GetTokens(name, tokens);
    time := StrToInt(tokens[1]);
    Tokens.free;
  end;
  try
    if UpperCase(ExtractFileExt(filename)) = '.PNG' then
    else
      FileStrings.LoadFromFile(filename);

    for i := 0 to FileStrings.Count - 1 do
    begin
      pname := '';
      ptime := '';
      p := Pos('<flame ', LowerCase(FileStrings[i]));
      if (p <> 0) then
      begin
        MainForm.ListXMLScanner.LoadFromBuffer(TCharType(TStringType(FileStrings[i])));
        MainForm.ListXMLScanner.Execute;
        if pname <> '' then
        begin
          if (Trim(pname) = Trim(name)) then
          begin
            ParamStrings.Add(FileStrings[i]);
            Break;
          end;
        end
        else
        begin
          if ptime <> '' then
          begin
            if StrToInt(ptime) = time then
            begin
              ParamStrings.Add(FileStrings[i]);
              Break;
            end;
          end;
        end;
      end;
    end;
    repeat
      inc(i);
      ParamStrings.Add(FileStrings[i]);
    until pos('</flame>', Lowercase(FileStrings[i])) <> 0;

    Result := ParamStrings.Text;

  finally
    FileStrings.free;
    ParamStrings.free;
  end;
end;

procedure AddThumbnail(renderer : TRenderer; width, height : double);
var
  Bmp: TBitmap;
  x, y : double;
begin
  Bmp := TBitmap.Create;
  Bmp.PixelFormat := pf24bit;
  Bmp.HandleType := bmDIB;
  Bmp.Width := ThumbnailSize;
  Bmp.Height := ThumbnailSize;

  x := ThumbnailSize / 2;
  y := ThumbnailSize / 2;

  x := x - width / 2;
  y := y - height / 2;

  with Bmp.Canvas do begin
    Brush.Color := GetSysColor(5); // window background
    FillRect(Rect(0, 0, Bmp.Width, Bmp.Height));
    Draw(round(x), round(y), renderer.GetImage);
  end;

  MainForm.UsedThumbnails.Add(bmp, nil);
  
  if (Bmp <> nil) then Bmp.Free;
end;

function ScanVariations(name:string):boolean;
var
  i,count:integer;
  vname:string;
begin
  count:=NrVar;
  for i:=0 to count - 1 do
  begin
    vname := VarNames(i);
    if (vname = name) then
    begin
      Result := true;
      exit;
    end;
  end;
  for i := 0 to MainForm.SubstSource.Count - 1 do
  begin
    vname := MainForm.SubstSource[i];
    if (vname = name) then
    begin
      Result := true;
      exit;
    end;
  end;
  Result := false;
end;
function ScanVariables(name:string):boolean;
var
  i,count:integer;
begin
  count:=GetNrVariableNames;
  for i:=0 to count - 1 do
  begin
    if (GetVariableNameAt(i) = name) then
    begin
      Result := true;
      exit;
    end;
  end;
  for i := 0 to MainForm.SubstSource.Count - 1 do
  begin
    if (MainForm.SubstSource[i] = name) then
    begin
      Result := true;
      exit;
    end;
  end;
  Result := false;
end;

procedure TMainForm.OpenBatchFromDisk;
var
  fn:string;
begin
{$ifdef DisableScripting}
{$else}
  ScriptEditor.Stopped := True;
{$endif}
  OpenDialog.Filter := TextByKey('common-filter-flamefiles') + '|*.flame;*.xml|' + TextByKey('common-filter-allfiles') + '|*.*';
  OpenDialog.InitialDir := ParamFolder;
  OpenDialog.FileName := '';
  if OpenSaveFileDialog(MainForm, '.flame', OpenDialog.Filter, OpenDialog.InitialDir, TextByKey('common-browse'), fn, true, false, false, true) then
  //if OpenDialog.Execute then
  begin
    OpenDialog.FileName := fn;
    MainForm.CurrentFileName := OpenDialog.FileName;
    LastOpenFile := OpenDialog.FileName;
    Maincp.name := '';
    ParamFolder := ExtractFilePath(OpenDialog.FileName);
    ListViewMenuRenameItem.Enabled := True;
    ListViewMenuDeleteItem.Enabled := True;
    OpenFile := OpenDialog.FileName;
    //MainForm.Caption := AppVersionString + ' - ' + OpenFile; // --Z--
    if APP_BUILD = '' then MainForm.Caption := AppVersionString + ' - ' + openFile
    else MainForm.Caption := AppVersionString + ' ' + APP_BUILD + ' - ' + openFile;
    OpenFileType := ftXML;
    (*if UpperCase(ExtractFileExt(OpenDialog.FileName)) = '.IFS' then
    begin
      OpenFileType := ftIfs;
      Variation := vLinear;
      VarMenus[0].Checked := True;
    end;
    if (UpperCase(ExtractFileExt(OpenDialog.FileName)) = '.FLA') or
      (UpperCase(ExtractFileExt(OpenDialog.FileName)) = '.APO') then
      OpenFileType := ftFla;   *)
    ListXML(OpenDialog.FileName, 1)
  end;
end;

procedure TMainForm.OnListViewMenuRenameClick(Sender: TObject);
begin
  //Ribbontodo
(*  if ListView.SelCount <> 0 then
    ListView.Items[ListView.Selected].EditCaption;*)
end;

procedure TMainForm.DeleteSelectedFlame;
var
  c: boolean;
begin
  if ListViewManager.SelectedIndex >= 0 then
  begin
    if ConfirmDelete then
      c := Application.MessageBox(
        PChar(Format(TextByKey('common-confirmdelete'), [Batch.GetFlameNameAt(ListViewManager.SelectedIndex)])),
        'Apophysis',
        36) = IDYES
    else
      c := True;

    if c then
    begin
      DeleteXMLEntry(Batch.GetFlameNameAt(ListViewManager.SelectedIndex), OpenFile);
      ListViewManager.RemoveItemAt(ListViewManager.SelectedIndex);
//    ListXML(OpenFile, ListView.Selected);
    end;
  end;
end;

procedure TMainForm.ShowOptionsWindow;
begin
  OptionsForm.ShowModal;
  // --Z--
  StopThread;
  PreviewRedrawDelayTimer.Enabled := True;
  //RIBBONTODO  tbQualityBox.Text := FloatToStr(defSampleDensity);
  //RIBBONTODO  tbShowAlpha.Down := ShowTransparency;
  DrawImageView;
  UpdateWindows;
end;

procedure TMainForm.ShowOutputPropertiesWindow;
begin
  AdjustForm.UpdateDisplay;
  AdjustForm.PageControl.TabIndex:=1;
  AdjustForm.Show;
end;

procedure TMainForm.BeginUpdatePreview;
begin
  PreviewRedrawDelayTimer.enabled := true;
end;

function GradientString(c: TColorMap): string;
var
  strings: TStringList;
  i, j, cl: integer;
begin
  strings := TStringList.Create;
  for i := 0 to 255 do
  begin
    j := round(i * (399 / 255));
    cl := (c[i][2] shl 16) + (c[i][1] shl 8) + (c[i][0]);
    strings.Add(' index=' + IntToStr(j) + ' color=' + intToStr(cl));
  end;
  Result := Strings.Text;
  strings.Free;
end;

procedure TMainForm.ShowEditorWindow;
begin
  EditForm.Show;
end;

procedure TMainForm.Close;
begin
{$ifdef DisableScripting}
{$else}
  ScriptEditor.Stopped := True;
{$endif}
  Close(TUiCommandAction.DefaultArgs);
end;


procedure TMainForm.SaveSelectedFlameToBatch;
{ Save parameters to a file }
begin
  SaveForm.SaveType := stSaveParameters;
  SaveForm.Filename := SavePath;
  SaveForm.Title := maincp.name;

  if SaveForm.ShowModal = mrOK then
  begin
    maincp.name := SaveForm.Title;
    SavePath := SaveForm.Filename;
    if ExtractFileExt(SavePath) = '' then
      SavePath := SavePath + '.flame';
    SaveXMLFlame(maincp, maincp.name, SavePath);
    StatusBar.Panels[3].Text := maincp.name;
    if (SavePath = OpenFile) then ListXML(OpenDialog.FileName, 0);
  end;
end;

procedure TMainForm.SaveBatchToDisk;
{ Save all parameters to a file }
var
  i, current: integer;
  currentXML : string;
begin
  SaveForm.SaveType := stSaveAllParameters;
  SaveForm.Filename := SavePath;

  if SaveForm.ShowModal = mrOK then
  begin
    SavePath := SaveForm.Filename;
    if ExtractFileExt(SavePath) = '' then 
      SavePath := SavePath + '.flame';
    current := ListViewManager.SelectedIndex;
    currentXML := Trim(FlameToXML(Maincp, false, true));
    for i := 0 to Batch.Count-1 do
    begin

      // -x- wtf?
      if (i = current) then begin
        ParseXML(maincp, PCHAR(currentXML), true);
        SaveXMLFlame(maincp, maincp.name, SavePath);
      end else begin
        LoadXMLFlame(OpenFile, Batch.GetFlameNameAt(i));
        SaveXMLFlame(maincp, maincp.name, SavePath);
      end;

    end;

    ListXML(SavePath, 2);
    if (current < 0) then current := 0;
    LoadXMLFlame(SavePath, Batch.GetFlameNameAt(ListViewManager.SelectedIndex));
  end;
end;

function GradTitle(str: string): string;
var
  p: integer;
begin
  p := pos('{', str);
  GradTitle := Trim(copy(str, 1, p - 1));
end;

procedure TMainForm.DisplayHint(Sender: TObject);
var
  T: TComponent;
begin
  T := MainForm.FindComponent('StatusBar');
  if T <> nil then
    if Application.Hint = '' then
    begin
      TStatusBar(T).SimpleText := '';
      TStatusBar(T).SimplePanel := False;
      TStatusBar(T).Refresh;
    end
    else
      TStatusBar(T).SimpleText := Application.Hint;
end;

{ ********************************* Form ************************************ }
procedure TMainForm.FavoriteClick(Sender: TObject);
var
  i: integer;
  s: string;
begin
{$ifdef DisableScripting}
{$else}
  i := TMenuItem(Sender).Tag;
  Script := favorites[i];
  ScriptEditor.Editor.Lines.LoadFromFile(Script);

  s := ExtractFileName(Script);
  s := Copy(s, 0, length(s) - Length(ExtractFileExt(s)));
  mnuRun.Caption := Format(TextByKey('main-menu-script-run2'), [s]);//'Run "' + s + '"';
  btnRunScript.Hint := Format(TextByKey('main-menu-script-run2'), [s]);//'Run Script (F8)|Runs the ' + s + ' script.';
  //ScriptEditor.Caption := s;
  ScriptEditor.RunScript;

{$endif}
end;

procedure TMainForm.ScriptItemClick(Sender: TObject);
var
  s: string;
begin
{$ifdef DisableScripting}
{$else}
  Script := ExtractFilePath(Application.ExeName) + scriptPath + '\' + TMenuItem(Sender).Hint + '.asc';
  ScriptEditor.Editor.Lines.LoadFromFile(Script);
  s := ExtractFileName(Script);
  s := Copy(s, 0, length(s) - Length(ExtractFileExt(s)));
  mnuRun.Caption := Format(TextByKey('main-menu-script-run2'), [s]);//'Run "' + s + '"';
  btnRunScript.Hint := Format(TextByKey('main-menu-script-run2'), [s]);//'Run Script (F8)|Runs the ' + s + ' script.';
  //ScriptEditor.Caption := s;
  ScriptEditor.RunScript;
{$endif}
end;

procedure TMainForm.GetScripts;
var
  NewItem: TMenuItem;
  NewItem2 : TMenuItem;
  searchResult: TSearchRec;
  i: integer;
  s: string;
  sl: TStringList;
  path : string;
begin
  (*
  sl := TStringList.Create;
  s := TextByKey('main-menu-script-directory');

  NewItem := mnuScript.Find(TextByKey('main-menu-script-directory'));
  if (NewItem <> nil) then mnuScript.Remove(NewItem);
  NewItem := mnuScript.Find(TextByKey('main-menu-script-more'));
  if (NewItem <> nil) then mnuScript.Remove(NewItem);

  {$ifdef DisableScripting}
  {$else}
  if FileExists(ExtractFilePath(Application.ExeName) + scriptFavsFilename) then begin
    Favorites.LoadFromFile(AppPath + scriptFavsFilename);
    if Trim(Favorites.Text) <> '' then begin
      if Favorites.count <> 0 then
      begin
        NewItem := TMenuItem.Create(self);
        NewItem.Caption := '-';
        mnuScript.Add(NewItem);
        for i := 0 to Favorites.Count - 1 do
        begin
          if FileExists(Favorites[i]) then
          begin
            NewItem := TMenuItem.Create(Self);
            if i < 12 then
              NewItem.ShortCut := TextToShortCut('Ctrl+F' + IntToStr(i + 1));
            NewItem.Tag := i;
            s := ExtractFileName(Favorites[i]);
            s := Copy(s, 0, length(s) - Length(ExtractFileExt(s)));
            NewItem.Caption := s;
            //NewItem.Hint := 'Loads and runs the ' + s + ' script.';
            NewItem.OnClick := FavoriteClick;
            OnClick := FavoriteClick;
            mnuScript.Add(NewItem);
            sl.Add(s);
          end;
        end;
        s := TextByKey('main-menu-script-more');
      end;
    end;
  end;

  // Try to find regular files matching *.asc in the scripts dir
  path := ExtractFilePath(Application.ExeName) + scriptPath + '\*.asc';
  if FindFirst(path, faAnyFile, searchResult) = 0 then begin
      NewItem := TMenuItem.Create(Self);
      NewItem.Caption := '-';
      mnuScript.Add(NewItem);
      NewItem := TMenuItem.Create(Self);
      NewItem.Caption := s;
      repeat
        NewItem2 := TMenuItem.Create(Self);
        s := searchResult.Name;
        s := Copy(s, 0, length(s) - Length(ExtractFileExt(s)));
        NewItem2.Caption := s;
        NewItem2.Hint := s;
        NewItem2.OnClick := ScriptItemClick;
        if (sl.IndexOf(s) < 0) then NewItem.Add(NewItem2);
      until (FindNext(searchResult) <> 0);
      FindClose(searchResult);
      mnuScript.Add(NewItem);
  end;

  // -X- Copypaste code...me lazy
  path := ExtractFilePath(Application.ExeName) + scriptPath + '\*.aposcript';
  if FindFirst(path, faAnyFile, searchResult) = 0 then begin
      NewItem := TMenuItem.Create(Self);
      NewItem.Caption := '-';
      mnuScript.Add(NewItem);
      NewItem := TMenuItem.Create(Self);
      NewItem.Caption := s;
      repeat
        NewItem2 := TMenuItem.Create(Self);
        s := searchResult.Name;
        s := Copy(s, 0, length(s) - Length(ExtractFileExt(s)));
        NewItem2.Caption := s;
        NewItem2.Hint := s;
        NewItem2.OnClick := ScriptItemClick;
        if (sl.IndexOf(s) < 0) then NewItem.Add(NewItem2);
      until (FindNext(searchResult) <> 0);
      FindClose(searchResult);
      mnuScript.Add(NewItem);
  end;

  {$endif}
  *)
end;

procedure TMainForm.RibbonLoaded;
var settingsFile: string;
begin
  {$ifdef DisableScripting}
    Ribbon.SetApplicationModes(0);
  {$else}
    Ribbon.SetApplicationModes(1);
  {$endif}

  settingsFile:= GetEnvVarValue('APPDATA') + '\apophysis-ribbon.dat';
  if FileExists(settingsFile) then
    Ribbon.LoadSettings(settingsFile);
end;

procedure TMainForm.CommandCreated(const Sender: TUIRibbon; const Command: TUICommand);
var
  densityCommand: TUiCommandAction;
begin
  inherited;
  case Command.CommandId of
    NewFlameCommand_Id: 
      begin
        NewFlameCommand := Command as TUICommandAction;
        NewFlameCommand.SetShortCut([ssCtrl], 'N');
        NewFlameCommand.OnExecute := CreateAndSelectNewFlame;
      end;
    OpenBatchCommand_Id: 
      begin
        OpenBatchCommand := Command as TUICommandAction;
        OpenBatchCommand.SetShortCut([ssCtrl], 'O');
        OpenBatchCommand.OnExecute := OpenBatchFromDisk;
      end;
    SaveFlameCommand_Id: 
      begin
        SaveFlameCommand := Command as TUICommandAction;
        SaveFlameCommand.SetShortCut([ssCtrl], 'S');
        SaveFlameCommand.OnExecute := SaveSelectedFlameToBatch;
      end;
    SaveBatchCommand_Id: 
      begin
        SaveBatchCommand := Command as TUICommandAction;
        SaveBatchCommand.SetShortCut([ssCtrl, ssAlt], 'S');
        SaveBatchCommand.OnExecute :=  SaveBatchToDisk;
      end;
    RestoreLastAutosaveCommand_Id: 
      begin
        RestoreLastAutosaveCommand := Command as TUICommandAction;
        RestoreLastAutosaveCommand.SetShortCut([ssCtrl, ssAlt], 'A');
        RestoreLastAutosaveCommand.OnExecute := OpenLastAutomaticallySavedBatch;
      end;

    UndoCommand_Id:
      begin
        UndoCommand := Command as TUICommandAction;
        UndoCommand.SetShortCut([ssCtrl], 'Z');
        UndoCommand.OnExecute := Undo;
      end;
    RedoCommand_Id:
      begin
        RedoCommand := Command as TUICommandAction;
        RedoCommand.SetShortCut([ssCtrl], 'Y');
        RedoCommand.OnExecute := Redo;      
      end;
    CopyCommand_Id:
      begin
        CopyCommand := Command as TUICommandAction;
        CopyCommand.SetShortCut([ssCtrl], 'C');
        CopyCommand.OnExecute := CopySelectedFlameToClipboard;       
      end;
    PasteCommand_Id:
      begin
        PasteCommand := Command as TUICommandAction;
        PasteCommand.SetShortCut([ssCtrl], 'V');
        PasteCommand.OnExecute := ReplaceSelectedFlameWithClipboard;       
      end;
      
    RenderFlameCommand_Id:
      begin
        RenderFlameCommand := Command as TUICommandAction;
        RenderFlameCommand.SetShortCut([ssCtrl], 'R');
        RenderFlameCommand.OnExecute := RenderSelectedFlame;       
      end;
    RenderBatchCommand_Id:
      begin
        RenderBatchCommand := Command as TUICommandAction;
        RenderBatchCommand.SetShortCut([ssCtrl, ssAlt], 'R');
        RenderBatchCommand.OnExecute := RenderBatch;       
      end;

    FullscreenPreviewCommand_Id:
      begin
        FullscreenPreviewCommand := Command as TUICommandAction;
        FullscreenPreviewCommand.SetShortCut([], VK_F3);
        FullscreenPreviewCommand.OnExecute := ShowFullscreenPreviewWindow;       
      end;
    ShowEditorCommand_Id:
      begin
        ShowEditorCommand := Command as TUICommandAction;
        ShowEditorCommand.SetShortCut([], VK_F4);
        ShowEditorCommand.OnExecute := ShowEditorWindow; 
      end;
    ShowAdjustmentCommand_Id:
      begin
        ShowCameraEditorCommand := Command as TUICommandAction;
        ShowCameraEditorCommand.SetShortCut([], VK_F5);
        ShowCameraEditorCommand.OnExecute := ShowCameraWindow;  
      end;
    ShowOutputPropertiesCommand_Id:
      begin
        ShowOutputPropertiesCommand := Command as TUICommandAction;
        ShowOutputPropertiesCommand.SetShortCut([], VK_F6);
        ShowOutputPropertiesCommand.OnExecute := ShowOutputPropertiesWindow;  
      end;
    ShowPaletteCommand_Id:
      begin
        ShowPaletteCommand := Command as TUICommandAction;
        ShowPaletteCommand.SetShortCut([], VK_F7);
        ShowPaletteCommand.OnExecute := ShowPaletteWindow;
      end;
    ShowCanvasCommand_Id:
      begin
        ShowPaletteCommand := Command as TUICommandAction;
        ShowPaletteCommand.SetShortCut([], VK_F8);
        ShowPaletteCommand.OnExecute := ShowCanvasSizeWindow;
      end;
    PaletteFromImageCommand_Id:
      begin
        PaletteFromImageCommand := Command as TUICommandAction;
        PaletteFromImageCommand.OnExecute := CreatePaletteFromImage;
      end;

    {$IfNDef DisableScripts}
    RunScriptCommand_Id:
      begin
        RunScriptCommand := Command as TUICommandAction;
        RunScriptCommand.SetShortCut([], VK_F9);
        RunScriptCommand.OnExecute := RunCurrentScript;
      end;
    StopScriptCommand_Id:
      begin
        StopScriptCommand := Command as TUICommandAction;
        StopScriptCommand.SetShortCut([ssShift], VK_F2);
        StopScriptCommand.OnExecute := StopCurrentScript;
      end;
    OpenScriptCommand_Id:
      begin
        OpenScriptCommand := Command as TUICommandAction;
        OpenScriptCommand.OnExecute := OpenScriptFromDisk;
      end;
    EditScriptCommand_Id:
      begin
        EditScriptCommand := Command as TUICommandAction;
        EditScriptCommand.OnExecute := ShowScriptEditorWindow;
      end;
    ManageScriptFavoritesCommand_Id:
      begin
        ManageScriptFavoritesCommand := Command as TUICommandAction;
        ManageScriptFavoritesCommand.OnExecute := ShowScriptFavoritesWindow;
      end;
    {$EndIf}

    PanModeCommand_Id:
      begin
        PanModeCommand := Command as TUICommandBoolean;
        PanModeCommand.OnToggle := SetCursorMode;
        PanModeCommand.Checked := true;
      end;
    RotateModeCommand_Id:
      begin
        RotateModeCommand := Command as TUICommandBoolean;
        RotateModeCommand.OnToggle := SetCursorMode;
      end;
    ZoomInCommand_Id:
      begin
        ZoomInModeCommand := Command as TUICommandBoolean;
        ZoomInModeCommand.OnToggle := SetCursorMode;
      end;
    ZoomOutCommand_Id:
      begin
        ZoomOutModeCommand := Command as TUICommandBoolean;
        ZoomOutModeCommand.OnToggle := SetCursorMode;
      end;

    ShowGuidelinesCommand_Id:
      begin
        ShowGuidelinesCommand := Command as TUICommandBoolean;
        ShowGuidelinesCommand.OnToggle := SetShowGuidelinesInPreview;  
      end;
    ShowTransparencyCommand_Id:
      begin
        ShowTransparencyCommand := Command as TUICommandBoolean;
        ShowTransparencyCommand.OnToggle := SetShowTransparencyInPreview;      
      end;
    ShowIconsCommand_Id:
      begin
        ShowIconsInListViewCommand := Command as TUICommandBoolean;
        ShowIconsInListViewCommand.OnToggle := SetShowIconsInListView;      
      end;

    ViewDensity5, ViewDensity10, ViewDensity15, ViewDensity25, 
    ViewDensity50, ViewDensity100, ViewDensity150, ViewDensity250,
    ViewDensity500, ViewDensity1000:
      begin
        densityCommand := Command as TUICommandAction;
        densityCommand.OnExecute := SetPreviewDensity;      
      end;
    
  end;
end;

procedure TMainForm.OnFormCreated(Sender: TObject);
var
  dte: string;
  showListIconsArgs: TUiCommandBooleanEventArgs;
begin
  //KnownPlugins := TList.Create;

  ApophysisSVN:=APP_VERSION;
  AppVersionString:=APP_NAME+' '+APP_VERSION;

  SubstSource := TStringList.Create;
  SubstTarget := TStringList.Create;

  CreateSubstMap;
  TbBreakWidth := 802;

  ListXmlScanner := TEasyXmlScanner.Create(nil);
  XmlScanner := TXmlScanner.Create(nil);

  MainForm.ListXmlScanner.Normalize := False;
  MainForm.ListXmlScanner.OnStartTag := OnBatchReaderTagEncountered;

  MainForm.XmlScanner.Normalize := False;
  MainForm.XmlScanner.OnContent := OnFlameReaderDataEncountered;
  MainForm.XmlScanner.OnEmptyTag := OnFlameReaderEmptyTagEncountered;
  MainForm.XmlScanner.OnEndTag := OnFlameReaderClosingTagEncountered;
  MainForm.XmlScanner.OnStartTag := OnFlameReaderTagEncountered;

  ListViewManager := TFlameListView.Create(ListView);

  ReadSettings;

  InternalBitsPerSample := 0;
  renderBitsPerSample := 0;

  // Re-save...
  SaveSettings;

  LoadLanguage(LanguageFile);
  InsertStrings;

  AvailableLanguages := TStringList.Create;
  AvailableLanguages.Add('');
  ListLanguages;

  CommandLine := TCommandLine.Create;
  CommandLine.Load;

  if (NXFORMS > 100) then AppVersionString := AppVersionString + ' (' + TextByKey('main-common-title-t500') + ')'
  else if (NXFORMS < 100) or (CommandLine.Lite) then AppVersionString := AppVersionString + ' (' + TextByKey('main-common-title-lite') + ')';

  LockListChangeUpdate := false;
  Screen.Cursors[crEditArrow]  := LoadCursor(HInstance, 'ARROW_WHITE');
  Screen.Cursors[crEditMove]   := LoadCursor(HInstance, 'MOVE_WB');
  Screen.Cursors[crEditRotate] := LoadCursor(HInstance, 'ROTATE_WB');
  Screen.Cursors[crEditScale]  := LoadCursor(HInstance, 'SCALE_WB');
  Caption := AppVersionString + APP_BUILD;

  CurrentCursorMode := msDrag;
  LimitVibrancy := False;
  Favorites := TStringList.Create;
  GetScripts;
  Randomize;
  MainSeed := Random(123456789);
  maincp := TControlPoint.Create;
  ParseCp := TControlPoint.create;
  OpenFileType := ftXML;
  Application.OnHint := DisplayHint;
  AppPath := ExtractFilePath(Application.ExeName);
  CanDrawOnResize := False;

  Dte := FormatDateTime('yymmdd', Now);
  if Dte <> RandomDate then
    RandomIndex := 0;
  RandomDate := Dte;
  //mnuExit.ShortCut := TextToShortCut('Alt+F4');

  //RIBBONTODO  tbQualityBox.Text := FloatToStr(defSampleDensity);
  //RIBBONTODO  tbShowAlpha.Down := ShowTransparency;
  IsDrawingSelection := true;
  FViewScale := 1;
  ThumbnailSize := 128;
  UsedThumbnails := LargeFlameThumbnailsList;
  if (UseSmallThumbnails) then begin
    ThumbnailSize := 96;
    UsedThumbnails := SmallFlameThumbnailsList;
  end;

  LoadThumbnailPlaceholder(ThumbnailSize);

  ListViewPanel.Width := ThumbnailSize + 90;
  BetweenListAndPreviewPanelSplitter.Left := ListViewPanel.Width;

  showListIconsArgs.Command := ShowIconsInListViewCommand;
  showListIconsArgs.Checked := not CommandLine.Lite;
  SetShowIconsInListView(showListIconsArgs);

end;

procedure TMainForm.OnFormShown(Sender: TObject);
var
  Registry: TRegistry;
  i: integer;
  a,b,c,d:integer;
  hnd,hr:Cardinal;
  index: integer;
  mins:integer;
  cmdl : TCommandLine;
  fn, flameXML : string;
  openScript : string;
begin
  //RIBBONTODO  tbGuides.Down := EnableGuides;
  DoNotAskAboutChange := true;
  { Read position from registry }
  Registry := TRegistry.Create;
  try
    Registry.RootKey := HKEY_CURRENT_USER;
    if Registry.OpenKey('\Software\' + APP_NAME + '\Forms\Main', False) then
    begin
      if Registry.ValueExists('Left') then
        MainForm.Left := Registry.ReadInteger('Left');
      if Registry.ValueExists('Top') then
        MainForm.Top := Registry.ReadInteger('Top');
      if Registry.ValueExists('Width') then
        MainForm.Width := Registry.ReadInteger('Width');
      if Registry.ValueExists('Height') then
        MainForm.Height := Registry.ReadInteger('Height');
    end;
    Registry.CloseKey;
  finally
    Registry.Free;
  end;
  { Synchronize menus etc..}
  // should be defaults....
  UndoIndex := 0;
  UndoMax := 0;
  index := 1;
  inc(MainSeed);
  RandSeed := MainSeed;
  Maincp.brightness := defBrightness;
  maincp.gamma := defGamma;
  maincp.vibrancy := defVibrancy;
  maincp.sample_density := defSampleDensity;
  maincp.spatial_oversample := defOversample;
  maincp.spatial_filter_radius := defFilterRadius;
  maincp.gammaThreshRelative := defGammaThreshold;
  inc(MainSeed);
  RandSeed := MainSeed;

// somehow this doesn't work:
//  Image.Width := BackPanel.Width - 2;
//  Image.Height := BackPanel.Height - 2;

// so we'll do it 'bad' way ;-)
  PreviewImage.Align := alNone;

  if FileExists(AppPath + 'default.map') then
  begin
    DefaultPalette := GradientBrowser.LoadFractintMap(AppPath + 'default.map');
    maincp.cmap := DefaultPalette;
  end
  else
  begin
    cmap_index := random(NRCMAPS);
    GetCMap(cmap_index, 1, maincp.cmap);
    DefaultPalette := maincp.cmap;
  end;
  if FileExists(GetEnvVarValue('APPDATA') + '\' + randFilename) then
    DeleteFile(GetEnvVarValue('APPDATA') + '\' + randFilename);

  cmdl := TCommandLine.Create;
  cmdl.Load;

  openScript := '';

  // get filename from command line argument
  if ParamCount > 0 then
    openFile := ParamStr(1)
  else
    openFile := defFlameFile;

  if ((openFile = '') or (not FileExists(openFile))) and (RememberLastOpenFile) then begin
    openFile := LastOpenFile;
    index := LastOpenFileEntry;
  end;

  if FileExists(openFile) and ((LowerCase(ExtractFileExt(OpenFile)) <> '.asc') or (LowerCase(ExtractFileExt(OpenFile)) <> '.aposcript')) then begin
    LastOpenFile := openFile;
    LastOpenFileEntry := index;
  end;

  if (openFile = '') or (not FileExists(openFile)) and ((LowerCase(ExtractFileExt(OpenFile)) <> '.asc') or (LowerCase(ExtractFileExt(OpenFile)) <> '.aposcript')) then
  begin
    MainCp.Width := PreviewImage.Width;
    MainCp.Height := PreviewImage.Height;
    EmptyBatch;
    if APP_BUILD = '' then MainForm.Caption := AppVersionString + ' - ' + TextByKey('main-common-untitled')
    else MainForm.Caption := AppVersionString + ' ' + APP_BUILD + ' - ' + TextByKey('main-common-untitled');
    OpenFile := GetEnvVarValue('APPDATA') + '\' + randFilename;
    ListXML(OpenFile, 1);
    OpenFileType := ftXML;
    DrawFlame;
  end
  else
  begin
    (*if (LowerCase(ExtractFileExt(OpenFile)) = '.apo') or (LowerCase(ExtractFileExt(OpenFile)) = '.fla') then
    begin
      ListFlames(OpenFile, index);
      OpenFileType := ftFla;
    end else*) if (LowerCase(ExtractFileExt(OpenFile)) = '.asc') or (LowerCase(ExtractFileExt(OpenFile)) = '.aposcript')  then
    begin
      openScript := OpenFile;
      RandomBatch;
      if APP_BUILD = '' then MainForm.Caption := AppVersionString + ' - ' + TextByKey('main-common-randombatch')
      else MainForm.Caption := AppVersionString + ' ' + APP_BUILD + ' - ' + TextByKey('main-common-randombatch');
      OpenFile := GetEnvVarValue('APPDATA') + '\' + randFilename;
      ListXML(OpenFile, 1);
      OpenFileType := ftXML;
      if batchsize = 1 then DrawFlame;
    end else begin
      ListXML(OpenFile, index);
      OpenFileType := ftXML;
    end;
    if APP_BUILD = '' then MainForm.Caption := AppVersionString + ' - ' + openFile
    else MainForm.Caption := AppVersionString + ' ' + APP_BUILD + ' - ' + openFile;
//    MainForm.Caption := AppVersionString + ' - ' + openFile;
  end;
  //ListView.SetFocus;
  CanDrawOnResize := True;
  Statusbar.Panels[3].Text := maincp.name;
{
  gradientForm.cmbPalette.Items.clear;
  for i := 0 to NRCMAPS -1 do
    gradientForm.cmbPalette.Items.Add(cMapnames[i]);
  GradientForm.cmbPalette.ItemIndex := 0;
}
  AdjustForm.cmbPalette.Items.clear;
  for i := 0 to NRCMAPS -1 do
    AdjustForm.cmbPalette.Items.Add(cMapnames[i]);
  AdjustForm.cmbPalette.ItemIndex := 0;
//  AdjustForm.cmbPalette.Items.clear;

  DoNotAskAboutChange := false;

  if (AutoSaveFreq = 0) then mins := 1
  else if (AutoSaveFreq = 1) then mins := 2
  else if (AutoSaveFreq = 2) then mins := 5
  else if (AutoSaveFreq = 3) then mins := 10
  else begin
    mins := 5;
    AutoSaveFreq := 2;
    AutoSaveEnabled := false;
  end;

  AutoSaveTimer.Interval := 60 * 1000 * mins;
  AutoSaveTimer.Enabled := AutoSaveEnabled;

  // loading done..now do what is told by cmdline  ...
  if (cmdl.CreateFromTemplate) then begin
    if FileExists(cmdl.TemplateFile) then begin
      fn:=cmdl.TemplateFile;
      flameXML := LoadXMLFlameText(fn, cmdl.TemplateName);
      UpdateUndo;
{$ifdef DisableScripting}
{$else}
  ScriptEditor.Stopped := True;
{$endif}
      StopThread;
      InvokeLoadXML(flameXML);
      Transforms := MainCp.TrianglesFromCP(MainTriangles);
      Statusbar.Panels[3].Text := MainCp.name;
      ResizeImage;
      PreviewRedrawDelayTimer.Enabled := True;
      Application.ProcessMessages;
      UpdateWindows;
      AdjustForm.TemplateRandomizeGradient;
    end;
  end;

  // .. and run autoexec.asc
{$ifdef DisableScripting}
{$else}
  SplashWindow.SetInfo(TextByKey('splash-execstartupscript'));
  if (FileExists(AppPath + 'autoexec.asc')) then begin
    ScriptEditor.LoadRunAndClear(AppPath + 'autoexec.asc');
    mnuRun.Caption := TextByKey('main-menu-script-run');
    btnRunScript.Hint := TextByKey('main-menu-script-run');
  end;

  if (openScript <> '') then begin
    ScriptEditor.LoadScriptFile(openScript);
    ScriptEditor.Show;
  end;
{$endif}

  PreviewThreadCount := Nrtreads;
  if not multithreadedPreview then
    PreviewThreadCount := 1;
end;

function TMainForm.SystemErrorMessage: string;
var 
  P: PChar;
begin
  if FormatMessage(Format_Message_Allocate_Buffer + Format_Message_From_System,
                   nil,
                   GetLastError,
                   0,
                   @P,
                   0,
                   nil) <> 0 then
  begin
    Result := P;
    LocalFree(Integer(P))
  end 
  else 
    Result := '';
end;
function TMainForm.SystemErrorMessage2(errno:cardinal): string;
var 
  P: PChar;
begin
  if FormatMessage(Format_Message_Allocate_Buffer + Format_Message_From_System,
                   nil,
                   errno,
                   0,
                   @P,
                   0,
                   nil) <> 0 then
  begin
    Result := P;
    LocalFree(Integer(P))
  end 
  else 
    Result := '';
end;

procedure TMainForm.OnFormClosed(Sender: TObject; var Action: TCloseAction);
var
  Registry: TRegistry;
begin
  if ConfirmExit and (UndoIndex <> 0) then
    if Application.MessageBox(PChar(TextByKey('common-confirmexit')), 'Apophysis', MB_ICONWARNING or MB_YESNO) <> IDYES then
    begin
      Action := caNone;
      exit;
    end;

{$ifdef DisableScripting}
{$else}
  ScriptEditor.Stopped := True;
{$endif}
  HtmlHelp(0, nil, HH_CLOSE_ALL, 0);
  { To capture secondary window positions }
  if EditForm.visible then EditForm.Close;
  if AdjustForm.visible then AdjustForm.close;
  if GradientBrowser.visible then GradientBrowser.close;
//  if GradientForm.visible then GradientForm.Close;
{$ifdef DisableScripting}
{$else}
  if ScriptEditor.visible then ScriptEditor.Close;
{$endif}

  { Stop the render thread }
  if RenderForm.Visible then RenderForm.Close;
  if assigned(Renderer) then Renderer.Terminate;
  if assigned(Renderer) then Renderer.WaitFor;
  { Write position to registry }
  Registry := TRegistry.Create;
  try
    Registry.RootKey := HKEY_CURRENT_USER;
    if Registry.OpenKey('\Software\' + APP_NAME + '\Forms\Main', True) then
    begin
      if MainForm.WindowState <> wsMaximized then begin
        Registry.WriteInteger('Top', MainForm.Top);
        Registry.WriteInteger('Left', MainForm.Left);
        Registry.WriteInteger('Width', MainForm.Width);
        Registry.WriteInteger('Height', MainForm.Height);
      end;
    end;
  finally
    Registry.Free;
  end;
  Application.ProcessMessages;
  CanDrawOnResize := False;
  if FileExists(GetEnvVarValue('APPDATA') + '\' + randFilename) then
    DeleteFile(GetEnvVarValue('APPDATA') + '\' + randFilename);
  if FileExists(GetEnvVarValue('APPDATA') + '\' + undoFilename) then
    DeleteFile(GetEnvVarValue('APPDATA') + '\' + undoFilename);
  SaveSettings;
  Ribbon.SaveSettings(GetEnvVarValue('APPDATA') + '\apophysis-ribbon.dat');
end;

procedure TMainForm.OnFormDestroyed(Sender: TObject);
begin
  if assigned(Renderer) then Renderer.Terminate;
  if assigned(Renderer) then Renderer.WaitFor;
  if assigned(Renderer) then Renderer.Free;
  if assigned(TransparentPreviewImage) then TransparentPreviewImage.Free;

  MainCP.free;
  ParseCp.free;
  Favorites.Free;
end;

procedure TMainForm.OnFormKeyPressed(Sender: TObject; var Key: Char);
var
  scale: double;
begin
  if Key = #27 then begin
    case CurrentCursorMode of
      msZoomWindowMove:
        CurrentCursorMode := msZoomWindow;
      msZoomOutWindowMove:
        CurrentCursorMode := msZoomOutWindow;
      msDragMove:
        begin
          CurrentCursorMode := msDrag;

          scale := FViewScale * PreviewImage.Width / TransparentPreviewImage.Width;
          FViewPos.X := FViewPos.X - (CursorStart.Right - CursorStart.Left) / scale;
          FViewPos.Y := FViewPos.Y - (CursorStart.Bottom - CursorStart.Top) / scale;
        end;
      msRotateMove:
        CurrentCursorMode := msRotate;
    end;
    DrawImageView;
  end;
{$ifdef DisableScripting}
{$else}
  ScriptEditor.Stopped := True;
{$endif}
end;

{ ****************************** Misc controls ****************************** }

procedure TMainForm.OnPreviewPanelResized(Sender: TObject);
begin
  StopThread;
  if CanDrawOnResize then
    PreviewRedrawDelayTimer.Enabled := True;

  ResizeImage;  
  DrawImageView;
end;

procedure TMainForm.LoadXMLFlame(filename, name: string);
var
  i, p: integer;
  FileStrings: TStringList;
  ParamStrings: TStringList;
  Tokens: TStringList;
  time: integer;
  ax,bx,cx,dx:integer;
  hwn,hr:cardinal;
  px:pansichar;
begin
  time := -1;
  FileStrings := TStringList.Create;
  ParamStrings := TStringList.Create;

  if pos('*untitled', name) <> 0 then
  begin
    Tokens := TStringList.Create;
    GetTokens(name, tokens);
    time := StrToInt(tokens[1]);
    Tokens.free;
  end;
  try
    FileStrings.LoadFromFile(filename);
    for i := 0 to FileStrings.Count - 1 do
    begin
      pname := '';
      ptime := '';
      p := Pos('<flame ', LowerCase(FileStrings[i]));
      if (p <> 0) then
      begin
        MainForm.ListXMLScanner.LoadFromBuffer(TCharType(TStringType(FileStrings[i])));
        MainForm.ListXMLScanner.Execute;
        if pname <> '' then
        begin
          if (Trim(pname) = Trim(name)) then
          begin
            ParamStrings.Add(FileStrings[i]);
            Break;
          end;
        end
        else
        begin
          if ptime='' then ptime:='0'; //hack
          if StrToInt(ptime) = time then
          begin
            ParamStrings.Add(FileStrings[i]);
            Break;
          end;
        end;
      end;
    end;
    repeat
      inc(i);
      ParamStrings.Add(FileStrings[i]);
    until pos('</flame>', Lowercase(FileStrings[i])) <> 0;

{$ifdef DisableScripting}
{$else}
  ScriptEditor.Stopped := True;
{$endif}
    StopThread;
    ParseXML(MainCp,PAramStrings.Text, true);

    SetCanUndo(false);
    PreviewPanelMenuUndoItem.Enabled := False;
    SetCanRedo(false);
    PreviewPanelMenuRedoItem.enabled := False;
    EditForm.mnuUndo.Enabled := False;
    EditForm.mnuRedo.enabled := False;
    EditForm.tbUndo.enabled := false;
    EditForm.tbRedo.enabled := false;
    AdjustForm.btnUndo.enabled := false;
    AdjustForm.btnRedo.enabled := false;

    Transforms := MainCp.TrianglesFromCP(MainTriangles);

    UndoIndex := 0;
    UndoMax := 0;
    if fileExists(GetEnvVarValue('APPDATA') + '\' + undoFilename) then
      DeleteFile(GetEnvVarValue('APPDATA') + '\' + undoFilename);
    Statusbar.Panels[3].Text := Maincp.name;
    PreviewRedrawDelayTimer.Enabled := True;
    Application.ProcessMessages;

    EditForm.SelectedTriangle := 0; // (?)

    UpdateWindows;
  finally
    FileStrings.free;
    ParamStrings.free;
  end;
end;

procedure TMainForm.ResizeImage;
var
  pw, ph: integer;
begin
  pw := PreviewPanel.Width - 2;
  ph := PreviewPanel.Height - 2;
  begin
    if MainCp = nil then
      Exit;

    if (MainCP.Width / MainCP.Height) > (pw / ph) then
    begin
      PreviewImage.Width := pw;
      PreviewImage.Height := round(MainCP.Height / MainCP.Width * pw);
      PreviewImage.Left := 1;
      PreviewImage.Top := (ph - PreviewImage.Height) div 2;
    end
    else begin
      PreviewImage.Height := ph;
      PreviewImage.Width := round(MainCP.Width / MainCP.Height * ph);
      PreviewImage.Top := 1;
      PreviewImage.Left := (pw - PreviewImage.Width) div 2;
    end;
  end;
  //MainCP.AdjustScale(Image.Width, Image.Height);
end;

procedure TMainForm.OnListViewSelectedItemChanged(Sender: TObject; Item: TListItem; Change: TItemChange);
begin
  if (ListViewManager.SelectedIndex >= 0) and (Trim(Batch.GetFlameNameAt(ListViewManager.SelectedIndex)) <> Trim(maincp.name)) then
  begin
    LastOpenFileEntry := ListViewManager.SelectedIndex + 1;
    PreviewRedrawDelayTimer.Enabled := False;
    StopThread;

    IsLoadingBatch := false;
    LoadXMLFlame(OpenFile, Batch.GetFlameNameAt(ListViewManager.SelectedIndex));
    NotifyMissingPlugin;
    ResizeImage;
    PrevListItem := Item;
  end;
end;

procedure TMainForm.UpdateWindows;
begin
  if AdjustForm.visible then AdjustForm.UpdateDisplay;
  if EditForm.visible then EditForm.UpdateDisplay;
  
end;

procedure TMainForm.LoadUndoFlame(index: integer; filename: string);
var
  FStrings: TStringList;
  IFSStrings: TStringList;
  EntryStrings, Tokens: TStringList;
  SavedPal: Boolean;
  i, j: integer;
  s: string;
  Palette: TColorMap;
begin
{$ifdef DisableScripting}
{$else}
  ScriptEditor.Stopped := True;
{$endif}
  FStrings := TStringList.Create;
  IFSStrings := TStringList.Create;
  Tokens := TStringList.Create;
  EntryStrings := TStringList.Create;
  try
    FStrings.LoadFromFile(filename);
    for i := 0 to FStrings.count - 1 do
      if Pos(Format('%.4d-', [UndoIndex]), Trim(FStrings[i])) = 1 then
        break;
    IFSStrings.Add(FStrings[i]);
    repeat
      inc(i);
      IFSStrings.Add(FStrings[i]);
    until Pos('}', FStrings[i]) <> 0;
    for i := 0 to FStrings.count - 1 do
    begin
      if Pos(Format('%.4d-', [UndoIndex]), Trim(Lowercase(FStrings[i]))) = 1 then
        break;
    end;
    inc(i);
    while (Pos('}', FStrings[i]) = 0) and (Pos('palette:', FStrings[i]) = 0) do
    begin
      EntryStrings.Add(FStrings[i]);
      inc(i);
    end;
    SavedPal := false;
    if Pos('palette:', FStrings[i]) = 1 then
    begin
      SavedPal := True;
      inc(i);
      for j := 0 to 255 do begin
        s := FStrings[i];
        GetTokens(s, tokens);
        Palette[j][0] := StrToInt(Tokens[0]);
        Palette[j][1] := StrToInt(Tokens[1]);
        Palette[j][2] := StrToInt(Tokens[2]);
        inc(i);
      end;
    end;
    maincp.Clear;
    FlameString := EntryStrings.Text;
    maincp.zoom := 0;
    maincp.center[0] := 0;
    maincp.center[0] := 0;
    maincp.ParseString(FlameString);
    maincp.sample_density := defSampleDensity;
    Center[0] := maincp.Center[0];
    Center[1] := maincp.Center[1];
//    cp.CalcBoundbox;
//    MainCP.NormalizeWeights;
    Transforms := MainCp.TrianglesFromCP(MainTriangles);
    // Trim undo index from title
    maincp.name := Copy(Fstrings[0], 6, length(Fstrings[0]) - 7);

    if SavedPal then maincp.cmap := palette;
    if AdjustForm.visible then AdjustForm.UpdateDisplay;

    PreviewRedrawDelayTimer.Enabled := True;
    UpdateWindows;
  finally
    IFSStrings.Free;
    FStrings.Free;
    Tokens.free;
    EntryStrings.free;
  end;
end;

procedure TMainForm.ResetLocation;
begin
  maincp.zoom := 0;
  //maincp.FAngle := 0;
  //maincp.Width := Image.Width;
  //maincp.Height := Image.Height;
  maincp.CalcBoundBox;
  center[0] := maincp.center[0];
  center[1] := maincp.center[1];
end;


procedure TMainForm.OnListViewEditCompleted(Sender: TObject; Item: TListItem;
  var S: string);
begin
  if s <> Item.Caption then

    if OpenFIleType = ftXML then
    begin
      if not RenameXML(Item.Caption, s) then
        s := Item.Caption;
    end
    else
      if not RenameIFS(Item.Caption, s) then
        s := Item.Caption

end;

procedure TMainForm.OnListViewMenuDeleteClick(Sender: TObject);
begin
  DeleteSelectedFlame(TUiCommandAction.DefaultArgs);
end;

procedure TMainForm.PreviewRedrawDelayTimerCallback(Sender: TObject);
{ Draw flame when timer fires. This seems to stop a lot of errors }
begin
  if CurrentCursorMode in [msZoomWindowMove, msZoomOutWindowMove, msDragMove, msRotateMove] then exit;

  PreviewRedrawDelayTimer.enabled := False;
  DrawFlame;
end;

procedure TMainForm.ShowPaletteWindow;
begin
  AdjustForm.UpdateDisplay;
  AdjustForm.PageControl.TabIndex:=2;
  AdjustForm.Show;
end;

procedure swapcolor(var clist: array of cardinal; i, j: integer);
var
  t: cardinal;
begin
  t := clist[j];
  clist[j] := clist[i];
  clist[i] := t;
end;

function diffcolor(clist: array of cardinal; i, j: integer): cardinal;
var
  r1, g1, b1, r2, g2, b2: byte;
begin
  r1 := clist[j] and 255;
  g1 := clist[j] shr 8 and 255;
  b1 := clist[j] shr 16 and 255;
  r2 := clist[i] and 255;
  g2 := clist[i] shr 8 and 255;
  b2 := clist[i] shr 16 and 255;
  Result := abs((r1 - r2) * (r1 - r2)) + abs((g1 - g2) * (g1 - g2)) +
    abs((b1 - b2) * (b1 - b2));
end;

procedure TMainForm.CreatePaletteFromImage(const Args: TUiCommandActionEventArgs);
{ From Draves' Smooth palette Gimp plug-in }
var
  Bitmap: TBitMap;
  JPEG: TJPEGImage;
  pal: TColorMap;
  strings: TStringlist;
  ident, FileName: string;
  len, len_best, as_is, swapd: cardinal;
  cmap_best, original, clist: array[0..255] of cardinal;
  p, total, j, rand, tryit, i0, i1, x, y, i, iw, ih: integer;
  fn:string;
begin
  Total := 10000;
  p := 0;
  Bitmap := TBitmap.Create;
  JPEG := TJPEGImage.Create;
  strings := TStringList.Create;
  try
    begin
      inc(MainSeed);
      RandSeed := MainSeed;
      OpenDialog.Filter := Format('%s|*.bmp;*.dib;*.jpg;*.jpeg|%s|*.bmp;*.dib|%s|*.jpg;*.jpeg|%s|*.*',
        [TextByKey('common-filter-allimages'), TextByKey('common-filter-bitmap'),
         TextByKey('common-filter-jpeg'), TextByKey('common-filter-allfiles')]);
      OpenDialog.InitialDir := ImageFolder;
      OpenDialog.Title := TextByKey('common-browse');
      OpenDialog.FileName := '';
      if OpenSaveFileDialog(MainForm, OpenDialog.DefaultExt, OpenDialog.Filter, OpenDialog.InitialDir, TextByKey('common-browse'), fn, true, false, false, true) then
      //if OpenDialog.Execute then
      begin
        OpenDialog.FileName := fn;
        ImageFolder := ExtractFilePath(OpenDialog.FileName);
        Application.ProcessMessages;
        len_best := 0;
        if (UpperCase(ExtractFileExt(Opendialog.FileName)) = '.BMP')
          or (UpperCase(ExtractFileExt(Opendialog.FileName)) = '.DIB') then
          Bitmap.LoadFromFile(Opendialog.FileName);
        if (UpperCase(ExtractFileExt(Opendialog.FileName)) = '.JPG')
          or (UpperCase(ExtractFileExt(Opendialog.FileName)) = '.JPEG') then
        begin
          JPEG.LoadFromFile(Opendialog.FileName);
          with Bitmap do
          begin
            Width := JPEG.Width;
            Height := JPEG.Height;
            Canvas.Draw(0, 0, JPEG);
          end;
        end;
        iw := Bitmap.Width;
        ih := Bitmap.Height;
        for i := 0 to 255 do
        begin
          { Pick colors from 256 random pixels in the image }
          x := random(iw);
          y := random(ih);
          clist[i] := Bitmap.canvas.Pixels[x, y];
        end;
        original := clist;
        cmap_best := clist;
        for tryit := 1 to 10 do
        begin
          clist := original;
          // scramble
          for i := 0 to 255 do
          begin
            rand := random(256);
            swapcolor(clist, i, rand);
          end;
          // measure
          len := 0;
          for i := 0 to 255 do
            len := len + diffcolor(clist, i, i + 1);
          // improve
          for i := 1 to 100000 do
          begin
            inc(p);
            //StatusBar.SimpleText := Format(StringReplace(TextByKey('main-status-calculatingpalette'), '%)', '%%)', [rfReplaceAll, rfIgnoreCase]), [(p div total)]);
            i0 := 1 + random(254);
            i1 := 1 + random(254);
            if ((i0 - i1) = 1) then
            begin
              as_is := diffcolor(clist, i1 - 1, i1) + diffcolor(clist, i0, i0 + 1);
              swapd := diffcolor(clist, i1 - 1, i0) + diffcolor(clist, i1, i0 + 1);
            end
            else if ((i1 - i0) = 1) then
            begin
              as_is := diffcolor(clist, i0 - 1, i0) + diffcolor(clist, i1, i1 + 1);
              swapd := diffcolor(clist, i0 - 1, i1) + diffcolor(clist, i0, i1 + 1);
            end
            else
            begin
              as_is := diffcolor(clist, i0, i0 + 1) + diffcolor(clist, i0, i0 - 1) +
                diffcolor(clist, i1, i1 + 1) + diffcolor(clist, i1, i1 - 1);
              swapd := diffcolor(clist, i1, i0 + 1) + diffcolor(clist, i1, i0 - 1) +
                diffcolor(clist, i0, i1 + 1) + diffcolor(clist, i0, i1 - 1);
            end;
            if (swapd < as_is) then
            begin
              swapcolor(clist, i0, i1);
              len := abs(len + swapd - as_is);
            end;
          end;
          if (tryit = 1) or (len < len_best) then
          begin
            cmap_best := clist;
            len_best := len;
          end;
        end;
        clist := cmap_best;
        // clean
        for i := 1 to 1024 do
        begin
          i0 := 1 + random(254);
          i1 := i0 + 1;
          as_is := diffcolor(clist, i0 - 1, i0) + diffcolor(clist, i1, i1 + 1);
          swapd := diffcolor(clist, i0 - 1, i1) + diffcolor(clist, i0, i1 + 1);
          if (swapd < as_is) then
          begin
            swapcolor(clist, i0, i1);
            len_best := len_best + swapd - as_is;
          end;
        end;
        { Convert to TColorMap, Gradient and save }
        FileName := lowercase(ExtractFileName(Opendialog.FileName));
        ident := CleanEntry(FileName);
        strings.add(ident + ' {');
        strings.add('gradient:');
        strings.add(' title="' + CleanUPRTitle(FileName) + '" smooth=no');
        for i := 0 to 255 do
        begin
          pal[i][0] := clist[i] and 255;
          pal[i][1] := clist[i] shr 8 and 255;
          pal[i][2] := clist[i] shr 16 and 255;
          j := round(i * (399 / 255));
          strings.Add(' index=' + IntToStr(j) + ' color=' + intToStr(clist[i]));
        end;
        strings.Add('}');
        SaveGradient(Strings.Text, Ident, defSmoothPaletteFile);

        StopThread;
        UpdateUndo;
        maincp.cmap := Pal;
        maincp.cmapindex := -1;
        AdjustForm.UpdateDisplay;

        if EditForm.Visible then EditForm.UpdateDisplay;
        PreviewRedrawDelayTimer.enabled := true;

      end;
      StatusBar.SimpleText := '';
    end;
  finally
    Bitmap.Free;
    JPEG.Free;
    strings.Free;
  end;
end;

procedure TMainForm.SetListViewVisibility(const Args: TUiCommandBooleanEventArgs);
var value: boolean;
begin
  if not Assigned(args.Command) then exit;

  value := Args.Checked;
  Args.Command.Checked := value;
  
  ListViewPanel.Visible := value;
  if value then
    BetweenListAndPreviewPanelSplitter.Width := 4
  else BetweenListAndPreviewPanelSplitter.Width := 0;
end;

procedure TMainForm.Undo;
begin
  if UndoIndex = UndoMax then
    SaveFlame(maincp, Format('%.4d-', [UndoIndex]) + maincp.name,
      GetEnvVarValue('APPDATA') + '\' + undoFilename);
  StopThread;
  Dec(UndoIndex);
  LoadUndoFlame(UndoIndex, GetEnvVarValue('APPDATA') + '\' + undoFilename);
  SetCanRedo(true);
  PreviewPanelMenuRedoItem.Enabled := True;
  EditForm.mnuRedo.Enabled := True;
  EditForm.tbRedo.enabled := true;
  AdjustForm.btnRedo.enabled := true;
  if UndoIndex = 0 then begin
    SetCanUndo(false);
    PreviewPanelMenuUndoItem.Enabled := false;
    EditForm.mnuUndo.Enabled := false;
    EditForm.tbUndo.enabled := false;
    AdjustForm.btnUndo.enabled := false;
  end;
  StatusBar.Panels[3].Text := maincp.name;
end;

procedure TMainForm.OnPreviewPanelMenuUndoClick(Sender: TObject);
begin
  Undo(TUiCommandAction.DefaultArgs);

end;

procedure TMainForm.Redo;
begin
  StopThread;
  Inc(UndoIndex);

  assert(UndoIndex <= UndoMax, 'Undo list index out of range!');

  LoadUndoFlame(UndoIndex, GetEnvVarValue('APPDATA') + '\' + undoFilename);
  SetCanUndo(true);
  PreviewPanelMenuUndoItem.Enabled := True;
  EditForm.mnuUndo.Enabled := True;
  EditForm.tbUndo.enabled := true;
  AdjustForm.btnUndo.enabled := true;
  if UndoIndex = UndoMax then begin
    SetCanRedo(false);
    PreviewPanelMenuRedoItem.Enabled := false;
    EditForm.mnuRedo.Enabled := false;
    EditForm.tbRedo.enabled := false;
    AdjustForm.btnRedo.enabled := false;
  end;
  StatusBar.Panels[3].Text := maincp.name;
end;

procedure TMainForm.OnPreviewPanelMenuFullscreenClick(Sender: TObject);
begin
  ShowFullscreenPreviewWindow(TUiCommandAction.DefaultArgs);
end;

procedure TMainForm.OnPreviewPanelMenuRedoClick(Sender: TObject);
begin
  Redo(TUiCommandAction.DefaultArgs);

end;

procedure TMainForm.SavePreviewImageToDisk(const Args: TUiCommandActionEventArgs);
begin
  SaveDialog.DefaultExt := 'bmp';
  SaveDialog.Filter := Format('%s|*.bmp;*.dib|%s|*.*', [TextByKey('common-filter-bitmap'), TextBykey('common-filter-allfiles')]);
  SaveDialog.Filename := maincp.name;
  if SaveDialog.Execute then
    PreviewImage.Picture.Bitmap.SaveToFile(SaveDialog.Filename)
end;

procedure TMainForm.ShowFullscreenPreviewWindow;
begin
  FullScreenForm.ActiveForm := Screen.ActiveForm;
  FullScreenForm.Width := Screen.Width;
  FullScreenForm.Height := Screen.Height;
  FullScreenForm.Top := 0;
  FullScreenForm.Left := 0;
  FullScreenForm.cp.Copy(maincp);
  FullScreenForm.cp.cmap := maincp.cmap;
  FullScreenForm.center[0] := center[0];
  FullScreenForm.center[1] := center[1];
  FullScreenForm.Calculate := True;
  FullScreenForm.Show;
end;

procedure TMainForm.RenderSelectedFlame;
var
  Ext: string;
  NewRender: Boolean;
begin
  NewRender := True;

  if Assigned(RenderForm.Renderer) then
    if Application.MessageBox(PChar(TextByKey('render-status-confirmstop')), 'Apophysis', 36) = ID_NO then
      NewRender := false;

  if NewRender then
  begin

    if Assigned(RenderForm.Renderer) then RenderForm.Renderer.Terminate;
    if Assigned(RenderForm.Renderer) then RenderForm.Renderer.WaitFor; // hmm #1
    RenderForm.ResetControls;
    RenderForm.PageCtrl.TabIndex := 0;

    case renderFileFormat of
      1: Ext := '.bmp';
      2: Ext := '.png';
      3: Ext := '.jpg';
    end;

    //RenderForm.caption := 'Render ' + #39 + maincp.name + #39 + ' to Disk';
    RenderForm.Filename := RenderPath + maincp.name + Ext;
    RenderForm.SaveDialog.FileName := RenderPath + maincp.name + Ext;
    RenderForm.txtFilename.Text := ChangeFileExt(RenderForm.SaveDialog.Filename, Ext);

    RenderForm.cp.Copy(MainCP);
    RenderForm.cp.cmap := maincp.cmap;
    RenderForm.zoom := maincp.zoom;
    RenderForm.Center[0] := center[0];
    RenderForm.Center[1] := center[1];
    if Assigned(RenderForm.Renderer) then RenderForm.Renderer.WaitFor; // hmm #2
  end;
  RenderForm.Show;
end;

procedure TMainForm.RenderBatch;
var
  Ext: string;
  NewRender: Boolean;
begin
  NewRender := True;

  if Assigned(RenderForm.Renderer) then
    if Application.MessageBox(PChar(TextByKey('render-status-confirmstop')), 'Apophysis', 36) = ID_NO then
      NewRender := false;

  if NewRender then
  begin

    if Assigned(RenderForm.Renderer) then RenderForm.Renderer.Terminate;
    if Assigned(RenderForm.Renderer) then RenderForm.Renderer.WaitFor; // hmm #1
    RenderForm.ResetControls;
    RenderForm.PageCtrl.TabIndex := 0;

    case renderFileFormat of
      1: Ext := '.bmp';
      2: Ext := '.png';
      3: Ext := '.jpg';
    end;

    //RenderForm.caption := 'Render all flames to disk';
    RenderForm.bRenderAll := true;
    RenderForm.Filename := RenderPath + maincp.name + Ext;
    RenderForm.SaveDialog.FileName := RenderForm.Filename;
    RenderForm.txtFilename.Text := ChangeFileExt(RenderForm.SaveDialog.Filename, Ext);

    RenderForm.cp.Copy(MainCP);
    RenderForm.cp.cmap := maincp.cmap;
    RenderForm.zoom := maincp.zoom;
    RenderForm.Center[0] := center[0];
    RenderForm.Center[1] := center[1];
    if Assigned(RenderForm.Renderer) then RenderForm.Renderer.WaitFor; // hmm #2
  end;
  RenderForm.Show;
end;

procedure TMainForm.ShowCameraWindow;
begin
  AdjustForm.UpdateDisplay;
  AdjustForm.PageControl.TabIndex := 0;
  AdjustForm.Show;
end;

procedure TMainForm.ResetSelectedFlameCamera;
var
  scale: double;
  dx, dy, cdx, cdy: double;
  sina, cosa: extended;
begin
  UpdateUndo;

  scale := MainCP.pixels_per_unit / MainCP.Width * power(2, MainCP.zoom);
  cdx := MainCP.center[0];
  cdy := MainCP.center[1];

  ResetLocation;

  cdx := MainCP.center[0] - cdx;
  cdy := MainCP.center[1] - cdy;
  Sincos(MainCP.FAngle, sina, cosa);
  if IsZero(sina) then begin
    dy := cdy*cosa {- cdx*sina};
    dx := (cdx {+ dy*sina})/cosa;
  end
  else begin
    dx := cdy*sina + cdx*cosa;
    dy := (dx*cosa - cdx)/sina;
  end;
  FViewPos.x := FViewPos.x - dx * scale * PreviewImage.Width;
  FViewPos.y := FViewPos.y - dy * scale * PreviewImage.Width;

  FViewScale := FViewScale * MainCP.pixels_per_unit / MainCP.Width * power(2, MainCP.zoom) / scale;

  DrawImageView;

  PreviewRedrawDelayTimer.enabled := true;
  UpdateWindows;
end;

procedure TMainForm.ShowAboutWindow;
begin
  AboutForm.ShowModal;
end;

procedure TMainForm.OnFormGotFocus(Sender: TObject);
begin
  if Assigned(Renderer) then Renderer.Priority := tpNormal;
end;

procedure TMainForm.OnFormLostFocus(Sender: TObject);
begin
  if Assigned(Renderer) then Renderer.Priority := tpLower;
end;


procedure TMainForm.ShowScriptEditorWindow;
begin
{$ifdef DisableScripting}
{$else}
  ScriptEditor.Show;
{$endif}
end;

procedure TMainForm.RunCurrentScript;
begin
  {$ifdef DisableScripting}
{$else}
  ScriptEditor.RunScript;
{$endif}
end;

procedure TMainForm.OpenScriptFromDisk;
begin
{$ifdef DisableScripting}
{$else}
  ScriptEditor.OpenScript;
{$endif}
end;

procedure TMainForm.StopCurrentScript;
begin
{$ifdef DisableScripting}
{$else}
  ScriptEditor.Stopped := True;
{$endif}
end;

procedure TMainForm.ShowScriptFavoritesWindow;
var
  MenuItem: TMenuItem;
  i: integer;
  s: string;
begin
{$ifdef DisableScripting}
{$else}
  if FavoritesForm.ShowModal = mrOK then
  begin
    if favorites.count <> 0 then
    begin
      mnuScript.Items[7].free; // remember to increment if add any items above
      for i := 0 to Favorites.Count - 1 do
      begin
        s := ExtractFileName(Favorites[i]);
        s := Copy(s, 0, length(s) - Length(ExtractFileExt(s)));
        MenuItem := mnuScript.Find(s);
        if MenuItem <> nil then
          MenuItem.Free;
      end
    end;
    GetScripts;
  end;
{$endif}
end;

procedure TMainForm.DisableFavorites;
var
  MenuItem: TMenuItem;
  i: integer;
  s: string;
begin

  (*for i := 0 to Favorites.Count - 1 do
  begin
    s := ExtractFileName(Favorites[i]);
    s := Copy(s, 0, length(s) - Length(ExtractFileExt(s)));
    MenuItem := mnuScript.Find(s);
    if MenuItem <> nil then
      MenuItem.Enabled := False;
  end;          *)
end;

procedure TMainForm.EnableFavorites;
var
  MenuItem: TMenuItem;
  i: integer;
  s: string;
begin
  (*for i := 0 to Favorites.Count - 1 do
  begin
    s := ExtractFileName(Favorites[i]);
    s := Copy(s, 0, length(s) - Length(ExtractFileExt(s)));
    MenuItem := mnuScript.Find(s);
    if MenuItem <> nil then
      MenuItem.Enabled := True;
  end;*)
end;

procedure TMainForm.ShowCanvasSizeWindow;
begin
//  SizeTool.Show;
  AdjustForm.UpdateDisplay;
  AdjustForm.PageControl.TabIndex:=3;
  AdjustForm.Show;
end;

procedure TMainForm.ClipboardWatcherEventsCallback(Sender: TObject);
begin
  if GradientInClipboard then
  begin
//    GradientForm.mnuPaste.enabled := true;
//    GradientForm.btnPaste.enabled := true;
    AdjustForm.mnuPaste.enabled := true;
    AdjustForm.btnPaste.enabled := true;
  end
  else
  begin
//    GradientForm.mnuPaste.enabled := false;
//    GradientForm.btnPaste.enabled := false;
    AdjustForm.mnuPaste.enabled := false;
    AdjustForm.btnPaste.enabled := false;
  end;

  SetCanPaste(FlameInClipboard);
end;

procedure TMainForm.ParseXML(var cp1: TControlPoint; const params: string; const ignoreErrors : boolean);
var
  i: integer; temp: string;
  h, s, v: real;
begin
  CurrentFlame := cp1.name;
  nxform := 0;
  FinalXformLoaded := false;
  ActiveXformSet := 0;
  XMLPaletteFormat := '';
  XMLPaletteCount := 0;
  ParseHandledPluginList := false;
  SurpressHandleMissingPlugins := ignoreErrors;
//  Parsecp.cmapindex := -2; // generate palette from cmapindex and hue (apo 1 and earlier)
//  ParseCp.symmetry := 0;
//  ParseCP.finalXformEnabled := false;
  //ParseCP.Clear;

  ParseCp.Free;                    // we're creating this CP from the scratch
  ParseCp := TControlPoint.create; // to reset variables properly (randomize)

  //LoadCpFromXmlCompatible(params, ParseCP, temp);

  XMLScanner.LoadFromBuffer(TCharType(TStringType(params)));
  XMLScanner.Execute;

  cp1.copy(ParseCp);
  if Parsecp.cmapindex = -2 then
  begin
    if cp1.cmapindex < NRCMAPS then
      GetCMap(cp1.cmapindex, 1, cp1.cmap)
    {else
      ShowMessage('Palette index too high')};

    if (cp1.hue_rotation > 0) and (cp1.hue_rotation < 1) then begin
      for i := 0 to 255 do
      begin
        RGBToHSV(cp1.cmap[i][0], cp1.cmap[i][1], cp1.cmap[i][2], h, s, v);
        h := Round(360 + h + (cp1.hue_rotation * 360)) mod 360;
        HSVToRGB(h, s, v, cp1.cmap[i][0], cp1.cmap[i][1], cp1.cmap[i][2]);
      end;
    end;
  end;

  if FinalXformLoaded = false then begin
    cp1{MainCP}.xform[nxform].Clear;
    cp1{MainCP}.xform[nxform].symmetry := 1;
  end;

  if nxform < NXFORMS then
     for i := nxform to NXFORMS - 1 do
      cp1.xform[i].density := 0;

  // Check for symmetry parameter
  if ParseCp.symmetry <> 0 then
  begin
    add_symmetry_to_control_point(cp1, ParseCp.symmetry);
    cp1.symmetry := 0;
  end;

  cp1.FillUsedPlugins;
  ParseHandledPluginList := false;
  SurpressHandleMissingPlugins := false;
end;

procedure TMainForm.ReplaceSelectedFlameWithClipboard;
begin
  if Clipboard.HasFormat(CF_TEXT) then begin
    UpdateUndo;
{$ifdef DisableScripting}
{$else}
  ScriptEditor.Stopped := True;
{$endif}
    StopThread;
    ParseXML(MainCP, PCHAR(Clipboard.AsText), false);
    NotifyMissingPlugin;
    Statusbar.Panels[3].Text := MainCp.name;
    {if ResizeOnLoad then}
    ResizeImage;
    PreviewRedrawDelayTimer.Enabled := True;
    Application.ProcessMessages;
    UpdateWindows;
  end;
end;

procedure TMainForm.CopySelectedFlameToClipboard;
var
  txt: string;
begin
  txt := Trim(FlameToXML(Maincp, false, false));
  Clipboard.SetTextBuf(PChar(txt));
  SetCanPaste(true);

  AdjustForm.mnuPaste.enabled := False;
  AdjustForm.btnPaste.enabled := False;
end;

function WinShellExecute(const Operation, AssociatedFile: string): Boolean;
var
  a1: string;
  r: Cardinal;
begin
  a1 := Operation;
  if a1 = '' then
    a1 := 'open';

  r := ShellExecute(
    application.handle
    , pchar(a1)
    , pchar(AssociatedFile)
    , ''
    , ''
    , SW_SHOWNORMAL
    );
  if (r > 32) then WinShellExecute := true
  else WinShellExecute := false;
end;

procedure WinShellOpen(const AssociatedFile: string);
begin
  WinShellExecute('open', AssociatedFile);
end;

////////////////////////////////////////////////////////////////////////////////

procedure ParseCompactColors(cp: TControlPoint; count: integer; in_data: string; alpha: boolean = true);
  function HexChar(c: Char): Byte;
  begin
    case c of
      '0'..'9':  Result := Byte(c) - Byte('0');
      'a'..'f':  Result := (Byte(c) - Byte('a')) + 10;
      'A'..'F':  Result := (Byte(c) - Byte('A')) + 10;
    else
      Result := 0;
    end;
  end;
var
  i, pos, len: integer;
  c: char;
  data: string;
begin
  // diable generating pallete
  if Parsecp.cmapindex = -2 then
    Parsecp.cmapindex := -1;

  Assert(Count = 256, 'only 256 color gradients are supported at the moment');
  data := '';
  for i := 1 to Length(in_data) do
  begin
    c := in_data[i];
    if CharInSet(c,['0'..'9']+['A'..'F']+['a'..'f']) then data := data + c;
  end;

  if alpha then len := count * 8
  else len := count * 6;

  Assert(len = Length(data), 'color-data size mismatch');

  for i := 0 to Count-1 do begin
    if alpha then pos := i*8 + 2
    else pos := i*6;
    Parsecp.cmap[i][0] := 16 * HexChar(Data[pos + 1]) + HexChar(Data[pos + 2]);
    Parsecp.cmap[i][1] := 16 * HexChar(Data[pos + 3]) + HexChar(Data[pos + 4]);
    Parsecp.cmap[i][2] := 16 * HexChar(Data[pos + 5]) + HexChar(Data[pos + 6]);
  end;
end;

procedure TMainForm.OnBatchReaderTagEncountered(Sender: TObject;
  TagName: string; Attributes: TAttrList);
begin
  pname := String(Attributes.value(TStringType('name')));
  ptime := String(Attributes.value(TStringType('time')));
end;

procedure TMainForm.OnFlameReaderTagEncountered(Sender: TObject; TagName: string;
  Attributes: TAttrList);
var
  Tokens: TStringList;
  v: TStringType;
  ParsePos, i : integer;
begin
  Tokens := TStringList.Create;
 try

  if TagName='xformset' then // unused in this release...
  begin
    v := Attributes.Value(TStringType('enabled'));
    if v <> '' then ParseCP.finalXformEnabled := (StrToInt(String(v)) <> 0)
    else ParseCP.finalXformEnabled := true;

    inc(activeXformSet);
  end
  else if TagName='flame' then
  begin
    BeginParsing;
    
    v := Attributes.value(TStringType('name'));
    if v <> '' then Parsecp.name := String(v) else Parsecp.name := 'untitled';
    v := Attributes.Value('time');
    if v <> '' then Parsecp.Time := StrToFloat(String(v));
    v := Attributes.value('palette');
    if v <> '' then
      Parsecp.cmapindex := StrToInt(String(v))
    else
      Parsecp.cmapindex := -1;
    v := Attributes.value('gradient');
    if v <> '' then
      Parsecp.cmapindex := StrToInt(String(v))
    else
      Parsecp.cmapindex := -1;
    ParseCP.hue_rotation := 1;

    v := Attributes.value('hue');
    if v <> '' then Parsecp.hue_rotation := StrToFloat(String(v));
    v := Attributes.Value('brightness');
    if v <> '' then Parsecp.Brightness := StrToFloat(String(v));
    v := Attributes.Value('gamma');
    if v <> '' then Parsecp.gamma := StrToFloat(String(v));
    v := Attributes.Value('vibrancy');
    if v <> '' then Parsecp.vibrancy := StrToFloat(String(v));
    if (LimitVibrancy) and (Parsecp.vibrancy > 1) then Parsecp.vibrancy := 1;
    v := Attributes.Value('gamma_threshold');
    if v <> '' then Parsecp.gamma_threshold := StrToFloat(String(v))
    else Parsecp.gamma_threshold := 0;

    v := Attributes.Value('zoom');
    if v <> '' then Parsecp.zoom := StrToFloat(String(v));
    v := Attributes.Value('scale');
    if v <> '' then Parsecp.pixels_per_unit := StrToFloat(String(v));
    v := Attributes.Value('rotate');
    if v <> '' then Parsecp.FAngle := -PI * StrToFloat(String(v))/180;
    v := Attributes.Value('angle');
    if v <> '' then Parsecp.FAngle := StrToFloat(String(v));

    // 3d
    v := Attributes.Value('cam_pitch');
    if v <> '' then Parsecp.cameraPitch := StrToFloat(String(v));
    v := Attributes.Value('cam_yaw');
    if v <> '' then Parsecp.cameraYaw := StrToFloat(String(v));
    v := Attributes.Value('cam_dist');
    if v <> '' then Parsecp.cameraPersp := 1/StrToFloat(String(v));
    v := Attributes.Value('cam_perspective');
    if v <> '' then Parsecp.cameraPersp := StrToFloat(String(v));
    v := Attributes.Value('cam_zpos');
    if v <> '' then Parsecp.cameraZpos := StrToFloat(String(v));
    v := Attributes.Value('cam_dof');
    if v <> '' then Parsecp.cameraDOF := abs(StrToFloat(String(v)));

    //density estimation
    v := Attributes.Value('estimator_radius');
    if v <> '' then Parsecp.estimator := StrToFloat(String(v));
    v := Attributes.Value('estimator_minimum');
    if v <> '' then Parsecp.estimator_min := StrToFloat(String(v));
    v := Attributes.Value('estimator_curve');
    if v <> '' then Parsecp.estimator_curve := StrToFloat(String(v));
    v := Attributes.Value('enable_de');
    if (v = '1') then Parsecp.enable_de := true;

    v := Attributes.Value('new_linear');
    if (v = '1') then Parsecp.noLinearFix := true
    else ParseCp.noLinearFix := false;

    try
      v := Attributes.Value('center');
      GetTokens(String(v), tokens);

      Parsecp.center[0] := StrToFloat(Tokens[0]);
      Parsecp.center[1] := StrToFloat(Tokens[1]);
    except
      Parsecp.center[0] := 0;
      Parsecp.center[1] := 0;
    end;

    v := Attributes.Value('size');
    GetTokens(String(v), tokens);

    Parsecp.width := StrToInt(Tokens[0]);
    Parsecp.height := StrToInt(Tokens[1]);

    try
      v := Attributes.Value('background');
      GetTokens(String(v), tokens);

      Parsecp.background[0] := Floor(StrToFloat(Tokens[0]) * 255);
      Parsecp.background[1] := Floor(StrToFloat(Tokens[1]) * 255);
      Parsecp.background[2] := Floor(StrToFloat(Tokens[2]) * 255);
    except
      Parsecp.background[0] := 0;
      Parsecp.background[1] := 0;
      Parsecp.background[2] := 0;
    end;

    v := Attributes.Value('soloxform');
    if v <> '' then Parsecp.soloXform := StrToInt(String(v));

    v := Attributes.Value('plugins');
    GetTokens(String(v), tokens);
    if (tokens.Count > 0) then begin
      ParseCP.used_plugins.Clear;

      for i := 0 to tokens.Count - 1 do
        ParseCP.used_plugins.Add(tokens[i]);
    end;
  end
  else if TagName='palette' then
  begin
    XMLPaletteFormat := String(Attributes.Value('format'));
    XMLPaletteCount := StrToIntDef(String(Attributes.Value('count')), 256);
  end;
 finally
    Tokens.free;
 end;
end;

function flatten_val(Attributes: TAttrList): double;
var
  vv: array of double;
  vn: array of string;
  i: integer;
  s: string;
  d: boolean;
begin

  SetLength(vv, 24);
  SetLength(vn, 24);

  d := false;

  vn[0] := 'linear3D'; vn[1] := 'bubble';
  vn[2] := 'cylinder'; vn[3] := 'zblur';
  vn[4] := 'blur3D'; vn[5] := 'pre_ztranslate';
  vn[6] := 'pre_rotate_x'; vn[7] := 'pre_rotate_y';
  vn[8] := 'ztranslate'; vn[9] := 'zcone';
  vn[10] := 'post_rotate_x'; vn[11] := 'post_rotate_y';
  vn[12] := 'julia3D'; vn[13] := 'julia3Dz';
  vn[14] := 'curl3D_cz'; vn[15] := 'hemisphere';
  vn[16] := 'bwraps2'; vn[17] := 'bwraps';
  vn[18] := 'falloff2'; vn[19] := 'crop';
  vn[20] := 'pre_falloff2'; vn[21] := 'pre_crop';
  vn[22] := 'post_falloff2'; vn[23] := 'post_crop';


  for i := 0 to 23 do
  begin
    s := String(Attributes.Value(TStringType(vn[i])));
    if (s <> '') then vv[i] := StrToFloat(s)
    else vv[i] := 0;
    d := d or (vv[i] <> 0);
  end;

  if (d) then Result := 0
  else Result := 1;

  SetLength(vv, 0);
  SetLength(vn, 0);
end;
function linear_val(Attributes: TAttrList): double;
var
  vv: array of double;
  vn: array of string;
  i: integer;
  s: string;
begin
  SetLength(vv, 2);
  SetLength(vn, 2);

  Result := 0;

  vn[0] := 'linear3D';
  vn[1] := 'linear';
  for i := 0 to 1 do
  begin
    s := String(Attributes.Value(TStringType(vn[i])));
    if (s <> '') then vv[i] := StrToFloat(s)
    else vv[i] := 0;
    Result := Result + vv[i];
  end;

  SetLength(vv, 0);
  SetLength(vn, 0);
end;

procedure TMainForm.OnFlameReaderDataEncountered(Sender: TObject; Content: String);
begin
  if XMLPaletteCount <= 0 then begin
    //ShowMessage('ERROR: No colors in palette!');
    Application.MessageBox(PChar(TextByKey('common-invalidformat')), 'Apophysis', MB_ICONERROR);
    exit;
  end;
  if XMLPaletteFormat = 'RGB' then
  begin
    ParseCompactColors(ParseCP, XMLPaletteCount, Content, false);
  end
  else if XMLPaletteFormat = 'RGBA' then
  begin
    ParseCompactColors(ParseCP, XMLPaletteCount, Content);
  end
  else begin
    Application.MessageBox(PChar(TextByKey('common-invalidformat')), 'Apophysis', MB_ICONERROR);
    exit;
  end;
  Parsecp.cmapindex := -1;

  XMLPaletteFormat := '';
  XMLPaletteCount := 0;
end;

procedure TMainForm.OnFlameReaderEmptyTagEncountered(Sender: TObject; TagName: string;
  Attributes: TAttrList);
var
  i: integer;
  v, l, l3d: TStringType;
  d, floatcolor, vl, vl3d: double;
  Tokens: TStringList;
begin

  Tokens := TStringList.Create;
  try
    if (TagName = 'xform') or (TagName = 'finalxform') then
     if {(TagName = 'finalxform') and} (FinalXformLoaded) then Application.MessageBox(PChar(TextByKey('common-invalidformat')), 'Apophysis', MB_ICONERROR)
     else
    begin
      for i := 0 to Attributes.Count - 1 do begin
        if not ScanVariations(String(attributes.Name(i))) and
           not ScanVariables(String(attributes.Name(i))) then
           CheckAttribute(String(Attributes.Name(i)));
      end;
      if (TagName = 'finalxform') or (activeXformSet > 0) then FinalXformLoaded := true;

     with ParseCP.xform[nXform] do begin
      Clear;
      v := Attributes.Value('weight');
      if (v <> '') and (TagName = 'xform') then density := StrToFloat(String(v));
      if (TagName = 'finalxform') then
      begin
        v := Attributes.Value('enabled');
        if v <> '' then ParseCP.finalXformEnabled := (StrToInt(String(v)) <> 0)
        else ParseCP.finalXformEnabled := true;
      end;

      if activexformset > 0 then density := 0; // tmp...

      v := Attributes.Value('color');
      if v <> '' then color := StrToFloat(String(v));
      v := Attributes.Value('var_color');
      if v <> '' then pluginColor := StrToFloat(String(v));
      v := Attributes.Value('symmetry');
      if v <> '' then symmetry := StrToFloat(String(v));
      v := Attributes.Value('coefs');
      GetTokens(String(v), tokens);
      if Tokens.Count < 6 then Application.MessageBox(PChar(TextByKey('common-invalidformat')), 'Apophysis', MB_ICONERROR);
      c[0][0] := StrToFloat(Tokens[0]);
      c[0][1] := StrToFloat(Tokens[1]);
      c[1][0] := StrToFloat(Tokens[2]);
      c[1][1] := StrToFloat(Tokens[3]);
      c[2][0] := StrToFloat(Tokens[4]);
      c[2][1] := StrToFloat(Tokens[5]);

      v := Attributes.Value('post');
      if v <> '' then begin
        GetTokens(String(v), tokens);
        if Tokens.Count < 6 then Application.MessageBox(PChar(TextByKey('common-invalidformat')), 'Apophysis', MB_ICONERROR);
        p[0][0] := StrToFloat(Tokens[0]);
        p[0][1] := StrToFloat(Tokens[1]);
        p[1][0] := StrToFloat(Tokens[2]);
        p[1][1] := StrToFloat(Tokens[3]);
        p[2][0] := StrToFloat(Tokens[4]);
        p[2][1] := StrToFloat(Tokens[5]);
      end;

      v := Attributes.Value('chaos');
      if v <> '' then begin
        GetTokens(String(v), tokens);
        for i := 0 to Tokens.Count-1 do
          modWeights[i] := Abs(StrToFloat(Tokens[i]));
      end;
      //else for i := 0 to NXFORMS-1 do modWeights[i] := 1;

      // for 2.09 flames compatibility
      v := Attributes.Value('opacity');
      if v <> '' then begin
        if StrToFloat(String(v)) = 0.0 then begin
          transOpacity := 0;
        end else begin
          transOpacity := StrToFloat(String(v));
        end;
      end;

      // 7x.9 name tag
      v := Attributes.Value('name');
      if v <> '' then begin
        TransformName := String(v);
      end;

      v := Attributes.Value('plotmode');
      if v <> '' then begin
        if v = 'off' then begin
          transOpacity := 0;
        end;
      end;

      {$ifndef Pre15c}
      // tricky: attempt to convert parameters to 15C+-format if necessary
      if (ParseCp.noLinearFix) then
        for i := 0 to 1 do
        begin
          SetVariation(i, 0);
          v := TStringType(ReadWithSubst(Attributes, varnames(i)));
          //v := Attributes.Value(AnsiString(varnames(i)));
          if v <> '' then
              SetVariation(i, StrToFloat(String(v)));
        end
      else begin
        SetVariation(0, linear_val(Attributes));
        SetVariation(1, flatten_val(Attributes));
      end;
      {$endif}

      // now parse the rest of the variations...as usual
      for i := {$ifndef Pre15c}2{$else}0{$endif} to NRVAR - 1 do
      begin
        SetVariation(i, 0);
        v := TStringType(ReadWithSubst(Attributes, varnames(i)));
        //v := Attributes.Value(AnsiString(varnames(i)));
        if v <> '' then
            SetVariation(i, StrToFloat(String(v)));
      end;

      // and the variables
      for i := 0 to GetNrVariableNames - 1 do begin
        v := TStringType(ReadWithSubst(Attributes, GetVariableNameAt(i)));
        //v := Attributes.Value(AnsiString(GetVariableNameAt(i)));
        if v <> '' then begin
          {$ifndef VAR_STR}
          d := StrToFloat(String(v));
          SetVariable(GetVariableNameAt(i), d);
          {$else}
          SetVariableStr(GetVariableNameAt(i), String(v));
          {$endif}
        end;
      end;

      // legacy variation/variable notation
      v := Attributes.Value('var1');
      if v <> '' then
      begin
        for i := 0 to NRVAR - 1 do
          SetVariation(i, 0);
        SetVariation(StrToInt(String(v)), 1);
      end;
      v := Attributes.Value('var');
      if v <> '' then
      begin
        for i := 0 to NRVAR - 1 do
          SetVariation(i, 0);
        GetTokens(String(v), tokens);
        if Tokens.Count > NRVAR then Application.MessageBox(PChar(TextByKey('common-invalidformat')), 'Apophysis', MB_ICONERROR);
        for i := 0 to Tokens.Count - 1 do
          SetVariation(i, StrToFloat(Tokens[i]));
      end;
     end;
      Inc(nXform);
    end;
    if TagName = 'color' then
    begin
      // disable generating palette
      //if Parsecp.cmapindex = -2 then
        Parsecp.cmapindex := -1;

      i := StrToInt(String(Attributes.value('index')));
      v := Attributes.value('rgb');
      GetTokens(String(v), tokens);
      floatcolor := StrToFloat(Tokens[0]);
      Parsecp.cmap[i][0] := round(floatcolor);
      floatcolor := StrToFloat(Tokens[1]);
      Parsecp.cmap[i][1] := round(floatcolor);
      floatcolor := StrToFloat(Tokens[2]);
      Parsecp.cmap[i][2] := round(floatcolor);
    end;
    if TagName = 'colors' then
    begin
      ParseCompactcolors(Parsecp, StrToInt(String(Attributes.value('count'))),
        String(Attributes.value('data')));
      Parsecp.cmapindex := -1;
    end;
    if TagName = 'symmetry' then
    begin
      i := StrToInt(String(Attributes.value('kind')));
      Parsecp.symmetry := i;
    end;
    if TagName = 'xdata' then
    begin
      Parsecp.xdata := Parsecp.xdata + String(Attributes.value('content'));
    end;
  finally
    Tokens.free;
  end;
end;

procedure TMainForm.OpenFractalFlamePublication;
begin
  WinShellOpen('http://media.xyrus-worx.org/fractal-flame-publication');
end;

///////////////////////////////////////////////////////////////////////////////
procedure TMainForm.OnPreviewImageMouseDown(Sender: TObject; Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
  if button = mbMiddle then begin
    //FMouseMoveState := msHeight;
    exit;
  end else if button = mbRight then begin
  //FMouseMoveState := msPitchYaw;
    camDragValueY := MainCP.cameraPitch * 180.0 / PI;
    camDragValueX := MainCP.cameraYaw * 180.0 / PI;

     camDragMode := true;
     camDragPos.x := 0;
     camDragPos.y := 0;
     camDragOld.x := x;
     camDragOld.y := y;
     camMM := false;
     //SetCaptureControl(TControl(Sender));

    //Screen.Cursor := crNone;
    //GetCursorPos(mousepos); // hmmm
    //mousePos := (Sender as TControl).ClientToScreen(Point(x, y));
    camDragged := false;
    exit;
  end;
  //if button <> mbLeft then exit;
  CursorStart.TopLeft := Point(x, y);
  CursorStart.BottomRight := CursorStart.TopLeft;
  case CurrentCursorMode of
    msZoomWindow:
      begin
        CursorCurrent.TopLeft := Point(x, y);
        CursorCurrent.BottomRight := Point(x, y);
        DrawZoomWindow;

//        if ssAlt in Shift then
//          FMouseMoveState := msZoomOutWindowMove
//        else
          CurrentCursorMode := msZoomWindowMove;
      end;
    msZoomOutWindow:
      begin
        CursorCurrent.TopLeft := Point(x, y);
        CursorCurrent.BottomRight := Point(x, y);
        DrawZoomWindow;

//        if ssAlt in Shift then
//          FMouseMoveState := msZoomWindowMove
//        else
          CurrentCursorMode := msZoomOutWindowMove;
      end;
    msDrag:
      begin
        if not assigned(TransparentPreviewImage) then exit;

//        FSelectRect.TopLeft := Point(x, y);
//        FSelectRect.BottomRight := Point(x, y);
        CurrentCursorMode := msDragMove;
      end;
    msRotate:
      begin
        CursorStartAngle := arctan2(y-PreviewImage.Height/2, PreviewImage.Width/2-x);

        CursorCurrentAngle := 0;
//        FSelectRect.Left := x;
        DrawRotateLines(CursorCurrentAngle);
        CurrentCursorMode := msRotateMove;
      end;
  end;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TMainForm.OnPreviewImageMouseMove(Sender: TObject; Shift: TShiftState; X, Y: Integer);
const
  snap_angle = 15*pi/180;
var
  dx, dy, cx, cy, sgn: integer;
  sc, vx, vy, scale: double;
  q : Extended;
begin
{
  case FMouseMoveState of
    msRotate, msRotateMove:
      Image.Cursor := crEditRotate;
    msDrag, msDragMove:
      Image.Cursor := crEditMove;
    else
      Image.Cursor := crEditArrow;
  end;
}
  case CurrentCursorMode of
    msZoomWindowMove,
    msZoomOutWindowMove:
      begin
        if IsDrawingSelection then DrawZoomWindow;
        CursorStart.BottomRight := Point(x, y);
        dx := x - CursorStart.TopLeft.X;
        dy := y - CursorStart.TopLeft.Y;

        if ssShift in Shift then begin
          if (dy = 0) or (abs(dx/dy) >= PreviewImage.Width/PreviewImage.Height) then
            dy := Round(dx / PreviewImage.Width * PreviewImage.Height)
          else
            dx := Round(dy / PreviewImage.Height * PreviewImage.Width);
          CursorCurrent.Left := CursorStart.TopLeft.X - dx;
          CursorCurrent.Top := CursorStart.TopLeft.Y - dy;
          CursorCurrent.Right := CursorStart.TopLeft.X + dx;
          CursorCurrent.Bottom := CursorStart.TopLeft.Y + dy;
        end
        else if ssCtrl in Shift then begin
          CursorCurrent.TopLeft := CursorStart.TopLeft;
          sgn := IfThen(dy*dx >=0, 1, -1);
          if (dy = 0) or (abs(dx/dy) >= PreviewImage.Width/PreviewImage.Height) then begin
            CursorCurrent.Right := x;
            CursorCurrent.Bottom := CursorStart.TopLeft.Y + sgn * Round(dx / PreviewImage.Width * PreviewImage.Height);
          end
          else begin
            CursorCurrent.Right := CursorStart.TopLeft.X + sgn * Round(dy / PreviewImage.Height * PreviewImage.Width);
            CursorCurrent.Bottom := y;
          end;
        end
        else begin
          sgn := IfThen(dy*dx >=0, 1, -1);
          if (dy = 0) or (abs(dx/dy) >= PreviewImage.Width/PreviewImage.Height) then begin
            cy := (y + CursorStart.TopLeft.Y) div 2;
            CursorCurrent.Left := CursorStart.TopLeft.X;
            CursorCurrent.Right := x;
            CursorCurrent.Top := cy - sgn * Round(dx / 2 / PreviewImage.Width * PreviewImage.Height);
            CursorCurrent.Bottom := cy + sgn * Round(dx / 2 / PreviewImage.Width * PreviewImage.Height);
          end
          else begin
            cx := (x + CursorStart.TopLeft.X) div 2;
            CursorCurrent.Left := cx - sgn * Round(dy / 2 / PreviewImage.Height * PreviewImage.Width);
            CursorCurrent.Right := cx + sgn * Round(dy / 2 / PreviewImage.Height * PreviewImage.Width);
            CursorCurrent.Top := CursorStart.TopLeft.Y;
            CursorCurrent.Bottom := y;
          end;
        end;
        DrawZoomWindow;
        IsDrawingSelection := true;
      end;
    msDragMove:
      begin
        assert(assigned(TransparentPreviewImage));
        assert(FViewScale <> 0);

        scale := FViewScale * PreviewImage.Width / TransparentPreviewImage.Width;
        FViewPos.X := FViewPos.X + (x - CursorStart.Right) / scale;
        FViewPos.Y := FViewPos.Y + (y - CursorStart.Bottom) / scale;
        //FClickRect.BottomRight := Point(x, y);

		    DrawImageView;
      end;
    msPitchYaw:
      begin
          if camDragMode and ( (x <> camDragOld.x) or (y <> camDragOld.y) ) then
          begin
            Inc(camDragPos.x, x - camDragOld.x);
            Inc(camDragPos.y, y - camDragOld.y);

            vx := Round6(camDragValueX + camDragPos.x / 10);
            vy := Round6(camDragValueY - camDragPos.y / 10);

            MainCP.cameraPitch := vy * PI / 180.0;
            MainCP.cameraYaw := vx * PI / 180.0;

            vx := Round(vx);
            vy := Round(vy);

            camDragged := True;
            //StatusBar.Panels.Items[1].Text := Format('Pitch: %f°, Yaw: %f°', [vx,vy]);
          end;
      end;
    msRotateMove:
      begin
        if IsDrawingSelection then DrawRotatelines(CursorCurrentAngle);

        CursorCurrentAngle := arctan2(y-PreviewImage.Height/2, PreviewImage.Width/2-x) - CursorStartAngle;
        if ssShift in Shift then // angle snap
          CursorCurrentAngle := Round(CursorCurrentAngle/snap_angle)*snap_angle;
        //SelectRect.Left := x;

//        pdjpointgen.Rotate(FRotateAngle);
//        FRotateAngle := 0;

        DrawRotatelines(CursorCurrentAngle);
        IsDrawingSelection := true;
{
        Image.Refresh;
if AdjustForm.Visible then begin
MainCp.FAngle:=-FRotateAngle;
AdjustForm.UpdateDisplay;
end;
}
      end;
  end;
  CursorStart.BottomRight := Point(x, y);
end;

function ScaleRect(r: TRect; scale: double): TSRect;
begin
  Result.Left := r.Left * scale;
  Result.Top := r.Top * scale;
  Result.Right := r.Right * scale;
  Result.Bottom := r.Bottom * scale;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TMainForm.OnPreviewImageMouseUp(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
var
  scale: double;
  rs: TSRect;
begin
  case CurrentCursorMode of
    msZoomWindowMove:
      begin
        DrawZoomWindow;
        CurrentCursorMode := msZoomWindow;
        if (abs(CursorCurrent.Left - CursorCurrent.Right) < 10) or
           (abs(CursorCurrent.Top - CursorCurrent.Bottom) < 10) then
          Exit; // zoom to much or double clicked

        StopThread;
        UpdateUndo;
        MainCp.ZoomtoRect(ScaleRect(CursorCurrent, MainCP.Width / PreviewImage.Width));

        FViewScale := FViewScale * PreviewImage.Width / abs(CursorCurrent.Right - CursorCurrent.Left);
        FViewPos.x := FViewPos.x - ((CursorCurrent.Right + CursorCurrent.Left) - PreviewImage.Width)/2;
        FViewPos.y := FViewPos.y - ((CursorCurrent.Bottom + CursorCurrent.Top) - PreviewImage.Height)/2;
        DrawImageView;

        PreviewRedrawDelayTimer.Enabled := True;
        UpdateWindows;
      end;
    msZoomOutWindowMove:
      begin
        DrawZoomWindow;
        CurrentCursorMode := msZoomOutWindow;
        if (abs(CursorCurrent.Left - CursorCurrent.Right) < 10) or
           (abs(CursorCurrent.Top - CursorCurrent.Bottom) < 10) then
          Exit; // zoom to much or double clicked

        StopThread;
        UpdateUndo;
        MainCp.ZoomOuttoRect(ScaleRect(CursorCurrent, MainCP.Width / PreviewImage.Width));

        scale := PreviewImage.Width / abs(CursorCurrent.Right - CursorCurrent.Left);
        FViewScale := FViewScale / scale;
        FViewPos.x := scale * (FViewPos.x + ((CursorCurrent.Right + CursorCurrent.Left) - PreviewImage.Width)/2);
        FViewPos.y := scale * (FViewPos.y + ((CursorCurrent.Bottom + CursorCurrent.Top) - PreviewImage.Height)/2);

        DrawImageView;

        PreviewRedrawDelayTimer.Enabled := True;
        UpdateWindows;
      end;
    msDragMove:
      begin
        CursorStart.BottomRight := Point(x, y);
        CurrentCursorMode := msDrag;

        if ((x = 0) and (y = 0)) or // double clicked
           ((CursorStart.left = CursorStart.right) and (CursorStart.top = CursorStart.bottom))
          then Exit;

        StopThread;
        UpdateUndo;
        MainCp.MoveRect(ScaleRect(CursorStart, MainCP.Width / PreviewImage.Width));

        PreviewRedrawDelayTimer.Enabled := True;
        UpdateWindows;
      end;
    msRotateMove:
      begin
        DrawRotatelines(CursorCurrentAngle);

        CurrentCursorMode := msRotate;

        if (CursorCurrentAngle = 0) then Exit; // double clicked

        StopThread;
        UpdateUndo;
        if MainForm_RotationMode = 0 then MainCp.Rotate(CursorCurrentAngle)
        else MainCp.Rotate(-CursorCurrentAngle);

        if assigned(TransparentPreviewImage) then begin
          TransparentPreviewImage.Free;
          TransparentPreviewImage := nil;
          DrawImageView;
        end;

        PreviewRedrawDelayTimer.Enabled := True;
        UpdateWindows;
      end;
    msPitchYaw:
      begin
        camDragMode := false;
        Screen.Cursor := crDefault;

        if camDragged then
        begin
          camDragged := False;
          PreviewRedrawDelayTimer.Enabled := True;
          UpdateWindows;
        end;


      end;
  end;
end;

procedure TMainForm.OnPreviewPanelMenuResetLocationClick(Sender: TObject);
begin
  ResetSelectedFlameCamera(TUiCommandAction.DefaultArgs);
end;

///////////////////////////////////////////////////////////////////////////////
procedure TMainForm.DrawImageView;
var
  i, j: integer;
  bm: TBitmap;
  r: TRect;
  scale: double;
const
  msg = #54; // 'NO PREVIEW';
var
  ok: boolean;
  GlobalMemoryInfo: TMemoryStatus; // holds the global memory status information
  area: int64;
  gridp: integer;
begin
  if mainCp = nil then
    Exit;

  bm := TBitmap.Create;
  bm.Width := PreviewImage.Width;
  bm.Height := PreviewImage.Height;
  with bm.Canvas do begin
    if ShowTransparency then begin
      Brush.Color := $F0F0F0;
      FillRect(Rect(0, 0, bm.Width, bm.Height));
      Brush.Color := $C0C0C0;
      for i := 0 to ((bm.Width - 1) shr 3) do begin
        for j := 0 to ((bm.Height - 1) shr 3) do begin
          if odd(i + j) then
            FillRect(Rect(i shl 3, j shl 3, (i+1) shl 3, (j+1) shl 3));
        end;
      end;
    end
    else begin
      Brush.Color := MainCP.background[0] or (MainCP.background[1] shl 8) or (MainCP.background[2] shl 16);
      FillRect(Rect(0, 0, bm.Width, bm.Height));
    end;
  end;

  ok := false;
  if assigned(TransparentPreviewImage) then begin
    scale := FViewScale * PreviewImage.Width / TransparentPreviewImage.Width;

    r.Left := PreviewImage.Width div 2 + round(scale * (FViewPos.X - TransparentPreviewImage.Width/2));
    r.Right := PreviewImage.Width div 2 + round(scale * (FViewPos.X + TransparentPreviewImage.Width/2));
    r.Top := PreviewImage.Height div 2 + round(scale * (FViewPos.Y - TransparentPreviewImage.Height/2));
    r.Bottom := PreviewImage.Height div 2 + round(scale * (FViewPos.Y + TransparentPreviewImage.Height/2));

    GlobalMemoryInfo.dwLength := SizeOf(GlobalMemoryInfo);
    GlobalMemoryStatus(GlobalMemoryInfo);
    area := abs(r.Right - r.Left) * int64(abs(r.Bottom - r.Top));

    if (area * 4 < GlobalMemoryInfo.dwAvailPhys div 2) or
      (area <= Screen.Width*Screen.Height*4) then
    try
      TransparentPreviewImage.Draw(bm.Canvas, r);
      ok := true;
    except
    end;

    // Gridlines for composition (taken from JK mod by Jed Kelsey)
    if (EnableGuides) then begin
    with bm.Canvas do begin
      Pen.Width := 1;
      Pen.Color := TColor(LineCenterColor); //$000000; // Center
      MoveTo(0, bm.Height shr 1); LineTo(bm.Width, bm.Height shr 1);
      MoveTo(bm.Width shr 1, 0); LineTo(bm.Width shr 1, bm.Height);
      Pen.Color := TColor(LineThirdsColor); //$C000C0; // Thirds
      gridp := Floor(bm.Height/3);
      MoveTo(0, gridp); LineTo(bm.Width, gridp);
      MoveTo(0, bm.Height-gridp); LineTo(bm.Width, bm.Height-gridp);
      gridp := Floor(bm.Width/3);
      MoveTo(gridp, 0); LineTo(gridp, bm.Height);
      MoveTo(bm.Width-gridp, 0); LineTo(bm.Width-gridp, bm.Height);
      Pen.Color := TColor(LineGRColor); //$0000F0; // "Golden Ratio" (per axis)
      gridp := Floor(bm.Height * 0.61803399);
      MoveTo(0, gridp); LineTo(bm.Width, gridp);
      MoveTo(0, bm.Height-gridp); LineTo(bm.Width, bm.Height-gridp);
      gridp := Floor(bm.Width * 0.61803399);
      MoveTo(gridp, 0); LineTo(gridp, bm.Height);
      MoveTo(bm.Width-gridp, 0); LineTo(bm.Width-gridp, bm.Height);
    end;
  end;
  end;

  if not ok then
    with bm.Canvas do
    begin
      Font.Name := 'Wingdings'; // 'Arial';
      Font.Height := bm.Height div 4;
      Font.Color := $808080;
      Brush.Style := bsClear;
      i := (bm.Width - TextWidth(msg)) div 2;
      j := (bm.Height - TextHeight(msg)) div 2;
      Font.Color := 0;
      TextOut(i+2,j+2, msg);
      Font.Color := clWhite; //$808080;
      TextOut(i,j, msg);
    end;
  PreviewImage.Picture.Graphic := bm;
  //EditForm.PaintBackground;
  PreviewImage.Refresh;
  bm.Free;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TMainForm.DrawPitchYawLines(YawAngle: double; PitchAngle: double);
var
  bkuPen: TPen;
  points: array[0..3] of TPoint;
  i: integer;
begin
  bkuPen := TPen.Create;
  bkuPen.Assign(PreviewImage.Canvas.Pen);
  PreviewImage.Canvas.Pen.Mode    := pmXor;
  PreviewImage.Canvas.Pen.Color   := clWhite;
  PreviewImage.Canvas.Pen.Style   := psDot; //psDash;
  PreviewImage.Canvas.Brush.Style := bsClear;

//  Image.Canvas.Rectangle(FSelectRect);
  points[0].x := 0;
  points[0].y := round((PreviewImage.Height / 2) * sin(PitchAngle));
  points[1].x := PreviewImage.Width - 1;
  points[1].y := points[0].y;
  points[2].x := points[1].x;
  points[2].y := round((PreviewImage.Height) - ((PreviewImage.Height / 2) * sin(PitchAngle)));
  points[3].x := points[0].x;
  points[3].y := points[2].y;

  PreviewImage.Canvas.MoveTo(Points[3].x, Points[3].y);
  for i := 0 to 3 do begin
    PreviewImage.Canvas.LineTo(Points[i].x, Points[i].y);
  end;

  PreviewImage.Canvas.Pen.Assign(bkuPen);
  bkuPen.Free;
end;

procedure TMainForm.DrawRotateLines(Angle: double);
var
  bkuPen: TPen;
  points: array[0..3] of TPoint;
  i,x,y: integer;
begin
  bkuPen := TPen.Create;
  bkuPen.Assign(PreviewImage.Canvas.Pen);
  PreviewImage.Canvas.Pen.Mode    := pmXor;
  PreviewImage.Canvas.Pen.Color   := clWhite;
  PreviewImage.Canvas.Pen.Style   := psDot; //psDash;
  PreviewImage.Canvas.Brush.Style := bsClear;

//  Image.Canvas.Rectangle(FSelectRect);
  points[0].x := (PreviewImage.Width div 2)-1;
  points[0].y := (PreviewImage.Height div 2)-1;
  points[1].x := (PreviewImage.Width div 2)-1;
  points[1].y := -PreviewImage.Height div 2;
  points[2].x := -PreviewImage.Width div 2;
  points[2].y := -PreviewImage.Height div 2;
  points[3].x := -PreviewImage.Width div 2;
  points[3].y := (PreviewImage.Height div 2)-1;

  for i := 0 to 3 do begin
    x := points[i].x;
    y := points[i].y;

    points[i].x := round(cos(Angle) * x + sin(Angle) * y) + PreviewImage.Width div 2;
    points[i].y := round(-sin(Angle) * x + cos(Angle) * y) + PreviewImage.Height div 2;
  end;

  PreviewImage.Canvas.MoveTo(Points[3].x, Points[3].y);
  for i := 0 to 3 do begin
    PreviewImage.Canvas.LineTo(Points[i].x, Points[i].y);
  end;

  PreviewImage.Canvas.Pen.Assign(bkuPen);
  bkuPen.Free;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TMainForm.DrawZoomWindow;
const
  cornerSize = 32;
var
  bkuPen: TPen;
  dx, dy, cx, cy: integer;
  l, r, t, b: integer;
begin
  bkuPen := TPen.Create;
  bkuPen.Assign(PreviewImage.Canvas.Pen);
  with PreviewImage.Canvas do begin
    Pen.Mode    := pmXor;
    Pen.Color   := clWhite;
    Brush.Style := bsClear;

    Pen.Style   := psDot; //psDash;

    if ssShift in FShiftState then
    begin
      dx := CursorStart.Right - CursorStart.Left;
      dy := CursorStart.Bottom - CursorStart.Top;
      Rectangle(CursorStart.Left - dx, CursorStart.Top - dy, CursorStart.Right, CursorStart.Bottom);
    end
    else Rectangle(CursorStart);

    dx := CursorCurrent.Right - CursorCurrent.Left;
    if dx >= 0 then begin
      l := CursorCurrent.Left - 1;
      r := CursorCurrent.Right;
    end
    else begin
      dx := -dx;
      l := CursorCurrent.Right - 1;
      r := CursorCurrent.Left;
    end;
    dx := min(dx div 2 - 1, cornerSize);

    dy := CursorCurrent.Bottom - CursorCurrent.Top;
    if dy >= 0 then begin
      t := CursorCurrent.Top - 1;
      b := CursorCurrent.Bottom;
    end
    else begin
      dy := -dy;
      t := CursorCurrent.Bottom - 1;
      b := CursorCurrent.Top;
    end;
    dy := min(dy div 2, cornerSize);

    pen.Style := psSolid;

    MoveTo(l + dx, t);
    LineTo(l, t);
    LineTo(l, t + dy);
    MoveTo(r - dx, t);
    LineTo(r, t);
    LineTo(r, t + dy);
    MoveTo(r - dx, b);
    LineTo(r, b);
    LineTo(r, b - dy);
    MoveTo(l + dx, b);
    LineTo(l, b);
    LineTo(l, b - dy);
{
    cx := (l + r) div 2;
    cy := (t + b) div 2;
    MoveTo(cx - dx div 2, cy);
    LineTo(cx + dx div 2 + 1, cy);
    MoveTo(cx, cy - dy div 2);
    LineTo(cx, cy + dy div 2 + 1);
}
    Pen.Assign(bkuPen);
  end;
  bkuPen.Free;
end;

procedure TMainForm.SetCursorMode(const args : TUiCommandBooleanEventArgs);
begin
  if not assigned(args.Command) then exit;
  
  case args.Command.CommandId of
    PanModeCommand_Id: SetCursorModePan;  
    RotateModeCommand_Id: SetCursorModeRotate;
    ZoomInCommand_Id: SetCursorModeZoomIn;
    ZoomOutCommand_Id: SetCursorModeZoomOut;
  end;
end;

procedure TMainForm.UpdateCursorModeCommandStates;
begin
  if Assigned(PanModeCommand) then PanModeCommand.Checked := CurrentCursorMode=msDrag;
  if Assigned(RotateModeCommand) then RotateModeCommand.Checked := CurrentCursorMode=msRotate; 
  if Assigned(ZoomInModeCommand) then ZoomInModeCommand.Checked := CurrentCursorMode=msZoomWindow;
  if Assigned(ZoomOutModeCommand) then ZoomOutModeCommand.Checked := CurrentCursorMode=msZoomOutWindow;
end;

procedure TMainForm.SetCursorModePan;
begin
  CurrentCursorMode := msDrag;
  UpdateCursorModeCommandStates;
end;

procedure TMainForm.SetCursorModeRotate;
begin
  CurrentCursorMode := msRotate;
  UpdateCursorModeCommandStates;
end;

procedure TMainForm.SetCursorModeZoomIn;
begin
  CurrentCursorMode := msZoomWindow;
  UpdateCursorModeCommandStates;
end;

procedure TMainForm.SetCursorModeZoomOut;
begin
  CurrentCursorMode := msZoomOutWindow;
  UpdateCursorModeCommandStates;
end;

procedure TMainForm.SetCanUndo(value: boolean);
begin
  if Assigned(UndoCommand) then
    UndoCommand.Enabled := value;
end;

procedure TMainForm.SetCanRedo(value: boolean);
begin
  if Assigned(RedoCommand) then
    RedoCommand.Enabled := value;
end;

procedure TMainForm.SetCanPaste(value: boolean);
begin
  if Assigned(PasteCommand) then
    PasteCommand.Enabled := value;
end;

procedure TMainForm.SetPreviewDensity(const Args: TUiCommandActionEventArgs);
var value: double;
begin
  if not Assigned(args.Command) then exit;

  case args.Command.CommandId of
    ViewDensity5: value := 5.0;
    ViewDensity10: value := 10.0;
    ViewDensity15: value := 15.0;
    ViewDensity25: value := 25.0;
    ViewDensity50: value := 50.0;
    ViewDensity100: value := 100.0;
    ViewDensity150: value := 150.0;
    ViewDensity250: value := 250.0;
    ViewDensity500: value := 500.0;
    ViewDensity1000: value := 1000.0;
  end;
  
  defSampleDensity := value;
  StopThread;
  PreviewRedrawDelayTimer.Enabled := True;
  UpdateWindows;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TMainForm.OnPreviewImageDoubleClick(Sender: TObject);
begin
  if CurrentCursorMode = msRotateMove then
  begin
    StopThread;
    UpdateUndo;
    MainCp.FAngle := 0;
    PreviewRedrawDelayTimer.Enabled := True;
    UpdateWindows;
  end
  else ResetSelectedFlameCamera(TUiCommandAction.DefaultArgs);
end;

///////////////////////////////////////////////////////////////////////////////
procedure TMainForm.SetShowTransparencyInPreview(const Args: TUiCommandBooleanEventArgs);
var value: boolean;
begin
  if not Assigned(args.Command) then exit;

  value := Args.Checked;
  Args.Command.Checked := value;
  
  ShowTransparency := value;
  DrawImageView;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TMainForm.ShowTraceWindow;
begin
  TraceForm.Show;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TMainForm.OnFormKeyStateChanged(Sender: TObject; var Key: Word;
  Shift: TShiftState);
var
  MousePos: TPoint;
begin
  if Shift <> FShiftState then begin
    if CurrentCursorMode in [msZoomWindowMove, msZoomOutWindowMove, msRotateMove, msDragMove] then
    begin
      // hack: to generate MouseMove event
      GetCursorPos(MousePos);
      SetCursorPos(MousePos.x, MousePos.y);
    end;

    if (CurrentCursorMode in [msZoomWindowMove, msZoomOutWindowMove]) then
    begin
      DrawZoomWindow;
      FShiftState := Shift;
      DrawZoomWindow;
    end
    else FShiftState := Shift;
  end;
end;

procedure TMainForm.OnListViewSelectedItemChanging(Sender: TObject; Item: TListItem; Change: TItemChange; var AllowChangeListViewItem: Boolean);
var
  selectedFlameName, focusedFlameName: string;
begin
  if (Item = nil) then exit;
  if (DoNotAskAboutChange = true)then exit;

  selectedFlameName := '';
  focusedFlameName := '';
  (*
  if (currentListView.Selected <> nil) then selectedFlameName := currentListView.Selected.Caption;
  if (currentListView.ItemFocused <> nil) then focusedFlameName := currentListView.ItemFocused.Caption;

  if (Trim(Item.Caption) = Trim(maincp.name)) and (Item.Selected) and (Item.Selected) and (Change = ctState) then
  begin
    if (UndoIndex <> 0) then
    begin
      if (SelectedFlameNameMemento = selectedFlameName) and (FocusedFlameNameMemento = focusedFlameName) then
      begin
        AllowChangeListViewItem := AllowChangeListViewItemMemento;

        // -x- good lord, give me strength!
        if Not AllowChangeListViewItem then begin
          currentListView.OnChange := nil;
          currentListView.OnChanging := nil;

          currentListView.Selected := Item;
          currentListView.ItemFocused := Item;

          currentListView.OnChanging := OnListViewSelectedItemChanging;
          currentListView.OnChange := OnListViewSelectedItemChanged;
        end;

        Exit;
      end;

      SelectedFlameNameMemento := selectedFlameName;
      FocusedFlameNameMemento := focusedFlameName;

      if Application.MessageBox('Do you really want to open another flame? All changes made to the current flame will be lost.', 'Apophysis', MB_ICONWARNING or MB_YESNO) <> IDYES then
      begin
        AllowChangeListViewItem := false;

        // -x- good lord, give me strength again!
        currentListView.OnChange := nil;
        currentListView.OnChanging := nil;

        currentListView.Selected := Item;
        currentListView.ItemFocused := Item;

        currentListView.OnChanging := OnListViewSelectedItemChanging;
        currentListView.OnChange := OnListViewSelectedItemChanged;
      end else begin
        AllowChangeListViewItem := true;
      end;

      AllowChangeListViewItemMemento := AllowChangeListViewItem;
    end;
  end;
  *)
end;

procedure TMainForm.SetShowIconsInListView(const Args: TUiCommandBooleanEventArgs);
var 
  value: boolean;
begin
  if not Assigned(args.Command) then exit;

  value := Args.Checked;
  Args.Command.Checked := value;

  ListViewManager.ShowThumbnails := value;
end;

procedure TMainForm.OnFlameReaderClosingTagEncountered(Sender: TObject; TagName: String);
var sb : string;
begin
  if (TagName='flame') then begin
    EndParsing(ParseCP, sb);
    MainForm.StatusBar.Panels[0].Text := sb;
  end;
end;

procedure TMainForm.CreateAndSelectNewFlame;
var
  i, ci:integer;
begin
  if (AlwaysCreateBlankFlame) then begin
    ClearCp(MainCp);
    inc(RandomIndex);
    MainCp.name := RandomPrefix + RandomDate + '-' + IntToStr(RandomIndex);
    ci := Random(256);
    GetCMap(ci, 1, MainCp.cmap);
    MainCp.cmapIndex := ci;

    if AdjustForm.Visible then AdjustForm.UpdateDisplay;
    if EditForm.Visible then EditForm.UpdateDisplay;
    
    PreviewRedrawDelayTimer.enabled := true;
  end
  else TemplateForm.Show;
end;

function Split(const fText: String; const fSep: Char; fTrim: Boolean=false; fQuotes: Boolean=false):TStringList;
var vI: Integer;
    vBuffer: String;
    vOn: Boolean;
begin
  Result:=TStringList.Create;
  vBuffer:='';
  vOn:=true;
  for vI:=1 to Length(fText) do
  begin
    if (fQuotes and(fText[vI]=fSep)and vOn)or(Not(fQuotes) and (fText[vI]=fSep)) then
    begin
      if fTrim then vBuffer:=Trim(vBuffer);
      if vBuffer='' then vBuffer:=fSep; // !!! e.g. split(',**',',')...
      if vBuffer[1]=fSep then
        vBuffer:=Copy(vBuffer,2,Length(vBuffer));
      Result.Add(vBuffer);
      vBuffer:='';
    end;
    if fQuotes then
    begin
      if fText[vI]='"' then
      begin
        vOn:=Not(vOn);
        Continue;
      end;
      if (fText[vI]<>fSep)or((fText[vI]=fSep)and(vOn=false)) then
        vBuffer:=vBuffer+fText[vI];
    end else
      if fText[vI]<>fSep then
        vBuffer:=vBuffer+fText[vI];
  end;
  if vBuffer<>'' then
  begin
    if fTrim then vBuffer:=Trim(vBuffer);
    Result.Add(vBuffer);
  end;
end;

procedure TMainForm.AutoSaveTimerCallback(Sender: TObject);
var
  filename,title : string;
  Tag: string;
  IFile: TextFile;
  FileList, FileListPre: TStringList;
  i, p: integer;
  erase : boolean;
  bakname: string;
begin
  erase := false;
  filename := AutoSavePath;
  title := CleanXMLName(maincp.name) + ' (' + FormatDateTime('MM-dd-yyyy hh:mm:ss', Now) + ')';
  Tag := RemoveExt(filename);

  if FileExists(filename) then begin
    FileListPre := TStringList.create;
      try
        FileListPre.LoadFromFile(filename);
        if (FileListPre.Count > 1000) then erase := true;
      finally
        FileListPre.Free;
      end;

      if (erase) then begin
        bakname := ChangeFileExt(filename, '.bak');
        if FileExists(bakname) then DeleteFile(bakname);
        RenameFile(filename, bakname);
      end;
  end;

  try
    if FileExists(filename) then
    begin
      bakname := ChangeFileExt(filename, '.temp');
      if FileExists(bakname) then DeleteFile(bakname);
      RenameFile(filename, bakname);

      FileList := TStringList.create;
      try
        FileList.LoadFromFile(bakname);

        if Pos('<flame name="' + title + '"', FileList.Text) <> 0 then
        begin
          i := 0;
          while Pos('<flame name="' + title + '"', Trim(FileList[i])) = 0 do
            inc(i);

          p := 0;
          while p = 0 do
          begin
            p := Pos('</flame>', FileList[i]);
            FileList.Delete(i);
          end;
        end;

//      FileList := TStringList.create;
//      try
//        FileList.LoadFromFile(filename);

        // fix first line
        if (FileList.Count > 0) then begin
          FileList[0] := '<flames name="' + Tag + '">';
        end;

        if FileList.Count > 2 then
        begin
          if pos('<flame ', FileList.text) <> 0 then
            repeat
              FileList.Delete(FileList.Count - 1);
            until (Pos('</flame>', FileList[FileList.count - 1]) <> 0)
          else
            repeat
              FileList.Delete(FileList.Count - 1);
            until (Pos('<' + Tag + '>', FileList[FileList.count - 1]) <> 0) or
                  (Pos('</flames>', FileList[FileList.count - 1]) <> 0);
        end else
        begin
          FileList.Delete(FileList.Count - 1);
        end;

        FileList.Add(Trim(FlameToXMLAS(maincp, title, false)));
        FileList.Add('</flames>');
        FileList.SaveToFile(filename);

      finally
        if FileExists(bakname) and not FileExists(filename) then
          RenameFile(bakname, filename);

        FileList.Free;
        if FileExists(bakname) then DeleteFile(bakname);
      end;
    end
    else
    begin
    // New file ... easy
      AssignFile(IFile, filename);
      ReWrite(IFile);
      Writeln(IFile, '<flames name="' + Tag + '">');
      Write(IFile, FlameToXMLAS(maincp, title, false));
      Writeln(IFile, '</flames>');
      CloseFile(IFile);
    end;
  except on E: EInOutError do
    begin
      //Application.MessageBox('Cannot save file', 'Apophysis', 16);
    end;
  end;
end;

procedure TMainForm.OpenLastAutomaticallySavedBatch;
var fn:string;
begin
  if (not fileexists(AutoSavePath)) then begin
    Application.MessageBox(PChar(TextByKey('main-status-noautosave')), PChar('Apophysis'), MB_ICONERROR);
    exit;
  end;

{$ifdef DisableScripting}
{$else}
  ScriptEditor.Stopped := True;
{$endif}
    fn := AutoSavePath;
    MainForm.CurrentFileName := fn;
    LastOpenFile := fn;
    Maincp.name := '';
    ParamFolder := ExtractFilePath(fn);
    ListViewMenuRenameItem.Enabled := True;
    ListViewMenuDeleteItem.Enabled := True;
    OpenFile := fn;
    if APP_BUILD = '' then MainForm.Caption := AppVersionString + ' - ' + openFile
    else MainForm.Caption := AppVersionString + ' ' + APP_BUILD + ' - ' + openFile;
    OpenFileType := ftXML;
    ListXML(fn, 1)
end;

procedure TMainForm.OpenHelp;
var
  URL, HelpTopic: string;
begin
  if (HelpPath <> '') then begin
    if (not WinShellExecute('open', HelpPath)) then begin
      MessageBox(self.Handle, PCHAR(Format(TextByKey('common-genericopenfailure'), [HelpPath])), PCHAR('Apophysis'), MB_ICONHAND);
    end;
  end else MessageBox(self.Handle, PCHAR(TextByKey('main-status-nohelpfile')), PCHAR('Apophysis'), MB_ICONHAND);
end;

function TMainForm.RetrieveXML(cp : TControlPoint):string;
begin
    Result := FlameToXML(cp, false, false);
end;

procedure TMainForm.SetShowGuidelinesInPreview(const Args: TUiCommandBooleanEventArgs);
var value: boolean;
begin
  if not Assigned(args.Command) then exit;

  value := Args.Checked;
  Args.Command.Checked := value;
  
  EnableGuides := value;
  DrawImageView;
end;

function WinExecAndWait32(FileName: string): integer;
var
  zAppName: array[0..1024] of Char;
  zCurDir: array[0..255] of Char;
  WorkDir: string;
  StartupInfo: TStartupInfo;
  ProcessInfo: TProcessInformation;
  r : dword;
begin
  StrPCopy(zAppName, FileName);
  GetDir(0, WorkDir);
  StrPCopy(zCurDir, WorkDir);
  FillChar(StartupInfo, Sizeof(StartupInfo), #0);
  StartupInfo.cb := Sizeof(StartupInfo);

  StartupInfo.dwFlags := STARTF_USESHOWWINDOW;
  StartupInfo.wShowWindow := 0;
  if (not CreateProcess(nil, zAppName, nil, nil, false, CREATE_NEW_CONSOLE or NORMAL_PRIORITY_CLASS, nil, nil, StartupInfo, ProcessInfo)) then
    Result := -1
  else begin
    WaitforSingleObject(ProcessInfo.hProcess, INFINITE);
    GetExitCodeProcess(ProcessInfo.hProcess, r);
    result := r;
    CloseHandle(ProcessInfo.hProcess);
    CloseHandle(ProcessInfo.hThread);
  end;
end;

procedure ListXML(FileName: string; sel: integer);
begin
  MainForm.IsLoadingBatch := true;

  MainForm.Batch := TBatch.Create(FileName);
  MainForm.ListViewManager.Batch := MainForm.Batch;

  (*
  case sel of
    0: MainForm.ListView.Selected := MainForm.ListView.Items.Count - 1;
    1: MainForm.ListView.Selected := 0;
    2: // do nothing
  end;
  *)

  MainForm.IsLoadingBatch := false;
end;

procedure TMainForm.OpenManual;
begin
  WinShellOpen('http://media.xyrus-worx.org/apophysis-usermanual');
end;

procedure TMainForm.CreateSubstMap;
begin
  SubstSource.Add('cross2'); SubstTarget.Add('cross');
  SubstSource.Add('Epispiral'); SubstTarget.Add('epispiral');
  SubstSource.Add('Epispiral_n'); SubstTarget.Add('epispiral_n');
  SubstSource.Add('Epispiral_thickness'); SubstTarget.Add('epispiral_thickness');
  SubstSource.Add('Epispiral_holes'); SubstTarget.Add('epispiral_holes');
  SubstSource.Add('bwraps2'); SubstTarget.Add('bwraps');
  SubstSource.Add('bwraps2_cellsize'); SubstTarget.Add('bwraps_cellsize');
  SubstSource.Add('bwraps2_space'); SubstTarget.Add('bwraps_space');
  SubstSource.Add('bwraps2_gain'); SubstTarget.Add('bwraps_gain');
  SubstSource.Add('bwraps2_inner_twist'); SubstTarget.Add('bwraps_inner_twist');
  SubstSource.Add('bwraps2_outer_twist'); SubstTarget.Add('bwraps_outer_twist');
  SubstSource.Add('pre_bwraps2'); SubstTarget.Add('pre_bwraps');
  SubstSource.Add('pre_bwraps2_cellsize'); SubstTarget.Add('pre_bwraps_cellsize');
  SubstSource.Add('pre_bwraps2_space'); SubstTarget.Add('pre_bwraps_space');
  SubstSource.Add('pre_bwraps2_gain'); SubstTarget.Add('pre_bwraps_gain');
  SubstSource.Add('pre_bwraps2_inner_twist'); SubstTarget.Add('pre_bwraps_inner_twist');
  SubstSource.Add('pre_bwraps2_outer_twist'); SubstTarget.Add('pre_bwraps_outer_twist');
  SubstSource.Add('post_bwraps2'); SubstTarget.Add('post_bwraps');
  SubstSource.Add('post_bwraps2_cellsize'); SubstTarget.Add('post_bwraps_cellsize');
  SubstSource.Add('post_bwraps2_space'); SubstTarget.Add('post_bwraps_space');
  SubstSource.Add('post_bwraps2_gain'); SubstTarget.Add('post_bwraps_gain');
  SubstSource.Add('post_bwraps2_inner_twist'); SubstTarget.Add('post_bwraps_inner_twist');
  SubstSource.Add('post_bwraps2_outer_twist'); SubstTarget.Add('post_bwraps_outer_twist');
  SubstSource.Add('bwraps7'); SubstTarget.Add('bwraps');
  SubstSource.Add('bwraps7_cellsize'); SubstTarget.Add('bwraps_cellsize');
  SubstSource.Add('bwraps7_space'); SubstTarget.Add('bwraps_space');
  SubstSource.Add('bwraps7_gain'); SubstTarget.Add('bwraps_gain');
  SubstSource.Add('bwraps7_inner_twist'); SubstTarget.Add('bwraps_inner_twist');
  SubstSource.Add('bwraps7_outer_twist'); SubstTarget.Add('bwraps_outer_twist');
  SubstSource.Add('logn'); SubstTarget.Add('log');
  SubstSource.Add('logn_base'); SubstTarget.Add('log_base');
end;
function TMainForm.ReadWithSubst(Attributes: TAttrList; attrname: string): string;
var i: integer; v: TStringType;
begin
  v := Attributes.Value(TStringType(attrname));
  if (v <> '') then begin
    Result := String(v);
    Exit;
  end;

  for i := 0 to SubstTarget.Count - 1 do begin
    if (SubstTarget[i] = attrname) then begin
      v := Attributes.Value(TStringType(SubstSource[i]));
      if (v <> '') then begin
        Result := String(v);
        Exit;
      end;
    end;
  end;

  Result := '';
end;

end.
