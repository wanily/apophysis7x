object AdjustForm: TAdjustForm
  Left = 164
  Top = 279
  BorderIcons = [biSystemMenu, biMinimize]
  BorderStyle = bsSingle
  Caption = 'Adjustment'
  ClientHeight = 292
  ClientWidth = 467
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  Icon.Data = {
    0000010001001010000001002000680400001600000028000000100000002000
    0000010020000000000040040000000000000000000000000000000000000000
    0000000000000000000000000000000000000000000000000000000000000000
    0000000000000000000000000000000000000000000000000000000000000000
    0000000000000000000000000000000000000000000000000000000000000000
    000000000000000000000000000000000000000000000000000000000000B7A2
    93FF604632FF634935FF634935FF634935FF634935FF634935FF634935FF6349
    35FF634935FF634935FF684E3AFF00000000000000000000000000000000BDA8
    99FFFCFEFCFFE7E6E4FFE7E2DCFFE6DCD4FFE5D6CBFFE4CFC1FFE3CAB8FFE3C5
    B1FFE2C2ACFFC7AA98FF684E3AFF00000000000000000000000000000000C6B0
    A1FFFCFEFCFFD8C4B9FFD6C0B5FFD3BCAFFFFAEFE6FFCFB3A5FFCCAF9FFFCCAF
    9FFFF6DBC8FFC7AA98FF684E3AFF00000000000000000000000000000000CCB6
    A7FFFCFEFCFFFCFEFCFFFCFEFCFFFBFAF7FFFBF5EFFF75716EFF575451FF0000
    00FFD2C2B5FFCBB2A1FF684E3AFF00000000000000000000000000000000CCB6
    A7FFFCFEFCFFDBCAC0FFDAC8BDFFD8C4B9FFFBFAF7FFA39D98FFFDFEFEFF5193
    A9FF0E1216FFB8B1A8FF78604DFF00000000000000000000000000000000CCB6
    A7FFFCFEFCFFFCFEFCFFFCFEFCFFFCFEFCFFFCFEFCFFA39D98FF88B7C7FF74CE
    E2FF499AB2FF0E1216FF7C7266FFE5D6CB01000000000000000000000000CCB6
    A7FFFCFEFCFFDBCAC0FFDBCAC0FFDBCAC0FFFCFEFCFFC1B9B4FF4D9CB3FF8CE0
    EEFF62BFD7FF499AB2FF0E1216FF2D62753B000000000000000000000000CCB6
    A7FFFCFEFCFFFCFEFCFFFCFEFCFFFCFEFCFFFCFEFCFFFBF8F3FFD5CEC8FF57A0
    B5FF8CE0EEFF62BFD7FF499AB2FF0E1216FF2D62753B0000000000000000EBAC
    8DFFEAAA8CFFEAA989FFE9A27EFFE89971FFE68F63FFE58758FFE69265FFCDA9
    95FF65A6B7FF8CE0EEFF62BFD7FF499AB2FF0E1216FF2855663000000000EBAC
    8DFFFFC3A2FFFEBF9DFFFCBB98FFFBB692FFFAB08BFFF8AB84FFF8A67CFFF3A6
    80FFD2B09EFF73ACB9FF8CE0EEFF6CC4D9FF7D8686FF353590FF2F39834EEBAC
    8DFFEAAA8BFFEAAA8BFFEAA889FFE9A281FFE89C77FFE7946BFFE68C60FFE586
    55FFE68D5FFFCA997FFF7DB0BBFFCAB8ACFF7385D1FF5E6CADFF353590FF0000
    0000000000000000000000000000000000000000000000000000000000000000
    000000000000EBAC8D037F9FAD465E6CADFF708FDFFF5E76D0FF5E6CADFF0000
    0000000000000000000000000000000000000000000000000000000000000000
    0000000000000000000000000000687EC2495E6CADFF5E6CADFF6579BC300000
    0000000000000000000000000000000000000000000000000000000000000000
    000000000000000000000000000000000000000000000000000000000000FFFF
    0000FFFF0000000F0000000F0000000F0000000F0000000F0000000700000007
    000000030000000100000000000000000000FFC00000FFF00000FFFF0000}
  OldCreateOrder = False
  Position = poDefault
  OnActivate = FormActivate
  OnClose = FormClose
  OnCreate = FormCreate
  OnDestroy = FormDestroy
  OnShow = FormShow
  DesignSize = (
    467
    292)
  PixelsPerInch = 96
  TextHeight = 13
  object PrevPnl: TPanel
    Left = 8
    Top = 5
    Width = 216
    Height = 148
    Cursor = crCross
    BevelOuter = bvLowered
    Color = clAppWorkSpace
    TabOrder = 0
    OnDblClick = PreviewImageDblClick
    OnMouseDown = PreviewImageMouseDown
    OnMouseMove = PreviewImageMouseMove
    OnMouseUp = PreviewImageMouseUp
    object PreviewImage: TImage
      Left = 1
      Top = 1
      Width = 210
      Height = 142
      IncrementalDisplay = True
      PopupMenu = QualityPopup
      Proportional = True
      OnDblClick = PreviewImageDblClick
      OnMouseDown = PreviewImageMouseDown
      OnMouseMove = PreviewImageMouseMove
      OnMouseUp = PreviewImageMouseUp
    end
  end
  object PageControl: TPageControl
    Left = 8
    Top = 157
    Width = 451
    Height = 130
    ActivePage = TabSheet3
    Anchors = [akLeft, akTop, akRight, akBottom]
    TabOrder = 1
    object TabSheet1: TTabSheet
      Caption = 'Camera'
      ImageIndex = 18
      DesignSize = (
        443
        102)
      object scrollZoom: TScrollBar
        Left = 112
        Top = 7
        Width = 259
        Height = 15
        Anchors = [akLeft, akTop, akRight]
        LargeChange = 100
        Max = 3000
        Min = -3000
        PageSize = 0
        SmallChange = 10
        TabOrder = 0
        OnChange = scrollZoomChange
        OnScroll = scrollZoomScroll
      end
      object txtZoom: TEdit
        Left = 380
        Top = 4
        Width = 63
        Height = 21
        Anchors = [akTop, akRight]
        TabOrder = 1
        Text = '0'
        OnChange = txtZoomChange
        OnEnter = txtZoomEnter
        OnExit = txtZoomExit
        OnKeyPress = txtZoomKeyPress
      end
      object scrollCenterX: TScrollBar
        Left = 112
        Top = 31
        Width = 259
        Height = 15
        Anchors = [akLeft, akTop, akRight]
        LargeChange = 100
        Max = 10000
        Min = -10000
        PageSize = 0
        SmallChange = 10
        TabOrder = 2
        OnChange = scrollCenterXChange
        OnScroll = scrollCenterXScroll
      end
      object txtCenterX: TEdit
        Left = 380
        Top = 28
        Width = 63
        Height = 21
        Anchors = [akTop, akRight]
        TabOrder = 3
        Text = '0'
        OnEnter = txtCenterXEnter
        OnExit = txtCenterXExit
        OnKeyPress = txtCenterXKeyPress
      end
      object scrollCenterY: TScrollBar
        Left = 112
        Top = 55
        Width = 259
        Height = 15
        Anchors = [akLeft, akTop, akRight]
        LargeChange = 100
        Max = 10000
        Min = -10000
        PageSize = 0
        SmallChange = 10
        TabOrder = 4
        OnChange = scrollCenterYChange
        OnScroll = scrollCenterYScroll
      end
      object txtCenterY: TEdit
        Left = 380
        Top = 52
        Width = 63
        Height = 21
        Anchors = [akTop, akRight]
        TabOrder = 5
        Text = '0'
        OnEnter = txtCenterYEnter
        OnExit = txtCenterYExit
        OnKeyPress = txtCenterYKeyPress
      end
      object scrollAngle: TScrollBar
        Left = 112
        Top = 79
        Width = 259
        Height = 15
        Anchors = [akLeft, akTop, akRight]
        LargeChange = 1500
        Max = 36000
        Min = -36000
        PageSize = 0
        SmallChange = 100
        TabOrder = 6
        OnChange = scrollAngleChange
        OnScroll = scrollAngleScroll
      end
      object txtAngle: TEdit
        Left = 380
        Top = 76
        Width = 63
        Height = 21
        Anchors = [akTop, akRight]
        TabOrder = 7
        Text = '0'
        OnEnter = txtAngleEnter
        OnExit = txtAngleExit
        OnKeyPress = txtAngleKeyPress
      end
      object pnlZoom: TPanel
        Left = 4
        Top = 4
        Width = 101
        Height = 21
        Cursor = crHandPoint
        Hint = 'Click and drag to change value'
        BevelOuter = bvLowered
        Caption = 'Zoom'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 8
        OnDblClick = DragPanelDblClick
        OnMouseDown = DragPanelMouseDown
        OnMouseMove = DragPanelMouseMove
        OnMouseUp = DragPanelMouseUp
      end
      object pnlXpos: TPanel
        Left = 4
        Top = 28
        Width = 101
        Height = 21
        Cursor = crHandPoint
        Hint = 'Click and drag to change value'
        BevelOuter = bvLowered
        Caption = 'X position'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 9
        OnDblClick = DragPanelDblClick
        OnMouseDown = DragPanelMouseDown
        OnMouseMove = DragPanelMouseMove
        OnMouseUp = DragPanelMouseUp
      end
      object pnlYpos: TPanel
        Left = 4
        Top = 52
        Width = 101
        Height = 21
        Cursor = crHandPoint
        Hint = 'Click and drag to change value'
        BevelOuter = bvLowered
        Caption = 'Y position'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 10
        OnDblClick = DragPanelDblClick
        OnMouseDown = DragPanelMouseDown
        OnMouseMove = DragPanelMouseMove
        OnMouseUp = DragPanelMouseUp
      end
      object pnlAngle: TPanel
        Left = 4
        Top = 76
        Width = 101
        Height = 21
        Cursor = crHandPoint
        Hint = 'Click and drag to change value'
        BevelOuter = bvLowered
        Caption = 'Rotation'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 11
        OnDblClick = DragPanelDblClick
        OnMouseDown = DragPanelMouseDown
        OnMouseMove = DragPanelMouseMove
        OnMouseUp = DragPanelMouseUp
      end
    end
    object TabSheet2: TTabSheet
      Caption = 'Rendering'
      ImageIndex = 35
      DesignSize = (
        443
        102)
      object pnlGamma: TPanel
        Left = 4
        Top = 4
        Width = 101
        Height = 21
        Cursor = crHandPoint
        Hint = 'Click and drag to change value'
        BevelOuter = bvLowered
        Caption = 'Gamma'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 8
        OnDblClick = DragPanelDblClick
        OnMouseDown = DragPanelMouseDown
        OnMouseMove = DragPanelMouseMove
        OnMouseUp = DragPanelMouseUp
      end
      object scrollGamma: TScrollBar
        Left = 112
        Top = 7
        Width = 259
        Height = 15
        Anchors = [akLeft, akTop, akRight]
        LargeChange = 10
        Max = 500
        Min = 100
        PageSize = 0
        Position = 500
        TabOrder = 0
        OnChange = scrollGammaChange
        OnScroll = scrollGammaScroll
      end
      object txtGamma: TEdit
        Left = 372
        Top = 4
        Width = 63
        Height = 21
        Anchors = [akTop, akRight]
        TabOrder = 1
        Text = '0'
        OnEnter = txtGammaEnter
        OnExit = txtGammaExit
        OnKeyPress = txtGammaKeyPress
      end
      object scrollBrightness: TScrollBar
        Left = 112
        Top = 31
        Width = 259
        Height = 15
        Anchors = [akLeft, akTop, akRight]
        LargeChange = 100
        Max = 10000
        PageSize = 0
        TabOrder = 2
        OnChange = scrollBrightnessChange
        OnScroll = scrollBrightnessScroll
      end
      object txtBrightness: TEdit
        Left = 372
        Top = 28
        Width = 63
        Height = 21
        Anchors = [akTop, akRight]
        TabOrder = 3
        Text = '0'
        OnEnter = txtBrightnessEnter
        OnExit = txtBrightnessExit
        OnKeyPress = txtBrightnessKeyPress
      end
      object scrollVibrancy: TScrollBar
        Left = 112
        Top = 55
        Width = 259
        Height = 15
        Anchors = [akLeft, akTop, akRight]
        LargeChange = 10
        Max = 1000
        PageSize = 0
        TabOrder = 4
        OnChange = scrollVibrancyChange
        OnScroll = scrollVibrancyScroll
      end
      object txtVibrancy: TEdit
        Left = 372
        Top = 52
        Width = 63
        Height = 21
        Anchors = [akTop, akRight]
        TabOrder = 5
        Text = '0'
        OnEnter = txtVibrancyEnter
        OnExit = txtVibrancyExit
        OnKeyPress = txtVibrancyKeyPress
      end
      object chkTransparent: TCheckBox
        Left = 112
        Top = 78
        Width = 81
        Height = 17
        Caption = 'Transparent'
        Enabled = False
        TabOrder = 7
        Visible = False
      end
      object pnlBrightness: TPanel
        Left = 4
        Top = 28
        Width = 101
        Height = 21
        Cursor = crHandPoint
        Hint = 'Click and drag to change value'
        BevelOuter = bvLowered
        Caption = 'Brightness'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 9
        OnDblClick = DragPanelDblClick
        OnMouseDown = DragPanelMouseDown
        OnMouseMove = DragPanelMouseMove
        OnMouseUp = DragPanelMouseUp
      end
      object pnlVibrancy: TPanel
        Left = 4
        Top = 52
        Width = 101
        Height = 21
        Cursor = crHandPoint
        Hint = 'Click and drag to change value'
        BevelOuter = bvLowered
        Caption = 'Vibrancy'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 10
        OnDblClick = DragPanelDblClick
        OnMouseDown = DragPanelMouseDown
        OnMouseMove = DragPanelMouseMove
        OnMouseUp = DragPanelMouseUp
      end
      object pnlGammaThreshold: TPanel
        Left = 250
        Top = 76
        Width = 121
        Height = 21
        Cursor = crHandPoint
        Hint = 'Click and drag to change value'
        Anchors = [akTop, akRight]
        BevelOuter = bvLowered
        Caption = ' Gamma threshold'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 11
        OnDblClick = DragPanelDblClick
        OnMouseDown = DragPanelMouseDown
        OnMouseMove = DragPanelMouseMove
        OnMouseUp = DragPanelMouseUp
      end
      object txtGammaThreshold: TEdit
        Left = 372
        Top = 76
        Width = 63
        Height = 21
        Anchors = [akTop, akRight]
        TabOrder = 12
        Text = '0'
        OnEnter = txtGammaThresholdEnter
        OnExit = txtGammaThresholdExit
        OnKeyPress = txtGammaThresholdKeyPress
      end
      object ColorPanel: TPanel
        Left = 112
        Top = 76
        Width = 131
        Height = 21
        Cursor = crHandPoint
        Anchors = [akLeft, akTop, akRight]
        BevelInner = bvRaised
        BevelOuter = bvLowered
        BorderStyle = bsSingle
        Color = clBlack
        TabOrder = 6
        OnClick = ColorPanelClick
        object Shape1: TShape
          Left = 2
          Top = 2
          Width = 123
          Height = 13
          Align = alClient
          Brush.Color = clBlack
          Pen.Style = psClear
          OnMouseUp = Shape1MouseUp
        end
      end
      object pnlBackground: TPanel
        Left = 4
        Top = 76
        Width = 101
        Height = 21
        Cursor = crArrow
        BevelOuter = bvLowered
        Caption = 'Background'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 13
        OnDblClick = DragPanelDblClick
        OnMouseDown = DragPanelMouseDown
        OnMouseMove = DragPanelMouseMove
        OnMouseUp = DragPanelMouseUp
      end
    end
    object TabSheet3: TTabSheet
      Caption = 'Gradient'
      ImageIndex = 11
      DesignSize = (
        443
        102)
      object lblVal: TLabel
        Left = 430
        Top = 79
        Width = 6
        Height = 13
        Caption = '0'
        Visible = False
      end
      object btnMenu: TSpeedButton
        Left = 4
        Top = 36
        Width = 109
        Height = 21
        Hint = 'Click for menu'
        Anchors = [akLeft, akBottom]
        Caption = 'Rotate'
        Glyph.Data = {
          5E040000424D5E04000000000000360400002800000005000000050000000100
          08000000000028000000120B0000120B0000000100000000000000000000FFFF
          FF00DEDAD800FFFFFF0000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000000000000000
          0000000000000000000000000000000000000000000000000000020202020200
          0000020200020200000002000000020000000000000000000000020202020200
          0000}
        Layout = blGlyphRight
        ParentShowHint = False
        ShowHint = True
        OnClick = btnMenuClick
        ExplicitTop = 46
      end
      object btnOpen: TSpeedButton
        Left = 396
        Top = 61
        Width = 23
        Height = 21
        Hint = 'Open Gradient Browser'
        Anchors = [akRight, akBottom]
        Glyph.Data = {
          36030000424D3603000000000000360000002800000010000000100000000100
          18000000000000030000120B0000120B00000000000000000000FF00FFFF00FF
          FF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00
          FFFF00FFFF00FFFF00FFFF00FFFF00FFB7A29363493563493563493563493563
          4935634935634935634935634935634935FF00FFFF00FFFF00FFFF00FFFF00FF
          B7A293FFFFFFB7A293B7A293B7A293B7A293B7A293B7A293B7A293B7A2936349
          35FF00FFFF00FFFF00FFFF00FFFF00FFB7A293FFFFFFFFFFFFFCFAF9F7F1EEF1
          E7E1ECDDD5E6D3C9E1CABDB7A293634935FF00FFFF00FFFF00FFFF00FFFF00FF
          B7A293FFFFFFFFFFFFFEFEFEFAF7F5F5EDE9EFE3DCEAD9D1E4CFC4B7A2936349
          35FF00FFFF00FFFF00FFFF00FFFF00FFB7A293FFFFFF4454FA9A7CE8C52BBFE1
          769FF6EB6DF6E242E7D5CBB7A293634935FF00FFFF00FFFF00FFFF00FFFF00FF
          BAA596FFFFFF7370FFB489F4D432D8E877BDF6EB99F6E581EBDBD3B7A2936349
          35FF00FFFF00FFFF00FFFF00FFFF00FFBEA99AFFFFFF38BFFF76D4DB42D87A84
          B776E27A83D55963EEE1DAB7A293634935FF00FFFF00FFFF00FFFF00FFFF00FF
          C3AE9EFFFFFF03CFFF53E3D20DED5A6AC35AF37974E95651F1E7E1B7A2936349
          35FF00FFFF00FFFF00FFFF00FFFF00FFC8B2A3FFFFFFFFFFFFFFFFFFFFFFFFFF
          FFFFFEFEFEFAF7F5F5EDE9B7A293634935FF00FFFF00FFFF00FFFF00FFFF00FF
          CCB6A7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFDFCFBB7A293B7A293644A
          36FF00FFFF00FFFF00FFFF00FFFF00FFD1BBABFFFFFFFFFFFFFFFFFFFFFFFFFF
          FFFFFFFFFFB7A293644A36644A36644A36FF00FFFF00FFFF00FFFF00FFFF00FF
          D5BFAFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFB9A495D4C5BA644A36E1D5
          CDFF00FFFF00FFFF00FFFF00FFFF00FFD8C2B2FFFFFFFFFFFFFFFFFFFFFFFFFF
          FFFFFFFFFFC0AB9C644A36E2D6CDFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF
          D8C2B2D8C2B2D8C2B2D8C2B2D8C2B2D4BEAECFB9A9C9B3A4E2D6CDFF00FFFF00
          FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF
          00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF}
        ParentShowHint = False
        ShowHint = True
        Transparent = False
        OnClick = btnOpenClick
        ExplicitTop = 71
      end
      object btnSmoothPalette: TSpeedButton
        Left = 419
        Top = 61
        Width = 23
        Height = 21
        Hint = 'Smooth Palette'
        Anchors = [akRight, akBottom]
        Glyph.Data = {
          36030000424D3603000000000000360000002800000010000000100000000100
          18000000000000030000120B0000120B00000000000000000000BEA99A634935
          6349356349356349356349356349356349356349356349356349356349356349
          35634935634935634935BFAA9BFEF0E8FEEFE6FEEDE3FEECE1FEEADFFEE9DDFE
          E7DBFFE6D9FFE5D7FFE3D5FFE2D3FFE1D2FFE0D0FFDFCF634935C0AB9CFEF1EA
          2C2C2C3C3C3C4E4E4E6161617676768989899E9E9EB1B1B1C3C3C3D3D3D3E2E2
          E2EEEEEEFFE0D0634935C1AB9CFEF3EC2C2C2C3C3C3C4E4E4E61616176767689
          89899E9E9EB1B1B1C3C3C3D3D3D3E2E2E2EEEEEEFFE1D2634935C2AC9DFEF4EE
          2C2C2C3C3C3C4E4E4E6161617676768989899E9E9EB1B1B1C3C3C3D3D3D3E2E2
          E2EEEEEEFFE2D3634935C2AD9EFEF6F02C2C2C3C3C3C4E4E4E61616176767689
          89899E9E9EB1B1B1C3C3C3D3D3D3E2E2E2EEEEEEFFE3D5634935C3AE9FFDF7F2
          2C2C2C3C3C3C4E4E4E6161617676768989899E9E9EB1B1B1C3C3C3D3D3D3E2E2
          E2EEEEEEFFE5D7634935C5AFA0FDF8F42C2C2C3C3C3C4E4E4E61616176767689
          89899E9E9EB1B1B1C3C3C3D3D3D3E2E2E2EEEEEEFFE6D9634935C6B0A1FDFAF6
          2C2C2C3C3C3C4E4E4E6161617676768989899E9E9EB1B1B1C3C3C3D3D3D3E2E2
          E2EEEEEEFEE7DB634935C7B1A2FDFBF82C2C2C3C3C3C4E4E4E61616176767689
          89899E9E9EB1B1B1C3C3C3D3D3D3E2E2E2EEEEEEFEE9DD634935C7B2A3FDFCF9
          2C2C2C3C3C3C4E4E4E6161617676768989899E9E9EB1B1B1C3C3C3D3D3D3E2E2
          E2EEEEEEFEEADF634935C8B3A4FDFDFB2C2C2C3C3C3C4E4E4E61616176767689
          89899E9E9EB1B1B1C3C3C3D3D3D3E2E2E2EEEEEEFEECE1634935C9B3A4FDFEFC
          2C2C2C3C3C3C4E4E4E6161617676768989899E9E9EB1B1B1C3C3C3D3D3D3E2E2
          E2EEEEEEFEEDE3634935CAB4A5FDFEFD2C2C2C3C3C3C4E4E4E61616176767689
          89899E9E9EB1B1B1C3C3C3D3D3D3E2E2E2EEEEEEFEEFE6634935CBB5A6FDFFFE
          FDFEFDFDFEFCFDFDFBFDFCF9FDFBF8FDFAF6FDF8F4FDF7F2FEF6F0FEF4EEFEF3
          ECFEF1EAFEF0E8634935CBB5A6CBB5A6CAB4A5C9B3A4C8B3A4C7B2A3C7B1A2C6
          B0A1C5AFA0C3AE9FC2AD9EC2AC9DC1AB9CC0AB9CBFAA9BBEA99A}
        ParentShowHint = False
        ShowHint = True
        Transparent = False
        OnClick = mnuSmoothPaletteClick
        ExplicitTop = 71
      end
      object btnColorPreset: TSpeedButton
        Left = 4
        Top = 62
        Width = 109
        Height = 21
        Hint = 'Click to choose random preset'
        Anchors = [akLeft, akBottom]
        Caption = 'Preset'
        ParentShowHint = False
        ShowHint = True
        OnClick = btnColorPresetClick
        ExplicitTop = 72
      end
      object GradientPnl: TPanel
        Left = 0
        Top = 0
        Width = 443
        Height = 33
        Align = alTop
        Anchors = [akLeft, akTop, akRight, akBottom]
        BevelInner = bvRaised
        BevelOuter = bvLowered
        BorderStyle = bsSingle
        Color = clAppWorkSpace
        TabOrder = 0
        object GradientImage: TImage
          Left = 2
          Top = 2
          Width = 435
          Height = 25
          Cursor = crHandPoint
          Align = alClient
          PopupMenu = GradientPopup
          Stretch = True
          OnDblClick = GradientImageDblClick
          OnMouseDown = GradImageMouseDown
          OnMouseMove = GradImageMouseMove
          OnMouseUp = GradImageMouseUp
          ExplicitHeight = 36
        end
      end
      object ScrollBar: TScrollBar
        Left = 120
        Top = 39
        Width = 179
        Height = 15
        Anchors = [akLeft, akRight, akBottom]
        LargeChange = 16
        Max = 128
        Min = -128
        PageSize = 0
        TabOrder = 1
        OnChange = ScrollBarChange
        OnScroll = ScrollBarScroll
      end
      object cmbPalette: TComboBox
        Left = 120
        Top = 62
        Width = 270
        Height = 21
        BevelInner = bvLowered
        BevelOuter = bvRaised
        Style = csOwnerDrawFixed
        Anchors = [akLeft, akRight, akBottom]
        Color = clBlack
        DropDownCount = 20
        Font.Charset = ANSI_CHARSET
        Font.Color = clWhite
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = []
        ItemHeight = 15
        ParentFont = False
        ParentShowHint = False
        ShowHint = False
        TabOrder = 2
        OnChange = cmbPaletteChange
        OnDrawItem = cmbPaletteDrawItem
        Items.Strings = (
          'south-sea-bather'
          'sky-flesh'
          'blue-bather'
          'no-name'
          'pillows'
          'mauve-splat'
          'facial-treescape 6'
          'fasion-bug'
          'leafy-face'
          'mouldy-sun'
          'sunny-harvest'
          'peach-tree'
          'fire-dragon'
          'ice-dragon'
          'german-landscape'
          'no-name'
          'living-mud-bomb'
          'cars'
          'unhealthy-tan'
          'daffodil'
          'rose'
          'healthy-skin'
          'orange'
          'white-ivy'
          'summer-makeup'
          'glow-buzz'
          'deep-water'
          'afternoon-beach'
          'dim-beach'
          'cloudy-brick'
          'burning-wood'
          'aquatic-garden'
          'no-name'
          'fall-quilt'
          'night-blue-sky'
          'shadow-iris'
          'solid-sky'
          'misty-field'
          'wooden-highlight'
          'jet-tundra'
          'pastel-lime'
          'hell'
          'indian-coast'
          'dentist-decor'
          'greenland'
          'purple-dress'
          'no-name'
          'spring-flora'
          'andi'
          'gig-o835'
          'rie02'
          'rie05'
          'rie11'
          'etretat.ppm'
          'the-hollow-needle-at-etretat.ppm'
          'rouen-cathedral-sunset.ppm'
          'the-houses-of-parliament.ppm'
          'starry-night.ppm'
          'water-lilies-sunset.ppm'
          'gogh.chambre-arles.ppm'
          'gogh.entrance.ppm'
          'gogh.the-night-cafe.ppm'
          'gogh.vegetable-montmartre.ppm'
          'matisse.bonheur-vivre.ppm'
          'matisse.flowers.ppm'
          'matisse.lecon-musique.ppm'
          'modigliani.nude-caryatid.ppm'
          'braque.instruments.ppm'
          'calcoast09.ppm'
          'dodge102.ppm'
          'ernst.anti-pope.ppm'
          'ernst.ubu-imperator.ppm'
          'fighting-forms.ppm'
          'fog25.ppm'
          'geyser27.ppm'
          'gris.josette.ppm'
          'gris.landscape-ceret.ppm'
          'kandinsky.comp-9.ppm'
          'kandinsky.yellow-red-blue.ppm'
          'klee.insula-dulcamara.ppm'
          'nile.ppm'
          'picasso.jfille-chevre.ppm'
          'pollock.lavender-mist.ppm'
          'yngpaint.ppm')
      end
      object txtVal: TEdit
        Left = 306
        Top = 36
        Width = 49
        Height = 21
        Anchors = [akRight, akBottom]
        TabOrder = 3
        Text = '0'
        OnExit = txtValExit
        OnKeyPress = txtValKeyPress
      end
      object btnReset: TButton
        Left = 363
        Top = 36
        Width = 79
        Height = 21
        Anchors = [akRight, akBottom]
        Caption = 'Reset'
        TabOrder = 4
        OnClick = btnResetClick
      end
    end
    object TabSheet4: TTabSheet
      Caption = 'Image Size'
      ImageIndex = 51
      DesignSize = (
        443
        102)
      object Bevel2: TBevel
        Left = 4
        Top = 4
        Width = 173
        Height = 93
        Shape = bsFrame
      end
      object Bevel1: TBevel
        Left = 184
        Top = 4
        Width = 138
        Height = 87
        Anchors = [akLeft, akTop, akRight, akBottom]
        Shape = bsFrame
        ExplicitHeight = 94
      end
      object Bevel3: TBevel
        Left = 330
        Top = 4
        Width = 111
        Height = 93
        Anchors = [akTop, akRight]
        Shape = bsFrame
      end
      object btnSet1: TSpeedButton
        Left = 288
        Top = 12
        Width = 25
        Height = 25
        Anchors = [akTop, akRight]
        Glyph.Data = {
          36030000424D3603000000000000360000002800000010000000100000000100
          18000000000000030000120B0000120B00000000000000000000FF00FFFF00FF
          FF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00
          FFFF00FFFF00FFFF00FFFF00FFEFCECFC36465AA5556A452539F4F509A4D4D94
          4A4A8F4747894444834141783B3C783B3C7239396E3637FF00FFFF00FFCF6B6C
          F38E8FE68081AA4424473221C3B4ABC6BBB3CAC1BCCEC8C4564D489E3E339C3D
          36983931723939FF00FFFF00FFD16F70FF999AEC8687E68081715B4B473C348D
          7868EDE0D8F1E7E08F7F73A34135A2423C9C3D35783B3CFF00FFFF00FFD47576
          FF9FA0F59091EC8687715B4B000000473C34E9D9CEECDDD4857467AE4B43AA49
          44A3423C7D3E3EFF00FFFF00FFD77B7CFFA9AAFB9FA0F59394715B4B715B4B71
          5B4B715B4B7662527D6A5BBA5654B24F4CAA4944834141FF00FFFF00FFDB8384
          FFB3B4FFADAEFCA3A4F48E8FEC8687E68081DF797AD77172D16B6CC15D5CBA56
          54B2504C894444FF00FFFF00FFDF8A8BFFBBBCFFB6B7C96360C45E56BE584BB8
          523FB34D34AD4728A7411CA13B11C15D5CBA56548F4747FF00FFFF00FFE29192
          FFBDBECC6667FFFFFFFFFFFFFBF8F6F6EEEAF0E5DEEADBD2E5D1C6E1CABDA13B
          11C25D5C944A4AFF00FFFF00FFE59798FFBDBED36D6EFFFFFFFFFFFFFFFFFFFB
          F8F6F6EEEAF0E5DEEADBD2E5D1C6A7411CCC67679A4D4DFF00FFFF00FFE99E9F
          FFBDBEDC7677FFFFFFFFFFFFFFFFFFFFFFFFFBF8F6F6EEEAF0E5DEEADBD2AD47
          28D771729F4F50FF00FFFF00FFEDA6A7FFBDBEE68081FFFFFFFFFFFFFFFFFFFF
          FFFFFFFFFFFBF8F6F6EEEAF0E5DEB34D34DF797AA45253FF00FFFF00FFF0ACAD
          FFBDBEEF898AFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFBF8F6F6EEEAB852
          3F673333AA5556FF00FFFF00FFF3B2B3FFBDBEF89293FFFFFFFFFFFFFFFFFFFF
          FFFFFFFFFFFFFFFFFFFFFFFBF8F6BE584BB05859B05859FF00FFFF00FFF5B6B7
          F5B6B7F3B2B3F1ADAEEEA7A8EAA1A2E79A9BE49394E08E8FDD8788DA8081D67A
          7BD37475D16F70FF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF
          00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF}
        ParentShowHint = False
        ShowHint = True
        OnClick = btnSet1Click
      end
      object btnSet2: TSpeedButton
        Left = 288
        Top = 38
        Width = 25
        Height = 25
        Anchors = [akTop, akRight]
        Glyph.Data = {
          36030000424D3603000000000000360000002800000010000000100000000100
          18000000000000030000120B0000120B00000000000000000000FF00FFFF00FF
          FF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00
          FFFF00FFFF00FFFF00FFFF00FFEFCECFC36465AA5556A452539F4F509A4D4D94
          4A4A8F4747894444834141783B3C783B3C7239396E3637FF00FFFF00FFCF6B6C
          F38E8FE68081AA4424473221C3B4ABC6BBB3CAC1BCCEC8C4564D489E3E339C3D
          36983931723939FF00FFFF00FFD16F70FF999AEC8687E68081715B4B473C348D
          7868EDE0D8F1E7E08F7F73A34135A2423C9C3D35783B3CFF00FFFF00FFD47576
          FF9FA0F59091EC8687715B4B000000473C34E9D9CEECDDD4857467AE4B43AA49
          44A3423C7D3E3EFF00FFFF00FFD77B7CFFA9AAFB9FA0F59394715B4B715B4B71
          5B4B715B4B7662527D6A5BBA5654B24F4CAA4944834141FF00FFFF00FFDB8384
          FFB3B4FFADAEFCA3A4F48E8FEC8687E68081DF797AD77172D16B6CC15D5CBA56
          54B2504C894444FF00FFFF00FFDF8A8BFFBBBCFFB6B7C96360C45E56BE584BB8
          523FB34D34AD4728A7411CA13B11C15D5CBA56548F4747FF00FFFF00FFE29192
          FFBDBECC6667FFFFFFFFFFFFFBF8F6F6EEEAF0E5DEEADBD2E5D1C6E1CABDA13B
          11C25D5C944A4AFF00FFFF00FFE59798FFBDBED36D6EFFFFFFFFFFFFFFFFFFFB
          F8F6F6EEEAF0E5DEEADBD2E5D1C6A7411CCC67679A4D4DFF00FFFF00FFE99E9F
          FFBDBEDC7677FFFFFFFFFFFFFFFFFFFFFFFFFBF8F6F6EEEAF0E5DEEADBD2AD47
          28D771729F4F50FF00FFFF00FFEDA6A7FFBDBEE68081FFFFFFFFFFFFFFFFFFFF
          FFFFFFFFFFFBF8F6F6EEEAF0E5DEB34D34DF797AA45253FF00FFFF00FFF0ACAD
          FFBDBEEF898AFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFBF8F6F6EEEAB852
          3F673333AA5556FF00FFFF00FFF3B2B3FFBDBEF89293FFFFFFFFFFFFFFFFFFFF
          FFFFFFFFFFFFFFFFFFFFFFFBF8F6BE584BB05859B05859FF00FFFF00FFF5B6B7
          F5B6B7F3B2B3F1ADAEEEA7A8EAA1A2E79A9BE49394E08E8FDD8788DA8081D67A
          7BD37475D16F70FF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF
          00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF}
        ParentShowHint = False
        ShowHint = True
        OnClick = btnSet2Click
      end
      object btnSet3: TSpeedButton
        Left = 288
        Top = 64
        Width = 25
        Height = 25
        Anchors = [akTop, akRight]
        Glyph.Data = {
          36030000424D3603000000000000360000002800000010000000100000000100
          18000000000000030000120B0000120B00000000000000000000FF00FFFF00FF
          FF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00
          FFFF00FFFF00FFFF00FFFF00FFEFCECFC36465AA5556A452539F4F509A4D4D94
          4A4A8F4747894444834141783B3C783B3C7239396E3637FF00FFFF00FFCF6B6C
          F38E8FE68081AA4424473221C3B4ABC6BBB3CAC1BCCEC8C4564D489E3E339C3D
          36983931723939FF00FFFF00FFD16F70FF999AEC8687E68081715B4B473C348D
          7868EDE0D8F1E7E08F7F73A34135A2423C9C3D35783B3CFF00FFFF00FFD47576
          FF9FA0F59091EC8687715B4B000000473C34E9D9CEECDDD4857467AE4B43AA49
          44A3423C7D3E3EFF00FFFF00FFD77B7CFFA9AAFB9FA0F59394715B4B715B4B71
          5B4B715B4B7662527D6A5BBA5654B24F4CAA4944834141FF00FFFF00FFDB8384
          FFB3B4FFADAEFCA3A4F48E8FEC8687E68081DF797AD77172D16B6CC15D5CBA56
          54B2504C894444FF00FFFF00FFDF8A8BFFBBBCFFB6B7C96360C45E56BE584BB8
          523FB34D34AD4728A7411CA13B11C15D5CBA56548F4747FF00FFFF00FFE29192
          FFBDBECC6667FFFFFFFFFFFFFBF8F6F6EEEAF0E5DEEADBD2E5D1C6E1CABDA13B
          11C25D5C944A4AFF00FFFF00FFE59798FFBDBED36D6EFFFFFFFFFFFFFFFFFFFB
          F8F6F6EEEAF0E5DEEADBD2E5D1C6A7411CCC67679A4D4DFF00FFFF00FFE99E9F
          FFBDBEDC7677FFFFFFFFFFFFFFFFFFFFFFFFFBF8F6F6EEEAF0E5DEEADBD2AD47
          28D771729F4F50FF00FFFF00FFEDA6A7FFBDBEE68081FFFFFFFFFFFFFFFFFFFF
          FFFFFFFFFFFBF8F6F6EEEAF0E5DEB34D34DF797AA45253FF00FFFF00FFF0ACAD
          FFBDBEEF898AFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFBF8F6F6EEEAB852
          3F673333AA5556FF00FFFF00FFF3B2B3FFBDBEF89293FFFFFFFFFFFFFFFFFFFF
          FFFFFFFFFFFFFFFFFFFFFFFBF8F6BE584BB05859B05859FF00FFFF00FFF5B6B7
          F5B6B7F3B2B3F1ADAEEEA7A8EAA1A2E79A9BE49394E08E8FDD8788DA8081D67A
          7BD37475D16F70FF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF
          00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF}
        ParentShowHint = False
        ShowHint = True
        OnClick = btnSet3Click
      end
      object Label7: TLabel
        Left = 136
        Top = 30
        Width = 26
        Height = 13
        Caption = 'pixels'
        Visible = False
      end
      object Label6: TLabel
        Left = 120
        Top = 16
        Width = 15
        Height = 36
        Caption = '}'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -32
        Font.Name = 'Times New Roman'
        Font.Style = []
        ParentFont = False
        Visible = False
      end
      object btnPreset1: TButton
        Left = 192
        Top = 12
        Width = 95
        Height = 25
        Anchors = [akLeft, akTop, akRight]
        Caption = 'Preset 1'
        TabOrder = 0
        OnClick = btnPreset1Click
      end
      object btnPreset2: TButton
        Left = 192
        Top = 38
        Width = 95
        Height = 25
        Anchors = [akLeft, akTop, akRight]
        Caption = 'Preset 2'
        TabOrder = 1
        OnClick = btnPreset2Click
      end
      object btnPreset3: TButton
        Left = 192
        Top = 64
        Width = 95
        Height = 25
        Anchors = [akLeft, akTop, akRight]
        Caption = 'Preset 3'
        TabOrder = 2
        OnClick = btnPreset3Click
      end
      object btnApplySize: TBitBtn
        Left = 336
        Top = 64
        Width = 99
        Height = 25
        Anchors = [akLeft, akTop, akRight]
        Caption = 'Apply'
        TabOrder = 3
        OnClick = btnApplySizeClick
      end
      object chkMaintain: TCheckBox
        Left = 14
        Top = 70
        Width = 157
        Height = 19
        Anchors = [akLeft, akTop, akRight]
        Caption = 'Keep aspect ratio'
        TabOrder = 4
        OnClick = chkMaintainClick
      end
      object chkResizeMain: TCheckBox
        Left = 336
        Top = 10
        Width = 99
        Height = 34
        Alignment = taLeftJustify
        Anchors = [akLeft, akTop, akRight, akBottom]
        Caption = 'Resize Main Window'
        Checked = True
        State = cbChecked
        TabOrder = 7
        WordWrap = True
      end
      object pnlWidth: TPanel
        Left = 12
        Top = 12
        Width = 85
        Height = 21
        Cursor = crArrow
        BevelOuter = bvLowered
        Caption = 'Width'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 8
        OnDblClick = DragPanelDblClick
        OnMouseDown = DragPanelMouseDown
        OnMouseMove = DragPanelMouseMove
        OnMouseUp = DragPanelMouseUp
      end
      object pnlHeight: TPanel
        Left = 12
        Top = 38
        Width = 85
        Height = 21
        Cursor = crArrow
        BevelOuter = bvLowered
        Caption = 'Height'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 9
        OnDblClick = DragPanelDblClick
        OnMouseDown = DragPanelMouseDown
        OnMouseMove = DragPanelMouseMove
        OnMouseUp = DragPanelMouseUp
      end
      object txtHeight: TComboBox
        Left = 96
        Top = 38
        Width = 75
        Height = 21
        Anchors = [akLeft, akTop, akRight]
        TabOrder = 6
        Text = '384'
        OnChange = txtHeightChange
        OnKeyPress = txtSizeKeyPress
        Items.Strings = (
          '384'
          '400'
          '480'
          '512'
          '600'
          '768'
          '960'
          '1024')
      end
      object txtWidth: TComboBox
        Left = 96
        Top = 12
        Width = 75
        Height = 21
        Anchors = [akLeft, akTop, akRight]
        TabOrder = 5
        Text = '512'
        OnChange = txtWidthChange
        OnKeyPress = txtSizeKeyPress
        Items.Strings = (
          '512'
          '640'
          '800'
          '1024'
          '1280')
      end
    end
  end
  object pnlPitch: TPanel
    Left = 232
    Top = 34
    Width = 105
    Height = 21
    Cursor = crHandPoint
    Hint = 'Click and drag to change value'
    BevelOuter = bvLowered
    Caption = ' Pitch'
    ParentShowHint = False
    ShowHint = True
    TabOrder = 2
    OnDblClick = DragPanelDblClick
    OnMouseDown = DragPanelMouseDown
    OnMouseMove = DragPanelMouseMove
    OnMouseUp = DragPanelMouseUp
  end
  object pnlYaw: TPanel
    Left = 232
    Top = 58
    Width = 105
    Height = 21
    Cursor = crHandPoint
    Hint = 'Click and drag to change value'
    BevelOuter = bvLowered
    Caption = ' Yaw'
    ParentShowHint = False
    ShowHint = True
    TabOrder = 3
    OnDblClick = DragPanelDblClick
    OnMouseDown = DragPanelMouseDown
    OnMouseMove = DragPanelMouseMove
    OnMouseUp = DragPanelMouseUp
  end
  object pnlPersp: TPanel
    Left = 232
    Top = 106
    Width = 105
    Height = 21
    Cursor = crHandPoint
    Hint = 'Click and drag to change value'
    BevelOuter = bvLowered
    Caption = ' Perspective'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -11
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
    ParentShowHint = False
    ShowHint = True
    TabOrder = 4
    OnDblClick = DragPanelDblClick
    OnMouseDown = DragPanelMouseDown
    OnMouseMove = DragPanelMouseMove
    OnMouseUp = DragPanelMouseUp
  end
  object txtPitch: TEdit
    Left = 336
    Top = 34
    Width = 124
    Height = 21
    Anchors = [akLeft, akTop, akRight]
    TabOrder = 6
    Text = '0'
    OnEnter = txtCamEnter
    OnExit = txtCamPitchExit
    OnKeyPress = txtCamPitchKeyPress
  end
  object txtYaw: TEdit
    Left = 336
    Top = 58
    Width = 124
    Height = 21
    Anchors = [akLeft, akTop, akRight]
    TabOrder = 7
    Text = '0'
    OnEnter = txtCamEnter
    OnExit = txtCamYawExit
    OnKeyPress = txtCamYawKeyPress
  end
  object txtPersp: TEdit
    Left = 336
    Top = 106
    Width = 124
    Height = 21
    Anchors = [akLeft, akTop, akRight]
    TabOrder = 8
    Text = '0'
    OnEnter = txtCamEnter
    OnExit = txtCamDistExit
    OnKeyPress = txtCamDistKeyPress
  end
  object pnlMasterScale: TPanel
    Left = 232
    Top = 130
    Width = 105
    Height = 21
    Cursor = crHandPoint
    Hint = 'Click and drag to change value'
    BevelOuter = bvLowered
    Caption = ' Scale'
    ParentShowHint = False
    ShowHint = True
    TabOrder = 10
    OnDblClick = DragPanelDblClick
    OnMouseDown = DragPanelMouseDown
    OnMouseMove = DragPanelMouseMove
    OnMouseUp = DragPanelMouseUp
  end
  object editPPU: TEdit
    Left = 336
    Top = 130
    Width = 124
    Height = 21
    Anchors = [akLeft, akTop, akRight]
    TabOrder = 5
    Text = '0'
    OnExit = editPPUValidate
    OnKeyPress = editPPUKeyPress
  end
  object pnlZpos: TPanel
    Left = 232
    Top = 82
    Width = 105
    Height = 21
    Cursor = crHandPoint
    Hint = 'Click and drag to change value'
    BevelOuter = bvLowered
    Caption = ' Height'
    ParentShowHint = False
    ShowHint = True
    TabOrder = 11
    OnDblClick = DragPanelDblClick
    OnMouseDown = DragPanelMouseDown
    OnMouseMove = DragPanelMouseMove
    OnMouseUp = DragPanelMouseUp
  end
  object txtZpos: TEdit
    Left = 336
    Top = 82
    Width = 124
    Height = 21
    Anchors = [akLeft, akTop, akRight]
    TabOrder = 9
    Text = '0'
    OnEnter = txtCamEnter
    OnExit = txtCamDistExit
    OnKeyPress = txtCamDistKeyPress
  end
  object Panel1: TPanel
    Left = 8
    Top = 300
    Width = 401
    Height = 46
    BevelOuter = bvNone
    BorderStyle = bsSingle
    Color = clInfoBk
    Ctl3D = False
    ParentCtl3D = False
    TabOrder = 12
    object Label1: TLabel
      Left = 8
      Top = 8
      Width = 384
      Height = 26
      Caption = 
        'WARNING! Using the "Zoom" setting will drastically decrease rend' +
        'er speed. Use this setting only when you are sure what you are d' +
        'oing.'
      WordWrap = True
    end
  end
  object pnlDOF: TPanel
    Left = 232
    Top = 10
    Width = 105
    Height = 21
    Cursor = crHandPoint
    Hint = 'Click and drag to change value'
    BevelOuter = bvLowered
    Caption = ' Depth blur'
    ParentShowHint = False
    ShowHint = True
    TabOrder = 13
    OnDblClick = DragPanelDblClick
    OnMouseDown = DragPanelMouseDown
    OnMouseMove = DragPanelMouseMove
    OnMouseUp = DragPanelMouseUp
  end
  object txtDOF: TEdit
    Left = 336
    Top = 10
    Width = 123
    Height = 21
    Anchors = [akLeft, akTop, akRight]
    TabOrder = 14
    Text = '0'
    OnEnter = txtCamEnter
    OnExit = txtCamDofExit
    OnKeyPress = txtCamDofKeyPress
  end
  object QualityPopup: TPopupMenu
    Left = 168
    Top = 16
    object mnuLowQuality: TMenuItem
      Caption = 'Low Quality'
      RadioItem = True
      OnClick = mnuLowQualityClick
    end
    object mnuMediumQuality: TMenuItem
      Caption = 'Medium Quality'
      Checked = True
      RadioItem = True
      OnClick = mnuMediumQualityClick
    end
    object mnuHighQuality: TMenuItem
      Caption = 'High Quality'
      RadioItem = True
      OnClick = mnuHighQualityClick
    end
    object N8: TMenuItem
      Caption = '-'
    end
    object mnuInstantPreview: TMenuItem
      Caption = 'Instant Preview'
      Checked = True
      OnClick = mnuInstantPreviewClick
    end
  end
  object ColorDialog: TColorDialog
    Options = [cdFullOpen]
    Left = 200
    Top = 16
  end
  object GradientPopup: TPopupMenu
    Left = 168
    Top = 56
    object mnuRandomize: TMenuItem
      Caption = 'Randomize'
      OnClick = mnuRandomizeClick
    end
    object N7: TMenuItem
      Caption = '-'
    end
    object mnuInvert: TMenuItem
      Caption = 'Invert'
      OnClick = mnuInvertClick
    end
    object mnuReverse: TMenuItem
      Caption = '&Reverse'
      OnClick = mnuReverseClick
    end
    object N3: TMenuItem
      Caption = '-'
    end
    object mnuSmoothPalette: TMenuItem
      Caption = 'Smooth Palette...'
      ImageIndex = 34
      OnClick = mnuSmoothPaletteClick
    end
    object mnuGradientBrowser: TMenuItem
      Caption = 'Gradient Browser...'
      ImageIndex = 22
      OnClick = btnOpenClick
    end
    object N4: TMenuItem
      Caption = '-'
    end
    object SaveGradient1: TMenuItem
      Caption = 'Save Gradient...'
      ImageIndex = 2
      OnClick = SaveGradient1Click
    end
    object SaveasMapfile1: TMenuItem
      Caption = 'Save as Map file...'
      OnClick = SaveasMapfile1Click
    end
    object N6: TMenuItem
      Caption = '-'
    end
    object mnuSaveasDefault: TMenuItem
      Caption = 'Save as Default'
      OnClick = mnuSaveasDefaultClick
    end
    object N5: TMenuItem
      Caption = '-'
    end
    object mnuCopy: TMenuItem
      Caption = 'Copy'
      ImageIndex = 7
    end
    object mnuPaste: TMenuItem
      Caption = 'Paste'
      ImageIndex = 8
    end
  end
  object scrollModePopup: TPopupMenu
    AutoHotkeys = maManual
    AutoPopup = False
    Left = 200
    Top = 56
    object mnuRotate: TMenuItem
      Caption = 'Rotate'
      OnClick = mnuRotateClick
    end
    object N1: TMenuItem
      Caption = '-'
    end
    object mnuHue: TMenuItem
      Caption = 'Hue'
      OnClick = mnuHueClick
    end
    object mnuSaturation: TMenuItem
      Caption = 'Saturation'
      OnClick = mnuSaturationClick
    end
    object mnuBrightness: TMenuItem
      Caption = 'Brightness'
      OnClick = mnuBrightnessClick
    end
    object Contrast1: TMenuItem
      Caption = 'Contrast'
      OnClick = mnuContrastClick
    end
    object N2: TMenuItem
      Caption = '-'
    end
    object mnuBlur: TMenuItem
      Caption = 'Blur'
      OnClick = mnuBlurClick
    end
    object mnuFrequency: TMenuItem
      Caption = 'Frequency'
      OnClick = mnuFrequencyClick
    end
  end
  object SaveDialog: TSaveDialog
    DefaultExt = 'map'
    Filter = 'Map files|*.map'
    Left = 168
    Top = 96
  end
  object ApplicationEvents: TApplicationEvents
    OnActivate = ApplicationEventsActivate
    Left = 200
    Top = 96
  end
end
