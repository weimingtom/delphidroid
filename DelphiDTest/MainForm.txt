object Form1: TForm1
  Left = 192
  Top = 124
  Caption = 'Form1'
  ClientHeight = 295
  ClientWidth = 294
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 13
  object HelloLabel: TLabel
    Left = 102
    Top = 128
    Width = 77
    Height = 13
    Caption = 'I Say Hello Here'
    OnClick = HelloLabelClick
  end
  object btnSayHello: TButton
    Left = 104
    Top = 176
    Width = 75
    Height = 25
    Caption = 'Say Hello'
    TabOrder = 0
    OnClick = btnSayHelloClick
  end
  object txtName: TEdit
    Left = 80
    Top = 56
    Width = 121
    Height = 21
    TabOrder = 1
  end
end
