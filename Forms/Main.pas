unit Main;

interface uses
  Windows,
  Forms,
  Menus,
  Controls,
  ComCtrls,
  Classes,
  Messages,
  ExtCtrls,
  Jpeg,
  SysUtils,
  ClipBrd,
  Graphics,
  Math,
  AppEvnts,
  Registry,
  Global,
  ControlPoint,
  PaletteIO,
  RenderThread,
  PngImage,
  Translation,
  UIRibbonForm,
  UIRibbon,
  UIRibbonCommands,
  ApophysisRibbon,
  FlameListView,
  ParameterIO,
  ApophysisCommandManager;

const
  randFilename = 'Apophysis7X.rand';
  undoFilename = 'Apophysis7X.undo';

type

  TMainForm = class(TUIRibbonForm, ICommandImplementor)
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

    procedure Trace1(const str: string);
    procedure Trace2(const str: string);

    procedure LoadFlameXmlIntoWorkspace(const data: string);
    procedure LoadCpIntoWorkspace(newCp: TControlPoint);
    procedure OpenAdjustFormWithTabIndexSelected(const tabIndex: integer);

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

    procedure OnCommandCanExecuteUpdated(const Command: TUICommand);

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
    ApophysisVersion : string;

    PreviewThreadCount: integer;
    PreviewThreadChangingContext: boolean;
    TransparentPreviewImage: TPngObject;

    IsDrawingSelection: boolean;
    CurrentCursorMode: TCursorMode;
    CursorCurrent: TRect;
    CursorStart: TRect;
    CursorCurrentAngle: double;
    CursorStartAngle: double;

    ViewPosition, OldViewPosition: TSPoint;
    ViewScale: double;
    ViewShiftState: TShiftState;

  public

   // Public fields
    Renderer: TRenderThread;
    ListViewManager: TFlameListView;
    Batch: TBatch;
    FlameInWorkspace: TControlPoint;
    CommandManager: TCommandManager;

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

   // ICommandImplementor
    function AsComponent: TComponent;

    procedure CreateNewFlameInWorkspace;
    procedure OpenBatchFromFile(const fileName: string; selectedIndex: integer);
    procedure LoadLastFlameFromBatchToWorkspace(const fileName: string);
    procedure SaveBatchToFile(const fileName: string);
    function SaveWorkspaceFlameToBatchFile(const title, fileName: string): integer;

    procedure OpenRenderViewForSingleFlameInWorkspace;
    procedure OpenRenderViewForEntireOpenBatch;

    procedure CopyWorkspaceFlameToClipboard;
    procedure ReadFlameFromClipboardIntoWorkspace;

    procedure CreatePaletteFromImageAndApplyToFlameInWorkspace(const fileName: string);

    procedure RevertLastAction;
    procedure CommitLastRevertedAction;

    procedure OpenPreviewInFullscreenMode;
    procedure OpenTransformEditor;
    procedure OpenCameraEditor;
    procedure OpenOutputOptionsEditor;
    procedure OpenPaletteEditor;
    procedure OpenCanvasEditor;
    procedure OpenSettingsEditor;

    procedure ShowHelp;
    procedure ShowInformation;

    procedure OpenFractalFlamePublicationOnline;
    procedure OpenUserManualOnline;
    procedure OpenDonationPoolOnline;

    procedure ExitApophysis;

    function GetCurrentlySelectedFlameName: string;

    procedure SetCursorMode(const mode: TCursorMode);
    procedure SetShowTransparencyInPreview(const enabled: boolean);
    procedure SetShowGuidelinesInPreview(const enabled: boolean);
    procedure SetShowThumbnailsInBatchListView(const enabled: boolean);
    procedure SetPreviewSampleDensityAndUpdate(const density: double);

    procedure UpdateBatchList(selectedIndex: integer);
  end;

var MainForm: TMainForm;

implementation uses
  Editor,
  Options,
  Settings,
  FullScreen,
  FormRender,
  Adjust,
  Browser,
  About,
  RndFlame,
  Tracer,
  PluginVariation;

{$R *.dfm}
{$R 'System\UIRibbon.res'}
{$R 'Ribbon\ApophysisRibbon.res'}
{$I 'delphiversion.pas'}

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
  if assigned(FlameInWorkspace) then
    cp := FlameInWorkspace.Clone
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
  if assigned(FlameInWorkspace) then
    sourceCP := FlameInWorkspace.Clone
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
  StatusBar.Panels[3].Text := FlameInWorkspace.name;
  Application.ProcessMessages;
end;

