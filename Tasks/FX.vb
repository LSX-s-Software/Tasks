Imports System.Threading

Public Class FX '窗口的特殊效果类，如阴影、圆角等
    Public Shared listitem As ListViewItem
    Public Shared index, targetindex As Byte
    Public Shared td As Thread
    Public Const CS_DROPSHADOW = &H20000
    Public Const GCL_STYLE = (-26)
    Public Declare Function GetClassLong Lib "user32" Alias "GetClassLongA" (ByVal hwnd As Integer, ByVal nIndex As Integer) As Integer
    Public Declare Function SetClassLong Lib "user32" Alias "SetClassLongA" (ByVal hwnd As Integer, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer

    Public Shared Function RoundedRectPath(ByVal Rectangle As Rectangle, ByVal r As Integer) As Drawing2D.GraphicsPath
        Rectangle.Offset(-1, -1)
        Dim RoundRect As New Rectangle(Rectangle.Location, New Size(r - 1, r - 1))
        Dim path As New Drawing2D.GraphicsPath
        path.AddArc(RoundRect, 180, 90)
        RoundRect.X = Rectangle.Right - r
        path.AddArc(RoundRect, 270, 90)
        RoundRect.Y = Rectangle.Bottom - r
        path.AddArc(RoundRect, 0, 90)
        RoundRect.X = Rectangle.Left
        path.AddArc(RoundRect, 90, 90)
        path.CloseFigure()
        Return path
    End Function

    Public Shared Sub AnimateOut(obj As Object)
        Dim speed As Byte
        Do Until obj.Left() > Screen.PrimaryScreen.WorkingArea.Width
            Select Case Screen.PrimaryScreen.WorkingArea.Width - obj.Left
                Case < 50
                    speed = 1
                Case > 100
                    speed = 5
                Case > 150
                    speed = 10
                Case > 170
                    speed = 1
            End Select
            obj.Left = obj.Left + speed
            Thread.Sleep(2)
        Loop
    End Sub

    Public Shared Sub ProgressAnimate(Start As SByte, Target As Byte, obj As ListViewItem)
        listitem = obj
        index = If(Start < 0, 0, Start)
        targetindex = Target
        td = New Thread(AddressOf fun1)
        td.Start()
    End Sub

    Public Shared Sub fun1()
        Debug.Print(Form1.IsHandleCreated)
        If Form1.IsHandleCreated Then
            Do Until index >= targetindex
                Form1.Invoke(New Dg(AddressOf Change), index)
                index = index + 1
                Thread.Sleep(5)
            Loop
        End If
        td.Abort()
    End Sub

    Delegate Sub Dg(ByVal index As Byte)
    Public Shared Sub Change(ByVal index As Byte)
        listitem.ImageIndex = index
    End Sub
End Class
