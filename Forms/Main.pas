unit Main;

interface

uses
  Windows, Forms, Dialogs, Menus, Controls, ComCtrls, ToolWin, StdCtrls, Classes, Messages,
  ExtCtrls, ImgList, Jpeg, SyncObjs, SysUtils, ClipBrd, Graphics, Math, ExtDlgs, AppEvnts,
  ShellAPI, Registry, System.ImageList,

  Global, Xform, XFormMan, ControlPoint, CMap, RenderThread, RenderingCommon, RenderingInterface,
  PngImage, StrUtils, LoadTracker, Translation, UIRibbonForm, UIRibbon, UIRibbonCommands,
  ApophysisRibbon, FlameListView, ParameterIO;

const
  randFilename = 'Apophysis7X.rand';
  undoFilename = 'Apophysis7X.undo';

type
  TMouseMoveState =
    (msUsual, msZoomWindow, msZoomOutWindow, msZoomWindowMove,
     msZoomOutWindowMove, msDrag, msDragMove, msRotate,
     msRotateMove, msPitchYaw, msHeight);

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

   // Vieuals
    ListViewPanel: TPanel;
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

    procedure LoadUndoFlame(index: integer; filename: string);
    function SaveUndoFlame(cp1: TControlPoint; title, filename: string): boolean;

    procedure DrawImageView;
    procedure DrawZoomWindow;
    procedure DrawRotatelines(Angle: double);

    procedure HandleThreadCompletion(var Message: TMessage); message WM_THREAD_COMPLETE;
    procedure HandleThreadTermination(var Message: TMessage); message WM_THREAD_TERMINATE;

    procedure SetCursorModePan;
    procedure SetCursorModeRotate;
    procedure SetCursorModeZoomIn;
    procedure SetCursorModeZoomOut;

    procedure SetCanUndo(value: boolean);
    procedure SetCanRedo(value: boolean);
    procedure SetCanPaste(value: boolean);

    procedure UpdateCursorModeCommandStates;

    procedure Trace1(const str: string);
    procedure Trace2(const str: string);

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
    procedure OnFormDisplayHint(Sender: TObject);
    procedure OnFormClosed(Sender: TObject; var Action: TCloseAction);
    procedure OnFormDestroyed(Sender: TObject);

    procedure OnListViewSelectedItemChanged(index: integer);
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
    procedure OnPreviewProgressUpdated(prog: double);

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

    ViewPosition, OldViewPosition: TSPoint;
    ViewScale: double;
    ViewShiftState: TShiftState;

    CameraDragMode, CameraWasDragged: boolean;
    CameraDragPosition, OldCameraDragPosition: TPoint;
    CameraDragOffsetX, CameraDragOffsetY: double;

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

   // Public fields
    Renderer: TRenderThread;
    ListViewManager: TFlameListView;
    Batch: TBatch;

    UndoIndex, UndoStackSize: integer;
    CameraCenter: array[0..1] of double;
    PreviewStartTimestamp: TDateTime;

   // Public methods
    procedure PushWorkspaceToUndoStack;

    procedure EmptyBatch;
    procedure RandomBatch;

    procedure FitPreviewImageSize;
    procedure StopPreviewRenderThread;

    procedure SendSelectedFlameToToolWindows;

    procedure ResetControlPoint(var cp: TControlPoint);
    procedure RandomizeControlPoint(var cp1: TControlPoint; alg: integer = 0);

    function SerializeColorMapToPaletteString(const pal: TColorMap; const title: string): string;
    function SavePaletteString(Gradient, Title, FileName: string): boolean;

   // Commands
    procedure ExecuteUndo(const args : TUiCommandActionEventArgs);
    procedure ExecuteRedo(const args : TUiCommandActionEventArgs);

    procedure ExecuteOpenFractalFlamePublication(const args : TUiCommandActionEventArgs);
    procedure ExecuteOpenManual(const args : TUiCommandActionEventArgs);
    procedure ExecuteOpenHelp(const args : TUiCommandActionEventArgs);

    procedure ExecuteSetCursorMode(const args : TUiCommandBooleanEventArgs);

    procedure ExecuteOpenBatchFromDisk(const args : TUiCommandActionEventArgs);
    procedure ExecuteLoadLastAutomaticallySavedFlame(const args : TUiCommandActionEventArgs);
    procedure ExecuteSaveBatchToDisk(const args : TUiCommandActionEventArgs);
    procedure ExecuteRenderBatch(const args : TUiCommandActionEventArgs);

    procedure ExecuteCreateAndSelectNewFlame(const args : TUiCommandActionEventArgs);
    procedure ExecuteSaveSelectedFlameToBatch(const args : TUiCommandActionEventArgs);
    procedure ExecuteRenderSelectedFlame(const args : TUiCommandActionEventArgs);
    procedure ExecuteResetSelectedFlameCamera(const args : TUiCommandActionEventArgs);
    procedure ExecuteCopySelectedFlameToClipboard(const args : TUiCommandActionEventArgs);
    procedure ExecuteReplaceSelectedFlameWithClipboard(const args : TUiCommandActionEventArgs);
    procedure ExecuteDeleteSelectedFlame(const args : TUiCommandActionEventArgs);

    procedure ExecuteRunCurrentScript(const args : TUiCommandActionEventArgs);
    procedure ExecuteStopCurrentScript(const args : TUiCommandActionEventArgs);
    procedure ExecuteOpenScriptFromDisk(const args : TUiCommandActionEventArgs);

    procedure ExecuteCreatePaletteFromImage(const Args: TUiCommandActionEventArgs);

    procedure ExecuteSetShowGuidelinesInPreview(const Args: TUiCommandBooleanEventArgs);
    procedure ExecuteSetShowTransparencyInPreview(const Args: TUiCommandBooleanEventArgs);
    procedure ExecuteSetShowIconsInListView(const Args: TUiCommandBooleanEventArgs);
    procedure ExecuteSetListViewVisibility(const Args: TUiCommandBooleanEventArgs);
    procedure ExecuteSetPreviewDensity(const Args: TUiCommandActionEventArgs);

    procedure ExecuteShowFullscreenPreviewWindow(const args : TUiCommandActionEventArgs);
    procedure ExecuteShowEditorWindow(const args : TUiCommandActionEventArgs);
    procedure ExecuteShowCameraWindow(const args : TUiCommandActionEventArgs);
    procedure ExecuteShowPaletteWindow(const args : TUiCommandActionEventArgs);
    procedure ExecuteShowOutputPropertiesWindow(const args : TUiCommandActionEventArgs);
    procedure ExecuteShowCanvasSizeWindow(const args : TUiCommandActionEventArgs);
    procedure ExecuteShowScriptEditorWindow(const args : TUiCommandActionEventArgs);
    procedure ExecuteShowScriptFavoritesWindow(const args : TUiCommandActionEventArgs);
    procedure ExecuteShowOptionsWindow(const args : TUiCommandActionEventArgs);
    procedure ExecuteShowTraceWindow(const args : TUiCommandActionEventArgs);
    procedure ExecuteShowAboutWindow(const args : TUiCommandActionEventArgs);

    procedure ExecuteClose(const args : TUiCommandActionEventArgs);

  end;

