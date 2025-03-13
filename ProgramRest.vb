Imports System
Imports System.Net.Http
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Threading.Tasks
Module ProgramRest


    Sub CallService()
        Dim handler As New HttpClientHandler()
        handler.ServerCertificateCustomValidationCallback = AddressOf ServerCertificateCustomValidation
        Dim client As New HttpClient(handler)
        Try
            Console.WriteLine("---Invoking Client Rest---")
            Dim response As HttpResponseMessage = client.GetAsync("https://ec.europa.eu").GetAwaiter().GetResult()
            response.EnsureSuccessStatusCode()
            Dim responseBody As String = response.Content.ReadAsStringAsync().GetAwaiter().GetResult()
            Console.WriteLine($"Read {responseBody.Length} characters")
        Catch e As HttpRequestException
            Console.WriteLine(vbCrLf & "Exception Caught!")
            Console.WriteLine($"Message: {e.Message} ")
        Finally
            handler.Dispose()
            client.Dispose()
        End Try
    End Sub

    Public Function ServerCertificateCustomValidation(requestMessage As HttpRequestMessage, certificate As X509Certificate2, chain As X509Chain, sslErrors As SslPolicyErrors) As Boolean
        Console.WriteLine("---ServerCertificateCustomValidation---")
        Console.WriteLine($"Requested URI: {requestMessage.RequestUri}")
        Console.WriteLine($"Effective date: {certificate?.GetEffectiveDateString()}")
        Console.WriteLine($"Exp date: {certificate?.GetExpirationDateString()}")
        Console.WriteLine($"Issuer: {certificate?.Issuer}")
        Console.WriteLine($"Subject: {certificate?.Subject}")
        Console.WriteLine($"Errors: {sslErrors}")
        Console.WriteLine("---ServerCertificateCustomValidation---")
        Console.WriteLine()
        Return sslErrors = SslPolicyErrors.None
    End Function
End Module
