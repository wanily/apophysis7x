object RenderForm: TRenderForm
  Left = 851
  Top = 205
  BorderIcons = [biSystemMenu, biMinimize]
  BorderStyle = bsSingle
  Caption = 'Render to Disk'
  ClientHeight = 409
  ClientWidth = 497
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnClose = FormClose
  OnCloseQuery = FormCloseQuery
  OnCreate = FormCreate
  OnDestroy = FormDestroy
  OnShow = FormShow
  DesignSize = (
    497
    409)
  PixelsPerInch = 96
  TextHeight = 13
  object btnRender: TButton
    Left = 256
    Top = 360
    Width = 75
    Height = 25
    Anchors = [akRight, akBottom]
    Caption = 'Start'
    Default = True
    TabOrder = 0
    OnClick = btnRenderClick
    ExplicitTop = 420
  end
  object btnCancel: TButton
    Left = 416
    Top = 360
    Width = 75
    Height = 25
    Anchors = [akRight, akBottom]
    Caption = 'Close'
    TabOrder = 1
    OnClick = btnCancelClick
    ExplicitTop = 420
  end
  object btnPause: TButton
    Left = 336
    Top = 360
    Width = 75
    Height = 25
    Anchors = [akRight, akBottom]
    Caption = 'Pause'
    TabOrder = 2
    OnClick = btnPauseClick
    ExplicitTop = 420
  end
  object PageCtrl: TPageControl
    Left = 8
    Top = 8
    Width = 481
    Height = 313
    ActivePage = TabSettings
    Anchors = [akLeft, akTop, akRight, akBottom]
    Images = MainForm.Buttons
    TabOrder = 3
    ExplicitHeight = 373
    object TabSettings: TTabSheet
      Caption = 'Settings'
      ImageIndex = 18
      ExplicitHeight = 344
      DesignSize = (
        473
        284)
      object btnBrowse: TSpeedButton
        Left = 416
        Top = 11
        Width = 24
        Height = 24
        Hint = 'Browse...'
        Anchors = [akTop, akRight]
        Flat = True
        Font.Charset = ANSI_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Arial'
        Font.Style = [fsBold]
        Glyph.Data = {
          36030000424D3603000000000000360000002800000010000000100000000100
          18000000000000030000120B0000120B00000000000000000000FF00FFFF00FF
          FF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00
          FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF
          00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF75848F66808F
          607987576E7B4E626F4456613948522E3A43252E351B222914191E0E12160E13
          18FF00FFFF00FFFF00FF77879289A1AB6AB2D4008FCD008FCD008FCD048CC708
          88BE0F82B4157CA91B779F1F7296224B5C87A2ABFF00FFFF00FF7A8A957EBED3
          8AA4AE7EDCFF5FCFFF55CBFF4CC4FA41BCF537B3F02EAAEB24A0E5138CD42367
          805E696DFF00FFFF00FF7D8E9879D2EC8BA4AD89C2CE71D8FF65D3FF5CCEFF51
          C9FE49C1FA3FB9F534B0EE29A8E91085CD224B5B98B2BAFF00FF80919C81D7EF
          7DC5E08CA6B080DDFE68D3FF67D4FF62D1FF58CDFF4EC7FC46BEF73BB6F231AC
          EC2569817A95A1FF00FF83959F89DCF18CE2FF8DA8B18CBAC774D8FF67D4FF67
          D4FF67D4FF5FD0FF54CDFF4BC5FC41BBF72EA2DB51677498B2BA869AA392E1F2
          98E8FD80C4DE8EA7B081DEFD84E0FF84E0FF84E0FF84E0FF81DFFF7BDDFF74D8
          FF6BD6FF56A9D18F9BA4889CA59AE6F39FEBFB98E8FE8BACB98BACB98AAAB788
          A6B386A3AF839FAA819AA67F95A17C919D7A8E99798B957788938BA0A8A0EAF6
          A6EEF99FEBFB98E8FE7ADAFF67D4FF67D4FF67D4FF67D4FF67D4FF67D4FF7788
          93FF00FFFF00FFFF00FF8EA2ABA7EEF6ABF0F7A6EEF99FEBFB98E8FD71D4FB89
          9EA78699A382949F7E909A7A8C97778893FF00FFFF00FFFF00FF8FA4ACA0D2DA
          ABF0F7ABF0F7A6EEF99FEBFB8DA1AAB5CBD0FF00FFFF00FFFF00FFFF00FFFF00
          FFFF00FFFF00FFFF00FFBDCED48FA4AC8FA4AC8FA4AC8FA4AC8FA4ACB5CBD0FF
          00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF
          FF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00
          FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF
          00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF}
        ParentFont = False
        ParentShowHint = False
        ShowHint = True
        OnClick = btnBrowseClick
      end
      object btnGoTo: TSpeedButton
        Left = 440
        Top = 11
        Width = 24
        Height = 24
        Hint = 'Browse...'
        Anchors = [akTop, akRight]
        Flat = True
        Font.Charset = ANSI_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Arial'
        Font.Style = [fsBold]
        Glyph.Data = {
          36030000424D3603000000000000360000002800000010000000100000000100
          18000000000000030000120B0000120B00000000000000000000FF00FFFF00FF
          FF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00
          FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF
          00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF
          FF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00
          FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF
          00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF
          FF00FFFF00FFFF00FFFF00FFFF00FF964924EADBD3FF00FFFF00FFFF00FFFF00
          FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF9D
          4D259D4E28EADBD3FF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF
          FF00FFCF835D9247239A4B25A24F27AB5429BF6A3FA0502AEADBD3FF00FFFF00
          FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFD78B65FDB089F7905CEC8856DE
          7F4FD17648C46E42A25631EADBD3FF00FFFF00FFFF00FFFF00FFFF00FFFF00FF
          FF00FFDE926CFCB997FDA578FC935EF28C59E58453D87B4CC66E41AE582BFF00
          FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFE49872FBC3A6FDBE9EFEAE85FF
          A87DF89D6FE58351AE582BF4E7E1FF00FFFF00FFFF00FFFF00FFFF00FFFF00FF
          FF00FFE89C76E29670DA8E68D1855FDB906AF79A6BAE582BF4E7E1FF00FFFF00
          FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFE5
          9973C5764EF3E6DFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF
          FF00FFFF00FFFF00FFFF00FFFF00FFE89C76F8EDE8FF00FFFF00FFFF00FFFF00
          FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF
          00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF
          FF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00
          FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF
          00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF}
        ParentFont = False
        ParentShowHint = False
        ShowHint = True
        OnClick = btnGoToClick
      end
      object GroupBox2: TGroupBox
        Left = 8
        Top = 69
        Width = 233
        Height = 95
        Anchors = [akLeft, akTop, akRight]
        Caption = 'Size'
        TabOrder = 0
        DesignSize = (
          233
          95)
        object chkMaintain: TCheckBox
          Left = 8
          Top = 70
          Width = 217
          Height = 17
          Anchors = [akLeft, akTop, akRight]
          Caption = 'Keep aspect ratio'
          Checked = True
          State = cbChecked
          TabOrder = 0
          OnClick = chkMaintainClick
        end
        object pnlWidth: TPanel
          Left = 8
          Top = 20
          Width = 113
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Width'
          TabOrder = 3
        end
        object pnlHeight: TPanel
          Left = 8
          Top = 44
          Width = 113
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Height'
          TabOrder = 4
        end
        object cbHeight: TComboBox
          Left = 120
          Top = 44
          Width = 105
          Height = 21
          Anchors = [akLeft, akTop, akRight]
          BiDiMode = bdLeftToRight
          Enabled = False
          ParentBiDiMode = False
          TabOrder = 2
          OnChange = txtHeightChange
          Items.Strings = (
            '200'
            '240'
            '480'
            '600'
            '768'
            '1024'
            '1200'
            '1920'
            '2048'
            '2400')
        end
        object cbWidth: TComboBox
          Left = 120
          Top = 20
          Width = 105
          Height = 21
          Anchors = [akLeft, akTop, akRight]
          BiDiMode = bdLeftToRight
          Enabled = False
          ParentBiDiMode = False
          TabOrder = 1
          OnChange = txtWidthChange
          Items.Strings = (
            '320'
            '640'
            '800'
            '1024'
            '1280'
            '1600'
            '1920'
            '2048'
            '2560'
            '3200')
        end
      end
      object GroupBox3: TGroupBox
        Left = 248
        Top = 69
        Width = 218
        Height = 95
        Anchors = [akTop, akRight]
        Caption = 'Quality settings'
        TabOrder = 1
        DesignSize = (
          218
          95)
        object udOversample: TUpDown
          Left = 196
          Top = 68
          Width = 13
          Height = 21
          Anchors = [akTop, akRight]
          Associate = txtOversample
          Min = 1
          Max = 16
          Position = 1
          TabOrder = 3
        end
        object pnlDensity: TPanel
          Left = 8
          Top = 20
          Width = 121
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Density'
          TabOrder = 4
        end
        object pnlFilter: TPanel
          Left = 8
          Top = 44
          Width = 121
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Filter radius'
          TabOrder = 5
        end
        object pnlOversample: TPanel
          Left = 8
          Top = 68
          Width = 121
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Oversample'
          TabOrder = 6
        end
        object txtDensity: TComboBox
          Left = 128
          Top = 20
          Width = 82
          Height = 21
          AutoComplete = False
          Anchors = [akLeft, akTop, akRight]
          TabOrder = 0
          OnChange = txtDensityChange
          OnCloseUp = txtDensityChange
          Items.Strings = (
            '200'
            '500'
            '1000'
            '2000'
            '4000')
        end
        object txtFilterRadius: TEdit
          Left = 128
          Top = 44
          Width = 82
          Height = 21
          Anchors = [akLeft, akTop, akRight]
          BiDiMode = bdRightToLeft
          ParentBiDiMode = False
          TabOrder = 1
          Text = '0.1'
          OnChange = txtFilterRadiusChange
        end
        object txtOversample: TEdit
          Left = 128
          Top = 68
          Width = 68
          Height = 21
          Anchors = [akLeft, akTop, akRight]
          BiDiMode = bdRightToLeft
          Enabled = False
          ParentBiDiMode = False
          ReadOnly = True
          TabOrder = 2
          Text = '1'
          OnChange = txtOversampleChange
        end
      end
      object GroupBox4: TGroupBox
        Left = 8
        Top = 172
        Width = 458
        Height = 99
        Anchors = [akLeft, akTop, akRight]
        Caption = 'Resource usage'
        TabOrder = 2
        DesignSize = (
          458
          99)
        object lblApproxMem: TLabel
          Left = 439
          Top = 100
          Width = 42
          Height = 13
          Alignment = taRightJustify
          Caption = '0000 Mb'
          Visible = False
        end
        object lblMaxbits: TLabel
          Left = 440
          Top = 116
          Width = 33
          Height = 13
          Hint = '- No render stats -'
          Alignment = taRightJustify
          Caption = '99.999'
          ParentShowHint = False
          ShowHint = True
          Visible = False
        end
        object Label9: TLabel
          Left = 440
          Top = 108
          Width = 96
          Height = 13
          Hint = '- No render stats -'
          Caption = 'Max bits per sample:'
          ParentShowHint = False
          ShowHint = True
          Visible = False
        end
        object lblMemory: TLabel
          Left = 11
          Top = 18
          Width = 442
          Height = 24
          Anchors = [akLeft, akTop, akRight]
          AutoSize = False
          Caption = 
            'The render process will use 0000 Mb of 0000MB available physical' +
            ' memory'
          Font.Charset = DEFAULT_CHARSET
          Font.Color = clWindowText
          Font.Height = -11
          Font.Name = 'MS Sans Serif'
          Font.Style = []
          ParentFont = False
          Layout = tlCenter
          WordWrap = True
        end
        object lblCPUCores: TLabel
          Left = 11
          Top = 43
          Width = 442
          Height = 14
          Anchors = [akLeft, akTop, akRight]
          AutoSize = False
          Font.Charset = DEFAULT_CHARSET
          Font.Color = clWindowText
          Font.Height = -11
          Font.Name = 'MS Sans Serif'
          Font.Style = []
          ParentFont = False
          Layout = tlCenter
          WordWrap = True
        end
        object chkLimitMem: TCheckBox
          Left = 444
          Top = 134
          Width = 125
          Height = 17
          Caption = 'Limit memory usage to:'
          TabOrder = 0
          Visible = False
        end
        object pnlLimit: TPanel
          Left = 8
          Top = 68
          Width = 121
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Memory limit'
          TabOrder = 2
        end
        object cbMaxMemory: TComboBox
          Left = 128
          Top = 68
          Width = 97
          Height = 21
          Style = csDropDownList
          BiDiMode = bdLeftToRight
          ItemIndex = 0
          ParentBiDiMode = False
          TabOrder = 1
          Text = 'No limit'
          OnChange = cbMaxMemoryChange
          Items.Strings = (
            'No limit'
            '32'
            '64'
            '128'
            '256'
            '512'
            '1024'
            '1536')
        end
        object PBMem: TProgressBar
          Left = 232
          Top = 68
          Width = 217
          Height = 21
          TabOrder = 3
        end
      end
      object pnlTarget: TPanel
        Left = 8
        Top = 12
        Width = 121
        Height = 21
        Cursor = crArrow
        BevelOuter = bvLowered
        Caption = 'Destination'
        TabOrder = 4
      end
      object txtFilename: TEdit
        Left = 128
        Top = 12
        Width = 288
        Height = 21
        Anchors = [akLeft, akTop, akRight]
        TabOrder = 3
        OnChange = txtFilenameChange
      end
      object chkBinary: TCheckBox
        Left = 8
        Top = 349
        Width = 457
        Height = 17
        Anchors = [akLeft, akTop, akRight]
        Caption = 
          'Write raw data (WARNING: this is experimental and slows down the' +
          ' rendering!!!)'
        Enabled = False
        TabOrder = 5
        Visible = False
      end
      object chkSaveIncompleteRenders: TCheckBox
        Left = 247
        Top = 41
        Width = 216
        Height = 17
        Anchors = [akLeft, akTop, akRight]
        Caption = 'Save incomplete renders'
        TabOrder = 6
        OnClick = chkSaveIncompleteRendersClick
      end
      object chkSave: TCheckBox
        Left = 8
        Top = 41
        Width = 233
        Height = 17
        Anchors = [akLeft, akTop, akRight]
        Caption = 'Save parameters'
        Checked = True
        State = cbChecked
        TabOrder = 7
      end
    end
    object TabOutput: TTabSheet
      Caption = 'Output'
      ImageIndex = 38
      ExplicitHeight = 344
      object Output: TMemo
        Left = 0
        Top = 0
        Width = 473
        Height = 284
        Align = alClient
        BorderStyle = bsNone
        Color = clBtnFace
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clBtnText
        Font.Height = -11
        Font.Name = 'Courier New'
        Font.Style = []
        ParentFont = False
        ReadOnly = True
        ScrollBars = ssVertical
        TabOrder = 0
        ExplicitHeight = 344
      end
    end
  end
  object StatusBar: TStatusBar
    Left = 0
    Top = 390
    Width = 497
    Height = 19
    Panels = <
      item
        Width = 161
      end
      item
        Width = 150
      end
      item
        Width = 50
      end>
    ExplicitTop = 450
  end
  object ProgressBar2: TProgressBar
    Left = 8
    Top = 328
    Width = 481
    Height = 25
    Anchors = [akLeft, akRight, akBottom]
    TabOrder = 5
    ExplicitTop = 388
  end
  object SaveDialog: TSaveDialog
    Left = 168
    Top = 464
  end
end
