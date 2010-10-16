unit Android.Dialog;

interface

uses
  Dialogs;

procedure Android_AlertBox(aText: string);

implementation

procedure Android_AlertBox(aText: string);
begin
  MessageDlg(aText,mtInformation, mbOKCancel, 0);
end;

end.
 