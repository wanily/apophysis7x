unit TransformSelection;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants, System.Classes, Vcl.Graphics,
  Vcl.Controls, Vcl.Forms, Vcl.Dialogs, Vcl.ImgList, Vcl.StdCtrls, Translation, XForm, ControlPoint,
  Global;

type
  TTransformSelectionForm = class(TForm)
    excluded: TListBox;
    included: TListBox;
    arrows: TImageList;
    include: TButton;
    exclude: TButton;
    cancel: TButton;
    ok: TButton;
    excludedLabel: TLabel;
    includedLabel: TLabel;

    procedure FormCreate(Sender: TObject);
    procedure includeClick(Sender: TObject);
    procedure excludeClick(Sender: TObject);
    procedure cancelClick(Sender: TObject);
    procedure okClick(Sender: TObject);
    procedure UpdateButtonStates(Sender: TObject);

  private
    currentcp: TControlPoint;

    procedure Move(A, B: TListBox; AA, BB: TStringList);

  public
    ExcludedIndices, IncludedIndices: TStringList;

    procedure Localize;
    procedure Select(cp: TControlPoint; selected, count: integer);
  end;

var
    TransformSelectionForm: TTransformSelectionForm;

implementation

{$R *.dfm}

function formatXform(xf: TXForm; i: integer): string;
begin
  if xf.TransformName = '' then
    Result := Format(TextByKey('selecttransform-item'), [i+1])
  else Result := Format(TextByKey('selecttransform-item-withname'), [i+1, xf.TransformName]);
end;

procedure TTransformSelectionForm.Select(cp: TControlPoint; selected, count: integer);
var
  i: integer;
begin
  currentcp := cp;

  ExcludedIndices.Clear;
  IncludedIndices.Clear;

  Excluded.Clear;
  Included.Clear;

  if (count < NXFORMS) and (selected <> count) then
  begin
    for i := 0 to count - 1 do
    begin
      if i <> selected then
      begin
        excluded.Items.Add(FormatXform(cp.xform[i],i));
        ExcludedIndices.Add(IntToStr(i));
      end;
    end;

    included.Items.Add(FormatXform(cp.xform[selected],selected));
    IncludedIndices.Add(IntToStr(selected));
  end;
end;

procedure TTransformSelectionForm.Move(A, B: TListBox; AA, BB: TStringList);
var
  i: Integer;
  j: Integer;
  sel: array of boolean;
  C, CC : TStringList;
begin
  if (A.Count = 0) then Exit;
  setLength(sel, A.Count);

  for i := 0 to A.Count - 1 do sel[i] := A.Selected[i];
  for i := 0 to Length(sel) - 1 do
  begin
    if sel[i] then
    begin
      j := StrToInt(AA.Strings[i]);
      B.Items.Add(FormatXform(currentcp.xform[j],j));
      BB.Add(AA[i]);
    end;
  end;

  C := TStringList.Create;
  CC := TStringList.Create;

  for i := 0 to Length(sel) - 1 do
  begin
    if not sel[i] then
    begin
      C.Add(A.Items[i]);
      CC.Add(AA.Strings[i]);
    end;
  end;

  A.Clear;
  AA.Clear;
  for i := 0 to C.Count - 1 do
  begin
    A.Items.Add(C[i]);
    AA.Add(CC[i]);
  end;

  C.Free;
  CC.Free;

  C := nil;
  CC := nil;

  A.Refresh;
  B.Refresh;

  UpdateButtonStates(nil);
end;

procedure TTransformSelectionForm.okClick(Sender: TObject);
begin
  ModalResult := mrOK;
end;

procedure TTransformSelectionForm.includeClick(Sender: TObject);
begin
  Move(Excluded, Included, ExcludedIndices, IncludedIndices);
end;

procedure TTransformSelectionForm.excludeClick(Sender: TObject);
begin
  Move(Included, Excluded, IncludedIndices, ExcludedIndices);
end;

procedure TTransformSelectionForm.cancelClick(Sender: TObject);
begin
  ModalResult := mrCancel;
  Close;
end;

procedure TTransformSelectionForm.Localize;
begin
  includedLabel.Caption := TextByKey('selecttransform-included');
  excludedLabel.Caption := TextByKey('selecttransform-excluded');

  ok.Caption := TextByKey('common-ok');
  cancel.Caption := TextByKey('common-cancel');

  self.Caption := TextByKey('selecttransform-title');
end;

procedure TTransformSelectionForm.UpdateButtonStates(Sender: TObject);
var excl, incl: boolean; i:integer;
begin
  excl:=false;
  incl:=false;

  (*
  for i := 0 to excluded.Count-1 do excl := excl or excluded.Selected[i];
  for i := 0 to included.Count-1 do incl := incl or included.Selected[i];

  include.Enabled := excl;
  exclude.Enabled := incl;
  *)

  ok.Enabled := included.Count > 0;
end;

procedure TTransformSelectionForm.FormCreate(Sender: TObject);
begin
  Localize;

  IncludedIndices := TStringList.Create;
  ExcludedIndices := TStringList.Create;
end;

end.
