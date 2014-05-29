unit About;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  StdCtrls, ExtCtrls, Global, Translation;

type
  TAboutForm = class(TForm)
    okButton: TButton;
    createdUsing: TLabel;
    linkLabel: TLabel;
    fractalFlames: TLabel;
    copyrightScottDraves: TLabel;
    flam3: TLabel;
    fractalFlamesSeparator: TBevel;
    copyright2005_2008: TLabel;
    peterSbodnov: TLabel;
    piotrBorys: TLabel;
    ronaldHordijk: TLabel;
    copyright2001_2004: TLabel;
    scripterStudio: TLabel;
    xmlParser: TLabel;
    copyrightSeparator: TBevel;
    georgKiehne: TLabel;
    copyright2009_2014: TLabel;
    markTownsend: TLabel;
    createdUsingSeparator: TBevel;
    pcreDelphi: TLabel;
    versionSpace: TLabel;
    infoSpace: TLabel;
    logoPicture: TImage;

    procedure DevelopersClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure okButtonClick(Sender: TObject);

  public
    procedure Localize;
  end;

var
  AboutForm: TAboutForm;

implementation

uses Main, ShellAPI;

{$R *.DFM}

procedure TAboutForm.FormCreate(Sender: TObject);
begin
  Localize;
end;

procedure TAboutForm.Localize;
var s1, s2, s3:string;
begin
  okButton.Caption := TextByKey('common-close');
  infoSpace.Caption := APP_VERSION;

  if (LanguageFile <> AvailableLanguages.Strings[0]) and (LanguageFile <> '') then
  begin
    LanguageInfo(LanguageFile, s1, s2);
    s3 := LanguageAuthor(LanguageFile);
    versionSpace.Visible := (s1 <> '') and (s3 <> '');
    versionSpace.Caption := s1 + ' translation contributed by: ' + s3;
  end;
end;

procedure TAboutForm.okButtonClick(Sender: TObject);
begin
  Close;
end;

procedure TAboutForm.DevelopersClick(Sender: TObject);
begin
  ShellExecute(ValidParentForm(Self).Handle, 'open', PChar(TLabel(Sender).Hint), nil, nil, SW_SHOWNORMAL);
end;

end.
