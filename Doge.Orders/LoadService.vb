Imports System.Diagnostics
Imports System.IO
Imports System.Timers

Public Class LoadService

    Private eventId As Integer = 1

    Public Sub New()
        MyBase.New
        InitializeComponent()
        Me.OrdersLog = New EventLog
        If Not EventLog.SourceExists("DOGE ServiceApp") Then
            EventLog.CreateEventSource("DOGE ServiceApp", "Application")
        End If
        OrdersLog.Source = "DOGE ServiceApp"
        OrdersLog.Log = "Application"

        If Not Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\DOGE\") Then
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\DOGE\")
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\DOGE\Loaded\")
            File.Create(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\DOGE\Orders.CSV")
        End If
    End Sub
    Protected Overrides Sub OnStart(ByVal args() As String)
        OrdersLog.WriteEntry("In OnStart.")

        Dim timer As Timer = New Timer()
        timer.Interval = 60000 ' 60 seconds
        AddHandler timer.Elapsed, AddressOf Me.OnTimer
        timer.Start()

        Dim processingOrders As Timer = New Timer()
        processingOrders.Interval = 5000 ' 5 seconds
        AddHandler processingOrders.Elapsed, AddressOf Me.ManageOrders
        processingOrders.Start()
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        OrdersLog.WriteEntry("In OnStop.")
    End Sub

    Protected Overrides Sub OnContinue()
        OrdersLog.WriteEntry("In OnContinue.")
    End Sub

    Private Sub OnTimer(sender As Object, e As Timers.ElapsedEventArgs)
        ' TODO: Insert monitoring activities here.
        OrdersLog.WriteEntry("Monitoring the Service.", EventLogEntryType.Information, eventId)
        eventId = eventId + 1
    End Sub

    Private Sub ManageOrders(sender As Object, e As Timers.ElapsedEventArgs)
        Try

            For Each order As String In My.Computer.FileSystem.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\DOGE\", FileIO.SearchOption.SearchTopLevelOnly, "*.ord")
                OrdersLog.WriteEntry("Processing Order.", "Order File: " + order, EventLogEntryType.Information, eventId)
                eventId = eventId + 1

                Using sw As New IO.StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\DOGE\Orders.CSV")
                    Using sr As New IO.StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\DOGE\" + order)

                        Dim line As String
                        Do
                            line = sr.ReadLine()

                            sw.WriteLine(line)

                        Loop Until line Is Nothing
                        sr.Close()
                    End Using
                End Using
            Next

        Catch ex As Exception
            OrdersLog.WriteEntry("Error Processing Orders.", ex.Message, EventLogEntryType.Error, eventId)
            eventId = eventId + 1
            Me.Stop()
        End Try
    End Sub

End Class
