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
Module ProgramSoap

    Sub CallService()
        Dim handler As New CustomHttpClientHandler()
        handler.ServerCertificateCustomValidationCallback = AddressOf ServerCertificateCustomValidation
        Dim wBinding = GetBindingHttps()

        Dim wEndpoint As New EndpointAddress("https://ec.europa.eu/taxation_customs/vies/services/checkVatService")
        Dim wFactory As New ChannelFactory(Of CheckVAT.checkVatPortType)(wBinding, wEndpoint)

        ' Configurare il factory per usare il nostro HttpClientHandler
        wFactory.Endpoint.EndpointBehaviors.Add(New CustomHttpClientHandlerBehavior(handler))
        Dim client = wFactory.CreateChannel()
        Try
            Dim wRequestBody = New CheckVAT.checkVatRequestBody With {
            .countryCode = "IT",
            .vatNumber = "00905811006"
            }
            Dim wRequest = New CheckVAT.checkVatRequest With {
           .Body = wRequestBody
            }
            Console.WriteLine("---Invoking Service Soap---")
            Dim response = client.checkVat(wRequest)
            If response Is Nothing Then
                Console.WriteLine("No response")
            Else
                Console.WriteLine("---Service Invocation OK---")
                Console.WriteLine($"Name: {response.Body.name}")
                Console.WriteLine($"Address: {response.Body.address}")
            End If

        Catch e As HttpRequestException
            Console.WriteLine(vbCrLf & "Exception Caught!")
            Console.WriteLine($"Message: {e.Message} ")
        Finally
            handler.Dispose()

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

Public Class CustomHttpClientHandler
    Inherits HttpClientHandler

    Public Sub New()

        Me.ServerCertificateCustomValidationCallback = AddressOf ServerCertificateCustomValidation
    End Sub

End Class

Public Class CustomHttpClientHandlerBehavior
    Implements IEndpointBehavior

    Private ReadOnly _handler As HttpClientHandler

    Public Sub New(handler As HttpClientHandler)
        _handler = handler
    End Sub

    Public Sub AddBindingParameters(endpoint As ServiceEndpoint, bindingParameters As BindingParameterCollection) Implements IEndpointBehavior.AddBindingParameters
        If bindingParameters Is Nothing Then Throw New ArgumentNullException("bindingParameters non valorizzato")
        bindingParameters.Add(_handler)
    End Sub

    Public Sub ApplyClientBehavior(endpoint As ServiceEndpoint, clientRuntime As ClientRuntime) Implements IEndpointBehavior.ApplyClientBehavior
        Return
    End Sub

    Public Sub ApplyDispatchBehavior(endpoint As ServiceEndpoint, endpointDispatcher As EndpointDispatcher) Implements IEndpointBehavior.ApplyDispatchBehavior
        Return
    End Sub

    Public Sub Validate(endpoint As ServiceEndpoint) Implements IEndpointBehavior.Validate
        Return
    End Sub
End Class