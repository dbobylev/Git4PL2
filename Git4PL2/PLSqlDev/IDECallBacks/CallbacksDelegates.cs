using System;
using System.Runtime.InteropServices;

namespace Git4PL2.PLSqlDev.IDECallBacks
{
    // # 1 
    [return: MarshalAs(UnmanagedType.U4)]
    delegate int SYS_Version();

    // # 11
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool IDE_Connected();

    // # 14 
    [return: MarshalAs(UnmanagedType.U4)]
    delegate int IDE_GetWindowType();

    // # 25
    delegate void IDE_SetReadOnly(bool ReadyOnly);

    // # 26 
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool IDE_GetReadOnly();

    // # 30
    [return: MarshalAs(UnmanagedType.LPStr)]
    delegate string IDE_GetText();

    // # 31
    [return: MarshalAs(UnmanagedType.LPStr)]
    delegate string IDE_GetSelectedText();

    // # 34
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool IDE_SetText(string Text);

    // # 35
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool IDE_SetStatusMessage(string Text);

    // # 40
    [return: MarshalAs(UnmanagedType.U4)]
    delegate int SQL_Execute(string SQL);

    // # 41 
    [return: MarshalAs(UnmanagedType.U4)]
    delegate int SQL_FieldCount();

    // # 42
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool SQL_Eof();

    // # 43 
    [return: MarshalAs(UnmanagedType.U4)]
    delegate int SQL_Next();

    // # 44
    [return: MarshalAs(UnmanagedType.LPStr)]
    delegate string SQL_Field(int Field);

    // # 45
    [return: MarshalAs(UnmanagedType.LPStr)]
    delegate string SQL_FieldName(int Field);

    // # 46 
    [return: MarshalAs(UnmanagedType.U4)]
    delegate int SQL_FieldIndex(string Name);

    // # 48
    [return: MarshalAs(UnmanagedType.LPStr)]
    delegate string SQL_ErrorMessage();

    // # 52
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool SQL_CheckConnection();

    // # 110
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool IDE_GetWindowObject([Out, MarshalAs(UnmanagedType.LPStr)] out string ObjectType,
                                      [Out, MarshalAs(UnmanagedType.LPStr)] out string ObjectOwner,
                                      [Out, MarshalAs(UnmanagedType.LPStr)] out string ObjectName,
                                      [Out, MarshalAs(UnmanagedType.LPStr)] out string SubObject);
    // # 141 
    [return: MarshalAs(UnmanagedType.U4)]
    delegate int IDE_GetCursorX();

    // # 142 
    [return: MarshalAs(UnmanagedType.U4)]
    delegate int IDE_GetCursorY();

    // # 143
    delegate void IDE_SetCursor(int X, int Y);

    // # 150
    delegate void IDE_CreateToolButton(int ID, int Index, string Name, string BitmapFile, IntPtr BitmapHandle);

    // # 240
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool IDE_GetConnectionInfoEx(int ix, [Out, MarshalAs(UnmanagedType.LPStr)] out string Username,
                                                  [Out, MarshalAs(UnmanagedType.LPStr)] out string Password,
                                                  [Out, MarshalAs(UnmanagedType.LPStr)] out string Database,
                                                  [Out, MarshalAs(UnmanagedType.LPStr)] out string ConnectAs);

    // # 245
    [return: MarshalAs(UnmanagedType.U4)]
    delegate int IDE_GetWindowConnection();
}
