Public Class Main

    'main class for a console application. request a user to choose a service to call 1 REST or 2 SOAP
    'then call the appropriate method
    Public Shared Sub Main(args As String())
        Console.WriteLine("Choose a service to call:")
        Console.WriteLine("1 - Rest")
        Console.WriteLine("2 - SOAP")
        Dim service As Integer = Convert.ToInt32(Console.ReadLine())
        Select Case service
            Case 1
                ProgramRest.CallService()
            Case 2
                ProgramSoap.CallService()
            Case Else
                Console.WriteLine("Invalid choice")
        End Select
        Console.WriteLine("Exit")
    End Sub
End Class
