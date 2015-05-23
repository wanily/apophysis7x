unit Settings;

interface

uses graphics, Messages, Translation;

function ReadPluginDir: string;
procedure ReadSettings;
procedure SaveSettings;

implementation

uses Windows, Classes, SysUtils, StrUtils, Forms, Registry, Global, Dialogs,
  XFormMan;

function ReadPluginDir: string;
var
  settingFileName: string;
  sl: TStringList;
begin
  sl := TStringList.Create;

  settingFileName := ExtractFilePath(Application.ExeName) + 'ApoPluginSrc.dat';
  if FileExists(settingFileName) then
    sl.LoadFromFile(settingFileName)
  else
  begin
    settingFileName := GetEnvVarValue('APPDATA') + '\ApoPluginSrc.dat';

    if FileExists(settingFileName) then
      sl.LoadFromFile(settingFileName)
    else
      sl.Text := ExtractFilePath(Application.ExeName) + 'Plugins\';
  end;

  if Trim(sl.Text) = '' then
    sl.Text := ExtractFilePath(Application.ExeName) + 'Plugins\';

  Result := Trim(sl.Text);
  if (RightStr(Result, 1) <> '\') and (Result <> '') then
    Result := Result + '\';

  sl.Destroy;
end;

procedure SavePluginDir(data: string);
var
  settingFileName: string;
  sl: TStringList;
begin
  settingFileName := ExtractFilePath(Application.ExeName) + 'ApoPluginSrc.dat';
  sl := TStringList.Create;
  sl.Text := PluginPath;

  try
    sl.SaveToFile(settingFileName);
    sl.Destroy;
  except
    // not elevated?
    settingFileName := GetEnvVarValue('APPDATA') + '\ApoPluginSrc.dat';
    try
      sl.SaveToFile(settingFileName);
    except
      MessageBox(0, PCHAR(TextByKey('main-status-pluginpath-ioerror')),
        PCHAR('Apophysis'), MB_ICONWARNING);
    end;
    sl.Destroy;
  end;
end;

procedure ReadSettings;
var
  Registry: TRegistry;
  DefaultPath: string;
  i, maxVars: integer;
  VariationOptions: int64;
begin
  DefaultPath := GetEnvVarValue('USERPROFILE');
  /// ExtractFilePath(Application.Exename);
  Registry := TRegistry.Create;
  try
    Registry.RootKey := HKEY_CURRENT_USER;
    { Defaults }
    if Registry.OpenKey('Software\' + APP_NAME + '\Defaults', False) then
    begin
      if Registry.ValueExists('DefaultFlameFile3D') then
      begin
        defFlameFile := Registry.ReadString('DefaultFlameFile3D');
      end
      else
      begin
        if Registry.ValueExists('DefaultFlameFile') then
          defFlameFile := Registry.ReadString('DefaultFlameFile')
        else
          defFlameFile := '';
      end;

      if Registry.ValueExists('AlwaysCreateBlankFlame') then
      begin
        AlwaysCreateBlankFlame := Registry.ReadBool('AlwaysCreateBlankFlame');
      end
      else
      begin
        AlwaysCreateBlankFlame := False;
      end;

      if Registry.ValueExists('GradientFile') then
      begin
        GradientFile := Registry.ReadString('GradientFile');
      end
      else
      begin
        GradientFile := ''
      end;

      if Registry.ValueExists('SavePath3D') then
      begin
        SavePath := Registry.ReadString('SavePath3D');
      end
      else
      begin
        if Registry.ValueExists('SavePath') then
          SavePath := Registry.ReadString('SavePath')
        else
          SavePath := DefaultPath + '\Flames.flame';
      end;

      if Registry.ValueExists('WarnOnMissingPlugin') then
      begin
        WarnOnMissingPlugin := Registry.ReadBool('WarnOnMissingPlugin');
      end
      else
        WarnOnMissingPlugin := true;

      if Registry.ValueExists('MultithreadedPreview') then
      begin
        MultithreadedPreview := Registry.ReadBool('MultithreadedPreview');
      end
      else
        MultithreadedPreview := False;

      if Registry.ValueExists('LanguageFile') then
      begin
        LanguageFile := Registry.ReadString('LanguageFile');
      end
      else
        LanguageFile := '';

      if Registry.ValueExists('SmoothPaletteFile') then
      begin
        defSmoothPaletteFile := Registry.ReadString('SmoothPaletteFIle');
      end
      else
      begin
        defSmoothPaletteFile := DefaultPath + '\SmoothPalette.ugr';
      end;

      if Registry.ValueExists('PlaySoundOnRenderComplete') then
        PlaySoundOnRenderComplete :=
          Registry.ReadBool('PlaySoundOnRenderComplete')
      else
        PlaySoundOnRenderComplete := False;
      if Registry.ValueExists('RenderCompleteSoundFile') then
        RenderCompleteSoundFile :=
          Registry.ReadString('RenderCompleteSoundFile')
      else
        RenderCompleteSoundFile := '';

      if Registry.ValueExists('ConfirmDelete') then
        ConfirmDelete := Registry.ReadBool('ConfirmDelete')
      else
        ConfirmDelete := true;
      if Registry.ValueExists('ConfirmExit') then
        ConfirmExit := Registry.ReadBool('ConfirmExit')
      else
        ConfirmExit := true;

      if Registry.ValueExists('PreserveQuality') then
      begin
        PreserveQuality := Registry.ReadBool('PreserveQuality');
      end
      else
      begin
        PreserveQuality := true;
      end;

      if Registry.ValueExists('KeepBackground') then
      begin
        KeepBackground := Registry.ReadBool('KeepBackground');
      end
      else
      begin
        KeepBackground := False;
      end;

      if Registry.ValueExists('MinTransforms') then
      begin
        randMinTransforms := Registry.ReadInteger('MinTransforms');
        if randMinTransforms <= 0 then
          randMinTransforms := 2;
      end
      else
      begin
        randMinTransforms := 2;
      end;
      if Registry.ValueExists('MaxTransforms') then
      begin
        randMaxTransforms := Registry.ReadInteger('MaxTransforms');
        if randMaxTransforms < randMinTransforms then
          randMaxTransforms := randMinTransforms;
      end
      else
      begin
        randMaxTransforms := randMinTransforms + 1;
      end;

      if Registry.ValueExists('MutationMinTransforms') then
      begin
        mutantMinTransforms := Registry.ReadInteger('MutationMinTransforms');
        if mutantMinTransforms <= 0 then
          mutantMinTransforms := 2;
      end
      else
      begin
        mutantMinTransforms := 2;
      end;
      if Registry.ValueExists('MutationMaxTransforms') then
      begin
        mutantMaxTransforms := Registry.ReadInteger('MutationMaxTransforms');
        if mutantMaxTransforms < mutantMinTransforms then
          mutantMinTransforms := mutantMinTransforms;
      end
      else
      begin
        mutantMaxTransforms := mutantMinTransforms + 1;
      end;

      if Registry.ValueExists('ParameterFolder3D') then
      begin
        ParamFolder := Registry.ReadString('ParameterFolder3D');
      end
      else if Registry.ValueExists('ParameterFolder') then
      begin
        ParamFolder := Registry.ReadString('ParameterFolder');
      end
      else
      begin
        ParamFolder := DefaultPath + '\';
      end;

      if Registry.ValueExists('UPRPath') then

        if Registry.ValueExists('BrowserPath') then
        begin
          BrowserPath := Registry.ReadString('BrowserPath');
        end
        else
        begin
          BrowserPath := DefaultPath + '\';
        end;
      if Registry.ValueExists('EditPreviewQaulity') then
      begin
        EditPrevQual := Registry.ReadInteger('EditPreviewQaulity');
      end
      else
      begin
        EditPrevQual := 1;
      end;
      if Registry.ValueExists('MutatePreviewQaulity') then
      begin
        MutatePrevQual := Registry.ReadInteger('MutatePreviewQaulity');
        if MutatePrevQual <= 0 then
          MutatePrevQual := 1;
      end
      else
      begin
        MutatePrevQual := 1;
      end;
      if Registry.ValueExists('AdjustPreviewQaulity') then
      begin
        AdjustPrevQual := Registry.ReadInteger('AdjustPreviewQaulity');
        if AdjustPrevQual <= 0 then
          AdjustPrevQual := 1;
      end
      else
      begin
        AdjustPrevQual := 1;
      end;
      if Registry.ValueExists('RandomPrefix') then
      begin
        RandomPrefix := Registry.ReadString('RandomPrefix');
      end
      else
      begin
        RandomPrefix := 'Apo3D-'
      end;
      if Registry.ValueExists('RandomDate') then
      begin
        RandomDate := Registry.ReadString('RandomDate');
      end
      else
      begin
        RandomDate := ''
      end;
      if Registry.ValueExists('RandomIndex') then
      begin
        RandomIndex := Registry.ReadInteger('RandomIndex');
      end
      else
      begin
        RandomIndex := 0;
      end;
      if Registry.ValueExists('SymmetryType') then
      begin
        SymmetryType := Registry.ReadInteger('SymmetryType');
      end
      else
      begin
        SymmetryType := 0;
      end;
      if Registry.ValueExists('SymmetryOrder') then
      begin
        SymmetryOrder := Registry.ReadInteger('SymmetryOrder');
      end
      else
      begin
        SymmetryOrder := 4;
      end;
      if Registry.ValueExists('SymmetryNVars') then
      begin
        SymmetryNVars := Registry.ReadInteger('SymmetryNVars');
      end
      else
      begin
        SymmetryNVars := 12;
      end;

      if Registry.ValueExists('VariationOptions') then
      begin
        VariationOptions := Registry.ReadInteger('VariationOptions');
      end
      else
      begin
        VariationOptions := 262143;
      end;
      if Registry.ValueExists('VariationOptions2') then
      begin
        VariationOptions := VariationOptions or
          (int64(Registry.ReadInteger('VariationOptions2')) shl 32);
      end;

      if Registry.ValueExists('RotationMode') then
        MainForm_RotationMode := Registry.ReadInteger('RotationMode')
      else
        MainForm_RotationMode := 0;

      if Registry.ValueExists('ScriptPath') then
      begin
        ScriptPath := Registry.ReadString('ScriptPath');
      end
      else
      begin
        ScriptPath := DefaultPath + '\Scripts\';
      end;
      if Registry.ValueExists('FunctionLibrary') then
      begin
        defLibrary := Registry.ReadString('FunctionLibrary');
      end
      else
      begin
        defLibrary := ExtractFilePath(Application.ExeName) + 'Functions.asc';
      end;

      if Registry.ValueExists('ShowProgress') then
      begin
        ShowProgress := Registry.ReadBool('ShowProgress');
      end
      else
      begin
        ShowProgress := true;
      end;
      if Registry.ValueExists('ToolBarWidth1') then
      begin
        TBWidth1 := Registry.ReadInteger('ToolBarWidth1');
      end
      else
      begin
        TBWidth1 := 0;
      end;
      if Registry.ValueExists('ToolBarWidth2') then
      begin
        TBWidth2 := Registry.ReadInteger('ToolBarWidth2');
      end
      else
      begin
        TBWidth2 := 0;
      end;
      if Registry.ValueExists('ToolBarWidth3') then
      begin
        TBWidth3 := Registry.ReadInteger('ToolBarWidth3');
      end
      else
      begin
        TBWidth3 := 0;
      end;
      if Registry.ValueExists('ToolBarWidth4') then
      begin
        TBWidth4 := Registry.ReadInteger('ToolBarWidth4');
      end
      else
      begin
        TBWidth4 := 0;
      end;
      if Registry.ValueExists('ToolBarWidth5') then
      begin
        TBWidth5 := Registry.ReadInteger('ToolBarWidth5');
      end
      else
      begin
        TBWidth5 := 0;
      end;

      if Registry.ValueExists('LineCenterColor') then
      begin
        LineCenterColor := Registry.ReadInteger('LineCenterColor');
      end
      else
      begin
        LineCenterColor := $FFFFFF;
      end;
      if Registry.ValueExists('LineThirdsColor') then
      begin
        LineThirdsColor := Registry.ReadInteger('LineThirdsColor');
      end
      else
      begin
        LineThirdsColor := $0000FF;
      end;
      if Registry.ValueExists('LineGRColor') then
      begin
        LineGRColor := Registry.ReadInteger('LineGRColor');
      end
      else
      begin
        LineGRColor := $00FF00;
      end;
      if Registry.ValueExists('EnableGuides') then
      begin
        EnableGuides := Registry.ReadBool('EnableGuides');
      end
      else
      begin
        EnableGuides := False;
      end;

      { FormRender }
      if Registry.ValueExists('SaveIncompleteRenders') then
      begin
        SaveIncompleteRenders := Registry.ReadBool('SaveIncompleteRenders');
      end
      else
      begin
        SaveIncompleteRenders := False;
      end;
      if Registry.ValueExists('ShowRenderStats') then
      begin
        ShowRenderStats := Registry.ReadBool('ShowRenderStats');
      end
      else
      begin
        ShowRenderStats := False;
      end;
      if Registry.ValueExists('LowerRenderPriority') then
      begin
        LowerRenderPriority := Registry.ReadBool('LowerRenderPriority');
      end
      else
      begin
        LowerRenderPriority := False;
      end;

      if Registry.ValueExists('PNGTransparency') then
      begin
        PNGTransparency := Registry.ReadInteger('PNGTransparency');

        if PNGTransparency > 1 then
          PNGTransparency := 1; // tmp

      end
      else
      begin
        PNGTransparency := 1
      end;
      if Registry.ValueExists('ShowTransparency') then
      begin
        ShowTransparency := Registry.ReadBool('ShowTransparency');
      end
      else
      begin
        ShowTransparency := False;
      end;
      if Registry.ValueExists('ExtendMainPreview') then
      begin
        ExtendMainPreview := Registry.ReadBool('ExtendMainPreview');
      end
      else
      begin
        ExtendMainPreview := true;
      end;
      if Registry.ValueExists('MainPreviewScale') then
      begin
        MainPreviewScale := Registry.ReadFloat('MainPreviewScale');
        if MainPreviewScale < 1 then
          MainPreviewScale := 1
        else if MainPreviewScale > 3 then
          MainPreviewScale := 3;
      end
      else
      begin
        MainPreviewScale := 1.2;
      end;

      if Registry.ValueExists('NrTreads') then
      begin
        NrTreads := Registry.ReadInteger('NrTreads');
        if NrTreads <= 0 then
          NrTreads := 1;
      end
      else
      begin
        NrTreads := 1;
      end;
      if Registry.ValueExists('UseNrThreads') then
      begin
        UseNrThreads := Registry.ReadInteger('UseNrThreads');
        if UseNrThreads <= 0 then
          UseNrThreads := 1;
      end
      else
      begin
        UseNrThreads := 1;
      end;

      if Registry.ValueExists('InternalBitsPerSample') then
      begin
        InternalBitsPerSample := Registry.ReadInteger('InternalBitsPerSample');
      end
      else
      begin
        InternalBitsPerSample := 0;
      end;

      if Registry.ValueExists('AutoOpenLog') then
      begin
        AutoOpenLog := Registry.ReadBool('AutoOpenLog');
      end
      else
      begin
        AutoOpenLog := False;
      end;

      if Registry.ValueExists('StartupCheckForUpdates') then
      begin
        StartupCheckForUpdates := Registry.ReadBool('StartupCheckForUpdates');
      end
      else
      begin
        StartupCheckForUpdates := true;
      end;

      if Registry.ValueExists('LastOpenFile') then
      begin
        LastOpenFile := Registry.ReadString('LastOpenFile');
      end
      else
      begin
        LastOpenFile := '';
      end;

      if Registry.ValueExists('RememberLastOpenFile') then
      begin
        RememberLastOpenFile := Registry.ReadBool('RememberLastOpenFile');
      end
      else
      begin
        RememberLastOpenFile := False;
      end;

      if Registry.ValueExists('UseSmallThumbnails') then
      begin
        UseSmallThumbnails := Registry.ReadBool('UseSmallThumbnails');
      end
      else
      begin
        UseSmallThumbnails := true;
      end;

    end
    else
    begin
      StartupCheckForUpdates := true;
      AlwaysCreateBlankFlame := False;
      MainForm_RotationMode := 0;
      EditPrevQual := 1;
      MutatePrevQual := 1;
      AdjustPrevQual := 1;
      GradientFile := '';
      defFlameFile := '';
      SavePath := DefaultPath + '\Flames.flame';
      WarnOnMissingPlugin := true;
      MultithreadedPreview := False;
      LanguageFile := '';
      defSmoothPaletteFile := DefaultPath + '\SmoothPalette.ugr';
      ConfirmDelete := true;
      ConfirmExit := true;
      randMinTransforms := 2;
      randMaxTransforms := 3;
      mutantMinTransforms := 2;
      mutantMaxTransforms := 6;
      PreserveQuality := False;
      KeepBackground := False;
      ParamFolder := DefaultPath + '\';
      RandomPrefix := 'Apo7X-';
      RandomIndex := 0;
      RandomDate := '';
      SymmetryType := 0;
      SymmetryOrder := 4;
      SymmetryNVars := 12;
      VariationOptions := 262143;
      ScriptPath := DefaultPath + '\';
      defLibrary := ExtractFilePath(Application.ExeName) + 'Functions.asc';
      ShowProgress := true;
      SaveIncompleteRenders := False;
      LowerRenderPriority := False;
      ShowRenderStats := False;
      PNGTransparency := 1;
      ShowTransparency := False;
      MainPreviewScale := 1.2;
      ExtendMainPreview := true;
      NrTreads := 1;
      UseNrThreads := 1;
      InternalBitsPerSample := 0;
      AutoOpenLog := False;
      LastOpenFile := '';
      RememberLastOpenFile := False;
      UseSmallThumbnails := true;
      TBWidth1 := 0;
      TBWidth2 := 0;
      TBWidth3 := 0;
      TBWidth4 := 0;
      TBWidth5 := 0;
      LineCenterColor := $FFFFFF;
      LineThirdsColor := $0000FF;
      LineGRColor := $00FF00;
      EnableGuides := False;
    end;
    Registry.CloseKey;

    SetLength(Variations, NRVAR);
    if Registry.OpenKey('Software\' + APP_NAME + '\Variations', False) then
    begin
      for i := 0 to NRVAR - 1 do
      begin
        if Registry.ValueExists(Varnames(i)) then
          Variations[i] := Registry.ReadBool(Varnames(i))
        else
          Variations[i] := False;
      end;
    end
    else
    begin
      if NRVAR >= 64 then
        maxVars := 63
      else
        maxVars := NRVAR - 1;
      for i := 0 to maxVars do
        Variations[i] := boolean(VariationOptions shr i and 1);
    end;
    Registry.CloseKey;

    { Editor } // --Z-- moved from EditForm
    if Registry.OpenKey('Software\' + APP_NAME + '\Forms\Editor', False) then
    begin
      if Registry.ValueExists('UseTransformColors') then
        UseTransformColors := Registry.ReadBool('UseTransformColors')
      else
        UseTransformColors := False;
      if Registry.ValueExists('HelpersEnabled') then
        HelpersEnabled := Registry.ReadBool('HelpersEnabled')
      else
        HelpersEnabled := true;
      if Registry.ValueExists('ShowAllXforms') then
        ShowAllXforms := Registry.ReadBool('ShowAllXforms')
      else
        ShowAllXforms := true;
      if Registry.ValueExists('EnableEditorPreview') then
        EnableEditorPreview := Registry.ReadBool('EnableEditorPreview')
      else
        EnableEditorPreview := False;
      if Registry.ValueExists('EditorPreviewTransparency') then
        EditorPreviewTransparency :=
          Registry.ReadInteger('EditorPreviewTransparency')
      else
        EditorPreviewTransparency := 192;

      if Registry.ValueExists('BackgroundColor') then
        EditorBkgColor := Registry.ReadInteger('BackgroundColor')
      else
        EditorBkgColor := integer(clBlack);
      if Registry.ValueExists('GridColor1') then
        GridColor1 := Registry.ReadInteger('GridColor1')
      else
        GridColor1 := $444444;
      if Registry.ValueExists('GridColor2') then
        GridColor2 := Registry.ReadInteger('GridColor2')
      else
        GridColor2 := $333333;
      if Registry.ValueExists('HelpersColor') then
        HelpersColor := Registry.ReadInteger('HelpersColor')
      else
        HelpersColor := $808080;
      if Registry.ValueExists('ReferenceTriangleColor') then
        ReferenceTriangleColor := Registry.ReadInteger('ReferenceTriangleColor')
      else
        ReferenceTriangleColor := $7F7F7F;
      if Registry.ValueExists('ExtendedEdit') then
        ExtEditEnabled := Registry.ReadBool('ExtendedEdit')
      else
        ExtEditEnabled := true;
      if Registry.ValueExists('LockTransformAxis') then
        TransformAxisLock := Registry.ReadBool('LockTransformAxis')
      else
        TransformAxisLock := true;
      if Registry.ValueExists('RebuildXaosLinks') then
        RebuildXaosLinks := Registry.ReadBool('RebuildXaosLinks')
      else
        RebuildXaosLinks := true;
    end
    else
    begin
      UseTransformColors := False;
      HelpersEnabled := true;
      ShowAllXforms := true;
      EnableEditorPreview := False;
      EditorPreviewTransparency := 192;
      EditorBkgColor := $000000;
      GridColor1 := $444444;
      GridColor2 := $333333;
      HelpersColor := $808080;
      ReferenceTriangleColor := integer(clGray);
      ExtEditEnabled := true;
      TransformAxisLock := true;
      RebuildXaosLinks := true;
    end;
    Registry.CloseKey;

    { Render }
    if Registry.OpenKey('Software\' + APP_NAME + '\Render', False) then
    begin
      if Registry.ValueExists('Path') then
      begin
        RenderPath := Registry.ReadString('Path');
      end
      else
      begin
        RenderPath := DefaultPath + '\';
      end;
      if Registry.ValueExists('SampleDensity') then
      begin
        renderDensity := Registry.ReadFloat('SampleDensity');
      end
      else
      begin
        renderDensity := 200;
      end;
      if Registry.ValueExists('FilterRadius') then
      begin
        renderFilterRadius := Registry.ReadFloat('FilterRadius');
      end
      else
      begin
        renderFilterRadius := 0.4;
      end;
      if Registry.ValueExists('Oversample') then
      begin
        renderOversample := Registry.ReadInteger('Oversample');
      end
      else
      begin
        renderOversample := 2;
      end;
      if Registry.ValueExists('Width') then
      begin
        renderWidth := Registry.ReadInteger('Width');
      end
      else
      begin
        renderWidth := 1024;
      end;
      if Registry.ValueExists('Height') then
      begin
        renderHeight := Registry.ReadInteger('Height');
      end
      else
      begin
        renderHeight := 768;
      end;
      if Registry.ValueExists('JPEGQuality') then
      begin
        JPEGQuality := Registry.ReadInteger('JPEGQuality');
      end
      else
      begin
        JPEGQuality := 100;
      end;
      if Registry.ValueExists('FileFormat') then
      begin
        renderFileFormat := Registry.ReadInteger('FileFormat');
      end
      else
      begin
        renderFileFormat := 3;
      end;
      if Registry.ValueExists('BitsPerSample') then
      begin
        renderBitsPerSample := Registry.ReadInteger('BitsPerSample');
      end
      else
      begin
        renderBitsPerSample := 0;
      end;

      if Registry.ValueExists('StoreEXIF') then
      begin
        StoreEXIF := Registry.ReadBool('StoreEXIF');
      end
      else
      begin
        StoreEXIF := False;
      end;
      if Registry.ValueExists('StoreParamsEXIF') then
      begin
        StoreParamsEXIF := Registry.ReadBool('StoreParamsEXIF');
      end
      else
      begin
        StoreParamsEXIF := False;
      end;
      if Registry.ValueExists('ExifAuthor') then
      begin
        ExifAuthor := Registry.ReadString('ExifAuthor');
      end
      else
      begin
        ExifAuthor := '';
      end;

    end
    else
    begin
      renderFileFormat := 2;
      JPEGQuality := 100;
      RenderPath := DefaultPath + '\';
      renderDensity := 200;
      renderOversample := 2;
      renderFilterRadius := 0.4;
      renderWidth := 1024;
      renderHeight := 768;
      renderBitsPerSample := 0;
      StoreEXIF := False;
      ExifAuthor := '';
      StoreParamsEXIF := False;
    end;
    Registry.CloseKey;

    if Registry.OpenKey('Software\' + APP_NAME + '\Display', False) then
    begin
      if Registry.ValueExists('SampleDensity') then
      begin
        defSampleDensity := Registry.ReadFloat('SampleDensity');
      end
      else
      begin
        defSampleDensity := 5;
      end;
      if Registry.ValueExists('Gamma') then
      begin
        defGamma := Registry.ReadFloat('Gamma');
      end
      else
      begin
        defGamma := 4;
      end;
      if Registry.ValueExists('Brightness') then
      begin
        defBrightness := Registry.ReadFloat('Brightness');
      end
      else
      begin
        defBrightness := 4;
      end;
      if Registry.ValueExists('Vibrancy') then
      begin
        defVibrancy := Registry.ReadFloat('Vibrancy');
      end
      else
      begin
        defVibrancy := 1;
      end;
      if Registry.ValueExists('FilterRadius') then
      begin
        defFilterRadius := Registry.ReadFloat('FilterRadius');
      end
      else
      begin
        defFilterRadius := 0.2;
      end;
      if Registry.ValueExists('GammaThreshold') then
      begin
        defGammaThreshold := Registry.ReadFloat('GammaThreshold');
      end
      else
      begin
        defGammaThreshold := 0.01;
      end;
      if Registry.ValueExists('Oversample') then
      begin
        defOversample := Registry.ReadInteger('Oversample');
      end
      else
      begin
        defOversample := 1;
      end;
      if Registry.ValueExists('PreviewDensity') then
      begin
        defPreviewDensity := Registry.ReadFloat('PreviewDensity');
      end
      else
      begin
        defPreviewDensity := 0.5;
      end;
      if Registry.ValueExists('PreviewLowQuality') then
      begin
        prevLowQuality := Registry.ReadFloat('PreviewLowQuality');
      end
      else
      begin
        prevLowQuality := 0.1;
      end;
      if Registry.ValueExists('PreviewMediumQuality') then
      begin
        prevMediumQuality := Registry.ReadFloat('PreviewMediumQuality');
      end
      else
      begin
        prevMediumQuality := 1;
      end;
      if Registry.ValueExists('PreviewHighQuality') then
      begin
        prevHighQuality := Registry.ReadFloat('PreviewHighQuality');
      end
      else
      begin
        prevHighQuality := 5;
      end;
    end
    else
    begin
      defSampleDensity := 5;
      defGamma := 4;
      defBrightness := 4;
      defVibrancy := 1;
      defFilterRadius := 0.2;
      defOversample := 1;
      defGammaThreshold := 0.01;
      defPreviewDensity := 0.5;
      prevLowQuality := 0.1;
      prevMediumQuality := 1;
      prevHighQuality := 5;
    end;
    Registry.CloseKey;

    if Registry.OpenKey('Software\' + APP_NAME + '\Autosave', False) then
    begin
      if Registry.ValueExists('AutoSaveEnabled') then
      begin
        AutoSaveEnabled := Registry.ReadBool('AutoSaveEnabled');
      end
      else
      begin
        AutoSaveEnabled := False;
      end;
      if Registry.ValueExists('AutoSaveFreq') then
      begin
        AutoSaveFreq := Registry.ReadInteger('AutoSaveFreq');
      end
      else
      begin
        AutoSaveFreq := 2;
      end;
      if Registry.ValueExists('AutoSavePath') then
      begin
        AutoSavePath := Registry.ReadString('AutoSavePath');
      end
      else
      begin
        AutoSavePath := GetEnvVarValue('USERPROFILE') + '\autosave.flame';
      end;
    end
    else
    begin
      AutoSaveEnabled := False;
      AutoSaveFreq := 2;
      AutoSavePath := GetEnvVarValue('USERPROFILE') + '\autosave.flame';
    end;
    Registry.CloseKey;
  finally
    Registry.Free;
  end;

  PluginPath := ReadPluginDir;
end;

procedure SaveSettings;
var
  Registry: TRegistry;
  i: integer;
begin
  SavePluginDir(PluginPath);

  Registry := TRegistry.Create;
  try
    Registry.RootKey := HKEY_CURRENT_USER;
    { Defaults }
    if Registry.OpenKey('\Software\' + APP_NAME + '\Defaults', true) then
    begin
      Registry.WriteBool('StartupCheckForUpdates', StartupCheckForUpdates);
      Registry.WriteBool('AlwaysCreateBlankFlame', AlwaysCreateBlankFlame);
      Registry.WriteString('GradientFile', GradientFile);
      Registry.WriteBool('PlaySoundOnRenderComplete',
        PlaySoundOnRenderComplete);
      Registry.WriteString('RenderCompleteSoundFile', RenderCompleteSoundFile);
      Registry.WriteBool('AutoOpenLog', AutoOpenLog);
      Registry.WriteBool('WarnOnMissingPlugin', WarnOnMissingPlugin);
      Registry.WriteBool('MultithreadedPreview', MultithreadedPreview);
      Registry.WriteString('LanguageFile', LanguageFile);
      Registry.WriteString('LastOpenFile', LastOpenFile);
      Registry.WriteBool('RememberLastOpenFile', RememberLastOpenFile);
      Registry.WriteBool('UseSmallThumbnails', UseSmallThumbnails);
      Registry.WriteInteger('ToolBarWidth1', TBWidth1);
      Registry.WriteInteger('ToolBarWidth2', TBWidth2);
      Registry.WriteInteger('ToolBarWidth3', TBWidth3);
      Registry.WriteInteger('ToolBarWidth4', TBWidth4);
      Registry.WriteInteger('ToolBarWidth5', TBWidth5);
      Registry.WriteInteger('LineCenterColor', LineCenterColor);
      Registry.WriteInteger('LineThirdsColor', LineThirdsColor);
      Registry.WriteInteger('LineGRColor', LineGRColor);
      Registry.WriteBool('EnableGuides', EnableGuides);
      Registry.WriteBool('ConfirmDelete', ConfirmDelete);
      Registry.WriteBool('ConfirmExit', ConfirmExit);
      Registry.WriteInteger('MinTransforms', randMinTransforms);
      Registry.WriteInteger('MaxTransforms', randMaxTransforms);
      Registry.WriteInteger('MutationMinTransforms', mutantMinTransforms);
      Registry.WriteInteger('MutationMaxTransforms', mutantMaxTransforms);
      Registry.WriteString('ParameterFolder3D', ParamFolder);
      Registry.WriteString('SavePath3D', SavePath);
      Registry.WriteString('BrowserPath', BrowserPath);
      Registry.WriteInteger('EditPreviewQaulity', EditPrevQual);
      Registry.WriteInteger('MutatePreviewQaulity', MutatePrevQual);
      Registry.WriteInteger('AdjustPreviewQaulity', AdjustPrevQual);
      Registry.WriteString('RandomPrefix', RandomPrefix);
      Registry.WriteString('RandomDate', RandomDate);
      Registry.WriteInteger('RandomIndex', RandomIndex);
      Registry.WriteString('DefaultFlameFile3D', defFlameFile);
      Registry.WriteString('SmoothPalettePath', SmoothPalettePath);
      Registry.WriteString('GradientFile', GradientFile);
      Registry.WriteString('SmoothPaletteFile', defSmoothPaletteFile);
      Registry.WriteInteger('SymmetryType', SymmetryType);
      Registry.WriteInteger('SymmetryOrder', SymmetryOrder);
      Registry.WriteInteger('SymmetryNVars', SymmetryNVars);
      // Registry.WriteInteger('VariationOptions', VariationOptions);
      // Registry.WriteInteger('VariationOptions2', VariationOptions shr 32);
      // Registry.WriteInteger('ReferenceMode', ReferenceMode);
      Registry.WriteInteger('RotationMode', MainForm_RotationMode);
      Registry.WriteString('ScriptPath', ScriptPath);
      Registry.WriteBool('ShowProgress', ShowProgress);
      Registry.WriteBool('KeepBackground', KeepBackground);
      Registry.WriteBool('PreserveQuality', PreserveQuality);
      Registry.WriteString('FunctionLibrary', defLibrary);

      Registry.WriteBool('ShowTransparency', ShowTransparency);
      Registry.WriteInteger('PNGTransparency', PNGTransparency);
      Registry.WriteBool('ExtendMainPreview', ExtendMainPreview);
      Registry.WriteFloat('MainPreviewScale', MainPreviewScale);

      Registry.WriteBool('SaveIncompleteRenders', SaveIncompleteRenders);
      Registry.WriteBool('ShowRenderStats', ShowRenderStats);
      Registry.WriteBool('LowerRenderPriority', LowerRenderPriority);

      Registry.WriteInteger('NrTreads', NrTreads);
      Registry.WriteInteger('UseNrThreads', UseNrThreads);
      Registry.WriteInteger('InternalBitsPerSample', InternalBitsPerSample);
    end;
    Registry.CloseKey;

    if Registry.OpenKey('\Software\' + APP_NAME + '\Variations', true) then
    begin
      for i := 0 to NRVAR - 1 do
      begin
        if Registry.ValueExists(Varnames(i)) then
          if Registry.ReadBool(Varnames(i)) = Variations[i] then
            continue;
        Registry.WriteBool(Varnames(i), Variations[i]);
      end;
    end;
    Registry.CloseKey;

    { Editor }
    if Registry.OpenKey('\Software\' + APP_NAME + '\Forms\Editor', true) then
    begin
      Registry.WriteBool('UseTransformColors', UseTransformColors);
      Registry.WriteBool('HelpersEnabled', HelpersEnabled);
      Registry.WriteBool('ShowAllXforms', ShowAllXforms);
      Registry.WriteBool('EnableEditorPreview', EnableEditorPreview);
      Registry.WriteInteger('EditorPreviewTransparency',
        EditorPreviewTransparency);
      Registry.WriteInteger('BackgroundColor', EditorBkgColor);
      Registry.WriteInteger('GridColor1', GridColor1);
      Registry.WriteInteger('GridColor2', GridColor2);
      Registry.WriteInteger('HelpersColor', HelpersColor);
      Registry.WriteInteger('ReferenceTriangleColor', ReferenceTriangleColor);
      Registry.WriteBool('ExtendedEdit', ExtEditEnabled);
      Registry.WriteBool('LockTransformAxis', TransformAxisLock);
      Registry.WriteBool('RebuildXaosLinks', RebuildXaosLinks);
    end;
    Registry.CloseKey;

    { Display }
    if Registry.OpenKey('\Software\' + APP_NAME + '\Display', true) then
    begin
      Registry.WriteFloat('SampleDensity', defSampleDensity);
      Registry.WriteFloat('Gamma', defGamma);
      Registry.WriteFloat('Brightness', defBrightness);
      Registry.WriteFloat('Vibrancy', defVibrancy);
      Registry.WriteFloat('FilterRadius', defFilterRadius);
      Registry.WriteInteger('Oversample', defOversample);
      Registry.WriteFloat('GammaThreshold', defGammaThreshold);
      Registry.WriteFloat('PreviewDensity', defPreviewDensity);
      Registry.WriteFloat('PreviewLowQuality', prevLowQuality);
      Registry.WriteFloat('PreviewMediumQuality', prevMediumQuality);
      Registry.WriteFloat('PreviewHighQuality', prevHighQuality);
    end;
    Registry.CloseKey;

    if Registry.OpenKey('\Software\' + APP_NAME + '\Render', true) then
    begin
      Registry.WriteString('Path', RenderPath);
      Registry.WriteFloat('SampleDensity', renderDensity);
      Registry.WriteInteger('Oversample', renderOversample);
      Registry.WriteFloat('FilterRadius', renderFilterRadius);
      Registry.WriteInteger('Width', renderWidth);
      Registry.WriteInteger('Height', renderHeight);
      Registry.WriteInteger('JPEGQuality', JPEGQuality);
      Registry.WriteInteger('FileFormat', renderFileFormat);
      Registry.WriteInteger('BitsPerSample', renderBitsPerSample);
      Registry.WriteBool('StoreEXIF', StoreEXIF);
      Registry.WriteBool('StoreParamsEXIF', StoreParamsEXIF);
      Registry.WriteString('ExifAuthor', ExifAuthor);
    end;
    Registry.CloseKey;

    if Registry.OpenKey('\Software\' + APP_NAME + '\Autosave', true) then
    begin
      Registry.WriteBool('AutoSaveEnabled', AutoSaveEnabled);
      Registry.WriteInteger('AutoSaveFreq', AutoSaveFreq);
      Registry.WriteString('AutoSavePath', AutoSavePath);
    end;
    Registry.CloseKey;
  finally
    Registry.Free;
  end;
end;

end.
