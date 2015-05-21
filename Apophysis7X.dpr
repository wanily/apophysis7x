program Apophysis7X;

{%ToDo 'Assets\Apophysis7X.todo'}
{$R 'Resources\Apophysis7X.res'}
{$SetPEFlags $20}
{$Include 'delphiversion.pas'}

uses

{-- BASIC --}
  FastMM4 in 'System\FastMM4.pas',
  FastMM4Messages in 'System\FastMM4Messages.pas',
  Forms, Dialogs, SysUtils,
  sdStringTable in 'System\sdStringTable.pas',
  CustomDrawControl in 'System\CustomDrawControl.pas',
  LibXmlComps in 'System\LibXmlComps.pas',
  LibXmlParser in 'System\LibXmlParser.pas',
  Windows7 in 'System\Windows7.pas',
  RegexHelper in 'System\RegexHelper.pas',

{-- CORE --}
  Global in 'Core\Global.pas',
  CommandLine in 'IO\CommandLine.pas',
  MissingPlugin in 'IO\MissingPlugin.pas',
  Settings in 'IO\Settings.pas',
  Translation in 'Core\Translation.pas',
  ParameterIO in 'IO\ParameterIO.pas',

{-- FLAME --}
  RndFlame in 'Flame\RndFlame.pas',
  ControlPoint in 'Flame\ControlPoint.pas',
  cmapdata in 'ColorMap\cmapdata.pas',
  cmap in 'ColorMap\cmap.pas',
  GradientHlpr in 'ColorMap\GradientHlpr.pas',
  XFormMan in 'Core\XFormMan.pas',
  XForm in 'Flame\XForm.pas',
  BaseVariation in 'Core\BaseVariation.pas',

{-- RENDERER --}
  RenderingCommon in 'Rendering\RenderingCommon.pas',
  RenderingInterface in 'Rendering\RenderingInterface.pas',
  RenderingImplementation in 'Rendering\RenderingImplementation.pas',
  BucketFillerThread in 'Rendering\BucketFillerThread.pas',
  RenderThread in 'Rendering\RenderThread.pas',
  ImageMaker in 'Rendering\ImageMaker.pas',

{-- VARIATIONS --}
  varHemisphere in 'Variations\varHemisphere.pas',
  varLog in 'Variations\varLog.pas',
  varPolar2 in 'Variations\varPolar2.pas',
  varRings2 in 'Variations\varRings2.pas',
  varFan2 in 'Variations\varFan2.pas',
  varCross in 'Variations\varCross.pas',
  varWedge in 'Variations\varWedge.pas',
  varEpispiral in 'Variations\varEpispiral.pas',
  varBwraps in 'Variations\varBwraps.pas',
  varPDJ in 'Variations\varPDJ.pas',
  varJuliaN in 'Variations\varJuliaN.pas',
  varJuliaScope in 'Variations\varJuliaScope.pas',
  varJulia3Djf in 'Variations\varJulia3Djf.pas',
  varJulia3Dz in 'Variations\varJulia3Dz.pas',
  varCurl in 'Variations\varCurl.pas',
  varCurl3D in 'Variations\varCurl3D.pas',
  varRadialBlur in 'Variations\varRadialBlur.pas',
  varBlurCircle in 'Variations\varBlurCircle.pas',
  varBlurZoom in 'Variations\varBlurZoom.pas',
  varBlurPixelize in 'Variations\varBlurPixelize.pas',
  varFalloff2 in 'Variations\varFalloff2.pas',
  varRectangles in 'Variations\varRectangles.pas',
  varSplits in 'Variations\varSplits.pas',
  varSeparation in 'Variations\varSeparation.pas',
  varBipolar in 'Variations\varBipolar.pas',
  varLoonie in 'Variations\varLoonie.pas',
  varEscher in 'Variations\varEscher.pas',
  varScry in 'Variations\varScry.pas',
  varNGon in 'Variations\varNGon.pas',
  varFoci in 'Variations\varFoci.pas',
  varLazysusan in 'Variations\varLazysusan.pas',
  varMobius in 'Variations\varMobius.pas',
  varCrop in 'Variations\varCrop.pas',
  // circlecrop
  varElliptic in 'Variations\varElliptic.pas',
  varWaves2 in 'Variations\varWaves2.pas',
  varAuger in 'Variations\varAuger.pas',
  // glynnsim2
  // flux
  // boarders2
  varPreSpherical in 'Variations\varPreSpherical.pas',
  varPreSinusoidal in 'Variations\varPreSinusoidal.pas',
  varPreDisc in 'Variations\varPreDisc.pas',
  // pre_boarders2
  varPreBwraps in 'Variations\varPreBwraps.pas',
  varPreCrop in 'Variations\varPreCrop.pas',
  // pre_circlecrop
  varPreFalloff2 in 'Variations\varPreFalloff2.pas',
  // post_boarders2
  varPostBwraps in 'Variations\varPostBwraps.pas',
  varPostCurl in 'Variations\varPostCurl.pas',
  varPostCurl3D in 'Variations\varPostCurl3D.pas',
  varPostCrop in 'Variations\varPostCrop.pas',
  // post_circlecrop
  varPostFalloff2 in 'Variations\varPostFalloff2.pas',
  varGenericPlugin in 'Variations\varGenericPlugin.pas',

