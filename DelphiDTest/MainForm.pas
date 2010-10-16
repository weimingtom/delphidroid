unit MainForm;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TForm1 = class(TForm)
    btnSayHello: TButton;
    txtName: TEdit;
    HelloLabel: TLabel;
    procedure HelloLabelClick(Sender: TObject);
    procedure btnSayHelloClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

uses
  Android.Dialog;

procedure TForm1.btnSayHelloClick(Sender: TObject);
begin
  HelloLabel.Caption := 'Hello, ' + txtName.Text;
end;

procedure TForm1.HelloLabelClick(Sender: TObject);
begin
  Android_AlertBox(HelloLabel.Caption);
end;

end.
