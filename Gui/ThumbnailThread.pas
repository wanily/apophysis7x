unit ThumbnailThread;

interface uses
  Classes,
  Windows,
  Graphics,
  Controls,
  ParameterIO;

type TThumbnailThread = class(TThread)

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

implementation uses
  SysUtils,
  RenderingInterface,
  ControlPoint;

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

constructor TThumbnailThread.Create;
begin
  mThumbnailSize := 120;
  mImages := TImageList.CreateSize(mThumbnailSize, mThumbnailSize);

  inherited Create(true);
end;

procedure TThumbnailThread.SetThumbnailSize(value: Integer);
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

procedure TThumbnailThread.SetBatch(value: TBatch);
begin
  mBatch := value;
  mImages.Clear;
end;

procedure TThumbnailThread.Execute;
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

destructor TThumbnailThread.Destroy;
begin
  mBatch := nil; // -x- not owned by this class, don't destroy!

  if Assigned(mImages) then
    mImages.Destroy;

  mImages := nil;

  inherited Destroy;
end;




end.
