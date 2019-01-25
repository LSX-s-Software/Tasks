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

    Public Shared Sub SizeAnimate(obj As Object, NewSize As Size, RightFix As Boolean)
        Dim speed As Byte
        Dim OldSize As Size = obj.Size
        Do Until obj.Width = NewSize.Width
            Debug.Print(speed)
            Select Case 1 - Math.Abs(obj.Width - OldSize.Width) / Math.Abs(OldSize.Width - NewSize.Width)
                Case < 0.05
                    speed = 1
                Case > 0.1
                    speed = 5
                Case > 0.4
                    speed = 10
                Case > 0.6
                    speed = 5
                Case > 0.85
                    speed = 1
            End Select
            If NewSize.Width > obj.Width Then
                obj.Width = obj.Width + speed
                If RightFix Then obj.Left = obj.Left - speed
            Else
                obj.Width = obj.Width - speed
                If RightFix Then obj.Left = obj.Left + speed
            End If
            If obj.Height <> NewSize.Height Then
                Select Case Math.Abs(obj.Height - NewSize.Height) / Math.Abs(OldSize.Height - NewSize.Height)
                    Case < 0.25
                        speed = 1
                    Case > 0.4
                        speed = 5
                    Case > 0.6
                        speed = 10
                    Case > 0.9
                        speed = 1
                End Select
                If NewSize.Height > obj.Height Then
                    obj.Height = obj.Height + speed
                Else
                    obj.Height = obj.Height - speed
                End If
            End If
            Debug.Print(obj.Size.ToString)
            obj.Refresh()
            Thread.Sleep(2)
        Loop
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

    Public Shared Function GetHoverColor()
        Return Color.FromArgb(Math.Abs(My.Settings.ThemeColor.R - 10), Math.Abs(My.Settings.ThemeColor.G - 10), Math.Abs(My.Settings.ThemeColor.B - 10))
    End Function

    Delegate Sub Dg(ByVal index As Byte)
    Public Shared Sub Change(ByVal index As Byte)
        listitem.ImageIndex = index
    End Sub
End Class
