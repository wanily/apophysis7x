unit ApophysisCommandManager;

interface

uses
  Classes,
  SysUtils,
  UiRibbonForm,
  UiRibbonCommands;

type
  TCursorMode = (cmPan, cmRotate, cmZoomIn, cmZoomOut);

  ICommandImplementor = interface
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

  TCommandManager = class

    private

      mImplementor: ICommandImplementor;

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

      procedure UpdateCursorModeCommandStates(const mode: TCursorMode);

    protected

      property Implementor: ICommandImplementor read mImplementor;

      procedure ExecuteUndo(const args : TUiCommandActionEventArgs); virtual;
      procedure ExecuteRedo(const args : TUiCommandActionEventArgs); virtual;

      procedure ExecuteOpenFractalFlamePublication(const args : TUiCommandActionEventArgs); virtual;
      procedure ExecuteOpenManual(const args : TUiCommandActionEventArgs); virtual;
      procedure ExecuteOpenHelp(const args : TUiCommandActionEventArgs); virtual;

      procedure ExecuteSetCursorMode(const args : TUiCommandBooleanEventArgs); virtual;

      procedure ExecuteOpenBatchFromDisk(const args : TUiCommandActionEventArgs); virtual;
      procedure ExecuteLoadLastAutomaticallySavedFlame(const args : TUiCommandActionEventArgs); virtual;
      procedure ExecuteSaveBatchToDisk(const args : TUiCommandActionEventArgs); virtual;
      procedure ExecuteRenderBatch(const args : TUiCommandActionEventArgs); virtual;

      procedure ExecuteCreateAndSelectNewFlame(const args : TUiCommandActionEventArgs); virtual;
      procedure ExecuteSaveSelectedFlameToBatch(const args : TUiCommandActionEventArgs); virtual;
      procedure ExecuteRenderSelectedFlame(const args : TUiCommandActionEventArgs); virtual;
      procedure ExecuteResetSelectedFlameCamera(const args : TUiCommandActionEventArgs); virtual;
      procedure ExecuteCopySelectedFlameToClipboard(const args : TUiCommandActionEventArgs); virtual;
      procedure ExecuteReplaceSelectedFlameWithClipboard(const args : TUiCommandActionEventArgs); virtual;
      procedure ExecuteDeleteSelectedFlame(const args : TUiCommandActionEventArgs); virtual;

      procedure ExecuteCreatePaletteFromImage(const Args: TUiCommandActionEventArgs); virtual;

      procedure ExecuteSetShowGuidelinesInPreview(const Args: TUiCommandBooleanEventArgs); virtual;
      procedure ExecuteSetShowTransparencyInPreview(const Args: TUiCommandBooleanEventArgs); virtual;
      procedure ExecuteSetShowIconsInListView(const Args: TUiCommandBooleanEventArgs); virtual;
      procedure ExecuteSetListViewVisibility(const Args: TUiCommandBooleanEventArgs); virtual;
      procedure ExecuteSetPreviewDensity(const Args: TUiCommandActionEventArgs); virtual;

      procedure ExecuteShowFullscreenPreviewWindow(const args : TUiCommandActionEventArgs); virtual;
      procedure ExecuteShowEditorWindow(const args : TUiCommandActionEventArgs); virtual;
      procedure ExecuteShowCameraWindow(const args : TUiCommandActionEventArgs); virtual;
      procedure ExecuteShowPaletteWindow(const args : TUiCommandActionEventArgs); virtual;
      procedure ExecuteShowOutputPropertiesWindow(const args : TUiCommandActionEventArgs); virtual;
      procedure ExecuteShowCanvasSizeWindow(const args : TUiCommandActionEventArgs); virtual;
      procedure ExecuteShowOptionsWindow(const args : TUiCommandActionEventArgs); virtual;
      procedure ExecuteShowTraceWindow(const args : TUiCommandActionEventArgs); virtual;
      procedure ExecuteShowAboutWindow(const args : TUiCommandActionEventArgs); virtual;

      procedure ExecuteClose(const args : TUiCommandActionEventArgs); virtual;

    public
      CanExecuteUpdated: procedure(const Command: TUICommand) of object;

      constructor Create(implementor: ICommandImplementor);
      destructor Destroy; override;

      procedure InitializeCommand(const Command: TUICommand);

      procedure SetCanExecuteReplaceSelectedFlameWithClipboard(const value: boolean);
      procedure SetCanExecuteUndo(const value: boolean);
      procedure SetCanExecuteRedo(const value: boolean);
  end;

