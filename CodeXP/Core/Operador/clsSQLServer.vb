Imports System.Data.SqlClient
Imports System.Configuration

Public Class clsSQLServer
#Region "Propiedades Privadas"
    Private _cnn As SqlConnection
    Private _cmd As SqlCommand
    Private _prm As SqlParameter
    Private _da As SqlDataAdapter
    Private _dtResultado As DataTable
    Private _dsResultado As DataSet
    Private _sp As String
    Private _resultado As ErrorSQL

    Private _BaseDatos As String
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Resultado As ErrorSQL
        Get
            Return _resultado
        End Get
    End Property
    Public Property Procedimiento() As String
        Get
            Return _sp
        End Get
        Set(ByVal value As String)
            _sp = value
            _cmd = New SqlCommand
            _cmd.CommandType = CommandType.StoredProcedure
            _cmd.CommandText = _sp
            _cmd.Connection = _cnn
        End Set
    End Property
    Public Structure ErrorSQL
        Private _CodError As Integer
        Private _MsgError As String

        Public ReadOnly Property CodigoError As Integer
            Get
                Return _CodError
            End Get
        End Property
        Public ReadOnly Property MensajeError As String
            Get
                Return _MsgError
            End Get
        End Property

        Sub New(ByVal CodigoError As Integer, ByVal MensajeError As String)
            _CodError = CodigoError
            _MsgError = MensajeError
        End Sub
    End Structure

    Public ReadOnly Property BaseDatos() As String
        Get
            Return _BaseDatos
        End Get
    End Property

#End Region

#Region "Metodos Privados"
    Private Sub GuardarResultado(ByVal CodigoError As Integer, ByVal MensajeError As String)
        _resultado = New ErrorSQL(CodigoError, MensajeError)
    End Sub
#End Region

#Region "Metodos Publicos"
    Public Sub New(ByVal Conexion As String)
        _cnn = New SqlConnection(ConfigurationSettings.AppSettings.Item(Conexion).ToString())
    End Sub

    Public Sub addParam(ByVal Nombre As String, ByVal Valor As Object, Optional Tipo As SqlDbType = SqlDbType.VarChar)
        Try
            Select Case Tipo
                Case SqlDbType.Int
                    _prm = New SqlParameter(Nombre, Tipo)
                    _prm.Value = CInt(Valor)
                Case SqlDbType.Money
                    _prm = New SqlParameter(Nombre, Tipo)
                    _prm.Value = CDec(Valor)
                Case SqlDbType.DateTime
                    _prm = New SqlParameter(Nombre, Tipo)
                    _prm.Value = CDate(Valor)
                Case SqlDbType.Xml
                    _prm = New SqlParameter(Nombre, Tipo)
                    _prm.Value = CDate(Valor)
                Case Else
                    _prm = New SqlParameter(Nombre, Valor)
            End Select
        Catch ex As Exception
            _prm = New SqlParameter(Nombre, Valor)
        End Try

        _cmd.Parameters.Add(_prm)
    End Sub
    Public Function ExecSP() As Integer
        _dtResultado = New DataTable
        ExecSP = 1

        Try
            _cnn.Open()
            _BaseDatos = _cnn.DataSource.ToString + " ; " + _cnn.Database.ToString
            _cmd.CommandTimeout = 300 '5 MIN
            _da = New SqlDataAdapter(_cmd)
            _da.Fill(_dtResultado)
            _cnn.Close()

            If _dtResultado.Rows.Count > 0 Then
                ExecSP = Val(_dtResultado.Rows(0).Item(0).ToString)
            End If
            GuardarResultado(1, "Proceso OK")
        Catch ex As Exception
            _cnn.Close()
            GuardarResultado(0, ex.Message)
        End Try
    End Function

    Public Function ExecSPdt(Optional ByVal TableName As String = "") As DataTable
        _dtResultado = New DataTable(TableName)
        Try
            _cnn.Open()
            _BaseDatos = _cnn.DataSource.ToString + " ; " + _cnn.Database.ToString
            _cmd.CommandTimeout = 300 '5 MIN
            _da = New SqlDataAdapter(_cmd)
            _da.Fill(_dtResultado)
            _cnn.Close()

            GuardarResultado(1, "Proceso OK")
        Catch ex As Exception
            _cnn.Close()
            GuardarResultado(0, ex.Message)
        End Try
        Return _dtResultado
    End Function

    Public Function ExecSPds() As DataSet
        _dsResultado = New DataSet
        Try
            _cnn.Open()
            _BaseDatos = _cnn.DataSource.ToString + " ; " + _cnn.Database.ToString
            _cmd.CommandTimeout = 900 '5 MIN
            _da = New SqlDataAdapter(_cmd)
            _da.Fill(_dsResultado)
            _cnn.Close()

            GuardarResultado(1, "Proceso OK")
        Catch ex As Exception
            _cnn.Close()
            GuardarResultado(0, ex.Message)
        End Try
        Return _dsResultado
    End Function
#End Region
End Class