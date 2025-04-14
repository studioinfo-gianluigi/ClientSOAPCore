Imports System
Imports System.Data
Imports System.Net.Http
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Threading.Tasks
Imports System.ServiceModel
Imports System.ServiceModel.Channels
Imports System.ServiceModel.Description
Imports System.ServiceModel.Dispatcher
Imports System.ServiceModel.Security
Imports System.IdentityModel.Selectors
Module ProgramSoapSolution

    Sub CallService()
        Dim wEndpoint As New EndpointAddress("https://ec.europa.eu/taxation_customs/vies/services/checkVatService")
        Dim wBinding = GetBindingHttps()
        Dim wClient = New CheckVAT.checkVatPortTypeClient(wBinding, wEndpoint)
        wClient.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = New X509ServiceCertificateAuthentication() With {
            .CertificateValidationMode = X509CertificateValidationMode.Custom,
            .CustomCertificateValidator = New CustomCertValidator()
            }
        Try

            Console.WriteLine("---Invoking Service Soap---")
            Dim wName As String = ""
            Dim wAddress As String = ""
            Dim response = wClient.checkVat("IT", "00905811006", True, wName, wAddress)

            If response Is Nothing Then
                Console.WriteLine("No response")
            Else
                Console.WriteLine("---Service Invocation OK---")
                Console.WriteLine($"Name: {wName}")
                Console.WriteLine($"Address: {wAddress}")

            End If

        Catch e As HttpRequestException
            Console.WriteLine(vbCrLf & "Exception Caught!")
            Console.WriteLine($"Message: {e.Message} ")
        Finally
            ' handler.Dispose()

        End Try
    End Sub

    Public Function GetBindingHttps() As ServiceModel.Channels.Binding
        Dim wBinding = New System.ServiceModel.BasicHttpBinding
        wBinding.Security.Mode = BasicHttpSecurityMode.Transport
        Return wBinding
    End Function

    Private Function ServerCertificateCustomValidation(requestMessage As HttpRequestMessage, certificate As X509Certificate2, chain As X509Chain, sslErrors As SslPolicyErrors) As Boolean
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

Public Class CustomCertValidator
    Inherits X509CertificateValidator

    Public Overrides Sub Validate(certificate As X509Certificate2)
        Console.WriteLine("---ServerCertificateCustomValidation---")
        Console.WriteLine($"Effective date: {certificate?.GetEffectiveDateString()}")
        Console.WriteLine($"Exp date: {certificate?.GetExpirationDateString()}")
        Console.WriteLine($"Issuer: {certificate?.Issuer}")
        Console.WriteLine($"Subject: {certificate?.Subject}")
        Console.WriteLine("---ServerCertificateCustomValidation---")
        Console.WriteLine()

    End Sub
End Class