implementation

uses Windows, Dialogs, Forms, Controls, Global, Save, ApophysisRibbon;

constructor TCommandManager.Create(implementor: ICommandImplementor);
begin
  if not Assigned(implementor) then raise EArgumentNilException.Create('implementor');
  mImplementor := implementor;
end;

destructor TCommandManager.Destroy;
begin
  mImplementor := nil;
end;

procedure TCommandManager.InitializeCommand(const Command: TUICommand);
var
  densityCommand: TUiCommandAction;
begin
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

procedure TCommandManager.SetCanExecuteReplaceSelectedFlameWithClipboard(const value: boolean);
begin
  if Assigned(PasteCommand) then
  begin
    PasteCommand.Enabled := value;
    if Assigned(CanExecuteUpdated) then
      CanExecuteUpdated(PasteCommand);
  end;
end;

procedure TCommandManager.SetCanExecuteUndo(const value: boolean);
begin
  if Assigned(UndoCommand) then
  begin
    UndoCommand.Enabled := value;
    if Assigned(CanExecuteUpdated) then
      CanExecuteUpdated(UndoCommand);
  end;
end;

procedure TCommandManager.SetCanExecuteRedo(const value: boolean);
begin
  if Assigned(RedoCommand) then
  begin
    RedoCommand.Enabled := value;
    if Assigned(CanExecuteUpdated) then
      CanExecuteUpdated(RedoCommand);
  end;
end;

procedure TCommandManager.UpdateCursorModeCommandStates(const mode: TCursorMode);
begin
  if Assigned(PanModeCommand) then PanModeCommand.Checked := mode=cmPan;
  if Assigned(RotateModeCommand) then RotateModeCommand.Checked := mode=cmRotate;
  if Assigned(ZoomInModeCommand) then ZoomInModeCommand.Checked := mode=cmZoomIn;
  if Assigned(ZoomOutModeCommand) then ZoomOutModeCommand.Checked := mode=cmZoomOut;
end;

procedure TCommandManager.ExecuteUndo(const args : TUiCommandActionEventArgs);
begin
  Implementor.RevertLastAction;
end;

procedure TCommandManager.ExecuteRedo(const args : TUiCommandActionEventArgs);
begin
  Implementor.CommitLastRevertedAction;
end;

procedure TCommandManager.ExecuteOpenFractalFlamePublication(const args : TUiCommandActionEventArgs);
begin
  Implementor.OpenFractalFlamePublicationOnline;
end;

procedure TCommandManager.ExecuteOpenManual(const args : TUiCommandActionEventArgs);
begin
  Implementor.OpenUserManualOnline;
end;

procedure TCommandManager.ExecuteOpenHelp(const args : TUiCommandActionEventArgs);
begin
  Implementor.ShowHelp;
end;

procedure TCommandManager.ExecuteSetCursorMode(const args : TUiCommandBooleanEventArgs);
var
  mode: TCursorMode;
begin
  if not assigned(args.Command) then exit;

  mode := cmPan; // default

  case args.Command.CommandId of
    PanModeCommand_Id: mode := cmPan;
    RotateModeCommand_Id: mode := cmRotate;
    ZoomInCommand_Id: mode := cmZoomIn;
    ZoomOutCommand_Id: mode := cmZoomOut;
  end;

  Implementor.SetCursorMode(mode);
  UpdateCursorModeCommandStates(mode);