procedure TMainForm.PushWorkspaceToUndoStack;
begin
  SaveUndoFlame(FlameInWorkspace, Format('%.4d-', [UndoIndex]) + FlameInWorkspace.name, GetEnvVarValue('APPDATA') + '\' + undoFilename);

  Inc(UndoIndex);
  UndoStackSize := UndoIndex;

  CommandManager.SetCanExecuteUndo(true);
  CommandManager.SetCanExecuteRedo(false);
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
    if CurrentCursorMode in [cmRotate, cmZoomIn, cmZoomOut] then
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
    ResetControlPoint(FlameInWorkspace);
    //MainCp.CalcBoundbox;

    FlameInWorkspace.name := RandomPrefix + RandomDate + '-' + IntToStr(RandomIndex);
    ci := Random(256); //Random(NRCMAPS);
    GetIntegratedPaletteByIndex(ci, FlameInWorkspace.cmap);
    FlameInWorkspace.cmapIndex := ci;

    SaveCpToXmlCompatible(str, FlameInWorkspace);
    Write(F, str);

    Write(F, '</random_batch>');
    CloseFile(F);
  except
    on EInOutError do Application.MessageBox(PChar(TextByKey('main-status-batcherror')), PChar('Apophysis'), 16);
  end;
  RandFile := GetEnvVarValue('APPDATA') + '\' + randFilename;
  FlameInWorkspace.name := '';
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
      RandomizeControlPoint(FlameInWorkspace);
      FlameInWorkspace.CalcBoundbox;

(*     Title := RandomPrefix + RandomDate + '-' +
        IntToStr(RandomIndex);
  *)
      FlameInWorkspace.name := RandomPrefix + RandomDate + '-' + IntToStr(RandomIndex);

      SaveCpToXmlCompatible(str, FlameInWorkspace);
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
  FlameInWorkspace.name := '';
end;


procedure TMainForm.LoadLastFlameFromBatchToWorkspace(const fileName: string);
var
  localBatch: TBatch;
begin
  if not FileExists(fileName) then
    raise EFileNotFoundException.Create(fileName);

  localBatch := TBatch.LoadBatch(AutoSavePath);

  if (localBatch.Count = 0) then
  begin
    Application.MessageBox(
      PWideChar('The batch located at "' + fileName + '" does not contain any flame.'),
      PWideChar('Apophysis'),
      MB_ICONERROR);
    exit;
  end;

  PushWorkspaceToUndoStack;
  StopPreviewRenderThread;

  LoadFlameXmlIntoWorkspace(localBatch.GetFlameXmlAt(localBatch.Count - 1));
end;

procedure TMainForm.OpenBatchFromFile(const fileName: string; selectedIndex: integer);
var
  oldSelectedIndex: integer;
begin
  if not FileExists(fileName) then
    raise EFileNotFoundException.Create(fileName);

  ListViewMenuRenameItem.Enabled := True;
  ListViewMenuDeleteItem.Enabled := True;

  if APP_BUILD = '' then
    Caption := ApophysisVersion + ' - ' + openFile
  else
    Caption := ApophysisVersion + ' ' + APP_BUILD + ' - ' + openFile;

  oldSelectedIndex := ListViewManager.SelectedIndex;

  Batch := TBatch.LoadBatch(fileName);
  ListViewManager.Batch := MainForm.Batch;

  if selectedIndex < 0 then
    if oldSelectedIndex < 0 then
      ListViewManager.SelectedIndex := 0
    else
      ListViewManager.SelectedIndex := oldSelectedIndex
  else
    ListViewManager.SelectedIndex := selectedIndex;

  StatusBar.Panels[3].Text := FlameInWorkspace.name;
end;

function TMainForm.SaveWorkspaceFlameToBatchFile(const title, fileName: string): integer;
var
  i, j: integer;
  saveBatch: TBatch;
begin

  if FileExists(fileName) then
    saveBatch := TBatch.LoadBatch(fileName)
  else
    saveBatch := TBatch.CreateBatch(fileName);

  i := -1;
  for j := 0 to saveBatch.Count do
  begin
    if Lowercase(saveBatch.GetFlameNameAt(j)) = Lowercase(FlameInWorkspace.Name) then
    begin
      i := j;
      Break;
    end;
  end;

  FlameInWorkspace.name := title;

  if i >= 0 then
    saveBatch.StoreControlPoint(i, FlameInWorkspace)
  else begin
    saveBatch.AppendControlPoint(FlameInWorkspace);
    i := saveBatch.Count - 1;
  end;

  saveBatch.SaveBatch(fileName);
  saveBatch.Destroy;

  Result := i;
end;

procedure TMainForm.SaveBatchToFile(const fileName: string);
begin
  Batch.StoreControlPoint(ListViewManager.SelectedIndex, FlameInWorkspace);
  Batch.SaveBatch(fileName);
end;

procedure TMainForm.CopyWorkspaceFlameToClipboard;
var
  txt: string;
begin
  SaveCpToXmlCompatible(txt, FlameInWorkspace);
  Clipboard.SetTextBuf(PChar(txt));
  CommandManager.SetCanExecuteReplaceSelectedFlameWithClipboard(true);
end;

procedure TMainForm.ReadFlameFromClipboardIntoWorkspace;
begin
  if not Clipboard.HasFormat(CF_TEXT) then
    Exit;

  LoadFlameXmlIntoWorkspace(Clipboard.AsText);
end;

procedure TMainForm.LoadFlameXmlIntoWorkspace(const data: string);
var
  newCp: TControlPoint;
  backupCp: TControlPoint;

  status: string;

begin
  backupCp := TControlPoint.Create;
  newCp := TControlPoint.Create;
  backupCp.Copy(FlameInWorkspace);

  try
    PushWorkspaceToUndoStack;

    if (ParameterIO.LoadCpFromXmlCompatible(data, newCp, status)) then
    begin
      LoadCpIntoWorkspace(newCp);
    end
    else
    begin
      raise Exception.Create(status);
    end;

  except
    Application.MessageBox(
      PWideChar('Unable to read flame data. Most likely, the data has an invalid format.'),
      PWideChar('Apophysis'),
      MB_ICONERROR);
    FlameInWorkspace.Copy(backupCp);
  end;

  backupCp.Destroy;
  newCp.Destroy;
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

procedure TMainForm.OpenSettingsEditor;
begin
  OptionsForm.ShowModal;
end;

procedure TMainForm.OpenAdjustFormWithTabIndexSelected(const tabIndex: integer);
begin
  AdjustForm.UpdateDisplay;
  AdjustForm.PageControl.TabIndex := tabIndex;
  AdjustForm.Show;
end;

procedure TMainForm.OpenOutputOptionsEditor;
begin
  OpenAdjustFormWithTabIndexSelected(1);
end;

procedure TMainForm.BeginUpdatePreview;
begin
  PreviewRedrawDelayTimer.enabled := false;
  PreviewRedrawDelayTimer.enabled := true;
end;

procedure TMainForm.OpenTransformEditor;
begin
  EditForm.Show;
end;

procedure TMainForm.ExitApophysis;
begin
  Close;
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

function TMainForm.AsComponent: TComponent;
begin
  Result := self;
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
begin
  CommandManager.InitializeCommand(Command);
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

  CommandManager := TCommandManager.Create(self);

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



  CurrentCursorMode := cmPan;
  LimitVibrancy := False;
  Favorites := TStringList.Create;
  Randomize;
  MainSeed := Random(123456789);
  FlameInWorkspace := TControlPoint.Create;
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
  SetShowThumbnailsInBatchListView(true);

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
  FlameInWorkspace.brightness := defBrightness;
  FlameInWorkspace.gamma := defGamma;
  FlameInWorkspace.vibrancy := defVibrancy;
  FlameInWorkspace.sample_density := defSampleDensity;
  FlameInWorkspace.spatial_oversample := defOversample;
  FlameInWorkspace.spatial_filter_radius := defFilterRadius;
  FlameInWorkspace.gammaThreshRelative := defGammaThreshold;
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
    FlameInWorkspace.cmap := DefaultPalette;
  end
  else
  begin
    cmap_index := random(NRCMAPS);
    GetIntegratedPaletteByIndex(cmap_index, FlameInWorkspace.cmap);
    DefaultPalette := FlameInWorkspace.cmap;
  end;

  if FileExists(GetEnvVarValue('APPDATA') + '\' + randFilename) then
    DeleteFile(GetEnvVarValue('APPDATA') + '\' + randFilename);

  //ListView.SetFocus;
  CanDrawOnResize := True;
  Statusbar.Panels[3].Text := FlameInWorkspace.name;
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

    Batch := TBatch.LoadBatch(openFile);
  end else begin
    Batch := TBatch.CreateVolatileBatch;
    CreateNewFlameInWorkspace;
    StopPreviewRenderThread;
    Batch.AppendControlPoint(FlameInWorkspace);
  end;

  ListViewManager.Batch := Batch;
  ListViewManager.SelectedIndex := 0;
  PreviewRedrawDelayTimerCallback(self);
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

  FlameInWorkspace.free;
  Favorites.Free;
end;

procedure TMainForm.OnFormKeyPressed(Sender: TObject; var Key: Char);
var
  scale: double;
begin
  if Key = #27 then begin
    case CurrentCursorMode of
      cmZoomInHot:
        CurrentCursorMode := cmZoomIn;
      cmZoomOutHot:
        CurrentCursorMode := cmZoomOut;
      cmPanHot:
        begin
          CurrentCursorMode := cmPan;

          scale := ViewScale * PreviewImage.Width / TransparentPreviewImage.Width;
          ViewPosition.X := ViewPosition.X - (CursorStart.Right - CursorStart.Left) / scale;
          ViewPosition.Y := ViewPosition.Y - (CursorStart.Bottom - CursorStart.Top) / scale;
        end;
      cmRotateHot:
        CurrentCursorMode := cmRotate;
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
    if FlameInWorkspace = nil then
      Exit;

    if (FlameInWorkspace.Width / FlameInWorkspace.Height) > (pw / ph) then
    begin
      PreviewImage.Width := pw;
      PreviewImage.Height := round(FlameInWorkspace.Height / FlameInWorkspace.Width * pw);
      PreviewImage.Left := 1;
      PreviewImage.Top := (ph - PreviewImage.Height) div 2;
    end
    else begin
      PreviewImage.Height := ph;
      PreviewImage.Width := round(FlameInWorkspace.Width / FlameInWorkspace.Height * ph);
      PreviewImage.Top := 1;
      PreviewImage.Left := (pw - PreviewImage.Width) div 2;
    end;
  end;
end;

procedure TMainForm.OnListViewSelectedItemChanged(index: integer);
var
  cp: TControlPoint;
begin
  if (index >= 0) and (Trim(Batch.GetFlameNameAt(index)) <> Trim(FlameInWorkspace.name)) then
  begin
    StopPreviewRenderThread;

    UndoIndex := 0;
    UndoStackSize := 0;

    CommandManager.SetCanExecuteUndo(false);
    CommandManager.SetCanExecuteRedo(false);

    if fileExists(GetEnvVarValue('APPDATA') + '\' + undoFilename) then
      DeleteFile(GetEnvVarValue('APPDATA') + '\' + undoFilename);

    cp := TControlPoint.Create;
    Batch.LoadControlPoint(index, cp);
    LoadCpIntoWorkspace(cp);
    cp.Destroy;
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
        ParsePaletteTokenString(s, tokens);
        Palette[j][0] := StrToInt(Tokens[0]);
        Palette[j][1] := StrToInt(Tokens[1]);
        Palette[j][2] := StrToInt(Tokens[2]);
        inc(i);
      end;
    end;
    FlameInWorkspace.Clear;
    FlameString := EntryStrings.Text;
    FlameInWorkspace.zoom := 0;
    FlameInWorkspace.center[0] := 0;
    FlameInWorkspace.center[0] := 0;
    FlameInWorkspace.ParseString(FlameString);
    FlameInWorkspace.sample_density := defSampleDensity;
    CameraCenter[0] := FlameInWorkspace.Center[0];
    CameraCenter[1] := FlameInWorkspace.Center[1];
//    cp.CalcBoundbox;
//    MainCP.NormalizeWeights;
    Transforms := FlameInWorkspace.TrianglesFromCP(MainTriangles);
    // Trim undo index from title
    FlameInWorkspace.name := Copy(Fstrings[0], 6, length(Fstrings[0]) - 7);

    if SavedPal then FlameInWorkspace.cmap := palette;
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

procedure TMainForm.PreviewRedrawDelayTimerCallback(Sender: TObject);
var
  GlobalMemoryInfo: TMemoryStatus; // holds the global memory status information
  RenderCP: TControlPoint;
  Mem, ApproxMem: cardinal;
  bs: integer;
begin
  if CurrentCursorMode in [cmPanHot, cmRotateHot, cmZoomInHot, cmZoomOutHot] then exit;

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
    if EditForm.Visible and ((FlameInWorkspace.Width / FlameInWorkspace.Height) <> (EditForm.cp.Width / EditForm.cp.Height))
      then EditForm.UpdateDisplay(true); // preview only?
    if AdjustForm.Visible then AdjustForm.UpdateDisplay(true); // preview only!

    RenderCP := FlameInWorkspace.Clone;
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

procedure TMainForm.OpenPaletteEditor;
begin
  OpenAdjustFormWithTabIndexSelected(2);
end;

procedure TMainForm.CreatePaletteFromImageAndApplyToFlameInWorkspace(const fileName: string);
var
  Bitmap: TBitMap;
  JPEG: TJPEGImage;
  pal: TColorMap;
  strings: TStringlist;
  ident: string;
  len, len_best, as_is, swapd: cardinal;
  cmap_best, original, clist: array[0..255] of cardinal;
  p, total, j, rand, tryit, i0, i1, x, y, i, ii, iw, ih: integer;
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
      ImageFolder := ExtractFilePath(fileName);
      Application.ProcessMessages;
      len_best := 0;
      if (UpperCase(ExtractFileExt(fileName)) = '.BMP')
        or (UpperCase(ExtractFileExt(fileName)) = '.DIB') then
        Bitmap.LoadFromFile(fileName);
      if (UpperCase(ExtractFileExt(fileName)) = '.JPG')
        or (UpperCase(ExtractFileExt(fileName)) = '.JPEG') then
      begin
        JPEG.LoadFromFile(fileName);
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
      ident := lowercase(ExtractFileName(fileName));
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
      FlameInWorkspace.cmap := Pal;
      FlameInWorkspace.cmapindex := -1;
      AdjustForm.UpdateDisplay;

      if EditForm.Visible then EditForm.UpdateDisplay;
      PreviewRedrawDelayTimer.enabled := true;

      StatusBar.SimpleText := '';
    end;
  finally
    Bitmap.Free;
    JPEG.Free;
    strings.Free;
  end;
end;

procedure TMainForm.RevertLastAction;
begin
  if UndoIndex = UndoStackSize then
  begin
    SaveUndoFlame(FlameInWorkspace, Format('%.4d-', [UndoIndex]) + FlameInWorkspace.name, GetEnvVarValue('APPDATA') + '\' + undoFilename);
  end;

  StopPreviewRenderThread;
  Dec(UndoIndex);

  LoadUndoFlame(UndoIndex, GetEnvVarValue('APPDATA') + '\' + undoFilename);
  CommandManager.SetCanExecuteRedo(true);

  if UndoIndex = 0 then begin
    CommandManager.SetCanExecuteUndo(false);
  end;

  StatusBar.Panels[3].Text := FlameInWorkspace.name;
end;

procedure TMainForm.OnPreviewPanelMenuUndoClick(Sender: TObject);
begin
  RevertLastAction;
end;

procedure TMainForm.CommitLastRevertedAction;
begin
  StopPreviewRenderThread;
  Inc(UndoIndex);

  assert(UndoIndex <= UndoStackSize, 'Undo list index out of range!');

  LoadUndoFlame(UndoIndex, GetEnvVarValue('APPDATA') + '\' + undoFilename);
  CommandManager.SetCanExecuteUndo(true);

  if UndoIndex = UndoStackSize then begin
    CommandManager.SetCanExecuteRedo(false);
  end;

  StatusBar.Panels[3].Text := FlameInWorkspace.name;
end;

procedure TMainForm.OnPreviewPanelMenuFullscreenClick(Sender: TObject);
begin
  OpenPreviewInFullscreenMode;
end;

procedure TMainForm.OnPreviewPanelMenuRedoClick(Sender: TObject);
begin
  CommitLastRevertedAction;
end;

procedure TMainForm.OpenPreviewInFullscreenMode;
begin
  FullScreenForm.ActiveForm := Screen.ActiveForm;
  FullScreenForm.Width := Screen.Width;
  FullScreenForm.Height := Screen.Height;
  FullScreenForm.Top := 0;
  FullScreenForm.Left := 0;
  FullScreenForm.cp.Copy(FlameInWorkspace);
  FullScreenForm.cp.cmap := FlameInWorkspace.cmap;
  FullScreenForm.center[0] := CameraCenter[0];
  FullScreenForm.center[1] := CameraCenter[1];
  FullScreenForm.Calculate := True;
  FullScreenForm.Show;
end;

procedure TMainForm.OpenRenderViewForSingleFlameInWorkspace;
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
    if Assigned(RenderForm.Renderer) then RenderForm.Renderer.WaitFor;
    RenderForm.ResetControls;
    RenderForm.PageCtrl.TabIndex := 0;

    case renderFileFormat of
      1: Ext := '.bmp';
      2: Ext := '.png';
      3: Ext := '.jpg';
    end;

    RenderForm.Filename := RenderPath + FlameInWorkspace.name + Ext;
    RenderForm.SaveDialog.FileName := RenderPath + FlameInWorkspace.name + Ext;
    RenderForm.txtFilename.Text := ChangeFileExt(RenderForm.SaveDialog.Filename, Ext);

    RenderForm.cp.Copy(FlameInWorkspace);
    RenderForm.cp.cmap := FlameInWorkspace.cmap;
    RenderForm.zoom := FlameInWorkspace.zoom;
    RenderForm.Center[0] := CameraCenter[0];
    RenderForm.Center[1] := CameraCenter[1];
    if Assigned(RenderForm.Renderer) then RenderForm.Renderer.WaitFor;
  end;

  RenderForm.Show;
end;

procedure TMainForm.OpenRenderViewForEntireOpenBatch;
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
    if Assigned(RenderForm.Renderer) then RenderForm.Renderer.WaitFor;
    RenderForm.ResetControls;
    RenderForm.PageCtrl.TabIndex := 0;

    case renderFileFormat of
      1: Ext := '.bmp';
      2: Ext := '.png';
      3: Ext := '.jpg';
    end;

    RenderForm.bRenderAll := true;
    RenderForm.Filename := RenderPath + FlameInWorkspace.name + Ext;
    RenderForm.SaveDialog.FileName := RenderForm.Filename;
    RenderForm.txtFilename.Text := ChangeFileExt(RenderForm.SaveDialog.Filename, Ext);

    RenderForm.cp.Copy(FlameInWorkspace);
    RenderForm.cp.cmap := FlameInWorkspace.cmap;
    RenderForm.zoom := FlameInWorkspace.zoom;
    RenderForm.Center[0] := CameraCenter[0];
    RenderForm.Center[1] := CameraCenter[1];
    if Assigned(RenderForm.Renderer) then RenderForm.Renderer.WaitFor;
  end;
  RenderForm.Show;
end;

procedure TMainForm.OpenCameraEditor;
begin
  OpenAdjustFormWithTabIndexSelected(0);
end;

procedure TMainForm.ShowInformation;
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


procedure TMainForm.OpenCanvasEditor;
begin
  OpenAdjustFormWithTabIndexSelected(3);
end;

procedure TMainForm.ClipboardWatcherEventsCallback(Sender: TObject);
var
  flamestr: string;
  isstart, isend: integer;
  flameInClipboard: boolean;
begin
  flameInClipboard := false;

  if Clipboard.HasFormat(CF_TEXT) then
  begin
    flamestr := Clipboard.AsText;
    isstart := Pos('<flame', flamestr);
    isend := Pos('</flame>', flamestr);
    if (isstart > 0) and (isend > 0) and (isstart < isend) then flameInClipboard := true;
  end;

  CommandManager.SetCanExecuteReplaceSelectedFlameWithClipboard(flameInClipboard);
end;

procedure TMainForm.OpenFractalFlamePublicationOnline;
begin
  WinShellOpen('http://media.xyrus-worx.org/fractal-flame-publication');
end;

procedure TMainForm.OnPreviewImageMouseDown(Sender: TObject; Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
begin
  if button <> mbLeft then exit;

  CursorStart.TopLeft := Point(x, y);
  CursorStart.BottomRight := CursorStart.TopLeft;

  case CurrentCursorMode of
    cmZoomIn:
      begin
        CursorCurrent.TopLeft := Point(x, y);
        CursorCurrent.BottomRight := Point(x, y);
        DrawZoomWindow;
        CurrentCursorMode := cmZoomInHot;
      end;
    cmZoomOut:
      begin
        CursorCurrent.TopLeft := Point(x, y);
        CursorCurrent.BottomRight := Point(x, y);
        DrawZoomWindow;
        CurrentCursorMode := cmZoomOutHot;
      end;
    cmPan:
      begin
        if not assigned(TransparentPreviewImage) then exit;
        CurrentCursorMode := cmPanHot;
      end;
    cmRotate:
      begin
        CursorStartAngle := arctan2(y-PreviewImage.Height/2, PreviewImage.Width/2-x);
        CursorCurrentAngle := 0;
        DrawRotateLines(CursorCurrentAngle);
        CurrentCursorMode := cmRotateHot;
      end;
  end;
end;

procedure TMainForm.OnPreviewImageMouseMove(Sender: TObject; Shift: TShiftState; X, Y: Integer);
const
  snap_angle = 15*pi/180;
var
  dx, dy, cx, cy, sgn: integer;
  sc, vx, vy, scale: double;
  q : Extended;
begin
  case CurrentCursorMode of
    cmZoomInHot,
    cmZoomOutHot:
      begin
        if IsDrawingSelection then
          DrawZoomWindow;

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
    cmPanHot:
      begin
        assert(assigned(TransparentPreviewImage));
        assert(ViewScale <> 0);

        scale := ViewScale * PreviewImage.Width / TransparentPreviewImage.Width;
        ViewPosition.X := ViewPosition.X + (x - CursorStart.Right) / scale;
        ViewPosition.Y := ViewPosition.Y + (y - CursorStart.Bottom) / scale;

		    DrawImageView;
      end;
    cmRotateHot:
      begin
        if IsDrawingSelection then DrawRotatelines(CursorCurrentAngle);

        CursorCurrentAngle := arctan2(y-PreviewImage.Height/2, PreviewImage.Width/2-x) - CursorStartAngle;
        if ssShift in Shift then
          CursorCurrentAngle := Round(CursorCurrentAngle/snap_angle)*snap_angle;

        DrawRotatelines(CursorCurrentAngle);
        IsDrawingSelection := true;
      end;
  end;
  CursorStart.BottomRight := Point(x, y);
end;

procedure TMainForm.OnPreviewImageMouseUp(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
var
  scale: double;
  rs: TSRect;
begin
  case CurrentCursorMode of
    cmZoomInHot:
      begin
        DrawZoomWindow;
        CurrentCursorMode := cmZoomIn;
        if (abs(CursorCurrent.Left - CursorCurrent.Right) < 10) or
           (abs(CursorCurrent.Top - CursorCurrent.Bottom) < 10) then
          Exit;

        StopPreviewRenderThread;
        PushWorkspaceToUndoStack;
        FlameInWorkspace.ZoomtoRect(ScaleRect(CursorCurrent, FlameInWorkspace.Width / PreviewImage.Width));

        ViewScale := ViewScale * PreviewImage.Width / abs(CursorCurrent.Right - CursorCurrent.Left);
        ViewPosition.x := ViewPosition.x - ((CursorCurrent.Right + CursorCurrent.Left) - PreviewImage.Width)/2;
        ViewPosition.y := ViewPosition.y - ((CursorCurrent.Bottom + CursorCurrent.Top) - PreviewImage.Height)/2;
        DrawImageView;

        PreviewRedrawDelayTimer.Enabled := True;
        SendSelectedFlameToToolWindows;
      end;
    cmZoomOutHot:
      begin
        DrawZoomWindow;
        CurrentCursorMode := cmZoomOut;
        if (abs(CursorCurrent.Left - CursorCurrent.Right) < 10) or
           (abs(CursorCurrent.Top - CursorCurrent.Bottom) < 10) then
          Exit;

        StopPreviewRenderThread;
        PushWorkspaceToUndoStack;
        FlameInWorkspace.ZoomOuttoRect(ScaleRect(CursorCurrent, FlameInWorkspace.Width / PreviewImage.Width));

        scale := PreviewImage.Width / abs(CursorCurrent.Right - CursorCurrent.Left);
        ViewScale := ViewScale / scale;
        ViewPosition.x := scale * (ViewPosition.x + ((CursorCurrent.Right + CursorCurrent.Left) - PreviewImage.Width)/2);
        ViewPosition.y := scale * (ViewPosition.y + ((CursorCurrent.Bottom + CursorCurrent.Top) - PreviewImage.Height)/2);

        DrawImageView;

        PreviewRedrawDelayTimer.Enabled := True;
        SendSelectedFlameToToolWindows;
      end;
    cmPanHot:
      begin
        CursorStart.BottomRight := Point(x, y);
        CurrentCursorMode := cmPan;

        if ((x = 0) and (y = 0)) or ((CursorStart.left = CursorStart.right) and (CursorStart.top = CursorStart.bottom))
          then Exit;

        StopPreviewRenderThread;
        PushWorkspaceToUndoStack;
        FlameInWorkspace.MoveRect(ScaleRect(CursorStart, FlameInWorkspace.Width / PreviewImage.Width));

        PreviewRedrawDelayTimer.Enabled := True;
        SendSelectedFlameToToolWindows;
      end;
    cmRotateHot:
      begin
        DrawRotatelines(CursorCurrentAngle);
        CurrentCursorMode := cmRotate;

        if (CursorCurrentAngle = 0) then Exit;

        StopPreviewRenderThread;
        PushWorkspaceToUndoStack;
        if MainForm_RotationMode = 0 then FlameInWorkspace.Rotate(CursorCurrentAngle)
        else FlameInWorkspace.Rotate(-CursorCurrentAngle);

        if assigned(TransparentPreviewImage) then begin
          TransparentPreviewImage.Free;
          TransparentPreviewImage := nil;
          DrawImageView;
        end;

        PreviewRedrawDelayTimer.Enabled := True;
        SendSelectedFlameToToolWindows;
      end;
  end;
end;

procedure TMainForm.OnPreviewPanelMenuResetLocationClick(Sender: TObject);
var
  scale: double;
  dx, dy, cdx, cdy: double;
  sina, cosa: extended;
begin
  PushWorkspaceToUndoStack;

  scale := FlameInWorkspace.pixels_per_unit / FlameInWorkspace.Width * power(2, FlameInWorkspace.zoom);
  cdx := FlameInWorkspace.center[0];
  cdy := FlameInWorkspace.center[1];

  FlameInWorkspace.zoom := 0;
  FlameInWorkspace.CalcBoundBox;

  CameraCenter[0] := FlameInWorkspace.center[0];
  CameraCenter[1] := FlameInWorkspace.center[1];

  cdx := FlameInWorkspace.center[0] - cdx;
  cdy := FlameInWorkspace.center[1] - cdy;
  Sincos(FlameInWorkspace.FAngle, sina, cosa);
  if IsZero(sina) then begin
    dy := cdy*cosa;
    dx := (cdx)/cosa;
  end
  else begin
    dx := cdy*sina + cdx*cosa;
    dy := (dx*cosa - cdx)/sina;
  end;
  ViewPosition.x := ViewPosition.x - dx * scale * PreviewImage.Width;
  ViewPosition.y := ViewPosition.y - dy * scale * PreviewImage.Width;

  ViewScale := ViewScale * FlameInWorkspace.pixels_per_unit / FlameInWorkspace.Width * power(2, FlameInWorkspace.zoom) / scale;

  DrawImageView;

  PreviewRedrawDelayTimer.enabled := true;
  SendSelectedFlameToToolWindows;
end;

procedure TMainForm.DrawImageView;
var
  i, j: integer;
  bm: TBitmap;
  r: TRect;
  scale: double;
const
  msg = #54;
var
  ok: boolean;
  GlobalMemoryInfo: TMemoryStatus;
  area: int64;
  gridp: integer;
begin
  if FlameInWorkspace = nil then
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
      Brush.Color := FlameInWorkspace.background[0] or (FlameInWorkspace.background[1] shl 8) or (FlameInWorkspace.background[2] shl 16);
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

procedure TMainForm.UpdateBatchList(selectedIndex: integer);
begin
  ListViewManager.Refresh(selectedIndex);
end;

function TMainForm.GetCurrentlySelectedFlameName: string;
begin
  Result := Batch.GetFlameNameAt(ListViewManager.SelectedIndex);
end;

procedure TMainForm.SetCursorMode(const mode: TCursorMode);
begin
  case mode of
    cmPan: SetCursorModePan;
    cmRotate: SetCursorModeRotate;
    cmZoomIn: SetCursorModeZoomIn;
    cmZoomOut: SetCursorModeZoomOut;
  end;
end;

procedure TMainForm.SetCursorModePan;
begin
  CurrentCursorMode := cmPan;
end;

procedure TMainForm.SetCursorModeRotate;
begin
  CurrentCursorMode := cmRotate;
end;

procedure TMainForm.SetCursorModeZoomIn;
begin
  CurrentCursorMode := cmZoomIn;
end;

procedure TMainForm.SetCursorModeZoomOut;
begin
  CurrentCursorMode := cmZoomOut;
end;

procedure TMainForm.OnCommandCanExecuteUpdated(const Command: TUICommand);
begin
  case Command.CommandId of
    UndoCommand_Id:
      begin
        PreviewPanelMenuUndoItem.Enabled := Command.Enabled;
        EditForm.mnuUndo.Enabled := Command.Enabled;
        EditForm.tbUndo.enabled := Command.Enabled;
      end;
    RedoCommand_Id:
      begin
        PreviewPanelMenuRedoItem.Enabled := Command.Enabled;
        EditForm.mnuRedo.Enabled := Command.Enabled;
        EditForm.tbRedo.enabled := Command.Enabled;
      end;
    PasteCommand_Id:
      begin
        AdjustForm.mnuPaste.Enabled := Command.Enabled;
      end;
  end;
end;

procedure TMainForm.SetPreviewSampleDensityAndUpdate(const density: Double);
var value: double;
begin
  defSampleDensity := density;
  StopPreviewRenderThread;
  BeginUpdatePreview;
  SendSelectedFlameToToolWindows;
end;

procedure TMainForm.LoadCpIntoWorkspace(newCp: TControlPoint);
begin
  StopPreviewRenderThread;

  FlameInWorkspace.Copy(newCp);
  Transforms := FlameInWorkspace.TrianglesFromCP(MainTriangles);

  Statusbar.Panels[3].Text := FlameInWorkspace.name;

  FitPreviewImageSize;
  SendSelectedFlameToToolWindows;
  EditForm.SelectedTriangle := 0;

  BeginUpdatePreview;
end;

procedure TMainForm.OnPreviewImageDoubleClick(Sender: TObject);
begin
  if CurrentCursorMode = cmRotate then
  begin
    StopPreviewRenderThread;
    PushWorkspaceToUndoStack;
    FlameInWorkspace.FAngle := 0;
    PreviewRedrawDelayTimer.Enabled := True;
    SendSelectedFlameToToolWindows;
  end
  else OnPreviewPanelMenuResetLocationClick(Sender);
end;

procedure TMainForm.SetShowTransparencyInPreview(const enabled: Boolean);
begin
  ShowTransparency := enabled;
  DrawImageView;
end;

procedure TMainForm.OnFormKeyStateChanged(Sender: TObject; var Key: Word;
  Shift: TShiftState);
var
  MousePos: TPoint;
begin
  if Shift <> ViewShiftState then begin
    if CurrentCursorMode in [cmPanHot, cmRotateHot, cmZoomInHot, cmZoomOutHot] then
    begin
      // -x- amazing what ideas people get to force a mouse move event...
      GetCursorPos(MousePos);
      SetCursorPos(MousePos.x, MousePos.y);
    end;

    if (CurrentCursorMode in [cmZoomInHot, cmZoomOutHot]) then
    begin
      DrawZoomWindow;
      ViewShiftState := Shift;
      DrawZoomWindow;
    end
    else ViewShiftState := Shift;
  end;
end;

procedure TMainForm.SetShowThumbnailsInBatchListView(const enabled: Boolean);
begin
  ListViewManager.ShowThumbnails := enabled;
end;

procedure TMainForm.CreateNewFlameInWorkspace;
var
  i, ci : integer;
  cp: TControlPoint;
  xml: string;
begin
  inc(RandomIndex);

  cp := TControlPoint.Create;

  for i := 0 to Transforms do
    cp.xform[i].Clear;

  cp.name := RandomPrefix + RandomDate + '-' + IntToStr(RandomIndex);

  cp.xform[0].SetVariation(0, 1);
  cp.xform[0].density := 0.5;
  cp.xform[1].symmetry := 1;

  cp.center[0] := 0;
  cp.center[1] := 0;
  cp.zoom := 0;
  cp.pixels_per_unit := cp.Width/4;
  cp.FAngle := 0;

  ci := Random(256);
  cp.cmapIndex := ci;
  GetIntegratedPaletteByIndex(ci, cp.cmap);

  SaveCpToXmlCompatible(xml, cp);
  cp.Destroy;

  LoadFlameXmlIntoWorkspace(xml);
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

  ident := FlameInWorkspace.name;
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
        FlameInWorkspace.Copy(cpp);
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
      FlameInWorkspace.Copy(cpp);
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

procedure TMainForm.ShowHelp;
var
  URL, HelpTopic: string;
begin
  if (HelpPath <> '') then begin
    if (not WinShellOpen(HelpPath)) then begin
      MessageBox(self.Handle, PCHAR(Format(TextByKey('common-genericopenfailure'), [HelpPath])), PCHAR('Apophysis'), MB_ICONHAND);
    end;
  end else MessageBox(self.Handle, PCHAR(TextByKey('main-status-nohelpfile')), PCHAR('Apophysis'), MB_ICONHAND);
end;

procedure TMainForm.SetShowGuidelinesInPreview(const enabled: Boolean);
begin
  EnableGuides := enabled;
  DrawImageView;
end;

procedure TMainForm.OpenUserManualOnline;
begin
  WinShellOpen('http://media.xyrus-worx.org/apophysis-usermanual');
end;

procedure TMainForm.OpenDonationPoolOnline;
begin
  WinShellOpen('http://bit.ly/xwdonate');
end;

end.
