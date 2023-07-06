Imports System.Xml

Public Class WsLog

    Private _IdMetodo As Integer
    Public ReadOnly Property IdMetodo() As Integer
        Get
            Return _IdMetodo
        End Get
        'Set(ByVal value As Integer)
        '    _IdMetodo = value
        'End Set
    End Property

    Private _RucCliente As String
    Public ReadOnly Property RucCliente() As String
        Get
            Return _RucCliente
        End Get
        'Set(ByVal value As String)
        '    _RucCliente = value
        'End Set
    End Property

    Private _ClaveConexion As String
    Public ReadOnly Property ClaveConexion() As String
        Get
            Return _ClaveConexion
        End Get
        'Set(ByVal value As String)
        '    _ClaveConexion = value
        'End Set
    End Property

    Private _TipoConsulta As String
    Public ReadOnly Property TipoConsulta() As String
        Get
            Return _TipoConsulta
        End Get
        'Set(ByVal value As String)
        '    _TipoConsulta = value
        'End Set
    End Property

    Private _Request As XmlDocument
    Public ReadOnly Property Request() As XmlDocument
        Get
            Return _Request
        End Get
        'Set(ByVal value As XmlDocument)
        '    _Request = value
        'End Set
    End Property

    Private _Registros As Integer
    Public ReadOnly Property Registros() As Integer
        Get
            Return _Registros
        End Get
        'Set(ByVal value As Integer)
        '    _Registros = value
        'End Set
    End Property

    Private _Paginas As Integer
    Public ReadOnly Property Paginas() As Integer
        Get
            Return _Paginas
        End Get
        'Set(ByVal value As Integer)
        '    _Paginas = value
        'End Set
    End Property

    Private _BaseDatos As String
    Public ReadOnly Property BaseDatos() As String
        Get
            Return _BaseDatos
        End Get
        'Set(ByVal value As String)
        '    _BaseDatos = value
        'End Set
    End Property

    Private _CodError As String
    Public ReadOnly Property CodError() As String
        Get
            Return _CodError
        End Get
        'Set(ByVal value As String)
        '    _CodError = value
        'End Set
    End Property

    Private _MsgError As String
    Public ReadOnly Property MsgError() As String
        Get
            Return _MsgError
        End Get
        'Set(ByVal value As String)
        '    _MsgError = value
        'End Set
    End Property

    Sub New(ByVal IdMetodo As Integer, ByVal RucCliente As String, ByVal ClaveConexion As String, _
            ByVal TipoConsulta As String, ByVal Request As XmlDocument, ByVal Registros As Integer, _
            ByVal Paginas As Integer, ByVal BaseDatos As String, ByVal CodError As String, ByVal MsgError As String)

        Me._IdMetodo = IdMetodo
        Me._RucCliente = RucCliente
        Me._ClaveConexion = ClaveConexion
        Me._TipoConsulta = TipoConsulta
        Me._Request = Request
        Me._Registros = Registros
        Me._Paginas = Paginas
        Me._BaseDatos = BaseDatos
        Me._CodError = CodError
        Me._MsgError = MsgError

    End Sub

End Class
