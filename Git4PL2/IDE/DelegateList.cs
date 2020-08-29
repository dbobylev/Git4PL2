using System;
using System.Runtime.InteropServices;

namespace Git4PL2.IDE
{
    // # 1 
    delegate int SYS_Version();

    // # 11
    delegate bool IDE_Connected();

    // # 14 
    delegate int IDE_GetWindowType();

    // # 25
    delegate void IDE_SetReadOnly(bool ReadyOnly);

    // # 26 
    delegate bool IDE_GetReadOnly();

    // # 30
    delegate string IDE_GetText();

    // # 31
    delegate string IDE_GetSelectedText();

    // # 34
    delegate bool IDE_SetText(string Text);

    // # 35
    delegate bool IDE_SetStatusMessage(string Text);

    // # 40
    delegate int SQL_Execute(string SQL);

    // # 41 
    delegate int SQL_FieldCount();

    // # 42
    delegate bool SQL_Eof();

    // # 43 
    delegate int SQL_Next();

    // # 44
    delegate string SQL_Field(int Field);

    // # 45
    delegate string SQL_FieldName(int Field);

    // # 46 
    delegate int SQL_FieldIndex(string Name);

    // # 48
    delegate string SQL_ErrorMessage();

    // # 52
    delegate bool SQL_CheckConnection();

    // #64
    delegate void IDE_RefreshMenus(int ID);

    // # 110
    delegate bool IDE_GetWindowObject(out string ObjectType, out string ObjectOwner, out string ObjectName, out string SubObject);

    // # 141 
    delegate int IDE_GetCursorX();

    // # 142 
    delegate int IDE_GetCursorY();

    // # 143
    delegate void IDE_SetCursor(int X, int Y);

    // # 150
    delegate void IDE_CreateToolButton(int ID, int Index, string Name, string BitmapFile, IntPtr BitmapHandle);

    // # 240
    delegate bool IDE_GetConnectionInfoEx(int ix, out string Username,
                                                  out string Password,
                                                  out string Database,
                                                  out string ConnectAs);

    // # 245
    delegate int IDE_GetWindowConnection();
}
