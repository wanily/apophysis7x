unit CoreVariation;

interface

  type TCoreVariationData = record
    name: string;
    index: integer;
    supports3D: boolean;
  end;

  type ACoreVariationData = array of TCoreVariationData;

  type TCoreVariation = class

    private

      class var mData: ACoreVariationData;

    public

      class procedure RegisterCoreVariation(const name: string; const supports3D: boolean);
      class function GetInfoByIndex(const i: integer): TCoreVariationData;
      class function GetInfoByName(const name: string): TCoreVariationData;

      class function Count: integer;

  end;

implementation

class procedure TCoreVariation.RegisterCoreVariation(const name: string; const supports3D: boolean);
var
  j: integer;
begin

  if not Assigned(mData) then
    SetLength(mData, 1)
  else SetLength(mData, Length(mData) + 1);

  j := Length(mData) - 1;

  mData[j].name := name;
  mData[j].index := j;
  mData[j].supports3D := supports3D;

end;

class function TCoreVariation.GetInfoByIndex(const i: integer): TCoreVariationData;
begin
  if (i < 0) or (i >= Length(mData)) then
  begin
    Result.name := '';
    Result.index := -1;
    Result.supports3D := false;
    exit;
  end;

  Result := mData[i];
end;

class function TCoreVariation.GetInfoByName(const name: string): TCoreVariationData;
var
  i, j: integer;
begin

  i := -1;
  for j := 0 to Length(mData) - 1 do
  begin
    if (name = mData[j].name) then
    begin
      i := j;
      Break;
    end;
  end;

  Result := GetInfoByIndex(i);
end;

class function TCoreVariation.Count: integer;
begin
  Result := Length(mData);
end;

end.
