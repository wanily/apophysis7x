program Apophysis7X;


{$SetPEFlags $20}

uses
  FastMM4 in 'System\FastMM4.pas',
  FastMM4Messages in 'System\FastMM4Messages.pas',
  sdStringTable in 'System\sdStringTable.pas',
  CustomDrawControl in 'System\CustomDrawControl.pas',
  LibXmlComps in 'System\LibXmlComps.pas',
  LibXmlParser in 'System\LibXmlParser.pas',
  Windows7 in 'System\Windows7.pas',
  RegexHelper in 'System\RegexHelper.pas',
  ApophysisRibbon in 'Ribbon\ApophysisRibbon.pas',
  UIRibbon in 'System\UIRibbon.pas',
  UIRibbonForm in 'System\UIRibbonForm.pas',
  UIRibbonUtils in 'System\UIRibbonUtils.pas',
  UIRibbonApi in 'System\UIRibbonApi.pas',
  UIRibbonCommands in 'System\UIRibbonCommands.pas',
  UIRibbonActions in 'System\UIRibbonActions.pas',
  WinApiEx in 'System\WinApiEx.pas',
  FlameListView in 'System\FlameListView.pas' {-- CORE --},
  Global in 'Core\Global.pas',
  Settings in 'IO\Settings.pas',
  Translation in 'Core\Translation.pas',
  ParameterIO in 'IO\ParameterIO.pas',
  RndFlame in 'Flame\RndFlame.pas',
  ControlPoint in 'Flame\ControlPoint.pas',
  PaletteIO in 'IO\PaletteIO.pas',
  CoreVariation in 'Flame\CoreVariation.pas',
  Variation in 'Flame\Variation.pas',
  VariationPoolManager in 'Flame\VariationPoolManager.pas',
  XForm in 'Flame\XForm.pas',
  RenderingCommon in 'Rendering\RenderingCommon.pas',
  RenderingInterface in 'Rendering\RenderingInterface.pas',
  RenderingImplementation in 'Rendering\RenderingImplementation.pas',
  BucketFillerThread in 'Rendering\BucketFillerThread.pas',
  RenderThread in 'Rendering\RenderThread.pas',
  ImageMaker in 'Rendering\ImageMaker.pas',
  varHemisphere in 'Variations\varHemisphere.pas',
  varLog in 'Variations\varLog.pas',
  varPolar2 in 'Variations\varPolar2.pas',
  varRings2 in 'Variations\varRings2.pas',
  varFan2 in 'Variations\varFan2.pas',
  varCross in 'Variations\varCross.pas',
  varWedge in 'Variations\varWedge.pas',
  varEpispiral in 'Variations\varEpispiral.pas',
  varBwraps in 'Variations\varBwraps.pas',
  varpdj in 'Variations\varpdj.pas',
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
  varElliptic in 'Variations\varElliptic.pas',
  varWaves2 in 'Variations\varWaves2.pas',
  varAuger in 'Variations\varAuger.pas',
  varPreSpherical in 'Variations\varPreSpherical.pas',
  varPreSinusoidal in 'Variations\varPreSinusoidal.pas',
  varPreDisc in 'Variations\varPreDisc.pas',
  varPreBwraps in 'Variations\varPreBwraps.pas',
  varPreCrop in 'Variations\varPreCrop.pas',
  varPreFalloff2 in 'Variations\varPreFalloff2.pas',
  varPostBwraps in 'Variations\varPostBwraps.pas',
  varPostCurl in 'Variations\varPostCurl.pas',
  varPostCurl3D in 'Variations\varPostCurl3D.pas',
  varPostCrop in 'Variations\varPostCrop.pas',
  varPostFalloff2 in 'Variations\varPostFalloff2.pas',
  PluginVariation in 'Flame\PluginVariation.pas',
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
  TransformSelection in 'Forms\TransformSelection.pas' {TransformSelectionForm},
  Vcl.Forms,
  ApophysisCommandManager in 'Gui\ApophysisCommandManager.pas',
  ThumbnailThread in 'Gui\ThumbnailThread.pas';

begin
  Application.Initialize;

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
  Application.CreateForm(TTransformSelectionForm, TransformSelectionForm);
  Application.Run;
end.