var
  MainForm: TMainForm;
  MainCp: TControlPoint;
  ApophysisVersion:string;

implementation

{$Include 'delphiversion.pas'}

uses
  Editor, Options, Settings, FullScreen, FormRender, Adjust, Browser, Save,
  About, CmapData, RndFlame, Tracer, Types, varGenericPlugin;

{$R *.dfm}
{$R 'System\UIRibbon.res'}
{$R 'Ribbon\ApophysisRibbon.res'}

procedure TMainForm.Trace1(const str: string);
begin
  if TraceLevel >= 1 then
    TraceForm.MainTrace.Lines.Add('. ' + str);
end;

procedure TMainForm.Trace2(const str: string);
begin
  if TraceLevel >= 2 then
    TraceForm.MainTrace.Lines.Add('. . ' + str);
end;

procedure TMainForm.StopPreviewRenderThread;
begin
  PreviewRedrawDelayTimer.Enabled := False;
  if Assigned(Renderer) then begin
    assert(Renderer.Suspended = false);
    PreviewThreadChangingContext := true;
    Renderer.BreakRender;
    Renderer.WaitFor;
  end;
end;

procedure TMainForm.ResetControlPoint(var cp: TControlPoint);
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

procedure TMainForm.RandomizeControlPoint(var cp1: TControlPoint; alg: integer = 0);
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

function TMainForm.SerializeColorMapToPaletteString(const pal: TColorMap; const title: string): string;
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

procedure TMainForm.OnPreviewProgressUpdated(prog: double);
var
  Elapsed, Remaining: TDateTime;
  IntProg: Integer;
begin
  IntProg := (round(prog * 100));
  //pnlLSPFrame.Visible := true;
  StatusProgressBar.Position := IntProg;
  if (IntProg = 100) then StatusProgressBar.Position := 0;
  Elapsed := Now - PreviewStartTimestamp;
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

