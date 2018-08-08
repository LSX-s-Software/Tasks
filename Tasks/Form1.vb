Imports System.ComponentModel
Imports Microsoft.VisualBasic.FileIO.FileSystem

Public Class Form1
    Public SelectedView As ListView
    Public reminded As ListViewItem()
    Public RemindInt, t As Long
    Private Sub StatusStrip1_MouseEnter(sender As Object, e As EventArgs) Handles StatusStrip1.MouseEnter
        Cursor = Cursors.Hand
    End Sub

    Private Sub StatusStrip1_MouseLeave(sender As Object, e As EventArgs) Handles StatusStrip1.MouseLeave
        Cursor = Cursors.Default
    End Sub

    Public Sub Add()
        Dim result
        Dim itm As ListViewItem
        If ListView1.Visible = True Then
            Dialog1.RadioButton1.Checked = True
            Dialog1.RadioButton2.Checked = False
        Else
            Dialog1.RadioButton2.Checked = True
            Dialog1.RadioButton1.Checked = False
        End If
        result = Dialog1.ShowDialog()
        If result = DialogResult.OK Then
            If Dialog1.RadioButton1.Checked = True Then
                itm = ListView1.Items.Add(Dialog1.TextBox1.Text)
                itm.SubItems.AddRange({Dialog1.DateTimePicker1.Value, Dialog1.TextBox2.Text, Now})
            Else
                itm = ListView2.Items.Add(Dialog1.DateTimePicker1.Value)
                Dim nextt As Date
                Select Case Dialog1.ComboBox1.Text
                    Case "从不"
                        nextt = Dialog1.DateTimePicker1.Value
                    Case "每天"
                        nextt = DateAdd(DateInterval.DayOfYear, 1, Dialog1.DateTimePicker1.Value)
                    Case "每两天"
                        nextt = DateAdd(DateInterval.DayOfYear, 2, Dialog1.DateTimePicker1.Value)
                    Case "每周"
                        nextt = DateAdd(DateInterval.DayOfYear, 7, Dialog1.DateTimePicker1.Value)
                    Case "每两周"
                        nextt = DateAdd(DateInterval.DayOfYear, 14, Dialog1.DateTimePicker1.Value)
                    Case "每月"
                        nextt = DateAdd(DateInterval.Month, 1, Dialog1.DateTimePicker1.Value)
                    Case "每年"
                        nextt = DateAdd(DateInterval.Year, 1, Dialog1.DateTimePicker1.Value)
                End Select
                itm.SubItems.AddRange({Dialog1.TextBox1.Text, Dialog1.TextBox2.Text, Now, Dialog1.ComboBox1.Text, nextt})
            End If
        End If
    End Sub

    Private Sub Button_Add_Click(sender As Object, e As EventArgs) Handles Button_Add.Click
        Add()
    End Sub

    Private Sub Button_Del_Click(sender As Object, e As EventArgs) Handles Button_Del.Click
        On Error Resume Next
        SelectedView.SelectedItems(0).Remove()
    End Sub

    Private Sub ToolStripStatusLabel2_Click(sender As Object, e As EventArgs) Handles ToolStripStatusLabel2.Click
        SelectedView = ListView1
        ToolStripStatusLabel2.ForeColor = My.Settings.ThemeColor
        ToolStripStatusLabel3.ForeColor = Color.Black
        ListView1.Visible = True
        ListView2.Visible = False
        Label1.Text = "任务"
    End Sub

    Private Sub ToolStripStatusLabel3_Click(sender As Object, e As EventArgs) Handles ToolStripStatusLabel3.Click
        SelectedView = ListView2
        ToolStripStatusLabel3.ForeColor = My.Settings.ThemeColor
        ToolStripStatusLabel2.ForeColor = Color.Black
        ListView2.Visible = True
        ListView1.Visible = False
        Label1.Text = "提醒事项"
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick  '提醒功能
        Dim i As Integer
        Dim t1, t2 As Int64
        For i = 0 To ListView1.Items.Count - 1
            t1 = System.DateTime.Parse(ListView1.Items(i).SubItems(1).Text).Subtract(DateTime.Now).TotalMilliseconds '现在时间到截止时间的长度
            t2 = System.DateTime.Parse(DateTime.Now).Subtract(ListView1.Items(i).SubItems(3).Text).TotalMilliseconds + 1 '从任务创建到现在的时间
            If ((t1 + t2) > t2) And (t1 >= 0) Then
                ListView1.Items(i).ImageIndex = Int(t2 / (t1 + t2) * 100)
            Else
                ListView1.Items(i).ImageIndex = 99
            End If
            If (t1 <= 0) AndAlso Not Find(ListView1.Items(i)) Then
                Form2.Show() '打开提醒窗体
                Form2.Label2.Text = ListView1.Items(i).Text
                If reminded Is Nothing Then
                    reminded = New ListViewItem() {ListView1.Items(i)}
                Else
                    reminded = reminded.Concat({ListView1.Items(i)}).ToArray
                End If
                Enabled = False
                Timer1.Enabled = False
                Timer3.Enabled = False
                Exit Sub
            End If
        Next
        '------------------------------------------
        For i = 0 To ListView2.Items.Count - 1
            t1 = System.DateTime.Parse(ListView2.Items(i).Text).Subtract(DateTime.Now).TotalMilliseconds '现在时间到截止时间的长度
            t2 = System.DateTime.Parse(DateTime.Now).Subtract(ListView2.Items(i).SubItems(3).Text).TotalMilliseconds + 1 '从任务创建到现在的时间
            If ((t1 + t2) > t2) And (t1 >= 0) Then
                ListView2.Items(i).ImageIndex = Int(t2 / (t1 + t2) * 100)
            Else
                ListView2.Items(i).ImageIndex = 99
            End If
            If (t1 <= 0) AndAlso Not Find(ListView2.Items(i)) Then
                If ListView2.Items(i).SubItems(4).Text <> "从不" Then
                    Dim nextt As Date
                    Select Case ListView2.Items(i).SubItems(4).Text
                        Case "每天"
                            nextt = DateAdd(DateInterval.DayOfYear, 1, CDate(ListView2.Items(i).Text))
                        Case "每两天"
                            nextt = DateAdd(DateInterval.DayOfYear, 2, CDate(ListView2.Items(i).Text))
                        Case "每周"
                            nextt = DateAdd(DateInterval.DayOfYear, 7, CDate(ListView2.Items(i).Text))
                        Case "每两周"
                            nextt = DateAdd(DateInterval.DayOfYear, 14, CDate(ListView2.Items(i).Text))
                        Case "每月"
                            nextt = DateAdd(DateInterval.Month, 1, CDate(ListView2.Items(i).Text))
                        Case "每年"
                            nextt = DateAdd(DateInterval.Year, 1, CDate(ListView2.Items(i).Text))
                    End Select
                    Dim itm = ListView2.Items(i).Clone()
                    ListView2.Items.Add(itm)
                    itm.Text = ListView2.Items(i).SubItems(5).Text
                    itm.Subitems(5).text = nextt
                    itm = Nothing
                End If
                Form2.Show() '打开提醒窗体
                Form2.Label1.Text = "您的提醒事项" & ChrW(13) & ChrW(13) & "时间已到"
                Form2.Label2.Text = ListView2.Items(i).SubItems(1).Text
                If reminded Is Nothing Then
                    reminded = New ListViewItem() {ListView2.Items(i)}
                Else
                    reminded = reminded.Concat({ListView2.Items(i)}).ToArray
                End If
                Enabled = False
                Timer1.Enabled = False
                Timer3.Enabled = False
                    Exit Sub
                End If
        Next
    End Sub

    Private Function Find(ByVal item As ListViewItem) As Boolean
        Dim i As Integer
        Find = False
        If reminded Is Nothing Then Return False
        For i = 0 To reminded.Count - 1
            If reminded(i) Is item Then
                Return True
            End If
        Next
    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim a As New Drawing2D.GraphicsPath()
        a.AddEllipse(2, 2, 35, 35)
        Button_Add.Region = New Region(a)
        Button_Add.BringToFront()
        ToolStripStatusLabel2.ForeColor = My.Settings.ThemeColor
        SelectedView = ListView1
        If My.Settings.RemindInterval <> "永不重复" Then
            RemindInt = CInt(My.Settings.RemindInterval) * 60
        Else
            Timer3.Enabled = False
        End If
        DrawProgressBar()
    End Sub

    Private Sub DrawProgressBar()
        Dim bmp As Bitmap
        Dim gra As Graphics
        Dim ThemeColor As Color = My.Settings.ThemeColor
        Dim border As Pen = New Pen(ThemeColor, 2)
        Dim b1 As SolidBrush = New SolidBrush(ThemeColor)
        Dim b As SolidBrush = New SolidBrush(Color.White)
        Dim i, c As Integer
        Dim NoticeLevel As Integer = My.Settings.NoticeLevel
        Dim WarningLevel As Integer = My.Settings.WarningLevel
        For i = 1 To 100
            If i < NoticeLevel Then
                border = New Pen(ThemeColor, 2)
                b1 = New SolidBrush(My.Settings.ThemeColor)
            ElseIf (i >= NoticeLevel) And (i < WarningLevel) Then
                border = New Pen(Color.Orange, 2)
                b1 = New SolidBrush(Color.Orange)
            ElseIf (i >= WarningLevel) Then
                border = New Pen(Color.FromArgb(255, 0, 0), 2)
                b1 = New SolidBrush(Color.FromArgb(255, 0, 0))
            End If
            bmp = New Bitmap(10, 25)
            gra = Graphics.FromImage(bmp)
            gra.FillRectangle(b, 0, 0, 10, 25)
            gra.DrawRectangle(border, 1, 1, 8, 23)
            c = Int(i / 4) - 1
            gra.FillRectangle(b1, 1, 1, 8, c)
            ImageList1.Images.Add(bmp)
        Next
    End Sub

    Private Sub ListView2_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles ListView2.ItemChecked
        If ListView2.CheckedItems IsNot Nothing AndAlso ListView2.CheckedItems.Count > 0 Then
            Dim i As Integer
            For i = ListView2.CheckedItems.Count - 1 To 0 Step -1
                ListView2.CheckedItems(i).Remove()
            Next
        End If
    End Sub

    Private Sub ListView1_Click(sender As Object, e As EventArgs) Handles ListView1.Click
        If ListView1.SelectedItems.Count > 0 Then
            Button_Del.Visible = True
            Button_Edit.Visible = True
        Else
            Button_Del.Visible = False
            Button_Edit.Visible = False
        End If
    End Sub

    Private Sub ListView2_Click(sender As Object, e As EventArgs) Handles ListView2.Click
        If ListView2.SelectedItems.Count > 0 Then
            Button_Del.Visible = True
            Button_Edit.Visible = True
        Else
            Button_Del.Visible = False
            Button_Edit.Visible = False
        End If
    End Sub

    Private Sub Button_Edit_Click(sender As Object, e As EventArgs) Handles Button_Edit.Click
        Edit()
    End Sub

    Public Sub Edit()
        If ListView1.SelectedItems.Count = 0 And ListView2.SelectedItems.Count = 0 Then
            Exit Sub
        End If
        Dim r As DialogResult
        r = Dialog2.ShowDialog()
        If r = DialogResult.OK Then
            If SelectedView Is ListView1 Then
                SelectedView.SelectedItems(0).Text = Dialog2.TextBox1.Text
                SelectedView.SelectedItems(0).SubItems(1).Text = Dialog2.DateTimePicker1.Value
                SelectedView.SelectedItems(0).SubItems(2).Text = Dialog2.TextBox2.Text
            Else
                SelectedView.SelectedItems(0).Text = Dialog2.DateTimePicker1.Value
                SelectedView.SelectedItems(0).SubItems(1).Text = Dialog2.TextBox1.Text
                SelectedView.SelectedItems(0).SubItems(2).Text = Dialog2.TextBox2.Text
            End If
        End If
    End Sub

    Private Sub 新建ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 新建ToolStripMenuItem.Click
        Add()
    End Sub

    Private Sub 编辑ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 编辑ToolStripMenuItem.Click
        Edit()
    End Sub

    Private Sub 删除ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 删除ToolStripMenuItem.Click
        On Error Resume Next
        SelectedView.SelectedItems(0).Remove()
    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening
        If ListView1.SelectedItems.Count > 0 Then
            ToolStripTextBox1.Visible = True
            ToolStripTextBox2.Visible = True
            ToolStripMenuItem1.Visible = True
            ToolStripMenuItem2.Visible = True
            ToolStripSeparator1.Visible = True
            ToolStripTextBox1.Text = ListView1.SelectedItems(0).SubItems(3).Text
            ToolStripTextBox2.Text = ListView1.SelectedItems(0).SubItems(2).Text
        Else
            ToolStripTextBox1.Visible = False
            ToolStripTextBox2.Visible = False
            ToolStripMenuItem1.Visible = False
            ToolStripMenuItem2.Visible = False
            ToolStripSeparator1.Visible = False
        End If
    End Sub

    Private Sub ContextMenuStrip2_Opening(sender As Object, e As CancelEventArgs) Handles ContextMenuStrip2.Opening
        If ListView2.SelectedItems.Count > 0 Then
            ToolStripTextBox3.Visible = True
            ToolStripMenuItem3.Visible = True
            ToolStripSeparator2.Visible = True
            ToolStripTextBox3.Text = ListView2.SelectedItems(0).SubItems(3).Text
        Else
            ToolStripTextBox3.Visible = False
            ToolStripMenuItem3.Visible = False
            ToolStripSeparator2.Visible = False
        End If
    End Sub

    Private Sub ToolStripMenuItem5_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem5.Click
        Add()
    End Sub

    Private Sub ToolStripMenuItem6_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem6.Click
        Edit()
    End Sub

    Private Sub ToolStripMenuItem7_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem7.Click
        On Error Resume Next
        SelectedView.SelectedItems(0).Remove()
    End Sub

    Private Sub Button_Setting_Click(sender As Object, e As EventArgs) Handles Button_Setting.Click
        Settings.ShowDialog()
    End Sub

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        If t >= RemindInt Then
            reminded = {}
            t = 0
        Else
            t = t + 10
        End If
    End Sub

    Private Sub 打开主窗体ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 打开主窗体ToolStripMenuItem.Click
        Show()
    End Sub

    Private Sub 设置ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 设置ToolStripMenuItem.Click
        Settings.ShowDialog(Me)
    End Sub

    Private Sub 退出ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 退出ToolStripMenuItem.Click
        Save()
        Application.Exit()
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        Show()
    End Sub

    Private Sub Save()
        '-----------------保存----------------
        Dim file As String = ""
        Text = "Tasks - [保存信息中...]"
        For i = 0 To ListView1.Items.Count - 1
            file = file & ListView1.Items(i).Text & "|"
            For j = 1 To ListView1.Items(0).SubItems.Count - 1
                file = file & ListView1.Items(i).SubItems(j).Text & "|"
            Next j
        Next i
        file = file & "END OF LIST1|"
        For i = 0 To ListView2.Items.Count - 1
            file = file & ListView2.Items(i).Text & "|"
            For j = 1 To ListView2.Items(0).SubItems.Count - 1
                file = file & ListView2.Items(i).SubItems(j).Text & "|"
            Next j
        Next
        file = file & "END OF FILE" '文件终止
        If Not (FileExists(Application.StartupPath & "\Saves.tasks")) Then
            WriteAllText(Application.StartupPath & "\Saves.tasks", file, False, System.Text.Encoding.UTF8)
        Else
            WriteAllText(Application.StartupPath & "\Saves.tasks", file, False)
        End If
        '------------------------------------
    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Dim r As MsgBoxResult = MsgBoxResult.No
        If sender Is Me Then
            r = MsgBox("是否需要隐藏至托盘？", MsgBoxStyle.YesNo)
        End If
        If r = MsgBoxResult.Yes Then
            e.Cancel = True
            Hide()
            NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
            NotifyIcon1.BalloonTipTitle = "Tasks"
            NotifyIcon1.BalloonTipText = "Tasks已隐藏至托盘↓"
            NotifyIcon1.ShowBalloonTip(3000)
        Else
            Save()
            Application.Exit()
        End If
    End Sub
End Class