end;

procedure TCommandManager.ExecuteOpenBatchFromDisk(const args : TUiCommandActionEventArgs);
var
  openDialog: TFileOpenDialog;
begin
  openDialog := TFileOpenDialog.Create(Implementor.AsComponent);
  openDialog.DefaultFolder := Global.ParamFolder;

  with openDialog.FileTypes.Add do
  begin
    DisplayName := 'Fractal Flame Batches (*.flame)';
    FileMask := '*.flame';
  end;

  with openDialog.FileTypes.Add do
  begin
    DisplayName := 'All Files (*.*)';
    FileMask := '*.*';
  end;

  openDialog.DefaultExtension := 'flame';

  if (openDialog.Execute) then
  begin
    Implementor.OpenBatchFromFile(openDialog.FileName, 0);
    Global.OpenFile := openDialog.FileName;
    Global.ParamFolder := ExtractFilePath(openDialog.FileName);
    Global.LastOpenFile := openDialog.FileName;
  end;

  openDialog.Destroy;
end;

procedure TCommandManager.ExecuteLoadLastAutomaticallySavedFlame(const args : TUiCommandActionEventArgs);
begin
  if (not FileExists(Global.AutoSavePath)) then
  begin
    Application.MessageBox(
      PWideChar('There is no automatically saved flame available to load.'),
      PWideChar('Apophysis'),
      MB_ICONERROR);
    exit;
  end;

  Implementor.LoadLastFlameFromBatchToWorkspace(Global.AutoSavePath);
end;

procedure TCommandManager.ExecuteSaveBatchToDisk(const args : TUiCommandActionEventArgs);
begin
  SaveForm.SaveType := stSaveAllParameters;
  SaveForm.Filename := Global.SavePath;
  SaveForm.Title := '';

  if SaveForm.ShowModal = mrOK then
  begin
    Global.SavePath := SaveForm.Filename;
    if ExtractFileExt(Global.SavePath) = '' then
      Global.SavePath := Global.SavePath + '.flame';

    Implementor.SaveBatchToFile(SaveForm.FileName);
    Implementor.OpenBatchFromFile(SaveForm.FileName, -1);
  end;
end;

procedure TCommandManager.ExecuteRenderBatch(const args : TUiCommandActionEventArgs);
begin
  // -x- todo
end;

procedure TCommandManager.ExecuteCreateAndSelectNewFlame(const args : TUiCommandActionEventArgs);
begin
  Implementor.CreateNewFlameInWorkspace;
end;

procedure TCommandManager.ExecuteSaveSelectedFlameToBatch(const args : TUiCommandActionEventArgs);
var
  index: integer;
begin
  SaveForm.SaveType := stSaveParameters;
  SaveForm.Filename := Global.SavePath;
  SaveForm.Title := Implementor.GetCurrentlySelectedFlameName;

  if SaveForm.ShowModal = mrOK then
  begin
    Global.SavePath := SaveForm.Filename;
    if ExtractFileExt(Global.SavePath) = '' then
      Global.SavePath := Global.SavePath + '.flame';

    index := Implementor.SaveWorkspaceFlameToBatchFile(SaveForm.Title, SaveForm.FileName);
    Implementor.OpenBatchFromFile(SaveForm.FileName, index);
  end;
end;

procedure TCommandManager.ExecuteRenderSelectedFlame(const args : TUiCommandActionEventArgs);
begin
  // -x- todo
end;

procedure TCommandManager.ExecuteResetSelectedFlameCamera(const args : TUiCommandActionEventArgs);
begin
  // -x- todo
end;

procedure TCommandManager.ExecuteCopySelectedFlameToClipboard(const args : TUiCommandActionEventArgs);
begin
  Implementor.CopyWorkspaceFlameToClipboard;
end;

procedure TCommandManager.ExecuteReplaceSelectedFlameWithClipboard(const args : TUiCommandActionEventArgs);
begin
  Implementor.ReadFlameFromClipboardIntoWorkspace;
