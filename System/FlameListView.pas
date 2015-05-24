unit FlameListView;

interface

uses
  Classes,
  Windows,
  Graphics,
  Controls,
  ComCtrls,
  SysUtils,
  StrUtils,
  RenderingInterface,
  RenderingImplementation,
  ControlPoint,
  ParameterIO;

type

  TFlameListThumbnailThread = class(TThread)
    private

      mBatch: TBatch;
      mImages: TImageList;
      mThumbnailSize: integer;
      mPlaceholder: TBitmap;

      procedure SetBatch(value: TBatch);
      procedure SetThumbnailSize(value: integer);

    protected

      procedure Execute; override;

    public

      ThumbnailCompleted: procedure(index: integer) of object;

      constructor Create;
      destructor Destroy; override;

      property Batch: TBatch
        read mBatch
        write SetBatch;

      property ThumbnailSize: integer
        read mThumbnailSize
        write SetThumbnailSize;

      property Images: TImageList
        read mImages;

  end;

  TFlameListView = class
    private

      mList: TListView;
      mThumbnails: TFlameListThumbnailThread;

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

function LoadThumbnailPlaceholder(ThumbnailSize: Integer): TBitmap;
var
  placeholder: TBitmap;
  placeholderIcon: TBitmap;
const
  pi_width = 48;
  pi_height = 48;
begin
  placeholder := TBitmap.Create;
  placeholderIcon := TBitmap.Create;

  placeholderIcon.Handle := LoadBitmap(hInstance, 'THUMB_PLACEHOLDER');
  placeholder.PixelFormat := pf32bit;
  placeholder.HandleType := bmDIB;
  placeholder.Width := ThumbnailSize;
  placeholder.Height := ThumbnailSize;

  with placeholder.Canvas do
  begin
    Brush.Color := $000000;
    FillRect(Rect(0, 0, placeholder.Width, placeholder.Height));
    Draw(round(ThumbnailSize / 2 - pi_width / 2),
      round(ThumbnailSize / 2 - pi_height / 2), placeholderIcon);
  end;

  placeholderIcon.Free;
  Result := placeholder;
end;

//-- TFlameListThumbnailThread
constructor TFlameListThumbnailThread.Create;
begin
  mThumbnailSize := 120;
  mImages := TImageList.CreateSize(mThumbnailSize, mThumbnailSize);

  inherited Create(true);
end;

procedure TFlameListThumbnailThread.SetThumbnailSize(value: Integer);
var
  i: integer;
begin
  if value <= 0 then
    raise Exception.Create('Thumbnail size invalid');

  mThumbnailSize := value;

  mImages.Clear;
  mImages.Width := value;
  mImages.Height := value;

  if Assigned(mPlaceholder) then
    mPlaceholder.Free;

  mPlaceholder := LoadThumbnailPlaceholder(value);
  mImages.Add(mPlaceholder, nil);
end;

procedure TFlameListThumbnailThread.SetBatch(value: TBatch);
begin
  mBatch := value;
  mImages.Clear;
end;

procedure TFlameListThumbnailThread.Execute;
var
  Renderer : TRenderer;
  Flame : TControlPoint;
  Thumbnail : TBitmap;

  width, height, ratio : double;
  status: string;
  i : integer;

  memstream : TMemoryStream;
begin
  Inherited;

  Renderer := TRenderer.Create;
  Flame := TControlPoint.Create;

  for i := 0 to mBatch.Count - 1 do begin
    if Assigned(Flame) then
    begin
      Flame.Destroy;
    end;

    Flame := TControlPoint.Create;
    mBatch.LoadControlPoint(i, Flame);

    width := Flame.Width;
    height := Flame.Height;
    ratio := width / height;

    if (width < height) then begin
      width := (*ratio * *)ThumbnailSize;
      height := ThumbnailSize;
    end else if (width > height) then begin
      height := ThumbnailSize(* / ratio*);
      width := ThumbnailSize;
    end else begin
      width := ThumbnailSize;
      height := ThumbnailSize;
    end;

    Flame.AdjustScale(round(width), round(height));
    Flame.Width := round(width);
    Flame.Height := round(height);
    Flame.spatial_oversample := 1;
    Flame.spatial_filter_radius := 0.5;
    Flame.sample_density := 3;

    Renderer.SetCP(Flame);
    Renderer.Render;

    Thumbnail := TBitmap.Create;
    Thumbnail.PixelFormat := pf32bit;
    Thumbnail.HandleType := bmDIB;
    Thumbnail.Width := ThumbnailSize;
    Thumbnail.Height := ThumbnailSize;
    //Thumbnail.Canvas.Brush.Color := GetSysColor(5);
    Thumbnail.Canvas.FillRect(Rect(0, 0, ThumbnailSize, ThumbnailSize));
    Thumbnail.Canvas.Draw(round(ThumbnailSize / 2 - width / 2), round(ThumbnailSize / 2 - height / 2), renderer.GetImage);

    mImages.Add(Thumbnail, nil);

    if Assigned(ThumbnailCompleted) then
      ThumbnailCompleted(i);

    Thumbnail.Free;
    Thumbnail := nil;

    Flame.Destroy;
    Flame := nil;
  end;

  Flame.Free;
  Renderer.Free;
end;

destructor TFlameListThumbnailThread.Destroy;
begin
  mBatch := nil; // -x- not owned by this class, don't destroy!

  if Assigned(mImages) then
    mImages.Destroy;

  mImages := nil;

  inherited Destroy;
end;

//-- TFlameListView
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

  mThumbnails := TFlameListThumbnailThread.Create;
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