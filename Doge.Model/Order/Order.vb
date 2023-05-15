Public Class Order

#Region "Declarations"
    Private _orderID As String
    Private _requiredShippedTimestamp As Long
    Private _status As Boolean
    Private _shippedAddress As String
#End Region

#Region "Properties"
    Public Property OrderId As String
        Get
            Return _orderID
        End Get
        Set(value As String)
            _orderID = value
        End Set
    End Property
    Public Property RequiredShippedTimestamp As Long
        Get
            Return _requiredShippedTimestamp
        End Get
        Set(value As Long)
            _requiredShippedTimestamp = value
        End Set
    End Property
    Public Property Status As Boolean
        Get
            Return _status
        End Get
        Set(value As Boolean)
            _status = value
        End Set
    End Property
    Public Property ShippedAddress As String
        Get
            Return _shippedAddress
        End Get
        Set(value As String)
            _shippedAddress = value
        End Set
    End Property
#End Region

End Class
