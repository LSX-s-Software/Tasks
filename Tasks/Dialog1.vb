Public Class Dialog1
    Dim speed As Byte
    Dim steps As Byte
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
        OK_Button.Visible = False
        Button_Next.Visible = True
        GroupBox1.Visible = False
        GroupBox2.Visible = False
        GroupBox3.Visible = False
        GroupBox4.Visible = False
        TextBox1.Text = ""
        TextBox2.Text = ""
        Button_Next.Enabled = False
        steps = 1
        DateTimePicker1.Value = Now
        DateTimePicker1.CustomFormat = "yyyy/MM/dd HH:mm:ss"

    End Sub

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
End Class