procedure TMainForm.PushWorkspaceToUndoStack;
begin
  MainCp.FillUsedPlugins;
  SaveUndoFlame(MainCp, Format('%.4d-', [UndoIndex]) + MainCp.name,
    GetEnvVarValue('APPDATA') + '\' + undoFilename);
  Inc(UndoIndex);
  UndoStackSize := UndoIndex; //Inc(UndoMax);
  SetCanUndo(true);
  PreviewPanelMenuUndoItem.Enabled := True;
  SetCanRedo(false);
  PreviewPanelMenuRedoItem.Enabled := false;
end;

function TMainForm.SaveUndoFlame(cp1: TControlPoint; title, filename: string): boolean;
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

function TMainForm.SavePaletteString(Gradient, Title, FileName: string): boolean;
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

{ ****************************** Display ************************************ }

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
  else oldscale := ViewScale;

  TransparentPreviewImage := Renderer.GetTransparentImage;

  if (TransparentPreviewImage <> nil) and (TransparentPreviewImage.Width > 0) then begin
    ViewScale := TransparentPreviewImage.Width / PreviewImage.Width;

    ViewPosition.X := ViewScale/oldscale * (ViewPosition.X - OldViewPosition.X);
    ViewPosition.Y := ViewScale/oldscale * (ViewPosition.Y - OldViewPosition.Y);

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


{ ************************** IFS and triangle stuff ************************* }

procedure TMainForm.EmptyBatch;
var
  F: TextFile;
  b, RandFile: string;
  cmap_index,ci: integer;
  str: string;
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
    ResetControlPoint(MainCp);
    //MainCp.CalcBoundbox;

    MainCp.name := RandomPrefix + RandomDate + '-' + IntToStr(RandomIndex);
    ci := Random(256); //Random(NRCMAPS);
    GetCMap(ci, 1, MainCp.cmap);
    MainCp.cmapIndex := ci;

    SaveCpToXmlCompatible(str, mainCp);
    Write(F, str);

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
  str: string;
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
      RandomizeControlPoint(MainCp);
      MainCp.CalcBoundbox;

(*     Title := RandomPrefix + RandomDate + '-' +
        IntToStr(RandomIndex);
  *)
      MainCp.name := RandomPrefix + RandomDate + '-' + IntToStr(RandomIndex);

      SaveCpToXmlCompatible(str, MainCp);
      Write(F, str);
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


procedure TMainForm.ExecuteLoadLastAutomaticallySavedFlame;
var
  autoSaveBatch: TBatch;
begin
  if (not FileExists(AutoSavePath)) then
  begin
    Application.MessageBox(
      PWideChar('There is no automatically saved flame available to load.'),
      PWideChar('Apophysis'),
      MB_ICONERROR);
    exit;
  end;

  autoSaveBatch := TBatch.Create(AutoSavePath);

  if (autoSaveBatch.Count = 0) then
  begin
    Application.MessageBox(
      PWideChar('There is no automatically saved flame available to load.'),
      PWideChar('Apophysis'),
      MB_ICONERROR);
    exit;
  end;

  autoSaveBatch.LoadControlPoint(autoSaveBatch.Count - 1, MainCp);
  autoSaveBatch.Destroy;
end;

procedure TMainForm.ExecuteOpenBatchFromDisk;
var
  fileName, filter, directory: string;
begin
  filter := TextByKey('common-filter-flamefiles') + '|*.flame;*.xml|' + TextByKey('common-filter-allfiles') + '|*.*';
  directory := ParamFolder;

  if OpenSaveFileDialog(MainForm, '.flame', filter, directory, TextByKey('common-browse'), fileName, true, false, false, true) then
  begin
    LastOpenFile := fileName;
    ParamFolder := ExtractFilePath(fileName);

    ListViewMenuRenameItem.Enabled := True;
    ListViewMenuDeleteItem.Enabled := True;

    OpenFile := fileName;

    if APP_BUILD = '' then
      Caption := ApophysisVersion + ' - ' + openFile
    else
      Caption := ApophysisVersion + ' ' + APP_BUILD + ' - ' + openFile;

    Batch := TBatch.Create(fileName);
    ListViewManager.Batch := MainForm.Batch;
    ListViewManager.SelectedIndex := 0;
  end;
end;

procedure TMainForm.ExecuteSaveSelectedFlameToBatch;
var
  i, j: integer;
begin
  SaveForm.SaveType := stSaveParameters;
  SaveForm.Filename := SavePath;
  SaveForm.Title := maincp.name;

  if SaveForm.ShowModal = mrOK then
  begin
    MainCp.name := SaveForm.Title;
    SavePath := SaveForm.Filename;

    if ExtractFileExt(SavePath) = '' then
      SavePath := SavePath + '.flame';

    i := -1;
    for j := 0 to Batch.Count do
    begin
      if Lowercase(Batch.GetFlameNameAt(j)) = MainCp.Name then
      begin
        i := j;
        Break;
      end;
    end;

    if i >= 0 then
      Batch.StoreControlPoint(i, MainCp)
    else begin
      Batch.AppendControlPoint(MainCp);
      i := Batch.Count - 1;
    end;

    Batch.SaveBatch(SavePath);
    StatusBar.Panels[3].Text := maincp.name;

    if (SavePath = OpenFile) then
      ListViewManager.Refresh(i);
  end;
end;

procedure TMainForm.ExecuteSaveBatchToDisk;
var
  i: integer;
begin
  SaveForm.SaveType := stSaveAllParameters;
  SaveForm.Filename := SavePath;

  if SaveForm.ShowModal = mrOK then
  begin
    SavePath := SaveForm.Filename;

    if ExtractFileExt(SavePath) = '' then
      SavePath := SavePath + '.flame';

    i := ListViewManager.SelectedIndex;
    Batch.StoreControlPoint(i, MainCp);
    Batch.SaveBatch(SavePath);
    ListViewManager.Refresh(-1);
  end;
end;

procedure TMainForm.ExecuteCopySelectedFlameToClipboard;
var
  txt: string;
begin
  SaveCpToXmlCompatible(txt, MainCp);
  Clipboard.SetTextBuf(PChar(txt));
  SetCanPaste(true);
end;

procedure TMainForm.ExecuteReplaceSelectedFlameWithClipboard;
var
  status: string;
begin
  if Clipboard.HasFormat(CF_TEXT) then begin
    PushWorkspaceToUndoStack;

    StopPreviewRenderThread;

    LoadCpFromXmlCompatible(Clipboard.AsText, MainCp, status);
    Batch.StoreControlPoint(ListViewManager.SelectedIndex, MainCp);
    ListViewManager.Refresh(-1);

    Statusbar.Panels[3].Text := MainCp.name;
    FitPreviewImageSize;

    BeginUpdatePreview;
    SendSelectedFlameToToolWindows;
  end;
end;

// -x- todo move to FlameListView.BeginEditSelectedItem
procedure TMainForm.OnListViewMenuRenameClick(Sender: TObject);
begin
  //Ribbontodo
  if ListViewManager.SelectedIndex >= 0 then
  begin
    ListView.Items[ListViewManager.SelectedIndex].EditCaption;
  end;
end;

procedure TMainForm.ExecuteDeleteSelectedFlame;
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
      Batch.RemoveAt(ListViewManager.SelectedIndex);
      ListViewManager.Refresh(-1);
    end;
  end;
end;

procedure TMainForm.ExecuteShowOptionsWindow;
begin
  OptionsForm.ShowModal;
  // --Z--
  StopPreviewRenderThread;
  PreviewRedrawDelayTimer.Enabled := True;
  //RIBBONTODO  tbQualityBox.Text := FloatToStr(defSampleDensity);
  //RIBBONTODO  tbShowAlpha.Down := ShowTransparency;
  DrawImageView;
  SendSelectedFlameToToolWindows;
end;

procedure TMainForm.ExecuteShowOutputPropertiesWindow;
begin
  AdjustForm.UpdateDisplay;
  AdjustForm.PageControl.TabIndex:=1;
  AdjustForm.Show;
end;

procedure TMainForm.BeginUpdatePreview;
begin
  PreviewRedrawDelayTimer.enabled := true;
end;

procedure TMainForm.ExecuteShowEditorWindow;
begin
  EditForm.Show;
end;

procedure TMainForm.ExecuteClose;
begin
  ExecuteClose(TUiCommandAction.DefaultArgs);
end;





procedure TMainForm.OnFormDisplayHint(Sender: TObject);
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

procedure TMainForm.RibbonLoaded;
var settingsFile: string;
begin
  Ribbon.SetApplicationModes(0);
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
        NewFlameCommand.OnExecute := ExecuteCreateAndSelectNewFlame;
      end;
    OpenBatchCommand_Id: 
      begin
        OpenBatchCommand := Command as TUICommandAction;
        OpenBatchCommand.SetShortCut([ssCtrl], 'O');
        OpenBatchCommand.OnExecute := ExecuteOpenBatchFromDisk;
      end;
    SaveFlameCommand_Id: 
      begin
        SaveFlameCommand := Command as TUICommandAction;
        SaveFlameCommand.SetShortCut([ssCtrl], 'S');
        SaveFlameCommand.OnExecute := ExecuteSaveSelectedFlameToBatch;
      end;
    SaveBatchCommand_Id: 
      begin
        SaveBatchCommand := Command as TUICommandAction;
        SaveBatchCommand.SetShortCut([ssCtrl, ssAlt], 'S');
        SaveBatchCommand.OnExecute :=  ExecuteSaveBatchToDisk;
      end;
    RestoreLastAutosaveCommand_Id: 
      begin
        RestoreLastAutosaveCommand := Command as TUICommandAction;
        RestoreLastAutosaveCommand.SetShortCut([ssCtrl, ssAlt], 'A');
        RestoreLastAutosaveCommand.OnExecute := ExecuteLoadLastAutomaticallySavedFlame;
      end;

    UndoCommand_Id:
      begin
        UndoCommand := Command as TUICommandAction;
        UndoCommand.SetShortCut([ssCtrl], 'Z');
        UndoCommand.OnExecute := ExecuteUndo;
      end;
    RedoCommand_Id:
      begin
        RedoCommand := Command as TUICommandAction;
        RedoCommand.SetShortCut([ssCtrl], 'Y');
        RedoCommand.OnExecute := ExecuteRedo;
      end;
    CopyCommand_Id:
      begin
        CopyCommand := Command as TUICommandAction;
        CopyCommand.SetShortCut([ssCtrl], 'C');
        CopyCommand.OnExecute := ExecuteCopySelectedFlameToClipboard;
      end;
    PasteCommand_Id:
      begin
        PasteCommand := Command as TUICommandAction;
        PasteCommand.SetShortCut([ssCtrl], 'V');
        PasteCommand.OnExecute := ExecuteReplaceSelectedFlameWithClipboard;
      end;
      
    RenderFlameCommand_Id:
      begin
        RenderFlameCommand := Command as TUICommandAction;
        RenderFlameCommand.SetShortCut([ssCtrl], 'R');
        RenderFlameCommand.OnExecute := ExecuteRenderSelectedFlame;
      end;
    RenderBatchCommand_Id:
      begin
        RenderBatchCommand := Command as TUICommandAction;
        RenderBatchCommand.SetShortCut([ssCtrl, ssAlt], 'R');
        RenderBatchCommand.OnExecute := ExecuteRenderBatch;
      end;

    FullscreenPreviewCommand_Id:
      begin
        FullscreenPreviewCommand := Command as TUICommandAction;
        FullscreenPreviewCommand.SetShortCut([], VK_F3);
        FullscreenPreviewCommand.OnExecute := ExecuteShowFullscreenPreviewWindow;
      end;
    ShowEditorCommand_Id:
      begin
        ShowEditorCommand := Command as TUICommandAction;
        ShowEditorCommand.SetShortCut([], VK_F4);
        ShowEditorCommand.OnExecute := ExecuteShowEditorWindow;
      end;
    ShowAdjustmentCommand_Id:
      begin
        ShowCameraEditorCommand := Command as TUICommandAction;
        ShowCameraEditorCommand.SetShortCut([], VK_F5);
        ShowCameraEditorCommand.OnExecute := ExecuteShowCameraWindow;
      end;
    ShowOutputPropertiesCommand_Id:
      begin
        ShowOutputPropertiesCommand := Command as TUICommandAction;
        ShowOutputPropertiesCommand.SetShortCut([], VK_F6);
        ShowOutputPropertiesCommand.OnExecute := ExecuteShowOutputPropertiesWindow;
      end;
    ShowPaletteCommand_Id:
      begin
        ShowPaletteCommand := Command as TUICommandAction;
        ShowPaletteCommand.SetShortCut([], VK_F7);
        ShowPaletteCommand.OnExecute := ExecuteShowPaletteWindow;
      end;
    ShowCanvasCommand_Id:
      begin
        ShowPaletteCommand := Command as TUICommandAction;
        ShowPaletteCommand.SetShortCut([], VK_F8);
        ShowPaletteCommand.OnExecute := ExecuteShowCanvasSizeWindow;
      end;
    PaletteFromImageCommand_Id:
      begin
        PaletteFromImageCommand := Command as TUICommandAction;
        PaletteFromImageCommand.OnExecute := ExecuteCreatePaletteFromImage;
      end;

    {$IfNDef DisableScripts}
    RunScriptCommand_Id:
      begin
        RunScriptCommand := Command as TUICommandAction;
        RunScriptCommand.SetShortCut([], VK_F9);
        RunScriptCommand.OnExecute := ExecuteRunCurrentScript;
      end;
    StopScriptCommand_Id:
      begin
        StopScriptCommand := Command as TUICommandAction;
        StopScriptCommand.SetShortCut([ssShift], VK_F2);
        StopScriptCommand.OnExecute := ExecuteStopCurrentScript;
      end;
    OpenScriptCommand_Id:
      begin
        OpenScriptCommand := Command as TUICommandAction;
        OpenScriptCommand.OnExecute := ExecuteOpenScriptFromDisk;
      end;
    EditScriptCommand_Id:
      begin
        EditScriptCommand := Command as TUICommandAction;
        EditScriptCommand.OnExecute := ExecuteShowScriptEditorWindow;
      end;
    ManageScriptFavoritesCommand_Id:
      begin
        ManageScriptFavoritesCommand := Command as TUICommandAction;
        ManageScriptFavoritesCommand.OnExecute := ExecuteShowScriptFavoritesWindow;
      end;
    {$EndIf}

    PanModeCommand_Id:
      begin
        PanModeCommand := Command as TUICommandBoolean;
        PanModeCommand.OnToggle := ExecuteSetCursorMode;
        PanModeCommand.Checked := true;
      end;
    RotateModeCommand_Id:
      begin
        RotateModeCommand := Command as TUICommandBoolean;
        RotateModeCommand.OnToggle := ExecuteSetCursorMode;
      end;
    ZoomInCommand_Id:
      begin
        ZoomInModeCommand := Command as TUICommandBoolean;
        ZoomInModeCommand.OnToggle := ExecuteSetCursorMode;
      end;
    ZoomOutCommand_Id:
      begin
        ZoomOutModeCommand := Command as TUICommandBoolean;
        ZoomOutModeCommand.OnToggle := ExecuteSetCursorMode;
      end;

    ShowGuidelinesCommand_Id:
      begin
        ShowGuidelinesCommand := Command as TUICommandBoolean;
        ShowGuidelinesCommand.OnToggle := ExecuteSetShowGuidelinesInPreview;
      end;
    ShowTransparencyCommand_Id:
      begin
        ShowTransparencyCommand := Command as TUICommandBoolean;
        ShowTransparencyCommand.OnToggle := ExecuteSetShowTransparencyInPreview;
      end;
    ShowIconsCommand_Id:
      begin
        ShowIconsInListViewCommand := Command as TUICommandBoolean;
        ShowIconsInListViewCommand.OnToggle := ExecuteSetShowIconsInListView;
      end;

    ViewDensity5, ViewDensity10, ViewDensity15, ViewDensity25, 
    ViewDensity50, ViewDensity100, ViewDensity150, ViewDensity250,
    ViewDensity500, ViewDensity1000:
      begin
        densityCommand := Command as TUICommandAction;
        densityCommand.OnExecute := ExecuteSetPreviewDensity;
      end;
    
  end;
end;

procedure TMainForm.OnFormCreated(Sender: TObject);
var
  dte: string;
  showListIconsArgs: TUiCommandBooleanEventArgs;
begin
  InitializePlugins;

  {$ifdef Apo7X64}
  Application.Title := 'Apophysis 7x (64 bit)';
  {$else}
  Application.Title := 'Apophysis 7x (32 bit)';
  {$endif}

  Application.HelpFile := 'Apophysis7x.chm';
  Application.UpdateFormatSettings := False;

  {$ifdef UseFormatSettings}
    FormatSettings.DecimalSeparator := '.';
  {$else}
    DecimalSeparator := '.';
  {$endif}


  ApophysisVersion := APP_NAME + ' ' + APP_VERSION;
  Caption := ApophysisVersion + APP_BUILD;

  ListViewManager := TFlameListView.Create(ListView);
  ListViewManager.SelectedIndexChanged := OnListViewSelectedItemChanged;

  if (UseSmallThumbnails) then
    ListViewManager.ThumbnailSize := 96
  else ListViewManager.ThumbnailSize := 128;

  ListViewPanel.Width := ListViewManager.ThumbnailSize + 100;

  ReadSettings;

  // -x- We would need to save again so that the settings key exists right after start, if this is the first start
  SaveSettings;

  LoadLanguage(LanguageFile);

  AvailableLanguages := TStringList.Create;
  AvailableLanguages.Add('');
  ListLanguages;

  Screen.Cursors[crEditArrow]  := LoadCursor(HInstance, 'ARROW_WHITE');
  Screen.Cursors[crEditMove]   := LoadCursor(HInstance, 'MOVE_WB');
  Screen.Cursors[crEditRotate] := LoadCursor(HInstance, 'ROTATE_WB');
  Screen.Cursors[crEditScale]  := LoadCursor(HInstance, 'SCALE_WB');



  CurrentCursorMode := msDrag;
  LimitVibrancy := False;
  Favorites := TStringList.Create;
  Randomize;
  MainSeed := Random(123456789);
  maincp := TControlPoint.Create;
  Application.OnHint := OnFormDisplayHint;
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
  ViewScale := 1;

  BetweenListAndPreviewPanelSplitter.Left := ListViewPanel.Width;

  showListIconsArgs.Command := ShowIconsInListViewCommand;
  showListIconsArgs.Checked := true;
  ExecuteSetShowIconsInListView(showListIconsArgs);

end;

procedure TMainForm.OnFormShown(Sender: TObject);
var
  Registry: TRegistry;
  i: integer;
  a,b,c,d:integer;
  hnd,hr:Cardinal;
  index: integer;
  mins:integer;
  fn, flameXML : string;
  openScript : string;
begin
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
  UndoStackSize := 0;
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

  openFile := '';

  if ParamCount > 0 then
    openFile := ParamStr(1)
  else if RememberLastOpenFile then
    openFile := LastOpenFile
  else
    openFile := defFlameFile;

  if (openFile <> '') then
  begin
    if APP_BUILD = '' then
      Caption := ApophysisVersion + ' - ' + openFile
    else
      Caption := ApophysisVersion + ' ' + APP_BUILD + ' - ' + openFile;

    Batch := TBatch.Create(openFile);
    ListViewManager.Batch := MainForm.Batch;
    ListViewManager.SelectedIndex := 0;
  end else EmptyBatch;

  if (openFile = '') or (not FileExists(openFile)) and (RememberLastOpenFile) then begin
    openFile := LastOpenFile;
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

  PreviewThreadCount := Nrtreads;
  if not multithreadedPreview then
    PreviewThreadCount := 1;
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

  HtmlHelp(0, nil, HH_CLOSE_ALL, 0);
  { To capture secondary window positions }
  if EditForm.visible then EditForm.Close;
  if AdjustForm.visible then AdjustForm.close;
  if GradientBrowser.visible then GradientBrowser.close;
//  if GradientForm.visible then GradientForm.Close;

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

          scale := ViewScale * PreviewImage.Width / TransparentPreviewImage.Width;
          ViewPosition.X := ViewPosition.X - (CursorStart.Right - CursorStart.Left) / scale;
          ViewPosition.Y := ViewPosition.Y - (CursorStart.Bottom - CursorStart.Top) / scale;
        end;
      msRotateMove:
        CurrentCursorMode := msRotate;
    end;
    DrawImageView;
  end;
end;

{ ****************************** Misc controls ****************************** }

procedure TMainForm.OnPreviewPanelResized(Sender: TObject);
begin
  StopPreviewRenderThread;
  if CanDrawOnResize then
    PreviewRedrawDelayTimer.Enabled := True;

  FitPreviewImageSize;
  DrawImageView;
end;

procedure TMainForm.FitPreviewImageSize;
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
end;

procedure TMainForm.OnListViewSelectedItemChanged(index: integer);
begin
  if (index >= 0) and (Trim(Batch.GetFlameNameAt(index)) <> Trim(maincp.name)) then
  begin
    StopPreviewRenderThread;

    StopPreviewRenderThread;
    Batch.LoadControlPoint(index, MainCp);

    UndoIndex := 0;
    UndoStackSize := 0;

    SetCanUndo(false);
    SetCanRedo(false);

    if fileExists(GetEnvVarValue('APPDATA') + '\' + undoFilename) then
      DeleteFile(GetEnvVarValue('APPDATA') + '\' + undoFilename);

    Transforms := MainCp.TrianglesFromCP(MainTriangles);
    EditForm.SelectedTriangle := 0;

    Statusbar.Panels[3].Text := Maincp.name;

    Application.ProcessMessages;
    self.BeginUpdatePreview;
    SendSelectedFlameToToolWindows;
    FitPreviewImageSize;
  end;
end;

procedure TMainForm.SendSelectedFlameToToolWindows;
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
    CameraCenter[0] := maincp.Center[0];
    CameraCenter[1] := maincp.Center[1];
//    cp.CalcBoundbox;
//    MainCP.NormalizeWeights;
    Transforms := MainCp.TrianglesFromCP(MainTriangles);
    // Trim undo index from title
    maincp.name := Copy(Fstrings[0], 6, length(Fstrings[0]) - 7);

    if SavedPal then maincp.cmap := palette;
    if AdjustForm.visible then AdjustForm.UpdateDisplay;

    PreviewRedrawDelayTimer.Enabled := True;
    SendSelectedFlameToToolWindows;
  finally
    IFSStrings.Free;
    FStrings.Free;
    Tokens.free;
    EntryStrings.free;
  end;
end;

procedure TMainForm.OnListViewEditCompleted(Sender: TObject; Item: TListItem; var S: string);
var
  Strings: TStringList;
  i: integer;
  bakname: string;
  result: boolean;
begin

  if s <> Item.Caption then
  begin
    Result := True;
    Strings := TStringList.Create;
    try
      try
        i := 0;
        Strings.LoadFromFile(OpenFile);
        if Pos('name="' + Item.Caption + '"', Strings.Text) <> 0 then
        begin
          while Pos('name="' + Item.Caption + '"', Strings[i]) = 0 do
          begin
            inc(i);
          end;
          Strings[i] := StringReplace(Strings[i], Item.Caption, s, []);

          bakname := ChangeFileExt(OpenFile, '.bak');
          if FileExists(bakname) then DeleteFile(bakname);
          RenameFile(OpenFile, bakname);

          Strings.SaveToFile(OpenFile);
        end
        else
          Result := False;
      except on Exception do
        Result := False;
      end;
    finally
      Strings.Free;
    end;

    if not Result then
      s := item.Caption;
  end;
end;

procedure TMainForm.OnListViewMenuDeleteClick(Sender: TObject);
begin
  ExecuteDeleteSelectedFlame(TUiCommandAction.DefaultArgs);
end;

procedure TMainForm.PreviewRedrawDelayTimerCallback(Sender: TObject);
var
  GlobalMemoryInfo: TMemoryStatus; // holds the global memory status information
  RenderCP: TControlPoint;
  Mem, ApproxMem: cardinal;
  bs: integer;
begin
  if CurrentCursorMode in [msZoomWindowMove, msZoomOutWindowMove, msDragMove, msRotateMove] then exit;

  PreviewRedrawDelayTimer.enabled := False;
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
    RenderCP.sample_density := defSampleDensity;
    RenderCP.spatial_oversample := 1;
    RenderCP.spatial_filter_radius := 0.001;
    RenderCP.Transparency := true;

    GlobalMemoryInfo.dwLength := SizeOf(GlobalMemoryInfo);
    GlobalMemoryStatus(GlobalMemoryInfo);

    Mem := GlobalMemoryInfo.dwAvailPhys;

    if (singleBuffer) then
      bs := 16
    else
      bs := 32;

    Trace1('--- Previewing "' + RenderCP.name + '" ---');
    Trace1(Format('  Available memory: %f Mb', [Mem / (1024*1024)]));

    ApproxMem := int64(RenderCp.Width) * int64(RenderCp.Height) * (bs + 4 + 4);
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

    Trace1(Format('  Size: %dx%d, Quality: %f', [RenderCP.Width, RenderCP.Height, RenderCP.sample_density]));

    OldViewPosition.x := ViewPosition.x;
    OldViewPosition.y := ViewPosition.y;

    PreviewStartTimestamp := Now;

    try
      Renderer := TRenderThread.Create;
      Renderer.TargetHandle := MainForm.Handle;

      if TraceLevel > 0 then
        Renderer.Output := TraceForm.MainTrace.Lines;

      Renderer.OnProgress := OnPreviewProgressUpdated;
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

procedure TMainForm.ExecuteShowPaletteWindow;
begin
  AdjustForm.UpdateDisplay;
  AdjustForm.PageControl.TabIndex:=2;
  AdjustForm.Show;
end;

procedure TMainForm.ExecuteCreatePaletteFromImage(const Args: TUiCommandActionEventArgs);
{ From Draves' Smooth palette Gimp plug-in }
var
  Bitmap: TBitMap;
  JPEG: TJPEGImage;
  pal: TColorMap;
  strings: TStringlist;
  ident, FileName: string;
  len, len_best, as_is, swapd: cardinal;
  cmap_best, original, clist: array[0..255] of cardinal;
  p, total, j, rand, tryit, i0, i1, x, y, i, ii, iw, ih: integer;
  fn, ff, dir, title:string;
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
      ff := Format('%s|*.bmp;*.dib;*.jpg;*.jpeg|%s|*.bmp;*.dib|%s|*.jpg;*.jpeg|%s|*.*',
        [TextByKey('common-filter-allimages'), TextByKey('common-filter-bitmap'),
         TextByKey('common-filter-jpeg'), TextByKey('common-filter-allfiles')]);
      dir := ImageFolder;
      title := TextByKey('common-browse');
      if OpenSaveFileDialog(MainForm, '*.jpg', ff, dir, TextByKey('common-browse'), fn, true, false, false, true) then
      //if OpenDialog.Execute then
      begin
        ImageFolder := ExtractFilePath(fn);
        Application.ProcessMessages;
        len_best := 0;
        if (UpperCase(ExtractFileExt(fn)) = '.BMP')
          or (UpperCase(ExtractFileExt(fn)) = '.DIB') then
          Bitmap.LoadFromFile(fn);
        if (UpperCase(ExtractFileExt(fn)) = '.JPG')
          or (UpperCase(ExtractFileExt(fn)) = '.JPEG') then
        begin
          JPEG.LoadFromFile(fn);
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
        FileName := lowercase(ExtractFileName(fn));
        ident := FileName;
        for ii := 1 to Length(ident) do
        begin
          if ident[ii] = #32 then
            ident[ii] := '_'
          else if ident[ii] = '}' then
            ident[ii] := '_'
          else if ident[ii] = '{' then
            ident[ii] := '_';
        end;
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
        SavePaletteString(Strings.Text, Ident, defSmoothPaletteFile);

        StopPreviewRenderThread;
        PushWorkspaceToUndoStack;
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

procedure TMainForm.ExecuteSetListViewVisibility(const Args: TUiCommandBooleanEventArgs);
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

procedure TMainForm.ExecuteUndo;
begin
  if UndoIndex = UndoStackSize then
    SaveUndoFlame(maincp, Format('%.4d-', [UndoIndex]) + maincp.name,
      GetEnvVarValue('APPDATA') + '\' + undoFilename);
  StopPreviewRenderThread;
  Dec(UndoIndex);
  LoadUndoFlame(UndoIndex, GetEnvVarValue('APPDATA') + '\' + undoFilename);
  SetCanRedo(true);
  if UndoIndex = 0 then begin
    SetCanUndo(false);
  end;
  StatusBar.Panels[3].Text := maincp.name;
end;

procedure TMainForm.OnPreviewPanelMenuUndoClick(Sender: TObject);
begin
  ExecuteUndo(TUiCommandAction.DefaultArgs);
end;

procedure TMainForm.ExecuteRedo;
begin
  StopPreviewRenderThread;
  Inc(UndoIndex);

  assert(UndoIndex <= UndoStackSize, 'Undo list index out of range!');

  LoadUndoFlame(UndoIndex, GetEnvVarValue('APPDATA') + '\' + undoFilename);
  SetCanUndo(true);
  if UndoIndex = UndoStackSize then begin
    SetCanRedo(false);
  end;
  StatusBar.Panels[3].Text := maincp.name;
end;

procedure TMainForm.OnPreviewPanelMenuFullscreenClick(Sender: TObject);
begin
  ExecuteShowFullscreenPreviewWindow(TUiCommandAction.DefaultArgs);
end;

procedure TMainForm.OnPreviewPanelMenuRedoClick(Sender: TObject);
begin
  ExecuteRedo(TUiCommandAction.DefaultArgs);

end;

procedure TMainForm.ExecuteShowFullscreenPreviewWindow;
begin
  FullScreenForm.ActiveForm := Screen.ActiveForm;
  FullScreenForm.Width := Screen.Width;
  FullScreenForm.Height := Screen.Height;
  FullScreenForm.Top := 0;
  FullScreenForm.Left := 0;
  FullScreenForm.cp.Copy(maincp);
  FullScreenForm.cp.cmap := maincp.cmap;
  FullScreenForm.center[0] := CameraCenter[0];
  FullScreenForm.center[1] := CameraCenter[1];
  FullScreenForm.Calculate := True;
  FullScreenForm.Show;
end;

procedure TMainForm.ExecuteRenderSelectedFlame;
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
    RenderForm.Center[0] := CameraCenter[0];
    RenderForm.Center[1] := CameraCenter[1];
    if Assigned(RenderForm.Renderer) then RenderForm.Renderer.WaitFor; // hmm #2
  end;
  RenderForm.Show;
end;

procedure TMainForm.ExecuteRenderBatch;
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
    RenderForm.Center[0] := CameraCenter[0];
    RenderForm.Center[1] := CameraCenter[1];
    if Assigned(RenderForm.Renderer) then RenderForm.Renderer.WaitFor; // hmm #2
  end;
  RenderForm.Show;
end;

procedure TMainForm.ExecuteShowCameraWindow;
begin
  AdjustForm.UpdateDisplay;
  AdjustForm.PageControl.TabIndex := 0;
  AdjustForm.Show;
end;

procedure TMainForm.ExecuteResetSelectedFlameCamera;
var
  scale: double;
  dx, dy, cdx, cdy: double;
  sina, cosa: extended;
begin
  PushWorkspaceToUndoStack;

  scale := MainCP.pixels_per_unit / MainCP.Width * power(2, MainCP.zoom);
  cdx := MainCP.center[0];
  cdy := MainCP.center[1];

  maincp.zoom := 0;
  //maincp.FAngle := 0;
  //maincp.Width := Image.Width;
  //maincp.Height := Image.Height;
  maincp.CalcBoundBox;

  CameraCenter[0] := maincp.center[0];
  CameraCenter[1] := maincp.center[1];

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
  ViewPosition.x := ViewPosition.x - dx * scale * PreviewImage.Width;
  ViewPosition.y := ViewPosition.y - dy * scale * PreviewImage.Width;

  ViewScale := ViewScale * MainCP.pixels_per_unit / MainCP.Width * power(2, MainCP.zoom) / scale;

  DrawImageView;

  PreviewRedrawDelayTimer.enabled := true;
  SendSelectedFlameToToolWindows;
end;

procedure TMainForm.ExecuteShowAboutWindow;
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


procedure TMainForm.ExecuteShowScriptEditorWindow;
begin
end;

procedure TMainForm.ExecuteRunCurrentScript;
begin
end;

procedure TMainForm.ExecuteOpenScriptFromDisk;
begin
end;

procedure TMainForm.ExecuteStopCurrentScript;
begin
end;

procedure TMainForm.ExecuteShowScriptFavoritesWindow;
begin
end;

procedure TMainForm.ExecuteShowCanvasSizeWindow;
begin
//  SizeTool.Show;
  AdjustForm.UpdateDisplay;
  AdjustForm.PageControl.TabIndex:=3;
  AdjustForm.Show;
end;

procedure TMainForm.ClipboardWatcherEventsCallback(Sender: TObject);
var
  flamestr: string;
  isstart, isend: integer;
  flameInClipboard: boolean;
begin
  if GradientInClipboard then
  begin
//    GradientForm.mnuPaste.enabled := true;
//    GradientForm.btnPaste.enabled := true;
    AdjustForm.mnuPaste.enabled := true;
  end
  else
  begin
//    GradientForm.mnuPaste.enabled := false;
//    GradientForm.btnPaste.enabled := false;
    AdjustForm.mnuPaste.enabled := false;
  end;


  flameInClipboard := false;
  if Clipboard.HasFormat(CF_TEXT) then
  begin
    flamestr := Clipboard.AsText;
    isstart := Pos('<flame', flamestr);
    isend := Pos('</flame>', flamestr);
    if (isstart > 0) and (isend > 0) and (isstart < isend) then flameInClipboard := true;
  end;

  SetCanPaste(flameInClipboard);
end;

procedure TMainForm.ExecuteOpenFractalFlamePublication;
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
    CameraDragOffsetY := MainCP.cameraPitch * 180.0 / PI;
    CameraDragOffsetX := MainCP.cameraYaw * 180.0 / PI;

     CameraDragMode := true;
     CameraDragPosition.x := 0;
     CameraDragPosition.y := 0;
     OldCameraDragPosition.x := x;
     OldCameraDragPosition.y := y;
     //SetCaptureControl(TControl(Sender));

    //Screen.Cursor := crNone;
    //GetCursorPos(mousepos); // hmmm
    //mousePos := (Sender as TControl).ClientToScreen(Point(x, y));
    CameraWasDragged := false;
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
        assert(ViewScale <> 0);

        scale := ViewScale * PreviewImage.Width / TransparentPreviewImage.Width;
        ViewPosition.X := ViewPosition.X + (x - CursorStart.Right) / scale;
        ViewPosition.Y := ViewPosition.Y + (y - CursorStart.Bottom) / scale;
        //FClickRect.BottomRight := Point(x, y);

		    DrawImageView;
      end;
    msPitchYaw:
      begin
          if CameraDragMode and ( (x <> OldCameraDragPosition.x) or (y <> OldCameraDragPosition.y) ) then
          begin
            Inc(CameraDragPosition.x, x - OldCameraDragPosition.x);
            Inc(CameraDragPosition.y, y - OldCameraDragPosition.y);

            vx := Round6(CameraDragOffsetX + CameraDragPosition.x / 10);
            vy := Round6(CameraDragOffsetY - CameraDragPosition.y / 10);

            MainCP.cameraPitch := vy * PI / 180.0;
            MainCP.cameraYaw := vx * PI / 180.0;

            vx := Round(vx);
            vy := Round(vy);

            CameraWasDragged := True;
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

        StopPreviewRenderThread;
        PushWorkspaceToUndoStack;
        MainCp.ZoomtoRect(ScaleRect(CursorCurrent, MainCP.Width / PreviewImage.Width));

        ViewScale := ViewScale * PreviewImage.Width / abs(CursorCurrent.Right - CursorCurrent.Left);
        ViewPosition.x := ViewPosition.x - ((CursorCurrent.Right + CursorCurrent.Left) - PreviewImage.Width)/2;
        ViewPosition.y := ViewPosition.y - ((CursorCurrent.Bottom + CursorCurrent.Top) - PreviewImage.Height)/2;
        DrawImageView;

        PreviewRedrawDelayTimer.Enabled := True;
        SendSelectedFlameToToolWindows;
      end;
    msZoomOutWindowMove:
      begin
        DrawZoomWindow;
        CurrentCursorMode := msZoomOutWindow;
        if (abs(CursorCurrent.Left - CursorCurrent.Right) < 10) or
           (abs(CursorCurrent.Top - CursorCurrent.Bottom) < 10) then
          Exit; // zoom to much or double clicked

        StopPreviewRenderThread;
        PushWorkspaceToUndoStack;
        MainCp.ZoomOuttoRect(ScaleRect(CursorCurrent, MainCP.Width / PreviewImage.Width));

        scale := PreviewImage.Width / abs(CursorCurrent.Right - CursorCurrent.Left);
        ViewScale := ViewScale / scale;
        ViewPosition.x := scale * (ViewPosition.x + ((CursorCurrent.Right + CursorCurrent.Left) - PreviewImage.Width)/2);
        ViewPosition.y := scale * (ViewPosition.y + ((CursorCurrent.Bottom + CursorCurrent.Top) - PreviewImage.Height)/2);

        DrawImageView;

        PreviewRedrawDelayTimer.Enabled := True;
        SendSelectedFlameToToolWindows;
      end;
    msDragMove:
      begin
        CursorStart.BottomRight := Point(x, y);
        CurrentCursorMode := msDrag;

        if ((x = 0) and (y = 0)) or // double clicked
           ((CursorStart.left = CursorStart.right) and (CursorStart.top = CursorStart.bottom))
          then Exit;

        StopPreviewRenderThread;
        PushWorkspaceToUndoStack;
        MainCp.MoveRect(ScaleRect(CursorStart, MainCP.Width / PreviewImage.Width));

        PreviewRedrawDelayTimer.Enabled := True;
        SendSelectedFlameToToolWindows;
      end;
    msRotateMove:
      begin
        DrawRotatelines(CursorCurrentAngle);

        CurrentCursorMode := msRotate;

        if (CursorCurrentAngle = 0) then Exit; // double clicked

        StopPreviewRenderThread;
        PushWorkspaceToUndoStack;
        if MainForm_RotationMode = 0 then MainCp.Rotate(CursorCurrentAngle)
        else MainCp.Rotate(-CursorCurrentAngle);

        if assigned(TransparentPreviewImage) then begin
          TransparentPreviewImage.Free;
          TransparentPreviewImage := nil;
          DrawImageView;
        end;

        PreviewRedrawDelayTimer.Enabled := True;
        SendSelectedFlameToToolWindows;
      end;
    msPitchYaw:
      begin
        CameraDragMode := false;
        Screen.Cursor := crDefault;

        if CameraWasDragged then
        begin
          CameraWasDragged := False;
          PreviewRedrawDelayTimer.Enabled := True;
          SendSelectedFlameToToolWindows;
        end;


      end;
  end;
end;

procedure TMainForm.OnPreviewPanelMenuResetLocationClick(Sender: TObject);
begin
  ExecuteResetSelectedFlameCamera(TUiCommandAction.DefaultArgs);
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
    scale := ViewScale * PreviewImage.Width / TransparentPreviewImage.Width;

    r.Left := PreviewImage.Width div 2 + round(scale * (ViewPosition.X - TransparentPreviewImage.Width/2));
    r.Right := PreviewImage.Width div 2 + round(scale * (ViewPosition.X + TransparentPreviewImage.Width/2));
    r.Top := PreviewImage.Height div 2 + round(scale * (ViewPosition.Y - TransparentPreviewImage.Height/2));
    r.Bottom := PreviewImage.Height div 2 + round(scale * (ViewPosition.Y + TransparentPreviewImage.Height/2));

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

    if ssShift in ViewShiftState then
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

procedure TMainForm.ExecuteSetCursorMode(const args : TUiCommandBooleanEventArgs);
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

  PreviewPanelMenuUndoItem.Enabled := value;
  EditForm.mnuUndo.Enabled := value;
  EditForm.tbUndo.enabled := value;
end;

procedure TMainForm.SetCanRedo(value: boolean);
begin
  if Assigned(RedoCommand) then
    RedoCommand.Enabled := value;

  PreviewPanelMenuRedoItem.Enabled := value;
  EditForm.mnuRedo.enabled := value;
  EditForm.tbRedo.enabled := value;

end;

procedure TMainForm.SetCanPaste(value: boolean);
begin
  if Assigned(PasteCommand) then
    PasteCommand.Enabled := value;
end;

procedure TMainForm.ExecuteSetPreviewDensity(const Args: TUiCommandActionEventArgs);
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
  StopPreviewRenderThread;
  PreviewRedrawDelayTimer.Enabled := True;
  SendSelectedFlameToToolWindows;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TMainForm.OnPreviewImageDoubleClick(Sender: TObject);
begin
  if CurrentCursorMode = msRotateMove then
  begin
    StopPreviewRenderThread;
    PushWorkspaceToUndoStack;
    MainCp.FAngle := 0;
    PreviewRedrawDelayTimer.Enabled := True;
    SendSelectedFlameToToolWindows;
  end
  else ExecuteResetSelectedFlameCamera(TUiCommandAction.DefaultArgs);
end;

///////////////////////////////////////////////////////////////////////////////
procedure TMainForm.ExecuteSetShowTransparencyInPreview(const Args: TUiCommandBooleanEventArgs);
var value: boolean;
begin
  if not Assigned(args.Command) then exit;

  value := Args.Checked;
  Args.Command.Checked := value;
  
  ShowTransparency := value;
  DrawImageView;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TMainForm.ExecuteShowTraceWindow;
begin
  TraceForm.Show;
end;

///////////////////////////////////////////////////////////////////////////////
procedure TMainForm.OnFormKeyStateChanged(Sender: TObject; var Key: Word;
  Shift: TShiftState);
var
  MousePos: TPoint;
begin
  if Shift <> ViewShiftState then begin
    if CurrentCursorMode in [msZoomWindowMove, msZoomOutWindowMove, msRotateMove, msDragMove] then
    begin
      // hack: to generate MouseMove event
      GetCursorPos(MousePos);
      SetCursorPos(MousePos.x, MousePos.y);
    end;

    if (CurrentCursorMode in [msZoomWindowMove, msZoomOutWindowMove]) then
    begin
      DrawZoomWindow;
      ViewShiftState := Shift;
      DrawZoomWindow;
    end
    else ViewShiftState := Shift;
  end;
end;

procedure TMainForm.ExecuteSetShowIconsInListView(const Args: TUiCommandBooleanEventArgs);
var 
  value: boolean;
begin
  if not Assigned(args.Command) then exit;

  value := Args.Checked;
  Args.Command.Checked := value;

  ListViewManager.ShowThumbnails := value;
end;

procedure TMainForm.ExecuteCreateAndSelectNewFlame;
var
  i, ci:integer;
begin
  ResetControlPoint(MainCp);
  inc(RandomIndex);
  MainCp.name := RandomPrefix + RandomDate + '-' + IntToStr(RandomIndex);
  ci := Random(256);
  GetCMap(ci, 1, MainCp.cmap);
  MainCp.cmapIndex := ci;

  if AdjustForm.Visible then AdjustForm.UpdateDisplay;
  if EditForm.Visible then EditForm.UpdateDisplay;

  PreviewRedrawDelayTimer.enabled := true;
end;

procedure TMainForm.AutoSaveTimerCallback(Sender: TObject);
var
  filename,title : string;
  Tag, ident: string;
  IFile: TextFile;
  FileList, FileListPre: TStringList;
  i, p: integer;
  erase : boolean;
  bakname: string;
  str: string;
  cpp: TControlPoint;
begin
  erase := false;
  filename := AutoSavePath;

  ident := maincp.name;
  for i := 1 to Length(ident) do
  begin
    if ident[i] = '*' then
      ident[i] := '_'
    else if ident[i] = '"' then
      ident[i] := #39;
  end;

  title := ident + ' (' + FormatDateTime('MM-dd-yyyy hh:mm:ss', Now) + ')';
  Tag := ChangeFileExt(ExtractFileName(filename), '');

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

        cpp := TControlPoint.Create;
        MainCp.Copy(cpp);
        cpp.name := title;
        SaveCpToXmlCompatible(str, cpp);
        cpp.Destroy;

        FileList.Add(Trim(str));
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

      cpp := TControlPoint.Create;
      MainCp.Copy(cpp);
      cpp.name := title;
      SaveCpToXmlCompatible(str, cpp);
      cpp.Destroy;

      Write(IFile, str);
      Writeln(IFile, '</flames>');
      CloseFile(IFile);
    end;
  except on E: EInOutError do
    begin
      //Application.MessageBox('Cannot save file', 'Apophysis', 16);
    end;
  end;
end;

procedure TMainForm.ExecuteOpenHelp;
var
  URL, HelpTopic: string;
begin
  if (HelpPath <> '') then begin
    if (not WinShellOpen(HelpPath)) then begin
      MessageBox(self.Handle, PCHAR(Format(TextByKey('common-genericopenfailure'), [HelpPath])), PCHAR('Apophysis'), MB_ICONHAND);
    end;
  end else MessageBox(self.Handle, PCHAR(TextByKey('main-status-nohelpfile')), PCHAR('Apophysis'), MB_ICONHAND);
end;

procedure TMainForm.ExecuteSetShowGuidelinesInPreview(const Args: TUiCommandBooleanEventArgs);
var value: boolean;
begin
  if not Assigned(args.Command) then exit;

  value := Args.Checked;
  Args.Command.Checked := value;
  
  EnableGuides := value;
  DrawImageView;
end;

procedure TMainForm.ExecuteOpenManual;
begin
  WinShellOpen('http://media.xyrus-worx.org/apophysis-usermanual');
end;

end.
