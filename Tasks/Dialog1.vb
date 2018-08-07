Public Class Dialog1
    Dim speed As Byte
    Dim steps As Byte
    Dim originXY As Point
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles OK_Button.Click
        DialogResult = DialogResult.OK
        Hide()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles Cancel_Button.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    Private Sub Dialog1_Load(sender As Object, e As EventArgs) Handles Me.Load
        '初始化窗口
        Show()
        Label1.Top = 208
        Button_Next.BackColor = My.Settings.ThemeColor
        OK_Button.Visible = False
        Button_Next.Visible = True
        GroupBox1.Visible = False
        GroupBox2.Visible = False
        GroupBox3.Visible = False
        GroupBox4.Visible = False
        TextBox1.Text = ""
        TextBox2.Text = ""
        ComboBox1.SelectedItem = "从不"
        Button_Next.Enabled = False
        steps = 1
        DateTimePicker1.Value = Now
        DateTimePicker1.CustomFormat = "yyyy/MM/dd HH:mm:ss"
        Dim path As Drawing2D.GraphicsPath = RoundedRectPath(ClientRectangle, 30)
        Region = New Region(path)
        BackColor = My.Settings.ThemeColor
        ForeColor = My.Settings.TForeColor
    End Sub

    Private Function RoundedRectPath(ByVal Rectangle As Rectangle, ByVal r As Integer) As Drawing2D.GraphicsPath
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

    Private Sub Dialog1_MouseEnter(sender As Object, e As EventArgs) Handles Me.MouseEnter
        If Label1.Top > 140 Then
            Timer1.Enabled = True
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Label1.Top > 140 Then
            Select Case Label1.Top - 140
                Case < 10
                    speed = 1
                Case > 30
                    speed = 3
                Case > 50
                    speed = 5
            End Select
            Label1.Top = Label1.Top - speed
        Else
            GroupBox1.Visible = True
            Button_Next.Enabled = True
            Timer1.Enabled = False
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        Button_Next.Enabled = True
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        Button_Next.Enabled = True
    End Sub

    Private Sub Button_Next_Click(sender As Object, e As EventArgs) Handles Button_Next.Click
        If steps = 1 Then
            GroupBox2.Visible = True
            'GroupBox1.Visible = False
            GroupBox2.BringToFront()
            steps = steps + 1
            Exit Sub
        End If
        If steps = 2 Then
            GroupBox3.Visible = True
            GroupBox4.Visible = True
            'GroupBox2.Visible = False
            GroupBox3.BringToFront()
            steps = steps + 1
            TextBox1.Focus()
            Button_Next.Visible = False
            OK_Button.Visible = True
            OK_Button.Enabled = False
            Exit Sub
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text = "" Then
            OK_Button.Enabled = False
        Else
            OK_Button.Enabled = True
        End If
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = "|" Then
            e.Handled = True
            MsgBox("文本不能包含以下字符" & vbCrLf & "|")
        End If
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If e.KeyChar = "|" Then
            e.Handled = True
            MsgBox("文本不能包含以下字符" & vbCrLf & "|")
        End If
    End Sub

    Private Sub Dialog1_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        If e.Button = MouseButtons.Left Then
            Location = PointToScreen(e.Location) - originXY
        End If
    End Sub

    Private Sub Dialog1_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        originXY = e.Location
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        Dim dday, dhour, dmin, dsec As Integer
        dday = DateDiff(DateInterval.DayOfYear, Now, DateTimePicker1.Value)
        dhour = DateDiff(DateInterval.Hour, Now, DateTimePicker1.Value) - dday * 24
        dmin = DateDiff(DateInterval.Minute, Now, DateTimePicker1.Value) - dday * 24 * 60 - dhour * 60
        dsec = DateDiff(DateInterval.Second, Now, DateTimePicker1.Value) - dday * 24 * 86400 - dhour * 3600 - dmin * 60
        Label2.Text = "距今"
        If dday <> 0 Then Label2.Text = Label2.Text & dday & "天"
        If dhour <> 0 Then Label2.Text = Label2.Text & dhour & "小时"
        If dmin <> 0 Then Label2.Text = Label2.Text & dmin & "分"
        Label2.Text = Label2.Text & dsec & "秒"
    End Sub
End Class