{-- GUI --}
  Main in 'Forms\Main.pas' {MainForm},
  Tracer in 'Forms\Tracer.pas' {TraceForm},
  About in 'Forms\About.pas' {AboutForm},
  Adjust in 'Forms\Adjust.pas' {AdjustForm},
  Browser in 'Forms\Browser.pas' {GradientBrowser},
  Editor in 'Forms\Editor.pas' {EditForm},
  FormRender in 'Forms\FormRender.pas' {RenderForm},
  Fullscreen in 'Forms\Fullscreen.pas' {FullscreenForm},
  LoadTracker in 'Forms\LoadTracker.pas' {LoadForm},
  Options in 'Forms\Options.pas' {OptionsForm},
  Save in 'Forms\Save.pas' {SaveForm},
  Template in 'Forms\Template.pas' {TemplateForm},
  TransformSelection in 'Forms\TransformSelection.pas' {TransformSelectionForm}

  {$ifdef DisableScripting};
  // if scripting is disabled, don't import the scripting form units
  {$else},
  Preview in 'Forms\Preview.pas' {PreviewForm},
  FormFavorites in 'Forms\FormFavorites.pas' {FavoritesForm},
  ScriptForm in 'Forms\ScriptForm.pas' {ScriptEditor},
  ScriptRender in 'Forms\ScriptRender.pas'; {ScriptRenderForm}
  {$endif}

begin
  InitializePlugins;

  Application.Initialize;

  {$ifdef Apo7X64}
  Application.Title := 'Apophysis 7x (64 bit)';
  {$else}
  Application.Title := 'Apophysis 7x (32 bit)';
  {$endif}
  Application.HelpFile := 'Apophysis7x.chm';

  Application.CreateForm(TMainForm, MainForm);
  Application.CreateForm(TTraceForm, TraceForm);
  Application.CreateForm(TAboutForm, AboutForm);
  Application.CreateForm(TAdjustForm, AdjustForm);
  Application.CreateForm(TGradientBrowser, GradientBrowser);
  Application.CreateForm(TEditForm, EditForm);
  Application.CreateForm(TRenderForm, RenderForm);
  Application.CreateForm(TFullscreenForm, FullscreenForm);
  Application.CreateForm(TLoadForm, LoadForm);
  Application.CreateForm(TOptionsForm, OptionsForm);
  Application.CreateForm(TSaveForm, SaveForm);
  Application.CreateForm(TTemplateForm, TemplateForm);
  Application.CreateForm(TTransformSelectionForm, TransformSelectionForm);

  {$ifdef DisableScripting}
  // if scripting is disabled, don't create the scripting forms
  {$else}
  Application.CreateForm(TPreviewForm, PreviewForm);
  Application.CreateForm(TFavoritesForm, FavoritesForm);
  Application.CreateForm(TScriptEditor, ScriptEditor);
  Application.CreateForm(TScriptRenderForm, ScriptRenderForm);
  {$endif}

  Application.UpdateFormatSettings := False;
  {$ifdef UseFormatSettings}
    FormatSettings.DecimalSeparator := '.';
  {$else}
    DecimalSeparator := '.';
  {$endif}
  Application.Run;
end.



