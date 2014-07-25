object OptionsForm: TOptionsForm
  Left = 899
  Top = 428
  BorderIcons = [biSystemMenu, biMinimize, biMaximize, biHelp]
  BorderStyle = bsSingle
  Caption = 'Options'
  ClientHeight = 479
  ClientWidth = 487
  Color = clBtnFace
  Font.Charset = ANSI_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  HelpFile = 'Apophysis 2.0.chm'
  Icon.Data = {
    0000010001001010000001002000680400001600000028000000100000002000
    0000010020000000000040040000000000000000000000000000000000000000
    0000000000000000000000000000000000000000000000000000000000000000
    0000000000000000000000000000000000000000000000000000000000007C75
    73FF4E4B4BFF544F4DFF544F4DFF544F4DFF56514DFF53504CFF524F4BFF524F
    4CFF524F4BFF514E4BFF544E46FF00000000000000000000000000000000D9CF
    C8FFFFFFFFFFFBFDFFFFFBFDFFFFFFFFF6FFFFFFF6FFFFFFF6FFFFF9ECFFFFF0
    E1FFFFEAD5FFFFEAD2FF544E46FF00000000000000000000000000000000D2C8
    C1FFFFFFFFFFC4AFA2FFC4AFA2FFFFFBF9FFC4AFA2FFC4AFA2FFC4AFA2FFC4AF
    A2FFC4AFA2FFFDDECBFF544E46FF00000000000000000000000000000000D2C8
    C1FFFFFFFFFFFBFDFFFFFBFDFFFFFFFEFCFFFDFAF8FFFBF4EFFFFBEEE6FFFAE9
    DEFFF8E2D2FFFFE2D0FF544E46FF00000000000000000000000000000000D3C9
    C2FFFFFFFFFFC4AFA2FFC4AFA2FFFFFFFEFFC4AFA2FFC4AFA2FFC4AFA2FFC4AF
    A2FFC4AFA2FFFFE5D6FF544E46FF00000000000000000000000000000000D1C7
    C0FFFFFFFFFFFBFDFFFFFBFDFFFFFFFFFFFFFEFEFDFFFEFBF8FFFDF6F2FFFCF0
    E8FFFCF0E8FFFFE9DCFF544E46FF00000000000000000000000000000000D1C8
    C1FFFFFFFFFFFBFDFFFFFBFDFFFFFFFFFFFFFFFFFFFFFEFEFCFFB0ADACFF415C
    72FFE3D9D3FFFFEDE3FF544E46FF00000000000000000000000000000000D1C8
    C1FFFFFFFFFFE2E9E9FF5E7584FFDFE4E5FFFFFFFFFFC2CACEFF4A6170FF2EA9
    D6FF0B101BFF5D5C60FFA49D96FF00000000000000001C6629791C6629FFDF9D
    7DFFF1CAB7FF8FA4ACFF86D3E5FF4B6170FFA79289FF4A6170FF61C1DEFF574D
    59FF1FD0FFFF152733FF10070AFF02212EFF4F5665FF59785BFF188C32FFDF9D
    7DFFFFC5A4FFE5C9B9FF8FA4ACFF83E1F6FF4B6170FF7ACDE2FF526067FF68ED
    FFFF413D50FF32B2DFFF1D99C8FF1593C4FF14628EFF406651FF29973FFFDF9D
    7DFFDF9D7DFFDF9D7DFFDABAAAFF8FA4ACFF7FE3F9FF538495FF68EDFFFF303A
    4FFF69DBF6FF58D2F3FF40C3EDFF31BBEAFF11A8ECFF50908CFF329E41FF0000
    0000000000000000000000000000869BA43341576FE168EDFFFF5898AEFF6EEB
    FFFF72E1F9FF6ADDF7FF56CFF2FF4BC7EDFF22BAFAFF5FA2A6FF41AC53FF0000
    0000000000000000000000000000A2AAB7A686E0F7A642566BDD6EEBFFFF6EEB
    FFFF6EEBFFFF72E2FAFF67D7F4FF54BDDCFF51728BFF699C89FF85CC85FF0000
    0000000000000000000096AAB005939EACDA86AEBCE06778854880A5B4FC8097
    A3FF8096A0FF7A8F99FF738593FF5B7080FE7A8B967A41AC538741AC53C00000
    0000000000000000000000000000000000000000000000000000000000000000
    000000000000000000000000000000000000000000000000000000000000FFFF
    0000000F0000000F0000000F0000000F0000000F0000000F0000000F0000000C
    0000000000000000000000000000F0000000F0000000E0000000FFFF0000}
  OldCreateOrder = True
  Position = poDefault
  OnClose = FormClose
  OnCreate = FormCreate
  OnShow = FormShow
  DesignSize = (
    487
    479)
  PixelsPerInch = 96
  TextHeight = 13
  object Label45: TLabel
    Left = 16
    Top = 600
    Width = 244
    Height = 26
    Caption = 
      'You must restart Apophysis when changed thumbnail size to see ef' +
      'fect!'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -11
    Font.Name = 'Tahoma'
    Font.Style = [fsBold]
    ParentFont = False
    Visible = False
    WordWrap = True
  end
  object pnlWarning: TPanel
    Left = -1
    Top = -1
    Width = 490
    Height = 41
    Anchors = [akLeft, akTop, akRight]
    BevelKind = bkFlat
    BevelOuter = bvNone
    Color = clInfoBk
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -11
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentBackground = False
    ParentFont = False
    TabOrder = 4
    object cbMultithreadedPreviewWarning: TLabel
      Left = 7
      Top = 5
      Width = 475
      Height = 28
      Alignment = taCenter
      AutoSize = False
      Caption = 
        'WARNING: multithreaded previews are an experimental and might ca' +
        'use problems while using Apophysis 7X. Please disable this setti' +
        'ng in case of excessive memory usage or slowdowns.'
      Font.Charset = ANSI_CHARSET
      Font.Color = clInfoText
      Font.Height = -11
      Font.Name = 'Tahoma'
      Font.Style = []
      ParentFont = False
      WordWrap = True
    end
  end
  object GroupBox15: TGroupBox
    Left = 64
    Top = 108
    Width = 297
    Height = 69
    Caption = 'On render complete'
    TabOrder = 2
    object btnBrowseSound: TSpeedButton
      Left = 264
      Top = 41
      Width = 24
      Height = 24
      Hint = 'Browse...'
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
      OnClick = btnBrowseSoundClick
    end
    object btnPlay: TSpeedButton
      Left = 264
      Top = 14
      Width = 24
      Height = 24
      Hint = 'Play'
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
        FF00FFFF00FFFF00FFDEEAE0FF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00
        FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF096314DEEAE0FF
        00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF
        FF00FFFF00FFFF00FF11681B04600FDEEAE0FF00FFFF00FFFF00FFFF00FFFF00
        FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF1A6F2420732C04
        600FDEEAE0FF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF
        FF00FFFF00FFFF00FF23752E2F833D20732C04600FDEEAE0FF00FFFF00FFFF00
        FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF2E7C3750A25A2F
        833D20732C04600FDEEAE0FF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF
        FF00FFFF00FFFF00FF3883415DB06850A25A2F833D20732C0B6618DEEAE0FF00
        FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF438A4C6BBF766B
        BF7650A25A2F7639D6EDD9FF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF
        FF00FFFF00FFFF00FF4B90536BBF76A3DAB02F7639D6EDD9FF00FFFF00FFFF00
        FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF52945AA3DAB02F
        7639D6EDD9FF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF
        FF00FFFF00FFFF00FF5898602F7639D6EDD9FF00FFFF00FFFF00FFFF00FFFF00
        FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF589860D6EDD9FF
        00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF
        FF00FFFF00FFFF00FFD6EDD9FF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00
        FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF
        00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FFFF00FF}
      ParentFont = False
      ParentShowHint = False
      ShowHint = True
      OnClick = btnPlayClick
    end
    object Label44: TLabel
      Left = 10
      Top = 44
      Width = 49
      Height = 13
      Caption = 'File name:'
    end
    object txtSoundFile: TEdit
      Left = 64
      Top = 42
      Width = 193
      Height = 21
      HelpContext = 1000
      ParentShowHint = False
      ShowHint = False
      TabOrder = 0
    end
    object chkPlaysound: TCheckBox
      Left = 8
      Top = 18
      Width = 81
      Height = 17
      Caption = 'Play sound'
      TabOrder = 1
    end
  end
  object Tabs: TPageControl
    Left = 8
    Top = 49
    Width = 475
    Height = 396
    ActivePage = GeneralPage
    Anchors = [akLeft, akRight, akBottom]
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -11
    Font.Name = 'Tahoma'
    Font.Style = []
    MultiLine = True
    ParentFont = False
    TabOrder = 3
    TabStop = False
    object GeneralPage: TTabSheet
      HelpContext = 1
      Caption = 'General'
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'Tahoma'
      Font.Style = []
      ParentFont = False
      DesignSize = (
        467
        368)
      object SpeedButton1: TSpeedButton
        Left = 437
        Top = 7
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
        OnClick = SpeedButton1Click
      end
      object pnlJPEGQuality: TPanel
        Left = 8
        Top = 36
        Width = 105
        Height = 21
        Cursor = crArrow
        BevelOuter = bvLowered
        Caption = 'JPEG Quality'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 9
      end
      object chkConfirmDel: TCheckBox
        Left = 236
        Top = 143
        Width = 221
        Height = 17
        HelpContext = 1005
        Caption = 'Confirm delete'
        TabOrder = 0
      end
      object chkConfirmExit: TCheckBox
        Left = 236
        Top = 169
        Width = 221
        Height = 17
        HelpContext = 1005
        Caption = 'Confirm exit'
        TabOrder = 1
      end
      object chkConfirmStopRender: TCheckBox
        Left = 236
        Top = 194
        Width = 221
        Height = 17
        Caption = 'Confirm stop render'
        TabOrder = 2
      end
      object cbUseTemplate: TCheckBox
        Left = 236
        Top = 40
        Width = 221
        Height = 17
        Caption = 'Always create blank flame'
        TabOrder = 3
      end
      object cbMissingPlugin: TCheckBox
        Left = 236
        Top = 65
        Width = 221
        Height = 17
        Caption = 'Warn when plugins are missing'
        TabOrder = 4
        WordWrap = True
      end
      object cbEmbedThumbs: TCheckBox
        Left = 236
        Top = 115
        Width = 221
        Height = 17
        Caption = 'Enable thumbnail embedding'
        TabOrder = 5
        WordWrap = True
      end
      object chkShowRenderStats: TCheckBox
        Left = 236
        Top = 219
        Width = 221
        Height = 17
        Caption = 'Show extended render statistics'
        TabOrder = 6
      end
      object pnlMultithreading: TPanel
        Left = 8
        Top = 92
        Width = 105
        Height = 21
        Cursor = crArrow
        BevelOuter = bvLowered
        Caption = 'Multithreading'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 7
      end
      object cbNrTheads: TComboBox
        Left = 112
        Top = 92
        Width = 113
        Height = 21
        Style = csDropDownList
        ItemIndex = 0
        TabOrder = 8
        Text = 'Off'
        Items.Strings = (
          'Off'
          '2'
          '3'
          '4'
          '5'
          '6'
          '7'
          '8'
          '9'
          '10'
          '11'
          '12')
      end
      object pnlPNGTransparency: TPanel
        Left = 8
        Top = 64
        Width = 105
        Height = 21
        Cursor = crArrow
        BevelOuter = bvLowered
        Caption = 'PNG Transparency'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 11
      end
      object grpGuidelines: TGroupBox
        Left = 8
        Top = 124
        Width = 217
        Height = 141
        Anchors = [akLeft, akTop, akBottom]
        Caption = 'Guidelines'
        TabOrder = 13
        object cbGL: TCheckBox
          Left = 8
          Top = 23
          Width = 193
          Height = 17
          Caption = 'Enable'
          TabOrder = 0
          OnClick = cbGLClick
        end
        object pnlCenterLine: TPanel
          Left = 112
          Top = 48
          Width = 97
          Height = 21
          Cursor = crHandPoint
          BevelInner = bvRaised
          BevelOuter = bvLowered
          BorderStyle = bsSingle
          Color = clBlack
          TabOrder = 1
          OnClick = pnlCenterLineClick
          object shCenterLine: TShape
            Left = 2
            Top = 2
            Width = 89
            Height = 13
            Align = alClient
            OnMouseUp = shCenterLineMouseUp
          end
        end
        object pnlThirdsLine: TPanel
          Left = 112
          Top = 72
          Width = 97
          Height = 21
          Cursor = crHandPoint
          BevelInner = bvRaised
          BevelOuter = bvLowered
          BorderStyle = bsSingle
          Color = clBlack
          TabOrder = 2
          OnClick = pnlThirdsLineClick
          object shThirdsLine: TShape
            Left = 2
            Top = 2
            Width = 89
            Height = 13
            Align = alClient
            OnMouseUp = shThirdsLineMouseUp
          end
        end
        object pnlGRLine: TPanel
          Left = 112
          Top = 96
          Width = 97
          Height = 21
          Cursor = crHandPoint
          BevelInner = bvRaised
          BevelOuter = bvLowered
          BorderStyle = bsSingle
          Color = clBlack
          TabOrder = 3
          OnClick = pnlGRLineClick
          object shGRLine: TShape
            Left = 2
            Top = 2
            Width = 89
            Height = 13
            Align = alClient
            OnMouseUp = shGRLineMouseUp
          end
        end
        object pnlCenter: TPanel
          Left = 8
          Top = 48
          Width = 105
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Center'
          ParentShowHint = False
          ShowHint = True
          TabOrder = 4
        end
        object pnlThirds: TPanel
          Left = 8
          Top = 72
          Width = 105
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Thirds'
          ParentShowHint = False
          ShowHint = True
          TabOrder = 5
        end
        object pnlGoldenRatio: TPanel
          Left = 8
          Top = 96
          Width = 105
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Golden ratio'
          ParentShowHint = False
          ShowHint = True
          TabOrder = 6
        end
      end
      object rgRotationMode: TRadioGroup
        Left = 5
        Top = 272
        Width = 220
        Height = 81
        Anchors = [akLeft, akBottom]
        Caption = 'Rotation Mode'
        Items.Strings = (
          'Rotate image'
          'Rotate frame')
        TabOrder = 14
      end
      object rgZoomingMode: TRadioGroup
        Left = 232
        Top = 272
        Width = 226
        Height = 81
        Anchors = [akLeft, akRight, akBottom]
        Caption = 'Zooming mode'
        Items.Strings = (
          'Preserve quality'
          'Preserve speed')
        TabOrder = 15
      end
      object Panel46: TPanel
        Left = 8
        Top = 8
        Width = 105
        Height = 21
        Cursor = crArrow
        BevelOuter = bvLowered
        Caption = 'Language file'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 16
      end
      object txtLanguageFile: TComboBox
        Left = 112
        Top = 8
        Width = 323
        Height = 21
        Anchors = [akLeft, akTop, akRight]
        TabOrder = 17
      end
      object cbPNGTransparency: TComboBox
        Left = 112
        Top = 64
        Width = 113
        Height = 21
        Style = csDropDownList
        ItemIndex = 0
        TabOrder = 12
        Text = 'Disabled'
        Items.Strings = (
          'Disabled'
          'Enabled')
      end
      object txtJPEGquality: TComboBox
        Left = 112
        Top = 36
        Width = 113
        Height = 21
        ItemIndex = 2
        TabOrder = 10
        Text = '100'
        Items.Strings = (
          '60'
          '80'
          '100'
          '120')
      end
      object cbSinglePrecision: TCheckBox
        Left = 236
        Top = 243
        Width = 193
        Height = 17
        Caption = 'Use single-precision buffers'
        TabOrder = 18
        Visible = False
      end
      object cbMultithreadedPreview: TCheckBox
        Left = 236
        Top = 89
        Width = 221
        Height = 17
        Caption = 'Multithreaded preview'
        Font.Charset = ANSI_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = []
        ParentFont = False
        TabOrder = 19
        WordWrap = True
        OnClick = cbMultithreadedPreviewClick
      end
    end
    object EditorPage: TTabSheet
      Caption = 'Editor'
      ImageIndex = 8
      DesignSize = (
        467
        368)
      object GroupBox1: TGroupBox
        Left = 8
        Top = 4
        Width = 217
        Height = 61
        Caption = 'Editor Graph'
        TabOrder = 0
        object chkUseXFormColor: TCheckBox
          Left = 8
          Top = 16
          Width = 201
          Height = 17
          Caption = 'Use transform color'
          TabOrder = 0
        end
        object chkHelpers: TCheckBox
          Left = 8
          Top = 36
          Width = 201
          Height = 17
          Caption = 'Helper lines'
          Checked = True
          State = cbChecked
          TabOrder = 1
        end
      end
      object rgReferenceMode: TRadioGroup
        Left = 240
        Top = 96
        Width = 222
        Height = 105
        Anchors = [akTop, akRight]
        Caption = 'Reference Triangle'
        ItemIndex = 0
        Items.Strings = (
          'Normal'
          'Proportional'
          'Wandering (old-style)')
        TabOrder = 1
        Visible = False
      end
      object GroupBox21: TGroupBox
        Left = 240
        Top = 4
        Width = 222
        Height = 85
        Anchors = [akTop, akRight]
        Caption = 'Editor defaults'
        TabOrder = 2
        object chkAxisLock: TCheckBox
          Left = 8
          Top = 38
          Width = 209
          Height = 17
          Caption = 'Lock transform axis'
          Checked = True
          State = cbChecked
          TabOrder = 0
        end
        object chkExtendedEdit: TCheckBox
          Left = 8
          Top = 18
          Width = 209
          Height = 17
          Caption = 'Extended edit mode'
          Checked = True
          State = cbChecked
          TabOrder = 1
        end
        object chkXaosRebuild: TCheckBox
          Left = 8
          Top = 58
          Width = 209
          Height = 17
          Caption = 'Rebuild xaos links'
          Checked = True
          State = cbChecked
          TabOrder = 2
        end
      end
      object grpEditorColors: TGroupBox
        Left = 8
        Top = 72
        Width = 217
        Height = 129
        Caption = 'Editor colors'
        TabOrder = 3
        object pnlBackground: TPanel
          Left = 8
          Top = 24
          Width = 105
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Background'
          ParentShowHint = False
          ShowHint = True
          TabOrder = 5
        end
        object pnlReferenceC: TPanel
          Left = 8
          Top = 48
          Width = 105
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Reference'
          ParentShowHint = False
          ShowHint = True
          TabOrder = 6
        end
        object pnlHelpers: TPanel
          Left = 8
          Top = 72
          Width = 105
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Helpers'
          ParentShowHint = False
          ShowHint = True
          TabOrder = 7
        end
        object pnlGrid: TPanel
          Left = 8
          Top = 96
          Width = 105
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Grid'
          ParentShowHint = False
          ShowHint = True
          TabOrder = 8
        end
        object pnlBackColor: TPanel
          Left = 112
          Top = 24
          Width = 97
          Height = 21
          Cursor = crHandPoint
          BevelOuter = bvLowered
          BorderStyle = bsSingle
          Color = clBlack
          TabOrder = 0
          OnClick = pnlBackColorClick
          object shBackground: TShape
            Left = 1
            Top = 1
            Width = 91
            Height = 15
            Align = alClient
            Pen.Style = psClear
            OnMouseUp = shBackgroundMouseUp
          end
        end
        object pnlReference: TPanel
          Left = 112
          Top = 48
          Width = 97
          Height = 21
          Cursor = crHandPoint
          BevelOuter = bvLowered
          BorderStyle = bsSingle
          Color = clGray
          TabOrder = 1
          OnClick = pnlReferenceClick
          object shRef: TShape
            Left = 1
            Top = 1
            Width = 91
            Height = 15
            Align = alClient
            Pen.Style = psClear
            OnMouseUp = shRefMouseUp
          end
        end
        object pnlHelpersColor: TPanel
          Left = 112
          Top = 72
          Width = 97
          Height = 21
          Cursor = crHandPoint
          BevelOuter = bvLowered
          BorderStyle = bsSingle
          Color = clGray
          TabOrder = 2
          OnClick = pnlHelpersColorClick
          object shHelpers: TShape
            Left = 1
            Top = 1
            Width = 91
            Height = 15
            Align = alClient
            Pen.Style = psClear
            OnMouseUp = shHelpersMouseUp
          end
        end
        object pnlGridColor1: TPanel
          Left = 112
          Top = 96
          Width = 49
          Height = 21
          Cursor = crHandPoint
          BevelOuter = bvLowered
          BorderStyle = bsSingle
          Color = clBlack
          TabOrder = 3
          OnClick = pnlGridColor1Click
          object shGC1: TShape
            Left = 1
            Top = 1
            Width = 43
            Height = 15
            Align = alClient
            Pen.Style = psClear
            OnMouseUp = shGC1MouseUp
          end
        end
        object pnlGridColor2: TPanel
          Left = 164
          Top = 96
          Width = 45
          Height = 21
          Cursor = crHandPoint
          BevelOuter = bvLowered
          BorderStyle = bsSingle
          Color = clBlack
          TabOrder = 4
          OnClick = pnlGridColor2Click
          object shGC2: TShape
            Left = 1
            Top = 1
            Width = 39
            Height = 15
            Align = alClient
            Pen.Style = psClear
            OnMouseUp = shGC2MouseUp
          end
        end
      end
      object chkShowAllXforms: TCheckBox
        Left = 8
        Top = 264
        Width = 454
        Height = 17
        Anchors = [akLeft, akTop, akRight]
        Caption = 'Always show both type of transforms'
        Checked = True
        State = cbChecked
        TabOrder = 4
        WordWrap = True
      end
      object chkEnableEditorPreview: TCheckBox
        Left = 8
        Top = 208
        Width = 449
        Height = 17
        Anchors = [akLeft, akTop, akRight]
        Caption = 'Enable editor preview'
        TabOrder = 5
        OnClick = chkEnableEditorPreviewClick
      end
      object Panel48: TPanel
        Left = 16
        Top = 232
        Width = 105
        Height = 21
        Cursor = crArrow
        BevelOuter = bvLowered
        Caption = 'Transparency'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 6
      end
      object tbEPTransparency: TTrackBar
        Left = 128
        Top = 230
        Width = 329
        Height = 25
        Anchors = [akLeft, akTop, akRight]
        LineSize = 4
        Max = 255
        PageSize = 32
        Frequency = 16
        TabOrder = 7
      end
    end
    object DisplayPage: TTabSheet
      Caption = 'Display'
      DesignSize = (
        467
        368)
      object GroupBox2: TGroupBox
        Left = 253
        Top = 84
        Width = 209
        Height = 109
        Anchors = [akTop, akRight]
        Caption = 'Preview density'
        TabOrder = 1
        object Panel8: TPanel
          Left = 8
          Top = 24
          Width = 105
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Low quality'
          ParentShowHint = False
          ShowHint = True
          TabOrder = 3
        end
        object Panel9: TPanel
          Left = 8
          Top = 48
          Width = 105
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Medium quality'
          ParentShowHint = False
          ShowHint = True
          TabOrder = 4
        end
        object Panel10: TPanel
          Left = 8
          Top = 72
          Width = 105
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'High quality'
          ParentShowHint = False
          ShowHint = True
          TabOrder = 5
        end
        object txtHighQuality: TEdit
          Left = 112
          Top = 72
          Width = 81
          Height = 21
          HelpContext = 1014
          TabOrder = 2
        end
        object txtMediumQuality: TEdit
          Left = 112
          Top = 48
          Width = 81
          Height = 21
          HelpContext = 1013
          TabOrder = 1
        end
        object txtLowQuality: TEdit
          Left = 112
          Top = 24
          Width = 81
          Height = 21
          HelpContext = 1012
          TabOrder = 0
        end
      end
      object grpRendering: TGroupBox
        Left = 8
        Top = 84
        Width = 217
        Height = 205
        Caption = 'Rendering'
        TabOrder = 0
        object Panel1: TPanel
          Left = 8
          Top = 24
          Width = 105
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Density'
          ParentShowHint = False
          ShowHint = True
          TabOrder = 7
        end
        object Panel2: TPanel
          Left = 8
          Top = 48
          Width = 105
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Gamma'
          ParentShowHint = False
          ShowHint = True
          TabOrder = 8
        end
        object Panel3: TPanel
          Left = 8
          Top = 72
          Width = 105
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Brightness'
          ParentShowHint = False
          ShowHint = True
          TabOrder = 9
        end
        object Panel4: TPanel
          Left = 8
          Top = 96
          Width = 105
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Vibrancy'
          ParentShowHint = False
          ShowHint = True
          TabOrder = 10
        end
        object Panel5: TPanel
          Left = 8
          Top = 120
          Width = 105
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Gamma threshold'
          ParentShowHint = False
          ShowHint = True
          TabOrder = 11
        end
        object Panel6: TPanel
          Left = 8
          Top = 144
          Width = 105
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Oversample'
          ParentShowHint = False
          ShowHint = True
          TabOrder = 12
        end
        object Panel7: TPanel
          Left = 8
          Top = 168
          Width = 105
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Filter radius'
          ParentShowHint = False
          ShowHint = True
          TabOrder = 13
        end
        object txtGammaThreshold: TEdit
          Left = 112
          Top = 120
          Width = 89
          Height = 21
          HelpContext = 1011
          TabOrder = 6
        end
        object txtFilterRadius: TEdit
          Left = 112
          Top = 168
          Width = 89
          Height = 21
          HelpContext = 1011
          TabOrder = 5
        end
        object txtOversample: TEdit
          Left = 112
          Top = 144
          Width = 89
          Height = 21
          HelpContext = 1010
          TabOrder = 4
        end
        object txtVibrancy: TEdit
          Left = 112
          Top = 96
          Width = 89
          Height = 21
          HelpContext = 1009
          TabOrder = 3
        end
        object txtBrightness: TEdit
          Left = 112
          Top = 72
          Width = 89
          Height = 21
          HelpContext = 1008
          TabOrder = 2
        end
        object txtGamma: TEdit
          Left = 112
          Top = 48
          Width = 89
          Height = 21
          HelpContext = 1007
          TabOrder = 1
        end
        object txtSampleDensity: TEdit
          Left = 112
          Top = 24
          Width = 89
          Height = 21
          HelpContext = 1006
          TabOrder = 0
        end
      end
      object GroupBox20: TGroupBox
        Left = 8
        Top = 8
        Width = 454
        Height = 73
        Anchors = [akLeft, akTop, akRight]
        Caption = 'Main Window Preview'
        TabOrder = 2
        object Label48: TLabel
          Left = 424
          Top = 20
          Width = 11
          Height = 13
          Caption = '%'
        end
        object chkShowTransparency: TCheckBox
          Left = 8
          Top = 42
          Width = 233
          Height = 17
          Caption = 'Show Transparency'
          TabOrder = 2
        end
        object chkExtendMainPreview: TCheckBox
          Left = 8
          Top = 20
          Width = 225
          Height = 17
          Caption = 'Extend preview buffer'
          TabOrder = 0
        end
        object pnlExtension: TPanel
          Left = 244
          Top = 16
          Width = 105
          Height = 21
          Cursor = crArrow
          BevelOuter = bvLowered
          Caption = 'Buffer extension'
          ParentShowHint = False
          ShowHint = True
          TabOrder = 3
        end
        object cbExtendPercent: TComboBox
          Left = 348
          Top = 16
          Width = 73
          Height = 21
          TabOrder = 1
          Items.Strings = (
            '0'
            '10'
            '25'
            '50'
            '100'
            '150'
            '200')
        end
      end
      object chkUseSmallThumbs: TCheckBox
        Left = 253
        Top = 202
        Width = 209
        Height = 31
        Anchors = [akTop, akRight]
        Caption = 'Use small thumbnails'
        TabOrder = 3
        WordWrap = True
        OnClick = chkUseSmallThumbsClick
      end
    end
    object PathsPage: TTabSheet
      Caption = 'Environment'
      ImageIndex = 7
      DesignSize = (
        467
        368)
      object btnDefGradient: TSpeedButton
        Left = 437
        Top = 7
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
        OnClick = btnDefGradientClick
      end
      object btnSmooth: TSpeedButton
        Left = 437
        Top = 31
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
        OnClick = btnSmoothClick
      end
      object SpeedButton2: TSpeedButton
        Left = 437
        Top = 55
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
        OnClick = SpeedButton2Click
      end
      object btnHelp: TSpeedButton
        Left = 437
        Top = 80
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
        OnClick = btnHelpClick
      end
      object Label49: TLabel
        Left = 245
        Top = 214
        Width = 37
        Height = 13
        Caption = 'minutes'
      end
      object btnFindDefaultSaveFile: TSpeedButton
        Left = 437
        Top = 185
        Width = 24
        Height = 24
        Hint = 'Browse...'
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
        OnClick = btnFindDefaultSaveFileClick
      end
      object btnPluginPath: TSpeedButton
        Left = 437
        Top = 106
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
        OnClick = btnPluginPathClick
      end
      object chkRememberLastOpen: TCheckBox
        Left = 8
        Top = 138
        Width = 433
        Height = 17
        Caption = 'Remember last opened parameters'
        TabOrder = 0
        OnClick = chkRememberLastOpenClick
      end
      object Panel39: TPanel
        Left = 8
        Top = 8
        Width = 129
        Height = 21
        Cursor = crArrow
        BevelOuter = bvLowered
        Caption = 'Default parameters'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 1
      end
      object txtDefParameterFile: TEdit
        Left = 136
        Top = 8
        Width = 302
        Height = 21
        HelpContext = 1000
        Anchors = [akLeft, akTop, akRight]
        ParentShowHint = False
        ShowHint = False
        TabOrder = 2
      end
      object Panel40: TPanel
        Left = 8
        Top = 32
        Width = 129
        Height = 21
        Cursor = crArrow
        BevelOuter = bvLowered
        Caption = 'Smooth palette file'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 4
      end
      object txtDefSmoothFile: TEdit
        Left = 136
        Top = 32
        Width = 302
        Height = 21
        HelpContext = 1001
        Anchors = [akLeft, akTop, akRight]
        TabOrder = 3
      end
      object Panel41: TPanel
        Left = 8
        Top = 56
        Width = 129
        Height = 21
        Cursor = crArrow
        BevelOuter = bvLowered
        Caption = 'Function library'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 7
      end
      object Panel43: TPanel
        Left = 8
        Top = 80
        Width = 129
        Height = 21
        Cursor = crArrow
        BevelOuter = bvLowered
        Caption = 'Help file'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 8
      end
      object txtLibrary: TEdit
        Left = 136
        Top = 56
        Width = 302
        Height = 21
        HelpContext = 1000
        Anchors = [akLeft, akTop, akRight]
        ParentShowHint = False
        ShowHint = False
        TabOrder = 5
      end
      object txtHelp: TEdit
        Left = 136
        Top = 80
        Width = 302
        Height = 21
        HelpContext = 1000
        Anchors = [akLeft, akTop, akRight]
        ParentShowHint = False
        ShowHint = False
        TabOrder = 6
      end
      object cbEnableAutosave: TCheckBox
        Left = 8
        Top = 162
        Width = 425
        Height = 17
        Caption = 'Enable autosave'
        TabOrder = 11
        OnClick = cbEnableAutosaveClick
      end
      object Panel44: TPanel
        Left = 24
        Top = 186
        Width = 113
        Height = 21
        Cursor = crArrow
        BevelOuter = bvLowered
        Caption = 'File name'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 12
      end
      object txtDefaultSaveFile: TEdit
        Left = 136
        Top = 186
        Width = 302
        Height = 21
        HelpContext = 1000
        ParentShowHint = False
        ShowHint = False
        TabOrder = 10
      end
      object Panel45: TPanel
        Left = 24
        Top = 210
        Width = 113
        Height = 21
        Cursor = crArrow
        BevelOuter = bvLowered
        Caption = 'Save frequency'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 13
      end
      object cbFreq: TComboBox
        Left = 136
        Top = 210
        Width = 105
        Height = 21
        Style = csDropDownList
        ItemIndex = 2
        TabOrder = 9
        Text = '5'
        Items.Strings = (
          '1'
          '2'
          '5'
          '10')
      end
      object Panel50: TPanel
        Left = 8
        Top = 106
        Width = 129
        Height = 21
        Cursor = crArrow
        BevelOuter = bvLowered
        Caption = 'Plugin folder'
        ParentShowHint = False
        ShowHint = True
        TabOrder = 14
      end
      object txtPluginFolder: TEdit
        Left = 136
        Top = 106
        Width = 302
        Height = 21
        HelpContext = 1000
        Anchors = [akLeft, akTop, akRight]
        ParentShowHint = False
        ShowHint = False
        TabOrder = 15
      end
    end
  end
  object btnOK: TButton
    Left = 304
    Top = 450
    Width = 86
    Height = 25
    Anchors = [akRight, akBottom]
    Caption = 'OK'
    Default = True
    TabOrder = 0
    OnClick = btnOKClick
  end
  object btnCancel: TButton
    Left = 397
    Top = 450
    Width = 86
    Height = 25
    Anchors = [akRight, akBottom]
    Caption = 'Cancel'
    TabOrder = 1
    OnClick = btnCancelClick
  end
  object OpenDialog: TOpenDialog
    Left = 8
    Top = 408
  end
end