end;

procedure TCommandManager.ExecuteDeleteSelectedFlame(const args : TUiCommandActionEventArgs);
begin
  // -x- todo
end;

procedure TCommandManager.ExecuteCreatePaletteFromImage(const Args: TUiCommandActionEventArgs);
var
  openDialog: TFileOpenDialog;
begin
  openDialog := TFileOpenDialog.Create(Implementor.AsComponent);
  openDialog.DefaultFolder := Global.ParamFolder;

  with openDialog.FileTypes.Add do
  begin
    DisplayName := 'Images (*.bmp; *.jpg; *.jpeg; *.dib)';
    FileMask := '*.bmp;*.jpg;*.jpeg;*.dib';
  end;

  with openDialog.FileTypes.Add do
  begin
    DisplayName := 'All Files (*.*)';
    FileMask := '*.*';
  end;

  openDialog.DefaultExtension := 'jpg';

  if (openDialog.Execute) then
  begin
    Implementor.CreatePaletteFromImageAndApplyToFlameInWorkspace(openDialog.FileName);
  end;

  openDialog.Destroy;

end;

procedure TCommandManager.ExecuteSetShowGuidelinesInPreview(const Args: TUiCommandBooleanEventArgs);
begin
  Global.EnableGuides := Args.Checked;
  Implementor.SetShowGuidelinesInPreview(Args.Checked);
end;

procedure TCommandManager.ExecuteSetShowTransparencyInPreview(const Args: TUiCommandBooleanEventArgs);
begin
  Global.ShowTransparency := Args.Checked;
  Implementor.SetShowTransparencyInPreview(Args.Checked);
end;

procedure TCommandManager.ExecuteSetShowIconsInListView(const Args: TUiCommandBooleanEventArgs);
begin
  Implementor.SetShowThumbnailsInBatchListView(Args.Checked);
end;

procedure TCommandManager.ExecuteSetListViewVisibility(const Args: TUiCommandBooleanEventArgs);
begin
  // -x- todo
end;

procedure TCommandManager.ExecuteSetPreviewDensity(const Args: TUiCommandActionEventArgs);
var
  value: double;
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

  Global.defSampleDensity := value;
  Implementor.SetPreviewSampleDensityAndUpdate(value);
end;

procedure TCommandManager.ExecuteShowFullscreenPreviewWindow(const args : TUiCommandActionEventArgs);
begin
  Implementor.OpenPreviewInFullscreenMode;
end;

procedure TCommandManager.ExecuteShowEditorWindow(const args : TUiCommandActionEventArgs);
begin
  Implementor.OpenTransformEditor;
end;

procedure TCommandManager.ExecuteShowCameraWindow(const args : TUiCommandActionEventArgs);
begin
  Implementor.OpenCameraEditor;
end;

procedure TCommandManager.ExecuteShowPaletteWindow(const args : TUiCommandActionEventArgs);
begin
  Implementor.OpenPaletteEditor;
end;

procedure TCommandManager.ExecuteShowOutputPropertiesWindow(const args : TUiCommandActionEventArgs);
begin
  Implementor.OpenOutputOptionsEditor;
end;

procedure TCommandManager.ExecuteShowCanvasSizeWindow(const args : TUiCommandActionEventArgs);
begin
  Implementor.OpenCanvasEditor;
end;

procedure TCommandManager.ExecuteShowOptionsWindow(const args : TUiCommandActionEventArgs);
begin
  Implementor.OpenSettingsEditor;
end;

procedure TCommandManager.ExecuteShowTraceWindow(const args : TUiCommandActionEventArgs);
begin
  // -x- todo
end;

procedure TCommandManager.ExecuteShowAboutWindow(const args : TUiCommandActionEventArgs);
begin
  Implementor.ShowInformation;
end;

procedure TCommandManager.ExecuteClose(const args : TUiCommandActionEventArgs);
begin
  Implementor.ExitApophysis;
end;

end.
