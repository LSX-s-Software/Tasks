Imports System.ComponentModel
Imports Microsoft.VisualBasic.FileIO.FileSystem

Public Class Form1
    Public PicList() As Image
    Dim PicIndex As Integer = 0
    Public SelectedView As ListView
    Public reminded， reminded1 As ListViewItem()
    Public RemindInt, t, t1 As Long
    Private Sub StatusStrip1_MouseEnter(sender As Object, e As EventArgs) Handles StatusStrip1.MouseEnter
        Cursor = Cursors.Hand
    End Sub

    Private Sub StatusStrip1_MouseLeave(sender As Object, e As EventArgs) Handles StatusStrip1.MouseLeave
        Cursor = Cursors.Default
    End Sub

    Public Sub Add()
        Dim result As DialogResult
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
                If ListView1.Items.Count <> 0 AndAlso ItemExisted(ListView1, Dialog1.TextBox1.Text) Then
                    If MsgBox("项目已存在，仍要继续添加？", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
                End If
                itm = ListView1.Items.Add(Dialog1.TextBox1.Text)
                itm.SubItems.AddRange({Dialog1.DateTimePicker1.Value, Dialog1.TextBox2.Text, Now})
                ListView1.BackgroundImage = Nothing
            Else
                If ListView2.Items.Count <> 0 AndAlso ItemExisted(ListView2, Dialog1.TextBox1.Text) Then
                    If MsgBox("项目已存在，仍要继续添加？", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
                End If
                itm = ListView2.Items.Add(Dialog1.DateTimePicker1.Value)
                itm.SubItems.AddRange({Dialog1.TextBox1.Text, Dialog1.TextBox2.Text, Now, Dialog1.ComboBox1.Text, CalculateNextDate(Dialog1.DateTimePicker1.Value, Dialog1.ComboBox1.Text)})
            End If
            If My.Settings.FirstRun Then
                TableLayoutPanel1.Visible = False
                StatusStrip1.Visible = True
                ListView1.BackgroundImage = PicList(3)
            End If
        End If
    End Sub

    Private Function ItemExisted(list As ListView, text As String) As Boolean
        If list Is ListView1 Then
            If ListView1.FindItemWithText(text, False, 0, False) IsNot Nothing Then Return True
        Else
            If ListView2.FindItemWithText(text, True, 0, False) IsNot Nothing Then Return True
        End If
        Return False
    End Function

    Private Sub Button_Add_Click(sender As Object, e As EventArgs) Handles Button_Add.Click
        Add()
    End Sub

    Private Sub Button_Del_Click(sender As Object, e As EventArgs) Handles Button_Del.Click
        On Error Resume Next
        SelectedView.SelectedItems(0).Remove()
        If ListView1.Items.Count = 0 Then ListView1.BackgroundImage = My.Resources.背景1
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
            t1 = Date.Parse(ListView1.Items(i).SubItems(1).Text).Subtract(Date.Now).TotalMilliseconds '现在时间到截止时间的长度
            t2 = Date.Parse(Date.Now).Subtract(ListView1.Items(i).SubItems(3).Text).TotalMilliseconds + 1 '从任务创建到现在的时间
            If ((t1 + t2) > t2) And (t1 >= 0) Then
                ListView1.Items(i).ImageIndex = Int(t2 / (t1 + t2) * 100)
            Else
                ListView1.Items(i).ImageIndex = 99
            End If
            If (t1 <= 0) AndAlso Find(ListView1.Items(i), 1) = -1 Then
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
            ElseIf (t2 / (t1 + t2) * 100 >= 50) And (t2 / (t1 + t2) * 100 < My.Settings.NoticeLevel) AndAlso Find(ListView1.Items(i), 2) = -1 Then
                Form3.Show()
                Form3.Label1.Text = "您的任务" & ChrW(13) & ChrW(13) & ChrW(13) & "剩余时间不到50%"
                Form3.Label2.Text = ListView1.Items(i).Text
                Form3.BackColor = My.Settings.ThemeColor
                If reminded1 Is Nothing Then
                    reminded1 = New ListViewItem() {ListView1.Items(i)}
                Else
                    reminded1 = reminded1.Concat({ListView1.Items(i)}).ToArray
                End If
            ElseIf (t2 / (t1 + t2) * 100 >= My.Settings.NoticeLevel) And (t2 / (t1 + t2) * 100 < My.Settings.WarningLevel) AndAlso Find(ListView1.Items(i), 2) = -1 Then
                Form3.Show()
                Form3.Label1.Text = "您的任务" & ChrW(13) & ChrW(13) & ChrW(13) & "剩余时间不到" & My.Settings.NoticeLevel & "%"
                Form3.Label2.Text = ListView1.Items(i).Text
                Form3.BackColor = Color.Orange
                If reminded1 Is Nothing Then
                    reminded1 = New ListViewItem() {ListView1.Items(i)}
                Else
                    reminded1 = reminded1.Concat({ListView1.Items(i)}).ToArray
                End If
            ElseIf (t2 / (t1 + t2) * 100 >= My.Settings.WarningLevel) And (t1 > 0) AndAlso Find(ListView1.Items(i), 2) = -1 Then
                Form3.Show()
                Form3.Label1.Text = "您的任务" & ChrW(13) & ChrW(13) & ChrW(13) & "剩余时间不到" & My.Settings.WarningLevel & "%"
                Form3.Label2.Text = ListView1.Items(i).Text
                Form3.BackColor = Color.FromArgb(255, 0, 0)
                If reminded1 Is Nothing Then
                    reminded1 = New ListViewItem() {ListView1.Items(i)}
                Else
                    reminded1 = reminded1.Concat({ListView1.Items(i)}).ToArray
                End If
            End If
        Next
        '------------------------------------------
        For i = 0 To ListView2.Items.Count - 1
            t1 = Date.Parse(ListView2.Items(i).Text).Subtract(Date.Now).TotalMilliseconds '现在时间到截止时间的长度
            t2 = Date.Parse(Date.Now).Subtract(ListView2.Items(i).SubItems(3).Text).TotalMilliseconds + 1 '从任务创建到现在的时间
            If (t1 <= 0) AndAlso Find(ListView2.Items(i), 1) = -1 Then
                If (ListView2.Items(i).SubItems(4).Text <> "从不") AndAlso ItemExisted(ListView2, ListView2.Items(i).SubItems(1).Text) Then
                    Dim itm = ListView2.Items(i).Clone()
                    ListView2.Items.Add(itm)
                    itm.Text = ListView2.Items(i).SubItems(5).Text
                    itm.Subitems(5).text = CalculateNextDate(itm.Text, itm.SubItems(4).Text)
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

    Public Function CalculateNextDate(origin As Date, 方式 As String) As Date
        Select Case 方式
            Case "从不"
                Return origin
            Case "每天"
                Return DateAdd(DateInterval.DayOfYear, 1, origin)
            Case "每两天"
                Return DateAdd(DateInterval.DayOfYear, 2, origin)
            Case "每周"
                Return DateAdd(DateInterval.DayOfYear, 7, origin)
            Case "每两周"
                Return DateAdd(DateInterval.DayOfYear, 14, origin)
            Case "每月"
                Return DateAdd(DateInterval.Month, 1, origin)
            Case "每年"
                Return DateAdd(DateInterval.Year, 1, origin)
            Case Else
                Return origin
        End Select
    End Function

    Private Function Find(ByVal item As ListViewItem, listnum As Integer) As Integer
        Dim i As Integer
        Find = -1
        Select Case listnum
            Case 1
                If reminded Is Nothing Then Return -1
                For i = 0 To reminded.Count - 1
                    If reminded(i) Is item Then
                        Return i
                    End If
                Next
            Case 2
                If reminded1 Is Nothing Then Return -1
                For i = 0 To reminded1.Count - 1
                    If reminded1(i) Is item Then
                        Return i
                    End If
                Next
        End Select
    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim a As New Drawing2D.GraphicsPath
        a.AddEllipse(2, 2, 35, 35)
        Button_Add.Region = New Region(a)
        a = New Drawing2D.GraphicsPath
        a.AddEllipse(2, 2, 25, 25)
        Button_Del.Region = New Region(a)
        Button_Edit.Region = New Region(a)
        ToolStripStatusLabel2.ForeColor = My.Settings.ThemeColor
        SelectedView = ListView1
        If My.Settings.RemindInterval <> "永不重复" Then
            RemindInt = CInt(My.Settings.RemindInterval) * 60
        Else
            Timer3.Enabled = False
        End If
        DrawProgressBar()
        If ListView1.Items.Count > 0 Then ListView1.BackgroundImage = Nothing
        If My.Settings.FirstRun Then
            Demo()
        Else
            PicList = Nothing
        End If
    End Sub

    Private Sub Demo()
        StatusStrip1.Visible = False
        With TableLayoutPanel1
            .Dock = DockStyle.Fill
            .Visible = True
            .BackgroundImage = PicList(0)
        End With
    End Sub

    Private Sub DrawProgressBar()
        Dim bmp As Bitmap = New Bitmap(32, 32)
        Dim gra As Graphics
        Dim ThemeColor As Color = My.Settings.ThemeColor
        Dim border As Pen = New Pen(ThemeColor, 2)
        Dim b1 As SolidBrush = New SolidBrush(ThemeColor)
        Dim b As SolidBrush = New SolidBrush(Color.White)
        Dim i As Integer
        Dim NoticeLevel As Integer = My.Settings.NoticeLevel
        Dim WarningLevel As Integer = My.Settings.WarningLevel
        ImageList1.ImageSize = bmp.Size
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
            bmp = New Bitmap(32, 32)
            gra = Graphics.FromImage(bmp)
            gra.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            gra.FillRectangle(b, 0, 0, bmp.Size.Width, bmp.Size.Height)
            gra.DrawEllipse(border, 0, 0, bmp.Size.Width - border.Width, bmp.Size.Height - border.Width)
            Dim rect As New Rectangle(2, 2, bmp.Size.Width - border.Width * 2 - 2, bmp.Size.Height - border.Width * 2 - 2)
            gra.FillPie(b1, rect, -90, 360 * i / 100)
            ImageList1.Images.Add(bmp)
        Next
        bmp.Dispose()
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
                SelectedView.SelectedItems(0).SubItems(4).Text = Dialog2.ComboBox1.Text
                SelectedView.SelectedItems(0).SubItems(5).Text = CalculateNextDate(Dialog2.DateTimePicker1.Value, Dialog2.ComboBox1.Text)
            End If
        End If
        Dim index As Integer = Find(SelectedView.SelectedItems(0), 1)
        If index > -1 Then
            For i = index To reminded.Count - 2
                reminded(i) = reminded(i + 1)
            Next
            reminded(reminded.Count - 1) = Nothing
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
        If ListView1.Items.Count = 0 Then ListView1.BackgroundImage = My.Resources.背景1
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
            ToolStripTextBox4.Text = ListView2.SelectedItems(0).SubItems(4).Text & "(下次：" & CDate(ListView2.SelectedItems(0).SubItems(5).Text).ToShortDateString & ")"
        Else
            ToolStripTextBox3.Visible = False
            ToolStripMenuItem3.Visible = False
            ToolStripSeparator2.Visible = False
            ToolStripMenuItem8.Visible = False
            ToolStripTextBox4.Visible = False
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
        If t1 >= 600 Then
            reminded1 = {}
            t1 = 0
        Else
            t1 = t1 + 10
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
        If (sender Is Me) AndAlso (ListView1.Items.Count + ListView2.Items.Count > 0) Then
            r = MsgBox("SmartSense发现您有未完成的任务或提醒事项" & vbCrLf & "是否需要隐藏至托盘？", MsgBoxStyle.YesNo)
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
            If (ListView1.Items.Count + ListView2.Items.Count > 0) AndAlso Not My.Settings.RunWhenSysStart Then
                r = MsgBox("SmartSense发现您有未完成的任务或提醒事项" & vbCrLf & "是否需要开机启动？", MsgBoxStyle.YesNo)
                If r = MsgBoxResult.Yes Then
                    Dim Reg As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
                    Reg.SetValue(Application.ProductName, Application.StartupPath & "\" & Application.ProductName & ".exe" & " -h") '写入注册表
                    Reg.Close()
                    My.Settings.RunWhenSysStart = True
                End If
            End If
            If (ListView1.Items.Count + ListView2.Items.Count = 0) AndAlso My.Settings.RunWhenSysStart Then
                r = MsgBox("SmartSense发现您没有未完成的任务或提醒事项" & vbCrLf & "是否需要关闭开机启动？", MsgBoxStyle.YesNo)
                If r = MsgBoxResult.Yes Then
                    Dim Reg As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
                    Reg.DeleteValue(Application.ProductName) '删除注册表键
                    Reg.Close()
                    My.Settings.RunWhenSysStart = False
                End If
            End If
            Application.Exit()
        End If
    End Sub

    Private Sub ListView1_DoubleClick(sender As Object, e As EventArgs) Handles ListView1.DoubleClick
        If ListView1.SelectedItems.Count > 0 Then
            ListView1.SelectedItems(0).BeginEdit()
        End If
    End Sub

    Private Sub ListView1_KeyDown(sender As Object, e As KeyEventArgs) Handles ListView1.KeyDown
        If (e.KeyCode = Keys.Delete) AndAlso (ListView1.Items.Count > 0) Then
            ListView1.SelectedItems(0).Remove()
            If ListView1.Items.Count = 0 Then ListView1.BackgroundImage = My.Resources.背景1
        End If
        If (e.KeyCode = Keys.F2) AndAlso (ListView1.Items.Count > 0) Then ListView1.SelectedItems(0).BeginEdit()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ListView2.BackgroundImage = Nothing
        My.Settings.FirstRun = False
        Button1.Visible = False
    End Sub

    Private Sub Next_Button_Click(sender As Object, e As EventArgs) Handles Next_Button.Click
        Select Case PicIndex
            Case 1
                Button_Add.BringToFront()
                Next_Button.Visible = False
        End Select
        Pre_Button.Visible = True
        PicIndex = PicIndex + 1
        TableLayoutPanel1.BackgroundImage = PicList(PicIndex)
    End Sub

    Private Sub Pre_Button_Click(sender As Object, e As EventArgs) Handles Pre_Button.Click
        PicIndex = PicIndex - 1
        TableLayoutPanel1.BackgroundImage = PicList(PicIndex)
        Next_Button.Visible = True
        If PicIndex = 0 Then Pre_Button.Visible = False
    End Sub

    Private Sub ListView2_KeyDown(sender As Object, e As KeyEventArgs) Handles ListView2.KeyDown
        If (e.KeyCode = Keys.Delete) AndAlso (ListView2.Items.Count > 0) Then ListView2.SelectedItems(0).Remove()
    End Sub
End Class