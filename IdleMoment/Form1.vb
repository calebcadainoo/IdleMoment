Imports System.Drawing.Text
Imports System.Runtime.InteropServices

Public Class Form1
    ' help check mouse activity
    <StructLayout(LayoutKind.Sequential)> Structure LASTINPUTINFO 
        <MarshalAs(UnmanagedType.U4)> Public cbSize As Integer
        <MarshalAs(UnmanagedType.U4)> Public dwTime As Integer
    End Structure
    <DllImport("user32.dll")> Shared Function GetLastInputInfo(ByRef plii As LASTINPUTINFO) As Boolean

    End Function

    Dim idletime As Integer
    Dim lastInputInf As New LASTINPUTINFO()
    Public Function GetLastInputTime() As Integer
        idletime = 0
        lastInputInf.cbSize = Marshal.SizeOf(lastInputInf)
        lastInputInf.dwTime = 0

        If GetLastInputInfo(lastInputInf) Then
            idletime = Environment.TickCount - lastInputInf.dwTime
        End If

        If idletime > 0 Then
            Return idletime / 1000
        Else : Return 0
        End If
    End Function

    Private sumofidletime As TimeSpan = New TimeSpan(0)
    Private LastLastIdletime As Integer = 0

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        Me.Close()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BackColor = Color.Black
        Me.ForeColor = Color.White
        Me.Opacity = 0.9
        Me.TopMost = True
        'Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None

        Me.WindowState = FormWindowState.Normal
        Me.StartPosition = FormStartPosition.Manual
        With Screen.PrimaryScreen.WorkingArea
            Me.SetBounds(.Left, .Top, .Width, .Height)
        End With

        Me.Bounds = Screen.GetBounds(Me)
        'Me.MaximumSize = Screen.FromControl(Me).WorkingArea.Size
        Me.WindowState = FormWindowState.Maximized

        Label1.Width = Me.Width
        Label1.Height = Me.Height

        'Label1.BackColor = Color.Green
        Label1.Location = New Point(0, 0)
        Dim customFont As PrivateFontCollection = New PrivateFontCollection
        customFont.AddFontFile("G:\FONTS\FontsUnpacked\BalooBhaina2-Regular.ttf")
        Label1.Font = New Font(customFont.Families(0), 140)
        Label1.AutoSize = False
        Label1.TextAlign = ContentAlignment.MiddleCenter

        Timer1.Start()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Label1.Text = TimeOfDay.ToString("hh • mm • ss")
        'Label1.Text = TimeOfDay.ToString("hh • mm • ss tt")

        Dim it As Integer = GetLastInputTime()
        If LastLastIdletime > it Then
            Label2.Text = "IDLE STATE CHANGED!"
            sumofidletime = sumofidletime.Add(TimeSpan.FromSeconds(LastLastIdletime))
            Label3.Text = "Sum of idle time: " & sumofidletime.ToString
        Else
            Label2.Text = GetLastInputTime()
        End If
        LastLastIdletime = it

    End Sub
End Class
