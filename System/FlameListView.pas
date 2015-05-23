unit FlameListView;

interface

uses Classes, ComCtrls, Controls, SysUtils, StrUtils, ParameterIO, rkView;

type

  TBatch = class
    private

      mNames: TStringList;
      mData: TStringList;

      procedure Parse(const path: string);
      function CountFlames: integer;

    public

      constructor Create(const path: string);
      destructor Destroy; override;

      property Count: integer read CountFlames;

      function GetFlameNameAt(i: integer): string;
      function GetFlameXmlAt(i: integer): string;

  end;

  TFlameListThumbnailThread = class(TThread)
    private

      mBatch: TBatch;
      mImages: TImageList;
      mThumbnailSize: integer;

      procedure SetBatch(value: TBatch);
      procedure SetThumbnailSize(value: integer);

    protected

      procedure Execute; override;

    public

      constructor Create;
      destructor Destroy;

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

      function GetSelectedIndex: integer;

      procedure SetBatch(value: TBatch);
      procedure SetShowThumbnails(value: boolean);

      procedure OnThumbnailsChange(sender: TObject);

    public

      constructor Create(list: TListView);
      destructor Destroy;

      procedure RemoveItemAt(i: integer);
      procedure SelectIndex(i: integer);

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

uses Windows, RenderingInterface, RenderingImplementation, ControlPoint, Graphics;

//-- TBatch
constructor TBatch.Create(const path: string);
begin
  if not FileExists(path) then
    raise Exception.Create('Could not find file: "' + path + '"');

  mNames := TStringList.Create;
  mData := TStringList.Create;

  Parse(path);
end;

procedure TBatch.Parse(const path: string);
var
  fileContent: TStringList;
  flames: TStringList;

  i: integer;
  name: string;
begin
  fileContent := TStringList.Create;
  fileContent.LoadFromFile(path, TEncoding.Default);  // -x- todo: evaluate effects of using UTF8

  flames := TStringList.Create;
  EnumParameters(fileContent.Text, flames);

  mNames.Clear;
  mData.Clear;

  for i := 0 to flames.Count - 1 do
  begin
    name := NameOf(flames[i]);
    mNames.Add(name);
    mData.Add(flames[i]);
  end;

  flames.Destroy;
  fileContent.Destroy;
end;

function TBatch.CountFlames: integer;
begin
  Assert(mNames.Count = mData.Count);
  Result := mNames.Count;
end;

function TBatch.GetFlameNameAt(i: Integer): string;
begin
  Result := mNames[i];
end;

function TBatch.GetFlameXmlAt(i: Integer): string;
begin
  Result := mData[i];
end;

destructor TBatch.Destroy;
begin
  mNames.Destroy;
  mData.Destroy;
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

  xml : string;
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

    xml := mBatch.GetFlameXmlAt(i);
    LoadCpFromXmlCompatible(xml, Flame, status);

    width := Flame.Width;
    height := Flame.Height;
    ratio := width / height;

    if (width < height) then begin
      width := ratio * ThumbnailSize;
      height := ThumbnailSize;
    end else if (width > height) then begin
      height := ThumbnailSize / ratio;
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
    Thumbnail.PixelFormat := pf24bit;
    Thumbnail.HandleType := bmDIB;
    Thumbnail.Width := ThumbnailSize;
    Thumbnail.Height := ThumbnailSize;
    Thumbnail.Canvas.Brush.Color := GetSysColor(5);
    Thumbnail.Canvas.FillRect(Rect(0, 0, ThumbnailSize, ThumbnailSize));
    Thumbnail.Canvas.Draw(round(ThumbnailSize / 2 - width / 2), round(ThumbnailSize / 2 - height / 2), renderer.GetImage);

    mImages.Add(Thumbnail, nil);

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
  mThumbnails := TFlameListThumbnailThread.Create;
  mThumbnails.Images.OnChange := OnThumbnailsChange;
end;

procedure TFlameListView.SetBatch(value: TBatch);
var
  i: Integer;
begin
  mBatch := value;
  mThumbnails.Batch := value;

  mIsUpdating := true;
  mList.Items.Clear;

  for i := 0 to value.Count - 1 do
  begin
    mList.AddItem(value.GetFlameNameAt(i), nil);
    mList.Items[i].ImageIndex := i;
  end;

  mIsUpdating := false;
  mList.Refresh;

  mThumbnails.Start;
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

  mIsUpdating := true;
  mThumbnails.Images.Clear;
  mIsUpdating := false;
  mList.Refresh;
end;

procedure TFlameListView.OnThumbnailsChange(sender: TObject);
begin
  if mIsUpdating then exit;
  mList.Refresh;
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
  mList.Selected := mList.Items[i];
end;

destructor TFlameListView.Destroy;
begin
  mThumbnails.Destroy;
  mBatch := nil;

  inherited Destroy;
end;

end.