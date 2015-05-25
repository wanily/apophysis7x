unit FlameListView;

interface uses
  Classes,
  Controls,
  ComCtrls,
  ThumbnailThread,
  ParameterIO;

type TFlameListView = class

  private

    mList: TListView;
    mThumbnails: TThumbnailThread;

    mBatch: TBatch;
    mShowThumbnails: boolean;
    mIsUpdating: boolean;
    mIsSelecting: boolean;
    mThumbnailSize: integer;

    function GetSelectedIndex: integer;
    function GetThumbnailSize: integer;

    procedure SetBatch(value: TBatch);
    procedure SetShowThumbnails(value: boolean);
    procedure SetThumbnailSize(value: integer);

    procedure OnThumbnailCompleted(index: integer);
    procedure OnListSelectionChanged(sender: TObject; item: TListItem; selected: boolean);

  public

    SelectedIndexChanged: procedure(index: integer) of object;

    constructor Create(list: TListView);
    destructor Destroy; override;

    procedure RemoveItemAt(i: integer);
    procedure SelectIndex(i: integer);

    procedure Refresh(newSelection: integer);

    property ThumbnailSize: integer
      read GetThumbnailSize
      write SetThumbnailSize;

    property Batch: TBatch
      read mBatch
      write SetBatch;

    property ShowThumbnails: boolean
      read mShowThumbnails
      write SetShowThumbnails;

    property SelectedIndex: integer
      read GetSelectedIndex
      write SelectIndex;

end;

implementation

constructor TFlameListView.Create(list: TListView);
begin
  mList := list;
  mList.OnSelectItem := OnListSelectionChanged;
end;

function TFlameListView.GetThumbnailSize;
begin
  Result := mThumbnailSize;
end;

procedure TFlameListView.SetThumbnailSize(value: Integer);
begin
  mThumbnailSize := value;

  if Assigned(mThumbnails) then
     mThumbnails.ThumbnailSize := value;
end;

procedure TFlameListView.OnListSelectionChanged(sender: TObject; item: TListItem; selected: boolean);
begin
  if mIsSelecting then
    exit;

  if Assigned(SelectedIndexChanged) then
    SelectedIndexChanged(SelectedIndex);
end;

procedure TFlameListView.Refresh(newSelection: Integer);
var
  i, oldSelection: integer;
begin
  oldSelection := SelectedIndex;

  mIsUpdating := true;
  mList.Items.Clear;

  for i := 0 to mBatch.Count - 1 do
  begin
    mList.AddItem(mBatch.GetFlameNameAt(i), nil);
    mList.Items[i].ImageIndex := 0;
  end;

  mIsUpdating := false;

  if newSelection < 0 then
    newSelection := oldSelection;

  mList.Refresh;
  SelectedIndex := newSelection;

  if Assigned(mThumbnails) then
    mThumbnails.Destroy;

  mThumbnails := TThumbnailThread.Create;
  mThumbnails.ThumbnailCompleted := OnThumbnailCompleted;
  mThumbnails.ThumbnailSize := mThumbnailSize;

  mThumbnails.Batch := mBatch;
  mList.LargeImages := mThumbnails.Images;

  mThumbnails.Start;
end;

procedure TFlameListView.SetBatch(value: TBatch);
var
  i: Integer;
begin
  mBatch := value;
  Refresh(SelectedIndex);
end;

procedure TFlameListView.RemoveItemAt(i: integer);
begin
  mIsUpdating := true;
  mList.Items.Delete(i);
  mThumbnails.Images.Delete(i);
  mIsUpdating := false;
  mList.Refresh;
end;

procedure TFlameListView.SetShowThumbnails(value: boolean);
begin
  mShowThumbnails := value;

  if value then
  begin
    mList.ViewStyle := vsIcon;
  end else begin
    mList.ViewStyle := vsList;
  end;
end;

function TFlameListView.GetSelectedIndex;
begin
  if not Assigned(mList.Selected) then
  begin
    Result := -1;
    Exit;
  end;

  Result := mList.Selected.Index;
end;

procedure TFlameListView.SelectIndex(i: Integer);
begin
  mIsSelecting := true;

  mList.Selected := mList.Items[i];
  if Assigned(SelectedIndexChanged) then
    SelectedIndexChanged(SelectedIndex);

  mIsSelecting := false;
end;

procedure TFlameListView.OnThumbnailCompleted(index: Integer);
begin
  mList.Items[index].ImageIndex := index;
end;

destructor TFlameListView.Destroy;
begin
  mList.LargeImages := nil;
  mList := nil;
  mBatch := nil;

  if Assigned(mThumbnails) then
    mThumbnails.Destroy;

  inherited Destroy;
end;

end.